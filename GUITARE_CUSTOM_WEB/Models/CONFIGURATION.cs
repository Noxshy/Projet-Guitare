namespace GUITARE_CUSTOM_WEB.Models;

public class CONFIGURATION
{
    public int IdConfiguration { get; set; }
    public string NomConfiguration { get; set; } = string.Empty;
    public DateTime DateCreation { get; set; }
    public DateTime? DateModification { get; set; }
    public int IdUtilisateur { get; set; }
    public int IdVibrato { get; set; }
}