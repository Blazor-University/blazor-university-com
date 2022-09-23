---
title: "Constraining route parameters"
date: "2019-07-16"
order: 3
---

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Routing/ConstrainingRouteParameters)

In addition to being able to specify URL templates that include parameters,
it is also possible to ensure Blazor will only match a URL to a component if the value of the parameter meets certain criteria.

For example, in an application where purchase order numbers are always integers
we would want the parameter in our URL to match our component for displaying purchase orders only if the URL's value for
`OrderNumber` is actually a number.

To define a constraint for a parameter it is suffixed with a colon and then the constraint type.
For example `:int` will only match the component's URL if it contains a valid integer value in the correct position.

```razor
@page "/"
@page "/purchase-order/{OrderNumber:int}"

<h1>
    Order number:
    @if (!OrderNumber.HasValue)
    {
        @:None
    }
    else
    {
        @OrderNumber
    }
</h1>
<h3>Select a purchase order</h3>
<ul>
    <li><a href="/purchase-order/1/">Order 1</a></li>
    <li><a href="/purchase-order/2/">Order 2</a></li>
    <li><a href="/purchase-order/42/">Order 42</a></li>
</ul>

@code {
    [Parameter]
    public int? OrderNumber { get; set; }
}
```

## Constraint types

<table>
    <thead>
        <tr>
            <td><strong>Constraint<strong></td>
            <td><strong>.NET type<strong></td>
            <td><strong>Valid<strong></td>
            <td><strong>Invalid<strong></td>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td><strong>:bool</strong></td>
            <td>System.Boolean</td>
            <td><ul><li>false</li><li>true</li></ul></td>
            <td><ul><li>1</li><li>Hello</li></ul></td>
        </tr>
        <tr>
            <td><strong>:datetime</strong></td>
            <td>System.DateTime</td>
            <td><ul><li>2001-01-01</li><li>02-29-2000</li></ul></td>
            <td><ul><li>29-02-2000</li></ul></td>
        </tr>
        <tr>
            <td><strong>:decimal</strong></td>
            <td>System.Decimal</td>
            <td><ul><li>2.34</li><li>0.234</li></ul></td>
            <td><ul><li>2,34</li><li>૦.૨૩૪</li></ul></td>
        </tr>
        <tr>
            <td><strong>:double</strong></td>
            <td>System.Double</td>
            <td><ul><li>2.34</li><li>0.234</li></ul></td>
            <td><ul><li>2,34</li><li>૦.૨૩૪</li></ul></td>
        </tr>
        <tr>
            <td><strong>:float</strong></td>
            <td>System.Single</td>
            <td><ul><li>2.34</li><li>0.234</li></ul></td>
            <td><ul><li>2,34</li><li>૦.૨૩૪</li></ul></td>
        </tr>
        <tr>
            <td><strong>:guid</strong></td>
            <td>System.Guid</td>
            <td><ul><li>99303dc9-8c76-42d9-9430-de3ee1ac25d0</li></ul></td>
            <td><ul><li>{99303dc9-8c76-42d9-9430-de3ee1ac25d0}</li></ul></td>
        </tr>
        <tr>
            <td><strong>:int</strong></td>
            <td>System.Int32</td>
            <td><ul><li>-1</li><li>42</li><li>299792458</li></ul></td>
            <td><ul><li>12.34</li><li>૨૩</li></ul></td>
        </tr>
        <tr>
            <td><strong>:long</strong></td>
            <td>System.Int64</td>
            <td><ul><li>-1</li><li>42</li><li>299792458</li></ul></td>
            <td><ul><li>12.34</li><li>૨૩</li></ul></td>
        </tr>
    </tbody>
</table>

## Localization

Blazor constraints do not currently support localization.

- Numeric digits are only considered valid if they are in the form `0..9`, and not from a non-English language such as
  `૦..૯` (Gujarati).
- Dates are only valid in the form `MM-dd-yyyy`, `MM-dd-yy`, or in ISO format `yyyy-MM-dd`.
- Boolean values must be `true` or `false`.

## Unsupported constraint types

Blazor constraints do not support the following constraint types, but hopefully will in future:

- **Greedy parameters**  
In ASP.NET MVC it is possible to provide a parameter name that starts with an asterisk and catches a chunk of the URL
including forward slashes.  
    `/articles/{Subject}/{*TheRestOfTheURL}`
- **Regular expressions**  
Blazor does not currently support the ability to constrain a parameter based on a regular expression.
- **Enums**  
It's not currently possible to constrain a parameter to match a value of an enum.
- **Custom constraints**  
It is not possible to define a custom class that determines whether or not a value passed to a parameter is valid.
