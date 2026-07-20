---
title: "Creating a component"
date: "2026-07-16"
order: 1
---

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Components/CreatingAComponent)

Create a new Blazor app with interactivity set to WebAssembly by running:

```sh
dotnet new blazor -int WebAssembly
```

In the **Components** folder create a file named **MyFirstComponent.razor** and enter the following mark-up.

```razor
<div>
    <h2>This is my first component</h2>
</div>
```

Now edit the **Home.razor** file in **Components/Pages/** and add the following

```razor
<MyFirstComponent/>
```

If you create your component elsewhere you will need to either fully qualify that component
name with a namespace like so `MyFirstBlazorApp.Client.MyNewFolder.MyFirstComponent`, or
edit the **Components/_Imports.razor** file and add `@using MyFirstBlazorApp.Client.MyNewFolder`. The using statements
here are cascaded into all Razor views.

```razor
@page "/"

<MyFirstComponent/>
```

Now run the app and we will see the following.

## Component naming conventions

Component file names should use PascalCase, matching the component class name. For example, a component defined in **MyFirstComponent.razor** becomes the `MyFirstComponent` class. We can also use a code-behind approach by creating a **MyFirstComponent.razor.cs** partial class file alongside the Razor file.

![](images/ThisIsMyFirstComponent.jpg)
