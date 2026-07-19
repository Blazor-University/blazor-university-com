---
title: "Component scoped dependencies"
date: "2026-07-16"
order: 5
---

So far we have learned about the three dependency injection scopes: Singleton, Scoped, and Transient. We have also experimented to see how these [different dependency injection scopes compare](/dependency-injection/dependency-lifetimes-and-scopes/comparing-dependency-scopes/) to each other, and how the [Scoped lifetime differs](/dependency-injection/dependency-lifetimes-and-scopes/scoped-dependencies/) between ASP.NET MVC and the various Blazor render modes.

Thread-safety is a key concern when writing services for server-side Blazor. A Singleton registered dependency will be used by multiple threads across different circuits simultaneously. Even a Scoped dependency can be accessed by multiple threads during concurrent rendering phases (see [Multi-threaded rendering](/components/multi-threaded-rendering/)). If we consume a library that is not thread-safe (EntityFrameworkCore's `DbContext` being the classic example), we need a way to ensure each component receives its own private instance. That is the problem component-scoped dependencies solve.

In some cases we might need more control over the lifetimes of our injected dependencies, and to control whether they are shared across components or for use by a single component only. The following sections will cover a number of scenarios and how they might be implemented; some of which are built into Blazor already, and some custom made based on what we learn along the way.

- [OwningComponentBase\<T\>](/dependency-injection/component-scoped-dependencies/owningcomponentbase-generic/) -- Using the generic `OwningComponentBase<T>` to own a single dependency.
- [Owning multiple dependencies: The wrong way](/dependency-injection/component-scoped-dependencies/owning-multiple-dependencies-the-wrong-way/) -- A demonstration of the pitfalls when trying to own multiple dependencies.
- [Owning multiple dependencies: The right way](/dependency-injection/component-scoped-dependencies/owning-multiple-dependencies-the-right-way/) -- Using the non-generic `OwningComponentBase` to own multiple dependencies.

