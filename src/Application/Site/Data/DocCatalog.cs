using DocumentNumber.Portugal.Niss.Generator;
using DocumentNumber.Portugal.Niss.Validator;
using DocumentNumber.Portugal.CitizenCard.Generator;
using DocumentNumber.Portugal.CitizenCard.Validator;
using DocumentNumber.Portugal.BankAccountNumber.Generator;
using DocumentNumber.Portugal.BankAccountNumber.Validator;
using DocumentNumber.InternationalBankAccountNumber.Validator;
using DocumentNumber.PaymentCardNumber.AmericanExpress.Generator;
using DocumentNumber.PaymentCardNumber.AmericanExpress.Validator;
using DocumentNumber.PaymentCardNumber.Maestro.Generator;
using DocumentNumber.PaymentCardNumber.Maestro.Validator;
using DocumentNumber.PaymentCardNumber.MaestroUK.Generator;
using DocumentNumber.PaymentCardNumber.MaestroUK.Validator;
using DocumentNumber.PaymentCardNumber.Mastercard.Generator;
using DocumentNumber.PaymentCardNumber.Mastercard.Validator;
using DocumentNumber.PaymentCardNumber.VISA.Generator;
using DocumentNumber.PaymentCardNumber.VISA.Validator;
using DocumentNumber.PaymentCardNumber.VISAElectron.Generator;
using DocumentNumber.PaymentCardNumber.VISAElectron.Validator;
// VatValidator/VatGenerator exist in 4 country namespaces — alias to avoid ambiguity
using BrVatGen = DocumentNumber.Brazil.Vat.Generator.VatGenerator;
using BrVatVal = DocumentNumber.Brazil.Vat.Validator.VatValidator;
using FrVatGen = DocumentNumber.France.Vat.Generator.VatGenerator;
using FrVatVal = DocumentNumber.France.Vat.Validator.VatValidator;
using PtVatGen = DocumentNumber.Portugal.Vat.Generator.VatGenerator;
using PtVatVal = DocumentNumber.Portugal.Vat.Validator.VatValidator;
using EsVatGen = DocumentNumber.Spain.Vat.Generator.VatGenerator;
using EsVatVal = DocumentNumber.Spain.Vat.Validator.VatValidator;

namespace Site.Data;

/// <summary>
/// Single source of truth for every supported document type: drives the homepage
/// cards, the interactive demo, the SEO landing pages and the sitemap.
/// </summary>
public sealed class DocSpec
{
    public required string Slug { get; init; }
    public required string Name { get; init; }
    public required string Country { get; init; }
    public required string Group { get; init; }
    public required string Summary { get; init; }
    public required string HowItWorks { get; init; }
    /// <summary>A real value accepted by <see cref="Validate"/> (verified by tests).</summary>
    public required string Example { get; init; }
    public required string ValidatorPackage { get; init; }
    public required string ValidatorType { get; init; }
    public required Func<string, bool> Validate { get; init; }
    public string? GeneratorPackage { get; init; }
    public string? GeneratorType { get; init; }
    public Func<string>? Generate { get; init; }
    public bool CanGenerate => Generate is not null;
}

public static class DocCatalog
{
    public static readonly IReadOnlyList<DocSpec> All = new DocSpec[]
    {
        new()
        {
            Slug = "nif",
            Name = "NIF",
            Country = "Portugal",
            Group = "Portugal",
            Summary = "Portuguese taxpayer number (NIF) — 9 digits with an official check digit.",
            HowItWorks = "The validator parses the 9-digit NIF, recomputes the weighted check digit and compares it with the last digit, following the rule used by the Portuguese Tax Authority.",
            Example = "798945320",
            ValidatorPackage = "DocumentNumber.Portugal.Vat.Validator",
            ValidatorType = "DocumentNumber.Portugal.Vat.Validator.VatValidator",
            Validate = s => new PtVatVal().Validate(s),
            GeneratorPackage = "DocumentNumber.Portugal.Vat.Generator",
            GeneratorType = "DocumentNumber.Portugal.Vat.Generator.VatGenerator",
            Generate = () => new PtVatGen().GenerateDocumentNumber(),
        },
        new()
        {
            Slug = "niss",
            Name = "NISS",
            Country = "Portugal",
            Group = "Portugal",
            Summary = "Portuguese social security number (NISS) — 11 digits with embedded check digits.",
            HowItWorks = "The validator checks the 11-digit NISS structure and recomputes the check digits used by Segurança Social before accepting the number.",
            Example = "14165152022",
            ValidatorPackage = "DocumentNumber.Portugal.Niss.Validator",
            ValidatorType = "DocumentNumber.Portugal.Niss.Validator.NissValidator",
            Validate = s => new NissValidator().Validate(s),
            GeneratorPackage = "DocumentNumber.Portugal.Niss.Generator",
            GeneratorType = "DocumentNumber.Portugal.Niss.Generator.NissGenerator",
            Generate = () => new NissGenerator().GenerateDocumentNumber(),
        },
        new()
        {
            Slug = "citizen-card",
            Name = "Citizen Card",
            Country = "Portugal",
            Group = "Portugal",
            Summary = "Portuguese Citizen Card document number — 12 characters, digits plus a 3-letter series.",
            HowItWorks = "The validator normalises the document number, verifies the body against the expected12-character shape and recomputes the Citizen Card check digit.",
            Example = "835099610IP8",
            ValidatorPackage = "DocumentNumber.Portugal.CitizenCard.Validator",
            ValidatorType = "DocumentNumber.Portugal.CitizenCard.Validator.CitizenCardValidator",
            Validate = s => new CitizenCardValidator().Validate(s),
            GeneratorPackage = "DocumentNumber.Portugal.CitizenCard.Generator",
            GeneratorType = "DocumentNumber.Portugal.CitizenCard.Generator.CitizenCardNumberGenerator",
            Generate = () => new CitizenCardNumberGenerator().GenerateDocumentNumber(),
        },
        new()
        {
            Slug = "nib",
            Name = "NIB",
            Country = "Portugal",
            Group = "Portugal",
            Summary = "Portuguese bank account number (NIB) — 21 digits with bank check digits.",
            HowItWorks = "The validator checks the 21-digit NIB structure and the bank-specific check digits defined by Banco de Portugal.",
            Example = "000201231234567890154",
            ValidatorPackage = "DocumentNumber.Portugal.BankAccountNumber.Validator",
            ValidatorType = "DocumentNumber.Portugal.BankAccountNumber.Validator.BankAccountValidator",
            Validate = s => new BankAccountValidator().Validate(s),
        },
        new()
        {
            Slug = "iban",
            Name = "IBAN",
            Country = "International",
            Group = "International",
            Summary = "International Bank Account Number — mod-97 checksum per ISO 13616.",
            HowItWorks = "The validator moves the check digits to the end, applies the ISO 13616 mod-97-10 algorithm and accepts the IBAN only when the remainder is 1.",
            Example = "PT50000201231234567890154",
            ValidatorPackage = "DocumentNumber.InternationalBankAccountNumber.Validator",
            ValidatorType = "DocumentNumber.InternationalBankAccountNumber.Validator.InternationalBankAccountNumberValidator",
            Validate = s => new InternationalBankAccountNumberValidator().Validate(s),
            GeneratorPackage = "DocumentNumber.Portugal.BankAccountNumber.Generator",
            GeneratorType = "DocumentNumber.Portugal.BankAccountNumber.Generator.PortugalIbanGenerator",
            Generate = () => new PortugalIbanGenerator().GenerateDocumentNumber(),
        },
        new()
        {
            Slug = "spain-vat",
            Name = "DNI / NIE / CIF",
            Country = "Spain",
            Group = "Spain",
            Summary = "Spanish tax identifiers — DNI, NIE and CIF with their official check digits.",
            HowItWorks = "The validator identifies the document kind (DNI, NIE or CIF) and recomputes the matching check character: a letter for DNI/NIE, a control digit or letter for CIF.",
            Example = "49365682T",
            ValidatorPackage = "DocumentNumber.Spain.Vat.Validator",
            ValidatorType = "DocumentNumber.Spain.Vat.Validator.VatValidator",
            Validate = s => new EsVatVal().Validate(s),
            GeneratorPackage = "DocumentNumber.Spain.Vat.Generator",
            GeneratorType = "DocumentNumber.Spain.Vat.Generator.VatGenerator",
            Generate = () => new EsVatGen().GenerateDocumentNumber(),
        },
        new()
        {
            Slug = "france-vat",
            Name = "TVA (VAT)",
            Country = "France",
            Group = "France",
            Summary = "French VAT number (TVA) — SIREN-based with the official key check.",
            HowItWorks = "The validator normalises the FR-prefixed number, extracts the SIREN and verifies the two-key control defined by the French tax authorities.",
            Example = "FR94745420357",
            ValidatorPackage = "DocumentNumber.France.Vat.Validator",
            ValidatorType = "DocumentNumber.France.Vat.Validator.VatValidator",
            Validate = s => new FrVatVal().Validate(s),
            GeneratorPackage = "DocumentNumber.France.Vat.Generator",
            GeneratorType = "DocumentNumber.France.Vat.Generator.VatGenerator",
            Generate = () => new FrVatGen().GenerateDocumentNumber(),
        },
        new()
        {
            Slug = "brazil-vat",
            Name = "CPF / CNPJ",
            Country = "Brazil",
            Group = "Brazil",
            Summary = "Brazilian taxpayer identifiers — CPF (11 digits) and CNPJ (14 digits).",
            HowItWorks = "The validator applies the weighted check-digit algorithm of the Receita Federal to both CPF and CNPJ numbers.",
            Example = "24911211903",
            ValidatorPackage = "DocumentNumber.Brazil.Vat.Validator",
            ValidatorType = "DocumentNumber.Brazil.Vat.Validator.VatValidator",
            Validate = s => new BrVatVal().Validate(s),
            GeneratorPackage = "DocumentNumber.Brazil.Vat.Generator",
            GeneratorType = "DocumentNumber.Brazil.Vat.Generator.VatGenerator",
            Generate = () => new BrVatGen().GenerateDocumentNumber(),
        },
        new()
        {
            Slug = "american-express",
            Name = "American Express",
            Country = "International",
            Group = "Payment cards",
            Summary = "American Express card numbers — 15 digits, 34/37 prefixes, Luhn check.",
            HowItWorks = "The validator verifies the issuer prefix and length, then runs the Luhn algorithm over the digit string.",
            Example = "342633353118487",
            ValidatorPackage = "DocumentNumber.PaymentCardNumber.AmericanExpress.Validator",
            ValidatorType = "DocumentNumber.PaymentCardNumber.AmericanExpress.Validator.AmericanExpressPaymentCardValidator",
            Validate = s => new AmericanExpressPaymentCardValidator().Validate(s),
            GeneratorPackage = "DocumentNumber.PaymentCardNumber.AmericanExpress.Generator",
            GeneratorType = "DocumentNumber.PaymentCardNumber.AmericanExpress.Generator.AmericanExpressPaymentCardGenerator",
            Generate = () => new AmericanExpressPaymentCardGenerator().GenerateDocumentNumber(),
        },
        new()
        {
            Slug = "maestro",
            Name = "Maestro",
            Country = "International",
            Group = "Payment cards",
            Summary = "Maestro debit card numbers — 12 to 19 digits with Luhn check.",
            HowItWorks = "The validator checks the Maestro issuer ranges and length, then applies the Luhn checksum.",
            Example = "50188234134031",
            ValidatorPackage = "DocumentNumber.PaymentCardNumber.Maestro.Validator",
            ValidatorType = "DocumentNumber.PaymentCardNumber.Maestro.Validator.MaestroPaymentCardValidator",
            Validate = s => new MaestroPaymentCardValidator().Validate(s),
            GeneratorPackage = "DocumentNumber.PaymentCardNumber.Maestro.Generator",
            GeneratorType = "DocumentNumber.PaymentCardNumber.Maestro.Generator.MaestroPaymentCardGenerator",
            Generate = () => new MaestroPaymentCardGenerator().GenerateDocumentNumber(),
        },
        new()
        {
            Slug = "maestro-uk",
            Name = "Maestro UK",
            Country = "International",
            Group = "Payment cards",
            Summary = "Maestro UK debit card numbers — 12 to 19 digits with Luhn check.",
            HowItWorks = "The validator checks the Maestro UK issuer ranges and length, then applies the Luhn checksum.",
            Example = "67592821775211362",
            ValidatorPackage = "DocumentNumber.PaymentCardNumber.MaestroUK.Validator",
            ValidatorType = "DocumentNumber.PaymentCardNumber.MaestroUK.Validator.MaestroUKPaymentCardValidator",
            Validate = s => new MaestroUKPaymentCardValidator().Validate(s),
            GeneratorPackage = "DocumentNumber.PaymentCardNumber.MaestroUK.Generator",
            GeneratorType = "DocumentNumber.PaymentCardNumber.MaestroUK.Generator.MaestroUKPaymentCardGenerator",
            Generate = () => new MaestroUKPaymentCardGenerator().GenerateDocumentNumber(),
        },
        new()
        {
            Slug = "mastercard",
            Name = "Mastercard",
            Country = "International",
            Group = "Payment cards",
            Summary = "Mastercard numbers — 16 digits, 51–55 and 2221–2720 ranges, Luhn check.",
            HowItWorks = "The validator checks the Mastercard BIN ranges and length, then applies the Luhn checksum.",
            Example = "5375718830831320",
            ValidatorPackage = "DocumentNumber.PaymentCardNumber.Mastercard.Validator",
            ValidatorType = "DocumentNumber.PaymentCardNumber.Mastercard.Validator.MastercardPaymentCardValidator",
            Validate = s => new MastercardPaymentCardValidator().Validate(s),
            GeneratorPackage = "DocumentNumber.PaymentCardNumber.Mastercard.Generator",
            GeneratorType = "DocumentNumber.PaymentCardNumber.Mastercard.Generator.MastercardPaymentCardGenerator",
            Generate = () => new MastercardPaymentCardGenerator().GenerateDocumentNumber(),
        },
        new()
        {
            Slug = "visa",
            Name = "VISA",
            Country = "International",
            Group = "Payment cards",
            Summary = "Visa card numbers — 13, 16 or 19 digits starting with 4, Luhn check.",
            HowItWorks = "The validator verifies the Visa prefix and length, then applies the Luhn checksum.",
            Example = "4072347806866",
            ValidatorPackage = "DocumentNumber.PaymentCardNumber.VISA.Validator",
            ValidatorType = "DocumentNumber.PaymentCardNumber.VISA.Validator.VisaPaymentCardValidator",
            Validate = s => new VisaPaymentCardValidator().Validate(s),
            GeneratorPackage = "DocumentNumber.PaymentCardNumber.VISA.Generator",
            GeneratorType = "DocumentNumber.PaymentCardNumber.VISA.Generator.VisaPaymentCardGenerator",
            Generate = () => new VisaPaymentCardGenerator().GenerateDocumentNumber(),
        },
        new()
        {
            Slug = "visa-electron",
            Name = "VISA Electron",
            Country = "International",
            Group = "Payment cards",
            Summary = "Visa Electron numbers — specific issuer ranges, 13 or 16 digits, Luhn check.",
            HowItWorks = "The validator verifies the Visa Electron issuer ranges and length, then applies the Luhn checksum.",
            Example = "4844660042374762",
            ValidatorPackage = "DocumentNumber.PaymentCardNumber.VISAElectron.Validator",
            ValidatorType = "DocumentNumber.PaymentCardNumber.VISAElectron.Validator.VisaElectronPaymentCardValidator",
            Validate = s => new VisaElectronPaymentCardValidator().Validate(s),
            GeneratorPackage = "DocumentNumber.PaymentCardNumber.VISAElectron.Generator",
            GeneratorType = "DocumentNumber.PaymentCardNumber.VISAElectron.Generator.VisaElectronPaymentCardGenerator",
            Generate = () => new VisaElectronPaymentCardGenerator().GenerateDocumentNumber(),
        },
    };

    public static readonly IReadOnlyList<string> Groups =
        All.Select(d => d.Group).Distinct().ToArray();

    public static DocSpec? Find(string? slug) =>
        slug is null ? null : All.FirstOrDefault(d => d.Slug == slug);

    public static IReadOnlyList<DocSpec> InGroup(string group) =>
        All.Where(d => d.Group == group).ToArray();

    public static IEnumerable<string> ValidatorSlugs =>
        All.Select(d => d.Slug);

    public static IEnumerable<string> GeneratorSlugs =>
        All.Where(d => d.CanGenerate).Select(d => d.Slug);
}
