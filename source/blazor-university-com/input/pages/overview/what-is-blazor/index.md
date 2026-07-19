---
title: "What is Blazor?"
date: "2026-07-16"
order: 1
---

Blazor is a web development framework for building interactive web UIs with .NET. The name Blazor is a combination of the words Browser and Razor (the .NET HTML view generating engine). Unlike traditional ASP.NET where Razor views execute on the server for every request, Blazor components can render statically on the server, interactively on the server via a SignalR connection, on the client via WebAssembly, or inside native applications through Blazor Hybrid.

## Render modes

In the unified Blazor Web App model (introduced in .NET 8), components use **render modes** to control where and how they execute:

- **Static Server Rendering (Static SSR)** - Components render to HTML on the server. No interactivity, no SignalR circuit. This is the default render mode.
- **Interactive Server** - Components run on the server and communicate with the browser over a persistent SignalR connection. UI events are handled server-side.
- **Interactive WebAssembly** - Components run in the browser via WebAssembly. The .NET runtime is downloaded to the client.
- **Interactive Auto** - Starts with Interactive Server while the WebAssembly assets download in the background, then switches to WebAssembly on subsequent visits.
- **Blazor Hybrid** - Components run inside a native .NET MAUI, WPF, or WinForms application using the `BlazorWebView` control.

We cover these render modes in more detail on the [hosting models](/overview/blazor-hosting-models/) page.

## What Blazor is not

Blazor is not like Silverlight, Microsoft's previous attempt at hosting in-browser applications. Silverlight required a browser plugin to run on the client, which prevented it from running on iOS devices. Blazor requires no plugin of any kind.

Because WebAssembly is a web standard, it is supported on all major browsers, which means Blazor apps can run on Windows, Linux, macOS, Android, and iOS.

## Why choose Blazor?

Blazor lets you write both client-side and server-side logic in C#, sharing code and libraries across the full stack. You can use the same .NET ecosystem, tooling, and NuGet packages you already know. This is a significant advantage over JavaScript-based single-page application frameworks, where you must work in two different languages and coordinate separate codebases.

## Blazor is open-source

Blazor source code is available [here](https://github.com/dotnet/aspnetcore/tree/main/src/Components). Blazor is developed by Microsoft as part of the ASP.NET Core project and is governed by [The .NET Foundation](https://dotnetfoundation.org/), a non-profit organization created for the purpose of supporting open-source projects based around the .NET platform.
