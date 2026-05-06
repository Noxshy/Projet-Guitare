namespace LIB_DAL_LOGIN;

public class UTILISATEUR_IDENTITY_DTO
{
    public int IdUtilisateur { get; set; }
    public string Email { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public byte[] Sel { get; set; }
    public byte[] MotDePasse { get; set; }
    public int IdRoleUtilisateur { get; set; }
}
