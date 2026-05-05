using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using VideoGameCollection.Models;

namespace VideoGameCollection.Pages.Home
{
    public class SearchModel : PageModel
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly IConfiguration _config;

        public SearchModel(IHttpClientFactory httpFactory, IConfiguration config)
        {
            _httpFactory = httpFactory;
            _config      = config;
        }

        [BindProperty(SupportsGet = true)]
        public string? Q { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; } = 1;
        public RawgSearchResult? Result { get; set; }
        public string? Error { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToPage("/Account/Login", new { returnUrl = "/Home/Search" });

            if (string.IsNullOrWhiteSpace(Q))
                return Page();

            const int pageSize = 24;
            var apiKey = _config["Rawg:ApiKey"] ?? "c05dc9df09cf4ab88181d598ca268ada";

            var url = $"https://api.rawg.io/api/games" +
                      $"?key={apiKey}" +
                      $"&search={Uri.EscapeDataString(Q)}" +
                      $"&page_size={pageSize}" +
                      $"&page={CurrentPage}" +
                      $"&search_precise=false" +
                      $"&search_exact=false";

            try
            {
                var client = _httpFactory.CreateClient();
                client.DefaultRequestHeaders.Add("User-Agent", "VideoGameCollection/1.0");
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json    = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Result      = JsonSerializer.Deserialize<RawgSearchResult>(json, options)
                              ?? new RawgSearchResult();

                TotalPages = (int)Math.Ceiling((double)Result.Count / pageSize);
            }
            catch (Exception ex)
            {
                Error  = $"Errore nella ricerca: {ex.Message}";
                Result = new RawgSearchResult();
            }

            return Page();
        }
    }
}
