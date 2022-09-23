---
title: "Forms"
date: "2019-04-27"
order: 6
---

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Forms/BasicForm)

The `EditForm` component is Blazor's approach to managing user-input in a way that makes it easy to perform validation against user input. It also provides the ability to check if all validation rules have been satisfied, and present the user with validation errors if they have not.

Although it is possible to create forms using the standard `<form>` HTML element, I recommend using the `EditForm` component because of the additional features it provides us with.

**Note:** If you have not done so already, I recommend you read the section on [Two-way binding directives](/components/two-way-binding/binding-directives/).

## The form model

The key feature to the `EditForm` is its `Model` parameter. This parameter provides the component with a context it can work with to enable user-interface binding and determine whether or not the user's input is valid.

Let's start by creating a class we can use with our `EditForm`. At this point a simple empty class will suffice.

public class Person
{
}

Edit the standard **index.razor** page as follows:

@page "/"

<EditForm Model=@Person>
	<input type="submit" value="Submit" class="btn btn-primary"/>
</EditForm>

@code
{
	Person Person = new Person();
}

Line 9 creates an instance of a `Person` for our form to bind to. Line 3 creates an `EditForm` and sets its `Model` parameter to our instance. The preceding razor mark-up results in the following HTML.

<form>
	<input class="btn btn-primary" type="submit" value="Submit">
</form>

## Detecting form submission

When the user clicks the **Submit** button in the preceding example, the `EditForm` will trigger its `OnSubmit` event. We can use this event in our code to handle any business logic.

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

Form submission will be covered in more depth in the [Handling form submission](/forms/handling-form-submission/) section.

