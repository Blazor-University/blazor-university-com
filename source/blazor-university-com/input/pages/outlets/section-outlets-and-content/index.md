---
title: "Section outlets and content"
date: "2026-07-16"
order: 3
---

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Outlets/SectionOutletsAndContent)

The `PageTitle` and `HeadContent` components solve a specific problem: projecting content into the document `<head>`. But what if we want to project content into a custom region elsewhere in the component tree, such as a toolbar in the layout, a sidebar, or a page-specific action bar? For these scenarios, Blazor provides the Sections API, available in the `Microsoft.AspNetCore.Components.Sections` namespace.

The API consists of two components:

- **`SectionOutlet`** declares a named slot where content can appear.
- **`SectionContent`** supplies the content that fills that slot.

## Declaring a SectionOutlet

A `SectionOutlet` is placed where we want the projected content to appear. It is most commonly placed in a layout, but it can appear in any component.

```razor
@* MainLayout.razor *@
@inherits LayoutComponentBase

<div class="sidebar">
  <SectionOutlet SectionName="Sidebar" />
</div>

<div class="content">
  @Body
</div>
```

The `SectionName` parameter is a string that identifies this outlet. Any `SectionContent` component with a matching `SectionName` will render its children at this location.

## Filling the slot with SectionContent

A page (or any component) can inject content into the outlet using `SectionContent` with the same name.

```razor
@* ProductsPage.razor *@
@page "/products"

<SectionContent SectionName="Sidebar">
  <h3>Filter Products</h3>
  <input @bind="searchTerm" placeholder="Search..." />
  <ul>
    <li><a href="/products?category=electronics">Electronics</a></li>
    <li><a href="/products?category=books">Books</a></li>
  </ul>
</SectionContent>

<h1>Products</h1>
@* ... product listing ... *@
```

When the `ProductsPage` renders, its `<SectionContent>` is projected into the layout's sidebar. The page authors do not need to know where the outlet lives; they only need to know its name.

## Matching by SectionId

String-based names are convenient but can collide. If two libraries use the same section name, they will interfere with each other. For a more robust contract, we can match sections by a static object reference using the `SectionId` parameter.

First, define a static field to act as the section identity.

```cs
public static class AppSections
{
  public static readonly object Sidebar = new();
  public static readonly object Toolbar = new();
}
```

Then reference this field in both the outlet and the content.

```razor
@* In the layout *@
<SectionOutlet SectionId="AppSections.Sidebar" />

@* In a page *@
<SectionContent SectionId="AppSections.Sidebar">
  @* content *@
</SectionContent>
```

Because the match is by object identity, there is no risk of accidental name collisions.

## Most recently rendered wins

If multiple `SectionContent` components target the same outlet, the one rendered last wins. This is the same last-rendered-wins behavior we saw with `PageTitle` and `HeadContent`. It means a layout can provide default section content by placing a `SectionContent` before `@Body`, and individual pages can override it with their own `SectionContent` rendered after the layout's.

```razor
@* MainLayout.razor *@
@inherits LayoutComponentBase

<SectionContent SectionName="Toolbar">
  <span>Default toolbar</span>
</SectionContent>
<SectionOutlet SectionName="Toolbar" />

@Body
```

Any page that renders its own `<SectionContent SectionName="Toolbar">` will override the default, because the page's content renders after the layout's.

## Default content

A `SectionOutlet` can also include child content that acts as a fallback when no `SectionContent` provides anything.

```razor
<SectionOutlet SectionName="Sidebar">
  <p>No sidebar content provided.</p>
</SectionOutlet>
```

The child content of `SectionOutlet` is only rendered when no `SectionContent` targets that section. As soon as a `SectionContent` appears (even from a child component), the default content is replaced.

## Important caveat: parameter and cascading value scope

The content inside a `SectionContent` component executes in the context of where the `SectionContent` is declared, not where the `SectionOutlet` renders it. This means:

- Parameters bound to the section content are evaluated in the declaring component.
- Cascading values are inherited from the declaring component, not from the outlet's location.
- `@ref` and scoped services follow the declaring component's scope.

In practice, this is usually what we want. A page knows what data its sidebar content needs and can pass that data directly. But it is worth remembering if you are trying to make a section outlet that provides layout-level services: the section content cannot directly receive cascading values from the layout. If the layout needs to share state with section content, use a scoped service or pass the state explicitly.

The Sections API is the generalised mechanism that makes `PageTitle` and `HeadContent` work behind the scenes. Understanding it lets us create our own content projection points anywhere in the component tree.
