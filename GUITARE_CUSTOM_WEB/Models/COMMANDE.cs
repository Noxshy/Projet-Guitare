namespace GUITARE_CUSTOM_WEB.Models
{
    public class COMMANDE
    {
        public int IdCommande { get; set; }
        public DateTime DateCommande { get; set; }
        public string StatutCommande { get; set; }
        public int IdConfiguration { get; set; }
        public int IdUtilisateur { get; set; }
    }
}
