---
title: "Calling .NET From JavaScript"
date: "2019-04-27"
---

Sometimes our .NET application code needs to be executed from JavaScript.
Blazor enables us to asynchronously call methods on instances of objects, or static methods on classes.

## Identifying invokable .NET code

Blazor does not allow JavaScript to call just any static or instance method in our .NET code. There are conditions

1. The method must be decorated with the `JsInvokableAttribute`.
2. The method must be public.
3. The parameters of the method must be Json serializable.
4. The return type of the method must be Json serializable, `void`, a `Task`, or a `Task<T>` where `T` is Json serializable.
5. If specifying the `identifier` parameter on `JsInvokable` the value must be unique per class hierarchy
   (if an instance method) or unique per assembly (if a static method).

## Making .NET code invokable

[![](images/SourceLink-e1567978928628.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/JavaScriptInterop/CallingDotNetFromJavaScriptBasic)

To call a method on a .NET object instance, we first need to pass a reference to the object over to JavaScript.
We cannot pass our object directly because we want to give JavaScript a reference to our object rather than a Json
serialized representation of its state.
We do this by creating an instance of the `DotNetObjectReference` class.
To demonstrate this, we'll create a simple Blazor app that receives random text from JavaScript every second.

First, create a new Blazor application and change Index.razor to the following mark-up.

> **Warning**: The following code has a memory leak and should not be used in production. This will be highlighted and 
> corrected in [Lifetimes and Memory Leaks](/javascript-interop/calling-dotnet-from-javascript/lifetimes-and-memory-leaks/).

```razor
@page "/"
@inject IJSRuntime JSRuntime

<h1>Text received</h1>
<ul>
    @foreach (string text in TextHistory)
    {
        <li>@text</li>
    }
</ul>

@code
{
    List<string> TextHistory = new List<string>();

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);
        if (firstRender)
        {
            // See warning about memory above in the article
            var dotNetReference = DotNetObjectReference.Create(this);
            JSRuntime.InvokeVoidAsync("BlazorUniversity.startRandomGenerator", dotNetReference);
        }
    }

    [JSInvokable("AddText")]
    public void AddTextToTextHistory(string text)
    {
        TextHistory.Add(text.ToString());
        while (TextHistory.Count > 10)
            TextHistory.RemoveAt(0);
        StateHasChanged();
        System.Diagnostics.Debug.WriteLine("DotNet: Received " + text);
    }
}
```

- **Line 2**  
    Injects the `IJSRuntime` service. We use this to initialise our JavaScript, passing in a reference to the component that will receive the notifications.
- **Line 6**  
    Iterates over a `List<string>` and renders them as HTML `<li>` elements.
- **Line 22**  
    When the component is first rendered, we invoke a JavaScript function named **BlazorUniversity.startRandomGenerator**,
    passing a reference to the current component by calling `DotNetObjectReference.Create(this)`.
- **Line 26**  
    We decorate our call-back method with `JSInvokable`. 
    A specific `identifier` is given; this is a recommended practice,
    otherwise Blazor will infer the name from the name of the method,
    and so refactoring the method to a new name could break any JavaScript that executes it.
- **Line 27**  
    Our method conforms to Blazor callback requirements. It is public, has a `void` return type,
    and its only parameter is serializable from Json.
- **Lines 29-32**  
    Adds the received text to our `List<string>`,
    ensures there are no more than 10 items, and then calls `StateHasChanged` so Blazor knows it needs to recreate its [RenderTree](/components/render-trees/).
- **Line 33**  
    Outputs the text received by .NET to the Visual Studio output window.

## Invoking .NET code from JavaScript

First we need to edit either **/Pages/_Host.cshtml** (server side) or **/wwwroot/index.html** (WASM) and add a reference
to a script we are about to create.

```razor
<script src="/scripts/CallingDotNetFromJavaScript.js"></script>
```

Next, we'll create the function **BlazorUniversity.startRandomGenerator** and have it call back our .NET object with a random
number every second.

```razor
var BlazorUniversity = BlazorUniversity || {};
BlazorUniversity.startRandomGenerator = function(dotNetObject) {
    setInterval(function () {
        let text = Math.random() \* 1000;
        console.log("JS: Generated " + text);
        dotNetObject.invokeMethodAsync('AddText', text.toString());
    }, 1000);
};
```

Now run the application and press F12 to view the browser tools window.
Look in the console and we should see something like the following:

![](images/CallingDotNetFromJavaScript.png)

Browser view

![](images/CallingDotNetFromJavaScriptVS.png)

Visual Studio output view
