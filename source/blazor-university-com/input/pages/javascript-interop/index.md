---
title: "JavaScript interop"
date: "2019-04-27"
order: 8
---

At present, there are a number of features WebAssembly does not support, therefore Blazor does not supply direct access to them. These are typically browser API features such as:

- [Media Capture](https://developer.mozilla.org/en-US/docs/Web/API/Media_Streams_API)
- [Popups](https://www.w3schools.com/js/js_popup.asp)
- [Web GL](https://developer.mozilla.org/en-US/docs/Web/API/WebGL_API)
- [Web Storage](https://developer.mozilla.org/en-US/docs/Web/API/Web_Storage_API)

To access these browser features we need to use JavaScript as an intermediary between Blazor and the Browser;
that is what this next section covers.

## JavaScript interop caveats

There are a few caveats when working with JSInterop. These will be added to the following list as they are demonstrated
in future sections.

- [Do not invoke JSInterop during the server pre-rendering phase](/javascript-interop/calling-javascript-from-dotnet/updating-the-document-title#caveat).
- [Do not use ElementReference objects too soon.](/javascript-interop/calling-javascript-from-dotnet/passing-html-element-references#caveat)
- [Avoid memory leaks by disposing of resources](/javascript-interop/calling-dotnet-from-javascript/lifetimes-and-memory-leaks/).
- [Avoid invoking methods on disposed .NET references](/javascript-interop/calling-dotnet-from-javascript/lifetimes-and-memory-leaks/#caveat).
- [Do not invoke .NET methods before Blazor has initialized](/javascript-interop/calling-javascript-from-dotnet/passing-html-element-references/).

