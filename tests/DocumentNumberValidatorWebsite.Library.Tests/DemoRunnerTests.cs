using Site.Data;
using Site.Pages;
using Xunit;

namespace DocumentNumberValidatorWebsite.Library.Tests;

/// <summary>
/// The demo has two front doors (no-JavaScript form post and AJAX JSON handler) that
/// both call <see cref="DemoRunner"/>. These tests pin the shared behaviour.
/// </summary>
public class DemoRunnerTests
{
    [Theory]
    [InlineData("nif")]
    [InlineData("niss")]
    [InlineData("citizen-card")]
    [InlineData("iban")]
    [InlineData("visa")]
    public void Valid_example_reports_success_and_echoes_the_value(string slug)
    {
        var doc = DocCatalog.Find(slug)!;
        var result = DemoRunner.Run(doc, "validate", $"  {doc.Example}  ")!;

        Assert.Equal("success", result.Level);
        Assert.Equal(doc.Example, result.Value); // trimmed
        Assert.Contains(doc.Name, result.Message);
    }

    [Fact]
    public void Garbage_reports_error()
    {
        var doc = DocCatalog.Find("nif")!;
        var result = DemoRunner.Run(doc, "validate", "not-a-real-number")!;

        Assert.Equal("error", result.Level);
        Assert.Equal("not-a-real-number", result.Value);
    }

    [Fact]
    public void Empty_input_reports_warning()
    {
        var result = DemoRunner.Run(DocCatalog.Find("nif")!, "validate", "   ")!;

        Assert.Equal("warn", result.Level);
        Assert.Equal("Enter a value to validate.", result.Message);
    }

    [Fact]
    public void Generate_returns_a_sample_that_validates()
    {
        var doc = DocCatalog.Find("nif")!;
        var result = DemoRunner.Run(doc, "generate", null)!;

        Assert.Equal("success", result.Level);
        Assert.NotNull(result.Value);
        Assert.True(doc.Validate(result.Value!));
    }

    [Fact]
    public void Generate_without_a_generator_warns_and_keeps_the_input()
    {
        var doc = DocCatalog.Find("nib")!;
        var result = DemoRunner.Run(doc, "generate", "0000")!;

        Assert.Equal("warn", result.Level);
        Assert.Null(result.Value); // null = leave the input box untouched
    }
}
