function upFile() {
    document.getElementById("myFile").disabled = !0;
}
function slideForTable() {
    $(".follow").hide();
    var t = $(".cusor").children().length / $(".cusor").length - 1,
        e = $(".no_follow");
    for (let a = 0; a < t; a++) e.append('<div class="radiotext follow" style="display: none; margin-top: 3px;"><label></label></div>');
    $(".cusor").click(function () {
        $(this).parent().find(".follow").slideToggle();
    });
}
function addRow(t, e) {
    $(`.${t}`).click(function () {
        $(`#${e}`).each(function () {
            var t = "<tr>";
            jQuery.each($("tr:last td:eq(0)", this), function () {
                t += "<td>" + $(this).html() + "</td>";
            }),
                jQuery.each($("tr:last td:eq(1)", this), function () {
                    var e = parseInt($(this).html()) + 1;
                    t += "<td>" + e + "</td>";
                }),
                jQuery.each($("tr:last td:eq(2)", this), function () {
                    t += "<td>" + $(this).html() + "</td>";
                }),
                jQuery.each($("tr:last td:eq(3)", this), function () {
                    t += "<td>" + $(this).html() + "</td>";
                }),
                jQuery.each($("tr:last td:eq(4)", this), function () {
                    t += "<td>" + $(this).html() + "</td>";
                }),
                (t += "</tr>"),
                $("tbody", this).length > 0 ? $("tbody", this).append(t) : $(this).append(t);
        });
    });
}
$(document).ready(function () {
    function t(t, e) {
        let i = t.parent(),
            o = parseInt(i.attr("data-id")),
            d = $(`.${e}_buttons .next`),
            c = $(`.${e}_buttons .prev`);
        o > 1 && (i.attr("data-id", o - 1), a(o - 1, -1, e)), o - 1 == 1 && c.addClass("hide"), o - 1 < 5 && d.text("Tiếp tục"), o < 5 && d.attr("modal-show", "");
    }
    function e(t, e) {
        let i = t.parent(),
            o = parseInt(i.attr("data-id")),
            d = $(`.${e}_buttons .next`),
            c = $(`.${e}_buttons .prev`);
        o < 5 && (i.attr("data-id", o + 1), a(o + 1, 1, e)), o + 1 > 1 && c.removeClass("hide"), o + 1 === 5 && d.text("Hoàn thành"), 5 === o && d.attr("modal-show", "show");
    }
    function a(t, e, a) {
        let i = $(`.${a}_step-${e > 0 ? t : t + 1}`),
            o = (function (t, e) {
                let a = "";
                for (let i = 0; i < e; i++) a += `#${t}_step-${i + 1}${i < e - 1 ? ", " : ""}`;
                return a;
            })(a, 5);
        e > 0 ? i.addClass("active") : i.removeClass("active"), $(`${o}`).addClass("hide"), $(`#${a}_step-${t}`).removeClass("hide");
    }
    $(".select2").select2(),
        $(".form_time").timepicker({ minuteStep: 5, showInputs: !1, disableFocus: !0, defaultTime: !1, showMeridian: !1 }),
        $(".bars").click(function () {
            $(".main-left").toggleClass("slide"), $(".main-web").toggleClass("slide");
        }),
        $(function () {
            var t = function (t) {
                t.matches && $(".main-left").hasClass("slide") && ($(".main-left").removeClass("slide"), $(".main-web").removeClass("slide"));
            },
                e = window.matchMedia("(max-width: 991px)");
            t(e), e.addEventListener("change", () => t(e));
        }),
        $(".more-options").click(function () {
            $(this).children(".options").toggleClass("popup");
        }),
        $("#add-contract-customer-lookingup_modal .add-contract-customer_buttons .prev").click(function () {
            t($(this), $("#add-contract-customer-lookingup_modal").attr("data-id"));
        }),
        $("#add-contract-customer-lookingup_modal .add-contract-customer_buttons .next").click(function () {
            e($(this), $("#add-contract-customer-lookingup_modal").attr("data-id"));
        }),
        $("#add-contract_modal .add-contract_buttons .prev").click(function () {
            t($(this), $("#add-contract_modal").attr("data-id"));
        }),
        $("#add-contract_modal .add-contract_buttons .next").click(function () {
            e($(this), $("#add-contract_modal").attr("data-id"));
        }),
        $("#detail-customer-contract_modal .contract-detail_buttons .prev").click(function () {
            t($(this), $("#detail-customer-contract_modal").attr("data-id"));
        }),
        $("#detail-customer-contract_modal .contract-detail_buttons .next").click(function () {
            e($(this), $("#detail-customer-contract_modal").attr("data-id"));
        }),
        $("#edit-contract_modal .edit-contract_buttons .prev").click(function () {
            t($(this), $("#edit-contract_modal").attr("data-id"));
        }),
        $("#edit-contract_modal .edit-contract_buttons .next").click(function () {
            e($(this), $("#edit-contract_modal").attr("data-id"));
        }),
        $("#deposit-agreement_modal .deposit-agreement_buttons .prev").click(function () {
            t($(this), $("#deposit-agreement_modal").attr("data-id"));
        }),
        $("#deposit-agreement_modal .deposit-agreement_buttons .next").click(function () {
            e($(this), $("#deposit-agreement_modal").attr("data-id"));
        }),
        $(".date-select img").click(function () {
            $(this).prev().focus();
        }),
        $(".date-select img").click(function () {
            $(this).prev().focus();
        });
}),
    $("#id1, #id2, #id3, #id4").click(function () {
        $("#id1, #id2, #id3, #id4").removeClass("active"), $(this).addClass("active");
    }),
    $(".timepicker").timepicker({ minuteStep: 5, showInputs: !1, disableFocus: !0, defaultTime: !1, showMeridian: !1, autoclose: !0 }),
    $(".more_infor_common").click(function () {
        $(this).text(function (t, e) {
            return "Xem chi tiết -" == e ? "Xem chi tiết +" : "Xem chi tiết -";
        });
    }),
    $(".form_datetime").datetimepicker({ startView: 2, minView: 2, format: "dd/mm/yyyy", autoclose: !0 }),
    $("tr").click(function () {
        $(this).parent().children().removeClass("selected"), $(this).addClass("selected");
    }),
    slideForTable(),
    addRow("add_row1", "add_row_table1"),
    addRow("add_row2", "add_row_table2");
