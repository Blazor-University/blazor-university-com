---
title: "Component libraries"
date: "2019-07-16"
order: 7
---

Component libraries enable us to package components and pages into a single re-usable project, along with any supporting files such as CSS files, JavaScript, and images.

Create a new Blazor solution named ClassLibraryConsumer. Right-click the solution and select **Add**\->**New Project**, and then select **Razor Class Library** - name it **BlazorUniversity.ClassLibrary**.

This will create a new Razor class library inside a new folder named BlazorUniversity.ClassLibrary, and create a new `csproj` file with the same name. Add the new library to the current solution, and then reference the new library from the ClassLibraryConsumer project.

Our new class library can now be used from any number of projects by including it in the solution and referencing it, or it can be pushed to NuGet.org and consumed as a NuGet package.

## Adding supporting files

The default project created for us has a folder named `wwwroot`. This is where we are expected to place any supporting files the consumer of our library requires, such as JavaScript etc.

## Accessing resources in a consumed component library

Resources within the wwwroot folder of a consumed component library will be published with your project automatically. To access resources from a consumed library we need to use the following URL format.

`/_content/PackageId/MyImage.png`

- `_content` is the part of the path where all consumed component libraries' resources end up.
- `PackageId` is the Package Id of the binary that contains the resources. This is the name you see entered in the **Package id** input when you right-click your class library, select **Properties**, and select the **Package** tab. If you installed the library via NuGet, it is the name of the package you installed.
- `MyImage.png` is the name of any resource within the component library's `wwwroot` folder. The resource can be directly within that folder, or the path can identify a resource within any level of sub-folders, such as `/_content/BlazorUniversity.ConsumedLibrary/scripts/HelloWorld.js`

Note that any components within our component library should also reference resources using the same format.

## Consuming a component library

Consuming a component library is as simple as either

- Adding a project reference to the library  
    or,
- Adding a NuGet reference to the library.

Make sure to read any notes from the library's author as you might need to add CSS and/or JavaScript references to your HTML.

### Referencing consumed scripts in Client-side Blazor

In a client-side Blazor application this typically involves adding a `<script>` reference to our project's **wwwroot/index.html** file.

### **Referencing consumed scripts i**n Server-side Blazor

For a server-side Blazor app it is added into the file **/Pages/\_Host.cshtml** and is usually added before the existing `<script>` tag referencing either `_framework/blazor.server.js` or `_framework/blazor.webassembly.js`

