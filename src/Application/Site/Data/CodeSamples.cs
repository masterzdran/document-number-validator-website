namespace Site.Data;

/// <summary>Real, runnable code snippets. Every snippet compiles against the packages referenced by the site.</summary>
public static class CodeSamples
{
    public const string Hero = """
        using DocumentNumber.Portugal.Vat.Validator;

        var validator = new VatValidator();

        if (validator.Validate("798945320"))
        {
            Console.WriteLine("Valid");
        }
        """;

    public const string Validate = """
        using DocumentNumber.Portugal.Vat.Validator;

        var validator = new VatValidator();
        var isValid = validator.Validate("798945320");

        Console.WriteLine(isValid ? "Valid" : "Invalid");
        """;

    public const string Generate = """
        using DocumentNumber.Portugal.Vat.Generator;

        var generator = new VatGenerator();
        var nif = generator.GenerateDocumentNumber();

        Console.WriteLine(nif);
        """;

    public const string Batch = """
        using DocumentNumber.Portugal.Vat.Generator;

        var generator = new VatGenerator();
        var samples = Enumerable.Range(0, 5)
            .Select(_ => generator.GenerateDocumentNumber())
            .Distinct()
            .ToArray();

        Console.WriteLine($"{samples.Length} sample NIFs generated");
        """;

    public const string Custom = """
        using DocumentNumber.ValidatorAbstractions;
        using DocumentNumber.Portugal.Vat.Validator;

        IDocumentNumberValidator validator = new VatValidator();
        var result = validator.Validate("798945320");

        Console.WriteLine(result);
        """;

    /// <summary>Validate snippet for a specific document type (landing pages).</summary>
    public static string ValidateFor(DocSpec d)
    {
        var (ns, type) = Split(d.ValidatorType);
        return $"""
            using {ns};

            var validator = new {type}();
            var isValid = validator.Validate("{d.Example}");

            Console.WriteLine(isValid ? "Valid" : "Invalid");
            """;
    }

    /// <summary>Generate snippet for a specific document type (landing pages).</summary>
    public static string GenerateFor(DocSpec d)
    {
        var (ns, type) = Split(d.GeneratorType!);
        return $"""
            using {ns};

            var generator = new {type}();
            var sample = generator.GenerateDocumentNumber();

            Console.WriteLine(sample);
            """;
    }

    private static (string Ns, string Type) Split(string fullName)
    {
        var i = fullName.LastIndexOf('.');
        return (fullName[..i], fullName[(i + 1)..]);
    }
}
