---
title: "Outlets"
date: "2026-07-16"
order: 3
---

A layout wraps a page's content inside a shared template. The page fills in the `@Body` placeholder, and the layout provides the surrounding structure. But what about content that needs to travel in the other direction? What if a page needs to set the browser's title bar, inject a `<meta>` tag into the document `<head>`, or render something into a region that belongs to the layout (such as a toolbar or a sidebar)?

Blazor solves these problems with **outlets**: projection points that let a child component contribute content to a parent or sibling location in the render tree. Outlets give us two-way content projection.

There are two kinds of outlet in Blazor:

- **Head outlets** let components control the document `<head>`. Any component anywhere in the tree can set the page title or emit `<meta>`, `<link>`, `<style>`, or `<script>` tags via `<PageTitle>` and `<HeadContent>`. The host page places a `<HeadOutlet />` component where those elements should be rendered.

- **Section outlets** (introduced in .NET 8) let us define arbitrary named regions. A layout can declare a `<SectionOutlet>` and any page or component using that layout can inject content into it with `<SectionContent>`. This is the generalised version of the same pattern: outlet declares the slot, content fills it.

In fact, `PageTitle` and `HeadContent` are themselves built on top of the Sections API. Blazor uses `SectionOutlet` internally to project head elements into the right place. Understanding sections gives us insight into how the head system works under the hood.

The next pages cover each outlet type in detail:

- [Head outlets and content](/outlets/head-outlets-and-content/) shows how to control the document head.
- [Section outlets and content](/outlets/section-outlets-and-content/) explains the generalised Sections API.
