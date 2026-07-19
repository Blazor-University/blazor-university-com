---
title: "Defining routes"
date: "2026-07-16"
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

In .NET 6 and later these auto-generated files are emitted by the Razor source generator and are not saved to disk by default.
If you wish to re-enable this feature then add the following code to your `csproj` file.

```xml
<PropertyGroup>
  <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
</PropertyGroup>
```

The `@page` directive generates a `RouteAttribute` on the component's class.
During start-up, Blazor scans for classes decorated with `RouteAttribute` and builds its route definitions accordingly.

## Route discovery

Route discovery is performed automatically by Blazor in its default project template.
If we look inside the `Routes.razor` file we will see the Router component.

```razor
<Router AppAssembly="typeof(Program).Assembly">
    <Found Context="routeData">
        <RouteView RouteData="routeData" DefaultLayout="typeof(MainLayout)" />
        <FocusOnNavigate RouteData="routeData" Selector="h1" />
    </Found>
    <NotFound>
        <PageTitle>Not found</PageTitle>
        <LayoutView Layout="typeof(MainLayout)">
            <p>Sorry, there is nothing at this address.</p>
        </LayoutView>
    </NotFound>
</Router>
```

The `Router` component scans all classes within the specified assembly that implement `IComponent`,
it then reflects over the class to see if it is decorated with any `RouteAttribute` attributes.
For each `RouteAttribute` it finds,
it parses its URL template string and adds a relationship from the URL to the component into its internal route table.

Note that route matching is case-insensitive in Blazor. A component with `@page "/HelloWorld"` matches `/helloworld`, `/HELLOWORLD`, or any other casing variation.

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
Components in external libraries are not discovered from `AppAssembly` alone. We must pass the library's assembly to the `AdditionalAssemblies` parameter:

```razor
<Router
    AppAssembly="typeof(Program).Assembly"
    AdditionalAssemblies="new[] { typeof(SomeLibrary.SomeComponent).Assembly }">
    ...
</Router>
```
