---
title: "Type safety"
date: "2019-11-23"
order: 2
---

In the section [Calling .NET from JavaScript](/javascript-interop/calling-dotnet-from-javascript/),
you may have noticed that line 6 of our JavaScript calls `toString()` on our randomly generated number before passing it
to .NET.

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
        System.Float / System.Decimal<br />System.Int32 (etc) if no decimal
        value
      </td>
    </tr>
    <tr>
      <td>Date</td>
      <td>System.DateTime or System.String</td>
    </tr>
  </tbody>
</table>

## Enums

When a `JSInvokable` .NET method has a parameter that is an `enum`,
JavaScript is expected to pass the numerical value of the enum.
The following example would invoke our .NET method with the value `TestEnum.SecondValue`.

```razor
public enum TestEnum
{
  FirstValue = 100,
  SecondValue = 200
};

[JSInvokable("OurInvokableDotNetMethod")]
public void OurInvokableDotNetMethod(TestEnum enumValue)
{
}

dotNetObject.invokeMethodAsync('OurInvokableDotNetMethod', 200);
```

However, if we decorate our `enum` with `[System.Text.Json.Serialization.JsonConverter]` we can enable our JavaScript to
pass string values instead.

```razor
[System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
public enum TestEnum
{
  FirstValue = 100,
  SecondValue = 200
};
```

Now the calling JavaScript can pass the name of the enum value **or** its numeric value. The two following calls are equivalent.

```cs
dotNetObject.invokeMethodAsync('OurInvokableDotNetMethod', 'FirstValue');
dotNetObject.invokeMethodAsync('OurInvokableDotNetMethod', 200);
```
