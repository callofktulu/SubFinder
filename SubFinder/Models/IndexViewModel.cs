using System.Collections.Generic;


namespace SubFinder.Models
{
    public class IndexViewModel
    {
        public List<LexicalItems.Word> WordList { get; set; }
        public List<LexicalItems.Sentence> SentenceList { get; set; }
        public double SentiWordNetResults { get; set; }
        public double SentiWordNetResultsWithStrength { get; set; }
        public double OpinionLexiconResults { get; set; }
        public double OpinionLexiconResultsWithStrength { get; set; }
        public double CombinedResults { get; set; }
        public double SentiWordNetResultsSentence { get; set; }
        public double SentiWordNetResultsWithStrengthSentence { get; set; }
        public double OpinionLexiconResultsSentence { get; set; }
        public double OpinionLexiconResultsWithStrengthSentence { get; set; }
        public double CombinedResultsSentence { get; set; }
        public int OpinionLexiconDetectionCount { get; set; }
        public int SentiWordNetDetectionCount { get; set; }
        public int LoopCount { get; set; }

    }
}
