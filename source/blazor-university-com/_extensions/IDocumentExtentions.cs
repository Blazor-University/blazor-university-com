namespace BlazorUniversityCom;

// using Octokit;
using Statiq.Razor;

public static class IDocumentExtentions
{
  public static string? GetDescription(this IDocument document) => document.GetString(WebKeys.Description);
  public static DateTime GetPublished(this IDocument document) => document.GetDateTime(WebKeys.Published);
  public static string GetFullLink(this IDocument document) => document.GetLink(true);
}
