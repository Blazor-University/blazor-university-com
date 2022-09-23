---
title: "Multi-threaded rendering"
date: "2020-06-03"
order: 11
---

As there is more than one thread available in server-side Blazor applications,
it is entirely possible that different components can have code executed on them by various threads.

This is seen most frequently in asynchronous `Task` based operations.
For example, multiple components sending an HTTP request to a server will receive individual responses.
Each individual response will resume the calling method using whichever thread the system choses for us
from a pool of available threads.

The easiest way for us to observe this behavior is to create some asynchronous methods that perform an `await`.
For this example,
we'll use the `OnInitializedAsync` [lifecycle](https://blazor-university.com/components/component-lifecycles/) method.

[![](images/SourceLink-e1567978928628.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Components/MultithreadedRendering)

To demonstrate this, we'll first need to create a new server-side Blazor application.
Then, in the **/Shared** folder, create a component named `SynchronousInitComponent`.
This component will capture the `Thread.ManagedThreadId` of the current thread when `OnInitialized` is executed.
This value will then be displayed on the page when our component renders.

```razor{: .line-numbers}
<p>Sync rendered by thread @IdOfRenderingThread</p>

@code
{
  int IdOfRenderingThread;

  protected override void OnInitialized()
  {
    base.OnInitialized();
    IdOfRenderingThread =
      System.Threading.Thread.CurrentThread.ManagedThreadId;
  }
}
```

- **Line 5**  
    A field is declared to hold a reference to a thread ID.
- **Line 7**  
    The `OnInitialized` [lifecycle](https://blazor-university.com/components/component-lifecycles/) method is overridden.
- **Line 10**  
    The ID of the current thread is stored in `IdOfRenderingThread` so it can be rendered.
- **Line 1**  
    Renders the ID of the thread that was captured on line 10.

Finally, edit the **/Pages/Index.razor** page to display 5 instances of our new component.

```razor
@page "/"

<h1>Components with synchronous OnInitialized()</h1>
@for (int i = 0; i < 5; i++)
{
  <SynchronousInitComponent />
}
```

Running the application will show the same thread ID for each component. Obviously,
your thread ID might not be the same as mine.

_Components with synchronous OnInitialized()_  
Sync rendered by thread 4  
Sync rendered by thread 4  
Sync rendered by thread 4  
Sync rendered by thread 4  
Sync rendered by thread 4

### Asynchronous

Next we'll create another new component in the **/Shared** folder named `AsynchronousInitComponent`.
This component will be identical to the `SynchronousInitComponent`,
but will additionally re-assign the value of `IdOfRenderingThread` in `OnInitializedAsync` after an `await` of 1 second.

```razor {: .line-numbers}
<p>Async rendered by thread @IdOfRenderingThread</p>

@code
{
  int IdOfRenderingThread;

  protected override async Task OnInitializedAsync()
  {
    // Runs synchronously as there is no code in base.OnInitialized(),
    // so the same thread is used
    await base.OnInitializedAsync().ConfigureAwait(false);
    IdOfRenderingThread =
      System.Threading.Thread.CurrentThread.ManagedThreadId;

    // Awaiting will schedule a job for later, and we will be assigned
    // whichever worker thread is next available
    await Task.Delay(1000).ConfigureAwait(false);
    IdOfRenderingThread =
      System.Threading.Thread.CurrentThread.ManagedThreadId;
  }
}
```

- **Line 7**  
    The `OnInitializedAsync` [lifecycle](https://blazor-university.com/components/component-lifecycles/) method is overridden.
- **Line 12**  
    As with the synchronous component, the `ManagedThreadId` of the current thread is assigned to `IdOfRenderingThread`
    so it can be rendered by the component. (See note)
- **Line 17**  
    We allow 1 second to elapse before continuing execution of the method.
- **Line 18**  
    `IdOfRenderingThread` is again updated, 
    showing the ID of the thread that re-rendered the component after the `await` of 1 second on line 17.

**Note**: It might seem to make sense that the `await` on line 11 would run asynchronously.
In fact, it runs synchronously.
This is because the base method does nothing.
There are no awaits to asynchronous code (such as `Task.Delay`) so the same thread continues the execution.

We'll also need another page that renders this new component.
Create a new page in **/Pages** named **AsyncInitPage.razor** with the following mark-up.

```razor
@page "/async-init"

<h1>Components with asynchronous OnInitializedAsync()</h1>
@for (int i = 0; i < 5; i++)
{
  <AsynchronousInitComponent/>
}
```

Running the application and navigating to this second page will produce output very similar to the first page,
where each component is rendered by a single thread.

_Components with asynchronous OnInitializedAsync()_  
Async rendered by thread 4  
Async rendered by thread 4  
Async rendered by thread 4  
Async rendered by thread 4  
Async rendered by thread 4

But then, after 1 second, the `await Task.Delay(1000)` in each of the components' `OnInitializedAsync` methods will complete
and update the `IdOfRenderingThread` before rendering the HTML for the browser.
This time, we can see different threads were used to complete the `OnInitializedAsync` methods.

_Components with asynchronous OnInitializedAsync()_  
Async rendered by thread 7  
Async rendered by thread 18  
Async rendered by thread 10  
Async rendered by thread 13  
Async rendered by thread 11

### What about ConfigureAwait(true)?

Specifying `ConfigureAwait(true)` on our `await` does not guarantee we will see all of our components rendered on the same
thread that initiated the `await`.
Specifying`ConfigureAwait(true)` will still result in a mixture of threads being used for the callbacks.

_Components with asynchronous OnInitializedAsync()_  
Async rendered by thread 11  
Async rendered by thread 11  
Async rendered by thread 9  
Async rendered by thread 13  
Async rendered by thread 13

Even if `ConfigureAwait(true)` did guarantee we could continue on the same thread,
this still would not ensure our UI is only ever being rendered by a single thread.
A component could be caused to re-render for many reasons, including (but not limited to).

- A callback from a System.Threading.Timer
- An event triggered by another thread on a Singleton instance shared by multiple users
- A data push from another server we've connected to via a Web Socket.

## Summary

In server-side Blazor applications there is no single UI thread.
Any available thread could be used when rendering work is required.

Additionally, if any method uses an `await` on code that performs asynchronous operations,
it is very likely that the thread assigned to continue the processing of
the method will not be the same one that started it.

In a Blazor WebAssembly application (which only has a single thread) there are no threading problems,
but in server-side applications, this can cause problems when using a non-thread-safe dependency across multiple components.

This issue will be addressed in the section on [OwningComponentBase<T>](https://blazor-university.com/dependency-injection/component-scoped-dependencies/owningcomponentbase-generic/).
