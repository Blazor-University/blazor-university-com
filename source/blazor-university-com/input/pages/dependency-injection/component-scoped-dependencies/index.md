---
title: "Component scoped dependencies"
date: "2020-05-25"
---

So far we've learned about the three dependency injection scopes: Singleton, Scoped, and Transient. We've also experimented to see how these [different dependency injection scopes compare](/dependency-injection/dependency-lifetimes-and-scopes/comparing-dependency-scopes/) to each other, and how the [Scoped lifetime differs](/dependency-injection/dependency-lifetimes-and-scopes/scoped-dependencies/) between ASP.NET MVC and Blazor.

In some cases we might need more control over the lifetimes of our injected dependencies, and to control whether they are shared across components or for use by a single component only. The following sections will cover a number of scenarios and how they might be implemented; some of which are built into Blazor already, and some custom made based on what we learn along the way.

