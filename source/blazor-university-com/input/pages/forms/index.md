---
title: "Forms"
date: "2026-07-16"
order: 7
---

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Forms/BasicForm)

The `EditForm` component is Blazor's approach to managing user-input in a way that makes it easy to perform validation against user input. It also provides the ability to check if all validation rules have been satisfied, and present the user with validation errors if they have not.

Although it is possible to create forms using the standard `<form>` HTML element, I recommend using the `EditForm` component because of the additional features it provides us with.

**Note:** If you have not done so already, I recommend you read the section on [Two-way binding directives](/components/two-way-binding/binding-directives/).

## The form model

The key feature to the `EditForm` is its `Model` parameter. This parameter provides the component with a context it can work with to enable user-interface binding and determine whether or not the user's input is valid.

Let's start by creating a class we can use with our `EditForm`. At this point a simple empty class will suffice.

```cs
public class Person
{
}
```

Edit the standard **index.razor** page as follows:

```razor {: .line-numbers}
@page "/"

<EditForm Model=@Person>
	<input type="submit" value="Submit" class="btn btn-primary"/>
</EditForm>

@code
{
	Person Person = new Person();
}
```

Line 8 creates an instance of a `Person` for our form to bind to. Line 2 creates an `EditForm` and sets its `Model` parameter to our instance. The preceding razor mark-up results in the following HTML.

```html
<form>
	<input class="btn btn-primary" type="submit" value="Submit">
</form>
```

## Interactive render modes and Static SSR

The examples in this section assume an interactive Blazor render mode such as Interactive Server or Interactive WebAssembly. In those modes the `EditForm` component handles form submission on the client without a full-page postback.

In Static Server-Side Rendering (Static SSR), there is no ongoing Blazor circuit. The `EditForm` still renders an HTML `<form>`, but form submission results in a standard HTTP POST. To handle this we give the form a `FormName` parameter and apply the `[SupplyParameterFromForm]` attribute to the model property on our page.

```razor
@page "/"
@using Microsoft.AspNetCore.Components.Forms

<EditForm Model=@Person FormName="PersonForm">
  ...
</EditForm>

@code {
  [SupplyParameterFromForm]
  public Person? Person { get; set; }
}
```

We will cover these patterns in more detail in the [Handling form submission](/forms/handling-form-submission/) page.

## Detecting form submission

When the user clicks the **Submit** button in the preceding example, the `EditForm` will trigger its `OnSubmit` event. We can use this event in our code to handle any business logic.

```razor
@page "/"

<h1>Status: @Status</h1>
<EditForm Model=@Person OnSubmit=@FormSubmitted>
	<input type="submit" value="Submit" class="btn btn-primary"/>
</EditForm>

@code
{
	string Status = "Not submitted";
	Person Person = new Person();

	void FormSubmitted()
	{
		Status = "Form submitted";
		// Post data to the server, etc
	}
}
```

**Note:** The `OnSubmit` event handler in the example above only executes when the form is running in an interactive render mode. In Static SSR, form submission is handled through the `FormName` and `[SupplyParameterFromForm]` approach described earlier.

Form submission will be covered in more depth in the [Handling form submission](/forms/handling-form-submission/) section.

## What is in this section

This section covers Blazor's forms, validation, and submission features across the following pages:

* [Editing form data](/forms/editing-form-data/) - using Blazor's built-in input components
* [Descending from InputBase\<T\>](/forms/descending-from-inputbase/) - creating custom input components by inheriting from `InputBase<T>`
* [Validation](/forms/validation/) - validating user input with data annotations and the new validation service
* [Handling form submission](/forms/handling-form-submission/) - processing form data on the server or client

