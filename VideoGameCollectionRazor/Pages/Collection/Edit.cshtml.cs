using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VideoGameCollection.Data;
using VideoGameCollection.Models;

namespace VideoGameCollection.Pages.Collection
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _db;

        public EditModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public VideoGame Game { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Account/Login");

            var game = await _db.VideoGames.FindAsync(id);
            if (game == null || game.UserId != userId.Value) 
                return NotFound();

            Game = game;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Account/Login");

            var gameFromDb = await _db.VideoGames.FindAsync(Game.Id);
            if (gameFromDb == null || gameFromDb.UserId != userId.Value)
                return NotFound();

            // Aggiorna solo i campi permessi
            gameFromDb.Title = Game.Title;
            gameFromDb.Platform = Game.Platform;
            gameFromDb.PersonalScore = Game.PersonalScore;
            gameFromDb.Genre = Game.Genre;
            gameFromDb.Notes = Game.Notes;
            gameFromDb.Status = Game.Status;
            
            await _db.SaveChangesAsync();
            TempData["Success"] = $"'{Game.Title}' aggiornato!";
            return RedirectToPage("/Collection/Index");
        }
    }
}
