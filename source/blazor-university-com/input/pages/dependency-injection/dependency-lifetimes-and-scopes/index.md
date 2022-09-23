---
title: "Dependency lifetimes and scopes"
date: "2020-05-02"
order: 2
---

Two important questions to ask when using dependency injection are: How long will the instances of these dependencies live,
and how many other objects have access to the same instance?

The answer depends on a number of factors,
mainly because different options have been made available to us so that we can make the decision for ourselves.
The first factor, and possibly the easiest to understand, is the `Scope` the dependency was registered with.
The scopes defined by `Microsoft.Extensions.DependencyInjection.ServiceLifetime` are Singleton, Scoped, and Transient.
