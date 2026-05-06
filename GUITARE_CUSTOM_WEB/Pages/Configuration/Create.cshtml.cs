using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GUITARE_CUSTOM_WEB.Models;

namespace GUITARE_CUSTOM_WEB.Pages.Configuration
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public CreateModel(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        [BindProperty]
        public string NomConfiguration { get; set; } = string.Empty;

        [BindProperty]
        public int IdVibrato { get; set; }

        [BindProperty]
        public List<int> IdMicros { get; set; } = new();

        [BindProperty]
        public List<int> PositionsMicros { get; set; } = new();

        [BindProperty]
        public List<int> IdBois { get; set; } = new();

        [BindProperty]
        public List<int> IdRolesBois { get; set; } = new();

        public List<VIBRATO> Vibratos { get; set; } = new();
        public List<MICRO> MicrosDisponibles { get; set; } = new();
        public List<BOIS_VIEW> BoisDisponibles { get; set; } = new();
        public List<ROLE_BOIS> RolesBois { get; set; } = new();

        public string Erreur { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            return await ChargerDonneesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Auth/Login");
            }

            if (string.IsNullOrWhiteSpace(NomConfiguration))
            {
                Erreur = "Le nom de la configuration est obligatoire.";
                return await ChargerDonneesAsync();
            }

            if (IdVibrato <= 0)
            {
                Erreur = "Le vibrato est obligatoire.";
                return await ChargerDonneesAsync();
            }

            if (IdBois.Count == 0 || IdRolesBois.Count == 0 || IdBois.Count != IdRolesBois.Count)
            {
                Erreur = "Les bois doivent être correctement renseignés.";
                return await ChargerDonneesAsync();
            }

            if (IdMicros.Count != PositionsMicros.Count)
            {
                Erreur = "Les micros sont invalides.";
                return await ChargerDonneesAsync();
            }

            if (IdBois.Any(id => id <= 0))
            {
                Erreur = "Tu dois choisir un bois pour chaque rôle.";
                return await ChargerDonneesAsync();
            }

            var microsValides = IdMicros
                .Select((id, index) => new { IdMicro = id, Position = PositionsMicros[index] })
                .Where(x => x.IdMicro > 0)
                .ToList();

            if (microsValides.Any(x => x.Position <= 0))
            {
                Erreur = "Les positions des micros doivent être supérieures à 0.";
                return await ChargerDonneesAsync();
            }

            if (microsValides.GroupBy(x => x.Position).Any(g => g.Count() > 1))
            {
                Erreur = "Deux micros ne peuvent pas avoir la même position.";
                return await ChargerDonneesAsync();
            }

            var configComplete = new CONFIG_COMPLETE
            {
                Config = new CONFIGURATION
                {
                    NomConfiguration = NomConfiguration,
                    IdVibrato = IdVibrato
                },
                Micros = new List<CONFIG_MICRO>(),
                Bois = new List<CONFIG_BOIS>()
            };

            for (int i = 0; i < IdMicros.Count; i++)
            {
                if (IdMicros[i] > 0)
                {
                    configComplete.Micros.Add(new CONFIG_MICRO
                    {
                        IdMicro = IdMicros[i],
                        Position_ = PositionsMicros[i]
                    });
                }
            }

            for (int i = 0; i < IdBois.Count; i++)
            {
                if (IdBois[i] > 0 && IdRolesBois[i] > 0)
                {
                    configComplete.Bois.Add(new CONFIG_BOIS
                    {
                        IdBois = IdBois[i],
                        IdRoleBois = IdRolesBois[i]
                    });
                }
            }

            try
            {
                var baseUrl = _config["Api:BaseUrl"];

                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _http.PostAsJsonAsync($"{baseUrl}/Configurations/Add", configComplete);

                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Erreur = $"Erreur API ({(int)response.StatusCode} - {response.StatusCode}) : {content}";
                    return await ChargerDonneesAsync();
                }

                return RedirectToPage("/Configuration/Index");
            }
            catch (Exception ex)
            {
                Erreur = $"Erreur front : {ex.Message}";
                return await ChargerDonneesAsync();
            }
        }

        private async Task<IActionResult> ChargerDonneesAsync()
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

                var vibratosResponse = await _http.GetAsync($"{baseUrl}/Vibratos/GetAll");
                var microsResponse = await _http.GetAsync($"{baseUrl}/Micros/GetAll");
                var boisResponse = await _http.GetAsync($"{baseUrl}/Bois/GetAll");
                var couleursResponse = await _http.GetAsync($"{baseUrl}/Couleurs/GetAll");
                var rolesBoisResponse = await _http.GetAsync($"{baseUrl}/RoleBois/GetAll");

                if (!vibratosResponse.IsSuccessStatusCode)
                {
                    Erreur = $"Vibratos KO : {(int)vibratosResponse.StatusCode} - {vibratosResponse.StatusCode}";
                    return Page();
                }

                if (!microsResponse.IsSuccessStatusCode)
                {
                    Erreur = $"Micros KO : {(int)microsResponse.StatusCode} - {microsResponse.StatusCode}";
                    return Page();
                }

                if (!boisResponse.IsSuccessStatusCode)
                {
                    Erreur = $"Bois KO : {(int)boisResponse.StatusCode} - {boisResponse.StatusCode}";
                    return Page();
                }

                if (!couleursResponse.IsSuccessStatusCode)
                {
                    Erreur = $"Couleurs KO : {(int)couleursResponse.StatusCode} - {couleursResponse.StatusCode}";
                    return Page();
                }

                if (!rolesBoisResponse.IsSuccessStatusCode)
                {
                    Erreur = $"RoleBois KO : {(int)rolesBoisResponse.StatusCode} - {rolesBoisResponse.StatusCode}";
                    return Page();
                }

                Vibratos = await vibratosResponse.Content.ReadFromJsonAsync<List<VIBRATO>>() ?? new();
                MicrosDisponibles = await microsResponse.Content.ReadFromJsonAsync<List<MICRO>>() ?? new();

                var bois = await boisResponse.Content.ReadFromJsonAsync<List<BOIS>>() ?? new();
                var couleurs = await couleursResponse.Content.ReadFromJsonAsync<List<COULEUR>>() ?? new();

                var couleursParId = couleurs.ToDictionary(c => c.IdCouleur, c => c);

                BoisDisponibles = bois.Select(b =>
                {
                    couleursParId.TryGetValue(b.IdCouleur, out var couleur);

                    return new BOIS_VIEW
                    {
                        IdBois = b.IdBois,
                        NomBois = b.NomBois,
                        TypeBois = b.TypeBois,
                        Quantite = b.Quantite,
                        Disponible = b.Disponible,
                        IdCouleur = b.IdCouleur,
                        NomCouleur = couleur?.NomCouleur ?? "Inconnue",
                        CodeCouleur = couleur?.CodeCouleur ?? "#000000"
                    };
                }).ToList();

                RolesBois = await rolesBoisResponse.Content.ReadFromJsonAsync<List<ROLE_BOIS>>() ?? new();

                return Page();
            }
            catch (Exception ex)
            {
                Erreur = $"Erreur chargement catalogue : {ex.Message}";
                return Page();
            }
        }
    }
}