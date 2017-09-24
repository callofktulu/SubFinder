using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubFinder.Data;
using SubFinder.Models;

namespace SubFinder.Controllers
{
    public class SentimentsController : Controller
    {
        private readonly SubFinderContext _context;

        public SentimentsController(SubFinderContext context)
        {
            _context = context;
        }

        // GET: Sentiments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sentiment.ToListAsync());
        }

        // GET: Sentiments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sentiment = await _context.Sentiment
                .SingleOrDefaultAsync(m => m.SentimentId == id);
            if (sentiment == null)
            {
                return NotFound();
            }

            return View(sentiment);
        }

        // GET: Sentiments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sentiments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SentimentId,Polarity,Unit,Strength,ListId")] Sentiment sentiment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sentiment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sentiment);
        }

        // GET: Sentiments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sentiment = await _context.Sentiment.SingleOrDefaultAsync(m => m.SentimentId == id);
            if (sentiment == null)
            {
                return NotFound();
            }
            return View(sentiment);
        }

        // POST: Sentiments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SentimentId,Polarity,Unit,Strength,ListId")] Sentiment sentiment)
        {
            if (id != sentiment.SentimentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sentiment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SentimentExists(sentiment.SentimentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sentiment);
        }

        // GET: Sentiments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sentiment = await _context.Sentiment
                .SingleOrDefaultAsync(m => m.SentimentId == id);
            if (sentiment == null)
            {
                return NotFound();
            }

            return View(sentiment);
        }

        // POST: Sentiments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sentiment = await _context.Sentiment.SingleOrDefaultAsync(m => m.SentimentId == id);
            _context.Sentiment.Remove(sentiment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SentimentExists(int id)
        {
            return _context.Sentiment.Any(e => e.SentimentId == id);
        }
    }
}
