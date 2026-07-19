---
title: "Head outlets and content"
date: "2026-07-16"
order: 2
---

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Outlets/HeadOutletsAndContent)

Before .NET 8, setting the page title from a Blazor component required a JavaScript interop call: inject `IJSRuntime`, invoke `document.title = ...`, and remember to restore the title on disposal. Injecting `<meta>` tags or other `<head>` elements was even more cumbersome, often requiring manual DOM manipulation through JS interop. Blazor now provides first-class support for controlling the document head through a dedicated outlet system.

## HeadOutlet

The entry point is the `<HeadOutlet />` component. This component must be placed inside the `<head>` element of the host page. In a default Blazor project, this is done in `Components/App.razor` (or the equivalent top-level entry point).

```razor
@* Components/App.razor *@
<!DOCTYPE html>
<html>
<head>
  <HeadOutlet />
</head>
<body>
  <Routes />
  <script src="_framework/blazor.web.js"></script>
</body>
</html>
```

`HeadOutlet` acts as a render target. Any `<PageTitle>` or `<HeadContent>` component rendered elsewhere in the component tree will project its content into this outlet.

## PageTitle

The `<PageTitle>` component sets the document's title. Place it anywhere inside a page, layout, or component. When the component renders, the browser's title bar updates to reflect the supplied content.

```razor
@* A page that wants to set its own title *@
@page "/product/{Id}"

<PageTitle>Product @Id - My Store</PageTitle>

<h1>Product @Id</h1>
@* ...rest of the page... *@
```

When this page is navigated to, the browser title becomes "Product 42 - My Store" (assuming an Id of 42). When the user navigates away, the title is updated to whatever `PageTitle` the next page renders.

### Last-rendered-wins

If multiple `PageTitle` components are rendered at the same time, the one rendered last wins. This is important when a layout and a page both set a title. The layout might render:

```razor
@* MainLayout.razor *@
@inherits LayoutComponentBase

<PageTitle>My Store</PageTitle>

<div class="content">
  @Body
</div>
```

And the page might render its own `<PageTitle>` as in the example above. During rendering, the layout's `PageTitle` renders first, then the page's `PageTitle` renders second. The page's title wins, giving each page control over its own title while allowing the layout to provide a sensible default.

## HeadContent

For anything other than the `<title>` tag, use `<HeadContent>`. This component projects arbitrary markup into the document `<head>`. You can inject `<meta>` descriptions, Open Graph tags, `<link>` elements, `<style>` blocks, or deferred `<script>` tags.

```razor
@page "/product/{Id}"

<PageTitle>Product @Id - My Store</PageTitle>
<HeadContent>
  <meta name="description" content="View product @Id at My Store" />
  <meta property="og:title" content="Product @Id - My Store" />
  <link rel="preload" href="/images/product-@Id.webp" as="image" />
</HeadContent>
```

The last-rendered-wins rule applies to `HeadContent` as well. If two components output a `<meta name="description">`, the one rendered last will be the one that appears in the DOM. This means a layout can provide default `<head>` content and individual pages can override it without conflict.

## Static SSR and prerendering

Because `HeadOutlet`, `PageTitle`, and `HeadContent` are part of the component render tree, they work during static server-side rendering and during the prerendering phase of interactive modes. The rendered `<title>` and `<head>` elements are written directly into the static HTML response. This makes Blazor applications fully crawler-friendly: search engines and social-media preview scrapers see the correct title and meta tags without needing any client-side JavaScript execution.

## A note on the old approach

If you have existing Blazor code that sets `document.title` via JavaScript interop, you can safely replace it with `<PageTitle>`. The new approach is declarative, works during SSR, and integrates with Blazor's lifecycle automatically. The JS-interop approach should be considered a legacy pattern.
