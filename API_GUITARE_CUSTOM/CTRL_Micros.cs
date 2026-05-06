using LIB_GUITARE_CUSTOM;
using Microsoft.AspNetCore.Mvc;

namespace WEBAPI_GUITARE;

[ApiController]
[Route("/Micros")]
public class CTRL_Micros : ControllerBase
{
    private BDD_GUITARE La_Base;

    public CTRL_Micros(BDD_GUITARE P_Base)
    {
        La_Base = P_Base;
    }

    [HttpGet("GetAll")]
    public async Task<IEnumerable<MICRO>> GetAll()
    {
        return await La_Base.Get_All_Micros();
    }

    [HttpPost("Add")]
    public async Task<MICRO> Add([FromBody] MICRO P_Micro)
    {
        return await La_Base.Add_Micro(
            P_Micro.NomModel,
            P_Micro.Marque,
            P_Micro.Quantite
        );
    }

    [HttpPut("Update")]
    public async Task<MICRO> Update([FromBody] MICRO P_Micro)
    {
        return await La_Base.Update_Micro(
            P_Micro.IdMicro,
            P_Micro.NomModel,
            P_Micro.Marque,
            P_Micro.Quantite,
            P_Micro.Disponible
        );
    }

    [HttpDelete("Del")]
    public async Task<bool> Delete([FromQuery] int P_Id)
    {
        return await La_Base.Del_Micro(P_Id);
    }
}