using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Site.Data;

namespace Site.Pages;

public class SitemapModel : PageModel
{
    public IActionResult OnGet()
    {
        var sb = new StringBuilder();
        sb.AppendLine("""<?xml version="1.0" encoding="UTF-8"?>""");
        sb.AppendLine("""<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">""");

        void Url(string loc, string priority)
        {
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{loc}</loc>");
            sb.AppendLine($"    <priority>{priority}</priority>");
            sb.AppendLine("  </url>");
        }

        Url($"{SiteInfo.BaseUrl}/", "1.0");
        foreach (var d in DocCatalog.All)
            Url($"{SiteInfo.BaseUrl}/validators/{d.Slug}", "0.8");
        foreach (var d in DocCatalog.All.Where(d => d.CanGenerate))
            Url($"{SiteInfo.BaseUrl}/generators/{d.Slug}", "0.7");
        Url($"{SiteInfo.BaseUrl}/privacy", "0.3");

        sb.AppendLine("</urlset>");
        return Content(sb.ToString(), "application/xml", Encoding.UTF8);
    }
}
