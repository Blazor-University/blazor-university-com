---
title: "Dependency lifetimes and scopes"
date: "2026-07-16"
order: 2
---

Two important questions to ask when using dependency injection are: How long will the instances of these dependencies live,
and how many other objects have access to the same instance?

The answer depends on a number of factors,
mainly because different options have been made available to us so that we can make the decision for ourselves.
The first factor, and possibly the easiest to understand, is the `Scope` the dependency was registered with.
The scopes defined by `Microsoft.Extensions.DependencyInjection.ServiceLifetime` are Singleton, Scoped, and Transient.

An important caveat in Blazor is that the effective scope semantics vary by render mode.
In Interactive Server (formerly Blazor Server), scoped services are tied to the SignalR circuit.
In Interactive WebAssembly, scoped services behave like singletons because the application runs entirely in the browser.
Static Server Rendering creates a new scope per HTTP request, similar to a traditional ASP.NET Core application.
We will cover these differences in each sub-section.

Blazor also supports **keyed services** (introduced in .NET 8), which allow us to register multiple implementations of the same interface under different keys, and then resolve a specific implementation by key using the `[FromKeyedServices]` attribute. Additionally, the DI container can be configured to validate at startup that all registrations can be resolved, which helps catch configuration errors early.

The following pages explore each lifetime in detail:

- [Transient dependencies](transient-dependencies/) - a new instance every time a dependency is requested
- [Scoped dependencies](scoped-dependencies/) - one instance per scope (circuit, request, or application, depending on render mode)
- [Singleton dependencies](singleton-dependencies/) - one instance shared by all consumers
- [OwningComponentBase](owning-component-base/) - creating custom scopes for fine-grained control over dependency disposal
