---
title: "Route parameters"
date: "2026-07-16"
order: 2
---

[![GitHub](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Routing/CapturingAParameterValue)

So far we've seen how to link a static URL to a Blazor component.
Static URLs are only useful for static content, if we want the same component to render different views based on
information in the URL (such as a customer ID) then we need to use route parameters.

A route parameter is defined in the URL by wrapping its name in a pair of `{` braces `}` when adding a component's
`@page` declaration.

```razor
@page "/customer/{CustomerId}"
```

## Capturing a parameter value

Capturing the value of a parameter is as simple as adding a property with the same name and decorating it with a
`[Parameter]` attribute.

```razor
@page "/"
@page "/customer/{CustomerId}"

<h1>
  Customer:
  @if (string.IsNullOrEmpty(CustomerId))
  {
    @:None
  }
  else
  {
    @CustomerId
  }
</h1>
<h3>Select a customer</h3>
<ul>
  <li><a href="/customer/Microsoft">Microsoft</a></li>
  <li><a href="/customer/Google">Google</a></li>
  <li><a href="/customer/IBM">IBM</a></li>
</ul>

@code {
  [Parameter]
  public string? CustomerId { get; set; }
}
```

All route parameters are captured as strings by default. The `[Parameter]` property must be of type `string` or a nullable string such as `string?` unless a route constraint is applied (see [Constraining route parameters](/routing/constraining-route-parameters/)). Route matching is also case-insensitive; `/customer/Microsoft` and `/customer/microsoft` resolve to the same component.

Blazor also supports catch-all route parameters using the `{*VariableName}` syntax. A catch-all parameter captures the remainder of the URL path, including forward slashes. For example, `@page "/blog/{*slug}"` would match `/blog/2026/07/hello-world` and set the `slug` parameter to `2026/07/hello-world`.

Note that when a navigation is made to a new URL that resolves to the same component type as the current page in an interactive render mode, the component will not be destroyed before navigation and the `OnInitialized*` lifecycle methods will not be executed. The navigation is simply seen as a change to the component's parameters. In Static Server Rendering (Static SSR) mode, however, each navigation is a full HTTP request that destroys and recreates the component, so `OnInitialized*` runs on every request.
