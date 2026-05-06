using LIB_GUITARE_CUSTOM;
using Microsoft.AspNetCore.Mvc;

namespace WEBAPI_GUITARE;

[ApiController]
[Route("/Clients")]
public class CTRL_Utilisateurs : ControllerBase
{
    private BDD_GUITARE La_Base;

    public CTRL_Utilisateurs(BDD_GUITARE P_Base)
    {
        La_Base = P_Base;
    }

    [HttpGet("GetAll")]
    public async Task<IEnumerable<UTILISATEUR>> GetAll()
    {
        return await La_Base.Get_All_Utilisateurs();
    }

    [HttpGet("GetById")]
    public async Task<UTILISATEUR> GetById([FromQuery] int P_Id)
    {
        return await La_Base.Get_Utilisateur_By_Id(P_Id);
    }
}