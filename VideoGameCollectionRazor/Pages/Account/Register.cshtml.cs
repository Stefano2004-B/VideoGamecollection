using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VideoGameCollection.Data;
using VideoGameCollection.Models;
using BC = BCrypt.Net.BCrypt;

namespace VideoGameCollection.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _db;

        public RegisterModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "Il nome utente è obbligatorio")]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "Il nome utente deve avere tra 3 e 100 caratteri")]
            [Display(Name = "Nome utente")]
            public string Username { get; set; } = string.Empty;

            [Required(ErrorMessage = "L'email è obbligatoria")]
            [EmailAddress(ErrorMessage = "Inserisci un'email valida")]
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "La password è obbligatoria")]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "La password deve avere almeno 6 caratteri")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "Conferma la password")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Le password non coincidono")]
            [Display(Name = "Conferma Password")]
            public string ConfirmPassword { get; set; } = string.Empty;
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

            // Controlla username duplicato
            if (await _db.Users.AnyAsync(u => u.Username == Input.Username))
            {
                ModelState.AddModelError("Input.Username", "Questo nome utente è già in uso.");
                return Page();
            }

            // Controlla email duplicata
            if (await _db.Users.AnyAsync(u => u.Email == Input.Email))
            {
                ModelState.AddModelError("Input.Email", "Questa email è già registrata.");
                return Page();
            }

            var user = new User
            {
                Username     = Input.Username,
                Email        = Input.Email,
                PasswordHash = BC.HashPassword(Input.Password),
                CreatedAt    = DateTime.Now
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // Login automatico dopo la registrazione
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetInt32("UserId", user.Id);

            TempData["Success"] = $"Benvenuto, {user.Username}! Account creato con successo.";
            return RedirectToPage("/Collection/Index");
        }
    }
}
