# Document Number Validator — Website

Marketing, documentation and live-demo website for the
[document-number-validator](https://github.com/masterzdran/document-number-validator) libraries.

> **Website:** <https://document-validator.fundisk.eu>

## Stack

- **.NET 10** — ASP.NET Core **Razor Pages** (server-rendered HTML for SEO, no SPA framework)
- No Bootstrap/icon fonts — a small custom CSS design system (`wwwroot/css/site.css`)
- Dark-mode-first UI with optional light theme (Fluent-inspired, Azure blue palette)
- The interactive demo runs the **real NuGet validator/generator packages** server-side.
  A small `fetch()` enhancement posts to a JSON handler for an in-place result; the plain
  form POST + Post/Redirect/Get stays as the no-JavaScript fallback

## Structure

```
src/Application/Site/
  Data/DocCatalog.cs        Single source of truth: 14 document types driving
                            homepage cards, demo, landing pages and sitemap
  Data/CodeSamples.cs       Real, compile-checked code snippets
  Data/CodeHighlight.cs     Dependency-free server-side syntax highlighting
  Data/JsonLd.cs            Schema.org structured data (SoftwareApplication + FAQPage)
  Pages/Demo.cs            DemoRunner (shared validate/generate logic) + the
                            OnPostDemo / OnPostDemoJson handlers (no-JS and fetch paths)
  Pages/Index.cshtml        Homepage (hero, benefits, doc types, code tabs, demo, features)
  Pages/Validators.cshtml   /validators/{slug}  — SEO landing page for every document type
  Pages/Generators.cshtml   /generators/{slug}  — same for generators (testing purposes)
  Pages/Sitemap.cshtml      /sitemap.xml generated from the catalogue
tests/…Library.Tests/       Catalogue integrity + generator→validator roundtrip tests
```

## Interactive demo

The demo runs the **real NuGet packages** server-side — input is never stored and never
leaves the request. Two front doors share one implementation (`DemoRunner` in
`Pages/Demo.cs`), so the live site and the no-JavaScript path can never drift apart:

| Path | Request | Response |
| --- | --- | --- |
| No JavaScript (fallback) | `POST {page}?handler=Demo` — form post + Post/Redirect/Get | full page re-render |
| JavaScript (default) | `POST {page}?handler=DemoJson` — `fetch` with the same form fields | `{ "level": "success\|error\|warn", "message": "…", "value": "…" }` |

The AJAX call is progressive enhancement: `wwwroot/js/site.js` intercepts the submit,
posts the same `FormData` (antiforgery token included) to the JSON handler and replaces
`#demo-result` (`role="status"`, `aria-live="polite"`) in place — no reload, results
announced to screen readers. Any failure falls back to the normal form submit, so the
demo keeps working with JavaScript disabled, offline, or on an old browser.

## Supported document types (v1.7.4 packages)

Portugal — NIF, NISS, Citizen Card, NIB, IBAN · Spain — DNI/NIE/CIF ·
France — TVA · Brazil — CPF/CNPJ · Payment cards — Amex, Maestro, Maestro UK,
Mastercard, VISA, VISA Electron.

## Build & run

```bash
dotnet build
dotnet test
dotnet run --project src/Application/Site
```

## Version badge

The footer version (`v1.0.0+build.247`) is composed from `Directory.Build.props` plus the CI
build number (`BUILD_BUILDNUMBER` in Azure Pipelines / `GITHUB_RUN_NUMBER` in GitHub Actions).

## Attribution

The logo shown on the landing page comes from the
[document-number-validator](https://github.com/masterzdran/document-number-validator)
repository ([`images/cards.512.png`](https://github.com/masterzdran/document-number-validator/blob/develop/images/cards.512.png)).
Original icons made by [Pixel perfect](https://icon54.com/) from [www.flaticon.com](https://www.flaticon.com/).

## License

MIT — © Fundisk Entertainment.
