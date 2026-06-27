---
title: "Creating a Blazor layout"
date: "2019-06-02"
order: 1
---

[![](images/SourceLink.png)](https://github.com/mrpmorris/blazor-university/tree/master/src/Layouts/CreatingALayout)

Any content you intend to act as a layout template for pages must descend from the `LayoutComponentBase` class.
To indicate a base component you add the following to the top of the `.razor` file.
```razor
@using <namespace of base class if necessary>
@inherits <name of base class>
```
To indicate where you want the content of your page to appear you simply output the contents of `Body`.

```razor
@inherits LayoutComponentBase

<div class="main">
  <header>
    <h1>This is the header</h1>
  </header>

  <div class="content">
    @Body
  </div>

  <footer>
    This is the footer
  </footer>
</div>
```
