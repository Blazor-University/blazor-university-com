---
title: "Updating the document title"
date: "2019-10-30"
order: 1
---

[![](images/SourceLink-e1567978928628.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/JavaScriptInterop/UpdatingDocumentTitle)

In the section [Creating a Blazor layout](/layouts/creating-a-blazor-layout/)
it we saw how a Blazor app lives within a HTML (or cshtml) document,
and only has control over the content within the main application element.

![](images/Layout.png)

Websites that are not single-page applications can specify text to appear in the browser's tab by adding a `<title>` element
within the `<head>` section, but our Blazor apps don't actually navigate to different server pages,
so they all have the same document title that was loaded when our application started.

We'll now fix this with a new `<Document>` component, which will use JavaScript interop to set the `document.title`,
which will be reflected in the browser's tab.
We'll create this as a Blazor server application; it could quite easily be created inside a reusable [component library](/component-libraries/),
but I'll leave that as an exercise for you.

Create a new Blazor server application, and then in **wwwroot** folder create a **scripts** folder,
and within that create a script named **DocumentInterop.js** with the following script.

```js
var BlazorUniversity = BlazorUniversity || {};
BlazorUniversity.setDocumentTitle = function (title) {
    document.title = title;
};
```

This creates an object named `BlazorUniversity` with a function called `setDocumentTitle` which takes a new title and
assigns it to `document.title`.

Next, edit the **/Pages/_Host.cshtml** file and add a reference to our new script.

```cshtml
<script src="\_framework/blazor.server.js"></script>
<script src="~/scripts/DocumentInterop.js"></script>
```

Finally, we need the `Document` component itself. In the /Shared folder create a new component named `Document.razor`
and enter the following markup.

```razor
@inject IJSRuntime JSRuntime
@code {
    [Parameter]
    public string Title { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await JSRuntime.InvokeVoidAsync("BlazorUniversity.setDocumentTitle", Title);
    }
}
```

This code has a deliberate error in it. Run the application and you will see a `NullReferenceException`
on the line that calls `JSRuntime.InvokeVoidAsync`.

The reason for this is that Blazor runs a pre-render phase on the server before handing control over to the client.
The purpose of this pre-render is to return valid rendered HTML from the server so that

- Web crawlers, such as Google, can index our site.
- The user sees a result immediately.

The problem here is that when the pre-render phase runs, there is no browser for `JSRuntime` to interop with.
Possible solutions are

1. Edit **/Pages/_Host.cshtml** and change `<component type="typeof(App)" render-mode="ServerPrerendered" />` to
   `<component type="typeof(App)" render-mode="Server"/>`  
    **Pro**: A simple fix.  
    **Con**: Google etc. will not see any content when visiting the pages of our website.
2. Instead of overriding `OnParametersSetAsync` override `OnAfterRenderAsync`.  

\#2 is the correct way to solve the problem.

```razor
@inject IJSRuntime JSRuntime
@code {
    [Parameter]
    public string Title { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            await JSRuntime.InvokeVoidAsync("BlazorUniversity.setDocumentTitle", Title);
    }
}
```

As explained in the section on [The JavaScript boot process](/javascript-interop/javascript-boot-process/),
when the server pre-renders the website before sending it do the client browser it will render the App component without
any JavaScript. The `OnAfterRender*` methods are invoked with `firstRender` set to `true` only once the HTML has been rendered
in the browser.

## Using the new Document component

Edit each of the pages in the /Pages folder, and add our new element `<Document Title="Index"/>` -
but obviously with the correct text you would like displayed in the browser's tabs.
