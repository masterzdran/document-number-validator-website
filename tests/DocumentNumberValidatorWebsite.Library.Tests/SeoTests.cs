using System.Text.Json;
using Site.Data;
using Xunit;

namespace DocumentNumberValidatorWebsite.Library.Tests;

public class SeoTests
{
    [Fact]
    public void Homepage_jsonld_is_valid_software_application_schema()
    {
        using var doc = JsonDocument.Parse(JsonLd.Website());
        Assert.Equal("SoftwareApplication", doc.RootElement.GetProperty("@type").GetString());
        Assert.Equal("https://schema.org", doc.RootElement.GetProperty("@context").GetString());
    }

    [Fact]
    public void Landing_jsonld_contains_faq_page_with_questions()
    {
        var nif = DocCatalog.Find("nif")!;
        using var doc = JsonDocument.Parse(JsonLd.Landing(nif, false, "https://example.org/validators/nif"));
        var root = doc.RootElement;
        Assert.Equal(JsonValueKind.Array, root.ValueKind);
        Assert.Contains(root.EnumerateArray(), e => e.GetProperty("@type").GetString() == "FAQPage");

        var faq = root.EnumerateArray().First(e => e.GetProperty("@type").GetString() == "FAQPage");
        Assert.True(faq.GetProperty("mainEntity").GetArrayLength() >= 4);
    }

    [Fact]
    public void Faq_answers_mention_the_real_nuget_package()
    {
        var niss = DocCatalog.Find("niss")!;
        var faqs = JsonLd.Faqs(niss, generatorPage: false);
        Assert.Contains(faqs, f => f.Answer == "dotnet add package DocumentNumber.Portugal.Niss.Validator");
    }

    [Fact]
    public void Code_samples_escape_markup()
    {
        var html = CodeHighlight.ToHtml("var x = \"<script>\"; // note", "csharp");
        Assert.DoesNotContain("<script>", html);
        Assert.Contains("&lt;script&gt;", html);
    }

    [Fact]
    public void Code_samples_highlight_keywords_and_strings()
    {
        var html = CodeHighlight.ToHtml("var n = 42;", "csharp");
        Assert.Contains("tok-k", html);
        Assert.Contains("tok-n", html);
    }

    [Fact]
    public void Landing_pages_have_real_runnable_snippets()
    {
        foreach (var d in DocCatalog.All)
        {
            var (ns, cls) = (d.ValidatorType[..d.ValidatorType.LastIndexOf('.')], d.ValidatorType[(d.ValidatorType.LastIndexOf('.') + 1)..]);
            var validate = CodeSamples.ValidateFor(d);
            Assert.Contains($"using {ns};", validate);
            Assert.Contains($"new {cls}()", validate);
            Assert.Contains(d.Example, validate);

            if (d.CanGenerate)
            {
                var g = d.GeneratorType!;
                Assert.Contains($"new {g[(g.LastIndexOf('.') + 1)..]}()", CodeSamples.GenerateFor(d));
            }
        }
    }
}
