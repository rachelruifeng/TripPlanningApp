(function() {
    var username = document.getElementById("username");
    username.innerHTML = "Jie Dong";

    var main = document.getElementById("main");
    main.onmouseenter = function() {
        main.style["backgound-color"] = "#888";
    };
    main.onmouseleave = function() {
        main.style["backgound-color"] = "";
    };
})()