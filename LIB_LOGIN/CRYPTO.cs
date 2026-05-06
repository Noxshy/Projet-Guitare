using System.Security.Cryptography;

namespace LIB_DAL_LOGIN;

static class CRYPTO
{
    public static int Iteration = 10_000;
    public static (byte[] Sel, byte[] Hash) Generate_Salt_Hash(string P_message)
    {
        byte[] Salt = new byte[64];
        using (var generateur = RandomNumberGenerator.Create())
        {
            generateur.GetBytes(Salt);
        }

        using var derivation = new Rfc2898DeriveBytes(P_message, Salt, Iteration, HashAlgorithmName.SHA512);
        byte[] Le_Hash = derivation.GetBytes(64);

        return (Salt, Le_Hash);
    }

    public static byte[] Calcule_Hash(string P_Message, byte[] P_Sel)
    {
        var Derivation = new Rfc2898DeriveBytes(P_Message, P_Sel, Iteration,
            HashAlgorithmName.SHA512);

        return Derivation.GetBytes(64);
    }

    public static bool Atteste_Identite(string P_Message, byte[] P_Sel, byte[] P_Hash)
    {
        var Hash_Calcule = Calcule_Hash(P_Message, P_Sel);

        return Hash_Calcule.SequenceEqual(P_Hash);
    }

}

//-----------------------------------------------