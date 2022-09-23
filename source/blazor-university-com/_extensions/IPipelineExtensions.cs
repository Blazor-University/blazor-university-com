namespace Statiq.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class IPipelineExtensions
{
  public static string Dump(this IPipeline pipeline)
  {
    string indentation = "  ";
    var sb = new StringBuilder();
    sb
      .AppendLine()
      .AppendLine($"{indentation}Deployment: {pipeline.Deployment}")
      .AppendLine($"{indentation}ExecutionPolicy: {pipeline.ExecutionPolicy}")
      .AppendLine($"{indentation}Isolated: {pipeline.Isolated}")
      .AppendLine($"{indentation}Dependencies({pipeline.Dependencies.Count}):{string.Join(',', pipeline.Dependencies)}")
      .AppendLine($"{indentation}DependencyOf({pipeline.DependencyOf.Count}):{string.Join(',', pipeline.DependencyOf)}");

    sb.AppendLine($"{indentation}InputModules({pipeline.InputModules.Count.ToString()})");
    AppendModules(pipeline.InputModules, indentation, sb);

    sb.AppendLine($"{indentation}OutputModules({pipeline.OutputModules.Count.ToString()})");
    AppendModules(pipeline.OutputModules, indentation, sb);

    sb.AppendLine($"{indentation}ProcessModules({pipeline.ProcessModules.Count.ToString()})");
    AppendModules(pipeline.ProcessModules, indentation, sb);

    sb.AppendLine($"{indentation}PostProcessModules({pipeline.PostProcessModules.Count.ToString()})");
    AppendModules(pipeline.PostProcessModules, indentation, sb);

    return sb.ToString();

    static void AppendModules(ModuleList moduleList, string indentation, StringBuilder sb)
    {
      foreach (IModule? module in moduleList)
      {
        sb.AppendLine($"{indentation}{indentation}{module.ToString()}");
      }
    }
  }
}
