---
title: "Creating a new project"
date: "2026-07-16"
order: 5
---

The easiest way to get started is to create a Blazor Web App via Visual Studio.

1. Open Visual Studio.
1. Click **Create a new project**.
1. In the search box type Blazor.
1. Select **Blazor Web App**.
1. Click **Next**.
1. Enter a project name, such as **MyFirstBlazorApp**.
1. Click **Next**.
1. Enter the following information
    - **Framework**: .NET 10 (or the latest version installed).
    - **Authentication type**: None
    - **Configure for HTTPS**: Checked
    - **Interactive render mode**: Server
    - **Interactivity location**: Global
    - **Include sample pages**: Checked
    - (Other options remain unchecked)
1. Click **Create**.

![](images/CreateABlazorServerProject.jpg)

### Using the CLI

To create the same project from the command line:

```
dotnet new blazor --interactivity Server --all-interactive -n MyFirstBlazorApp
```

The `--all-interactive` flag makes every page interactive by default, matching the **Global** interactivity location chosen in Visual Studio. Without it, interactivity is per-page/component and you must add `@rendermode` directives manually.

### Understanding the options

The **Interactive render mode** sets the default interactivity:
- **None** - Static SSR only. Pages render as static HTML with no event handling.
- **Server** - Interactive Server as the default render mode.
- **WebAssembly** - Interactive WebAssembly as the default render mode. Adds a `.Client` project.
- **Auto** - Interactive Auto (Server first, WebAssembly after download). Adds a `.Client` project.

**Interactivity location** controls whether interactivity is global (every component interactive by default) or per-page/component (you opt in using `@rendermode`).

### The generated project structure

After creation, you will see:

- `Program.cs` - Application entry point where services are registered and the pipeline is configured.
- `App.razor` - The root HTML document component containing `<html>`, `<head>`, and `<body>`.
- `Routes.razor` - The Router component that handles URL matching and page rendering.
- `Components/Pages/` - Contains the application's pages (e.g. `Home.razor`, `Counter.razor`).
- `Components/Layout/` - Contains layout components (e.g. `MainLayout.razor`, `NavMenu.razor`).
- `wwwroot/` - Static web assets (CSS, images, JavaScript).
