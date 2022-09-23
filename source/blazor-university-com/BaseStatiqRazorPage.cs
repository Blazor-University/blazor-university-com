namespace BlazorUniversityCom;

using Statiq.Razor;
using System.Text.Json;

public abstract class BaseStatiqRazorPage<TModel> : StatiqRazorPage<TModel>
{

  #region Document
  public string? Description => Document.GetDescription();
  public string? Title => Document.GetTitle();
  public DateTime Published => Document.GetPublished();
  public string PublishedDate => Document.GetPublished().ToLongDateString();
  public string FullLink => Document.GetFullLink();
  public DocumentList<IDocument> Posts => Document.GetChildren();
  public IDocument? FeaturedPost => Posts.Count > 0 ? Posts[0]: null;
  public IEnumerable<IDocument> NonFeaturedPosts => Posts.Skip(1);
  public int PostCount => Posts.Count;
  #endregion

  #region Context
  public string? FavIconLink => Context.GetLink("favicon.png");
  public string? SiteTitle => Context.GetString(MetaDataKeys.SiteTitle);
  #endregion

  #region  Outputs
  public IDocument GetRoot() => OutputPages.Get($"index.html");

  public FilteredDocumentList<IDocument> AtomFeeds => Outputs["**/*.atom"];
  public FilteredDocumentList<IDocument> RssFeeds => Outputs["**/*.rss"];

  #endregion
  protected BaseStatiqRazorPage() { }

  public string PageTitle => $"{SiteTitle} - {Title}".Trim(new[] { ' ', '-' });

}
