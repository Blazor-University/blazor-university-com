---
title: "Code generated HTML attributes"
date: "2019-07-11"
order: 7
---

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Components/CodeGeneratedHtmlAttributes)

Razor is great when it comes to conditional HTML output, or outputting HTML in a for-loop,
but when it comes to conditional code within the element itself things are a bit more tricky.
For example , the following code does not compile because you cannot add C# control blocks inside the `<` and `>` of an element.

```razor
<img
  @foreach(var nameAndValue in AdditionalAttributes)
  {
    @nameAndValue.Key = @nameAndValue.Value
  } 
  src="https://randomuser.me/api/portraits/lego/1.jpg" />

@code
{
  Dictionary<string, object> AdditionalAttributes;

  protected override void OnInitialized()
  {
    AdditionalAttributes = new Dictionary<string, object>
    {
      ["id"] = "EmmetImage",
      ["alt"] = "A photo of Emmet"
    };
    base.OnInitialized();
  }
}
```

The next approach we might attempt is to write a method that returns a string and call that inside the `<` and `>` characters.

```razor
<div @IfYouCanSeeThisTextThenTheCodeWasNotExecutedHere />
<span>@IfYouCanSeeThisTextThenTheCodeWasNotExecutedHere</span>

@code
{
  string IfYouCanSeeThisTextThenTheCodeWasNotExecutedHere = "The code here was executed";
}
```

But this doesn't work either. The preceding example would output the following HTML.

```html
<div @ifyoucanseethistextthenthecodewasnotexecutedhere=""></div>
<span>The code here was executed</span>
```

Razor will only execute C# code in the following places:

1. Inside an element's content area, for example `<span>@GetSomeHtml()</span>`.
2. When determining a value to assign into an element's attribute, for example `<img src=@GetTheImageForTheUrl() />`.
3. Within the `@code` section.

The technique we need to employ to generate one or more attributes + values for a HTML element is called "Attribute splatting".
Attribute splatting involves assigning a `Dictionary<string, object>` to an attribute with the special name `@attribute`.

```razor
<div @attributes=MyCodeGeneratedAttributes/>

@code
{
  Dictionary<string, object> MyCodeGeneratedAttributes;

  protected override void OnInitialized()
  {
    MyCodeGeneratedAttributes = new Dictionary<string, object>();
    for(int index = 1; index <= 5; index++)
    {
      MyCodeGeneratedAttributes["attribute_" + index] = index;
    }
  }
}
```

The preceding code will output a `<div>` with 5 attributes.

```html
<div attribute_1="1" attribute_2="2" attribute_3="3" attribute_4="4" attribute_5="5"></div>
```

## Special cases

Some HTML attributes, such as `readonly` and `disabled` require no values - their mere presence on the element is sufficient
for them to be effective.
In fact, even apply a value such as `false` will still activate them. The following `<input>` element will be both
`readonly` and `disabled`.

```html
<input readonly="false" disbabled="false"/>
```

In razor views the rule is slightly different.
If we output `readonly=@IsReadOnly` or `disabled=@IsDisabled` - whenever the value being assigned is false razor will not
output the attribute at all;
when the value being assigned is true razor will output the element without assigning a value.

`<input readonly=@true disabled=@false/>` will result in razor generated HTML that does not include the `disabled` attribute
at all.
