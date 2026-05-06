using LIB_GUITARE_CUSTOM;
using Microsoft.AspNetCore.Mvc;

namespace WEBAPI_GUITARE;

[ApiController]
[Route("/Bois")]
public class CTRL_Bois : ControllerBase
{
    private BDD_GUITARE La_Base;

    public CTRL_Bois(BDD_GUITARE P_Base)
    {
        La_Base = P_Base;
    }

    [HttpGet("GetAll")]
    public async Task<IEnumerable<BOIS>> GetAll()
    {
        return await La_Base.Get_All_Bois();
    }

    [HttpPost("Add")]
    public async Task<BOIS> Add([FromBody] BOIS P_Bois)
    {
        return await La_Base.Add_Bois(
            P_Bois.NomBois,
            P_Bois.TypeBois,
            P_Bois.Quantite,
            P_Bois.IdCouleur
        );
    }

    [HttpPut("Update")]
    public async Task<BOIS> Update([FromBody] BOIS P_Bois)
    {
        return await La_Base.Update_Bois(
            P_Bois.IdBois,
            P_Bois.NomBois,
            P_Bois.TypeBois,
            P_Bois.Quantite,
            P_Bois.Disponible,
            P_Bois.IdCouleur
        );
    }

    [HttpDelete("Del")]
    public async Task<bool> Delete([FromQuery] int P_Id)
    {
        return await La_Base.Del_Bois(P_Id);
    }
}