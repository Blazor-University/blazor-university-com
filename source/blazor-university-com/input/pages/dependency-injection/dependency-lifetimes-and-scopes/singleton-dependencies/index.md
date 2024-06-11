---
title: "Singleton dependencies"
date: "2020-05-02"
order: 2
---

A Singleton dependency is a single object instance that is shared by every object that depends upon it.
In a WebAssembly application, this is the lifetime of the current application that is running in the current tab of our browser.
Registering a dependency as a Singleton is acceptable when the class has no state or (in a server-side app) has state that
can be shared across all users connected to the same server; a Singleton dependency must be thread-safe.

To illustrate this shared state, let's create a very simple (i.e. non-scalable) chat application.

## The Singleton chat service

[![](images/SourceLink-e1567978928628.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/DependencyInjection/WebChat)

First, create a new Blazor Server App. Then create a new folder named **Services** and add the following interface.
This is the service our UI will use to send a message to other users, to be notified whenever a user sends a message,
and when our user first connects will enable them to see an limited history of the chat so far.
Because this is a Singleton dependency running on a Blazor server-side application,
it will be shared by all users on the same server.

```razor
public interface IChatService
{
  bool SendMessage(string username, string message);
  string ChatWindowText { get; }
  event EventHandler TextAdded;
}
```

To implement this service we'll use a `List<string>` to store the chat history,
and remove messages from the start of the list whenever there are more than 50 in the queue.
We'll use the `lock()` statement to ensure thread safety.

```razor {: .line-numbers}
public class ChatService : IChatService
{
  public event EventHandler TextAdded;
  public string ChatWindowText { get; private set; }

  private readonly object SyncRoot = new object();
  private List<string> ChatHistory = new List<string>();

  public bool SendMessage(string username, string message)
  {
    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(message))
      return false;

    string line = $"<{username}> {message}";

    lock (SyncRoot)
    {
      ChatHistory.Add(line);
      while (ChatHistory.Count > 50)
        ChatHistory.RemoveAt(0);

      ChatWindowText = string.Join("\\r\\n", ChatHistory.Take(50));
    }

    TextAdded?.Invoke(this, EventArgs.Empty);
    return true;
  }
}
```

- **Line 3**  
    An event our UI can hook into to be notified whenever a new message is posted to our chat server.
- **Line** 4  
    A string representing up to 50 lines of chat history.
- **Lines 16-23**  
    Locks `SyncRoot` to prevent concurrency issues, adds the current line to the chat history,
    removes the oldest history if more than 50 lines, and then recreates the `ChatWindowText` property's contents.
- **Line 25**  
    Informs all consumers of the chat service that the `ChatWindowText` has been updated.

To register the service, open **Startup.cs** and in `ConfigureServices` add the following

services.AddSingleton<IChatService, ChatService>();

## Defining the user interface

To separate our C# chat code from our display mark-up, we'll use a code-behind approach.
In the **Pages** folder create a new file named **Index.razor.cs**,
Visual Studio should automatically embed it beneath the **Index.Razor** file.
We then need to mark our new `Index` class as partial.

```razor
public partial class Index
{
}
```

Well need our component class to do the following

1. When initialised, subscribe to `ChatService.TextAdded`.
2. To avoid our Singleton holding on to references of disposed objects,
   when our component is disposed we should unsubscribe from `ChatService.TextAdded`.
3. Whenever `ChatService.TextAdded` is triggered we should update the user interface to show the new
   `IChatService.ChatWindowText` contents.
4. We should allow the user to enter their name + some text to send to other users.

Let's start with the easiest step, which is step 4, and then implement the other requirements in the order listed.

For simplicity, we'll add the `Name` and `Text` properties to our current class rather than creating a view model,
we'll also decorate them with the `RequiredAttribute` to provide feedback to the user when they try to post text without
filling in the required inputs.

```razor
public partial class Index
{
  [Required(ErrorMessage = "Enter name")]
  public string Name { get; set; }
  [Required(ErrorMessage = "Enter a message")]
  public string Text { get; set; }
}
```

### Initial mark-up and validation

We'll replace the contents of **Index.razor** and replace it with a simple `EditForm` consisting of a `DataAnnotationsValidator`
component and some Bootstrap CSS decorated HTML for inputting a user name and text.

```razor {: .line-numbers}
@page "/"
<h1>Blazor web chat</h1>

<EditForm Model=@this>
  <DataAnnotationsValidator/>
  <div class="mt-1 row">
    <div class="col-3">
      <InputText class="form-control" placeholder="Name" @bind-Value=Name maxlength=20/>
      <ValidationMessage For=@( () => Name )/>
    </div>
    <div class="col-9">
      <div class="input-group">
        <InputText class="form-control" placeholder="..." @bind-Value=Text maxlength=100 />
        <div class="input-group-append">
          <button class="btn btn-primary" type=submit>Send</button>
        </div>
      </div>
      <ValidationMessage For=@( () => Text )/>
    </div>
  </div>
</EditForm>
```

- **Line 4**  
    Creates an [EditForm](https://blazor-university.com/forms/editcontext-fieldidentifiers-and-fieldstate/)
    that is bound to `this`.
- **Line 5**  
    Enables validation based on data annotations such as `RequiredAttribute`.
- **Line 8**  
    Binds a Blazor [InputText](https://blazor-university.com/forms/editing-form-data/) component to the `Name` property.
- **Line 9**  
    Displays any validation errors for the `Name` property.
- **Line 13**  
    Binds a Blazor InputText component to the `Text` property.
- **Line 18**  
    Displays any validation errors for the `Text` property.

## Consuming IChatService

Next we'll inject the `IChatService` and hook it up fully to our component. To achieve this, we'll need to do the following.

```razor {: .line-numbers}
public partial class Index : IDisposable
{
  [Required(ErrorMessage = "Enter name")]
  public string Name { get; set; }
  [Required(ErrorMessage = "Enter a message")]
  public string Text { get; set; }

  [Inject]
  private IChatService ChatService { get; set; }

  private string ChatWindowText => ChatService.ChatWindowText;

  protected override void OnInitialized()
  {
    base.OnInitialized();
    ChatService.TextAdded += TextAdded;
  }

  private void SendMessage()
  {
    if (ChatService.SendMessage(Name, Text))
      Text = "";
  }

  private void TextAdded(object sender, EventArgs e)
  {
    InvokeAsync(StateHasChanged);
  }

  void IDisposable.Dispose()
  {
    ChatService.TextAdded -= TextAdded;
  }
}
```

- **Lines 8-9**  
    Declares a dependency on `IChatService` that should be automatically injected.
- **Line 11**  
    Declares a property that makes accessing `IChatService.ChatWindowText` simple.
- **Line 16**  
    Subscribes to the `IChatService.TextAdded` event.
- **Line 21**  
    Sends the current user's input to the chat service.
- **Line 27**  
    Refreshes the user interface every time `IChatService.TextAdded` is invoked.
- **Line 32**  
    When the component is disposed, unsubscribe from `IChatService.TextAdded` to avoid memory leaks.

**Note**: We must wrap our `StateHasChanged` call in a call to `InvokeAsync`.
This is because the `IChatService.TextAdded` event will be triggered by whichever user added the text,
and will therefore be triggered by various threads.
We need Blazor to marshal these calls using `InvokeAsync` to ensure all threaded calls on our component are performed in
sequence.

## Adding the chat window to our user interface

We now only need to add an HTML `<textarea>` control to our mark-up and bind it to our `ChatWindowText` property,
and ensure that when the `EditForm` is submitted without validation errors it calls our `SendMessage` method.

The final user interface mark-up looks like this.

```razor{: .line-numbers}
@page "/"

<h1>Blazor web chat</h1>

<EditForm Model=@this OnValidSubmit=@SendMessage>
  <DataAnnotationsValidator/>
  <div class="row">
    <textarea class="form-control" rows=20 readonly>@ChatWindowText</textarea>
  </div>
  <div class="mt-1 row">
    <div class="col-3">
      <InputText class="form-control" placeholder="Name" @bind-Value=Name maxlength=20/>
      <ValidationMessage For=@( () => Name )/>
    </div>
    <div class="col-9">
      <div class="input-group">
        <InputText class="form-control" placeholder="..." @bind-Value=Text maxlength=100 />
        <div class="input-group-append">
          <button class="btn btn-primary" type=submit>Send</button>
        </div>
      </div>
      <ValidationMessage For=@( () => Text )/>
    </div>
  </div>
</EditForm>
```

- **Line 5**  
    Calls `SendMessage` when the user presses enter on an `InputText` and the input validation passes.
- **Lines 7-9**  
    HTML to output an HTML `<textarea>` and bind it to `WindowChatText`.

## Singleton dependencies in WebAssembly applications

The preceding application will only allow users to chat with each other if the Blazor application is a Blazor server-side
application.

This is because Singleton dependencies are shared per-application process.
Blazor server-side applications actually run on the server,
and so singleton instances are shared across multiple users that are running in the same server application process.

When running in a WebAssembly application each browser tab is its own separate application process,
therefore users would be unable to chat with each other if they are each running individual processes in their browsers
(WebAssembly hosted applications) because they are not sharing any common state.

This is the same when using multiple servers.
As soon as our chat service is popular enough to warrant one or more additional servers there is no longer a globally
shared state for all users, only a shared state per server.

Once we need to scale up our servers,
or we wish to implement our chat client as a WebAssembly app to take some of the workload away from our servers,
we'd need to set up a more robust method of sharing state.
This is not something within the scope of this section,
as this section's purpose is only to demonstrate how dependencies registered as Singletons are shared across a single
application process.

## Task for the reader

The browser is unlikely to have enough vertical space to display 50 chat messages all at once,
so the user must manually scroll the chat area to see the latest messages.

To improve the user experience, our component should really scroll the `<textarea>` scrollbar to the bottom every time
new text is added.
If you don't wish to tackle this yourself then just take a look at the project that accompanies this section,
the work is done for you. If you do fancy tackling it, here are some clues.

1. You'll need some JavaScript that will take a control as a parameter and set `control.scrollTop = control.scrollHeight`.
2. You'll need to [invoke this JavaScript](https://blazor-university.com/javascript-interop/calling-javascript-from-dotnet/)
   after every time our component [renders](https://blazor-university.com/components/component-lifecycles/).
3. You'll need an [ElementReference](https://blazor-university.com/javascript-interop/calling-javascript-from-dotnet/passing-html-element-references/)
   to the `<textarea>` to pass to the JavaScript.
