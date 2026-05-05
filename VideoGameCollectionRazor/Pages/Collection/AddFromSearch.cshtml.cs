using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VideoGameCollection.Data;
using VideoGameCollection.Models;

namespace VideoGameCollection.Pages.Collection
{
    public class AddFromSearchModel : PageModel
    {
        private readonly AppDbContext _db;

        public AddFromSearchModel(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> OnPostAsync(
            int externalId, string title, string platform,
            string? genre, string? imageUrl)
        {
            // Evita duplicati
            if (await _db.VideoGames.AnyAsync(g => g.ExternalId == externalId))
            {
                TempData["Warning"] = $"'{title}' è già nella tua collezione!";
                return RedirectToPage("/Home/Search");
            }

            var game = new VideoGame
            {
                ExternalId    = externalId,
                Title         = title,
                Platform      = string.IsNullOrWhiteSpace(platform) ? "N/A" : platform,
                Genre         = genre,
                ImageUrl      = imageUrl,
                PersonalScore = 5,
                AddedDate     = DateTime.Now
            };

            _db.VideoGames.Add(game);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"'{title}' aggiunto! Modifica il voto dalla tua collezione.";
            return RedirectToPage("/Collection/Index");
        }
    }
}
