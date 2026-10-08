using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Data;

namespace Site.Pages;

public class DemoInput
{
    public string DocSlug { get; set; } = DocCatalog.All[0].Slug;
    public string? Value { get; set; }
}

/// <summary>View model for the shared demo form (homepage select-mode or landing fixed-mode).</summary>
public sealed record DemoVm(DemoInput Input, DocSpec? Fixed, string Heading, string Blurb);

/// <summary>Shared GET/POST behaviour for every page that embeds the demo.</summary>
public abstract class DocPageModel : PageModel
{
    [BindProperty]
    public DemoInput Input { get; set; } = new();

    // Populated after the post-redirect-get cycle; read by the demo partial.
    public string? DemoLevel => TempData["demo.level"] as string;
    public string? DemoMessage => TempData["demo.message"] as string;
    public string? DemoValue => TempData["demo.value"] as string;

    public IActionResult OnPostDemo(string action)
    {
        var doc = DocCatalog.Find(Input.DocSlug);
        if (doc is null)
            return RedirectToPage();

        var value = (Input.Value ?? string.Empty).Trim();
        string level, message;

        if (action == "generate")
        {
            if (doc.Generate is null)
            {
                level = "warn";
                message = $"{doc.Name} has no generator — validation only.";
            }
            else
            {
                var sample = doc.Generate();
                level = "success";
                message = $"Generated sample: {sample}";
                TempData["demo.value"] = sample;
            }
        }
        else if (value.Length == 0)
        {
            level = "warn";
            message = "Enter a value to validate.";
            TempData["demo.value"] = value;
        }
        else if (doc.Validate(value))
        {
            level = "success";
            message = $"“{value}” is a valid {doc.Name}.";
            TempData["demo.value"] = value;
        }
        else
        {
            level = "error";
            message = $"“{value}” is not a valid {doc.Name}.";
            TempData["demo.value"] = value;
        }

        TempData["demo.level"] = level;
        TempData["demo.message"] = message;
        return RedirectToPage();
    }
}
