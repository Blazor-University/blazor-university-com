---
title: "Using a layout"
date: "2026-07-16"
order: 2
---

## Specifying a default layout for the app

The application's default layout for all pages is specified in `/Components/Routes.razor` in the following line of code.

```razor
<RouteView RouteData="routeData"
           DefaultLayout="typeof(Layout.MainLayout)" />
```

The name of the layout is strongly typed. Blazor will only syntax-highlight the code correctly
if there is a layout with the name specified. The compiler will also fail if the identifier is incorrect.

_Note: If you just wish to alter the appearance of the existing layout you should alter the
`/Components/Layout/MainLayout.razor` file._

## Using _Imports.razor
`_Imports.razor` is a convention used by Blazor to specify defaults. These can be `@using` declarations for
specifying commonly imported namespaces or, in this case, a default `@layout` to use.

1. Edit `/Components/Routes.razor`.
1. Find the `<RouteView>` component.
1. Remove the attribute `DefaultLayout="typeof(Layout.MainLayout)"`.

If you now run the app it will look pretty awful, because there is no layout wrapping any of the pages. To
fix this, follow these steps:

1. Expand the `/Components/Pages` folder.
1. Create a new file `_Imports.razor`.
1. Add the following content
```razor
@layout MainLayout
```

Now run the app again and note the layout is back.

**WARNING**: Make sure you edit the `_Imports.razor` file inside `/Components/Pages` and **not** the one directly inside `/Components`. The reason is `/Components/Layout/MainLayout.razor` will inherit
all code defined inside `/Components/_Imports.razor`, meaning it will have itself as a layout. Blazor
detects circular layout references and throws an `InvalidOperationException` at runtime rather than
hanging.

## Specifying a default template for a sub area of the app

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Layouts/UsingALayout)

If your app has separate areas to it, for example an "Admin" area, it is possible to specify a
default layout to use for all pages within that area simply by grouping them within their own
child-folder that has its own `_Imports.razor` file.

We have seen this previously in the `Using _Imports.razor` section earlier.
During Razor compilation, Blazor walks up the folder hierarchy from the page's folder looking
for `_Imports.razor` files. When found, their contents are merged
into the top of the generated Razor class. This walk continues to the parent folder, then the grandparent,
and so on.

All the imported directives are combined before the Razor source generator produces the compiled class. When
a directive can only have a single value ('@layout', `@inherits`), the value defined in the
`_Imports.razor` closest to the page wins.

![_Imports](images/imports.jpg)

### Example

Create a new Blazor Web App, and then update the navigation menu to contain a link to a new page we'll
create shortly.

1. Open the `/Components/Layout/NavMenu.razor` file.
1. Locate the last `<div>` element, it should contain a `<NavLink>` component.
1. Duplicate the `<div>` element.
1. Change the NavLink's `href` attribute to `"admin/users"`.
1. Change the text inside the `<NavLink>` to `Admin users`.

Next we'll create a very basic page

1. Expand the `/Components/Pages` node in the Solution Explorer.
2. Create a folder named `Admin`.
3. Create a new file within the folder named `AdminUsers.razor`.

```razor
@page "/admin/users"

<h2>Users</h2>
```

_Note: The URL to the page does not have to reflect the folder structure._

Running the app now will present you with an app that has a new menu item named `Admin users`.
When you click on the item it will show a very basic page that simply says "Users".
Next we'll create a default layout for all Admin pages.

1. Create another new file in the `/Components/Pages/Admin` folder named `_Imports.razor`.
1. Enter the following code:

```razor
@layout AdminLayout
```

At this point there is no file within the app named `AdminLayout`, so you should see a red-line in
Visual Studio beneath the name indicating it cannot be found. You can fix this by creating an
`AdminLayout.razor` in the `/Components/Layout/` folder.

```razor
@inherits LayoutComponentBase

<h1>Admin</h1>
@Body
```

If you now run the app and click the Admin users link you will see an awful user experience consisting
merely of an `h1` and an `h2`. We will fix this in the section on [Nested layouts](/layouts/nested-layouts/),
but for now we'll use it as an exercise in how to explicitly specify a layout from the page itself.

## Specifying a layout explicitly for an individual page

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Layouts/SpecifyingALayoutExplicitly)

So far we've seen that a default layout can be specified in the `/Components/Pages/_Imports.razor` file, and
we've also seen that this setting can be overridden by Blazor finding a more specific `_Imports.razor`
file in a folder closer to the page it is rendering (`/Components/Pages/Admin/_Imports.razor`).

The most explicit level of specifying a template to use is to literally specify it in the page
itself using the `@layout` directive.

```razor
@page "/admin/users"
@layout MainLayout

<h2>Users</h2>
```

Running the app again and clicking on the `Admin users` link will now show basic page
using the app's standard layout.

It is also possible to opt out of any layout entirely on a per-page basis by specifying `@layout null`. This is useful for pages that should render without any surrounding template, such as a full-screen login page.
