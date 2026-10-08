using Site.Data;

namespace Site.Pages;

/// <summary>Model for the shared landing-page partial.</summary>
public sealed record LandingVm(DocSpec Doc, bool IsGenerator, DemoInput Input);
