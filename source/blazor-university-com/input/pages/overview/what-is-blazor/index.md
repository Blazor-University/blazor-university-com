---
title: "What is Blazor?"
date: "2019-04-27"
order: 1
---

Blazor is a web development framework for building web UIs with .NET.
The name Blazor is a combination/mutation of the words Browser and Razor (the .NET HTML view generating engine).
The implication being that instead of having to execute Razor views on the server in order to present HTML to the browser,
Blazor is capable of executing these views on the client.

Blazor supports statically rendered (non-interactive) pages, and pages that are interactive through code executed in the browser via [WebAssembly](/overview/what-is-webassembly/), or where user interactions are sent over SignalR to the server where code is executed to react to those interactions.

## What Blazor is not

Blazor is not like Silverlight, Microsoft's previous attempt at hosting in-browser applications.
Silverlight required a browser plugin in order to run on the client, which prevented it from running on iOS devices.

Blazor does not require any kind of plugin installed on the client in order to execute inside a browser.

Because WebAssembly is a web standard, it is supported on all major browsers, which means also client-side Blazor apps
will run inside a browser on Windows/Linux/Mac/Android and iOS.

## Blazor is open-source

Blazor source code is available [here](https://github.com/dotnet/aspnetcore/tree/main/src/Components).
The source code is owned by [The .NET Foundation](https://dotnetfoundation.org/), a non-profit organization created for
the purpose of supporting open-source projects based around the .NET framework.
