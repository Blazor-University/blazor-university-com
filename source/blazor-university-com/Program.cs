namespace BlazorUniversityCom;
using System.Threading.Tasks;
using Statiq.App;
using Statiq.Docs;

internal class Program
{
  public static async Task<int> Main(string[] aArgumentArray) =>
    await Bootstrapper
      .Factory
      .CreateDocs(aArgumentArray)
      .RunAsync();
}
