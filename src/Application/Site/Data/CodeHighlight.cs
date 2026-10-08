using System.Text.RegularExpressions;

namespace Site.Data;

/// <summary>
/// Minimal server-side syntax highlighter — no JS dependency, no CDN.
/// Single-pass combined regex over HTML-escaped text, so injected spans are never re-scanned.
/// </summary>
public static partial class CodeHighlight
{
    private const string CsharpKeywords =
        "abstract|as|async|await|base|bool|break|byte|case|catch|char|checked|class|const|continue|decimal|default|delegate|do|double|else|enum|event|explicit|extern|false|finally|fixed|float|for|foreach|goto|if|implicit|in|int|interface|internal|is|lock|long|namespace|new|null|object|operator|out|override|params|private|protected|public|readonly|ref|return|sbyte|sealed|short|sizeof|stackalloc|static|string|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|unsafe|ushort|using|var|virtual|void|volatile|while|record|dynamic|nameof|when|required|init";

    private const string ShellKeywords = "dotnet|nuget|add|package|restore|build|run|test|publish|new|tool|install";

    [GeneratedRegex(
        @"(?<c>//[^\n]*|#[^\n]*)|(?<s>@?""(?:[^""\\]|\\.)*""|'(?:[^'\\]|\\.)*')|\b(?<k>(?:" + CsharpKeywords + @"))\b|\b(?<n>\d+)\b|\b(?<t>[A-Z][A-Za-z0-9_]*)\b",
        RegexOptions.Multiline)]
    private static partial Regex Csharp();

    [GeneratedRegex(
        @"(?<c>#[^\n]*)|(?<s>""[^""]*"")|\b(?<k>(?:" + ShellKeywords + @"))\b",
        RegexOptions.Multiline)]
    private static partial Regex Shell();

    public static string ToHtml(string code, string lang) =>
        Wrap(Escape(code), lang == "bash" ? Shell() : Csharp());

    // Encode only markup characters: string regex needs literal '"' to survive.
    private static string Escape(string s) =>
        s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");

    private static string Wrap(string escaped, Regex regex) =>
        regex.Replace(escaped, static m =>
        {
            var cls = m.Groups["c"].Success ? "c"
                    : m.Groups["s"].Success ? "s"
                    : m.Groups["k"].Success ? "k"
                    : m.Groups["n"].Success ? "n"
                    : "t";
            return $"<span class=\"tok-{cls}\">{m.Value}</span>";
        });
}
