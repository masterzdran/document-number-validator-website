using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Data;

namespace Site.Pages;

public class IndexModel : DocPageModel
{
    public IActionResult OnGet()
    {
        ViewData["Title"] = "Document Number Validator for .NET | Validate and Generate Document Numbers";
        ViewData["Description"] = "Validate and generate document numbers using a high-performance .NET library. Support for multiple countries, extensible validation rules and production-ready APIs.";
        ViewData["Canonical"] = SiteInfo.BaseUrl + "/";
        ViewData["JsonLd"] = JsonLd.Website();
        return Page();
    }
}
