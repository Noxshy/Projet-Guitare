namespace GUITARE_CUSTOM_WEB.Services;

using System.Net.Http.Headers;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _httpContext;

    public ApiService(HttpClient http, IHttpContextAccessor httpContext)
    {
        _http = http;
        _httpContext = httpContext;
    }

    private void AddToken()
    {
        var token = _httpContext.HttpContext.Session.GetString("Token");

        if (!string.IsNullOrEmpty(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        AddToken();
        return await _http.GetAsync(url);
    }

    public async Task<HttpResponseMessage> PostAsync(string url, object data)
    {
        AddToken();
        return await _http.PostAsJsonAsync(url, data);
    }
}