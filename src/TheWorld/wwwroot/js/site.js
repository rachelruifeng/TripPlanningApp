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
    var $icon = $("#sidebarToggle i.fa");
    $("#sidebarToggle").on("click", function() {
        $sidebarAndWrapper.toggleClass("hide-sidebar");
        if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
            $icon.removeClass("fa-angle-left");
            $icon.addClass("fa-angle-right");
        } else {
            $icon.addClass("fa-angle-left");
            $icon.removeClass("fa-angle-right");
        }
    }); 
})();