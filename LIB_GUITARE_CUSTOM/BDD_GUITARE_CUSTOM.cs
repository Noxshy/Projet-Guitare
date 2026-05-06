using Dapper;
using Microsoft.Data.SqlClient;

namespace LIB_GUITARE_CUSTOM;

public class MICRO
{
    public int IdMicro { get; set; }
    public string NomModel { get; set; }
    public string Marque { get; set; }
    public int Quantite { get; set; }
    public bool Disponible { get; set; }
}

public class VIBRATO
{
    public int IdVibrato { get; set; }
    public string NomModel { get; set; }
    public string Marque { get; set; }
    public int Quantite { get; set; }
    public bool Disponible { get; set; }
}

public class BOIS
{
    public int IdBois { get; set; }
    public string NomBois { get; set; }
    public string TypeBois { get; set; }
    public int Quantite { get; set; }
    public bool Disponible { get; set; }
    public int IdCouleur { get; set; }
}

public class COULEUR
{
    public int IdCouleur { get; set; }
    public string NomCouleur { get; set; }
    public string CodeCouleur { get; set; }
}

public class UTILISATEUR
{
    public int IdUtilisateur { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Email { get; set; }
    public int IdRoleUtilisateur { get; set; }
}

public class ROLE_UTILISATEUR
{
    public int IdRoleUtilisateur { get; set; }
    public string NomRole { get; set; }
}

public class ROLE_BOIS
{
    public int IdRoleBois { get; set; }
    public string NomRole { get; set; }
}

public class CONFIG_MICRO
{
    public int IdMicro { get; set; }
    public int IdConfiguration { get; set; }
    public int Position_ { get; set; }
}

public class CONFIG_BOIS
{
    public int IdBois { get; set; }
    public int IdConfiguration { get; set; }
    public int IdRoleBois { get; set; }
}

public class CONFIGURATION
{
    public int IdConfiguration { get; set; }
    public string NomConfiguration { get; set; }
    public DateTime DateCreation { get; set; }
    public DateTime? DateModification { get; set; }
    public int IdUtilisateur { get; set; }
    public int IdVibrato { get; set; }
}

public class COMMANDE
{
    public int IdCommande { get; set; }
    public DateTime DateCommande { get; set; }
    public string StatutCommande { get; set; }
    public int IdConfiguration { get; set; }
    public int IdUtilisateur { get; set; }
}

public class SUIVI_FABRICATION
{
    public int IdSuivi { get; set; }
    public DateTime DatePhoto { get; set; }
    public string UrlPhoto { get; set; }
    public string Commentaire { get; set; }
    public int IdCommande { get; set; }
}

// Configuration complète
public class CONFIG_COMPLETE
{
    public CONFIGURATION Config { get; set; }
    public List<CONFIG_MICRO> Micros { get; set; }
    public List<CONFIG_BOIS> Bois { get; set; }
}

public class BDD_GUITARE
{
    const string CHAINE_CONNEXION = @"Server=tcp:srv-nh.database.windows.net,1433;
                                      Initial Catalog=BDD-GUITARE-CUSTOM;
                                      Persist Security Info=False;
                                      User ID=USER_BDD;
                                      Password=P455w0rd!;
                                      MultipleActiveResultSets=False;
                                      Encrypt=True;
                                      TrustServerCertificate=False;
                                      Connection Timeout=30;";

    //======================= CATALOGUE =======================//

    //======================= Méthodes UTILISATEUR =======================//
    public async Task<IEnumerable<UTILISATEUR>> Get_All_Utilisateurs()
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);
        string SQL = "SELECT IdUtilisateur, Nom, Prenom, Email, IdRoleUtilisateur FROM Utilisateur";
        return await La_Base.QueryAsync<UTILISATEUR>(SQL);
    }

    public async Task<UTILISATEUR> Get_Utilisateur_By_Id(int P_Id)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        return await La_Base.QueryFirstOrDefaultAsync<UTILISATEUR>(
            @"SELECT IdUtilisateur, Nom, Prenom, Email, IdRoleUtilisateur
          FROM Utilisateur
          WHERE IdUtilisateur = @ID",
            new { ID = P_Id });
    }

    //======================= Méthodes MICRO =======================//
    public async Task<IEnumerable<MICRO>> Get_All_Micros()
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);
        string SQL = "SELECT * FROM Micro";
        return await La_Base.QueryAsync<MICRO>(SQL);
    }

    public async Task<MICRO> Add_Micro(string P_NomModel, string P_Marque, int P_Quantite)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"INSERT INTO Micro (NomModel, Marque, Quantite, Disponible)
                           OUTPUT INSERTED.*
                           VALUES (@NOM, @MARQUE, @QTE, 1)";

        return await La_Base.QueryFirstOrDefaultAsync<MICRO>(SQL,
            new { NOM = P_NomModel, MARQUE = P_Marque, QTE = P_Quantite });
    }

    public async Task<MICRO> Update_Micro(int P_Id, string P_NomModel, string P_Marque, int P_Quantite, bool P_Dispo)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"UPDATE Micro
                           SET NomModel = @NOM,
                               Marque = @MARQUE,
                               Quantite = @QTE,
                               Disponible = @DISPO
                           OUTPUT INSERTED.*
                           WHERE IdMicro = @ID";

        return await La_Base.QueryFirstOrDefaultAsync<MICRO>(SQL,
            new { ID = P_Id, NOM = P_NomModel, MARQUE = P_Marque, QTE = P_Quantite, DISPO = P_Dispo });
    }

    public async Task<bool> Del_Micro(int P_Id)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);
        string SQL = "DELETE Micro WHERE IdMicro = @ID";
        int Nombre = await La_Base.ExecuteAsync(SQL, new { ID = P_Id });
        return Nombre > 0;
    }

    //======================= Méthodes VIBRATO =======================//
    public async Task<IEnumerable<VIBRATO>> Get_All_Vibratos()
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);
        return await La_Base.QueryAsync<VIBRATO>("SELECT * FROM Vibrato");
    }

    public async Task<VIBRATO> Add_Vibrato(string P_NomModel, string P_Marque, int P_Quantite)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"INSERT INTO Vibrato (NomModel, Marque, Quantite, Disponible)
                   OUTPUT INSERTED.*
                   VALUES (@NOM, @MARQUE, @QTE, 1)";

        return await La_Base.QueryFirstOrDefaultAsync<VIBRATO>(SQL,
            new { NOM = P_NomModel, MARQUE = P_Marque, QTE = P_Quantite });
    }

    public async Task<VIBRATO> Update_Vibrato(int P_Id, string P_NomModel, string P_Marque, int P_Quantite, bool P_Dispo)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"UPDATE Vibrato
                   SET NomModel=@NOM, Marque=@MARQUE, Quantite=@QTE, Disponible=@DISPO
                   OUTPUT INSERTED.*
                   WHERE IdVibrato=@ID";

        return await La_Base.QueryFirstOrDefaultAsync<VIBRATO>(SQL,
            new { ID = P_Id, NOM = P_NomModel, MARQUE = P_Marque, QTE = P_Quantite, DISPO = P_Dispo });
    }

    public async Task<bool> Del_Vibrato(int P_Id)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);
        int N = await La_Base.ExecuteAsync("DELETE Vibrato WHERE IdVibrato=@ID", new { ID = P_Id });
        return N > 0;
    }

    //======================= Méthodes BOIS =======================//
    public async Task<IEnumerable<BOIS>> Get_All_Bois()
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);
        return await La_Base.QueryAsync<BOIS>("SELECT * FROM Bois");
    }

    public async Task<BOIS> Add_Bois(string P_Nom, string P_Type, int P_Qte, int P_IdCouleur)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"INSERT INTO Bois (NomBois, TypeBois, Quantite, Disponible, IdCouleur)
                   OUTPUT INSERTED.*
                   VALUES (@NOM, @TYPE, @QTE, 1, @COULEUR)";

        return await La_Base.QueryFirstOrDefaultAsync<BOIS>(SQL,
            new { NOM = P_Nom, TYPE = P_Type, QTE = P_Qte, COULEUR = P_IdCouleur });
    }

    public async Task<BOIS> Update_Bois(int P_Id, string P_Nom, string P_Type, int P_Qte, bool P_Dispo, int P_IdCouleur)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"UPDATE Bois
                   SET NomBois=@NOM,
                       TypeBois=@TYPE,
                       Quantite=@QTE,
                       Disponible=@DISPO,
                       IdCouleur=@COULEUR
                   OUTPUT INSERTED.*
                   WHERE IdBois=@ID";

        return await La_Base.QueryFirstOrDefaultAsync<BOIS>(SQL,
            new { ID = P_Id, NOM = P_Nom, TYPE = P_Type, QTE = P_Qte, DISPO = P_Dispo, COULEUR = P_IdCouleur });
    }

    public async Task<bool> Del_Bois(int P_Id)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);
        int N = await La_Base.ExecuteAsync("DELETE Bois WHERE IdBois=@ID", new { ID = P_Id });
        return N > 0;
    }

    //======================= Méthodes COULEUR =======================//
    public async Task<IEnumerable<COULEUR>> Get_All_Couleurs()
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);
        return await La_Base.QueryAsync<COULEUR>("SELECT * FROM Couleur");
    }

    public async Task<COULEUR> Add_Couleur(string P_Nom, string P_Code)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"INSERT INTO Couleur (NomCouleur, CodeCouleur)
                   OUTPUT INSERTED.*
                   VALUES (@NOM, @CODE)";

        return await La_Base.QueryFirstOrDefaultAsync<COULEUR>(SQL,
            new { NOM = P_Nom, CODE = P_Code });
    }

    public async Task<COULEUR> Update_Couleur(int P_Id, string P_Nom, string P_Code)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"UPDATE Couleur
                   SET NomCouleur = @NOM,
                       CodeCouleur = @CODE
                   OUTPUT INSERTED.*
                   WHERE IdCouleur = @ID";

        return await La_Base.QueryFirstOrDefaultAsync<COULEUR>(SQL,
            new { ID = P_Id, NOM = P_Nom, CODE = P_Code });
    }

    public async Task<bool> Del_Couleur(int P_Id)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);
        return await La_Base.ExecuteAsync("DELETE Couleur WHERE IdCouleur=@ID",
            new { ID = P_Id }) > 0;
    }

    //======================= Méthodes ROLE_UTILISATEUR =======================//
    public async Task<IEnumerable<ROLE_UTILISATEUR>> Get_All_RoleUtilisateurs()
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);
        return await La_Base.QueryAsync<ROLE_UTILISATEUR>(
            "SELECT * FROM RoleUtilisateur");
    }

    public async Task<ROLE_UTILISATEUR> Add_RoleUtilisateur(string P_Nom)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"INSERT INTO RoleUtilisateur (NomRole)
                   OUTPUT INSERTED.*
                   VALUES (@NOM)";

        return await La_Base.QueryFirstOrDefaultAsync<ROLE_UTILISATEUR>(SQL,
            new { NOM = P_Nom });
    }

    public async Task<ROLE_UTILISATEUR> Update_RoleUtilisateur(int P_Id, string P_Nom)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"UPDATE RoleUtilisateur
                   SET NomRole = @NOM
                   OUTPUT INSERTED.*
                   WHERE IdRoleUtilisateur = @ID";

        return await La_Base.QueryFirstOrDefaultAsync<ROLE_UTILISATEUR>(SQL,
            new { ID = P_Id, NOM = P_Nom });
    }

    public async Task<bool> Del_RoleUtilisateur(int P_Id)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        int N = await La_Base.ExecuteAsync(
            "DELETE RoleUtilisateur WHERE IdRoleUtilisateur=@ID",
            new { ID = P_Id });

        return N > 0;
    }

    //======================= Méthodes ROLE_BOIS =======================//
    public async Task<IEnumerable<ROLE_BOIS>> Get_All_RoleBois()
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);
        return await La_Base.QueryAsync<ROLE_BOIS>(
            "SELECT * FROM RoleBois");
    }

    public async Task<ROLE_BOIS> Add_RoleBois(string P_Nom)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"INSERT INTO RoleBois (NomRole)
                   OUTPUT INSERTED.*
                   VALUES (@NOM)";

        return await La_Base.QueryFirstOrDefaultAsync<ROLE_BOIS>(SQL,
            new { NOM = P_Nom });
    }

    public async Task<ROLE_BOIS> Update_RoleBois(int P_Id, string P_Nom)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"UPDATE RoleBois
                   SET NomRole = @NOM
                   OUTPUT INSERTED.*
                   WHERE IdRoleBois = @ID";

        return await La_Base.QueryFirstOrDefaultAsync<ROLE_BOIS>(SQL,
            new { ID = P_Id, NOM = P_Nom });
    }

    public async Task<bool> Del_RoleBois(int P_Id)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        int N = await La_Base.ExecuteAsync(
            "DELETE RoleBois WHERE IdRoleBois=@ID",
            new { ID = P_Id });

        return N > 0;
    }

    //======================= TABLE D'ASSOCIATIONS =======================//

    //======================= Méthodes CONFIG_MICRO =======================//
    public async Task<IEnumerable<CONFIG_MICRO>> Get_Micros_By_Configuration(int P_IdConfiguration)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"SELECT * FROM Config_Micro
                   WHERE IdConfiguration = @CONF";

        return await La_Base.QueryAsync<CONFIG_MICRO>(SQL,
            new { CONF = P_IdConfiguration });
    }

    public async Task<bool> Add_Config_Micro(int P_IdMicro, int P_IdConfiguration, int P_Position)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"INSERT INTO Config_Micro (IdMicro, IdConfiguration, Position_)
                   VALUES (@MICRO, @CONF, @POS)";

        int N = await La_Base.ExecuteAsync(SQL,
            new { MICRO = P_IdMicro, CONF = P_IdConfiguration, POS = P_Position });

        return N == 1;
    }

    public async Task<bool> Update_Config_Micro_Position(int P_IdMicro, int P_IdConfiguration, int P_Position)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"UPDATE Config_Micro
                   SET Position_ = @POS
                   WHERE IdMicro = @MICRO AND IdConfiguration = @CONF";

        int N = await La_Base.ExecuteAsync(SQL,
            new { MICRO = P_IdMicro, CONF = P_IdConfiguration, POS = P_Position });

        return N == 1;
    }

    public async Task<bool> Del_Config_Micro(int P_IdMicro, int P_IdConfiguration)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"DELETE Config_Micro
                   WHERE IdMicro = @MICRO AND IdConfiguration = @CONF";

        int N = await La_Base.ExecuteAsync(SQL,
            new { MICRO = P_IdMicro, CONF = P_IdConfiguration });

        return N == 1;
    }

    //======================= Méthodes CONFIG_BOIS =======================//
    public async Task<IEnumerable<CONFIG_BOIS>> Get_Bois_By_Configuration(int P_IdConfiguration)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"SELECT * FROM Config_Bois
                   WHERE IdConfiguration = @CONF";

        return await La_Base.QueryAsync<CONFIG_BOIS>(SQL,
            new { CONF = P_IdConfiguration });
    }

    public async Task<bool> Add_Config_Bois(int P_IdBois, int P_IdConfiguration, int P_IdRoleBois)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"INSERT INTO Config_Bois (IdBois, IdConfiguration, IdRoleBois)
                   VALUES (@BOIS, @CONF, @ROLE)";

        int N = await La_Base.ExecuteAsync(SQL,
            new { BOIS = P_IdBois, CONF = P_IdConfiguration, ROLE = P_IdRoleBois });

        return N == 1;
    }

    public async Task<bool> Update_Config_Bois_Role(int P_IdBois, int P_IdConfiguration, int P_AncienRole, int P_NouveauRole)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"UPDATE Config_Bois
                   SET IdRoleBois = @NEW
                   WHERE IdBois = @BOIS
                   AND IdConfiguration = @CONF
                   AND IdRoleBois = @OLD";

        return await La_Base.ExecuteAsync(SQL, new
        { BOIS = P_IdBois, CONF = P_IdConfiguration, OLD = P_AncienRole, NEW = P_NouveauRole}) == 1;
    }

    public async Task<bool> Del_Config_Bois(int P_IdBois, int P_IdConfiguration, int P_IdRoleBois)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"DELETE Config_Bois
                   WHERE IdBois = @BOIS
                   AND IdConfiguration = @CONF
                   AND IdRoleBois = @ROLE";

        int N = await La_Base.ExecuteAsync(SQL,
            new { BOIS = P_IdBois, CONF = P_IdConfiguration, ROLE = P_IdRoleBois });

        return N == 1;
    }

    //======================= Méthodes SUIVI_FABRICATION =======================//
    public async Task<IEnumerable<dynamic>> Get_All_Suivi()
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"
                    SELECT s.IdSuivi,
                           s.DatePhoto,
                           s.UrlPhoto,
                           s.Commentaire,
                           c.IdCommande,
                           c.StatutCommande,
                           cl.Email
                    FROM SuiviFabrication s
                    JOIN Commande c ON c.IdCommande = s.IdCommande
                    JOIN Utilisateur cl ON cl.IdUtilisateur = c.IdUtilisateur
                    ORDER BY s.DatePhoto DESC";

        return await La_Base.QueryAsync(SQL);
    }

    public async Task<IEnumerable<dynamic>> Get_Suivi_By_Utilisateur(int P_IdUtilisateur)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"
        SELECT s.IdSuivi,
               s.DatePhoto,
               s.UrlPhoto,
               s.Commentaire,
               c.IdCommande,
               c.StatutCommande
        FROM SuiviFabrication s
        JOIN Commande c ON c.IdCommande = s.IdCommande
        WHERE c.IdUtilisateur = @UTILISATEUR
        ORDER BY s.DatePhoto DESC";

        return await La_Base.QueryAsync(SQL, new { UTILISATEUR = P_IdUtilisateur });
    }

    public async Task<IEnumerable<SUIVI_FABRICATION>> Get_Suivi_By_Commande(int P_IdCommande)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"SELECT * FROM SuiviFabrication
                   WHERE IdCommande = @CMD
                   ORDER BY DatePhoto ASC";

        return await La_Base.QueryAsync<SUIVI_FABRICATION>(SQL,
            new { CMD = P_IdCommande });
    }

    public async Task<SUIVI_FABRICATION> Add_Suivi(int P_IdCommande, string P_Url, string P_Commentaire)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"INSERT INTO SuiviFabrication
                   (DatePhoto, UrlPhoto, Commentaire, IdCommande)
                   OUTPUT INSERTED.*
                   VALUES (SYSDATETIME(), @URL, @COM, @CMD)";

        return await La_Base.QueryFirstOrDefaultAsync<SUIVI_FABRICATION>(SQL,
            new { URL = P_Url, COM = P_Commentaire, CMD = P_IdCommande });
    }

    public async Task<SUIVI_FABRICATION> Update_Suivi(int P_IdSuivi, string P_Url, string P_Commentaire)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"UPDATE SuiviFabrication
                   SET UrlPhoto = @URL,
                   Commentaire = @COM
                   OUTPUT INSERTED.*
                   WHERE IdSuivi = @ID";

        return await La_Base.QueryFirstOrDefaultAsync<SUIVI_FABRICATION>(SQL,
            new { ID = P_IdSuivi, URL = P_Url, COM = P_Commentaire });
    }

    public async Task<bool> Del_Suivi(int P_IdSuivi)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        int N = await La_Base.ExecuteAsync(
            "DELETE SuiviFabrication WHERE IdSuivi = @ID",
            new { ID = P_IdSuivi });

        return N > 0;
    }

    //======================= Méthodes COMMANDE =======================//
    public async Task<IEnumerable<COMMANDE>> Get_All_Commandes()
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"SELECT *
                   FROM Commande
                   ORDER BY DateCommande DESC";

        return await La_Base.QueryAsync<COMMANDE>(SQL);
    }

    public async Task<IEnumerable<COMMANDE>> Get_Commandes_By_Utilisateur(int P_IdUtilisateur)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"SELECT * FROM Commande
                   WHERE IdUtilisateur = @UTILISATEUR";

        return await La_Base.QueryAsync<COMMANDE>(SQL,
            new { UTILISATEUR = P_IdUtilisateur });
    }

    public async Task<COMMANDE> Get_Commande_By_Id(int P_Id)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        return await La_Base.QueryFirstOrDefaultAsync<COMMANDE>(
            "SELECT * FROM Commande WHERE IdCommande = @ID",
            new { ID = P_Id });
    }

    public async Task<COMMANDE> Add_Commande(int P_IdConfiguration, int P_IdUtilisateur)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);
        await La_Base.OpenAsync();

        using var Transaction = La_Base.BeginTransaction();

        try
        {
            // Récupérer la config complète
            var Config = await Get_Config_Complete(P_IdConfiguration);

            if (Config == null)
                throw new Exception("Configuration introuvable");

            // Consommer les micros
            foreach (var Micro in Config.Micros)
            {
                string SQL = @"
                    UPDATE Micro
                    SET Quantite = Quantite - 1,
                        Disponible = CASE WHEN Quantite - 1 > 0 THEN 1 ELSE 0 END
                    WHERE IdMicro = @ID AND Quantite > 0";

                int Rows = await La_Base.ExecuteAsync(SQL, new { ID = Micro.IdMicro }, Transaction);

                if (Rows == 0)
                    throw new Exception("Stock micro insuffisant");
            }

            // Consommer les bois
            foreach (var Bois in Config.Bois)
            {
                string SQL = @"
                    UPDATE Bois
                    SET Quantite = Quantite - 1,
                        Disponible = CASE WHEN Quantite - 1 > 0 THEN 1 ELSE 0 END
                    WHERE IdBois = @ID AND Quantite > 0";

                int Rows = await La_Base.ExecuteAsync(SQL, new { ID = Bois.IdBois }, Transaction);

                if (Rows == 0)
                    throw new Exception("Stock bois insuffisant");
            }

            // Consommer vibrato
            {
                string SQL = @"
                    UPDATE Vibrato
                    SET Quantite = Quantite - 1,
                        Disponible = CASE WHEN Quantite - 1 > 0 THEN 1 ELSE 0 END
                    WHERE IdVibrato = @ID AND Quantite > 0";

                int Rows = await La_Base.ExecuteAsync(SQL, new { ID = Config.Config.IdVibrato }, Transaction);

                if (Rows == 0)
                    throw new Exception("Stock vibrato insuffisant");
            }

            // Créer la commande
            string insertCommande = @"
                INSERT INTO Commande
                (DateCommande, StatutCommande, IdConfiguration, IdUtilisateur)
                OUTPUT INSERTED.*
                VALUES (SYSDATETIME(), 'EN_COURS', @CONF, @UTILISATEUR)";

            var Commande = await La_Base.QueryFirstOrDefaultAsync<COMMANDE>(
                insertCommande,
                new { CONF = P_IdConfiguration, UTILISATEUR = P_IdUtilisateur },
                Transaction);

            Transaction.Commit();

            return Commande;
        }
        catch
        {
            Transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> Annule_Commande(int P_IdCommande, int? P_IdUtilisateur = null)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);
        await La_Base.OpenAsync();

        using var Transaction = La_Base.BeginTransaction();

        try
        {
            var Commande = await La_Base.QueryFirstOrDefaultAsync<COMMANDE>(
                "SELECT * FROM Commande WHERE IdCommande = @ID",
                new { ID = P_IdCommande }, Transaction);

            if (Commande == null || Commande.StatutCommande != "EN_COURS")
                return false;

            if (P_IdUtilisateur != null && Commande.IdUtilisateur != P_IdUtilisateur)
                return false;

            var Config = await Get_Config_Complete(Commande.IdConfiguration);

            // RESTOCK micros
            foreach (var Micro in Config.Micros)
            {
                await La_Base.ExecuteAsync(
                    @"UPDATE Micro SET Quantite = Quantite + 1, Disponible = 1 WHERE IdMicro = @ID",
                    new { ID = Micro.IdMicro }, Transaction);
            }

            // RESTOCK bois
            foreach (var Bois in Config.Bois)
            {
                await La_Base.ExecuteAsync(
                    @"UPDATE Bois SET Quantite = Quantite + 1, Disponible = 1 WHERE IdBois = @ID",
                    new { ID = Bois.IdBois }, Transaction);
            }

            // RESTOCK vibrato
            await La_Base.ExecuteAsync(
                @"UPDATE Vibrato SET Quantite = Quantite + 1, Disponible = 1 WHERE IdVibrato = @ID",
                new { ID = Config.Config.IdVibrato }, Transaction);

            // annuler commande
            int Rows = await La_Base.ExecuteAsync(
                @"UPDATE Commande
              SET StatutCommande = 'ANNULEE'
              WHERE IdCommande = @ID",
                new { ID = P_IdCommande }, Transaction);

            Transaction.Commit();

            return Rows == 1;
        }
        catch
        {
            Transaction.Rollback();
            throw;
        }
    }

    public async Task<COMMANDE> Update_Commande_Statut(int P_Id, string P_Statut)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"UPDATE Commande
                   SET StatutCommande = @STATUT
                   OUTPUT INSERTED.*
                   WHERE IdCommande = @ID";

        return await La_Base.QueryFirstOrDefaultAsync<COMMANDE>(SQL,
            new { ID = P_Id, STATUT = P_Statut });
    }

    public async Task<COMMANDE> Update_Commande_Admin(int P_IdCommande, string P_StatutCommande, int? P_IdConfiguration = null)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"UPDATE Commande
                   SET StatutCommande = @STATUT";

        if (P_IdConfiguration.HasValue)
        {
            SQL += @", IdConfiguration = @CONF";
        }

        SQL += @" WHERE IdCommande = @ID";

        return await La_Base.QueryFirstOrDefaultAsync<COMMANDE>(SQL,
            new { ID = P_IdCommande, STATUT = P_StatutCommande, CONF = P_IdConfiguration });
    }


    public async Task<bool> Del_Commande(int P_Id)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        int N = await La_Base.ExecuteAsync(
            "DELETE Commande WHERE IdCommande = @ID",
            new { ID = P_Id });

        return N > 0;
    }

    // Consommation
    //public async Task<bool> Consommer_Micro(int P_IdMicro)
    //{
    //    using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

    //    string SQL = @"
    //        UPDATE Micro
    //        SET Quantite = Quantite - 1,
    //            Disponible = CASE WHEN Quantite - 1 > 0 THEN 1 ELSE 0 END
    //        WHERE IdMicro = @ID AND Quantite > 0";

    //    int Rows = await La_Base.ExecuteAsync(SQL, new { ID = P_IdMicro });

    //    return Rows == 1;
    //}

    //public async Task<bool> Ajouter_Stock_Micro(int P_IdMicro, int P_Quantite)
    //{
    //    using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

    //    string SQL = @"
    //        UPDATE Micro
    //        SET Quantite = Quantite + @QTE,
    //            Disponible = 1
    //        WHERE IdMicro = @ID";

    //    int Rows = await La_Base.ExecuteAsync(SQL, new { ID = P_IdMicro, QTE = P_Quantite });

    //    return Rows == 1;
    //}

    //public async Task<bool> Consommer_Bois(int P_IdBois)
    //{
    //    using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

    //    string SQL = @"
    //        UPDATE Bois
    //        SET Quantite = Quantite - 1,
    //            Disponible = CASE WHEN Quantite - 1 > 0 THEN 1 ELSE 0 END
    //        WHERE IdBois = @ID AND Quantite > 0";

    //    int Rows = await La_Base.ExecuteAsync(SQL, new { ID = P_IdBois });

    //    return Rows == 1;
    //}

    //public async Task<bool> Ajouter_Stock_Bois(int P_IdBois, int P_Quantite)
    //{
    //    using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

    //    string SQL = @"
    //        UPDATE Bois
    //        SET Quantite = Quantite + @QTE,
    //            Disponible = 1
    //        WHERE IdBois = @ID";

    //    int Rows = await La_Base.ExecuteAsync(SQL, new { ID = P_IdBois, QTE = P_Quantite });

    //    return Rows == 1;
    //}

    //public async Task<bool> Consommer_Vibrato(int P_IdVibrato)
    //{
    //    using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

    //    string SQL = @"
    //        UPDATE Vibrato
    //        SET Quantite = Quantite - 1,
    //            Disponible = CASE WHEN Quantite - 1 > 0 THEN 1 ELSE 0 END
    //        WHERE IdVibrato = @ID AND Quantite > 0";

    //    int Rows = await La_Base.ExecuteAsync(SQL, new { ID = P_IdVibrato });

    //    return Rows == 1;
    //}

    //public async Task<bool> Ajouter_Stock_Vibrato(int P_IdVibrato, int P_Quantite)
    //{
    //    using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

    //    string SQL = @"
    //        UPDATE Vibrato
    //        SET Quantite = Quantite + @QTE,
    //            Disponible = 1
    //        WHERE IdVibrato = @ID";

    //    int Rows = await La_Base.ExecuteAsync(SQL, new { ID = P_IdVibrato, QTE = P_Quantite });

    //    return Rows == 1;
    //}

    //======================= Méthodes CONFIGURATION =======================//
    public async Task<CONFIGURATION> Get_Configuration_By_Id(int P_Id)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        string SQL = @"SELECT * FROM Configuration
                   WHERE IdConfiguration = @ID";

        return await La_Base.QueryFirstOrDefaultAsync<CONFIGURATION>(SQL,
            new { ID = P_Id });
    }

    public async Task<bool> Del_Configuration(int P_Id)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        int N = await La_Base.ExecuteAsync(
            "DELETE Configuration WHERE IdConfiguration = @ID",
            new { ID = P_Id });

        return N > 0;
    }

    //======================= Méthodes CONFIGURATION COMPLETE (Micro et Bois) =======================//
    public async Task<IEnumerable<CONFIG_COMPLETE>> Get_All_Config_Complete()
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        var Configs = await La_Base.QueryAsync<CONFIGURATION>("SELECT * FROM Configuration");

        var Result = new List<CONFIG_COMPLETE>();

        foreach (var config in Configs)
        {
            Result.Add(await Get_Config_Complete(config.IdConfiguration));
        }

        return Result;
    }

    public async Task<IEnumerable<CONFIG_COMPLETE>> Get_Config_Complete_By_User(int P_IdUtilisateur)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        var Configs = await La_Base.QueryAsync<CONFIGURATION>(
            "SELECT * FROM Configuration WHERE IdUtilisateur = @ID",
            new { ID = P_IdUtilisateur });

        var Result = new List<CONFIG_COMPLETE>();

        foreach (var Config in Configs)
        {
            Result.Add(await Get_Config_Complete(Config.IdConfiguration));
        }

        return Result;
    }

    public async Task<CONFIG_COMPLETE> Get_Config_Complete(int P_IdConfig)
    {
        var Config = await Get_Configuration_By_Id(P_IdConfig);
        var Micros = (await Get_Micros_By_Configuration(P_IdConfig)).ToList();
        var Bois = (await Get_Bois_By_Configuration(P_IdConfig)).ToList();

        return new CONFIG_COMPLETE
        {
            Config = Config,
            Micros = Micros,
            Bois = Bois
        };
    }

    public async Task<CONFIG_COMPLETE> Add_Config_Complete(CONFIG_COMPLETE P_Data)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);
        await La_Base.OpenAsync();

        using var transaction = La_Base.BeginTransaction();

        try
        {
            // ================= VALIDATION =================

            var Roles = P_Data.Bois.Select(b => b.IdRoleBois).ToList();

            if (Roles.Count != Roles.Distinct().Count())
                throw new Exception("Un rôle de bois est en double.");

            var RolesBDD = (await Get_All_RoleBois())
                            .Select(r => r.IdRoleBois)
                            .ToList();

            foreach (var role in RolesBDD)
            {
                if (!Roles.Contains(role))
                    throw new Exception("Tous les rôles de bois doivent être définis.");
            }

            // Vérifier les positions des micros
            var positions = P_Data.Micros.Select(m => m.Position_).ToList();

            if (positions.Any(p => p <= 0))
                throw new Exception("Les positions des micros doivent être supérieures à 0.");

            if (positions.Count != positions.Distinct().Count())
                throw new Exception("Deux micros ne peuvent pas avoir la même position.");

            // ================= INSERT CONFIG =================

            var Config = await La_Base.QueryFirstOrDefaultAsync<CONFIGURATION>(
                @"INSERT INTO Configuration (NomConfiguration, DateCreation, IdUtilisateur, IdVibrato)
              OUTPUT INSERTED.*
              VALUES (@Nom, SYSDATETIME(), @Utilisateur, @Vibrato)",
                new
                {
                    Nom = P_Data.Config.NomConfiguration,
                    Utilisateur = P_Data.Config.IdUtilisateur,
                    Vibrato = P_Data.Config.IdVibrato
                }, transaction);

            // ================= INSERT MICROS =================

            foreach (var Micro in P_Data.Micros)
            {
                await La_Base.ExecuteAsync(
                    @"INSERT INTO Config_Micro (IdMicro, IdConfiguration, Position_)
                  VALUES (@Micro, @Config, @Pos)",
                    new
                    {
                        Micro = Micro.IdMicro,
                        Config = Config.IdConfiguration,
                        Pos = Micro.Position_
                    },
                    transaction);
            }

            // ================= INSERT BOIS =================

            foreach (var Bois in P_Data.Bois)
            {
                await La_Base.ExecuteAsync(
                    @"INSERT INTO Config_Bois (IdBois, IdConfiguration, IdRoleBois)
                  VALUES (@Bois, @Config, @Role)",
                    new
                    {
                        Bois = Bois.IdBois,
                        Config = Config.IdConfiguration,
                        Role = Bois.IdRoleBois
                    },
                    transaction);
            }

            transaction.Commit();

            return await Get_Config_Complete(Config.IdConfiguration);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<CONFIG_COMPLETE> Update_Config_Complete(CONFIG_COMPLETE P_Data)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);
        await La_Base.OpenAsync();

        using var Transaction = La_Base.BeginTransaction();

        try
        {
            // ================= VALIDATION =================

            var roles = P_Data.Bois.Select(b => b.IdRoleBois).ToList();

            if (roles.Count != roles.Distinct().Count())
                throw new Exception("Un rôle de bois est en double.");

            var rolesBDD = (await Get_All_RoleBois())
                .Select(r => r.IdRoleBois)
                .ToList();

            foreach (var role in rolesBDD)
            {
                if (!roles.Contains(role))
                    throw new Exception("Tous les rôles de bois doivent être définis.");
            }

            var positions = P_Data.Micros.Select(m => m.Position_).ToList();

            if (positions.Any(p => p < 1 || p > 3))
                throw new Exception("Les positions des micros doivent être comprises entre 1 et 3.");

            if (positions.Count != positions.Distinct().Count())
                throw new Exception("Deux micros ne peuvent pas avoir la même position.");

            // ================= UPDATE CONFIG =================

            await La_Base.ExecuteAsync(
                @"UPDATE Configuration
               SET NomConfiguration = @Nom,
               IdVibrato = @Vibrato,
               DateModification = SYSDATETIME()
               WHERE IdConfiguration = @ID",
                new
                {
                    ID = P_Data.Config.IdConfiguration,
                    Nom = P_Data.Config.NomConfiguration,
                    Vibrato = P_Data.Config.IdVibrato
                },
                Transaction);

            // DELETE anciens liens
            await La_Base.ExecuteAsync(
                "DELETE FROM Config_Micro WHERE IdConfiguration = @ID",
                new { ID = P_Data.Config.IdConfiguration },
                Transaction);

            await La_Base.ExecuteAsync(
                "DELETE FROM Config_Bois WHERE IdConfiguration = @ID",
                new { ID = P_Data.Config.IdConfiguration },
                Transaction);

            // REINSERT micros
            foreach (var Micro in P_Data.Micros)
            {
                await La_Base.ExecuteAsync(
                    @"INSERT INTO Config_Micro (IdMicro, IdConfiguration, Position_)
                   VALUES (@Micro, @Config, @Pos)",
                    new
                    {
                        Micro = Micro.IdMicro,
                        Config = P_Data.Config.IdConfiguration,
                        Pos = Micro.Position_
                    },
                    Transaction);
            }

            // REINSERT bois
            foreach (var Bois in P_Data.Bois)
            {
                await La_Base.ExecuteAsync(
                    @"INSERT INTO Config_Bois (IdBois, IdConfiguration, IdRoleBois)
                   VALUES (@Bois, @Config, @Role)",
                    new
                    {
                        Bois = Bois.IdBois,
                        Config = P_Data.Config.IdConfiguration,
                        Role = Bois.IdRoleBois
                    },
                    Transaction);
            }

            Transaction.Commit();

            return await Get_Config_Complete(P_Data.Config.IdConfiguration);
        }
        catch
        {
            Transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> Del_Config_Complete(int P_IdConfig)
    {
        using SqlConnection La_Base = new SqlConnection(CHAINE_CONNEXION);

        await La_Base.OpenAsync();
        using var Transaction = La_Base.BeginTransaction();

        try
        {
            await La_Base.ExecuteAsync(
                "DELETE FROM Config_Micro WHERE IdConfiguration = @ID",
                new { ID = P_IdConfig }, Transaction);

            await La_Base.ExecuteAsync(
                "DELETE FROM Config_Bois WHERE IdConfiguration = @ID",
                new { ID = P_IdConfig }, Transaction);

            int N = await La_Base.ExecuteAsync(
                "DELETE FROM Configuration WHERE IdConfiguration = @ID",
                new { ID = P_IdConfig }, Transaction);

            Transaction.Commit();

            return N > 0;
        }
        catch
        {
            Transaction.Rollback();
            throw;
        }
    }
}
