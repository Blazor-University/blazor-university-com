---
title: "Type safety"
date: "2026-07-16"
order: 2
---

In the section [Calling .NET from JavaScript](/javascript-interop/calling-dotnet-from-javascript/),
you may have noticed that line 6 of our JavaScript calls `toString()` on our randomly generated number before passing it
to .NET.

```js
var BlazorUniversity = BlazorUniversity || {};
BlazorUniversity.startRandomGenerator = function(dotNetObject) {
  setInterval(function () {
    let text = Math.random() * 1000;
    console.log("JS: Generated " + text);
    dotNetObject.invokeMethodAsync('AddText', text.toString());
  }, 1000);
};
```

Despite object types being quite interchangeable in JavaScript,
they are not so interchangeable when they are passed to our .NET Invokable method.
When invoking .NET, make sure you choose the correct .NET type for the variable being passed.

<table class="">
  <tbody>
    <tr>
      <td>JavaScript type</td>
      <td>.NET type</td>
    </tr>
    <tr>
      <td>boolean</td>
      <td>System.Boolean</td>
    </tr>
    <tr>
      <td>string</td>
      <td>System.String</td>
    </tr>
    <tr>
      <td>number</td>
      <td>
        System.Single (float) / System.Decimal<br />System.Int32 (etc) if no decimal
        value
      </td>
    </tr>
    <tr>
      <td>Date</td>
      <td>System.DateTime or System.String</td>
    </tr>
    <tr>
      <td>null / undefined</td>
      <td>Nullable types (e.g. `int?`, `bool?`, `DateTime?`)</td>
    </tr>
    <tr>
      <td>number (large)</td>
      <td>System.Int64 (long) can lose precision for values beyond 2^53</td>
    </tr>
    <tr>
      <td>Array</td>
      <td>System.Byte[] (and other array types)</td>
    </tr>
  </tbody>
</table>

> **Note on JSON serialization:** Modern Blazor applications compiled with AOT or trimming for WebAssembly use source-generated JSON serializers. Types used in `[JSInvokable]` interop must be included in the source generation context when trimming is enabled. This ensures the serializer can handle them at runtime without reflection.

## Enums

When a `JSInvokable` .NET method has a parameter that is an `enum`,
JavaScript is expected to pass the numerical value of the enum.
The following example would invoke our .NET method with the value `TestEnum.SecondValue`.

```cs
public enum TestEnum
{
  FirstValue = 100,
  SecondValue = 200
};

[JSInvokable("OurInvokableDotNetMethod")]
public void OurInvokableDotNetMethod(TestEnum enumValue)
{
}
```

```js
dotNetObject.invokeMethodAsync('OurInvokableDotNetMethod', 200);
```

However, if we decorate our `enum` with `[System.Text.Json.Serialization.JsonConverter]` we can enable our JavaScript to
pass string values instead.

```cs
[System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
public enum TestEnum
{
  FirstValue = 100,
  SecondValue = 200
};
```

Now the calling JavaScript can pass the name of the enum value **or** its numeric value. The two following calls are equivalent.

```js
dotNetObject.invokeMethodAsync('OurInvokableDotNetMethod', 'FirstValue');
dotNetObject.invokeMethodAsync('OurInvokableDotNetMethod', 200);
```
