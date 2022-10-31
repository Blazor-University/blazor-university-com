namespace BlazorUniversityCom;

using Statiq.Web.Modules;
using Statiq.Lunr;
using Statiq.Web.Pipelines;

public class SearchIndex : Pipeline
{
  public SearchIndex()
  {
    // Dependencies is a HashSet<string> that contains the names of pipelines that the current one depends on,
    // content pipeline should excute before searchindex pipeline
    Dependencies.AddRange(nameof(Content));

    // The post-process phase can be used for modules that need access to output documents from the process phase of every pipeline regardless of dependencies
    PostProcessModules = new ModuleList
    {
        new ExecuteIf(Config.FromSetting(CustomKeys.GenerateSearchIndex, false))
        {
            new ReplaceDocuments(nameof(Content)),

            new FilterDocuments(Config.FromDocument(WebKeys.ShouldOutput, true)),

            new ExecuteConfig(Config.FromContext(ctx =>
            {
                // Figure out additional search and result fields
                var additionalFields = new Dictionary<string, FieldType>(GenerateLunrIndex.DefaultFields, StringComparer.OrdinalIgnoreCase)
                                        {
                                          {"tags", FieldType.Searchable}
                                        };

                IReadOnlyList<string> additionalSearchableFields = ctx.GetList<string>(WebKeys.AdditionalSearchableFields);

                if (additionalSearchableFields is object)
                {
                    foreach (string additionalSearchableField in additionalSearchableFields)
                    {
                        additionalFields[additionalSearchableField] = additionalFields.ContainsKey(additionalSearchableField)
                            ? additionalFields[additionalSearchableField] | FieldType.Searchable
                            : FieldType.Searchable;
                    }
                }

                IReadOnlyList<string> additionalSearchResultFields = ctx.GetList<string>(WebKeys.AdditionalSearchResultFields);

                if (additionalSearchResultFields is object)
                {
                    foreach (string additionalSearchResultField in additionalSearchResultFields)
                    {
                        additionalFields[additionalSearchResultField] = additionalFields.ContainsKey(additionalSearchResultField)
                            ? additionalFields[additionalSearchResultField] | FieldType.Result
                            : FieldType.Result;
                    }
                }

                // Create the module
                GenerateLunrIndex generateLunrIndex = new GenerateLunrIndex()
                    .WithIndexPath(ctx.GetPath(WebKeys.SearchScriptPath))
                    .ZipIndexFile(ctx.GetBool(WebKeys.ZipSearchIndexFile, false))
                    .ZipResultsFile(ctx.GetBool(WebKeys.ZipSearchResultsFile, false))
                    .IncludeHostInLinks(ctx.GetBool(WebKeys.SearchIncludeHost))
                    .AllowPositionMetadata(ctx.GetBool(WebKeys.SearchAllowPositionMetadata))
                    .WithoutAnyFields()
                    .WithFields(additionalFields)
                    .WithStemming(ctx.GetBool(WebKeys.SearchStemming));

                // Set manual stop words
                // Stop Words: A stop word is a commonly used word (such as “the”, “a”, “an”, “in”) that a search engine has been programmed to ignore
                IReadOnlyList<string> stopWords = ctx.GetList<string>(WebKeys.SearchStopWords);
                NormalizedPath stopWordsFilePath = ctx.GetPath(WebKeys.SearchStopWordsFilePath);
                if (stopWords is object)
                {
                    generateLunrIndex.WithStopWords(Config.FromValue((IEnumerable<string>)stopWords));
                }
                else if (!stopWordsFilePath.IsNullOrEmpty)
                {
                    generateLunrIndex.WithStopWordsFromFile(stopWordsFilePath);
                }

                return generateLunrIndex;
            }))
        }
    };

    // Output phase pipline.
    OutputModules = new ModuleList
    {
        new WriteFiles()
    };
  }
}
