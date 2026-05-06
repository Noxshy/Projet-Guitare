using LIB_GUITARE_CUSTOM;
using Microsoft.AspNetCore.Mvc;

namespace WEBAPI_GUITARE;

[ApiController]
[Route("/ConfigMicros")]
public class CTRL_ConfigMicros : ControllerBase
{
    private BDD_GUITARE La_Base;

    public CTRL_ConfigMicros(BDD_GUITARE P_Base)
    {
        La_Base = P_Base;
    }

    [HttpGet("GetByConfiguration")]
    public async Task<IEnumerable<CONFIG_MICRO>> GetByConfiguration([FromQuery] int P_IdConfiguration)
    {
        return await La_Base.Get_Micros_By_Configuration(P_IdConfiguration);
    }

    [HttpPost("Add")]
    public async Task<bool> Add([FromBody] CONFIG_MICRO P_Config)
    {
        return await La_Base.Add_Config_Micro(
            P_Config.IdMicro,
            P_Config.IdConfiguration,
            P_Config.Position_
        );
    }

    [HttpPut("UpdatePosition")]
    public async Task<bool> UpdatePosition([FromBody] CONFIG_MICRO P_Config)
    {
        return await La_Base.Update_Config_Micro_Position(
            P_Config.IdMicro,
            P_Config.IdConfiguration,
            P_Config.Position_
        );
    }

    [HttpDelete("Del")]
    public async Task<bool> Delete([FromBody] CONFIG_MICRO P_Config)
    {
        return await La_Base.Del_Config_Micro(
            P_Config.IdMicro,
            P_Config.IdConfiguration
        );
    }
}