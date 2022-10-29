﻿namespace BlazorUniversityCom;

using Statiq.Web.Modules;
using Statiq.Lunr;
using Statiq.Web.Pipelines;

public class SearchIndex : Pipeline
{
  public SearchIndex()
  {
    Dependencies.AddRange(nameof(Content));

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
                    .ZipIndexFile(ctx.GetBool(WebKeys.ZipSearchIndexFile, true))
                    .ZipResultsFile(ctx.GetBool(WebKeys.ZipSearchResultsFile, true))
                    .IncludeHostInLinks(ctx.GetBool(WebKeys.SearchIncludeHost))
                    .AllowPositionMetadata(ctx.GetBool(WebKeys.SearchAllowPositionMetadata))
                    .WithoutAnyFields()
                    .WithFields(additionalFields)
                    .WithStemming(ctx.GetBool(WebKeys.SearchStemming));

                // Set manual stop words
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

    OutputModules = new ModuleList
    {
        new WriteFiles()
    };
  }
}
