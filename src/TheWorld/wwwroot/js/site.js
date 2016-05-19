(function() {
//    var username = $("#username");
//    username.text("Jie Dong");
//
//    var main = $("#main");
//    main.on("onmouseenter", function() {
//        main.style["backgound-color"] = "#888";
//    });
//    main.on("mouseleave", function() {
//        main.style["backgound-color"] = "";
//    });
//
//    var menuItems = $("ul.menu li a");
//    menuItems.on("click", function () {
//        var me = $(this);
//        alert(me.text());
    //    });

    var $sidebarAndWrapper = $("#sidebar, #wrapper");

    $("#sidebarToggle").on("click", function() {
        $sidebarAndWrapper.toggleClass("hide-sidebar");
        if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
            $(this).text("Show");
        } else {
            $(this).text("Hide");
        }
    }); 
})();