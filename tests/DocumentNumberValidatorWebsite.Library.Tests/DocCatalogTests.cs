using Site.Data;
using Xunit;

namespace DocumentNumberValidatorWebsite.Library.Tests;

public class DocCatalogTests
{
    [Fact]
    public void Slugs_are_unique_and_url_safe()
    {
        var slugs = DocCatalog.All.Select(d => d.Slug).ToList();
        Assert.Equal(slugs.Count, slugs.Distinct().Count());
        Assert.All(slugs, s => Assert.Matches("^[a-z0-9-]+$", s));
    }

    [Fact]
    public void Catalog_contains_the_document_families_from_the_library_repo()
    {
        // Portugal, Spain, France, Brazil, payment cards + IBAN (see document-number-validator README)
        Assert.Contains(DocCatalog.All, d => d.Slug == "nif");
        Assert.Contains(DocCatalog.All, d => d.Slug == "niss");
        Assert.Contains(DocCatalog.All, d => d.Slug == "citizen-card");
        Assert.Contains(DocCatalog.All, d => d.Slug == "nib");
        Assert.Contains(DocCatalog.All, d => d.Slug == "iban");
        Assert.Contains(DocCatalog.All, d => d.Slug == "spain-vat");
        Assert.Contains(DocCatalog.All, d => d.Slug == "france-vat");
        Assert.Contains(DocCatalog.All, d => d.Slug == "brazil-vat");
        Assert.Equal(6, DocCatalog.All.Count(d => d.Group == "Payment cards"));
    }

    [Fact]
    public void Every_document_has_a_validator_and_a_named_nuget_package()
    {
        Assert.All(DocCatalog.All, d =>
        {
            Assert.False(string.IsNullOrWhiteSpace(d.Name));
            Assert.False(string.IsNullOrWhiteSpace(d.Summary));
            Assert.False(string.IsNullOrWhiteSpace(d.HowItWorks));
            Assert.StartsWith("DocumentNumber.", d.ValidatorPackage);
            Assert.Contains('.', d.ValidatorType);
            Assert.NotNull(d.Validate);
        });
    }

    [Fact]
    public void Generator_slugs_exclude_documents_without_generation()
    {
        Assert.DoesNotContain("nib", DocCatalog.GeneratorSlugs);
        Assert.Contains("nib", DocCatalog.ValidatorSlugs);
        Assert.All(DocCatalog.All.Where(d => d.CanGenerate), d =>
        {
            Assert.StartsWith("DocumentNumber.", d.GeneratorPackage);
            Assert.NotNull(d.Generate);
        });
    }

    [Theory]
    [InlineData("nif")]
    [InlineData("niss")]
    [InlineData("citizen-card")]
    [InlineData("nib")]
    [InlineData("iban")]
    public void Published_examples_really_validate(string slug)
    {
        var doc = DocCatalog.Find(slug);
        Assert.NotNull(doc);
        Assert.True(doc!.Validate(doc.Example), $"{doc.Name} example '{doc.Example}' must be valid");
    }

    [Fact]
    public void Every_generated_sample_validates()
    {
        foreach (var doc in DocCatalog.All.Where(d => d.CanGenerate))
        {
            var sample = doc.Generate!();
            Assert.True(doc.Validate(sample), $"{doc.Name}: generated '{sample}' failed its own validator");
        }
    }

    [Fact]
    public void Every_validator_rejects_garbage()
    {
        foreach (var doc in DocCatalog.All)
        {
            Assert.False(doc.Validate("not-a-real-number"), $"{doc.Name} accepted garbage");
            Assert.False(doc.Validate(string.Empty), $"{doc.Name} accepted empty input");
        }
    }

    [Fact]
    public void Find_returns_null_for_unknown_or_null_slug()
    {
        Assert.Null(DocCatalog.Find("does-not-exist"));
        Assert.Null(DocCatalog.Find(null));
    }

    [Fact]
    public void Every_group_has_at_least_one_document()
    {
        Assert.All(DocCatalog.Groups, g => Assert.NotEmpty(DocCatalog.InGroup(g)));
    }
}
