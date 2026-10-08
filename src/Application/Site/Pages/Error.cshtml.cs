using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Data;

namespace Site.Pages;

public class ErrorModel : PageModel
{
    public void OnGet()
    {
        ViewData["Title"] = "Something went wrong | Document Number Validator";
        ViewData["Description"] = "An unexpected error occurred while processing the request.";
        ViewData["Canonical"] = SiteInfo.BaseUrl + "/Error";
        ViewData["JsonLd"] = JsonLd.Website();
    }
}
