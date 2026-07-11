---
title: "Creating a component"
date: "2019-06-06"
order: 1
---

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Components/CreatingAComponent)

Create a new Blazor app with interactivity set to WebAssembly.

In the **Components** folder create a file named **MyFirstComponent.razor** and enter the following mark-up.

```razor
<div>
    <h2>This is my first component</h2>
</div>
```

Now edit the **Index.razor** file and add the following

```razor
<MyFirstComponent/>
```

If you create your component elsewhere you will need to either fully qualify that component
name with a namespace like so `MyFirstBlazorApp.Client.MyNewFolder.MyFirstComponent`, or
Or edit **/_Imports.razor** and add `@using MyFirstBlazorApp.Client.MyNewFolder>`. The using statements
here are cascaded into all Razor views.

```razor
@page "/"

<h1>Hello, world!</h1>
<MyFirstComponent/>

Welcome to your new app.
```

Now run the app and we'll see the following.

![](images/ThisIsMyFirstComponent.jpg)
