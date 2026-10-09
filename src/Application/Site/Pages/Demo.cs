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

/// <summary>Outcome of one demo run, shared by the HTML (PRG) and JSON (fetch) paths.</summary>
/// <remarks>Value is null when the input box should keep whatever the user typed (e.g. no generator).</remarks>
public sealed record DemoResult(string Level, string Message, string? Value);

/// <summary>
/// Runs the demo against the real library. The single source of truth for both the
/// no-JavaScript form post and the AJAX endpoint, so the two never drift apart.
/// </summary>
public static class DemoRunner
{
    /// <returns>A result for the given document, or null when the slug is unknown.</returns>
    public static DemoResult? Run(DocSpec doc, string action, string? rawValue)
    {
        var value = (rawValue ?? string.Empty).Trim();

        if (action == "generate")
        {
            if (doc.Generate is null)
                return new DemoResult("warn", $"{doc.Name} has no generator — validation only.", null);

            var sample = doc.Generate();
            return new DemoResult("success", $"Generated sample: {sample}", sample);
        }

        if (value.Length == 0)
            return new DemoResult("warn", "Enter a value to validate.", value);

        return doc.Validate(value)
            ? new DemoResult("success", $"“{value}” is a valid {doc.Name}.", value)
            : new DemoResult("error", $"“{value}” is not a valid {doc.Name}.", value);
    }
}

/// <summary>Shared GET/POST behaviour for every page that embeds the demo.</summary>
public abstract class DocPageModel : PageModel
{
    [BindProperty]
    public DemoInput Input { get; set; } = new();

    // Populated after the post-redirect-get cycle; read by the demo partial.
    public string? DemoLevel => TempData["demo.level"] as string;
    public string? DemoMessage => TempData["demo.message"] as string;
    public string? DemoValue => TempData["demo.value"] as string;

    /// <summary>No-JavaScript path: post, redirect, get (the partial re-renders the result).</summary>
    public IActionResult OnPostDemo(string action)
    {
        var doc = DocCatalog.Find(Input.DocSlug);
        if (doc is null)
            return RedirectToPage();

        var result = DemoRunner.Run(doc, action, Input.Value)!;
        TempData["demo.level"] = result.Level;
        TempData["demo.message"] = result.Message;
        if (result.Value is not null)
            TempData["demo.value"] = result.Value;

        return RedirectToPage();
    }

    /// <summary>Progressive enhancement path: same logic, JSON response, no page reload.</summary>
    public IActionResult OnPostDemoJson(string action)
    {
        var doc = DocCatalog.Find(Input.DocSlug);
        if (doc is null)
            return new JsonResult(new { message = "Unknown document type." }) { StatusCode = StatusCodes.Status400BadRequest };

        var result = DemoRunner.Run(doc, action, Input.Value)!;
        return new JsonResult(new { level = result.Level, message = result.Message, value = result.Value });
    }
}
