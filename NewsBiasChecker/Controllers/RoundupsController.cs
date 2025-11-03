using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsBiasChecker.Data;
using NewsBiasChecker.Models;
using NewsBiasChecker.Models.ViewModels;
using NewsBiasChecker.Utils;

namespace NewsBiasChecker.Controllers
{
    public class RoundupsController : Controller
    {
        private readonly AppDbContext _context;

        public RoundupsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Roundups
        public async Task<IActionResult> Index()
        {
            var list = await _context.Roundups
                .Include(r => r.Stories)
                .OrderByDescending(r => r.Date)
                .ToListAsync();

            return View(list);
        }

        // GET: Roundups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var roundup = await _context.Roundups
                .Include(r => r.Stories)              // load child stories
                .FirstOrDefaultAsync(r => r.Id == id);

            if (roundup == null) return NotFound();

            return View(roundup);                     // pass a single Roundup to the view
        }




        // GET: Roundups/Create
        public IActionResult Create()
        {
            var vm = new RoundupEditViewModel
            {
                Date = DateTime.Today
            };
            return View(vm);
        }

        // POST: Roundups/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoundupEditViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var roundup = new Roundup
            {
                Title = vm.Title,
                Date = vm.Date,
                Topics = vm.Topics,
                StoryUrl = vm.StoryUrl,
                Description = vm.Description
            };

            foreach (var s in vm.Stories)
            {
                // Skip empty rows
                if (string.IsNullOrWhiteSpace(s.Title) && string.IsNullOrWhiteSpace(s.Url))
                    continue;

                roundup.Stories.Add(new Story
                {
                    Side = s.Side,
                    Title = s.Title,
                    Url = s.Url,
                    Text = s.Text,
                    Outlet = s.Outlet ?? UrlHelpers.OutletFrom(s.Url)
                });
            }

            _context.Add(roundup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Roundups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var r = await _context.Roundups
                .Include(x => x.Stories)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (r == null) return NotFound();

            var sides = new[] { "Left", "Center", "Right" };
            var vm = new RoundupEditViewModel
            {
                Id = r.Id,
                Title = r.Title,
                Date = r.Date,
                Topics = r.Topics,
                StoryUrl = r.StoryUrl,
                Description = r.Description,
                Stories = sides.Select(side =>
                {
                    var s = r.Stories.FirstOrDefault(t => t.Side == side);
                    return new StoryInput
                    {
                        Id = s?.Id,
                        Side = side,
                        Title = s?.Title ?? "",
                        Url = s?.Url,
                        Text = s?.Text,
                        Outlet = s?.Outlet
                    };
                }).ToList()
            };

            return View(vm);
        }

        // POST: Roundups/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoundupEditViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            var r = await _context.Roundups
                .Include(x => x.Stories)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (r == null) return NotFound();

            // Update parent
            r.Title = vm.Title;
            r.Date = vm.Date;
            r.Topics = vm.Topics;
            r.StoryUrl = vm.StoryUrl;
            r.Description = vm.Description;

            // Update or create the three stories
            foreach (var si in vm.Stories)
            {
                var existing = si.Id.HasValue
                    ? r.Stories.FirstOrDefault(s => s.Id == si.Id.Value)
                    : r.Stories.FirstOrDefault(s => s.Side == si.Side);

                if (existing == null)
                {
                    if (!string.IsNullOrWhiteSpace(si.Title) || !string.IsNullOrWhiteSpace(si.Url))
                    {
                        r.Stories.Add(new Story
                        {
                            Side = si.Side,
                            Title = si.Title,
                            Url = si.Url,
                            Text = si.Text,
                            Outlet = si.Outlet ?? UrlHelpers.OutletFrom(si.Url)
                        });
                    }
                }
                else
                {
                    existing.Side = si.Side;
                    existing.Title = si.Title;
                    existing.Url = si.Url;
                    existing.Text = si.Text;
                    existing.Outlet = si.Outlet ?? UrlHelpers.OutletFrom(si.Url);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Roundups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var roundup = await _context.Roundups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roundup == null) return NotFound();

            return View(roundup);
        }

        // POST: Roundups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roundup = await _context.Roundups.FindAsync(id);
            if (roundup != null)
            {
                _context.Roundups.Remove(roundup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoundupExists(int id)
        {
            return _context.Roundups.Any(e => e.Id == id);
        }
    }
}
