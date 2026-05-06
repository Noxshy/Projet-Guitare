using LIB_GUITARE_CUSTOM;
using Microsoft.AspNetCore.Mvc;

namespace WEBAPI_GUITARE;

[ApiController]
[Route("/Vibratos")]
public class CTRL_Vibratos : ControllerBase
{
    private BDD_GUITARE La_Base;

    public CTRL_Vibratos(BDD_GUITARE P_Base)
    {
        La_Base = P_Base;
    }

    [HttpGet("GetAll")]
    public async Task<IEnumerable<VIBRATO>> GetAll()
    {
        return await La_Base.Get_All_Vibratos();
    }

    [HttpPost("Add")]
    public async Task<VIBRATO> Add([FromBody] VIBRATO P_Vibrato)
    {
        return await La_Base.Add_Vibrato(
            P_Vibrato.NomModel,
            P_Vibrato.Marque,
            P_Vibrato.Quantite
        );
    }

    [HttpPut("Update")]
    public async Task<VIBRATO> Update([FromBody] VIBRATO P_Vibrato)
    {
        return await La_Base.Update_Vibrato(
            P_Vibrato.IdVibrato,
            P_Vibrato.NomModel,
            P_Vibrato.Marque,
            P_Vibrato.Quantite,
            P_Vibrato.Disponible
        );
    }

    [HttpDelete("Del")]
    public async Task<bool> Delete([FromQuery] int P_Id)
    {
        return await La_Base.Del_Vibrato(P_Id);
    }
}