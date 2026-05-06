using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GUITARE_CUSTOM_WEB.Pages
{
    public class IndexModel : PageModel
    {
        public int? Role { get; set; }

        public void OnGet()
        {
            Role = HttpContext.Session.GetInt32("Role");
        }
    }
}