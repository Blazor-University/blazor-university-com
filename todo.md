# blazor-university.com Modernization Plan — .NET 5 → .NET 10

## Important rules
1: Do not use en-dash or em-dash anywhere.
2: Emulate my style of writing as much as possible.

---

## Phase 1 — Infrastructure

### 1.1 Update Statiq.Web package
- **Current**: Statiq.Web v1.0.0-beta.49, Statiq.Docs v1.0.0-beta.3, Statiq.Lunr v1.0.0-beta.62
- **In `source/blazor-university-com/blazor-university-com.csproj`** (lines 9–11)
- **Task**: Check nuget.org for newer stable or beta releases compatible with `net10.0`
- **Sub-tasks**:
  - [x] Run `dotnet list package --outdated` in `source/blazor-university-com/`
  - [x] Updated: Statiq.Web `1.0.0-beta.49` → `1.0.0-beta.60`, Statiq.Docs `1.0.0-beta.3` → `1.0.0-beta.17`, Statiq.Lunr `1.0.0-beta.62` → `1.0.0-beta.72`
  - [x] Run `dotnet build` to verify no breaking changes in Statiq API

### 1.2 Verify build succeeds on current codebase
- **Task**: Establish a baseline — the build must pass before any content changes
- **Sub-tasks**:
  - [x] Run `dotnet build` from repo root
  - [x] Capture and fix any pre-existing build errors (not related to content)
  - [x] Document any Statiq pipeline warnings for later cleanup

### 1.3 Rename WordPress-style image filenames
- **Scope**: All images across all markdown files under `source/blazor-university-com/input/pages/`
- **Problem**: Many images have suffixes like `-300x251.png`, `-e1567978928628.png`, `-1024x427.jpg` — these are WordPress responsive-image artifacts
- **Sub-tasks**:
  - [ ] Glob for all image files: `Get-ChildItem -Recurse -Include *.png,*.jpg,*.jpeg,*.gif`
  - [ ] Identify files with WP-style suffixes
  - [ ] Rename files to clean names (e.g., `BlazorClientSide.png` instead of `BlazorClientSide-300x251.png`)
  - [ ] Update all `<img>` and `![](...)` references in markdown to match new filenames

### 1.4 Replace absolute blazor-university.com URLs with relative paths
- **Scope**: All markdown files
- **Examples found**:
  - `passing-html-element-references/index.md` line 58: `http://blazor-university.com/routing/#simulated-navigation`
  - `component-lifecycles/index.md` line 89: `https://blazor-university.com/components/render-trees/`
  - `component-lifecycles/index.md` line 107: `https://blazor-university.com/components/render-trees/`
  - `component-lifecycles/index.md` line 112: `http://blazor-university.com/components/render-trees/`
  - `layout/creating-a-blazor-layout/index.md` line 33: `http://blazor-university.com/javascript-interop/`
  - `creating-a-page/index.md` line 33: `http://blazor-university.com/routing/`
  - `dependency-injection/comparing-dependency-scopes/index.md` line 189: `https://blazor-university.com/...`
  - Many more across the codebase
- **Sub-tasks**:
  - [ ] Run `rg 'https?://blazor-university\.com' --include '*.md'` to find all occurrences
  - [ ] Replace each with a relative path (e.g., `/routing/` instead of `http://blazor-university.com/routing/`)
  - [ ] Ensure migrated links include the `.md` extension or use the Statiq URL convention

---



## Phase 2 — Corrections (Outdated Content)

### 2.1 Overview section

#### 2.1.1 `overview/blazor-hosting-models/index.md`
- **Lines 7–8**: "Blazor currently has two hosting models" — **WRONG for .NET 8+**. Now 4: Server, WebAssembly, Hybrid (.NET MAUI/WPF/WinForms), InteractiveAuto
- **Lines 35–37**: HTML comment TODO: "I don't think the Mono Framework is used anymore and AOT is out" — confirms known outdated content
- **Line 37**: "Ahead-of-time (AOT) compilation is planned for a future release" — AOT shipped in .NET 6 (and improved since), remove this caveat
- **Lines 39–40**: "Blazor Wasm does not yet support more than a single thread" — threading support is available since .NET 8 (experimental in 8, refined in 9/10)
- **Lines 148–153**: "Blazor Mobile Bindings" section — this experimental project was deprecated and never shipped. Remove entirely
- **Lines 44–145**: Server-side pros/cons section is largely still accurate, but update terminology from "server-side Blazor" to "Interactive Server rendering"
- **Sub-tasks**:
  - [ ] Rewrite opening: "Blazor currently has four hosting models: Server, WebAssembly, Hybrid, and InteractiveAuto"
  - [ ] Add Blazor Hybrid subsection (MAUI, WPF, WinForms)
  - [ ] Remove Mono Framework paragraph (lines 35–37)
  - [ ] Remove "AOT planned" sentence
  - [ ] Update "single-threaded" caveat to reflect threading support
  - [ ] Remove "Blazor Mobile Bindings" section (lines 148–153)
  - [ ] Update diagrams to show 4 models, not 2
  - [ ] Mention the unified Blazor Web App template introduced in .NET 8
  - [ ] Update `order:` in YAML frontmatter if page position in nav changes

#### 2.1.2 `overview/creating-a-new-project/index.md`
- **Lines 7–15**: References "Visual Studio Preview", "Blazor WebAssembly App" template, "ASP.NET Core hosted" checkbox — this is the .NET 5-era WASM template. Since .NET 8, the primary template is "Blazor Web App" with render mode selection
- **Sub-tasks**:
  - [ ] Rewrite to show `dotnet new blazor` CLI command with `--interactivity` flags
  - [ ] Show Visual Studio 2022+ "Blazor Web App" template with render mode options
  - [ ] Explain the new project structure (single project vs. separate Client/Server)
  - [ ] Include `--interactivity Server`, `--interactivity WebAssembly`, `--interactivity Auto` options

#### 2.1.3 `overview/creating-a-page/index.md`
- **Line 10**: References `MyFirstBlazorApp.Client` project and `Pages` folder — old WASM client project pattern
- **Sub-tasks**:
  - [ ] Update code example to new template structure (no `.Client` suffix)
  - [ ] Remove references to `SurveyPrompt` (not in current templates)
  - [ ] Update `_Imports.razor` section if needed
  - [ ] Ensure page paths reflect current project structure

#### 2.1.4 `overview/what-is-blazor/index.md`
- **Line 32**: Links to `https://github.com/dotnet/aspnetcore/tree/master/src/Components` — the `master` branch is now `main`
- **Lines 36–37**: ".NET foundation, at the time of writing it is supported by 3,700 companies and has 61,000 contributors" — refresh these stats or remove
- **Line 12**: Image `BlazorClientSide-300x251.png` — WP suffix, needs renaming (see 1.3)
- **Line 38**: Image `NetFoundationStats.png` — may need updating or removal
- **Sub-tasks**:
  - [ ] Update GitHub link to `main` branch
  - [ ] Remove or update .NET Foundation stats
  - [ ] Update/rename images

#### 2.1.5 `overview/installing-blazor/index.md`
- **Lines 7–9**: ".NET Core 3.2.0" and "Visual Studio 16.6" — extremely outdated
- **Sub-tasks**:
  - [ ] Rewrite for .NET 10 SDK installation
  - [ ] Show `dotnet --list-sdks` and `dotnet new` verification
  - [ ] Link to current download page
  - [ ] Remove VS 2019 references, use VS 2022

### 2.2 Components section

#### 2.2.1 `components/index.md`
- **Lines 13–15**: References `obj\Debug\netcoreapp3.0\Razor\Pages\Counter.razor.g.cs` — should be `net10.0`
- **Line 14**: "Blazor 3 apps" and "Blazor 5 or later" — update terminology
- **Line 27**: `namespace MyFirstBlazorApp.Client.Pages` — old WASM client pattern
- **Sub-tasks**:
  - [ ] Update framework version references to .NET 10
  - [ ] Update namespace examples from `MyFirstBlazorApp.Client.Pages` to current convention
  - [ ] Update generated file paths (`netcoreapp3.0` → `net10.0`)
  - [ ] Remove "after Blazor version 3 these files are no longer automatically written to disk" — reference current versions

#### 2.2.2 `components/creating-a-component/index.md`
- **Line 25**: `MyFirstBlazorApp.Client.Components.MyFirstComponent` — old WASM client namespace
- **Lines 39–40**: `SurveyPrompt` component — not in current templates
- **Sub-tasks**:
  - [ ] Update namespace to modern template convention
  - [ ] Replace `SurveyPrompt` with current template component (e.g., nothing, or a simple example)
  - [ ] Update source link to point to .NET 10 example code

### 2.3 Routing section

#### 2.3.1 `routing/defining-routes/index.md`
- **Line 25**: HTML comment TODO: "do we care about Blazor 3 anymore?" — answer: no
- **Line 26**: `obj\Debug\netcoreapp3.0\Razor\Pages\Index.razor.g.cs` — outdated path
- **Line 27**: `{DotNetVersion}` placeholder — hardcode to current version or use generic wording
- **Line 30**: "As of Blazor V5" — outdated version reference
- **Line 49**: `typeof(Startup).Assembly` — `Startup` class no longer exists in .NET 6+ templates
- **Sub-tasks**:
  - [ ] Remove TODO comment
  - [ ] Update framework version references to .NET 10
  - [ ] Replace `typeof(Startup).Assembly` with `typeof(App).Assembly` or `typeof(Program).Assembly` based on current template
  - [ ] Update generated file paths

#### 2.3.2 `routing/404-not-found/index.md` (file is at `routing/404-not-found/index.md`)
- **Line 17**: `typeof(Program).Assembly` — WASM-specific reference; update for unified hosting model
- **Sub-tasks**:
  - [ ] Change `typeof(Program).Assembly` to `typeof(App).Assembly` (works in both Server and WASM modes in .NET 8+)
  - [ ] Update any other WASM-specific code references

### 2.4 Dependency Injection section

#### 2.4.1 `dependency-injection/injecting-dependencies-into-blazor-components/index.md`
- **Lines 104–117**: "Registering injectables in a Blazor Server app" — references `Startup` class with `ConfigureServices` method. In .NET 6+, templates use `WebApplication` builder with `builder.Services`
- **Lines 119–120**: "This is the same for WASM applications when we check the ASP.NET Core hosted checkbox" — old template language
- **Lines 124–143**: "Registering injectables in a Blazor WASM app" — references `Program.Main` with `WebAssemblyHostBuilder.CreateDefault(args)`. While WASM still uses this pattern, the article should explain the unified model
- **Line 162**: "At the moment, constructor injection is not supported" — constructor injection IS supported in Blazor components since .NET 6+ in certain scenarios
- **Sub-tasks**:
  - [ ] Replace `Startup.ConfigureServices` with `builder.Services` in server-side examples
  - [ ] Update WASM section to show `builder.Services` (already partially correct but update surrounding text)
  - [ ] Update constructor injection caveat — mention it works for classes not descending from `ComponentBase`, and `[Inject]` is still needed for components
  - [ ] Remove "ASP.NET Core hosted checkbox" language

#### 2.4.2 `dependency-injection/dependency-lifetimes-and-scopes/comparing-dependency-scopes/index.md`
- **Line 88**: "Edit the Startup.cs file, and in the `ConfigureServices` method" — needs update
- **Lines 174–180**: References `_Host.cshtml` and `render-mode="ServerPrerendered"` — in .NET 8+ Blazor Web App, this is configured in `Program.cs` via `app.MapRazorComponents<App>()` and render mode attributes on components
- **Sub-tasks**:
  - [ ] Replace `Startup.cs` registration with `builder.Services` pattern
  - [ ] Replace `_Host.cshtml`/`ServerPrerendered` references with current render mode configuration
  - [ ] Update the prerendering discussion to reflect current Blazor Web App patterns

#### 2.4.3 `dependency-injection/dependency-lifetimes-and-scopes/transient-dependencies/index.md`
- **Line 54**: "In a server-side application edit `Startup.ConfigureServices`" — replace with `builder.Services`
- **Line 60**: "In a WebAssembly application edit `Program.Main`" — update to show WASM `builder.Services` approach
- **Sub-tasks**:
  - [ ] Replace `Startup.ConfigureServices` with `builder.Services`
  - [ ] Update WASM registration section to match current template

#### 2.4.4 `dependency-injection/dependency-lifetimes-and-scopes/singleton-dependencies/index.md`
- **Line 78**: "open **Startup.cs** and in `ConfigureServices`" — replace with `builder.Services`
- **Line 113**: `WeatherForecastService` in `Startup.cs` — update
- **Sub-tasks**:
  - [ ] Replace `Startup.cs` references with `builder.Services`
  - [ ] Update any applicable code snippets

#### 2.4.5 `dependency-injection/dependency-lifetimes-and-scopes/scoped-dependencies/index.md`
- No explicit `Startup.cs` references found but review for contextual updates

#### 2.4.6 `dependency-injection/component-scoped-dependencies/owningcomponentbase-generic/index.md`
- **Lines 105–115**: `ConfigureServices` in `Startup.cs` for `WeatherForecastService` registration — update to `builder.Services`
- **Sub-tasks**:
  - [ ] Replace `Startup.cs` references
  - [ ] Update code examples

### 2.5 Forms section

#### 2.5.1 `forms/writing-custom-validation/index.md`
- **Line 28**: References `blazor-validation` GitHub project by mrpmorris — verify this still exists/works with .NET 10
- **Line 121**: "registered in our app's `Startup.ConfigureServices` method" — replace with `builder.Services`
- **Lines 257–264**: Registration in `Startup.ConfigureServices` — update to `builder.Services`
- **Sub-tasks**:
  - [ ] Replace all `Startup.ConfigureServices` references with `builder.Services`
  - [ ] Verify FluentValidation package compatibility with .NET 10
  - [ ] Update code snippets

### 2.6 JavaScript Interop section

#### 2.6.1 `javascript-interop/index.md`
- **Lines 7–13**: Lists browser APIs unavailable to WebAssembly: Media Capture, Popups, Web GL, Web Storage — **most are now available** via Blazor's built-in JS interop or dedicated NuGet packages. Blazor 8+ has improved browser API access
- **Sub-tasks**:
  - [ ] Remove or rephrase the "currently unsupported" list — these are now accessible through JS interop
  - [ ] Add note that .NET 8+ provides many built-in abstractions for browser APIs

#### 2.6.2 `javascript-interop/calling-javascript-from-dotnet/index.md`
- **Lines 7–8**: "JavaScript should be added into either **/Pages/_Host.cshtml** in Server-side Blazor apps, or in **wwwroot/index.html** for Web Assembly Blazor apps" — since .NET 8, Blazor Web Apps use `Components/App.razor` for the server project, and the script placement differs
- **Sub-tasks**:
  - [ ] Update script placement guidance for .NET 8+ Blazor Web App model
  - [ ] Mention collocated JS (`{ComponentName}.razor.js`) as the modern approach
  - [ ] Keep backward compatibility note for existing patterns

#### 2.6.3 `javascript-interop/calling-javascript-from-dotnet/updating-the-document-title/index.md`
- **ENTIRE ARTICLE**: Shows a manual JS approach to set `document.title` — since .NET 6, Blazor has the built-in `PageTitle` component. This should be the primary approach, with JS interop as a fallback for advanced cases
- **Line 37**: References `_Host.cshtml` for script inclusion
- **Line 72**: References changing render mode from `ServerPrerendered` to `Server` in `_Host.cshtml`
- **Sub-tasks**:
  - [ ] Add introductory paragraph: "Since .NET 6, Blazor provides the built-in `PageTitle` component..."
  - [ ] Show `PageTitle` usage as primary method: `<PageTitle>@Title</PageTitle>`
  - [ ] Move JS interop approach to secondary/alternative section
  - [ ] Replace `_Host.cshtml` references with current Blazor Web App patterns

#### 2.6.4 `javascript-interop/calling-dotnet-from-javascript/calling-static-dotnet-methods/index.md`
- **Line 19**: "Create a new Blazor server-side application" — update to "Blazor Web App"
- **Line 47**: "Edit the **/Startup.cs** file" — replace with `Program.cs` / builder pattern
- **Lines 51–59**: Constructor injection of `IConfiguration` in `Startup` class — update for `WebApplication` builder
- **Line 82**: "Edit the **/Pages/_Host.cshtml** file" — update to `Components/App.razor` or `wwwroot/index.html`
- **Line 167**: Again references `_Host.cshtml` for `Blazor.start` configuration
- **Sub-tasks**:
  - [ ] Replace all `Startup.cs` references with `Program.cs` builder pattern
  - [ ] Replace all `_Host.cshtml` references with current template files
  - [ ] Update `Blazor.start` section for current bootstrapping patterns

#### 2.6.5 `javascript-interop/javascript-boot-process/index.md`
- **Line 38–39**: References `_Host.cshtml` and `ServerPrerendered` → `Server` — update for current templates
- **Line 43**: Script reference in `_Host.cshtml` — update
- **Sub-tasks**:
  - [ ] Update `_Host.cshtml` references for current project structure
  - [ ] Keep the conceptual explanation (JS boots before Blazor) — it's still correct

### 2.7 Layouts section

#### 2.7.1 `layouts/creating-a-blazor-layout/index.md`
- **Line 31**: "within the `<app>` element in a default Blazor application" — since .NET 8, Blazor Web Apps define the root component in `App.razor` instead. The `wwwroot/index.html` with `<app>` element is only for WASM standalone
- **Sub-tasks**:
  - [ ] Update DOM root element description for current Blazor Web App model
  - [ ] Differentiate between Server rendering (no `<app>` element, uses `Components/App.razor`) and WASM (`wwwroot/index.html`)

### 2.8 Component Libraries section

#### 2.8.1 `component-libraries/index.md`
- **Lines 41–47**: Separate guidance for "Client-side Blazor" vs "Server-side Blazor" — since .NET 8, Blazor Web Apps unify both. Script referencing should be explained for the unified model
- **Sub-tasks**:
  - [ ] Rewrite the script consumption section for the Blazor Web App model
  - [ ] Explain that collocated JS modules simplify this: `MyComponent.razor.js`

---

## Phase 3 — Additions (New Topics)

### 3.1 Render Modes (new section: `/pages/render-modes/`)

#### 3.1.1 `render-modes/index.md` — Overview
- Cover the 4 render modes introduced in .NET 8: Static Server, Interactive Server, Interactive WebAssembly, Interactive Auto
- Explain the `RenderMode` attribute: `@rendermode InteractiveServer`
- Explain the `@rendermode` directive at component/page level
- Show how to set render mode in `Program.cs` via `AddInteractiveServerComponents()`, `AddInteractiveWebAssemblyComponents()`
- Explain the Blazor Web App project template options (`--interactivity` flags)
- [ ] Add YAML frontmatter with `order:` to position correctly in nav tree

#### 3.1.2 `render-modes/interactive-server.md`
- Covers the `InteractiveServer` render mode and `InteractiveServerComponent`
- Prerendering behavior, SignalR circuit lifetime
- When to use vs. when not to use
- [ ] Add YAML frontmatter with `order:`

#### 3.1.3 `render-modes/interactive-webassembly.md`
- Covers `InteractiveWebAssembly` render mode
- Prerendering + WASM download behavior
- When to use
- [ ] Add YAML frontmatter with `order:`

#### 3.1.4 `render-modes/interactive-auto.md`
- Covers `InteractiveAuto` — starts with Server, transitions to WASM after download
- Best of both worlds, complexity tradeoffs
- [ ] Add YAML frontmatter with `order:`

#### 3.1.5 `render-modes/streaming-rendering.md`
- `[StreamRendering]` attribute for async page content
- How it works, when to use
- [ ] Add YAML frontmatter with `order:`

### 3.2 Blazor Hybrid (new section: `/pages/hybrid/`)
- .NET MAUI Blazor: `BlazorWebView` control, shared components
- WPF/WinForms: `BlazorWebView` for desktop
- Blazor Hybrid architecture (native shell + Blazor UI)
- Comparison with Server and WASM
- [ ] Create `/pages/hybrid/index.md` with YAML frontmatter including `order:`
- [ ] Ensure nav position by checking `order:` values of sibling sections

### 3.3 Component additions (under `/pages/components/` or new sub-pages)

#### 3.3.1 `PageTitle`, `HeadContent`, `HeadOutlet`
- `PageTitle` component: set document title declaratively
- `HeadContent`: add elements to `<head>`
- `HeadOutlet`: required in `App.razor` to render head content
- Replace the existing manual JS approach in updating-the-document-title
- [ ] Determine location (under components/ or a new section), create file with `order:`

#### 3.3.2 `ErrorBoundary`
- New in .NET 8
- Error boundary component for handling rendering exceptions
- Custom error content via `ErrorContent` parameter
- `OnErrorAsync` callback for logging
- [ ] Create file with `order:` in frontmatter

#### 3.3.3 `Virtualize`
- Virtual scrolling component for large lists
- `Items`, `ItemSize`, `OverscanCount` parameters
- `ItemsProvider` for async loading
- Placeholder content
- [ ] Create file with `order:` in frontmatter

#### 3.3.4 CSS isolation
- Scoped CSS files (`{Component}.razor.css`)
- `bundleconfig.json` / CSS isolation bundling
- `::deep` pseudo-selector for child component styling
- How it works at build time
- [ ] Create file with `order:` in frontmatter

#### 3.3.5 `EditorRequired` parameter
- New in .NET 7/8
- `[EditorRequired]` attribute on component parameters
- Build-time warnings for missing required parameters
- Usage examples
- [ ] Create file with `order:` in frontmatter

#### 3.3.6 `[ExcludeFromCodeCoverage]`
- Attribute for excluding components from coverage
- Usage scenarios
- [ ] Create file with `order:` in frontmatter

#### 3.3.7 `SectionOutlet` / `SectionContent`
- New in .NET 8
- Named slot pattern for components
- `SectionOutlet` defines a named region, `SectionContent` fills it
- [ ] Create file with `order:` in frontmatter

### 3.4 Routing additions (under `/pages/routing/` or new sub-pages)

#### 3.4.1 `FocusOnNavigate`
- New in .NET 8
- Auto-focus an element after navigation
- Accessibility use case
- [ ] Create under `/pages/routing/` with `order:` in frontmatter

#### 3.4.2 `NavigationLock`
- New in .NET 8
- Prevent navigation (e.g., unsaved form changes)
- `OnBeforeInternalNavigation` event
- [ ] Create under `/pages/routing/` with `order:` in frontmatter

#### 3.4.3 `SupplyParameterFromQuery`
- New in .NET 8
- Bind component parameters from query string
- `[SupplyParameterFromQuery]` attribute
- `Name` property for custom query param name
- [ ] Create under `/pages/routing/` with `order:` in frontmatter

### 3.5 Forms additions (under `/pages/forms/` or new sub-pages)

#### 3.5.1 `SupplyParameterFromForm` and enhanced form handling
- `[SupplyParameterFromForm]` for SSR forms
- Antiforgery token support
- Form naming with `FormName` parameter
- Enhanced form handling (automatic form posts without full page reload)
- [ ] Create under `/pages/forms/` with `order:` in frontmatter

#### 3.5.2 `InputRadio` / `InputRadioGroup`
- New input components for radio button groups
- Usage examples
- [ ] Create under `/pages/forms/` with `order:` in frontmatter

### 3.6 JavaScript Interop additions (under `/pages/javascript-interop/`)

#### 3.6.1 `[JSImport]` / `[JSExport]` attributes
- New in .NET 7/8 for WASM
- `[JSImport]`: import JS functions into C#
- `[JSExport]`: export C# methods to JS
- Generated source code approach (no manual `IJSRuntime` for many cases)
- Comparison with the classic `IJSRuntime` approach
- [ ] Add to javascript-interop section with `order:` in frontmatter

### 3.7 Performance / Features (new sub-pages)

#### 3.7.1 Blazor WebAssembly AOT compilation
- Update existing AOT mention in hosting-models
- Full article on enabling AOT: `<RunAOTCompilation>true</RunAOTCompilation>`
- Size tradeoffs, performance benefits
- [ ] Create article with `order:` in frontmatter (consider placing under a performance section or under overview)

#### 3.7.2 Razor compilation with source generators
- How Razor files are compiled using Roslyn source generators (since .NET 6)
- No more `.g.cs` files on disk by default
- How to re-enable with `<EmitCompilerGeneratedFiles>`
- [ ] Create article with `order:` in frontmatter

#### 3.7.3 QuickGrid
- New in .NET 8
- High-performance data grid component
- Sorting, filtering, pagination
- Custom templates
- [ ] Create article under components section with `order:` in frontmatter

#### 3.7.4 Enhanced navigation & form handling
- .NET 8's enhanced navigation (no full page reload on clicks)
- Enhanced form handling (automatic streaming updates)
- How it works, opt-out via `data-enhance-nav="false"`
- [ ] Create article with `order:` in frontmatter (consider placing under routing section)

### 3.8 Prerendering (new sub-page)

#### 3.8.1 Component state persistence
- `PersistentComponentState` and `PersistingComponentState`
- How to preserve state across prerender → interactive handoff
- `PreserveComponentStateAttribute` / `@persistComponentState`
- Fixing the "loaded twice" problem
- [ ] Create article with `order:` in frontmatter (consider placing under components section)

---

## Phase 4 — Removals

### 4.1 Remove Blazor Mobile Bindings
- **File**: `overview/blazor-hosting-models/index.md`, lines 148–153
- **Reason**: This experimental project was deprecated and never shipped as a product
- **Action**: Remove the subsection entirely

### 4.2 Remove all `Startup` class code examples
- **Files affected**:
  - `dependency-injection/injecting-dependencies-into-blazor-components/index.md` (lines 106–117)
  - `dependency-injection/dependency-lifetimes-and-scopes/comparing-dependency-scopes/index.md` (line 88)
  - `dependency-injection/dependency-lifetimes-and-scopes/transient-dependencies/index.md` (line 54)
  - `dependency-injection/dependency-lifetimes-and-scopes/singleton-dependencies/index.md` (line 78)
  - `dependency-injection/component-scoped-dependencies/owningcomponentbase-generic/index.md` (lines 105–115)
  - `forms/writing-custom-validation/index.md` (lines 121, 257–264)
  - `javascript-interop/calling-dotnet-from-javascript/calling-static-dotnet-methods/index.md` (lines 47, 51–59)
  - `routing/defining-routes/index.md` (line 49)
- **Action**: Replace every `Startup.ConfigureServices(IServiceCollection services)` pattern with `builder.Services` from the `WebApplication` builder pattern

### 4.3 Remove all `_Host.cshtml` patterns
- **Files affected**:
  - `javascript-interop/calling-javascript-from-dotnet/index.md` (line 7)
  - `javascript-interop/calling-javascript-from-dotnet/updating-the-document-title/index.md` (lines 37, 72)
  - `javascript-interop/calling-javascript-from-dotnet/passing-html-element-references/index.md` (line 87)
  - `javascript-interop/calling-dotnet-from-javascript/index.md` (line 97)
  - `javascript-interop/calling-dotnet-from-javascript/calling-static-dotnet-methods/index.md` (lines 82, 167)
  - `javascript-interop/javascript-boot-process/index.md` (lines 38–39, 43)
  - `dependency-injection/dependency-lifetimes-and-scopes/comparing-dependency-scopes/index.md` (lines 174–180)
  - `component-libraries/index.md` (line 47)
- **Action**: Replace with current Blazor Web App equivalents (`Components/App.razor`, collocated JS, etc.)

### 4.4 Remove `netcoreapp3.0` / `netstandard2.0` framework references
- **Files affected**:
  - `components/index.md` (lines 13–14)
  - `routing/defining-routes/index.md` (line 26)
- **Action**: Update to `net10.0` or use generic version-independent language

### 4.5 Remove `SurveyPrompt` component references
- **Files affected**:
  - `overview/creating-a-page/index.md` (indirect)
  - `components/creating-a-component/index.md` (line 40)
  - `components/render-trees/index.md` (line 51)
- **Action**: Remove or replace with current template content

### 4.6 Replace old WASM `Program.Main` bootstrap where it's presented as the only pattern
- **Files affected**: `dependency-injection/injecting-dependencies-into-blazor-components/index.md` (lines 124–143)
- **Action**: Keep as one pattern but present alongside unified model; update to current syntax

---

## Phase 5 — Verification

### 5.1 Verify all GitHub source code links
- Pattern: `https://github.com/mrpmorris/blazor-university/tree/master/src/...`
- Found in most articles as an image link: `[![](images/SourceLink.png)](...)`
- **Sub-tasks**:
  - [ ] Collect all unique GitHub URLs from all markdown files
  - [ ] Verify each resolves (they point to `master` branch, which should still work)
  - [ ] Create a tracking issue if the companion repo also needs updating to .NET 10

### 5.2 Run `dotnet build` and fix Statiq pipeline errors
- **Sub-tasks**:
  - [ ] Run build after Phases 1–4
  - [ ] Fix any broken links (internal cross-references)
  - [ ] Fix missing image references after renames
  - [ ] Verify Statiq generates correct HTML output

### 5.3 Verify Lunr search indexing works
- **Current**: Statiq.Lunr v1.0.0-beta.72
- **Sub-tasks**:
  - [ ] After content changes, verify search still returns results
  - [ ] Check that new pages are indexed
  - [ ] Check that renamed/removed pages don't cause search errors

### 5.4 Test navigation tree
- **Sub-tasks**:
  - [ ] Verify `_directory.yml` has no orphaned entries
  - [ ] Verify all new pages appear in navigation
  - [ ] Test that removing pages doesn't break the tree
  - [ ] Verify order/sort of navigation items is sensible

---

## Appendix: Files NOT requiring changes

These files were reviewed and contain no outdated patterns or references:

- `components/code-behind/index.md` — Still accurate; code-behind pattern hasn't changed
- `components/component-lifecycles/index.md` — Lifecycle model is still correct
- `components/render-trees/index.md` — Render tree concepts haven't changed
- `components/multi-threaded-rendering/index.md` — Threading behavior remains same
- `components/cascading-values/index.md` — Cascading values pattern unchanged
- `components/literals-expressions-and-directives/directives/index.md` — Directives still work the same
- `components/one-way-binding/index.md` — Binding concepts unchanged
- `components/two-way-binding/index.md` — Binding concepts unchanged
- `components/generics/index.md` — .NET generics in components unchanged
- `routing/navigating-our-app-via-code/index.md` — `NavigationManager` still used the same way
- `routing/navigating-our-app-via-html/index.md` — `NavLink` still works the same
- `routing/route-parameters/index.md` — Route parameters unchanged
- `routing/optional-route-parameters/index.md` — Optional params unchanged
- `routing/constraining-route-parameters/index.md` — Route constraints unchanged
- `routing/detecting-navigation-events/index.md` — Navigation events unchanged
- `forms/index.md` — Forms overview still correct
- `forms/editcontext-fieldidentifiers-and-fieldstate/index.md` — EditContext concepts unchanged
- `forms/editing-form-data/index.md` — `InputBase<T>` still relevant
- `forms/handling-form-submission/index.md` — Form submission unchanged
- `forms/accessing-form-state/index.md` — Form state unchanged
- `forms/validation/index.md` — DataAnnotations still the built-in approach
- `forms/descending-from-inputbase/index.md` — Custom input component still valid
- `javascript-interop/calling-dotnet-from-javascript/index.md` — .NET→JS calling still correct (minus `_Host.cshtml` ref)
- `javascript-interop/calling-dotnet-from-javascript/lifetimes-and-memory-leaks/index.md` — Lifetimes still correct
- `javascript-interop/calling-dotnet-from-javascript/type-safety/index.md` — Type safety still correct
- `javascript-interop/calling-javascript-from-dotnet/passing-html-element-references/index.md` — Element references still correct
- `templating-components-with-renderfragements/index.md` — RenderFragment patterns unchanged
- `layouts/using-layouts/index.md` — Layout usage unchanged
