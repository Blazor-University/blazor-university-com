---
title: "Defining routes"
date: "2019-07-16"
order: 1
---

To define a route we simply add a `@page` declaration at the top of any component.

```razor
@page "/"

<h1>Hello, world!</h1>

Welcome to your new app.
```

If we open the generated source code for this view we see the `@page` directive compiled to the following code.

```cs
[Microsoft.AspNetCore.Components.LayoutAttribute(typeof(MainLayout))]
[Microsoft.AspNetCore.Components.RouteAttribute("/")]
public class Index : Microsoft.AspNetCore.Components.ComponentBase { }
```

<!--- TODO: Cramer do we care about Blazor 3 anymore? --->
Auto-generated files can be found in **obj\Debug\netcoreapp3.0\Razor\Pages\Index.razor.g.cs** in Blazor 3,
or **obj\Debug\{DotNetVersion}\generated\Microsoft.NET.Sdk.Razor.SourceGenerators\Microsoft.NET.Sdk.Razor.SourceGenerators.RazorSourceGenerator\Pages_Index.razor.g.cs**
in later versions.

Note: As of Blazor V5 these auto-generated files are not saved to disk.
If you wish to re-enable this feature then add the following code to your `csproj` file.

```html
<PropertyGroup>
  <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
</PropertyGroup>
```

The `@page` directive generates a `RouteAttribute` on the component's class.
During start-up, Blazor scans for classes decorated with `RouteAttribute` and builds its route definitions accordingly.

## Route discovery

Route discovery is performed automatically by Blazor in its default project template.
If we look inside the `App.razor` file we'll see the use of a Router component.

```razor
… other code …
    <Router AppAssembly="typeof(Startup).Assembly">
        … other code …
    </Router>
… other code … 
```

The `Router` component scans all classes within the specified assembly that implement `IComponent`,
it then reflects over the class to see if it is decorated with any `RouteAttribute` attributes.
For each `RouteAttribute` it finds,
it parses its URL template string and adds a relationship from the URL to the component into its internal route table.

This means a single component may be decorated with zero, one, or many `RouteAttribute` attributes (`@page` declarations).
A component with zero cannot be reached via a URL,
whereas a component with multiple can be reached via any of the URL templates it specifies.

```razor
@page "/"
@page "/greeting"
@page "/HelloWorld"
@page "/hello-world"

<h1>Hello, world!</h1>
```

Pages may also be defined in [Component libraries](/component-libraries).
