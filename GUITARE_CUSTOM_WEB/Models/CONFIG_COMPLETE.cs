namespace GUITARE_CUSTOM_WEB.Models;

public class CONFIG_COMPLETE
{
    public CONFIGURATION Config { get; set; } = new();
    public List<CONFIG_MICRO> Micros { get; set; } = new();
    public List<CONFIG_BOIS> Bois { get; set; } = new();
}