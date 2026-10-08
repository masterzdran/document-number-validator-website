using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Site.Infrastructure;

/// <summary>Inline SVG icon set (feather-style, stroke: currentColor) — no icon font, no CDN.</summary>
public static class IconExtensions
{
    private static readonly Dictionary<string, string> Paths = new()
    {
        ["layers"] = "M12 2l9 5-9 5-9-5 9-5zM3 12l9 5 9-5M3 17l9 5 9-5",
        ["shield"] = "M12 2l7 3v6c0 4.5-3 8.4-7 10-4-1.6-7-5.5-7-10V5l7-3zM8.5 12l2.4 2.4L15.8 9.5",
        ["wand"] = "M15 4v2M15 16v2M8 9h2M19 9h2M17.5 6.5L19 5M17.5 15.5L19 17M3 21l9-9M11.5 5.5L10 4",
        ["cpu"] = "M5 5h14v14H5zM9 9h6v6H9zM9 1v2M15 1v2M9 21v2M15 21v2M1 9h2M1 15h2M21 9h2M21 15h2",
        ["code"] = "M16 18l6-6-6-6M8 6l-6 6 6 6",
        ["branch"] = "M6 3v12M18 9a3 3 0 100-6 3 3 0 000 6zM6 21a3 3 0 100-6 3 3 0 000 6zM18 9a9 9 0 01-9 9",
        ["cube"] = "M21 16V8a2 2 0 00-1-1.73l-7-4a2 2 0 00-2 0l-7 4A2 2 0 003 8v8a2 2 0 001 1.73l7 4a2 2 0 002 0l7-4A2 2 0 0021 16z",
        ["rocket"] = "M4.5 16.5c-1.5 1.26-2 5-2 5s3.74-.5 5-2c.71-.84.7-2.13-.09-2.91a2.18 2.18 0 00-2.91-.09zM12 15l-3-3a22 22 0 012-3.95A12.88 12.88 0 0122 2c0 2.72-.78 7.5-6 11a22.35 22.35 0 01-4 2z",
        ["activity"] = "M22 12h-4l-3 9L9 3l-3 9H2",
        ["pin"] = "M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0118 0z|M12 13a3 3 0 100-6 3 3 0 000 6z",
        ["zap"] = "M13 2L3 14h9l-1 8 10-12h-9l1-8z",
        ["terminal"] = "M4 17l6-6-6-6M12 19h8",
        ["check"] = "M22 11.08V12a10 10 0 11-5.93-9.14|M22 4L12 14.01l-3-3",
        ["globe"] = "M12 2a10 10 0 100 20 10 10 0 000-20z|M2 12h20M12 2a15.3 15.3 0 014 10 15.3 15.3 0 01-4 10 15.3 15.3 0 01-4-10 15.3 15.3 0 014-10z",
        ["book"] = "M4 19.5A2.5 2.5 0 016.5 17H20M6.5 2H20v20H6.5A2.5 2.5 0 014 19.5v-15A2.5 2.5 0 016.5 2z",
        ["grid"] = "M3 3h7v7H3zM14 3h7v7h-7zM14 14h7v7h-7zM3 14h7v7H3z",
        ["hash"] = "M4 9h16M4 15h16M10 3L8 21M16 3l-2 18",
        ["credit-card"] = "M2 5h20v14H2zM2 10h20",
    };

    public static IHtmlContent Icon(this IHtmlHelper html, string name, int size = 22)
    {
        if (!Paths.TryGetValue(name, out var path))
            path = Paths["check"];

        var subpaths = path.Split('|');
        var inner = string.Concat(subpaths.Select(p =>
            $"<path d=\"{p}\" stroke=\"currentColor\" stroke-width=\"1.7\" fill=\"none\" stroke-linecap=\"round\" stroke-linejoin=\"round\" />"));

        return new HtmlString($"<svg class=\"icon\" width=\"{size}\" height=\"{size}\" viewBox=\"0 0 24 24\" aria-hidden=\"true\">{inner}</svg>");
    }
}
