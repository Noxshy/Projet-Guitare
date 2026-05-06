using LIB_GUITARE_CUSTOM;
using Microsoft.AspNetCore.Mvc;

namespace WEBAPI_GUITARE;

[ApiController]
[Route("/Commandes")]
public class CTRL_Commandes : ControllerBase
{
    private BDD_GUITARE La_Base;

    public CTRL_Commandes(BDD_GUITARE P_Base)
    {
        La_Base = P_Base;
    }

    [HttpGet("GetAll")]
    public async Task<IEnumerable<COMMANDE>> GetAll()
    {
        return await La_Base.Get_All_Commandes();
    }

    [HttpGet("GetById")]
    public async Task<COMMANDE> GetById([FromQuery] int P_Id)
    {
        return await La_Base.Get_Commande_By_Id(P_Id);
    }

    [HttpGet("GetByUtilisateur")]
    public async Task<IEnumerable<COMMANDE>> GetByUtilisateur([FromQuery] int P_IdUtilisateur)
    {
        return await La_Base.Get_Commandes_By_Utilisateur(P_IdUtilisateur);
    }

    [HttpPost("Add")]
    public async Task<COMMANDE> Add([FromBody] COMMANDE P_Commande)
    {
        return await La_Base.Add_Commande(
            P_Commande.IdConfiguration,
            P_Commande.IdUtilisateur
        );
    }

    [HttpPut("UpdateStatut")]
    public async Task<COMMANDE> UpdateStatut([FromBody] COMMANDE P_Commande)
    {
        return await La_Base.Update_Commande_Statut(
            P_Commande.IdCommande,
            P_Commande.StatutCommande
        );
    }

    [HttpPut("Annuler")]
    public async Task<bool> Annuler([FromQuery] int P_IdCommande, [FromQuery] int? P_IdUtilisateur)
    {
        return await La_Base.Annule_Commande(P_IdCommande, P_IdUtilisateur);
    }

    [HttpPut("AdminUpdate")]
    public async Task<COMMANDE> AdminUpdate([FromBody] COMMANDE P_Commande)
    {
        return await La_Base.Update_Commande_Admin(
            P_Commande.IdCommande,
            P_Commande.StatutCommande,
            P_Commande.IdConfiguration
        );
    }

    [HttpDelete("Del")]
    public async Task<bool> Delete([FromQuery] int P_Id)
    {
        return await La_Base.Del_Commande(P_Id);
    }
}