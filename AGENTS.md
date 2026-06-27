We are updating a very old Blazor tutorial website to cover the latest versions of Blazor. The content was originally written for the early days of Blazor and needs to be brought up to date with modern Blazor (including Interactive Server, Interactive WebAssembly, Interactive Auto, Static Server Rendering, and Blazor Hybrid).

The pages' contents are rooted in `source\blazor-university-com\input\pages\`.

Pages are organized in nested subfolders. A page named `name-of-page` is found in `index.md` inside the folder `name-of-page`. For example, the page "What is WebAssembly?" lives at `source\blazor-university-com\input\pages\overview\what-is-webassembly\index.md`. When the user refers to a page by name, search for a folder with that name containing an `index.md`.

Writing style notes:

- Prefer prose over bullet lists. Use headings to structure content.
- Use bold sparingly. Reserve it for key terms, warnings, and UI labels.
- Avoid em-dashes and en-dashes. Use commas or parentheses instead.
- Use contractions rarely. Prefer "does not" over "doesn't", "is not" over "isn't", and so on.
- Write in a conversational but authoritative tone. Use "we" and "our" to create a shared learning journey. Use "I" when giving personal recommendations.
- Use code blocks heavily. Tag them with the appropriate language such as `razor`, `cs`, `xml`, or `html`.
- Keep headings short and descriptive. Use `##` and `###` only. Never use `#` in the body.
- Use relative markdown links for internal references. Use standard `[text](url)` for external links.
- Use the problem-solution pattern. Raise a question, then answer it through code and explanation.
- Place `.` after `)` when a sentence ends with a closing parenthesis.
