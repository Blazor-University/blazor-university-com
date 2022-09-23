---
title: "JavaScript boot process"
date: "2019-11-30"
order: 1
---

During the Blazor boot process, the browser will create the HTML document before Blazor is initialised,
this means any JavaScript referenced from the bootstrap HTML will be loaded immediately,
and any code that executes automatically within those JavaScript files will be executed before
Blazor has had a chance to initialize.

[![](images/SourceLink-e1567978928628.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/JavaScriptInterop/JavaScriptBootProcess)

To observe this, create a new Blazor server-side application:

- Edit **/App.razor**
- Add the following `OnInitialized` method.

```razor
@code {
  protected override void OnInitialized()
  {
    System.Diagnostics.Debug.WriteLine("Blazor initialised: " + DateTime.Now.ToString("mm:ss.fff"));
    base.OnInitialized();
  }
}
```

- Create a folder under the **/wwwroot** folder named **scripts**
- Within that folder create a file named **JavaScriptBootProcess.js**
- Add the following script

```razor
const now = new Date();
console.log('JavaScript initialized: ' + now.getMinutes() + ":" + now.getSeconds() + "." + now.getMilliseconds());
```

- Edit **/Pages/_Host.cshtml**
- Find the text "ServerPrerendered" and change it to "Server"
- Just below the existing `<script>` tag near the bottom of the page, add the following mark-up

```razor
<script src="~/scripts/JavaScriptBootProcess.js"></script>
```

Run the application and look in both the browser's console output and Visual Studio's output window.
Comparing the output, we will see something like the following:

```console
JavaScript initialized: 15:20.317
Blazor initialized: 15:20.466
```

Because of this behavior it is not possible for JavaScript to invoke .NET methods immediately.
When using JavaScript interop, I advise initiating the communication from the Blazor side if possible.

## ServerPrerendering

If we now edit the same project and change the render mode back to `ServerPrerendered` we'll see something like the following:

Blazor initialized: 42:22.559
JavaScript initialized: 42:22.631
Blazor initialized: 42:22.690

The first time the user visits a URL to our app,
Blazor will render the App.razor component outside of the browser and send the resulting HTML to the browser.
After that, JavaScript is initialized, and then finally Blazor is initialized for the client to interact with.

![](images/JavaScriptBootProcessDiagram.png)
