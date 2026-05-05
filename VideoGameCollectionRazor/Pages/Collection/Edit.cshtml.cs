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
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToPage("/Account/Login");

            var game = await _db.VideoGames.FindAsync(id);
            if (game == null) return NotFound();

            Game = game;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _db.VideoGames.Update(Game);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"'{Game.Title}' aggiornato!";
            return RedirectToPage("/Collection/Index");
        }
    }
}
