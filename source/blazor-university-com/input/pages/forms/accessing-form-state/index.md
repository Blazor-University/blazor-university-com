---
title: "Accessing form state"
date: "2026-07-16"
order: 6
---

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Forms/AccessingFormState)

Sometimes, we need access to form state inside the `<EditForm>` child content.
The most common use for this is when we need to access the CSS classes for an input,
indicating whether the input is modified or valid/invalid.

First, we need a model class with data annotations to drive validation.

```cs
using System.ComponentModel.DataAnnotations;

public class MyContact
{
    [Required, EmailAddress]
    public string EmailAddress { get; set; }
}
```

For example, if we use Bootstrap to create an email input control prepended with the `@` symbol,
we might end up with mark-up that looks something like the following.

```razor
<div class="form-group">
  <label for="EmailAddress">Email address</label>
  <div class="input-group">
    <div class="input-group-prepend">
      <span class="input-group-text">@@</span>
    </div>
    <InputText @bind-Value=MyContact.EmailAddress id="EmailAddress" class="form-control" type="email" />
  </div>
</div>
```

![](images/PrependedEmail.jpg)

Email input prepended with an @ symbol

The problem is, however, that when the user enters an invalid value the CSS `invalid` class is applied only to the `<InputText>` control.

![](images/PrependedEmailError.jpg)

Prepended email input with an error

If we want to apply the CSS `invalid` class to the `input-group` itself we can use the `EditContext` passed to us from the `<EditForm>` component.

The `ChildContent` parameter of `<EditForm>` is a `RenderFragment<EditContext>`,
which means the `EditContext` instance is passed into its inner content via a variable named `context`
(or whatever alias we tell Blazor to use).
See [Templating components with RenderFragments](/templating-components-with-renderfragments/) for more information.

```razor {: .line-numbers}
<EditForm Model=@MyContact Context="CurrentEditContext">
  <DataAnnotationsValidator />

  <div class="form-group">
    <label for="EmailAddress">Email address</label>
    <div class="input-group @CurrentEditContext.FieldCssClass( () => MyContact.EmailAddress)">
      <div class="input-group-prepend">
        <span class="input-group-text">@@</span>
      </div>
      <InputText @bind-Value=MyContact.EmailAddress id="EmailAddress" class="form-control" type="email" />
    </div>
  </div>

</EditForm>
```

- **Line 1**  
    Using the `Context=` syntax, we tell Blazor to use the variable name **CurrentEditContext** when passing in its `EditContext`.
- **Line 6**  
    Uses the `EditContext.FieldCssClass` method to obtain the correct CSS class name for the input based
    on its state (modified / valid / invalid).

![](images/PrependedEmailError2.jpg)

Prepended email input with error CSS applied to the parent element

If we wish, we can hide the red outline on the generated `<input>` HTML element with some simple CSS.

```css
.input-group > input.invalid
{
  outline: none;
}
```

This CSS tells the browser that our `<input>` HTML element with an **invalid** class applied should not have a red outline
if it is parented directly by an HTML element that has the **input-group** CSS class applied.

![](images/PrependedEmailError3.jpg)

Input with only the outer element outlined

## Using FieldCssClassProvider

In addition to the `FieldCssClass` method on `EditContext`, Blazor also supports `FieldCssClassProvider` for more advanced scenarios. We can create a custom class that derives from `FieldCssClassProvider` and override the `GetFieldCssClass` method to provide our own logic for determining CSS classes based on field state. We then register it on the `EditContext` instance using the `SetFieldCssClassProvider` method.

```cs
public class CustomFieldCssClassProvider : FieldCssClassProvider
{
    public override string GetFieldCssClass(EditContext editContext,
        in FieldIdentifier fieldIdentifier)
    {
        bool isModified = editContext.IsModified(fieldIdentifier);
        bool isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

        if (isModified)
            return isValid ? "modified valid" : "modified invalid";

        return "";
    }
}
```

We register this provider in our form's initialization.

```cs
protected override void OnInitialized()
{
    EditContext.SetFieldCssClassProvider(new CustomFieldCssClassProvider());
}
```

Note that the `valid` and `invalid` CSS classes will only appear if a validation mechanism (such as `DataAnnotationsValidator` or a custom validator) has been wired up in the `EditForm`. Without validation, the framework does not know whether a field is valid or invalid.
