using GUITARE_CUSTOM_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GUITARE_CUSTOM_WEB.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public LoginModel(HttpClient Http, IConfiguration Config)
        {
            _http = Http;
            _config = Config;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Erreur { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var baseUrl = _config["Api:BaseUrl"];

            var Request = new
            {
                Email = this.Email,
                Password = Password
            };

            try
            {
                var Response = await _http.PostAsJsonAsync($"{baseUrl}/Auth/Login", Request);

                var content = await Response.Content.ReadAsStringAsync();

                if (!Response.IsSuccessStatusCode)
                {
                    Erreur = string.IsNullOrWhiteSpace(content)
                        ? $"Erreur API ({(int)Response.StatusCode} - {Response.StatusCode})"
                        : content;

                    return Page();
                }

                var Result = await Response.Content.ReadFromJsonAsync<TokenResponse>();

                if (Result == null || string.IsNullOrEmpty(Result.AccessToken))
                {
                    Erreur = "Identifiants incorrects";
                    return Page();
                }

                HttpContext.Session.SetString("Token", Result.AccessToken);

                var Handler = new JwtSecurityTokenHandler();
                var JWT = Handler.ReadJwtToken(Result.AccessToken);

                var IdUtilisateur = JWT.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var Email = JWT.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var Prenom = JWT.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                var Nom = JWT.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                var Role = JWT.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (int.TryParse(IdUtilisateur, out int idUtilisateurInt))
                {
                    HttpContext.Session.SetInt32("IdUtilisateur", idUtilisateurInt);
                }

                HttpContext.Session.SetString("Email", Email ?? "");
                HttpContext.Session.SetString("Prenom", Prenom ?? "");
                HttpContext.Session.SetString("Nom", Nom ?? "");

                if (int.TryParse(Role, out int roleInt))
                {
                    HttpContext.Session.SetInt32("Role", roleInt);
                }

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                Erreur = $"Erreur front : {ex.Message}";
                return Page();
            }
        }
    }
}