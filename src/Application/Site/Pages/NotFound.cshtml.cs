using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Data;

namespace Site.Pages;

public class NotFoundModel : PageModel
{
    public void OnGet()
    {
        ViewData["Title"] = "Page not found | Document Number Validator";
        ViewData["Description"] = "The page you requested does not exist.";
        ViewData["Canonical"] = SiteInfo.BaseUrl + "/NotFound";
        ViewData["JsonLd"] = JsonLd.Website();
    }
}
