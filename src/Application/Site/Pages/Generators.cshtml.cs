using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Data;

namespace Site.Pages;

public class GeneratorsModel : DocPageModel
{
    [BindProperty(SupportsGet = true)]
    public string Slug { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        var doc = DocCatalog.Find(Slug);
        if (doc is null || !doc.CanGenerate)
            return NotFound();

        Input.DocSlug = doc.Slug;
        ViewData["Title"] = $"{doc.Name} Generator for .NET — {doc.Country} | Document Number Validator";
        ViewData["Description"] = $"Generate valid {doc.Name} sample numbers in .NET for tests and demos. {doc.Summary}";
        ViewData["Canonical"] = $"{SiteInfo.BaseUrl}/generators/{doc.Slug}";
        ViewData["JsonLd"] = JsonLd.Landing(doc, generatorPage: true, $"{SiteInfo.BaseUrl}/generators/{doc.Slug}");
        return Page();
    }
}
