using Dapper;
using LIB_DAL_LOGIN;
using Microsoft.Data.SqlClient;

namespace LIB_LOGIN;

public class UTILISATEUR_DTO
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Email { get; set; }
    public int Role { get; set; }
}

public class BDD_LOGIN
{
    string CHAINE_CONNEXION = @"Server=tcp:srv-nh.database.windows.net,1433;
                                Initial Catalog=BDD-GUITARE-CUSTOM;
                                Persist Security Info=False;
                                User ID=USER_BDD;
                                Password=P455w0rd!;
                                MultipleActiveResultSets=False;
                                Encrypt=True;
                                TrustServerCertificate=False;
                                Connection Timeout=30";

    const string EMAIL_ADMIN = "admin@admin.fr";
    const string PASSWORD_ADMIN = "Admin";
    const int ROLE_ADMIN = 1;


    public BDD_LOGIN()
    {
        Creation_Utilisateurs_Par_Default();
    }

    private void Creation_Utilisateurs_Par_Default()
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        var adminExiste = La_Base.ExecuteScalar<int>(
            "SELECT COUNT(1) FROM Utilisateur WHERE Email = @EMAIL",
            new { EMAIL = EMAIL_ADMIN }
        );

        if (adminExiste == 1) return;

        var info = CRYPTO.Generate_Salt_Hash(PASSWORD_ADMIN);

        La_Base.Execute(
            @"INSERT INTO Utilisateur (Nom, Prenom, Email, MotDePasse, Sel, IdRoleUtilisateur)
              VALUES ('Admin', 'Admin', @EMAIL, @MDP, @SEL, @ROLE)",
            new
            {
                EMAIL = EMAIL_ADMIN,
                MDP = info.Hash,
                SEL = info.Sel,
                ROLE = ROLE_ADMIN
            }
        );
    }

    public UTILISATEUR_DTO Phase_Login(string P_Email, string P_Password)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        var Utilisateur = La_Base.QueryFirstOrDefault<UTILISATEUR_IDENTITY_DTO>(
            @"SELECT IdUtilisateur, Nom, Prenom, Email, MotDePasse, Sel, IdRoleUtilisateur
              FROM Utilisateur
              WHERE Email = @EMAIL",
            new { EMAIL = P_Email }
        );

        if (Utilisateur == null) return null;

        bool OK = CRYPTO.Atteste_Identite(
            P_Password,
            Utilisateur.Sel,
            Utilisateur.MotDePasse
        );

        if (!OK) return null;

        return new UTILISATEUR_DTO
        {
            Id = Utilisateur.IdUtilisateur,
            Email = Utilisateur.Email,
            Role = Utilisateur.IdRoleUtilisateur,
            Nom = Utilisateur.Nom,
            Prenom = Utilisateur.Prenom
        };
    }

    public UTILISATEUR_DTO Creer_Utilisateur_Complet(string P_Nom, string P_Prenom, string P_Email, string P_Password, int P_Role)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        var Existe = La_Base.ExecuteScalar<int>(
            "SELECT 1 FROM Utilisateur WHERE Email = @EMAIL",
            new { EMAIL = P_Email }
        );

        if (Existe == 1) return null;

        var Crypto = CRYPTO.Generate_Salt_Hash(P_Password);

        var Id = La_Base.ExecuteScalar<int>(
            @"INSERT INTO Utilisateur
          (Nom, Prenom, Email, MotDePasse, Sel, IdRoleUtilisateur)
          OUTPUT INSERTED.IdUtilisateur
          VALUES (@NOM, @PRENOM, @EMAIL, @MDP, @SEL, @ROLE)",
            new
            {
                NOM = P_Nom,
                PRENOM = P_Prenom,
                EMAIL = P_Email,
                MDP = Crypto.Hash,
                SEL = Crypto.Sel,
                ROLE = P_Role
            }
        );

        return new UTILISATEUR_DTO
        {
            Id = Id,
            Email = P_Email,
            Role = P_Role,
            Nom = P_Nom,
            Prenom = P_Prenom
        };
    }

    public bool Update_Email(int P_IdUtilisateur, string P_Email)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        return La_Base.Execute(
            @"UPDATE Utilisateur
          SET Email = @EMAIL
          WHERE IdUtilisateur = @ID",
            new { ID = P_IdUtilisateur, EMAIL = P_Email }
        ) == 1;
    }

    public bool Update_MotDePasse(int P_IdUtilisateur, string NouveauMotDePasse)
    {
        using SqlConnection db = new SqlConnection(CHAINE_CONNEXION);

        var Info = CRYPTO.Generate_Salt_Hash(NouveauMotDePasse);

        return db.Execute(
            @"UPDATE Utilisateur
          SET MotDePasse = @MDP,
              Sel = @SEL
          WHERE IdUtilisateur = @ID",
            new
            {
                ID = P_IdUtilisateur,
                MDP = Info.Hash,
                SEL = Info.Sel
            }
        ) == 1;
    }

    public bool Del_Utilisateur(int P_IdUtilisateur)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        int Lignes = La_Base.Execute(
            "DELETE FROM Utilisateur WHERE IdUtilisateur = @ID",
            new { ID = P_IdUtilisateur }
        );

        return Lignes > 0;
    }
}