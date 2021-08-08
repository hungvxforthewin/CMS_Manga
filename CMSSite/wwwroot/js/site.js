// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//function alert(typeAlert, message) {
//    $.alert(message, {
//        // Enable auto close
//        autoClose: true,

//        // Auto close delay time in ms (>1000)
//        closeTime: 4000,
//        // Display a countdown timer
//        withTime: false,



//        // danger, success, warning or info
//        type: typeAlert,

//        // position+offeset
//        // top-left,top-right,bottom-left,bottom-right,center
//        position: ['center', [-0.42, 0]],

//        // Message title
//        title: false,


//        // For close button
//        close: '',

//        // <a href="https://www.jqueryscript.net/animation/">Animation</a> speed
//        speed: 'normal',

//        // Set to false to display multiple messages at a time
//        isOnly: true,

//        // Minimal space in PX from top
//        minTop: 10,


//        // onShow callback
//        onShow: function () {
//        },

//        // onClose callback
//        onClose: function () {
//        }

//    });
//}

$.showLoading = function () {
    $("#loading").modal({
        backdrop: "static", //remove ability to close modal with click
        keyboard: false, //remove option to close with keyboard
        show: true //Display loader!
    });
};

$.hideLoading = function () {
    $("#loading").modal("hide");
};

function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('-');
}

function htmlEncode(value) {
    if (value) {
        return $('<div/>').text(value).html();
    } else {
        return '';
    }
}

function displayImage(files, e) {
    if (FileReader && files && files.length) {
        var fr = new FileReader();
        fr.onload = function () {
            e.attr('src', fr.result);
        }
        fr.readAsDataURL(files[0]);
    }
    else {
        e.attr('src', '/images/no-image.png');
    }
}

function ShowModal(e) {
    $(e).addClass("show-modal");
    $(e).find(".content-modal").addClass("show-modal");
    $("body").addClass("active-modal");
}

function CloseModal(e) {
    $(e).removeClass("show-modal");
    $(e).find(".content-modal").removeClass("show-modal");
    $("body").removeClass("active-modal");
}

//function ShowModal(id) {
//    var ID = $(`#${id}`)[0];
//    $(ID).addClass("show-modal");
//    $(ID).find(".content-modal").addClass("show-modal");
//    $("body").addClass("active-modal");
//}

//function CloseModal(id) {
//    var ID = $(`#${id}`)[0];
//    setTimeout(function () {
//        $(".bs-modal").removeClass("show-modal");
//        $("body").removeClass("active-modal");
//    }, 500);
//    $(ID).parents(".bs-modal").find(".content-modal").removeClass("show-modal");
//}

function numberFormartAdmin(n) {
    n = n.replace(/[^0-9]+/g, '');
    return commaSeparateNumber(n);
}
function delayAdmin(callback, ms) {
    let timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}
function commaSeparateNumber(val) {
    if (!val && val != '0') return '';
    while (/(\d+)(\d{3})/.test(val.toString())) {
        val = val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
    }
    return val;
}

$(document).on('keyup', '.isNumber', delayAdmin(function (e) {
    let v = $(this).val();
    v = v.replace(/[^0-9]+/g, '');
    $(this).val(numberFormartAdmin(v));
}, 0));

$(document).on('keyup', '.isPercent', delayAdmin(function (e) {
    let v = $(this).val();
    v = v.replace(/[^0-9.]+/g, '');
    let inputValue = Math.round(v * 100) / 100;
    $(this).val(inputValue);
}, 1000));

$(document).on('keyup', '.isOneDecimalNumber', delayAdmin(function (e) {
    let v = $(this).val();
    if (v === '') {
        $(this).val('');
        return;
    }
    let inputValue = Math.round(v * 10) / 10;
    $(this).val(inputValue);
}, 1000));

$(document).on('keyup', '.revenue-range', delayAdmin(function (e) {
    let v1 = $(this).val();
    v1 = v1.replace(/[^0-9-]+/g, '');
    $(this).val(commaSeparateNumber(v1));
}, 1000));

$(function () {
    $(".form_datetime").attr('autocomplete', 'off');
    $(".only-month").attr('autocomplete', 'off');

    const countingElms = $(".counting");
    for (let i = 0; i < countingElms.length; i++) {
        const number = parseFloat($(countingElms[i]).attr("data-number"));
        const id = $(countingElms[i]).attr("id");
        const suffix = $(countingElms[i]).attr("data-suffix") || "";
        const prefix = $(countingElms[i]).attr("data-prefix") || "";
        let decimals = 0;
        if (number % 1 !== 0) decimals = 1;
        const options = {
            suffix,
            prefix,
        };
        let initCounting = new CountUp(
            id,
            0,
            parseFloat(number),
            decimals,
            2,
            options
        );
        if (!initCounting.error) {
            initCounting.start();
        } else {
            console.error(initCounting.error);
        }
    }
})

$(".bars").click(function () {
    $("#header").toggleClass("slide");
});

