using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Data;

namespace Site.Pages;

public class PrivacyModel : PageModel
{
    public void OnGet()
    {
        ViewData["Title"] = "Privacy Policy | Document Number Validator";
        ViewData["Description"] = "How the Document Number Validator website handles data: Google Analytics usage, cookies, and demo inputs never stored.";
        ViewData["Canonical"] = SiteInfo.BaseUrl + "/privacy";
        ViewData["JsonLd"] = JsonLd.Website();
    }
}
