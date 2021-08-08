let baseUrl = '/SaleAdmin/ShowUp/';
let selectUrl = '/SelectionData/';
$(function () {
    LoadProductSelect('#myModal_add_new');
    LoadProductSelect('#myModal_add_new_for_cc');

    $('#Edit-Show-Up').on('click', function () {
        $('.loading-wrapper').show();
        let selectedRow = $('#table-body tr.selected');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một show up để cập nhật');
            $('.loading-wrapper').hide();
            return;
        }
        let id = selectedRow.data('id');
        if (id) {
            $.get(baseUrl + 'Update?id=' + id, function (res) {
                if (res.result == 400) {
                    toastr.error(res.errors.join('<br />'));
                    $('.loading-wrapper').hide();
                }
                else {
                    $('#myModal_fix_data .content-modal').html(res);
                    let code = $('#myModal_fix_data .content-modal input[name="CodeEvent"]').val();

                    if (code.indexOf('CC') != -1) {
                        $('#myModal_fix_data .event-name').hide();
                    }
                    $.get(selectUrl + 'GetProducts', function (res) {
                        if (res.result == 200) {
                            $('#myModal_fix_data .product-select').html(`<option value="">Chọn sản phẩm</option>`);
                            $.each(res.data, function (index, item) {
                                $('#myModal_fix_data .product-select').append(`<option value="${item.productCode}">${item.name}</option>`)
                            });

                            ////select current item
                            let selectedProduct = $('#myModal_fix_data .hidden-product').val();
                            if (selectedProduct != null) {
                                $('#myModal_fix_data .product-select').val(selectedProduct);
                                $('#myModal_fix_data .product-select').trigger('change');
                            }

                            //setup show modal
                            $('#myModal_fix_data select').select2();
                            $("#myModal_fix_data .form_time").timepicker({ minuteStep: 5, showInputs: !1, disableFocus: !0, defaultTime: !1, showMeridian: !1 });
                            $("#myModal_fix_data .form_datetime").datetimepicker(
                                {
                                    startView: 2,
                                    minView: 2,
                                    format: "dd/mm/yyyy",
                                    autoclose: true
                                });

                            ShowModal('#myModal_fix_data');
                            $('.loading-wrapper').hide();
                        }
                        else {
                            toastr.error(res.errors.join('<br />'));
                            $('.loading-wrapper').hide();
                            return;
                        }
                    });

                    $('#myModal_fix_data #edit-show-up').click(function () {
                        $('.loading-wrapper').show();
                        var data = $('#update-form').serializeArray();
                        $.post(baseUrl + 'Update', data, function (res) {
                            if (res.result == 200) {
                                $('#search-show-up').trigger('click');
                                CloseModal('#myModal_fix_data');
                                toastr.success(res.message);
                            }
                            else {
                                toastr.error(res.errors.join('<br />'));
                                $('.loading-wrapper').hide();
                            }
                        });
                    });
                }
            });
        }
    });

    $('#View-Show-Up').on('click', function () {
        $('.loading-wrapper').show();
        let selectedRow = $('#table-body tr.selected');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một show up để xem chi tiết');
            $('.loading-wrapper').hide();
            return;
        }
        let id = selectedRow.data('id');
        if (id) {
            $.get(baseUrl + 'Update?id=' + id, function (res) {
                if (res.result == 400) {
                    toastr.error(res.errors.join('<br />'));
                    $('.loading-wrapper').hide();
                }
                else {
                    $('#myModal_detail .content-modal').html(res);
                    $.get(selectUrl + 'GetProducts', function (res) {
                        if (res.result == 200) {
                            $('#myModal_detail .product-select').html(`<option value="">Chọn sản phẩm</option>`);
                            $.each(res.data, function (index, item) {
                                $('#myModal_detail .product-select').append(`<option value="${item.productCode}">${item.name}</option>`)
                            });

                            ////select current item
                            let selectedProduct = $('#myModal_detail .hidden-product').val();
                            if (selectedProduct != null) {
                                $('#myModal_detail .product-select').val(selectedProduct);
                                $('#myModal_detail .product-select').trigger('change');
                            }

                            //setup show modal
                            $('#myModal_detail select').select2();
                            $('#myModal_detail .modal__title').text('Chi tiết');
                            $('#myModal_detail select').attr('disabled', 'disabled');
                            $('#myModal_detail input').attr('disabled', 'disabled');
                            $('#myModal_detail #edit-show-up').hide();
                            $('#myModal_detail .cancel').html("Đóng");;


                            ShowModal('#myModal_detail');
                            $('.loading-wrapper').hide();
                        }
                        else {
                            toastr.error(res.errors.join('<br />'));
                            $('.loading-wrapper').hide();
                            return;
                        }
                    });
                }
            });
        }
    });

    $('#myModal_add_new #create-show-up').on('click', function () {
        $('.loading-wrapper').show();
        var data = $('#myModal_add_new #create-form').serializeArray();
        $.post(baseUrl + 'Create', data, function (res) {
            if (res.result == 200) {
                $('#search-show-up').trigger('click');
                CloseModal('#myModal_add_new');
                ClearCreateModal();
                toastr.success(res.message);
            }
            else {
                toastr.error(res.errors.join('<br />'));
                $('.loading-wrapper').hide();
            }
        });
    });

    $('#myModal_add_new_for_cc #create-show-up').on('click', function () {
        $('.loading-wrapper').show();
        var data = $('#myModal_add_new_for_cc #create-form').serializeArray();
        $.post(baseUrl + 'CreateForCC', data, function (res) {
            if (res.result == 200) {
                $('#search-show-up').trigger('click');
                CloseModal('#myModal_add_new_for_cc');
                ClearCreateModal();
                toastr.success(res.message);
            }
            else {
                toastr.error(res.errors.join('<br />'));
                $('.loading-wrapper').hide();
            }
        });
    });

    $('#search-show-up').on('click', function () {
        $('.loading-wrapper').show();
        SetupPagination();
    });

    $('.module-content .select input').keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            $('#search-show-up').trigger('click');
        }
    });

    // update table data by size page
    $('#size-page select').on('change', function () {
        $('.loading-wrapper').show();
        SetupPagination();
    });

    $('.loading-wrapper').show();
    $('.select_lit.special select').select2();
    $('#myModal_add_new select').select2();
    $('#myModal_add_new_for_cc .event-name').hide();

    SetupPagination();
});

var LoadProductSelect = function (target) {
    $.get(selectUrl + 'GetProducts', function (res) {
        if (res.result == 200) {
            $(target + ' .product-select').html(`<option value="">Chọn sản phẩm</option>`);
            $.each(res.data, function (index, item) {
                $(target + ' .product-select').append(`<option value="${item.productCode}">${item.name}</option>`)
            });
        }
        else {
            toastr.error(res.errors.join('<br />'));
        }
    });
}

var ClearCreateModal = function () {
    $('#myModal_add_new input').val('');
    $('#myModal_add_new .product-select').val('');
    $('#myModal_add_new .product-select').trigger('change');
    $('#myModal_add_new .status-select').val('1');
    $('#myModal_add_new .status-select').trigger('change');

    $('#myModal_add_new_for_cc input').val('');
    $('#myModal_add_new_for_cc .product-select').val('');
    $('#myModal_add_new_for_cc .product-select').trigger('change');
    $('#myModal_add_new_for_cc .status-select').val('1');
    $('#myModal_add_new_for_cc .status-select').trigger('change');
}

var SetupPagination = function () {
    $('#pagination').pagination({
        ajax: function (options, refresh, $target) {
            var data = $('#search-form').serializeArray();
            data.push({ name: "Page", value: options.current });
            data.push({ name: "Size", value: $('#size-page select').val() });
            //console.log(data);
            $.ajax({
                url: baseUrl + "GetList",
                data: data,
                method: 'POST',
                dataType: 'json'
            }).done(function (res) {
                let div = $('#table-body');
                div.html(``);
                if (res.result == 200) {
                    $.each(res.data, function (index, item) {
                        div.append(`
                                    <tr class="hide_active" data-id="${item.id}" style="cursor: pointer;">
                                        <td></td>
                                        <td>
                                            <div class="radiotext">
                                                <label for='regular'>${item.codeEvent}</label>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="radiotext">
                                                <label for='regular'>${item.name}</label>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="radiotext">
                                                <label for='regular'>${item.productName ?? ''}</label>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="radiotext">
                                                <label for='regular'>${item.eventTimeString}</label>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="radiotext">
                                                <label for='regular'>${item.endTimeString}</label>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="radiotext">
                                                <label for='regular'>${item.status ? 'Mở khóa' : 'Khóa'}</label >
                                            </div >
                                        </td >
                                    </tr >
                                `);
                    });
                    refresh({
                        total: res.total, // optional
                        length: $('#size-page select').val()// optional
                    });
                    $('#size-page .from').val(($('#size-page select').val() - 1) * options.current + 1);
                    $('#size-page .to').val($('#size-page select').val() * options.current);
                    $('#size-page .total').val(res.total);
                    $('.loading-wrapper').hide();
                }
                else {
                    toastr.error(res.errors.join('<br />'));
                    $('#size-page .from').text(0);
                    $('#size-page .to').text(0);
                    $('#size-page .total').text(0);
                    $('.loading-wrapper').hide();
                    refresh({
                        total: 0, // optional
                        length: $('#size-page select').val()// optional
                    });
                }

            }).fail(function (error) {
                $('.loading-wrapper').hide();
            });
        }
    });
}