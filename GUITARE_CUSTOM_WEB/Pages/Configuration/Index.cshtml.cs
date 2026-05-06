using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GUITARE_CUSTOM_WEB.Models;

namespace GUITARE_CUSTOM_WEB.Pages.Configuration
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public List<CONFIG_COMPLETE> Configurations { get; set; } = new();
        public string Erreur { get; set; } = string.Empty;

        public IndexModel(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return await ChargerConfigurationsAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Auth/Login");
            }

            try
            {
                var baseUrl = _config["Api:BaseUrl"];

                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _http.DeleteAsync($"{baseUrl}/Configurations/Delete?P_Id={id}");
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Erreur = !string.IsNullOrWhiteSpace(content)
                        ? content
                        : $"Erreur API ({(int)response.StatusCode})";

                    return await ChargerConfigurationsAsync();
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Erreur = $"Erreur suppression : {ex.Message}";
                return await ChargerConfigurationsAsync();
            }
        }

        private async Task<IActionResult> ChargerConfigurationsAsync()
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Auth/Login");
            }

            try
            {
                var baseUrl = _config["Api:BaseUrl"];

                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _http.GetAsync($"{baseUrl}/Configurations/MyConfigs");
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Erreur = !string.IsNullOrWhiteSpace(content)
                        ? content
                        : $"Erreur API ({(int)response.StatusCode})";

                    return Page();
                }

                var result = await response.Content.ReadFromJsonAsync<List<CONFIG_COMPLETE>>();
                Configurations = result ?? new List<CONFIG_COMPLETE>();

                return Page();
            }
            catch (Exception ex)
            {
                Erreur = $"Erreur chargement configurations : {ex.Message}";
                return Page();
            }
        }
    }
}