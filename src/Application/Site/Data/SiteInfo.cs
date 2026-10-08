using System.Reflection;

namespace Site.Data;

public static class SiteInfo
{
    /// <summary>Canonical base URL of the deployed site (used for canonical/OG/sitemap URLs).</summary>
    public const string BaseUrl = "https://document-validator.fundisk.eu";

    public const string RepositoryUrl = "https://github.com/masterzdran/document-number-validator";
    public const string WebsiteRepositoryUrl = "https://github.com/masterzdran/document-number-validator-website";
    public const string NuGetProfileUrl = "https://www.nuget.org/profiles/masterzdran";

    /// <summary>e.g. "1.0.0" locally, "1.0.0+build.247" when CI sets BUILD_BUILDNUMBER/GITHUB_RUN_NUMBER.</summary>
    public static string Version { get; } =
        typeof(SiteInfo).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? "1.0.0";
}
