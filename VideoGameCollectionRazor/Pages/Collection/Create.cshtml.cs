using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VideoGameCollection.Data;
using VideoGameCollection.Models;

namespace VideoGameCollection.Pages.Collection
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _db;

        public CreateModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public VideoGame Game { get; set; } = new();

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToPage("/Account/Login");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToPage("/Account/Login");

            Game.UserId = userId.Value;
            Game.AddedDate = DateTime.Now;
            _db.VideoGames.Add(Game);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"'{Game.Title}' aggiunto alla collezione!";
            return RedirectToPage("/Collection/Index");
        }
    }
}
