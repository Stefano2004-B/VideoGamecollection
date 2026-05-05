using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VideoGameCollection.Data;
using VideoGameCollection.Models;

namespace VideoGameCollection.Pages.Collection
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;

        public IndexModel(AppDbContext db)
        {
            _db = db;
        }

        public List<VideoGame> Games { get; set; } = new();
        public List<string> Platforms { get; set; } = new();
        public int TotalGames { get; set; }
        public double AvgScore { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Platform { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Sort { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Status { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Account/Login", new { returnUrl = "/Collection/Index" });

            var query = _db.VideoGames.Where(g => g.UserId == userId.Value).AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
                query = query.Where(g => g.Title.Contains(Search));

            if (!string.IsNullOrWhiteSpace(Platform))
                query = query.Where(g => g.Platform == Platform);

            if (!string.IsNullOrWhiteSpace(Status))
                query = query.Where(g => g.Status == Status);

            query = Sort switch
            {
                "score_desc" => query.OrderByDescending(g => g.PersonalScore),
                "score_asc"  => query.OrderBy(g => g.PersonalScore),
                "title"      => query.OrderBy(g => g.Title),
                _            => query.OrderByDescending(g => g.AddedDate)
            };

            Games     = await query.ToListAsync();
            
            var userGames = _db.VideoGames.Where(g => g.UserId == userId.Value);
            Platforms = await userGames.Select(g => g.Platform).Distinct().ToListAsync();
            TotalGames = await userGames.CountAsync();
            AvgScore  = await userGames.AnyAsync()
                ? Math.Round(await userGames.AverageAsync(g => g.PersonalScore), 1)
                : 0;

            return Page();
        }

        // Handler per la cancellazione — POST /Collection/Index?handler=Delete
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Account/Login");

            var game = await _db.VideoGames.FindAsync(id);
            if (game == null || game.UserId != userId.Value) return NotFound();

            _db.VideoGames.Remove(game);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"'{game.Title}' rimosso dalla collezione.";
            return RedirectToPage();
        }
    }
}
