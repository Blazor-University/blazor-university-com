---
title: "Using @typeparam to create generic components"
date: "2020-04-18"
order: 3
---

[![](images/SourceLink-e1567978928628.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/TemplatedComponents/UsingTypeParamToCreateGenericComponents)

If you haven't already, please read the sections [Templating components with RenderFragments](/templating-components-with-renderfragements/)
and [Passing data to a RenderFragment](/templating-components-with-renderfragements/passing-data-to-a-renderfragement/).

Blazor transpiles its `.razor` files to `.cs` files during the build process.
As our `razor` files do not require us to declare the class name (it is inferred from the filename),
we need a way to additionally specify when the class generated should be a generic one.
The `@typeparam` directive allows us to define one or more comma-separated generic parameters on our class.
This can be especially useful when combined with the generic `RenderFragment<T>` class.

## Outlining our generic DataList component

First, we need to create a **DataList.razor** file in the **/Shared** folder and
identify it as a generic class with a single generic parameter called `TItem`.
We'll also add a `[Parameter]` property, expecting an `IEnumerable<TItem>`.

```razor
@typeparam TItem

@code
{
  [Parameter]
  public IEnumerable<TItem> Data { get; set; }
}
```

## Using the generic component

Create a `Person` class with three properties.

```cs
public class Person
{
  public string Salutation { get; set; }
  public string GivenName { get; set; }
  public string FamilyName { get; set; }
}
```

In the `Index` page, create some instances of `Person` that we wish to display to the user,
and pass those to our new `DataList` component.

```razor
<DataList Data=@People/>
@code
{
  private IEnumerable<Person> People;
  protected override void OnInitialized()
  {
    base.OnInitialized();
    People = new Person[]
    {
      new Person { Salutation = "Mr", GivenName = "Bob", FamilyName = "Geldof" },
      new Person { Salutation = "Mrs", GivenName = "Angela", FamilyName = "Rippon" },
      new Person { Salutation = "Mr", GivenName = "Freddie", FamilyName = "Mercury" }
    };
  }
}
```

## Rendering the data in our component using RenderFragment<TItem>

Finally, we'll add a `RenderFragment<TItem>` property and mark it as a `[Parameter]` so that consuming `razor` files may
specify a template for rendering each `TItem` in the `Data` property.

The final `DataList.razor` component mark-up will look like this.

```razor {: .line-numbers}
@typeparam TItem
<ul>
  @foreach(TItem item in Data ?? Array.Empty<TItem>())
  {
    <li>@ChildContent(item)</li>
  }
</ul>
@code
{
  [Parameter]
  public IEnumerable<TItem> Data { get; set; }

  [Parameter]
  public RenderFragment<TItem> ChildContent { get; set; }
}
```

- **Line 1**  
    Specifies this component is generic and has a single generic parameter named `TItem`.
- **Lines 10-11**  
    Declares a `[Parameter]` property named **Data** that is an enumerable property of type `ITem`.
- **Lines 13-14**  
    Declares a `[Parameter]` property named **ChildContent** that is a `RenderFragment<TItem>` -
    so we can pass an instance of `TItem` to it and have it give us some rendered HTML to output.
- **Line 3**  
    Iterates over the `Data` property and for each element renders the `RenderFragment<TItem>` named **ChildContent**
    by passing the current element to it.

## Final source code

### Index.razor

Note: Line 5 has been added to specify the `ChildContext` that we wish to be rendered for each element.
The element itself is passed via the `@context` variable,
so the `RenderFragment<TItem>` is in fact a `RenderFragment<Person>` -
therefore `@context` is a `Person` and therefore we have the benefit of type-safe compilation and IntelliSense.

```razor
@page "/"

<h1>A generic list of Person</h1>
<DataList Data=@People>
  @context.Salutation @context.FamilyName, @context.GivenName
</DataList>

@code
{
  private IEnumerable<Person> People;
  protected override void OnInitialized()
  {
    base.OnInitialized();
    People = new Person[]
    {
      new Person { Salutation = "Mr", GivenName = "Bob", FamilyName = "Geldof" },
      new Person { Salutation = "Mrs", GivenName = "Angela", FamilyName = "Rippon" },
      new Person { Salutation = "Mr", GivenName = "Freddie", FamilyName = "Mercury" }
    };
  }
}
```

### DataList.razor

```razor
@typeparam TItem
<ul>
  @foreach(TItem item in Data ?? Array.Empty<TItem>())
  {
    <li>@ChildContent(item)</li>
  }
</ul>
@code
{
  [Parameter]
  public IEnumerable<TItem> Data { get; set; }

  [Parameter]
  public RenderFragment<TItem> ChildContent { get; set; }
}
```

### Generated output

```html
<h1>A generic list of Person</h1>
<ul>
  <li>Mr Geldof, Bob</li>
  <li>Mrs Rippon, Angela</li>
  <li>Mr Mercury, Freddie</li>
</ul>
```

## Explicitly specifying generic parameter types

Because `razor` files transpile to `C#` classes,
we do not need to specify the type for the generic parameter that `DataList` is expecting
because it is inferred by the compiler from where we set `Data = (Some instance of IEnumerable<TItem>)`.
If ever we do need to specify the generic parameter type explicitly, we can write the following code.

```razor
<SomeGenericComponent TParam1=Person TParam2=Supplier TItem=etc/>
```
