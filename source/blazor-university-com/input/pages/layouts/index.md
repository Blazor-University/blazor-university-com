---
title: "Layouts"
date: "2019-04-27"
order: 2
---

A Blazor layout is similar to the ASP Webforms concept of a Master Page, and the same as a Razor layout in ASP MVC.

Almost every website on the Web has a template that is used either throughout the website (branding at the top of the page, copyright at the bottom), or throughout specific sub-sections of a website (such as a specific menu structure on the Admin pages of the site).

This is achieved by creating a view that acts as an HTML wrapper around the current page's content, the template contains a place-holder indicating where the wrapped page's content should appear.

<h1>This is the start of my reusable layout</h1>

<div class="Content">
  -- Some kind of indicator to specify the page's content will go here --
</div>

<footer>
  This is the end of the layout
</footer>

Individual pages can then optionally specify a single layout it would like its content to be wrapped in.

\-- Some way of indicating which template to wrap this page's content in --

<h1>This is the content of your embedded page</h1>

The resulting HTML would look like this

<h1>This is the start of my reusable layout</h1>

<div class="Content">
  <h1>This is the content of your embedded page</h1>
</div>

<footer>
  This is the end of the layout
</footer>

