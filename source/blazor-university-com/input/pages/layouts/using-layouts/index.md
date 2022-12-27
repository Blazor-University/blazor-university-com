---
title: "Using a layout"
date: "2019-06-02"
order: 2
---

## Specifying a default layout for the app

The most generic way to specify a layout is to edit the **/Pages/_Imports.razor** file and edit the single line of code to identify a different layout.

```razor
@layout MainLayout
```

The name of the layout is strongly typed. Blazor will only syntax-highlight the code correctly if it there is a layout with the name specified, the compiler will also fail if the identifier is incorrect.

**Note**: Obviously you can alter the **/Shared/MainLayout.razor** file if you just wish to alter the appearance of the existing layout.

## Specifying a default template for an area of the app

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Layouts/UsingALayout)

If your app has separate areas to it, for example an "Admin" area, it is possible to specify a default layout to use for all pages within that area simply by grouping them within their own child-folder that has its own **_Imports.razor** file.

Create a new Blazor client-side app, and then update the navigation menu to contain a link to a new page we'll create shortly.

1. Open the **/Shared/NavMenu.razor** file.
2. Locate the last `<div>` element, it should contain a `<NavLink>` element.
3. Duplicate the `<div>` element.
4. Change the NavLink's `href` attribute to `"admin/users"`.
5. Change the text of the link to **Admin users**.

Next we'll create a very basic page:

1. Expand the **/Pages** node in the Solution explorer.
2. Create a folder named **Admin**.
3. Create a new Razor Component named within the folder named **AdminUsers.razor**.
4. Replace the automatically generated text with the following code:

```razor
@page "/admin/users"

<h2>Users</h2>
```

**Note**: The URL to the page does not have to reflect the folder structure.

Running the app now will present you with an app that has a new menu item named "Admin users". When you click on the item it will show a very basic page that simply says "Users". Next we'll create a default layout for all Admin pages.

1. Create another Razor Component in the **Admin** folder named **\_Imports.razor**.
2. Replace the automatically generated text with the following code:

@layout AdminLayout

At this point there is no file within the app named **AdminLayout**, so you should see a red-line in Visual Studio following the name indicating it cannot be found. Let's fix that by creating the missing layout file:

1. Create another Razor Component in the **Shared** folder named **AdminLayout.razor**.
2. Replace the automatically generated text with the following code:

```razor
@inherits LayoutComponentBase

<h1>Admin</h1>
@Body
```

If you now run the app and click the Admin users link you will see a very basic page consisting of noting more than an `<h1>` and an `<h2>`. We'll address this in the section on [Nested layouts](/layouts/nested-layouts/).

## Specifying a layout explicitly for an individual page

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Layouts/SpecifyingALayoutExplicitly)

So far we've seen that a default layout can be specified in the **/Pages/\_Imports.razor** file. We've also seen that this setting can be overridden for a specific folder by specifying a more specific **\_Imports.razor** in the very folder, or one of its parent folders. (It will use the file closest to the page it is rendering.)

The final level of specifying a template to use is to explicitly specify it in the page itself using the `@layout` directive. For example, we can edit the **AdminUsers.razor** to add the layout directive to it so that it looks like the following:

```razor
@page "/admin/users"
@layout MainLayout

<h2>Users</h2>
```
