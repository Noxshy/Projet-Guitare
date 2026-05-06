namespace GUITARE_CUSTOM_WEB.Models;

public class VIBRATO
{
    public int IdVibrato { get; set; }
    public string NomModel { get; set; } = string.Empty;
    public string Marque { get; set; } = string.Empty;
    public int Quantite { get; set; }
    public bool Disponible { get; set; }
}