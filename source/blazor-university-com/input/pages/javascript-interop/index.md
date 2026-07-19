---
title: "JavaScript interop"
date: "2026-07-16"
order: 9
---

At present, there are a number of features WebAssembly does not support, therefore Blazor does not supply direct access to them. These are typically browser API features such as:

- [Media Capture](https://developer.mozilla.org/en-US/docs/Web/API/Media_Streams_API)
- [Popups](https://www.w3schools.com/js/js_popup.asp)
- [Web GL](https://developer.mozilla.org/en-US/docs/Web/API/WebGL_API)
- [Web Storage](https://developer.mozilla.org/en-US/docs/Web/API/Web_Storage_API)

To access these browser features we need to use JavaScript as an intermediary between Blazor and the Browser;
that is what this next section covers.

> **Note:** JavaScript interop is not available during static server-side rendering (static SSR) or during the prerendering phase of interactive render modes. Any attempt to call `IJSRuntime` during these phases will throw an `InvalidOperationException`. We must guard calls with `OnAfterRenderAsync` and the `firstRender` parameter.

## JavaScript module isolation

The recommended approach for JavaScript interop in modern Blazor is to use JavaScript module isolation. Instead of placing functions on the global `window` object, we import a JavaScript module and obtain an `IJSObjectReference`. Blazor also supports collocated JavaScript files: placing a script named `MyComponent.razor.js` alongside a component will have it automatically published as a web resource.

```razor
@inject IJSRuntime JSRuntime

@code {
    private async Task CallIsolatedJs()
    {
        var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/Index.razor.js");
        await module.InvokeVoidAsync("myFunction");
    }
}
```

This approach avoids polluting the global namespace and keeps scripts scoped to the components that need them.

## JavaScript interop caveats

There are a few caveats when working with JSInterop. These will be added to the following list as they are demonstrated
in future sections.

- [Do not invoke JSInterop during the server pre-rendering phase](/javascript-interop/calling-javascript-from-dotnet/updating-the-document-title/).
- [Do not use ElementReference objects too soon.](/javascript-interop/calling-javascript-from-dotnet/passing-html-element-references/#caveat)
- [Avoid memory leaks by disposing of resources](/javascript-interop/calling-dotnet-from-javascript/lifetimes-and-memory-leaks/).
- [Avoid invoking methods on disposed .NET references](/javascript-interop/calling-dotnet-from-javascript/lifetimes-and-memory-leaks/#caveat).
- [Do not invoke .NET methods before Blazor has initialized](/javascript-interop/javascript-boot-process/).

> **Note:** The exercises in this section for updating the document title and auto-focusing elements are now superseded by built-in Blazor features. Use the `<PageTitle>` component for document titles and `ElementReference.FocusAsync()` for focusing elements. The JavaScript interop versions are retained as teaching examples to illustrate the underlying mechanics.

