var message = document.querySelector('#searchbox');
var ul = document.querySelector('#search-results');
var query;
var searchModel = document.querySelector("#searchModel");

function searchResultsAppend(results) {

    console.log(JSON.stringify(results));

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
            li.classList.add("hover:bg-slate-700/75")
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
}

/*
    ctrl + C & esc key short triggers.
*/
document.onkeyup = function (e) {

    stopBodyFromScrolling();

    var searchModel = document.querySelector("#searchModel");
    if (e.which == 27) {
        makeBodyScrolable();
        searchModel.classList.add("hidden");
    } else if (e.ctrlKey && e.which == 67) {
        searchModel.classList.remove("hidden");
        message.focus();
    }
};

/*
    on click on main search window open a search model.
*/
function searchWindow() {

    stopBodyFromScrolling();

    var searchModel = document.querySelector("#searchModel");
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
        message = document.querySelector('#searchbox');
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

function makeBodyScrolable() {
    var body = document.querySelector("body");
    body.classList.remove("overflow-hidden");
}
