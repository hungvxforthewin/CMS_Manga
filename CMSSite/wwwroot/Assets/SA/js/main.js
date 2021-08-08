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
    $(".select2").select2();

    $(".form_time").timepicker({
        minuteStep: 5,
        showInputs: false,
        disableFocus: true,
        defaultTime: false,
        showMeridian: !1,
    });

    // menu
    $(".bars").click(function () {
        $(".main-left").toggleClass("slide");
        $(".main-web").toggleClass("slide");
    });

    $(function () {
        var handleMatchMedia = function (md) {
            if (md.matches) {
                if ($(".main-left").hasClass("slide")) {
                    $(".main-left").removeClass("slide");
                    $(".main-web").removeClass("slide");
                }
            }
        };
        var md = window.matchMedia("(max-width: 991px)");
        handleMatchMedia(md);
        md.addEventListener("change", () => handleMatchMedia(md));
    });

    $(".more-options").click(function () {
        $(this).children(".options").toggleClass("popup");
    });
    // id select
    $("#add-contract_modal .id-type").change((e) => {
        const dataId = e.target.value;
        const addressSelect = $(`.address-select`)
        for (let i = 0; i < addressSelect.length; i++) {
            let s = addressSelect[i]
            if ($(s).attr('data-id') === dataId) {
                $(s).addClass('active');
            }
            else if ($(s).hasClass('active')) {
                $(s).removeClass('active')
            }
        }
    })
    $('body').on('change', '#edit-contract_modal .id-type', function (e) {
        const dataId = e.target.value;
        const addressSelect = $(`.address-select`)
        for (let i = 0; i < addressSelect.length; i++) {
            let s = addressSelect[i]
            if ($(s).attr('data-id') === dataId) {
                $(s).addClass('active');
            }
            else if ($(s).hasClass('active')) {
                $(s).removeClass('active')
            }
        }
    });
    
    // **
    // contract
    // **

    // add contract customer
    $(
        "#add-contract-customer-lookingup_modal .add-contract-customer_buttons .prev"
    ).click(function () {
        let that = $(this);
        handlePrev(
            that,
            $("#add-contract-customer-lookingup_modal").attr("data-id")
        );
    });

    $(
        "#add-contract-customer-lookingup_modal .add-contract-customer_buttons .next"
    ).click(function () {
        let that = $(this);
        handleNext(
            that,
            $("#add-contract-customer-lookingup_modal").attr("data-id")
        );
    });

    // add contract
    $("#add-contract_modal .add-contract_buttons .prev").click(function (e) {
        e.preventDefault();
        let that = $(this);
        handlePrev(that, $("#add-contract_modal").attr("data-id"));
    });

    $("#add-contract_modal .add-contract_buttons .next").click(function (e) {
        e.preventDefault();
        let that = $(this);
        handleNext(that, $("#add-contract_modal").attr("data-id"));
    });

    // contract detail
    $("#detail-customer-contract_modal .contract-detail_buttons .prev").click(
        function () {
            let that = $(this);
            handlePrevNextView(that, $("#detail-customer-contract_modal").attr("data-id"));
        }
    );

    $("#detail-customer-contract_modal .contract-detail_buttons .next-view").click(
        function () {
            let that = $(this);
            handleNextView(that, $("#detail-customer-contract_modal").attr("data-id"));
        }
    );

    $('body').on('click', '#detail-customer-contract_modal .contract-detail_buttons .prev', function () {
        let that = $(this);
        handlePrevNextView(that, $("#detail-customer-contract_modal").attr("data-id"));
    })

    $('body').on('click', '#detail-customer-contract_modal .contract-detail_buttons .next-view', function () {
        let that = $(this);
        handleNextView(that, $("#detail-customer-contract_modal").attr("data-id"));
    })

    // edit contract
    $("#edit-contract_modal .edit-contract_buttons .prev").click(function () {
        let that = $(this);
        handlePrevNexUpdate(that, $("#edit-contract_modal").attr("data-id"));
    });

    $("#edit-contract_modal .edit-contract_buttons .next-update").click(function () {
        let that = $(this);
        handleNextUpdate(that, $("#edit-contract_modal").attr("data-id"));
    });

    $('body').on('click', '#edit-contract_modal .edit-contract_buttons .prev', function () {
        let that = $(this);
        handlePrevNexUpdate(that, $("#edit-contract_modal").attr("data-id"));
    })
    $('body').on('click', '#edit-contract_modal .edit-contract_buttons .next-update', function () {
        let that = $(this);
        handleNextUpdate(that, $("#edit-contract_modal").attr("data-id"));
    })
    // deposit agreement
    $("#deposit-agreement_modal .deposit-agreement_buttons .prev").click(function () {
        let that = $(this);
        handlePrev(that, $("#deposit-agreement_modal").attr("data-id"));
    });

    $("#deposit-agreement_modal .deposit-agreement_buttons .next").click(function () {
        let that = $(this);
        handleNext(that, $("#deposit-agreement_modal").attr("data-id"));
    });

    // deposit agreement edit
    $("#deposit-agreement_modal_edit .deposit-agreement_edit_buttons .prev").click(function () {
        let that = $(this);
        handlePrev(that, $("#deposit-agreement_modal_edit").attr("data-id"));
    });

    $("#deposit-agreement_modal_edit .deposit-agreement_edit_buttons .next").click(function () {
        let that = $(this);
        handleNext(that, $("#deposit-agreement_modal_edit").attr("data-id"));
    });
    $('body').on('click', '#deposit-agreement_modal_edit .deposit-agreement_edit_buttons .prev', function () {
        let that = $(this);
        handlePrevNexUpdate(that, $("#deposit-agreement_modal_edit").attr("data-id"));
    })
    $('body').on('click', '#deposit-agreement_modal_edit .deposit-agreement_edit_buttons .next-update', function () {
        let that = $(this);
        handleNextUpdate(that, $("#deposit-agreement_modal_edit").attr("data-id"));
    })
    // **
    // contract functions
    // **
    function handlePrev(that, id) {
        let parent = that.parent();
        let step = parseInt(parent.attr("data-id"));
        let next = $(`.${id}_buttons .next`);
        let prev = $(`.${id}_buttons .prev`);

        if (step > 1) {
            parent.attr("data-id", step - 1);
            updateSteps(step - 1, -1, id);
        }
        if (step - 1 === 1) {
            prev.addClass("hide");
        }
        if (step - 1 < 5) {
            next.text("Tiếp tục");
            next.removeAttr('id');
        }
        if (step < 5) {
            next.attr("modal-show", "");
        }
    }
    function handlePrevNextView(that, id) {
        let parent = that.parent();
        let step = parseInt(parent.attr("data-id"));
        let next = $(`.${id}_buttons .next-view`);
        let prev = $(`.${id}_buttons .prev`);

        if (step > 1) {
            parent.attr("data-id", step - 1);
            updateSteps(step - 1, -1, id);
        }
        if (step - 1 === 1) {
            prev.addClass("hide");
        }
        if (step - 1 < 5) {
            next.text("Tiếp tục");
            next.removeAttr('id');
        }
        if (step < 5) {
            next.attr("modal-show", "");
        }
    }
    function handleNext(that, id) {
        let parent = that.parent();
        let step = parseInt(parent.attr("data-id"));
        let next = $(`.${id}_buttons .next`);
        let prev = $(`.${id}_buttons .prev`);
        if (step < 5) {
            parent.attr("data-id", step + 1);
            updateSteps(step + 1, 1, id);
            next.removeAttr('id');
        }
        if (step + 1 > 1) {
            prev.removeClass("hide");
        }
        if (step + 1 === 5) {
            next.text("Hoàn thành");
            next.removeAttr('id');
        }
        if (step === 5) {
            /*next.attr("modal-show", "show");*/
            next.attr('id', 'btn-addContract-done');

        }
    }
    function handleNextView(that, id) {
        let parent = that.parent();
        let step = parseInt(parent.attr("data-id"));
        let next = $(`.${id}_buttons .next-view`);
        let prev = $(`.${id}_buttons .prev`);
        if (step < 5) {
            parent.attr("data-id", step + 1);
            updateSteps(step + 1, 1, id);
            next.removeAttr('id');
        }
        if (step + 1 > 1) {
            prev.removeClass("hide");
        }
        if (step + 1 === 5) {
            next.text("Đóng");
            next.removeAttr('id');
            next.attr('id', 'btn-addContract-close');
        }
        if (step === 5) {
            /*next.attr("modal-show", "show");*/
            next.attr('id', 'btn-addContract-close');

        }
    }
    function handlePrevNexUpdate(that, id) {
        let parent = that.parent();
        let step = parseInt(parent.attr("data-id"));
        let next = $(`.${id}_buttons .next-update`);
        let prev = $(`.${id}_buttons .prev`);

        if (step > 1) {
            parent.attr("data-id", step - 1);
            updateSteps(step - 1, -1, id);
        }
        if (step - 1 === 1) {
            prev.addClass("hide");
        }
        if (step - 1 < 5) {
            next.text("Tiếp tục");
            next.removeAttr('id');
        }
        if (step < 5) {
            next.attr("modal-show", "");
        }
    }
    function handleNextUpdate(that, id) {
        let parent = that.parent();
        let step = parseInt(parent.attr("data-id"));
        let next = $(`.${id}_buttons .next-update`);
        let prev = $(`.${id}_buttons .prev`);
        if (step < 5) {
            parent.attr("data-id", step + 1);
            updateSteps(step + 1, 1, id);
            next.removeAttr('id');
        }
        if (step + 1 > 1) {
            prev.removeClass("hide");
        }
        if (step + 1 === 5) {
            next.text("Update");
            next.removeAttr('id');
            next.attr('id', 'btn-updateContract-edit');
        }
        if (step === 5) {
            /*next.attr("modal-show", "show");*/
            next.attr('id', 'btn-updateContract-edit');

        }
    }
    function generateStepDetails(id, size) {
        let stepDetails = "";
        for (let i = 0; i < size; i++) {
            stepDetails += `#${id}_step-${i + 1}${i < size - 1 ? ", " : ""}`;
        }
        return stepDetails;
    }

    function updateSteps(i, val, id) {
        let step = $(`.${id}_step-${val > 0 ? i : i + 1}`);
        let stepDetails = generateStepDetails(id, 5);
        if (val > 0) {
            step.addClass("active");
        } else {
            step.removeClass("active");
        }
        $(`${stepDetails}`).addClass("hide");

        $(`#${id}_step-${i}`).removeClass("hide");
    }

    $(".date-select img").click(function () {
        $(this).prev().focus();
    });

    $(".date-select img").click(function () {
        $(this).prev().focus();
    });
});

$("#id1, #id2, #id3, #id4").click(function () {
    $("#id1, #id2, #id3, #id4").removeClass("active");
    $(this).addClass("active");
});

function upFile() {
    var x = document.getElementById("myFile");
    x.disabled = true;
}

$(".timepicker").timepicker({
    minuteStep: 5,
    showInputs: false,
    disableFocus: true,
    defaultTime: false,
    showMeridian: !1,
    autoclose: true
});

$(".more_infor_common").click(function () {
    $(this).text(function (i, old) {
        return old == "Xem chi tiết -" ? "Xem chi tiết +" : "Xem chi tiết -";
    });
});

$(".form_datetime").datetimepicker({
    startView: 2,
    minView: 2,
    format: "dd/mm/yyyy",
    autoclose: true
});

$('body').on('click', 'tr', function () {
    $(this).parent().children().removeClass("selected");
    $(this).addClass("selected");
});

function slideForTable() {
    $(".follow").hide();
    $(".cusor").click(function () {
        $(this).parent().find(".follow").slideToggle();
    });
}

slideForTable()


function addRow(add_row, add_row_table) {
    $(`.${add_row}`).click(function () {
        $(`#${add_row_table}`).each(function () {
            var tds = '<tr>';
            jQuery.each($('tr:last td:eq(0)', this), function () {
                tds += '<td>' + $(this).html() + '</td>';
            });
            jQuery.each($('tr:last td:eq(1)', this), function () {
                var t = parseInt($(this).html()) + 1;
                tds += '<td>' + t + '</td>';
            });
            jQuery.each($('tr:last td:eq(2)', this), function () {
                tds += '<td>' + $(this).html() + '</td>';
            });
            jQuery.each($('tr:last td:eq(3)', this), function () {
                tds += '<td>' + $(this).html() + '</td>';
            });
            jQuery.each($('tr:last td:eq(4)', this), function () {
                tds += '<td>' + $(this).html() + '</td>';
            });
            tds += '</tr>';
            if ($('tbody', this).length > 0) {
                $('tbody', this).append(tds);
            } else {
                $(this).append(tds);
            }
        });
    });
}

addRow("add_row1", "add_row_table1");
addRow("add_row2", "add_row_table2");