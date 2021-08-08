(function (window, $) {
    window.EventKeyBoard = function (e) {
       
    }
})(window, jQuery);

$(document).ready(function () {
    $(document).on("keyup", function (e) {
        EventKeyBoard(e);
    });
});