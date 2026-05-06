using LIB_GUITARE_CUSTOM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WEBAPI_GUITARE;

[ApiController]
[Route("/Configurations")]
public class CTRL_Configurations : ControllerBase
{
    private readonly BDD_GUITARE La_Base;

    public CTRL_Configurations(BDD_GUITARE P_Base)
    {
        La_Base = P_Base;
    }

    // ================= PUBLIC =================

    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<CONFIG_COMPLETE>>> GetAll()
    {
        try
        {
            var Result = await La_Base.Get_All_Config_Complete();
            return Ok(Result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur GetAll : {ex.Message}");
        }
    }

    [HttpGet("GetById")]
    public async Task<ActionResult<CONFIG_COMPLETE>> GetById(int P_Id)
    {
        try
        {
            var Config = await La_Base.Get_Config_Complete(P_Id);

            if (Config == null || Config.Config == null)
                return NotFound("Configuration introuvable.");

            return Ok(Config);
        }
        catch (Exception Exception)
        {
            return StatusCode(500, $"Erreur GetById : {Exception.Message}");
        }
    }

    // ================= USER =================

    [Authorize]
    [HttpGet("MyConfigs")]
    public async Task<ActionResult<IEnumerable<CONFIG_COMPLETE>>> GetMyConfigs()
    {
        var Claim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (Claim == null || !int.TryParse(Claim.Value, out int idUtilisateur))
            return Unauthorized("Id utilisateur introuvable ou invalide.");

        try
        {
            var Result = await La_Base.Get_Config_Complete_By_User(idUtilisateur);
            return Ok(Result);
        }
        catch (Exception Exception)
        {
            return StatusCode(500, $"Erreur MyConfigs : {Exception.Message}");
        }
    }

    [Authorize]
    [HttpPost("Add")]
    public async Task<ActionResult<CONFIG_COMPLETE>> Add([FromBody] CONFIG_COMPLETE P_Config)
    {
        var Claim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (Claim == null || !int.TryParse(Claim.Value, out int idUtilisateur))
            return Unauthorized("Id utilisateur introuvable ou invalide.");

        if (P_Config?.Config == null)
            return BadRequest("Configuration invalide.");

        try
        {
            P_Config.Config.IdUtilisateur = idUtilisateur;

            var Result = await La_Base.Add_Config_Complete(P_Config);
            return Ok(Result);
        }
        catch (Exception Exception)
        {
            return StatusCode(500, $"Erreur Add : {Exception.Message}");
        }
    }

    [Authorize]
    [HttpPut("Update")]
    public async Task<ActionResult<CONFIG_COMPLETE>> Update([FromBody] CONFIG_COMPLETE P_Config)
    {
        var Claim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (Claim == null || !int.TryParse(Claim.Value, out int idUtilisateur))
            return Unauthorized("Id utilisateur introuvable ou invalide.");

        if (P_Config?.Config == null)
            return BadRequest("Configuration invalide.");

        try
        {
            var Config = await La_Base.Get_Configuration_By_Id(P_Config.Config.IdConfiguration);

            if (Config == null)
                return NotFound("Configuration introuvable.");

            if (Config.IdUtilisateur != idUtilisateur)
                return Forbid();

            var Result = await La_Base.Update_Config_Complete(P_Config);
            return Ok(Result);
        }
        catch (Exception Exception)
        {
            return StatusCode(500, $"Erreur Update : {Exception.Message}");
        }
    }

    [Authorize]
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(int P_Id)
    {
        var Claim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (Claim == null || !int.TryParse(Claim.Value, out int idUtilisateur))
            return Unauthorized("Id utilisateur introuvable ou invalide.");

        try
        {
            var Config = await La_Base.Get_Configuration_By_Id(P_Id);

            if (Config == null)
                return NotFound("Configuration introuvable.");

            if (Config.IdUtilisateur != idUtilisateur)
                return Forbid();

            var Deleted = await La_Base.Del_Config_Complete(P_Id);

            if (!Deleted)
                return StatusCode(500, "La suppression a échoué.");

            return NoContent();
        }
        catch (Exception Exception)
        {
            return StatusCode(500, $"Erreur Delete : {Exception.Message}");
        }
    }

    // ================= ADMIN =================

    [Authorize(Roles = "1")]
    [HttpGet("Admin/GetByUser")]
    public async Task<ActionResult<IEnumerable<CONFIG_COMPLETE>>> AdminGetByUser(int IdUtilisateur)
    {
        try
        {
            var Result = await La_Base.Get_Config_Complete_By_User(IdUtilisateur);
            return Ok(Result);
        }
        catch (Exception Exception)
        {
            return StatusCode(500, $"Erreur AdminGetByUser : {Exception.Message}");
        }
    }

    [Authorize(Roles = "1")]
    [HttpPut("Admin/Update")]
    public async Task<ActionResult<CONFIG_COMPLETE>> AdminUpdate([FromBody] CONFIG_COMPLETE P_Config)
    {
        if (P_Config?.Config == null)
            return BadRequest("Configuration invalide.");

        try
        {
            var Config = await La_Base.Get_Configuration_By_Id(P_Config.Config.IdConfiguration);

            if (Config == null)
                return NotFound("Configuration introuvable.");

            var Result = await La_Base.Update_Config_Complete(P_Config);
            return Ok(Result);
        }
        catch (Exception Exception)
        {
            return StatusCode(500, $"Erreur AdminUpdate : {Exception.Message}");
        }
    }

    [Authorize(Roles = "1")]
    [HttpDelete("Admin/Delete")]
    public async Task<IActionResult> AdminDelete(int P_Id)
    {
        try
        {
            var Config = await La_Base.Get_Configuration_By_Id(P_Id);

            if (Config == null)
                return NotFound("Configuration introuvable.");

            var Deleted = await La_Base.Del_Config_Complete(P_Id);

            if (!Deleted)
                return StatusCode(500, "La suppression a échoué.");

            return NoContent();
        }
        catch (Exception Exception)
        {
            return StatusCode(500, $"Erreur AdminDelete : {Exception.Message}");
        }
    }
}