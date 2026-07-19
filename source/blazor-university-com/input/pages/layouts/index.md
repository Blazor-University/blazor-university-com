---
title: "Layouts"
date: "2026-07-16"
order: 2
---

This section covers [creating a Blazor layout](/layouts/creating-a-blazor-layout/), [using layouts](/layouts/using-layouts/), and [nested layouts](/layouts/nested-layouts/).

A Blazor layout is the same as a Razor layout in ASP.NET MVC/Razor Pages.

Almost every website on the Web has a template that is used either throughout the website (branding at the top of the page, copyright at the bottom), or throughout specific sub-sections of a website (such as a specific menu structure on the Admin pages of the site).

This is achieved by creating a view that acts as an HTML wrapper around the current page's content. The template contains a place-holder (in Blazor, a `RenderFragment` named `@Body`) indicating where the wrapped page's content should appear.

## The layout
```html
<h1>This is the start of my reusable layout</h1>

<div class="Content">
  -- Some kind of indicator to specify the page's
  -- content will go here
</div>

<footer>
  This is the end of the layout
</footer>
```

In modern Blazor projects, the default layout is wired up in `Components/Routes.razor` via `<RouteView DefaultLayout="typeof(MainLayout)" />`.

## Pages that use the layout
Individual pages can then optionally specify a single layout in which it would like its content to be wrapped.

```html
 -- Some way of indicating which template to
 -- wrap this page's content in

<h1>This is the content of your embedded page</h1>
```

## The rendered HTML
The resulting HTML would look like this

```html
<h1>This is the start of my reusable layout</h1>

<div class="Content">
  <h1>This is the content of your embedded page</h1>
</div>

<footer>
  This is the end of the layout
</footer>
```

## Why use layouts?

This approach is taken for a few reasons.

**Reusability.** Common page elements such as headers, navigation menus, and footers are defined once in a layout and reused across many pages. When a change is needed, only the layout is updated, and every page using it reflects the change automatically.

**Consistency.** By wrapping pages in a shared layout, we guarantee a uniform appearance across the application. Every page gets the same header, footer, and surrounding structure without each page having to replicate it.

**DRY (Don't Repeat Yourself).** Without layouts, every page would need to repeat the same boilerplate HTML for structural elements. Layouts eliminate this duplication, reducing the risk of inconsistencies and making maintenance simpler.

