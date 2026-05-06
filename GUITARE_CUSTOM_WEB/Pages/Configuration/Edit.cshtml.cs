using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GUITARE_CUSTOM_WEB.Models;

namespace GUITARE_CUSTOM_WEB.Pages.Configuration
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public EditModel(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        [BindProperty]
        public int IdConfiguration { get; set; }

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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            IdConfiguration = id;

            var chargement = await ChargerDonneesAsync();
            if (chargement is not PageResult)
                return chargement;

            return await ChargerConfigurationAsync(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Auth/Login");

            if (IdConfiguration <= 0)
            {
                Erreur = "Configuration invalide.";
                return await ChargerDonneesAsync();
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

            if (microsValides.Any(x => x.Position < 1 || x.Position > 3))
            {
                Erreur = "Les positions des micros doivent être comprises entre 1 et 3.";
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
                    IdConfiguration = IdConfiguration,
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
                        IdConfiguration = IdConfiguration,
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
                        IdConfiguration = IdConfiguration,
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

                var response = await _http.PutAsJsonAsync($"{baseUrl}/Configurations/Update", configComplete);

                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Erreur = !string.IsNullOrWhiteSpace(content)
                        ? content
                        : $"Erreur API ({(int)response.StatusCode})";

                    await ChargerDonneesAsync();
                    return Page();
                }

                return RedirectToPage("/Configuration/Index");
            }
            catch (Exception ex)
            {
                Erreur = $"Erreur front : {ex.Message}";
                await ChargerDonneesAsync();
                return Page();
            }
        }

        private async Task<IActionResult> ChargerConfigurationAsync(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            try
            {
                var baseUrl = _config["Api:BaseUrl"];

                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await _http.GetAsync($"{baseUrl}/Configurations/GetById?P_Id={id}");
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Erreur = !string.IsNullOrWhiteSpace(content)
                        ? content
                        : $"Erreur API ({(int)response.StatusCode})";
                    return Page();
                }

                var config = await response.Content.ReadFromJsonAsync<CONFIG_COMPLETE>();

                if (config?.Config == null)
                {
                    Erreur = "Configuration introuvable.";
                    return Page();
                }

                IdConfiguration = config.Config.IdConfiguration;
                NomConfiguration = config.Config.NomConfiguration;
                IdVibrato = config.Config.IdVibrato;

                IdMicros = new List<int> { 0, 0, 0 };
                PositionsMicros = new List<int> { 1, 2, 3 };

                foreach (var micro in config.Micros)
                {
                    if (micro.Position_ >= 1 && micro.Position_ <= 3)
                    {
                        IdMicros[micro.Position_ - 1] = micro.IdMicro;
                        PositionsMicros[micro.Position_ - 1] = micro.Position_;
                    }
                }

                IdBois = new List<int>();
                IdRolesBois = new List<int>();

                foreach (var role in RolesBois)
                {
                    IdRolesBois.Add(role.IdRoleBois);

                    var boisAssocie = config.Bois.FirstOrDefault(b => b.IdRoleBois == role.IdRoleBois);
                    IdBois.Add(boisAssocie?.IdBois ?? 0);
                }

                return Page();
            }
            catch (Exception ex)
            {
                Erreur = $"Erreur chargement configuration : {ex.Message}";
                return Page();
            }
        }

        private async Task<IActionResult> ChargerDonneesAsync()
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Auth/Login");

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