---
title: "Installing Blazor"
date: "2019-04-27"
order: 4
---

Blazor is part of the [.NET SDK](https://dotnet.microsoft.com/download), so we can get started by installing the latest version. It is included when you install the latest version of [Visual Studio](https://visualstudio.microsoft.com/vs/).

When installing, ensure you select the option **ASP.NET and web development** under the **Workloads** tab.

![](images/InstallAspWorkload.jpg)

For native applications hosted on **Android, iOS, macOS, Windows, and tvOS** using a Web UI interface, also select **.NET Multi-platform App UI development** in the Visual Studio installer, or install the MAUI workload via the CLI:

```
dotnet workload install maui
```

Blazor Hybrid apps hosted in WPF or WinForms do not need the MAUI workload. Instead, add the `Microsoft.AspNetCore.Components.WebView.Wpf` or `Microsoft.AspNetCore.Components.WebView.WindowsForms` NuGet package to your project.
