// simpleControl.js
(function () {
    "use strict";
    angular.module("simpleControls", []).directive("waitCursor", waitCursor);

    function waitCursor() {
        return {
            scope: {
                show: "=displayWhen"
            },
            restric: "E", //restrict to element style
            templateUrl:"/views/waitCursor.html"
        };
    }
})();