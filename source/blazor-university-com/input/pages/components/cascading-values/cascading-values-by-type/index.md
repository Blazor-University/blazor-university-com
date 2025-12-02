---
title: "Cascading values by type"
date: "2019-07-03"
order: 2
---

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/CascadingValues/CascadingValuesByType)

Previously we saw how to cascade a value by name. Setting a `Name` is important because it is used to push a value specified
in a `CascadingValue` into the correct properties in consuming components by matching up their names.
Another option is to specify a `CascadingValue` without specifying a `Name`, when Blazor encounters a cascading value
specified in this way it will inject its value into a component's property if the property meets the following criteria.

1. The property is decorated with a `CascadingParameterAttribute`.
2. The `[CascadingParameter]` does **not** have a `Name` specified.
3. The property is of the same `Type` as set in the `CascadingValue` (e.g. boolean).
4. The property has a setter.
5. The property is public.

For example, the following `CascadingValue` will match both `CascadingParameter` properties in `SomeComponent`.

```razor
<CascadingValue Value=@true>
  <SomeComponent/>
</CascadingValue>
```

```razor
Property1 = @Property1
Property2 = @Property2

@code
{
  [CascadingParameter]
  private bool Property1 { get; set; }

  [CascadingParameter]
  private bool Property2 { get; set; }
}
```

An unnamed `CascadingValue` isn't as specific as a `CascadingValue` that has a `Name` specified, because every `CascadingParameter`
decorated property with the correct type and no `Name` will consume the value.
In cases where we define a simple .NET type such as a `bool` or an `int` it is recommended we use a named parameter, however,
sometimes the type of the value is sufficient to identify its purpose;
specifying a name would be redundant, and excluding it is therefore a small time saver.

As the recruitment application grows we might end up with multiple cascading parameters, such as:

- `bool ViewAnonymizedData`  
    Indicates if personally-identifying information should be hidden.
- `string DateFormat`  
    Consuming components can use this to format dates in a uniform manner.
- `string LanguageCode`  
    Components could use this to display translated text.

The clear pattern emerging here is that these are all related to a user's preferences.
Rather than having Razor mark-up with multiple `CascadingValue` elements, like this:

```razor
<CascadingValue Name="ViewAnonymizedData" Value=@ViewAnonymizedData>
  <CascadingValue Name="DateFormat" Value=@DateFormat>
    <CascadingValue Name="LanguageCode" Value=@LanguageCode>
      (Body goes here)
    </CascadingValue>
  </CascadingValue>
</CascadingValue>
```

It would make more sense (and take less code) to have a custom class:

```cs
public class UserPreferences
{
  public bool ViewAnonymizedData { get; set; }
  public string DateFormat { get; set; }
  public string LanguageCode { get; set; }
}
```

and then create your Razor mark-up like this:

```razor
<CascadingValue Value=@UserPreferences>
</CascadingValue>
```

Consuming components then only need a single property marked as a `[CascadingParameter]` rather than three.

```razor
@if (!UserPreferences.ViewAnonymizedData)
{
  <div>
    <span>Name</span> @Candidate.Name
  </div>
  <div>
    <span>Date of birth</span> @Candidate.DateOfBirth.ToString(UserPreferences.DateFormat)
  </div>
  <ViewAddress Address=@Candidate.Address/>
}
else
{
  <span>[Anonmymized view]</span>
}

@code
{
  [CascadingParameter]
  private UserPreferences UserPreferences { get; set; }
}
```

Obviously, this example excludes how to translate the static text based on the `UserPreferences.LanguageCode`.
