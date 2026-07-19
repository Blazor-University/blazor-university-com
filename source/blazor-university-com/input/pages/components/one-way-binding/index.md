---
title: "One-way binding"
date: "2026-07-16"
order: 2
---

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Components/OneWayBinding)

At this point we have a component displaying inside a page, but the content is constant, and non-interactive.
What we really want is to be able to output content dynamically.

If we alter the contents of MyFirstComponent.razor** we can introduce a private member and
output the value of that member using the `@` symbol.

```razor
<div>
  CurrentCounterValue in MyFirstComponent is @CurrentCounterValue
</div>

@code {
  private int CurrentCounterValue = 42;
}
```

![](images/MyFirstBoundComponent.jpg)

## Receiving values via Parameters

`CurrentCounterValue` within the component always shows the value "42",
but what if we wanted the parent component to tell us which value to show?
To achieve this, create a new component named **MySecondComponent** and copy the mark-up from **MyFirstComponent**,
then change the private member to a public property.

```razor
<div>
  CurrentCounterValue in MySecondComponent is @CurrentCounterValue
</div>

@code {
  public int CurrentCounterValue { get; set; }
}
```

Edit the **Counter.razor** page in **Components/Pages/**, add a `MySecondComponent` component, and set its `CurrentCounterValue`, like so:

```razor
<MySecondComponent CurrentCounterValue=@currentCount/>
```

Attempting to build the app will now give a compile-time error.

> The component parameter 'CurrentCounterValue' is not defined by 'MySecondComponent'.

This tells us clearly what is missing.
To add a parameter to our component we must decorate our component's property with a `[Parameter]` attribute.

```razor
<div>
  CurrentCounterValue in MySecondComponent is @CurrentCounterValue
</div>

@code {
  [Parameter]
  public int CurrentCounterValue { get; set; }
}
```

This informs Blazor we want a parameter on our component that is settable via what looks like an HTML attribute.

Whenever the parent component is rerendered, Blazor will also rerender any child component it provides parameter values to. This ensures the child component is rerendered to represent any possible change
in the state passed down to the component via a `[Parameter]` decorated property.

If we run our application again and navigate to the Counter page, we'll see that whenever the `currentCount` in the
Counter page changes it will push that change down to our embedded component via its `CurrentCounterValue` property.

![](images/OneWayParameterBinding.gif)

**Note**: Parameters must be `public` properties.

Optionally, we can decorate a parameter with `[EditorRequired]` to indicate that the consuming component must supply a value for it. The compiler will issue a warning if the parameter is omitted.

```razor
[EditorRequired, Parameter]
public int CurrentCounterValue { get; set; }
```

## Render mode requirement

The dynamic one-way binding described above requires an interactive render mode on the parent component. Without one, the parent renders statically and the parameter value will only be applied once on initial render. We can assign an interactive render mode using the `@rendermode` directive:

```razor
<MySecondComponent @rendermode="InteractiveServer" CurrentCounterValue=@currentCount/>
```

See the [Directives](literals-expressions-and-directives/directives) section for more details on render modes.

Before continuing to learn how [two-way binding](/components/two-way-binding/) works, we first need to learn about [Component Events](/components/component-events/) and [Directives](literals-expressions-and-directives/directives).
