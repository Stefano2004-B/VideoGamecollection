using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VideoGameCollection.Pages.Account
{
    public class WelcomeModel : PageModel
    {
        public string Username { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null)
                return RedirectToPage("/Account/Login");

            Username = username;
            return Page();
        }
    }
}
