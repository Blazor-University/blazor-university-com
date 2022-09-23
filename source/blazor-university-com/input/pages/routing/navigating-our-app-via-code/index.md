---
title: "Navigating our app via code"
date: "2019-07-16"
order: 7
---

[![GitHub](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Routing/NavigatingViaCode)

Access to browser navigation from Blazor is provided via the `NavigationManager` service.
This can be injected into a Blazor component using `@inject` in a razor file, or the `[Inject]` attribute in a CS file.

The `NavigationManager` service has two members that are of particular interest; `NavigateTo` and `LocationChanged`.

The `LocationChanged` event will be explained in more detail in [Detecting navigation events](/routing/detecting-navigation-events/).

## The NavigateTo method

The `NavigationManager``.NavigateTo` method enables C# code to control the browser's URL.
As with intercepted navigations, the browser does not actually navigate to a new URL.
Instead the URL in the browser is replaced, and the previous URL is inserted into the browser's navigation history,
but no request is made to the server for the content of the new page.
Navigations made via `NavigateTo` will cause the `LocationChanged` event to trigger,
passing the new URL and `false` for `IsNavigationIntercepted`.

For this example we'll alter the standard Blazor template again.
We'll use what we learned previously in [Route parameters](/routing/route-parameters/) and [Optional route parameters](/routing/optional-route-parameters/).

First, delete the `Index.razor` and `FetchData.razor` pages,
and remove the links to those pages inside the `NavMenu.razor` file.
Also in NavMenu, change the `href` of the link to counter to `href=""`, because we are going to make it the default page.

Edit `Counter.razor` and give it two routes, `"/"` and `"/counter/{CurrentCount:int}"`

```razor
@page "/"
@page "/counter/{CurrentCount:int}"
```

We'll also need to change the `currentCount` field so it is a property with a getter and setter, and decorate it as a `[Parameter]`.
Note that it has been renamed from _camelCase_ to _PascalCase_ too.

```razor
[Parameter]
public int CurrentCount { get; set; }
```

We now have a counter page that can be accessed either simply be reaching the main page of the app,
or by specifying `/counter/X`, where _X_ is an integer value.

The `NavigationManager` was injected into our `CounterBase` class, and so is accessible in our `Counter.razor` file.

```razor
@code {
  [Parameter]
  public int CurrentCount { get; set; }

  bool forceLoad;

  void AlterBy(int adjustment)
  {
    int newCount = CurrentCount + adjustment;
    UriHelper.NavigateTo("/counter/" + newCount, forceLoad);
  }
}
```

We'll call the `AlterBy` method from two buttons, one to increment the `CurrentCount` and one to decrement it.
There is also an option the user will be able to select, `forceLoad`,
which will set the relevant parameter in the call to `NavigateTo` so we can see the difference.
The whole file should eventually look like this:

```razor
@page "/"
@page "/counter/{CurrentCount:int}"
@implements IDisposable
@inject NavigationManager NavigationManager

<h1>Counter value = @CurrentCount</h1>

<div class="form-check">
  <input @bind=@forceLoad type="checkbox" class="form-check-input" id="ForceLoadCheckbox" />
  <label class="form-check-label" for="ForceLoadCheckbox">Force page reload on navigate</label>
</div>

<div class="btn-group" role="group">
  <button @onclick=@( () => AlterBy(-1) ) class="btn btn-primary">-</button>
  <input value=@CurrentCount readonly class="form-control" />
  <button @onclick=@( () => AlterBy(1) ) class="btn btn-primary">+</button>
</div>
<a class="btn btn-secondary" href="/Counter/0">Reset</a>
<p>
  <em>Page redirects to ibm.com when count hits 10!</em>
</p>

@code {
  [Parameter]
  public int CurrentCount { get; set; }

  bool forceLoad;

  void AlterBy(int adjustment)
  {
    int newCount = CurrentCount + adjustment;

    if (newCount >= 10)
      NavigationManager.NavigateTo("https://ibm.com");

    NavigationManager.NavigateTo("/counter/" + newCount, forceLoad);
  }

  protected override void OnInitialized()
  {
    // Subscribe to the event
    NavigationManager.LocationChanged += LocationChanged;
    base.OnInitialized();
  }

  private void LocationChanged(object sender, LocationChangedEventArgs e)
  {
    string navigationMethod = e.IsNavigationIntercepted ? "HTML" : "code";
    System.Diagnostics.Debug.WriteLine($"Notified of navigation via {navigationMethod} to {e.Location}");
  }

  void IDisposable.Dispose()
  {
    // Unsubscribe from the event when our component is disposed
    NavigationManager.LocationChanged -= LocationChanged;
  }
}
```

Clicking the `-` or `+` buttons will call the `AlterBy` method which will instruct the `NavigationManager` service to
navigate to `/counter/X`, where _X_ is the value of the adjusted `CurrentCount` - resulting in the following output in
the browser's console:

```console
WASM: Notified of navigation via code to http://localhost:6812/counter/1  
WASM: Notified of navigation via code to http://localhost:6812/counter/2  
WASM: Notified of navigation via code to http://localhost:6812/counter/3  
WASM: Notified of navigation via code to http://localhost:6812/counter/4
```

Clicking the _Reset_ link will result in an _Intercepted_ navigation (i.e. not initiated in C# code) and
navigate to `/counter/0`, resetting the value of `CurrentCount`.

```console {5}
WASM: Notified of navigation via code to http://localhost:6812/counter/1  
WASM: Notified of navigation via code to http://localhost:6812/counter/2  
WASM: Notified of navigation via code to http://localhost:6812/counter/3  
WASM: Notified of navigation via code to http://localhost:6812/counter/4  
WASM: Notified of navigation via HTML to http://localhost:6812/Counter/0
```

### ForceLoad

The `forceLoad` parameter instructs Blazor to bypass its own routing system and instead have the browser actually
navigate to the new URL.
This will result in an HTTP request to the server to retrieve the content to display.

Note that a force load is not required in order to navigate to an off-site URL.
Calling `NavigateTo` to another domain will invoke a full browser navigation.

Play with the GitHub example for this section.
Look in the browser's Console window to see how `IsNavigationIntercepted` differs when navigating via the buttons and
the Reset link, and look in the browser's Network window to see how it behaves differently based on whether you are:

- Navigating with `forceLoad` set to `false`.
- Navigating with `forceLoad` set to `true`.
- Navigating to an off-site URL.

To observe the last scenario, you may wish to update your `AdjustBy` method to navigate off-site when `CurrentValue`
passes a specific value.

```razor
void AlterBy(int adjustment)
{
  int newCount = CurrentCount + adjustment;

  if (newCount >= 10)
    NavigationManager.NavigateTo("https://ibm.com");

  NavigationManager.NavigateTo("/counter/" + newCount, forceLoad);
}
```
