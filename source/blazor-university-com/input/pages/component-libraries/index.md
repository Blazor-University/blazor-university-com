---
title: "Component libraries"
date: "2026-07-16"
order: 8
---

Component libraries enable us to package components and pages into a single re-usable project, along with any supporting files such as CSS files, JavaScript, and images.

Create a new Blazor solution named ClassLibraryConsumer. Right-click the solution and select **Add**\->**New Project**, and then select **Razor Class Library** - name it **BlazorUniversity.ClassLibrary**.

This will create a new Razor class library inside a new folder named BlazorUniversity.ClassLibrary, and create a new `csproj` file with the same name. Add the new library to the current solution, and then reference the new library from the ClassLibraryConsumer project.

Our new class library can now be used from any number of projects by including it in the solution and referencing it, or it can be pushed to NuGet.org and consumed as a NuGet package.

Modern Razor class libraries target `net10.0` and support all Blazor rendering modes (Interactive Server, Interactive WebAssembly, Interactive Auto, and static server-side rendering). Older RCLs targeting `netstandard2.0` or earlier `net` frameworks can still be consumed but may lack support for the latest Blazor features.

## Adding supporting files

The default project created for us has a folder named `wwwroot`. This is where we are expected to place any supporting files the consumer of our library requires, such as JavaScript etc.

## Accessing resources in a consumed component library

Resources within the wwwroot folder of a consumed component library will be published with your project automatically. To access resources from a consumed library we need to use the following URL format.

`/_content/PackageId/MyImage.png`

- `_content` is the part of the path where all consumed component libraries' resources end up.
- `PackageId` is the Package Id of the binary that contains the resources. This is the name you see entered in the **Package id** input when you right-click your class library, select **Properties**, and select the **Package** tab. If you installed the library via NuGet, it is the name of the package you installed.
- `MyImage.png` is the name of any resource within the component library's `wwwroot` folder. The resource can be directly within that folder, or the path can identify a resource within any level of sub-folders, such as `/_content/BlazorUniversity.ConsumedLibrary/scripts/HelloWorld.js`

Note that any components within our component library should also reference resources using the same format.

## CSS isolation in component libraries

Razor class libraries support CSS isolation just like Blazor apps. Create a `{ComponentName}.razor.css` file alongside your component. The CSS in that file is scoped to that component using the `b-{identifier}` attribute convention. When the library is consumed, the compiled CSS is automatically bundled into the consuming project's build output.

For global CSS that applies across components, place a `wwwroot/lib.css` or similar file in the `wwwroot` folder and reference it via `/_content/PackageId/lib.css`.

## JavaScript isolation in component libraries

Instead of relying on global `<script>` tags, modern RCLs can load JavaScript modules on demand using `IJSRuntime` and `IJSObjectReference`. This avoids naming conflicts and ensures scripts are only loaded when needed.

Place a JavaScript module in your library's `wwwroot` folder:

```js
// wwwroot/scripts/componentLogic.js
export function initialize(element, dotNetObject) {
    element.addEventListener('click', function () {
        dotNetObject.invokeMethodAsync('OnClick');
    });
}

export function destroy(element) {
    element.removeEventListener('click');
}
```

Then load it from your component using the `_content` path. First add a module loader helper in your component library's documentation (or the consuming app can add one):

```html
<script>
    window.importModule = url => import(url);
</script>
```

Then load the module from your component:

```razor
@inject IJSRuntime JSRuntime

@implements IAsyncDisposable

<span @ref="ElementRef">Click me</span>

@code
{
    ElementReference ElementRef;
    IJSObjectReference Module;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module = await JSRuntime.InvokeAsync<IJSObjectReference>("importModule", "./_content/BlazorUniversity.ClassLibrary/scripts/componentLogic.js");
            await Module.InvokeVoidAsync("initialize", ElementRef, DotNetObjectReference.Create(this));
        }
    }

    [JSInvokable]
    public async Task OnClick()
    {
        // Handle the click
    }

    public async ValueTask DisposeAsync()
    {
        if (Module != null)
        {
            await Module.InvokeVoidAsync("destroy", ElementRef);
            await Module.DisposeAsync();
        }
    }
}
```

Note the path format `./_content/PackageId/scripts/componentLogic.js` which resolves to the library's embedded resources at runtime.

## Consuming a component library

Consuming a component library is as simple as either

- Adding a project reference to the library  
    or,
- Adding a NuGet reference to the library.

Make sure to read any notes from the library's author as you might need to add CSS and/or JavaScript references to your HTML.

### Referencing consumed scripts

In modern Blazor, scripts from a consumed library are referenced in the host page before the Blazor script tag. For Blazor WebAssembly, add the `<script>` to **wwwroot/index.html**. For server-side or unified Blazor apps, add it to **App.razor** or **Components/App.razor**. The Blazor script reference is `_framework/blazor.web.js` for all hosting models.

```html
<script src="_content/BlazorUniversity.ClassLibrary/scripts/HelloWorld.js"></script>
<script src="_framework/blazor.web.js"></script>
```

## End-to-end component library example

To tie everything together, here is a complete component that lives inside a Razor class library, uses CSS isolation, and loads a JavaScript module on demand.

Create a new Razor component in the class library project. For this example we will create a simple counter that logs clicks to the browser console. The consuming app must register the `importModule` helper as shown in the JavaScript isolation section above for module loading to work.

**CounterWithJs.razor**

```razor
@inject IJSRuntime JSRuntime

@implements IAsyncDisposable

<div @ref="ElementRef" class="counter-with-js">
    <p>Count: @currentCount</p>
    <button @onclick="IncrementCount">Click me</button>
</div>

@code
{
    ElementReference ElementRef;
    IJSObjectReference Module;
    int currentCount;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module = await JSRuntime.InvokeAsync<IJSObjectReference>("importModule", "./_content/BlazorUniversity.ClassLibrary/scripts/counterWithJs.js");
            var dotNetRef = DotNetObjectReference.Create(this);
            await Module.InvokeVoidAsync("initialize", ElementRef, dotNetRef);
        }
    }

    [JSInvokable]
    public async Task OnButtonClick()
    {
        currentCount++;
        await InvokeAsync(StateHasChanged);
    }

    public async ValueTask DisposeAsync()
    {
        if (Module != null)
        {
            await Module.InvokeVoidAsync("destroy", ElementRef);
            await Module.DisposeAsync();
        }
    }
}
```

**CounterWithJs.razor.css** (CSS isolation)

```css
.counter-with-js {
    border: 1px solid #ccc;
    padding: 1rem;
    border-radius: 4px;
    display: inline-block;
}
```

**wwwroot/scripts/counterWithJs.js**

```js
export function initialize(element, dotNetObject) {
    element.dataset.dotNetObject = dotNetObject;
    let button = element.querySelector('button');
    button.addEventListener('click', async function () {
        console.log('Button was clicked');
        await dotNetObject.invokeMethodAsync('OnButtonClick');
    });
}

export function destroy(element) {
    let button = element.querySelector('button');
    button.removeEventListener('click');
}
```

When another Blazor project references this class library, it can use `CounterWithJs` like any other component by adding `@using BlazorUniversity.ClassLibrary` to its imports. The CSS is automatically scoped, and the JavaScript module is loaded only when the component renders.

