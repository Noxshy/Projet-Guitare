using LIB_GUITARE_CUSTOM;
using Microsoft.AspNetCore.Mvc;

namespace WEBAPI_GUITARE;

[ApiController]
[Route("/Couleurs")]
public class CTRL_Couleurs : ControllerBase
{
    private BDD_GUITARE La_Base;

    public CTRL_Couleurs(BDD_GUITARE P_Base)
    {
        La_Base = P_Base;
    }

    [HttpGet("GetAll")]
    public async Task<IEnumerable<COULEUR>> GetAll()
    {
        return await La_Base.Get_All_Couleurs();
    }

    [HttpPost("Add")]
    public async Task<COULEUR> Add([FromBody] COULEUR P_Couleur)
    {
        return await La_Base.Add_Couleur(
            P_Couleur.NomCouleur,
            P_Couleur.CodeCouleur
        );
    }

    [HttpPut("Update")]
    public async Task<COULEUR> Update([FromBody] COULEUR P_Couleur)
    {
        return await La_Base.Update_Couleur(
            P_Couleur.IdCouleur,
            P_Couleur.NomCouleur,
            P_Couleur.CodeCouleur
        );
    }

    [HttpDelete("Del")]
    public async Task<bool> Delete([FromQuery] int P_Id)
    {
        return await La_Base.Del_Couleur(P_Id);
    }
}