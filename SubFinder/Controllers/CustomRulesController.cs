using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SubFinder.Data;
using SubFinder.Models;

namespace SubFinder.Controllers
{
    public class CustomRulesController : Controller
    {
        private readonly SubFinderContext _context;

        public CustomRulesController(SubFinderContext context)
        {
            _context = context;
        }

        // GET: CustomRules
        public async Task<IActionResult> Index()
        {
            return View(await _context.CustomRule.ToListAsync());
        }

        // GET: CustomRules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customRule = await _context.CustomRule
                .SingleOrDefaultAsync(m => m.CustomRuleId == id);
            if (customRule == null)
            {
                return NotFound();
            }

            return View(customRule);
        }

        // GET: CustomRules/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomRules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomRuleId,Unit,ExecutionRule,ExecutionParameters")] CustomRule customRule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customRule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customRule);
        }

        // GET: CustomRules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customRule = await _context.CustomRule.SingleOrDefaultAsync(m => m.CustomRuleId == id);
            if (customRule == null)
            {
                return NotFound();
            }
            return View(customRule);
        }

        // POST: CustomRules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomRuleId,Unit,ExecutionRule,ExecutionParameters")] CustomRule customRule)
        {
            if (id != customRule.CustomRuleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customRule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomRuleExists(customRule.CustomRuleId))
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
            return View(customRule);
        }

        // GET: CustomRules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customRule = await _context.CustomRule
                .SingleOrDefaultAsync(m => m.CustomRuleId == id);
            if (customRule == null)
            {
                return NotFound();
            }

            return View(customRule);
        }

        // POST: CustomRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customRule = await _context.CustomRule.SingleOrDefaultAsync(m => m.CustomRuleId == id);
            _context.CustomRule.Remove(customRule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomRuleExists(int id)
        {
            return _context.CustomRule.Any(e => e.CustomRuleId == id);
        }
    }
}
