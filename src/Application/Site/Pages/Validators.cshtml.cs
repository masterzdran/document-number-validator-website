using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Data;

namespace Site.Pages;

public class ValidatorsModel : DocPageModel
{
    [BindProperty(SupportsGet = true)]
    public string Slug { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        var doc = DocCatalog.Find(Slug);
        if (doc is null)
            return NotFound();

        Input.DocSlug = doc.Slug;
        ViewData["Title"] = $"{doc.Name} Validator for .NET — {doc.Country} | Document Number Validator";
        ViewData["Description"] = $"Validate {doc.Name} numbers online and in your .NET code. {doc.Summary}";
        ViewData["Canonical"] = $"{SiteInfo.BaseUrl}/validators/{doc.Slug}";
        ViewData["JsonLd"] = JsonLd.Landing(doc, generatorPage: false, $"{SiteInfo.BaseUrl}/validators/{doc.Slug}");
        return Page();
    }
}
