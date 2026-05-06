using LIB_GUITARE_CUSTOM;
using Microsoft.AspNetCore.Mvc;

namespace WEBAPI_GUITARE;

[ApiController]
[Route("/ConfigBois")]
public class CTRL_ConfigBois : ControllerBase
{
    private BDD_GUITARE La_Base;

    public CTRL_ConfigBois(BDD_GUITARE P_Base)
    {
        La_Base = P_Base;
    }

    [HttpGet("GetByConfiguration")]
    public async Task<IEnumerable<CONFIG_BOIS>> GetByConfiguration([FromQuery] int P_IdConfiguration)
    {
        return await La_Base.Get_Bois_By_Configuration(P_IdConfiguration);
    }

    [HttpPost("Add")]
    public async Task<bool> Add([FromBody] CONFIG_BOIS P_Config)
    {
        return await La_Base.Add_Config_Bois(
            P_Config.IdBois,
            P_Config.IdConfiguration,
            P_Config.IdRoleBois
        );
    }

    [HttpPut("UpdateRole")]
    public async Task<bool> UpdateRole([FromBody] CONFIG_BOIS P_Config, int AncienRole)
    {
        return await La_Base.Update_Config_Bois_Role(
            P_Config.IdBois,
            P_Config.IdConfiguration,
            AncienRole,
            P_Config.IdRoleBois
        );
    }

    [HttpDelete("Del")]
    public async Task<bool> Delete([FromBody] CONFIG_BOIS P_Config)
    {
        return await La_Base.Del_Config_Bois(
            P_Config.IdBois,
            P_Config.IdConfiguration,
            P_Config.IdRoleBois
        );
    }
}