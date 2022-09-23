---
title: "Owning multiple dependencies: The right way"
date: "2020-05-30"
order: 3
---

In the [previous section](https://blazor-university.com/dependency-injection/component-scoped-dependencies/owning-multiple-dependencies-the-wrong-way/),
we saw the wrong way to inject multiple owned dependencies into a component. This section will demonstrate the correct way to approach the problem.

As mentioned previously, the `OwningComponentBase<T>` class component will create its own dependency container and resolve
an instance of `T` within that container so the instance of `T` is private to our component.

If we need our component to privately own instances of multiple types of dependency then we have to do a little more work.
To achieve this, we need to use the non-generic `OwningComponentBase` class.
Like the generic version,
this component will create its own dependency container that will exist for the lifetime of the component.
However, instead of actually resolving any dependencies for us,
it will give us access to its dependency container so we can resolve instances of whichever types we need.

## Example

[![](images/SourceLink-e1567978928628.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/DependencyInjection/OwningMultipleDependenciesTheRightWay)

First, create a new Blazor application. Then, as we have done before,
we shall create some classes we can inject that will use a state member to keep track of how many instances
of the class have been created.

Create the following interfaces

```cs
public interface IOwnedDependency1
{
  public int InstanceNumber { get; }
}

public interface IOwnedDependency2
{
  public int InstanceNumber { get; }
}
```

Then create classes that implement those interfaces.
I'll just show the code for the first class, the second class will be identical.

```cs
public class OwnedDependency1 : IOwnedDependency1
{
  private static volatile int PreviousInstanceNumber;

  public int InstanceNumber { get; }
  public OwnedDependency1()
  {
    InstanceNumber =
      System.Threading.Interlocked.Increment(ref PreviousInstanceNumber);
  }
}
```

Register the interfaces + their implementing classes as `Scoped`
(see [Comparing dependency scopes](https://blazor-university.com/dependency-injection/dependency-lifetimes-and-scopes/comparing-dependency-scopes/)
if you need to be reminded how).

Next, edit the **Index.razor** page so the user of our app can toggle a component by clicking a checkbox.

```razor
@page "/"

<input id="show-component" type=checkbox @bind=ShowComponent />
<label for="show-component">Show component</label>

@if (ShowComponent)
{
  <MyOwningComponent />
}

@code
{
  bool ShowComponent = false;
}
```

When `ShowComponent` is true, our mark-up will create an instance of `MyOwningComponent` and render it.
Next, we'll create `MyOwningComponent`.

### OwningComponentBase

In the **Shared** folder, create a new Razor component named **MyOwningComponent**.
We'll descend this component from `OwningComponentBase`.

```razor
@inherits OwningComponentBase

Then create some class fields to hold our dependencies.

@code
{
  private IOwnedDependency1 OwnedDependency1;
  private IOwnedDependency2 OwnedDependency2;
}
```

### Resolving owned dependencies

The private dependency container that `OwningComponentBase` creates is made available to us via its `ScopedServices` property.

protected IServiceProvider ScopedServices { get; }

We can use this `IServiceProvider` to resolve instances of dependencies from the private
dependency container that our component owns.

```razor {: .line-numbers}
@inherits OwningComponentBase
@using Microsoft.Extensions.DependencyInjection

<div>
  OwnedDependency1.InstanceNumber = @OwnedDependency1.InstanceNumber
</div>
<div>
  OwnedDependency2.InstanceNumber = @OwnedDependency2.InstanceNumber
</div>

@code
{
  private IOwnedDependency1 OwnedDependency1;
  private IOwnedDependency2 OwnedDependency2;

  protected override void OnInitialized()
  {
    OwnedDependency1 =
      ScopedServices.GetService<IOwnedDependency1>();
    OwnedDependency2 =
      ScopedServices.GetService<IOwnedDependency2>();
  }
}
```

- **Line 1**  
    Descends from `OwningComponentBase` to give us our own private dependency container.
- **Line 2**  
    Uses the DependencyInjection namespace so we can use the `GetService<T>` extension method on `IServiceProvider`.
- **Lines 19 & 21**  
    Uses the `OwningComponentBase.ScopedServices` property to resolve instances of the dependencies our component requires.
- **Lines 6 & 9**  
    Display the instance numbers of the dependencies that were created for us.

### Running the example

If we run the example app and tick the checkbox, we will see the following output.

- OwnedDependency1.InstanceNumber = 1
- OwnedDependency2.InstanceNumber = 1

Untick the checkbox to allow our component to be removed,
and then tick it again to have Blazor create a new instance of `MyOwningComponent`.
The rendered output should now read as follows.

- OwnedDependency1.InstanceNumber = 2
- OwnedDependency2.InstanceNumber = 2

This shows that both dependencies we resolve in the `OnInitialized` method of our component are new
instances each time our component is created.

## Dependent lifetimes

The `OwningComponentBase` class implements the `IDisposable` interface.
When any component descending from `OwningComponentBase` is no longer rendered,
Blazor will execute the `Dispose` method on `OwningComponentBase`.

The `Dispose` method on the component will call `Dispose` on the private dependency container it owns.
In turn, any object instances this container created that implement `IDisposable` will also have their `Dispose` method executed.

To demonstrate this behavior, make the following changes to our application.

First, override `Dispose(bool isDisposing)` on our component and have it log some output when it is disposed.

```cs
public void Dispose()
{
  System.Diagnostics.Debug.WriteLine("Disposing " + GetType().Name);
}
```

Then, for each of our dependency classes (`OwnedDependency1` and `OwnedDependency2`) have them implement `IDisposable` and, again, have them log some output when `Dispose` is executed.

```cs
  public class OwnedDependency1 : IOwnedDependency1, IDisposable
  {
    ... Other code omitted for brevity ...

    public void Dispose()
    {
      System.Diagnostics.Debug.WriteLine($"Created {GetType().Name} instance {InstanceNumber}");
    }
  }
```

We could also add some logging in the constructors of our classes.

Running the application and toggling the checkbox now will output log text similar to the following.

- Created MyOwningComponent
- Created OwnedDependency1 instance 1
- Created OwnedDependency2 instance 1

- Disposing OwnedDependency2 instance 1
- Disposing OwnedDependency1 instance 1
- Disposing MyOwningComponent

- Created MyOwningComponent
- Created OwnedDependency1 instance 2
- Created OwnedDependency2 instance 2

- Disposing OwnedDependency2 instance 2
- Disposing OwnedDependency1 instance 2
- Disposing MyOwningComponent

## Conclusion

Descend from `OwningComponentBase<T>` when you need only a single dependency to be owned by your component,
and descend from the non-generic `OwningComponentBase` class when you need your component to own multiple dependencies.

Although the process of resolving instances of your component's dependencies is a manual process,
there is no need to dispose of any dependencies created as the component's dependency container will
dispose of them when `OwningComponentBase.Dispose` is executed.
