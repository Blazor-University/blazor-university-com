---
title: "Cascading values by name"
date: "2019-07-03"
order: 1
---

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/CascadingValues/CascadingValuesByName)

Specifying a value for a cascading parameter is very simple.
At any point in our Razor HTML mark-up we can create a `CascadingValue` element.
Everything rendered within that element will have access to the value specified.

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
It's not really logical to allow them to set via code on the consumer because the value is effectively owned by the
parent that sets the Cascading value.
