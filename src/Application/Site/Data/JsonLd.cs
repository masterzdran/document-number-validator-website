using System.Text.Json;
using System.Text.Json.Serialization;

namespace Site.Data;

/// <summary>Schema.org structured data + FAQ content shared by landing pages.</summary>
public static class JsonLd
{
    private static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public static string Website() => JsonSerializer.Serialize(new Dictionary<string, object?>
    {
        ["@context"] = "https://schema.org",
        ["@type"] = "SoftwareApplication",
        ["name"] = "Document Number Validator",
        ["description"] = "Validate and generate document numbers using a high-performance .NET library. Support for multiple countries, extensible validation rules and production-ready APIs.",
        ["url"] = SiteInfo.BaseUrl,
        ["applicationCategory"] = "DeveloperApplication",
        ["operatingSystem"] = ".NET",
        ["license"] = "https://opensource.org/licenses/MIT",
        ["codeRepository"] = SiteInfo.RepositoryUrl,
        ["author"] = new { @type = "Person", name = "Nuno Cancelo" },
        ["offers"] = new { @type = "Offer", price = 0, priceCurrency = "USD" },
    }, Options);

    public static string Landing(DocSpec d, bool generatorPage, string url) => JsonSerializer.Serialize(new object[]
    {
        new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "SoftwareSourcecode",
            ["name"] = $"{d.Name} — Document Number Validator",
            ["description"] = d.Summary,
            ["url"] = url,
            ["codeRepository"] = SiteInfo.RepositoryUrl,
            ["programmingLanguage"] = "C#",
            ["license"] = "https://opensource.org/licenses/MIT",
        },
        new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org",
            ["@type"] = "FAQPage",
            ["mainEntity"] = Faqs(d, generatorPage).Select(f => new Dictionary<string, object?>
            {
                ["name"] = f.Question,
                ["acceptedAnswer"] = new Dictionary<string, object?>
                {
                    ["@type"] = "Answer",
                    ["text"] = f.Answer,
                },
            }).ToArray(),
        },
    }, Options);

    public static IReadOnlyList<(string Question, string Answer)> Faqs(DocSpec d, bool generatorPage)
    {
        var list = new List<(string Question, string Answer)>
        {
            ($"What is {d.Name}?", d.Summary),
            (generatorPage
                ? $"How does {d.Name} generation work?"
                : $"How does {d.Name} validation work?",
             generatorPage
                ? $"The generator builds a structurally valid {d.Name} sample using the same rules the validator enforces — including check digits. Generated values are intended for tests and development only."
                : d.HowItWorks),
            (generatorPage
                ? $"How do I install the {d.Name} generator package?"
                : $"How do I install the {d.Name} validator package?",
             $"dotnet add package {(generatorPage ? d.GeneratorPackage : d.ValidatorPackage)}"),
            ("Is the value I enter in the demo stored?",
             "No. Demo inputs are processed in memory for the current request only and are never persisted or shared."),
        };
        return list;
    }
}
