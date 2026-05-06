using API_GUITARE_CUSTOM.Infrastructure;
using LIB_LOGIN;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WEBAPI_GUITARE;

[ApiController]
[Route("/Auth")]
public class CTRL_Auth : ControllerBase
{
    private readonly BDD_LOGIN La_Base;
    private readonly IConfiguration _config;

    public CTRL_Auth(BDD_LOGIN P_Base, IConfiguration Config)
    {
        La_Base = P_Base;
        _config = Config;
    }

    // ================= PUBLIC =================

    [HttpPost("Login")]
    public ActionResult Login([FromBody] LoginRequest P_Login)
    {
        if (P_Login == null || string.IsNullOrWhiteSpace(P_Login.Email) || string.IsNullOrWhiteSpace(P_Login.Password))
            return BadRequest("Email ou mot de passe manquant.");

        try
        {
            var User = La_Base.Phase_Login(P_Login.Email, P_Login.Password);

            if (User == null)
                return Unauthorized("Email ou mot de passe invalide.");

            var TokenProvider = new TokenProvider(_config);
            var Token = TokenProvider.GenerateToken(User);

            return Ok(new
            {
                accessToken = Token.AccessToken
            });
        }
        catch (Exception Exception)
        {
            return StatusCode(500, $"Erreur Login : {Exception.Message}");
        }
    }

    [HttpPost("Register")]
    public ActionResult<UTILISATEUR_DTO> Register([FromBody] RegisterRequest P_Register)
    {
        if (P_Register == null
            || string.IsNullOrWhiteSpace(P_Register.Nom)
            || string.IsNullOrWhiteSpace(P_Register.Prenom)
            || string.IsNullOrWhiteSpace(P_Register.Email)
            || string.IsNullOrWhiteSpace(P_Register.Password))
        {
            return BadRequest("Données d'inscription invalides.");
        }

        try
        {
            var User = La_Base.Creer_Utilisateur_Complet(
                P_Register.Nom,
                P_Register.Prenom,
                P_Register.Email,
                P_Register.Password,
                2
            );

            if (User == null)
                return StatusCode(500, "Création du compte impossible.");

            return Ok(User);
        }
        catch (Exception Exception)
        {
            return StatusCode(500, $"Erreur Register : {Exception.Message}");
        }
    }

    // ================= USER =================

    [Authorize]
    [HttpPut("UpdateEmail")]
    public ActionResult UpdateEmail(string P_Email)
    {
        var Claim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (Claim == null || !int.TryParse(Claim.Value, out int userId))
            return Unauthorized("Id utilisateur introuvable ou invalide.");

        if (string.IsNullOrWhiteSpace(P_Email))
            return BadRequest("Email invalide.");

        try
        {
            var Updated = La_Base.Update_Email(userId, P_Email);

            if (!Updated)
                return StatusCode(500, "Mise à jour de l'email impossible.");

            return NoContent();
        }
        catch (Exception Exception)
        {
            return StatusCode(500, $"Erreur UpdateEmail : {Exception.Message}");
        }
    }

    [Authorize]
    [HttpPut("UpdatePassword")]
    public ActionResult UpdatePassword(string P_Password)
    {
        var Claim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (Claim == null || !int.TryParse(Claim.Value, out int IdUtilisateur))
            return Unauthorized("Id utilisateur introuvable ou invalide.");

        if (string.IsNullOrWhiteSpace(P_Password))
            return BadRequest("Mot de passe invalide.");

        try
        {
            var Updated = La_Base.Update_MotDePasse(IdUtilisateur, P_Password);

            if (!Updated)
                return StatusCode(500, "Mise à jour du mot de passe impossible.");

            return NoContent();
        }
        catch (Exception Exception)
        {
            return StatusCode(500, $"Erreur UpdatePassword : {Exception.Message}");
        }
    }

    [Authorize]
    [HttpDelete("Delete")]
    public ActionResult Delete()
    {
        var Claim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (Claim == null || !int.TryParse(Claim.Value, out int idUtilisateur))
            return Unauthorized("Id utilisateur introuvable ou invalide.");

        try
        {
            var Deleted = La_Base.Del_Utilisateur(idUtilisateur);

            if (!Deleted)
                return NotFound("Utilisateur introuvable ou déjà supprimé.");

            return NoContent();
        }
        catch (Exception Exception)
        {
            return StatusCode(500, $"Erreur Delete : {Exception.Message}");
        }
    }

    // ================= ADMIN =================

    [Authorize(Roles = "1")]
    [HttpPost("CreateUser")]
    public ActionResult<UTILISATEUR_DTO> CreateUser([FromBody] RegisterRequest P_Register)
    {
        if (P_Register == null
            || string.IsNullOrWhiteSpace(P_Register.Nom)
            || string.IsNullOrWhiteSpace(P_Register.Prenom)
            || string.IsNullOrWhiteSpace(P_Register.Email)
            || string.IsNullOrWhiteSpace(P_Register.Password))
        {
            return BadRequest("Données utilisateur invalides.");
        }

        if (P_Register.Role != 1 && P_Register.Role != 2)
            return BadRequest("Rôle invalide.");

        try
        {
            var User = La_Base.Creer_Utilisateur_Complet(
                P_Register.Nom,
                P_Register.Prenom,
                P_Register.Email,
                P_Register.Password,
                P_Register.Role
            );

            if (User == null)
                return StatusCode(500, "Création de l'utilisateur impossible.");

            return Ok(User);
        }
        catch (Exception Exception)
        {
            return StatusCode(500, $"Erreur CreateUser : {Exception.Message}");
        }
    }

    [Authorize(Roles = "1")]
    [HttpPut("Admin/UpdateEmail")]
    public ActionResult AdminUpdateEmail(int P_IdUtilisateur, string P_Email)
    {
        if (P_IdUtilisateur <= 0)
            return BadRequest("Id utilisateur invalide.");

        if (string.IsNullOrWhiteSpace(P_Email))
            return BadRequest("Email invalide.");

        try
        {
            var Updated = La_Base.Update_Email(P_IdUtilisateur, P_Email);

            if (!Updated)
                return NotFound("Utilisateur introuvable.");

            return NoContent();
        }
        catch (Exception Exception)
        {
            return StatusCode(500, $"Erreur AdminUpdateEmail : {Exception.Message}");
        }
    }

    [Authorize(Roles = "1")]
    [HttpPut("Admin/UpdatePassword")]
    public ActionResult AdminUpdatePassword(int IdUtilisateur, string P_Password)
    {
        if (IdUtilisateur <= 0)
            return BadRequest("Id utilisateur invalide.");

        if (string.IsNullOrWhiteSpace(P_Password))
            return BadRequest("Mot de passe invalide.");

        try
        {
            var Updated = La_Base.Update_MotDePasse(IdUtilisateur, P_Password);

            if (!Updated)
                return NotFound("Utilisateur introuvable.");

            return NoContent();
        }
        catch (Exception Exception)
        {
            return StatusCode(500, $"Erreur AdminUpdatePassword : {Exception.Message}");
        }
    }

    [Authorize(Roles = "1")]
    [HttpDelete("Admin/Delete")]
    public ActionResult AdminDelete(int P_IdUtilisateur)
    {
        if (P_IdUtilisateur <= 0)
            return BadRequest("Id utilisateur invalide.");

        try
        {
            var Deleted = La_Base.Del_Utilisateur(P_IdUtilisateur);

            if (!Deleted)
                return NotFound("Utilisateur introuvable.");

            return NoContent();
        }
        catch (Exception Exception)
        {
            return StatusCode(500, $"Erreur AdminDelete : {Exception.Message}");
        }
    }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterRequest
{
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Role { get; set; }
}