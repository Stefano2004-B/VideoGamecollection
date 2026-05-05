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
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Account/Login");

            // Evita duplicati per l'utente corrente
            if (await _db.VideoGames.AnyAsync(g => g.ExternalId == externalId && g.UserId == userId.Value))
            {
                TempData["Warning"] = $"'{title}' è già nella tua collezione!";
                return RedirectToPage("/Home/Search");
            }

            var game = new VideoGame
            {
                UserId        = userId.Value,
                ExternalId    = externalId,
                Title         = title,
                Platform      = string.IsNullOrWhiteSpace(platform) ? "N/A" : platform,
                Genre         = genre,
                ImageUrl      = imageUrl,
                PersonalScore = 5,
                AddedDate     = DateTime.Now,
                Status        = "Da Giocare"
            };

            _db.VideoGames.Add(game);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"'{title}' aggiunto! Modifica il voto o lo stato dalla tua collezione.";
            return RedirectToPage("/Collection/Index");
        }
    }
}
