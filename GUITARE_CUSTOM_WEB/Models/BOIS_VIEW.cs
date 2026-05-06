namespace GUITARE_CUSTOM_WEB.Models;

public class BOIS_VIEW
{
    public int IdBois { get; set; }
    public string NomBois { get; set; } = string.Empty;
    public string TypeBois { get; set; } = string.Empty;
    public int Quantite { get; set; }
    public bool Disponible { get; set; }

    public int IdCouleur { get; set; }
    public string NomCouleur { get; set; } = string.Empty;
    public string CodeCouleur { get; set; } = string.Empty;
}