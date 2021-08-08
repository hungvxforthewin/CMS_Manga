const baseUrl = '/SaleAdmin/CheckInOut/';
let selectUrl = '/SelectionData/';
$(function () {
    //setup data for dropdownlist
    LoadProducts('#search-box .select_product');
    LoadProducts('#myModal_add_new .select_product');

    LoadResources('#search-box .select_resource');
    LoadResources('#myModal_add_new .select_resource');

    LoadSales('#search-box .select_sale');
    LoadSales('#myModal_add_new .select_sale');

    LoadTeleSales('#search-box .select_tele');
    LoadTeleSales('#myModal_add_new .select_tele');

    LoadSales('#search-box .select_sale_to');
    LoadSales('#myModal_add_new .select_sale_to');

    LoadActiveShowUps('#myModal_add_new .select_show_up', '');
    LoadShowUps('#checkin_import-modal .event_select', '');
    LoadShowUps('#search-form .event-select', $('#hidden-near-event').val());

    $('#search-box select').select2();
    $('#myModal_add_new select').select2();
    $('#checkin_import-modal select').select2();

    //set default status
    $('#myModal_add_new .select_product').prop("disabled", true);

    $('#myModal_add_new #allow-go-with').attr('checked', true);
    $('#myModal_add_new #allow-go-with').trigger('change');

    // update table data by size page
    $('#size-page select').on('change', function () {
        $('.loading-wrapper').show();
        var data = $('#search-form').serializeArray();
        SetupPagination(data);
    });

    // first loading
    $('.loading-wrapper').show();
    var data = $('#search-form').serializeArray();
    if (!data[0].value) {
        data[0].value = $('#hidden-near-event').val();
    }
    SetupPagination(data);
    RetrieveStaticalData();

    $('#search-checkin').on('click', function () {
        $('.loading-wrapper').show();
        var data = $('#search-form').serializeArray();
        SetupPagination(data);
    });

    $('.module-content .select input').keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            $('#search-checkin').trigger('click');
        }
    });

    $('#Edit-CheckInOut').on('click', function () {
        $('.loading-wrapper').show();
        let selectedRow = $('#table-body tr.selected');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một thông tin checkin để cập nhật');
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
                    $('#myModal_fix .content-modal').html(res);
                    //setup data for dropdownlist
                    LoadActiveShowUps('#myModal_fix .select_show_up', $('#myModal_fix .hidden-event').val());
                    LoadProducts('#myModal_fix .select_product', $('#myModal_fix .hidden-product').val());
                    LoadResources('#myModal_fix .select_resource', $('#myModal_fix .hidden-resource').val());
                    LoadTeleSales('#myModal_fix .select_tele', $('#myModal_fix .hidden-tele').val());
                    LoadSales('#myModal_fix .select_sale', $('#myModal_fix .hidden-sale').val());
                    LoadSales('#myModal_fix .select_sale_to', $('#myModal_fix .hidden-sale-to').val());

                    //set default status
                    $('#myModal_fix .select_product').prop("disabled", true);
                    $('#myModal_fix #allow-go-with').attr('checked', true);
                    $('#myModal_fix #allow-go-with').trigger('change');
                    $('#myModal_fix select').select2();

                    //setup show modal
                    ShowModal('#myModal_fix');
                    $('.loading-wrapper').hide();

                    $('#myModal_fix #edit-checkin').click(function () {
                        $('.loading-wrapper').show();
                        var Model = $('#myModal_fix #checkin-form').serializeArray();

                        var GoWithPersons = $('#myModal_fix #attached-form').serializeArray();
                        var index = 0;
                        for (let i = 0; i < GoWithPersons.length; i = i + 2) {
                            Model.push({ 'name': 'Group[' + index + '].Name', 'value': GoWithPersons[i].value });
                            Model.push({ 'name': 'Group[' + index + '].PhoneNumber', 'value': GoWithPersons[i + 1].value });
                            index++;
                        }
                        let note = $('#myModal_fix textarea').val();
                        Model.push({ 'name': 'Note', 'value': note });

                        $.post(baseUrl + 'Update', Model, function (res) {
                            if (res.result == 200) {
                                $('#search-checkin').trigger('click');
                                CloseModal('#myModal_fix');
                                RetrieveStaticalData();
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

    $('#View-CheckInOut').on('click', function () {
        $('.loading-wrapper').show();
        let selectedRow = $('#table-body tr.selected');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một thông tin checkin để xem chi tiết');
            $('.loading-wrapper').hide();
            return;
        }
        let id = selectedRow.data('id');
        if (id) {
            $.get(baseUrl + 'View?id=' + id, function (res) {
                if (res.result == 400) {
                    toastr.error(res.errors.join('<br />'));
                    $('.loading-wrapper').hide();
                    return;
                }
                else {
                    $('#myModal_detail .content-modal').html(res);
                    //setup data for dropdownlist
                    LoadShowUps('#myModal_detail .select_show_up', $('#myModal_detail .hidden-event').val());
                    LoadProducts('#myModal_detail .select_product', $('#myModal_detail .hidden-product').val());
                    LoadResources('#myModal_detail .select_resource', $('#myModal_detail .hidden-resource').val());
                    LoadTeleSales('#myModal_detail .select_tele', $('#myModal_detail .hidden-tele').val());
                    LoadSales('#myModal_detail .select_sale', $('#myModal_detail .hidden-sale').val());
                    LoadSales('#myModal_detail .select_sale_to', $('#myModal_detail .hidden-sale-to').val());

                    //set default status
                    $('#myModal_detail select').prop("disabled", true);
                    $('#myModal_detail input').prop("disabled", true);
                    $('#myModal_detail textarea').prop("disabled", true);
                    $('#myModal_detail #allow-go-with').attr('checked', true);
                    $('#myModal_detail #allow-go-with').trigger('change');
                    $('#myModal_detail select').select2();

                    //setup show modal
                    ShowModal('#myModal_detail');
                    $('.loading-wrapper').hide();
                }
            }).done(function () {
                $('#myModal_detail select').prop("disabled", true);
                $('#myModal_detail input').prop("disabled", true);
                $('#myModal_detail textarea').prop("disabled", true);
            });
        }
    });

    $('#myModal_add_new #create-new-checkin').on('click', function () {
        $('.loading-wrapper').show();
        var Model = $('#myModal_add_new #checkin-form').serializeArray();

        var GoWithPersons = $('#myModal_add_new #attached-form').serializeArray();
        var index = 0;
        for (let i = 0; i < GoWithPersons.length; i = i + 2) {
            Model.push({ 'name': 'Group[' + index + '].Name', 'value': GoWithPersons[i].value });
            Model.push({ 'name': 'Group[' + index + '].PhoneNumber', 'value': GoWithPersons[i + 1].value });
            index++;
        }

        let note = $('#myModal_add_new textarea').val();
        Model.push({ 'name': 'Note', 'value': note });

        $.post(baseUrl + 'Create', Model, function (res) {
            if (res.result == 200) {
                $('#search-checkin').trigger('click');
                CloseModal('#myModal_add_new');
                toastr.success(res.message);
                RetrieveStaticalData();

                //clear form
                $('#myModal_add_new textarea').val('');
                $('#myModal_add_new input').val('');
                $('#myModal_add_new select').val('');
                $('#myModal_add_new select').trigger('change');
                $('#myModal_add_new .wrap_table1').find('table tbody').html('');
            }
            else {
                toastr.error(res.errors.join('<br />'));
                $('.loading-wrapper').hide();
            }
        });
    });

    $('#checkin-showup').on('click', function () {
        $('.loading-wrapper').show();
        let selectedRow = $('#table-body tr.selected');
        if (selectedRow.length == 1) {
            let id = selectedRow.data('id');
            $('#myModal_checkin_accept .ShowId').val(id);
        }
            ShowModal('#myModal_checkin_accept');
            $('.loading-wrapper').hide();

            $('#btn_checkin-investor').unbind('click').on('click', function () {
                $('.loading-wrapper').show();

                let phone = $('#myModal_checkin_accept input[name="PhoneNumber"]').val();
                if (phone == '') {
                    let id = $('#myModal_checkin_accept .ShowId').val();
                    if (id) {
                        $.get(baseUrl + 'CheckIn?id=' + id, function (res) {
                            CloseModal('#myModal_checkin_accept');
                            if (res.result == 200) {
                                $('#search-checkin').trigger('click');
                                toastr.success(res.message);
                                $('#btn_checkin-investor').off('click');
                                RetrieveStaticalData();
                            }
                            else {
                                toastr.error(res.errors.join('<br />'));
                                $('.loading-wrapper').hide();
                            }
                        });
                    }
                    else {
                        toastr.error('Nhập số điện thoại để checkin');
                        $('.loading-wrapper').hide();
                        return;
                    }
                }
                else {
                    $.get(baseUrl + 'CheckinByPhoneNumber?phoneNumber=' + phone, function (res) {
                        CloseModal('#myModal_checkin_accept');
                        if (res.result == 200) {
                            $('#search-checkin').trigger('click');
                            toastr.success(res.message);
                            $('#btn_checkin-investor').off('click');
                            RetrieveStaticalData();
                        }
                        else {
                            toastr.error(res.errors.join('<br />'));
                            $('.loading-wrapper').hide();
                        }
                    });
                }
            });
    });

    $('#checkout-showup').on('click', function () {
        $('.loading-wrapper').show();
        let selectedRow = $('#table-body tr.selected');
        if (selectedRow.length == 1) {

            let id = selectedRow.data('id');
            $('#myModal_checkout .ShowId').val(id);
        }
            ShowModal('#myModal_checkout');
            $('.loading-wrapper').hide();

            $('#btn_checkout-investor').unbind('click').on('click', function () {
                $('.loading-wrapper').show();

                let phone = $('#myModal_checkout input[name="PhoneNumber"]').val();
                if (phone == '') {
                    let id = $('#myModal_checkout .ShowId').val();
                    if (id) {
                        $.get(baseUrl + 'CheckOut?id=' + id, function (res) {
                            CloseModal('#myModal_checkout');
                            if (res.result == 200) {
                                $('#search-checkin').trigger('click');
                                toastr.success(res.message);
                                $('#btn_checkout-investor').off('click');
                            }
                            else {
                                toastr.error(res.errors.join('<br />'));
                                $('.loading-wrapper').hide();
                            }
                        });
                    }
                    else {
                        toastr.error('Nhập số điện thoại để checkout');
                        $('.loading-wrapper').hide();
                        return;
                    }
                }
                else {
                    $.get(baseUrl + 'CheckOutByPhoneNumber?phoneNumber=' + phone, function (res) {
                        CloseModal('#myModal_checkout');
                        if (res.result == 200) {
                            $('#search-checkin').trigger('click');
                            toastr.success(res.message);
                            $('#btn_checkout-investor').off('click');
                        }
                        else {
                            toastr.error(res.errors.join('<br />'));
                            $('.loading-wrapper').hide();
                        }
                    });
                }
            });
        //}
    });

    $('#import-data').on('click', function () {
        $('.loading-wrapper').show();
        let file = $('#import-form input[name="file"]').val();
        if (!file) {
            toastr.warning("Chưa chọn file chứa danh sách checkin");
            $('.loading-wrapper').hide();
            return;
        }

        toastr.info("Vui lòng chờ hệ thống đang nhập danh sách checkin");
        let form = $('form#import-form');
        var formData = new FormData(form[0]);

        $.ajax({
            type: 'POST',
            url: baseUrl + 'Import',
            enctype: 'multipart/form-data',
            data: formData,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.result == 200) {
                    toastr.success(res.message);
                    $('#search-checkin').trigger('click');
                    CloseModal('#checkin_import-modal');
                }
                else {
                    toastr.error(res.errors.join('<br />'));
                }
                $('#checkin_import-modal input').val('');
                $('.loading-wrapper').hide();
            },
            fail: function (res) {
                $('.loading-wrapper').hide();
            }
        });
    });

    $('#Delete-CheckInOut').on('click', function () {
        $('.loading-wrapper').show();
        let selectedRow = $('#table-body tr.selected');
        if (selectedRow.length == 1) {
            let id = selectedRow.data('id');
            $('#myModal_remove_checkin .ShowId').val(id);
        }
        ShowModal('#myModal_remove_checkin');
        $('.loading-wrapper').hide();

        $('#btn_remove-checkin-info').unbind('click').on('click', function () {
            $('.loading-wrapper').show();

            let phone = $('#myModal_remove_checkin input[name="PhoneNumber"]').val();
            if (phone == '') {
                let id = $('#myModal_remove_checkin .ShowId').val();
                if (id) {
                    $.get(baseUrl + 'RemoveCheckin?id=' + id, function (res) {
                        CloseModal('#myModal_remove_checkin');
                        if (res.result == 200) {
                            $('#search-checkin').trigger('click');
                            toastr.success(res.message);
                            $('#btn_remove-checkin-info').off('click');
                            RetrieveStaticalData();
                        }
                        else {
                            toastr.error(res.errors.join('<br />'));
                            $('.loading-wrapper').hide();
                        }
                    });
                }
                else {
                    toastr.error('Nhập số điện thoại để xóa thông tin checkin');
                    $('.loading-wrapper').hide();
                    return;
                }
            }
            else {
                $.get(baseUrl + 'RemoveCheckinByPhoneNumber?phoneNumber=' + phone, function (res) {
                    CloseModal('#myModal_checkin_accept');
                    if (res.result == 200) {
                        $('#search-checkin').trigger('click');
                        toastr.success(res.message);
                        $('#btn_remove-checkin-info').off('click');
                        RetrieveStaticalData();
                    }
                    else {
                        toastr.error(res.errors.join('<br />'));
                        $('.loading-wrapper').hide();
                    }
                });
            }
        });
    });
});

var ClearInput = function (modal) {
    $(modal).find('input').val('');
    $(modal).find('textarea').val('');
    $(modal).find('select').val('').trigger('change');
    $(modal).find('select').select2();
}

var RetrieveStaticalData = function () {
    $.get(baseUrl + 'GetStaticalCheckinData', function (res) {
        if (res.result == 200) {
            $('.count_people #JoinedPersonNumber').text(res.data.checkedInPersonNumber);
            $('.count_people #ExpectedCheckinPersonNumber').text(res.data.expectedCheckinPersonNumber);
            $('.count_people #CheckinPercent').text(res.data.checkedinPercent);
        }
    });
}

var AllowAddGoWith = function (e) {
    var isAllow = $(e).is(':checked');
    $(e).closest('.body-modal').find('#add-go-with-person').prop('disabled', !isAllow);
    $(e).closest('.body-modal').find('#add_row_table1 input').prop('disabled', !isAllow);
    $(e).closest('.body-modal').find('#add_row_table1 img').prop('disabled', !isAllow);
}

var ChangeShowUp = function (e) {
    let product = $(e).find(':selected').data('product');
    if (product) {
        $(e).closest('.body-modal').find('.select_product').val(product);
        $(e).closest('.body-modal').find('.select_product').trigger('change');
    }
    else {
        $(e).closest('.body-modal').find('.select_product').val('');
        $(e).closest('.body-modal').find('.select_product').trigger('change');
    }
}

var AddGoWithPerson = function (e) {
    let rows = $(e).closest('.body-modal').find('.wrap_table1 table tbody');
    rows.append(`
                        <tr>
                            <td></td>
                            <td class="stt">${rows.find('tr').length + 1}</td>
                            <td>
                                <div class="radiotext">
                                    <input type="text" placeholder="Nhập tên khách" name="FullName" />
                                </div>
                            </td>
                            <td>
                                <div class="radiotext">
                                    <input type="text" placeholder="Nhập số điện thoại" name="PhoneNumber" />
                                </div>
                            </td>
                            <td>
                                <div class="radiotext">
                                    <label for='regular'>
                                        <div class="div_noti deletes_div">
                                            <img class="deletes" onclick="DeleteGoWithPerson(this)" src="/Assets/SA/images/list_check_io/Group 2245.svg" alt="" />
                                            <div class="notifi deletes_noti" style="color: #FF5959;">
                                                Xóa
                                            </div>
                                        </div>
                                    </label>
                                </div>
                            </td>
                        </tr>
                    `);
}

var LoadProducts = function (target, selected) {
    $(target).html('');
    $(target).append(`<option value="">--</option>`);
    $.get(selectUrl + 'GetProducts', function (res) {
        if (res.result == 200) {
            $.each(res.data, function (index, item) {
                $(target).append(`<option value="${item.productCode}">${item.name}</option>`)
            });

            if (selected) {
                $(target).val(selected);
                $(target).trigger('change');
            }
        }
        else {
            //toastr.error(res.errors.join('<br />'));
        }
    });
}

var LoadResources = function (target, selected) {
    $(target).html('');
    $(target).append(`<option value="">--</option>`);
    $.get(selectUrl + 'GetInvestorResource', function (res) {
        if (res.result == 200) {
            $.each(res.data, function (index, item) {
                $(target).append(`<option value="${item.investorResourceCode}">${item.addressFind}</option>`)
            });

            if (selected) {
                $(target).val(selected);
                $(target).trigger('change');
            }
        }
        else {
            //toastr.error(res.errors.join('<br />'));
        }
    });
}

var LoadSales = function (target, selected) {
    $(target).html('');
    $(target).append(`<option value="">--</option>`);
    $.get(selectUrl + 'GetSale', function (res) {
        if (res.result == 200) {
            $.each(res.data, function (index, item) {
                $(target).append(`<option value="${item.codeStaff}">${item.fullName}</option>`)
            });

            if (selected) {
                $(target).val(selected);
                $(target).trigger('change');
            }
        }
        else {
            //toastr.error(res.errors.join('<br />'));
        }
    });
}

var LoadTeleSales = function (target, selected) {
    $(target).html('');
    $(target).append(`<option value="">--</option>`);
    $.get(selectUrl + 'GetTeleSale', function (res) {
        if (res.result == 200) {
            $.each(res.data, function (index, item) {
                $(target).append(`<option value="${item.codeStaff}">${item.fullName}</option>`)
            });

            if (selected) {
                $(target).val(selected);
                $(target).trigger('change');
            }
        }
        else {
            //toastr.error(res.errors.join('<br />'));
        }
    });
}

var LoadShowUps = function (target, selected) {
    $(target).html('');
    $(target).append(`<option value="">--</option>`);
    $.get(selectUrl + 'GetShowUpList', function (res) {
        if (res.result == 200) {
            $.each(res.data, function (index, item) {
                $(target).append(`<option value="${item.codeEvent}" data-product="${item.productCode}" >${item.name + '_' + item.eventTimeString}</option>`)
            });

            if (selected) {
                $(target).val(selected);
                $(target).trigger('change');
            }
        }
        else {
            //toastr.error(res.errors.join('<br />'));
        }
    });
}

var LoadActiveShowUps = function (target, selected) {
    $(target).html('');
    $(target).append(`<option value="">--</option>`);
    $.get(selectUrl + 'GetActiveShowUpList?currentShowUpCode=' + selected, function (res) {
        if (res.result == 200) {
            $.each(res.data, function (index, item) {
                $(target).append(`<option value="${item.codeEvent}" data-product="${item.productCode}" >${item.name + '_' + item.eventTimeString}</option>`)
            });

            if (selected) {
                $(target).val(selected);
                $(target).trigger('change');
            }
        }
        else {
            //toastr.error(res.errors.join('<br />'));
        }
    });
}

var DeleteGoWithPerson = function (e) {
    let table = $(e).closest('table');
    $(e).closest('tr').remove();
    table.find('.stt').each(function (index, item) {
        $(this).text(index + 1);
    });
}

// setup pagination
var SetupPagination = function (data) {
    $('#pagination').pagination({
        ajax: function (options, refresh, $target) {
            if (!data[9]) {
                data.push({ name: "Page", value: options.current });
            }
            else {
                data[9].value = options.current;
            }
            if (!data[10]) {
                data.push({ name: "Size", value: $('#size-page select').val() });
            }
            else {
                data[10].value = $('#size-page select').val();
            }
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
                            <tr class="header" data-id="${item.id}" >
                                <td></td>
                                <td class="no_follow">
                                    <div class="radiotext">
                                    <label>${item.codeInvestor}</label>
                                    </div>
                                </td>
                                <td class="cusor">
                                    <div class="radiotext main">
                                    <label>${item.investorName}</label>
                                    </div>
                                </td>
                                <td>
                                    <div class="radiotext">
                                    <label>${item.phoneNumber}</label>
                                    </div>
                                </td>
                                <td class="no_follow">
                                    <div class="radiotext">
                                    <label>${item.sale ?? ''}</label>
                                    </div>
                                </td>
                                <td class="no_follow">
                                    <div class="radiotext">
                                    <label>${item.saleTO ?? ''}</label>
                                    </div>
                                </td>
                                <td class="no_follow">
                                    <div class="radiotext">
                                    <label>${item.teleSale ?? ''}</label>
                                    </div>
                                </td>
                                <td class="no_follow">
                                    <div class="radiotext">
                                    <label>${item.timeInString ?? ''}</label>
                                    </div>
                                </td>
                                <td class="no_follow">
                                    <div class="radiotext">
                                    <label>${item.timeOutString ?? ''}</label>
                                    </div>
                                </td>
                                                
                                <td class="no_follow">
                                    <div class="radiotext">
                                    <label>${item.eventName}</label>
                                    </div>
                                </td>
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