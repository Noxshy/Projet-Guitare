using LIB_GUITARE_CUSTOM;
using Microsoft.AspNetCore.Mvc;

namespace WEBAPI_GUITARE;

[ApiController]
[Route("/RoleBois")]
public class CTRL_RoleBois : ControllerBase
{
    private BDD_GUITARE La_Base;

    public CTRL_RoleBois(BDD_GUITARE P_Base)
    {
        La_Base = P_Base;
    }

    [HttpGet("GetAll")]
    public async Task<IEnumerable<ROLE_BOIS>> GetAll()
    {
        return await La_Base.Get_All_RoleBois();
    }

    [HttpPost("Add")]
    public async Task<ROLE_BOIS> Add([FromBody] ROLE_BOIS P_RoleBois)
    {
        return await La_Base.Add_RoleBois(
            P_RoleBois.NomRole
        );
    }

    [HttpPut("Update")]
    public async Task<ROLE_BOIS> Update([FromBody] ROLE_BOIS P_RoleBois)
    {
        return await La_Base.Update_RoleBois(
            P_RoleBois.IdRoleBois,
            P_RoleBois.NomRole
        );
    }

    [HttpDelete("Del")]
    public async Task<bool> Delete([FromQuery] int P_Id)
    {
        return await La_Base.Del_RoleBois(P_Id);
    }
}