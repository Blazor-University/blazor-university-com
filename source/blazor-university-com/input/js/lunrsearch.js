var message = document.querySelector('#search-model-input');
var ul = document.querySelector('#search-results');
var searchModel = document.querySelector("#search-model");
var query;

function searchResultsAppend(results) {

    if (results.length == 0) {

        // clear child nodes
        ul.innerHTML = "";
        ul.textContent = "";

        const li = document.createElement("li");
        const a = document.createElement("a")
        a.innerHTML = "No results found";
        addClasses(a);
        a.href = "#";
        li.classList.add("hover:bg-slate-700/75")
        li.appendChild(a);
        ul.append(li);
    }
    else {

        // clear child nodes
        ul.innerHTML = "";
        ul.textContent = "";

        for (var i = 0; i < results.length; ++i) {
            var res = results[i];
            var content;
            const li = document.createElement("li");
            const a = document.createElement("a");
            addClasses(a);
            a.href = res.link;
            a.innerHTML = res.title;
            li.classList.add("dark:hover:bg-cyan-700/75");
            li.classList.add("hover:bg-cyan-700/75");
            li.classList.add("rounded-lg");
            li.appendChild(a);
            ul.append(li);
        }

    }

}

function addClasses(element) {
    element.classList.add("block");
    element.classList.add("p-4");
    element.classList.add("border");
    element.classList.add("border-gray-600");
    element.classList.add("text-ellipsis");
    element.classList.add("bg-clip-padding");
    element.classList.add("underline");
    element.classList.add("decoration-wavy");
    element.classList.add("decoration-sky-500");
    element.classList.add("rounded-lg");
}

/*
    ctrl + x & esc key short triggers.
    27 = esc key
    88 = x key
*/
document.onkeyup = function (e) {  

    var searchModel = document.querySelector("#search-model");    
    
    if (e.which == 27) {
        makeBodyScrollable();
        searchModel.classList.add("hidden");
    } else if (e.ctrlKey && e.which == 88) {
        stopBodyFromScrolling();
        onSearchMode();
        searchWindow();
        searchModel.classList.remove("hidden");
        message.focus();
    }
};

/*
    on click on main search window open a search model.
*/
function searchWindow() {

    stopBodyFromScrolling();

    var searchModel = document.querySelector("#search-model");
    message = document.querySelector('#search-model-input');
    searchModel.classList.remove("hidden");
    message.focus();
}

/*
    Attach a search listner to search input box.
    Which triggers search scripts
*/
function addEventListener() {
    // search trigger
    message.addEventListener('input', function () {
        query = this.value;
        if (query.length != 0) {
            var results = search.search(query, searchResultsAppend, false);
        }

    });
}

/*
    on window load attache keyboard listener
*/
window.onload = function () {
    if (message == null) {
        message = document.querySelector('#search-model-input');
        addEventListener();
    }

    if (ul == null) {
        ul = document.querySelector('#search-results');
    }
}


function stopBodyFromScrolling() {
    var body = document.querySelector("body");
    body.classList.add("overflow-hidden");
}

function makeBodyScrollable() {
    var body = document.querySelector("body");
    body.classList.remove("overflow-hidden");
}

function onSearchMode()
{
    window.scrollTo(0, 0);
}
