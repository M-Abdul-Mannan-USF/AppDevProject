using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsBiasChecker.Data;
using NewsBiasChecker.Models;
using NewsBiasChecker.Models.ImportDtos;
using NewsBiasChecker.Utils;

namespace NewsBiasChecker.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: /Admin/Import
        public IActionResult Import() => View();

        // POST: /Admin/Import
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(string fileName = "huffpost.json")
        {
            var path = Path.Combine(_env.WebRootPath, "data", fileName);
            if (!System.IO.File.Exists(path))
            {
                TempData["Msg"] = $"File not found: {path}";
                return RedirectToAction(nameof(Import));
            }

            var text = await System.IO.File.ReadAllTextAsync(path);
            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var roundups = new List<Roundup>();
            int imported = 0;

            // Try NDJSON first (A)
            var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            bool looksNdjson = false;

            // Peek first valid object to decide shape
            foreach (var line in lines)
            {
                try
                {
                    using var obj = JsonDocument.Parse(line);
                    if (obj.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        looksNdjson = true;
                        break;
                    }
                }
                catch { /* skip blank/bad lines */ }
            }

            if (looksNdjson)
            {
                foreach (var line in lines)
                {
                    try
                    {
                        // Try HuffPost item
                        var hp = JsonSerializer.Deserialize<HuffPostNewsDto>(line, opts);
                        if (hp?.headline != null || hp?.link != null)
                        {
                            roundups.Add(MapHuffPost(hp));
                            imported++;
                            continue;
                        }

                        // (Optional) AllSides line
                        var al = JsonSerializer.Deserialize<AllSidesDto>(line, opts);
                        if (al?.Title_of_Headline_Roundup != null)
                        {
                            roundups.Add(MapAllSides(al));
                            imported++;
                            continue;
                        }
                    }
                    catch { /* skip bad lines */ }
                }
            }
            else
            {
                // Fallback: file might be a JSON array
                try
                {
                    using var doc = JsonDocument.Parse(text);
                    if (doc.RootElement.ValueKind == JsonValueKind.Array && doc.RootElement.GetArrayLength() > 0)
                    {
                        var first = doc.RootElement[0];

                        if (first.TryGetProperty("headline", out _))
                        {
                            var arr = JsonSerializer.Deserialize<List<HuffPostNewsDto>>(text, opts) ?? new();
                            foreach (var dto in arr) { roundups.Add(MapHuffPost(dto)); imported++; }
                        }
                        else if (first.TryGetProperty("left_story_title", out _))
                        {
                            var arr = JsonSerializer.Deserialize<List<AllSidesDto>>(text, opts) ?? new();
                            foreach (var dto in arr) { roundups.Add(MapAllSides(dto)); imported++; }
                        }
                    }
                }
                catch
                {
                    TempData["Msg"] = "Could not parse JSON (neither NDJSON nor array).";
                    return RedirectToAction(nameof(Import));
                }
            }

            if (imported == 0)
            {
                TempData["Msg"] = "No records imported. If this persists, paste the first 10–15 lines here.";
                return RedirectToAction(nameof(Import));
            }

            // (Optional) basic de-dup by Title+Date to avoid obvious repeats within this batch
            roundups = roundups
                .GroupBy(r => new { r.Title, r.Date })
                .Select(g => g.First())
                .ToList();

            _context.Roundups.AddRange(roundups);
            await _context.SaveChangesAsync();

            TempData["Msg"] = $"Imported {imported} items.";
            return RedirectToAction(nameof(Import));


            // ---- local mapping helpers ----
            Roundup MapHuffPost(HuffPostNewsDto dto)
            {
                DateTime d = DateTime.Today;
                if (!DateTime.TryParse(dto.date, out d))
                    DateTime.TryParseExact(dto.date,
                        new[] { "yyyy-MM-dd", "MM/dd/yyyy", "yyyy/MM/dd" },
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out d);

                var title = dto.headline?.Trim();
                var r = new Roundup
                {
                    Title = string.IsNullOrWhiteSpace(title) ? "(untitled)" : title!,
                    Topics = dto.category,
                    Date = d == default ? DateTime.Today : d,
                    StoryUrl = dto.link,
                    Description = dto.short_description
                };

                // One article → one Center story
                r.Stories.Add(new Story
                {
                    Side = "Center",
                    Title = r.Title,
                    Url = dto.link,
                    Text = dto.short_description,
                    Outlet = UrlHelpers.OutletFrom(dto.link)
                });

                return r;
            }

            Roundup MapAllSides(AllSidesDto dto)
            {
                DateTime d = DateTime.Today;
                if (!DateTime.TryParse(dto.Date, out d))
                    DateTime.TryParseExact(dto.Date,
                        new[] { "yyyy-MM-dd", "MM/dd/yyyy", "yyyy/MM/dd" },
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out d);

                var r = new Roundup
                {
                    Title = dto.Title_of_Headline_Roundup ?? "(untitled)",
                    Topics = dto.Topics,
                    Date = d == default ? DateTime.Today : d,
                    StoryUrl = dto.url_story,
                    Description = dto.description
                };

                void Add(string side, string? title, string? url, string? text)
                {
                    if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(url)) return;
                    r.Stories.Add(new Story
                    {
                        Side = side,
                        Title = title ?? "(untitled)",
                        Url = url,
                        Text = text,
                        Outlet = UrlHelpers.OutletFrom(url)
                    });
                }

                Add("Left", dto.left_story_title, dto.left_story_url, dto.left_story_text);
                Add("Center", dto.center_story_title, dto.center_story_url, dto.center_story_text);
                Add("Right", dto.right_story_title, dto.right_story_url, dto.right_story_text);

                return r;
            }
        }
    }
}

