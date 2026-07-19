---
title: "Cascading values by name"
date: "2026-07-16"
order: 1
---

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/CascadingValues/CascadingValuesByName)

Specifying a value for a cascading parameter is very simple.
At any point in our Razor HTML mark-up we can create a `CascadingValue` element.
Everything rendered within that element will have access to the value specified.

> **Prerequisite:** The interactive example on this page uses checkboxes with `@bind-value`, which requires an interactive render mode such as InteractiveServer or InteractiveWebAssembly. Without an interactive render mode, event binding will not function. See [Render modes](/render-modes) for details.

```razor
@page "/"

<h1>Toggle the options</h1>
<input @bind-value=FirstOptionValue type="checkbox" /> First option
<br />
<input @bind-value=SecondOptionValue type="checkbox" /> Second option
<br />

<CascadingValue Name="FirstOption" Value=@FirstOptionValue>
  <CascadingValue Name="SecondOption" Value=@SecondOptionValue>
    <FirstLevelComponent />
  </CascadingValue>
</CascadingValue>

@code {
  bool FirstOptionValue;
  bool SecondOptionValue;
}

Consuming the value is just as simple.
Any component, no matter how deeply nested it is inside the `CascadingValue` element, can access the value with a 
property decorated with the `CascadingParameter` attribute.

<ul>
  <li>FirstOption = @FirstOption</li>
  <li>SecondOption = @SecondOption</li>
</ul>

@code {
  [CascadingParameter(Name="FirstOption")]
  private bool FirstOption { get; set; }

  [CascadingParameter(Name="SecondOption")]
  private bool SecondOption { get; set; }
}
```

Note that the name of our property that consumes the value is irrelevant.
Blazor will not look for a property with the same name specified in the `CascadingValue` element;
we are free to name our property anything we like, it's actually the `Name` on the `CascadingParameterAttribute` that
identifies which cascading value should be injected.

It is good practice to set the visibility of properties that act as Cascading parameters to `private`.
It is not really logical to allow them to set via code on the consumer because the value is effectively owned by the
parent that sets the Cascading value.

## IsFixed and root-level registration

By default, Blazor re-renders every component between a `CascadingValue` and its consumers whenever the value changes. If we know the value will never change, we can set `IsFixed="true"` on the `CascadingValue` element to skip change tracking and improve render performance:

```razor
<CascadingValue Name="Theme" Value="@CurrentTheme" IsFixed="true">
  <Router ... />
</CascadingValue>
```

In .NET 8 and later, we can also register cascading values at the application root using `AddCascadingValue` in our `Program.cs`:

```cs
builder.Services.AddCascadingValue("FirstOption", _ => false);
builder.Services.AddCascadingValue("SecondOption", _ => false);
```

Root-level cascading values feed every component in the application, just as if they were wrapped in a `CascadingValue` element at the top of the render tree. The `IsFixed` option is available through the same registration:

```cs
builder.Services.AddCascadingValue("FirstOption", _ => false, isFixed: true);
```

## Warning on mistyped names

If the `Name` in `[CascadingParameter(Name = "...")]` does not match any `CascadingValue` in the render tree, Blazor does **not** throw an exception. Instead, the property retains its default value, which can make the bug difficult to diagnose. Double-check the name spelling when a cascaded value does not appear to arrive.
