---
title: "Detecting navigation events"
date: "2019-09-02"
order: 8
---

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Routing/NavigatingViaCode)

Access to browser navigation from Blazor is provided via the `NavigationManager` service.
This can be injected into a Blazor component using `@inject` in a razor file, or the `[Inject]` attribute in a CS file.

## The LocationChanged event

`LocationChanged` is an event that is triggered whenever the URL in the browser is altered.
It passes an instance of `LocationChangedEventArgs` which provides the following information:

```razor
public readonly struct LocationChangedEventArgs
{
  public string Location { get; }
  public bool IsNavigationIntercepted { get; }
}
```

The `Location` property is the full URL as it appears in the browser, including the protocol, path, and any query string.

**IsNavigationIntercepted** indicates whether the navigation was initiated via code or via an HTML navigation.

- `false`  
    The navigation was initiated by `NavigationManager.NavigateTo` being called from code.
- `true`  
    The user clicked an HTML navigation element (such as an `a href`) and Blazor intercepted the navigation instead of
    allowing the browser to actually navigate to a new URL, which would result in a request to the server.
    It will also be true in other cases, such as if some `JavaScript` on the page causes a navigation
    (for example, after a `timeOut`).
    Ultimately, any navigation event that wasn't initiated via `NavigationManager.NavigateTo` will be considered an intercepted
    navigation, and this value will be `true`.

Note there is currently no way to intercept a navigation and prevent it from proceeding.

### Observing OnLocationChanged events

It is important to note that the `NavigationManager` service is a long-living instance.
Consequently, any component that subscribes to its `LocationChanged` event will be strongly referenced for the duration
of the service's lifetime.
It is therefore important our components also unsubscribe from this event when they are destroyed,
otherwise they will not be garbage collected.

Currently, the `ComponentBase` class does not have a [lifecycle](http://blazor-university.com/components/component-lifecycles/)
event for when it is destroyed, but it is possible to implement the `IDisposable` interface.

```razor
@implements IDisposable
@inject NavigationManager NavigationManager

protected override void OnInitialized()
{
  // Subscribe to the event
  NavigationManager.LocationChanged += LocationChanged;
  base.OnInitialized();
}

void LocationChanged(object sender, LocationChangedEventArgs e)
{
  string navigationMethod = e.IsNavigationIntercepted ? "HTML" : "code";
  System.Diagnostics.Debug.WriteLine($"Notified of navigation via {navigationMethod} to {e.Location}");
}

void IDisposable.Dispose()
{
  // Unsubscribe from the event when our component is disposed
  NavigationManager.LocationChanged -= LocationChanged;
}
```
