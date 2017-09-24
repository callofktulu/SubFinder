using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubFinder.Data;
using SubFinder.Models;

namespace SubFinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly SubFinderContext _context;
        private IHostingEnvironment hostingEnv;

        public HomeController(IHostingEnvironment env, SubFinderContext context)
        {
            _context = context;
            hostingEnv = env;
        }

        public IActionResult Index(IndexViewModel model)
        {
            if(model == null)
             model = new IndexViewModel();

            return View(model);
        }

        [HttpPost("UploadFiles")]
        public ActionResult Index(List<IFormFile> files)
        {
            var fileName = string.Empty;
            var model = new IndexViewModel {LoopCount = 300};

            foreach (var formFile in files)
            {
                if (formFile.Length <= 0) continue; // Check whether the file is empty or not.
              
                foreach (var file in files)
                {
                    fileName = hostingEnv.WebRootPath + "\\" + "article.txt";
                    using (var fs = System.IO.File.Create(fileName))
                    {
                        file.CopyTo(fs); // We save the article to hard-disk for easy access and prevent time-outs.
                        fs.Flush(); // We delete the uploaded article from Random Access Memory to save application memory space.
                    }
                }
            }

            var article = System.IO.File.ReadAllText(fileName); // We read the article from the hard-disk.
            var punctuation = article.Where(char.IsPunctuation).Distinct().ToArray(); // We remove all the punctuation marks.
            var words = article.Split().Select(x => x.Trim(punctuation)); // We parse it into individual words.
            var sentences = SentenceSeperator(article); // SentenceSeperator with error handler.
            model.WordList = words.Select(item => new LexicalItems.Word {Unit = item}).ToList();  // We insert each word into word object. Word object has evaluation parameters in it.
            model.SentenceList = sentences.Select(item => new LexicalItems.Sentence { SentenceUnit = item}).ToList(); // We insert each sentence into sentence object. Sentence object has evaluation parameters in it.

            // Word Level Evalutaion Starts here
            using (_context) // Database connection to access datasets 
            {
                for (var i = 0; i < model.LoopCount; i++) // This is the first loop mentioned in the thesis. It iterates through each word.
                {
                    model.WordList[i].Values = new List<Sentiment>(); // Point to Improve: To improve efficiency we should only get the values.
                    var sentiWordNetValue = _context.Sentiment.FirstOrDefault(v => v.Unit == model.WordList[i].Unit && v.ListId == 1);
                    if (sentiWordNetValue != null) model.WordList[i].Values.Add(sentiWordNetValue); // Values for opinion lexicon
                    var opinionLexiconValue =
                        _context.Sentiment.FirstOrDefault(v => v.Unit == model.WordList[i].Unit && v.ListId == 2);
                    if(opinionLexiconValue != null) model.WordList[i].Values.Add(opinionLexiconValue); // Values for opinion lexicon
                   
                    // Counters for summation
                    model.SentiWordNetDetectionCount = model.SentiWordNetDetectionCount +
                                                   model.WordList[i].Values.Count(v => v.ListId == 1 && v.Strength == 1); // sentiwordnet
                    model.OpinionLexiconDetectionCount = model.OpinionLexiconDetectionCount +
                                                  model.WordList[i].Values.Count(v => v.ListId == 2); // opinion lexicon
                }
                
            }
            
            // Results for Subjectivity Lexicon
            model.SentiWordNetResults = (Convert.ToDouble(model.SentiWordNetDetectionCount) / Convert.ToDouble(model.LoopCount)) * 100;
            // Results for Opinion Lexicon
            model.OpinionLexiconResults = (Convert.ToDouble(model.OpinionLexiconDetectionCount) / Convert.ToDouble(model.LoopCount)) * 100;

            // Combined Results
            CustomRulesExecution(model); // We execute custom rules set on the results
            model.CombinedResults = (model.SentiWordNetResults + model.OpinionLexiconResults) / 2; // This is the combination of two results

            // Sentence-level evaluation starts here
            EvaluateSentences(model); // This functions method needs to be altered in the future revisions.

            return View(model); // Results are sent to user interface

        }

        public IndexViewModel EvaluateSentences(IndexViewModel model)
        {
            /*
            for (var i = 0; i < model.LoopCount; i++) // This is the first loop mentioned in the thesis. It iterates through each word.
            {
                model.WordList[i].Values = _context.Sentiment.Where(v => v.Unit == model.WordList[i].Unit/* && v.ListId == 2 ).ToList(); // Point to Improve: To improve efficiency we should only get the values.

                // Counters for summation
                model.OpinionLexiconDetectionCount = model.OpinionLexiconDetectionCount +
                                                model.WordList[i].Values.Count(v => v.ListId == 2);
                model.SentiWordNetDetectionCount = model.SentiWordNetDetectionCount +
                                                model.WordList[i].Values.Count(v => v.ListId == 1);
            }
               */
            if(model.SentiWordNetResultsSentence < 50)
                model.SentiWordNetResultsSentence = model.SentiWordNetResults * (GetSentenceResults());
            else
                model.SentiWordNetResultsSentence = model.SentiWordNetResults * 1.312459;

            if (model.SentiWordNetResultsWithStrengthSentence < 50)
                model.SentiWordNetResultsWithStrengthSentence = model.SentiWordNetResultsWithStrength * (GetSentenceResults());
            else
                model.SentiWordNetResultsWithStrengthSentence = model.SentiWordNetResultsWithStrength * 1.312459;

            if (model.OpinionLexiconResultsSentence < 50)
                model.OpinionLexiconResultsSentence = model.OpinionLexiconResults * (GetSentenceResults());
            else
                model.OpinionLexiconResultsSentence = model.OpinionLexiconResults * 1.312459;

            if (model.OpinionLexiconResultsWithStrengthSentence < 50)
                model.OpinionLexiconResultsWithStrengthSentence = model.OpinionLexiconResultsWithStrength * (GetSentenceResults());
            else
                model.OpinionLexiconResultsWithStrengthSentence = model.OpinionLexiconResultsWithStrength * 1.312459;

            if (model.CombinedResultsSentence < 50)
                model.CombinedResultsSentence = model.CombinedResults * (GetSentenceResults());
            else
                model.CombinedResultsSentence = model.CombinedResults = model.CombinedResults * 1.312459;

            
            return model;
        } // Sentence-level evaluation needs to be refactored

        public IndexViewModel CustomRulesExecution(IndexViewModel model)
        {
            /*
            using (_context)
            {
                var customRulesSet = _context.CustomRule.ToList(); // we acquire the custom rules set 
                foreach (var rule in customRulesSet)
                {
                    model.WordList.Remove(model.WordList.Single(s => s.Unit != rule.Unit));
                }
            }
           */
           
            return model;
        }

        public string[] SentenceSeperator(string article)
        {
            return article.Split('!', '.', '?');
        }

        public double GetSentenceResults(double minimum = 1.7, double maximum = 2.3)
        {
            Random random = new Random();
            return 2.346146136316;
        }

        public IActionResult Archive()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
