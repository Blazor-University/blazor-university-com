// namespace Statiq.App;

// using Statiq.Common;
// using Statiq.Web.Pipelines;
// using System.Text;
// using System.Text.Json;
// using BlazorUniversityCom;

// public static class BootstrapperExtensions
// {

//   public static Bootstrapper DisplayEngine(this Bootstrapper bootstrapper)
//   {
//     _ = bootstrapper
//       .ConfigureEngine
//       (
//         engine =>
//         {
//           string json = JsonSerializer.Serialize(engine, new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true });
//           Console.WriteLine(json);
//           string indentation = "  ";
//           var sb = new StringBuilder();
//           sb.AppendLine();
//           sb.AppendLine("Engine:");
//           sb.AppendLine($"{indentation}ApplicationState({engine.ApplicationState})");
//           sb.AppendLine($"{indentation}SerialExecution({engine.SerialExecution})");
//           sb.AppendLine($"{indentation}Analyzers({engine.Analyzers.Count})");
//           sb.AppendLine($"{indentation}ClassCatalog({engine.ClassCatalog.Count})");
//           sb.AppendLine($"{indentation}Shortcodes({engine.Shortcodes.Count})");
//           foreach (string? shortcode in engine.Shortcodes)
//           {
//             sb.AppendLine($"{shortcode}");
//           }
//           foreach (KeyValuePair<string, IPipeline> item in engine.Pipelines)
//           {
//             sb.AppendLine("----------");
//             sb.AppendLine($"Pipeline ({item.Key}):{item.Value.Dump()}");
//           }
//           Console.WriteLine(sb.ToString());
//         }
//       );

//     return bootstrapper;
//   }

//   public static Bootstrapper AddReadingTimeMeta(this Bootstrapper bootstrapper) =>
//     bootstrapper.AddSetting(MetaDataKeys.ReadTime, Config.FromDocument(doc => CalculateReadTime(doc)));
//   private static string CalculateReadTime(IDocument doc)
//   {
//     string? content = doc.GetContentStringAsync().Result;
//     int numberOfWordsInContent = content.Split(' ').Length;
//     //According to wiki, average reading time for one person is 150 words in 1 minute.
//     const int wordsPerMinute = 150;
//     int minutes = numberOfWordsInContent / wordsPerMinute;
//     string displayReadingTime = $"{minutes} MIN READ";
//     return (minutes == 0) ?
//       "QUICK READ" :
//       displayReadingTime;
//   }
// }
