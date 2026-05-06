using Microsoft.AspNetCore.Mvc;
using BDD_LOGIN;

namespace WEBAPI_CTRL_LOGIN
{
    [ApiController]
    [Route("/Auth")]
    public class CTRL_Auth : ControllerBase
    {
        private BDD_LOGIN_CLASS La_Base;

        public CTRL_Auth(BDD_LOGIN_CLASS P_Base)
        {
            La_Base = P_Base;
        }

        // ----------------------------------------------
        public class LoginQuery
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("Login")]
        public CLIENT_DTO Login([FromBody] LoginQuery P_Login)
        {
            return La_Base.Phase_Login(
                P_Login.Email,
                P_Login.Password
            );
        }

        // ----------------------------------------------
        public class AddCompteQuery
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public int Role { get; set; }
        }

        [HttpPost("Add")]
        public CLIENT_DTO Add([FromBody] AddCompteQuery P_Ajout)
        {
            return La_Base.Ajouter_Compte_Client(
                P_Ajout.Email,
                P_Ajout.Password,
                P_Ajout.Role
            );
        }

        // ----------------------------------------------
        public class DeleteCompteQuery
        {
            public int IdClient { get; set; }
        }

        [HttpDelete("Delete")]
        public bool Delete([FromBody] DeleteCompteQuery P_Delete)
        {
            return La_Base.Del_Client(
                P_Delete.IdClient
            );
        }
    }
}
