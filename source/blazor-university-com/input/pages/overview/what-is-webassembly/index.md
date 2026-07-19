---
title: "What is WebAssembly?"
date: "2026-07-16"
order: 2
---

WebAssembly (abbreviated "Wasm") is a binary instruction set designed to run on any conforming host. Any host that adheres to the Wasm specification can read and execute WebAssembly binaries, either by interpreting them or by compiling them to native machine code ahead of time.

Wasm is akin to the Common Intermediate Language (CIL) that .NET source code compiles to. Just as .NET IL can be compiled to machine code by the runtime, WebAssembly provides a portable compilation target for languages such as C, C++, Rust, and C#.

![](images/image.png)
*Conceptual diagram: how Blazor uses WebAssembly to run .NET code in the browser.*

Blazor does not require .NET to be installed on the client. Instead, the .NET runtime is compiled to WebAssembly and runs inside the browser alongside your application code.

## How Blazor uses WebAssembly

When a Blazor WebAssembly application loads, the browser downloads a .NET runtime compiled to WebAssembly (the mono runtime, or the modern jiterpreter), along with your compiled application assemblies. The runtime interprets or just-in-time compiles your C# IL instructions as they execute.

Since .NET 6, Blazor WebAssembly supports ahead-of-time (AOT) compilation, where your C# code is compiled directly to WebAssembly during the build step. This speeds up runtime execution at the cost of larger download sizes. Since .NET 9, WebAssembly multithreading is available as an opt-in feature, allowing Blazor WebAssembly apps to use multiple web workers for parallel execution.

Blazor assemblies ship in the [Webcil](https://github.com/dotnet/designs/blob/main/accepted/2021/WebAssemblyWebcil.md) (`.wasm`) container format by default, avoiding issues with firewalls that block `.dll` file downloads.

## WebAssembly version history

WebAssembly was announced in 2015 and reached version 1.0 in 2019, becoming a W3C recommendation in December of that year. It was created by engineers from Google, Mozilla, Microsoft, and Apple to provide a portable, secure, and fast compilation target. Wasm 2.0 was ratified as a W3C recommendation in 2024, and Wasm 3.0 was finalized in 2025, adding garbage collection, memory64, and tail call support. WebAssembly has expanded beyond the browser into server-side runtimes (WASI) and edge computing platforms.

## Browser support

WebAssembly is supported in all modern browsers. Legacy Internet Explorer (not the new Edge browser) and Opera Mini are the only browsers that do not support it. (Data from [CanIUse.com](https://caniuse.com/wasm).)
