namespace GUITARE_CUSTOM_WEB.Models;

public class BOIS
{
    public int IdBois { get; set; }
    public string NomBois { get; set; } = string.Empty;
    public string TypeBois { get; set; } = string.Empty;
    public int Quantite { get; set; }
    public bool Disponible { get; set; }
    public int IdCouleur { get; set; }
}