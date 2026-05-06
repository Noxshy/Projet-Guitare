using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using LIB_LOGIN;

namespace API_GUITARE_CUSTOM.Infrastructure;

public class Token
{
    public string AccessToken { get; set; }
}

public class TokenProvider
{
    private readonly IConfiguration _configuration;

    public TokenProvider(IConfiguration Configuration)
    {
        _configuration = Configuration;
    }

    public Token GenerateToken(UTILISATEUR_DTO User)
    {
        return new Token
        {
            AccessToken = GenerateAccessToken(User)
        };
    }

    private string GenerateAccessToken(UTILISATEUR_DTO Utilisateur)
    {
        var secretKey = _configuration["JWT:SecretKey"];

        if (string.IsNullOrEmpty(secretKey))
            throw new Exception("JWT SecretKey manquante");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, Utilisateur.Id.ToString()),
            new Claim(ClaimTypes.Name, Utilisateur.Email ?? ""),

            new Claim(ClaimTypes.Role, Utilisateur.Role.ToString()),

            new Claim(ClaimTypes.GivenName, Utilisateur.Prenom ?? ""),
            new Claim(ClaimTypes.Surname, Utilisateur.Nom ?? "")
        };

        var identity = new ClaimsIdentity(claims);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = DateTime.UtcNow.AddHours(1),

            SigningCredentials = credentials,
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"]

        };

        return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
    }
}