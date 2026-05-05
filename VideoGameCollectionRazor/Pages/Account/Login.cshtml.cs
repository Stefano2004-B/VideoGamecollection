using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VideoGameCollection.Data;
using BC = BCrypt.Net.BCrypt;

namespace VideoGameCollection.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _db;

        public LoginModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Il nome utente è obbligatorio")]
            [Display(Name = "Nome utente")]
            public string Username { get; set; } = string.Empty;

            [Required(ErrorMessage = "La password è obbligatoria")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = string.Empty;

            [Display(Name = "Ricordami")]
            public bool RememberMe { get; set; }
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Username") != null)
                return RedirectToPage("/Collection/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Username == Input.Username);

            if (user == null || !BC.Verify(Input.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Nome utente o password non validi.");
                return Page();
            }

            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetInt32("UserId", user.Id);

            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                return Redirect(ReturnUrl);

            return RedirectToPage("/Account/Welcome");
        }
    }
}
