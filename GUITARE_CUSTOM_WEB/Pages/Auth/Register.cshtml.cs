using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GUITARE_CUSTOM_WEB.Models;

namespace GUITARE_CUSTOM_WEB.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public RegisterModel(HttpClient Http, IConfiguration Config)
        {
            _http = Http;
            _config = Config;
        }

        [BindProperty]
        public string Nom { get; set; }

        [BindProperty]
        public string Prenom { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Erreur { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var baseUrl = _config["Api:BaseUrl"];

            var Request = new
            {
                Nom = Nom,
                Prenom = Prenom,
                Email = Email,
                Password = Password,
            };

            try
            {
                var Response = await _http.PostAsJsonAsync($"{baseUrl}/Auth/Register", Request);

                var content = await Response.Content.ReadAsStringAsync();

                if (!Response.IsSuccessStatusCode)
                {
                    Erreur = string.IsNullOrWhiteSpace(content)
                        ? $"Erreur API ({(int)Response.StatusCode} - {Response.StatusCode})"
                        : content;

                    return Page();
                }

                var Utilisateur = await Response.Content.ReadFromJsonAsync<UTILISATEUR_DTO>();

                if (Utilisateur == null)
                {
                    Erreur = "Erreur inattendue côté serveur";
                    return Page();
                }

                return RedirectToPage("/Auth/Login");
            }
            catch (Exception ex)
            {
                Erreur = $"Erreur front : {ex.Message}";
                return Page();
            }
        }
    }
}