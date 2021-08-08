const baseUrl = '/Tele/CheckInOut/';
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

    $('#search-box select').select2();
    $('#myModal_add_new select').select2();

    //set default status
    $('#myModal_add_new .select_product').prop("disabled", true);

    $('#myModal_add_new #allow-go-with').attr('checked', true);
    $('#myModal_add_new #allow-go-with').trigger('change');

    // update table data by size page
    $('#size-page select').on('change', function () {
        $('.loading-wrapper').show();
        SetupPagination();
    });

    // first loading
    $('.loading-wrapper').show();
    SetupPagination();

    $('#search-checkin').on('click', function () {
        $('.loading-wrapper').show();
        SetupPagination();
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
            toastr.error('Chọn một checkin để cập nhật');
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
            toastr.error('Chọn một checkin để xem chi tiết');
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

});

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