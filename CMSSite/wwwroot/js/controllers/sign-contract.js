let baseUrl = '/Sale/InvestorContract/';

$(function () {
    $('.loading-wrapper').show();
    SetupPagination();

    $('#size-page select').on('change', function () {
        $('.loading-wrapper').show();
        SetupPagination();
    });

    $('#btn-search-contract').on('click', function () {
        $('.loading-wrapper').show();
        SetupPagination();
    });

    $('#sign-contract').on('click', function () {
        $('.loading-wrapper').show();
        let selectedRow = $('#table-body tr.selected');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một hợp đồng để khách hàng ký');
            $('.loading-wrapper').hide();
            return;
        }
        let id = selectedRow.data('id');
        if (id) {
            $.get(baseUrl + 'Sign?id=' + id, function (res) {
                if (res.result == 400) {
                    toastr.error(res.errors.join('<br />'));
                    $('.loading-wrapper').hide();
                }
                else {
                    $('#save-png').data('contract-id', id);
                    $('#customer-confirm_modal .content-modal').html(res);
                    ShowModal('#customer-confirm_modal');
                    toastr.success('Hệ thống đã gửi mã xác thực thành công. Vui lòng nhập OTP để xác thực hành động');
                    $('.loading-wrapper').hide();
                }
            });
        }
    });
});

var ConfirmOTP = function () {
    $('.loading-wrapper').show();
    var otp = $('#customer-confirm_modal input[name="otp"]').val();
    $.get(baseUrl + "ConfirmOTP", { otp: otp }, function (res) {
        //console.log(res);
        if (res.result != 400) {
            $('#customer-sign_modal .content-modal').html(res);
            CloseModal('#customer-confirm_modal');
            ShowModal('#customer-sign_modal');

            let id = $('#table-body tr.selected').data('id');
            $('#customer-sign_modal #save-png').data('contract-id', id);

            var canvas = document.getElementById('signature-pad');

            // Adjust canvas coordinate space taking into account pixel ratio,
            // to make it look crisp on mobile devices.
            // This also causes canvas to be cleared.
            function resizeCanvas() {
                // When zoomed out to less than 100%, for some very strange reason,
                // some browsers report devicePixelRatio as less than 1
                // and only part of the canvas is cleared then.
                var ratio = Math.max(window.devicePixelRatio || 1, 1);
                canvas.width = canvas.offsetWidth * ratio;
                canvas.height = canvas.offsetHeight * ratio;
                canvas.getContext("2d").scale(ratio, ratio);
            }

            window.onresize = resizeCanvas;
            resizeCanvas();

            var signaturePad = new SignaturePad(canvas, {
                minWidth: 1.1,
                maxWidth: 1.5,
                penColor: "#2215e6", //set pen color
                backgroundColor: 'rgb(255, 255, 255, 0)' // necessary for saving image as JPEG; can be removed is only saving as PNG or SVG
            });

            document.getElementById('save-png').addEventListener('click', function () {
                $('.loading-wrapper').show();
                if (signaturePad.isEmpty()) {
                    toastr.warning('Chưa ký hợp đồng điện tử');
                    $('.loading-wrapper').hide();
                    return;
                }

                var base64string = signaturePad.toDataURL('image/png');
                //console.log(base64string);
                var cid = $('#save-png').data('contract-id');
                $.post(baseUrl + 'Sign', { data: base64string, id: cid }, function (res) {
                    //console.log(res);
                    if (res.result === 200) {
                        toastr.success(res.message);
                        CloseModal('#customer-sign_modal');
                    }
                    else {
                        toastr.error(res.errors.join('<br />'));
                    }
                    $('.loading-wrapper').hide();
                });
            });

            document.getElementById('clear').addEventListener('click', function () {
                signaturePad.clear();
            });

            document.getElementById('undo').addEventListener('click', function () {
                var data = signaturePad.toData();
                if (data) {
                    data.pop(); // remove the last dot or line
                    signaturePad.fromData(data);
                }
            });

            $('.loading-wrapper').hide();
        }
        else {
            toastr.error(res.errors.join('<br />'));
            $('.loading-wrapper').hide();
        }
    });
}

var ResendOTP = function () {
    $('.loading-wrapper').show();
    $.get(baseUrl + "ResendOTP", function (res) {
        //console.log(res);
        if (res.result == 200) {
            toastr.success(res.message);
        }
        else {
            toastr.error(res.errors.join('<br />'));
        }
        $('.loading-wrapper').hide();
    });
}

// setup pagination
var SetupPagination = function () {
    $('#pagination').pagination({
        ajax: function (options, refresh, $target) {
            var data = $('#search-form').serializeArray();
            data.push({ name: "Page", value: options.current });
            data.push({ name: "Size", value: $('#size-page select').val() });
            $.ajax({
                url: baseUrl + "GetList",
                data: data,
                method: 'POST',
                dataType: 'json'
            }).done(function (res) {
                let div = $('#table-body');
                div.html(``);
                if (res.result != 400) {
                    $.each(res.data, function (index, item) {
                        div.append(`
                                                       <tr data-id="${item.id}">
                                                            <td>${item.contractCode}</td>
                                                            <td>
                                                                <button type="button" class="view-info"
                                                                        onclick="OpenPrintModel('${item.id}')">
                                                                    ${item.nameInvestor}
                                                                </button>
                                                            </td>
                                                            <td>${item.investmentAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}</td>
                                                            <td>${item.phone}</td>
                                                            <td>${item.stock.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}</td>
                                                            <td>${item.teleSaleName ?? ''}</td>
                                                            <td>${item.nameStatus}</td>
                                                        </tr>
                                                    `);
                    });
                    refresh({
                        total: res.total, // optional
                        length: $('#size-page select').val()// optional
                    });

                    $('#size-page .from').text($('#size-page select').val() * (options.current - 1) + 1);
                    $('#size-page .to').text($('#size-page select').val() * (options.current - 1) + res.data.length);
                    $('#size-page .total').text(res.total);
                }
                else {
                    toastr.error(res.errors.join('<br />'));
                    $('#size-page .from').text(0);
                    $('#size-page .to').text(0);
                    $('#size-page .total').text(0);
                    refresh({
                        total: 0, // optional
                        length: $('#size-page select').val()// optional
                    });
                }
                $('.loading-wrapper').hide();
            }).fail(function (error) {
                $('.loading-wrapper').hide();
            });
        }
    });
}

function OpenPrintModel(id) {
    $('.loading-wrapper').show();
    $.ajax({
        url: "/PrintContract/SelectContractTemplate",
        method: 'GET',
        data: {
        },
        success: function (res) {
            $('#select-contract-type_modal .content-modal').html(res);
            ShowModal('#select-contract-type_modal');
            $('#select-contract-type_modal select').select2();
            $('#hidden-contract-id').val(id);
            $('.loading-wrapper').hide();
        }
    }).done(function () {
        $('#select-contract-template').click(function () {
            selectPrintTemplate(PrintTab);
        });
    });
}

function PrintTab(printUrl, id) {
    $('.loading-wrapper').show();
    CloseModal('#select-contract-type_modal');
    $.ajax({
        url: printUrl,
        method: 'POST',
        data: {
            id: id
        },
        success: function (res) {
            var left = ($(window).width() / 2) - (900 / 2);
            top = ($(window).height() / 2) - (900 / 2);

            var mywindow = window.open('', 'report', "width=900,height=900, top=" + top + ", left=" + left);

            mywindow.document.write('<html><head>');

            mywindow.document.write('</head><body>');
            mywindow.document.write(`<div id="tools" style="position: fixed;">
    <button type="button" onClick="$('#tools').hide(); PreviewPrint(); $('#tools').show();">In</button>
    <button type="button" onClick="Export2Doc('print', 'hợp đồng')">Tải về</button>
</div>`);
            mywindow.document.write('<script src="/lib/jquery/dist/jquery.min.js"></script>');
            mywindow.document.write(res.datahtml);
            mywindow.document.write('</body></html>');
            mywindow.document.close();
            $('.loading-wrapper').hide();
        }
    });
}    