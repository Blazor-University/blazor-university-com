namespace BlazorUniversityCom;
using System.Threading.Tasks;
using Statiq.App;
using Statiq.Common;
using Statiq.Core;
using Statiq.Docs;
using Statiq.Lunr;


internal class Program
{
  public static async Task<int> Main(string[] aArgumentArray) =>
    await Bootstrapper
      .Factory
      .CreateDocs(aArgumentArray)
      .AddSetting(CustomKeys.GenerateSearchIndex,true)
      .AddPipeline(
        "RawMarkdown",
        new ReadFiles("**/*.md"),
        new ExtractFrontMatter(new Statiq.Yaml.ParseYaml()),
        // Content pages live under input/pages but are served from the site root
        new SetDestination(
          Config.FromDocument(doc =>
            doc.Destination.Segments.Length > 0 && doc.Destination.Segments[0].ToString() == "pages"
              ? new NormalizedPath(doc.Destination.FullPath["pages/".Length..])
              : doc.Destination)),
        new WriteFiles())
      .RunAsync();
}
