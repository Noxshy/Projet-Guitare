using LIB_GUITARE_CUSTOM;
using Microsoft.AspNetCore.Mvc;

namespace WEBAPI_GUITARE;

[ApiController]
[Route("/Suivi")]
public class CTRL_Suivi : ControllerBase
{
    private BDD_GUITARE La_Base;

    public CTRL_Suivi(BDD_GUITARE P_Base)
    {
        La_Base = P_Base;
    }

    [HttpGet("GetAll")]
    public async Task<IEnumerable<dynamic>> GetAll()
    {
        return await La_Base.Get_All_Suivi();
    }

    [HttpGet("GetByClient")]
    public async Task<IEnumerable<dynamic>> GetByClient([FromQuery] int P_IdClient)
    {
        return await La_Base.Get_Suivi_By_Utilisateur(P_IdClient);
    }

    [HttpGet("GetByCommande")]
    public async Task<IEnumerable<SUIVI_FABRICATION>> GetByCommande([FromQuery] int P_IdCommande)
    {
        return await La_Base.Get_Suivi_By_Commande(P_IdCommande);
    }

    [HttpPost("Add")]
    public async Task<SUIVI_FABRICATION> Add([FromBody] SUIVI_FABRICATION P_Suivi)
    {
        return await La_Base.Add_Suivi(
            P_Suivi.IdCommande,
            P_Suivi.UrlPhoto,
            P_Suivi.Commentaire
        );
    }

    [HttpPut("Update")]
    public async Task<SUIVI_FABRICATION> Update([FromBody] SUIVI_FABRICATION P_Suivi)
    {
        return await La_Base.Update_Suivi(
            P_Suivi.IdSuivi,
            P_Suivi.UrlPhoto,
            P_Suivi.Commentaire
        );
    }

    [HttpDelete("Del")]
    public async Task<bool> Delete([FromQuery] int P_Id)
    {
        return await La_Base.Del_Suivi(P_Id);
    }
}