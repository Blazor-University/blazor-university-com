---
title: "Creating a Blazor layout"
date: "2026-07-16"
order: 1
---

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Layouts/CreatingALayout)

Any content you intend to act as a layout template for pages must descend from the `LayoutComponentBase` class.
To indicate a base component you add the following to the top of the `.razor` file.
```razor
@using <namespace of base class if necessary>
@inherits <name of base class>
```

In the default Blazor project template, the application layout lives at `Components/Layout/MainLayout.razor`.

To indicate where you want the content of your page to appear you simply output the contents of `Body`. `@Body` is a `RenderFragment` provided by the `LayoutComponentBase` base class; it is replaced at runtime with the content of the page being rendered.

```razor
@inherits LayoutComponentBase

<div class="main">
  <header>
    <h1>This is the header</h1>
  </header>

  <div class="content">
    @Body
  </div>

  <footer>
    This is the footer
  </footer>
</div>
```

Layouts often include a `<HeadOutlet />` component to render elements placed via `<PageTitle>` or `<HeadContent>` in individual pages. A typical layout template also references a companion scoped CSS file. For example, if the layout is `MainLayout.razor`, its scoped styles are placed in `MainLayout.razor.css` and are automatically applied to the layout's HTML output.
