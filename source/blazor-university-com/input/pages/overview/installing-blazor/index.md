---
title: "Installing Blazor"
date: "2026-07-16"
order: 4
---

Blazor is part of the [.NET SDK](https://dotnet.microsoft.com/download), so we can get started by installing the latest version. It is included when you install [Visual Studio](https://visualstudio.microsoft.com/vs/) (Community, Professional, or Enterprise edition).

## Cross-platform installation

Blazor development is fully supported on Windows, macOS, and Linux. In addition to Visual Studio on Windows, you can use:

- **Visual Studio Code** with the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) extension.
- **JetBrains Rider** on any platform.
- The **.NET CLI** directly from any terminal.

### CLI installation

1. Download and install the [.NET SDK](https://dotnet.microsoft.com/download) for your platform.
2. Verify the installation:

```
dotnet --version
```

3. Confirm that Blazor templates are available:

```
dotnet new list blazor
```

### Visual Studio

When installing Visual Studio, ensure you select the **ASP.NET and web development** workload under the **Workloads** tab.

For native applications hosted on **Android, iOS, macOS, and Windows** using a Web UI interface, also select **.NET Multi-platform App UI development** in the Visual Studio installer.

Or install the MAUI workload via the CLI:

```
dotnet workload install maui
```

Blazor Hybrid apps hosted in WPF or WinForms do not need the MAUI workload. Instead, add the `Microsoft.AspNetCore.Components.WebView.Wpf` or `Microsoft.AspNetCore.Components.WebView.WindowsForms` NuGet package to your project.
