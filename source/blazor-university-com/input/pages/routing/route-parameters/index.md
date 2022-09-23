---
title: "Route parameters"
date: "2019-04-27"
order: 2
---

[![GitHub](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Routing/CapturingAParameterValue)

So far we've seen how to link a static URL to a Blazor component.
Static URLs are only useful for static content, if we want the same component to render different views based on
information in the URL (such as a customer ID) then we need to use route parameters.

A route parameter is defined in the URL by wrapping its name in a pair of `{` braces `}` when adding a component's
`@page` declaration.

```razor
@page "/customer/{CustomerId}
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
  public string CustomerId { get; set; }
}
```

Note that when a navigation is made to a new URL that resolves to the same type of component as the current page,
the component will not be destroyed before navigation and the `OnInitialized*` lifecycle methods will not be executed.
The navigation is simply seen as a change to the component's parameters.
