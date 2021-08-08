let baseUrl = '/HR/TimeKeeping/';
var selectedMonth = '';
var curRow = null;

//update all DayInMonth's textbox
function ChangeDaysInMonth(e) {
    let days = $(e).val();
    $('.day-in-month').val(days);
}

//loaded done view
$(function () {
    //reset datepicker
    $('.only-month').datetimepicker('remove');
    $(".only-month").datetimepicker(
        {
            startView: 3,
            minView: 3,
            format: "mm/yyyy",
            autoclose: true
        });

    //initial for monthpicker
    var d = new Date();
    let month = d.getMonth() + 1;
    let year = d.getFullYear();
    $('#selected-month').val((month <= 9 ? '0' + month : month) + '/' + year);
    selectedMonth = $('#selected-month').val().split('/').reverse().join('/');

    $('#save-rule').click(function () {
        $('.loading-wrapper').show();
        let data = $('#rule_form').serializeArray();
        data[0].value = data[0].value.split('/').reverse().join('/');
        data[2].value = data[2].value.replaceAll(',', '');
        $.post(baseUrl + "SetupRuleInMonth", data, function (res) {
            if (res.result == 400) {
                toastr.error(res.errors.join('<br />'));
            }
            else {
                toastr.success(res.message);
                $('#rule_form input').val('');
                CloseModal('#attendance_setup-modal');
                if (data[0].value == $('#selected-month').val().split('/').reverse().join('/')) {
                    $('#total-working-setup').val(data[1].value);
                    $('#other-bonus-setup').val(commaSeparateNumber(data[2].value));
                }
            }
            $('.loading-wrapper').hide();
        });
    });

    $('#save-bonus').click(function () {
        $('.loading-wrapper').show();
        let bonus = $('#attendance_gift-modal input[name="OtherBonus"]').val().replaceAll(',', '');
        let selectTK = $('#attendance_gift-modal input[name="Id"]').val();
        $.post(baseUrl + "UpdateBonus", { otherBonus: bonus, id: selectTK }, function (res) {
            if (res.result == 400) {
                toastr.error(res.errors.join('<br />'));
            }
            else {
                toastr.success(res.message);
                CloseModal('#attendance_gift-modal');
            }
            $('.loading-wrapper').hide();
        });
    });

    //this is the event for search button
    $('#search-time').on('click', function () {
        $('.loading-wrapper').show();
        let month = $('#selected-month').val().split('/').reverse().join('/');
        let key = $('#key-search').val();
        $.get(baseUrl + 'GetList?month=' + month + '&key=' + key, function (res) {
            if (res.result == 200) {
                generateTable(res.data, '#body-table');
                selectedMonth = month;
                $('#total-working-setup').val(res.data.totalDaysInMonth);
                $('#other-bonus-setup').val(commaSeparateNumber(res.data.bonus));
            }
            else {
                $('#body-table').html('');
            }
        })
            .fail(function () { $('.loading-wrapper').hide(); })
            .done(function () {
                $('.loading-wrapper').hide();
                //DisableInput();
            });
    });

    $('.module-content .search input').keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            $('#search-time').trigger('click');
        }
    });

    //this is the event for cancel button
    $('#cancel-time').on('click', function () {
        //$('#body-table').find('input[type="number"]').val('0');
        location.reload();
    });

    //this is the event for update button to update timekeeping
    //$('#update-time').on('click', function () {
    //    $('.loading-wrapper').show();
    //    //check rule in month
    //    let workingInCurrentMonth = $('#total-working-setup').val();
    //    if (!workingInCurrentMonth) {
    //        toastr.warning("Thiết lập khai báo cho chung tháng trước khi khai báo chi tiết chấm công cho nhân viên!");
    //        $('.loading-wrapper').hide();
    //        return;
    //    }

    //    //setup data
    //    var datas = [];
    //    let values = selectedMonth.split('/').reverse();
    //    let month = values.join('/');
    //    $('#time-table #body-table').find('tr').each(function (i, el) {
    //        var $tds = $(this).find('td'),
    //            id = $tds.eq(0).find('span').attr('id') == 'undefined' ? 0 : $tds.eq(0).find('span').attr('id'),
    //            //id = 0,
    //            codeStaff = $tds.eq(0).text().trim(),
    //            fullName = $tds.eq(1).text().trim(),
    //            roleAccount = $tds.eq(2).data('role'),
    //            totalWorkingDays = $tds.eq(3).find('input').val(),
    //            totalLates = $tds.eq(4).find('input').val(),
    //            totalEarlyOuts = $tds.eq(5).find('input').val(),
    //            totalWithoutReason = $tds.eq(6).find('input').val(),
    //            forgetCheckOutIn = $tds.eq(7).find('input').val(),
    //            takeLeaveInMonth = $tds.eq(8).find('input').val(),
    //            showupInMonth = $tds.eq(9).find('input').val(),
    //            contractsInMonth = $tds.eq(10).find('input').val(),
    //            revenueInMonth = $tds.eq(11).find('input').val()?.replaceAll(',', '')
    //            ;
    //        if (totalWorkingDays && totalWorkingDays != '0') {
    //            datas.push({
    //                Id: id, Month: month, CodeStaff: codeStaff, FullName: fullName, RoleAccount: roleAccount,
    //                TotalWorkingDays: totalWorkingDays, TotalLates: totalLates, TotalEarlyOuts: totalEarlyOuts,
    //                TotalWithoutReason: totalWithoutReason, ForgetCheckOutIn: forgetCheckOutIn, /*IncomeTax: incomeTax,*/
    //                TotalTakeLeaveInMonth: takeLeaveInMonth, TotalShowupInMonth: showupInMonth, TotalContract: contractsInMonth,
    //                RevenueInMonth: revenueInMonth
    //            });
    //        }
    //    });

    //    $.post(baseUrl + "UploadTimeKeeping", { data: datas }, function (res) {
    //        if (res.result == 400) {
    //            toastr.error(res.errors.join('<br />'));
    //        }
    //        else {
    //            toastr.success(res.message);
    //            $('#search-time').trigger('click');
    //        }
    //        $('.loading-wrapper').hide();
    //    });
    //});

    //DisableInput();

    $('#import-data').on('click', function () {
        $('.loading-wrapper').show();
        let file = $('#import-form input[name="file"]').val();
        if (!file) {
            toastr.warning("Chưa chọn file chứa dữ liệu chấm công");
            $('.loading-wrapper').hide();
            return;
        }

        toastr.info("Vui lòng chờ hệ thống đang nhập dữ liệu chấm công");

        let form = $('form#import-form');
        var formData = new FormData(form[0]);
        formData.set('month', formData.get('month').split('/').reverse().join('/'));
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
                    $('#search-time').trigger('click');
                    CloseModal('#attendance_import-modal');
                }
                else {
                    toastr.error(res.errors.join('<br />'));
                }
                $('#attendance_import-modal input').val('');
                $('.loading-wrapper').hide();
            },
            fail: function (res) {
                $('.loading-wrapper').hide();
            }
        });
    });

    $(document).click(function (event) {
        var $target = $(event.target);
        let tr = $target.closest('tr');
        if (tr) {
            if (curRow && (tr.length == 0 || $(tr).find('td').eq(0).text().trim() != $(curRow).find('td').eq(0).text().trim())) {
                var $tds = $(curRow).find('td'),
                    id = $tds.eq(0).find('span').attr('id') == 'undefined' ? 0 : $tds.eq(0).find('span').attr('id'),
                    codeKeeping = $tds.eq(0).find('span').attr('data-code'),
                    //id = 0,
                    codeStaff = $tds.eq(0).text().trim(),
                    fullName = $tds.eq(1).text().trim(),
                    roleAccount = $tds.eq(2).data('role'),
                    totalWorkingDays = $tds.eq(3).find('input').val(),
                    totalLates = $tds.eq(4).find('input').val(),
                    totalEarlyOuts = $tds.eq(5).find('input').val(),
                    totalWithoutReason = $tds.eq(6).find('input').val(),
                    forgetCheckOutIn = $tds.eq(7).find('input').val(),
                    takeLeaveInMonth = $tds.eq(8).find('input').val(),
                    showupInMonth = $tds.eq(9).find('input').val(),
                    contractsInMonth = $tds.eq(10).find('input').val(),
                    revenueInMonth = $tds.eq(11).find('input').val()?.replaceAll(',', '')
                    ;
                if (totalWorkingDays && totalWorkingDays != '0') {
                    $('.loading-wrapper').show();
                    $('.loading-wrapper').show();
                    //check rule in month
                    let workingInCurrentMonth = $('#total-working-setup').val();
                    if (!workingInCurrentMonth) {
                        toastr.warning("Thiết lập khai báo cho chung tháng trước khi khai báo chi tiết chấm công cho nhân viên!");
                        $('.loading-wrapper').hide();
                        curRow = null;
                        return;
                    }

                    var data = {
                        Id: id, Month: selectedMonth, CodeStaff: codeStaff, FullName: fullName, RoleAccount: roleAccount,
                        TotalWorkingDays: totalWorkingDays, TotalLates: totalLates, TotalEarlyOuts: totalEarlyOuts,
                        TotalWithoutReason: totalWithoutReason, ForgetCheckOutIn: forgetCheckOutIn, /*IncomeTax: incomeTax,*/
                        TotalTakeLeaveInMonth: takeLeaveInMonth, TotalShowupInMonth: showupInMonth, TotalContract: contractsInMonth,
                        RevenueInMonth: revenueInMonth, CodeKeeping: codeKeeping
                    };
                    if (codeKeeping && codeKeeping != '') {
                        $.post(baseUrl + "UpdateExistedTimeKeeping", { data: data }, function (res) {
                            if (res.result == 400) {
                                toastr.error(res.errors.join('<br />'));
                            }
                            else {
                                toastr.success(res.message);
                            }
                            $('.loading-wrapper').hide();
                        });
                    }
                    else {
                        $.post(baseUrl + "UpdateNewTimeKeeping", { data: data }, function (res) {
                            if (res.result == 400) {
                                toastr.error(res.errors.join('<br />'));
                            }
                            else {
                                toastr.success(res.message);
                                $tds.eq(0).find('span').attr('data-code', res.data.codeKeeping);
                            }
                            $('.loading-wrapper').hide();
                        });
                    }
                }
            }

            if (tr.length == 0) {
                curRow = null;
            }
            else {
                curRow = tr;
            }
        }
        else {
            curRow = null;
        }
    });
});

var OpenEditingBonus = function (id) {
    if (id != '0') {
        $('#attendance_gift-modal #hiddenId').val(id);
        ShowModal('#attendance_gift-modal');
    }
    else {
        toastr.error('Dữ liệu chưa tồn tại trên hệ thống');
    }
}

var OpenEditingModal = function (id) {
    if (id != '0') {
        $.get(baseUrl + "Update?id=" + id, function (res) {
            if (res.result != 400) {
                $('#attendance_edit-modal .content-modal').html(res);
                ShowModal('#attendance_edit-modal');
                //$("#attendance_edit-modal .only-month").datetimepicker(
                //    {
                //        startView: 3,
                //        minView: 3,
                //        format: "mm/yyyy",
                //        autoclose: true
                //    });
            }
            else {
                toastr.error(res.errors.join('<br />'));
            }
        });
    }
    else {
        toastr.error('Dữ liệu chưa tồn tại trên hệ thống');
    }
}

//edit timekeeping info for a person
//var UpdateTimekeeping = function () {
//    $('.loading-wrapper').show();
//    var model = $('#attendance_edit-modal').find('form').serializeArray();
//    //console.log(model);
//    if (model[12]) {
//        model[12].value = model[12].value.replaceAll(',', '');
//    }

//    $.post(baseUrl + "UpdateTimeKeeping", model, function (res) {
//        if (res.result == 400) {
//            toastr.error(res.errors.join('<br />'));
//            $('.loading-wrapper').hide();
//        }
//        else {
//            //update this row
//            let editedRow = $(`#body-table span[id="${model[0].value}"]`);
//            //console.log(editedRow);
//            editedRow.closest('tr').find('.total-working-days').val(model[5].value);
//            editedRow.closest('tr').find('.total-lates').val(model[6].value);
//            editedRow.closest('tr').find('.total-early-outs').val(model[7].value);
//            editedRow.closest('tr').find('.total-without-reason').val(model[8].value);
//            editedRow.closest('tr').find('.forget-check-outin').val(model[9].value);
//            editedRow.closest('tr').find('.take-leave-in-month').val(model[10].value == '' ? 0 : model[10].value);
//            editedRow.closest('tr').find('.showup-in-month').val((model[4].value == '8' || model[4].value == '9') ? (model[11].value == '' ? 0 : model[11].value) : '');
//            editedRow.closest('tr').find('.contracts-in-month').val((model[4].value == '8' || model[4].value == '9') ? (model[12].value == '' ? 0 : model[12].value) : '');
//            editedRow.closest('tr').find('.revenue-in-month').val((model[4].value != '8' && model[4].value != '9') ? (commaSeparateNumber(model[11].value == '' ? 0 : model[11].value)) : '');

//            toastr.success(res.message);
//            CloseModal('#attendance_edit-modal');
//            $('.loading-wrapper').hide();
//        }
//    });
//};

//disable input in table
var DisableInput = function () {
    $('#body-table .total-working-days').each(function (i, item) {
        if ($(item).val() > 0) {
            $(this).closest('tr').find('input').attr('disabled', 'disabled');
        }
    });
}

var GetRoleName = function (role) {
    var roleList = [
        //{ key: 1, value: "Admin" },
        { key: '2', value: "Kế toán" },
        { key: '3', value: "HR" },
        { key: '4', value: "Capital Consultant" },
        { key: '5', value: "Capital Leader" },
        { key: '6', value: "Capital Manager" },
        { key: '7', value: "Admin" },
        { key: '8', value: "TeleSale" },
        { key: '9', value: "Leader TeleSale" },
        { key: '10', value: "Capital Director" },
        { key: '11', value: "Collabrator" },
    ];
    return roleList.find(x => x.key == (role)).value;
}

//generate table with retreived data
var generateTable = function (data, target) {
    //<td>
    //    <input type="text" min="0" class="income-tax isNumber" value="${commaSeparateNumber(item.incomeTax??0)}" style="text-align: center" />
    //</td>
    $(target).html('');
    $(target).append(`<tr>
                        <th> ID</th>
                        <th>Họ và tên</th>
                        <th>Chức vụ</th>
                        <th>Công thực tế</th>
                        <th>Đi muộn</th>
                        <th>Về sớm</th>
                        <th>Nghỉ không lý do</th>
                        <th>Quên checkout</th>
                        <th>Nghỉ có phép</th>
                        <th>Tổng số show up(telesale)</th>
                        <th>Tổng số hợp đồng(telesale)</th>
                        <th>Tổng số doanh thu(sale)</th>
                        <th></th>
                    </tr>`);
    $.each(data.timeKeepings, function (index, item) {
        $(target).append(
            `
            <tr>
                <td class="no-border">
                    <span id="${item.id}" data-code="${item.codeKeeping ?? ''}">${item.codeStaff}</span>
                </td>
                <td class="no-border">
                    <span>${item.fullName}</span>
                </td>
                <td class="no-border" data-role="${item.roleAccount}">
                    <span>${GetRoleName(item.roleAccount)}</span>
                </td>

                <td>
                    <input type="text" min="0" class="total-working-days isOneDecimalNumber" value="${item.totalWorkingDays ?? 0}" style="text-align: center" />
                </td>

                <td>
                    <input type="number" min="0" class="total-lates" value="${item.totalLates ?? 0}" style="text-align: center" />
                </td>

                <td>
                    <input type="number" min="0" class="total-early-outs" value="${item.totalEarlyOuts ?? 0}" style="text-align: center" />
                </td>

                <td>
                    <input type="number" min="0" class="total-without-reason" value="${item.totalWithoutReason ?? 0}" style="text-align: center" />
                </td>

                <td>
                    <input type="number" min="0" class="forget-check-outin" value="${item.forgetCheckOutIn ?? 0}" style="text-align: center" />
                </td>

                <td>
                    <input type="text" class="take-leave-in-month isOneDecimalNumber" value="${item.totalTakeLeaveInMonth ?? 0}" style="text-align: center" />
                </td>

                <td>
                    <input type="text" min="0" class="showup-in-month isOneDecimalNumber"
                    value="${(item.roleAccount == '8' || item.roleAccount == '9') ? (item.totalShowupInMonth ?? 0) : ''}" 
                    disabled
                    style="text-align: center" />
                </td>

                <td>
                    <input type="text" min="0" class="contracts-in-month isOneDecimalNumber"
                    value="${(item.roleAccount == '8' || item.roleAccount == '9') ? (item.totalContract ?? 0) : ''}" 
                    disabled
                    style="text-align: center" />
                </td>

                <td>
                    <input type="text" min="0" class="revenue-in-month isNumber"
                    value="${(item.roleAccount != '8' && item.roleAccount != '9') ? (commaSeparateNumber(item.revenueInMonth ?? 0)) : ''}" 
                    disabled
                    style="text-align: center; min-width: 150px;" />
                </td>

                <td>
                    <div class="more-options attendance_more-options">
                        <img src="/Assets/crm/images/employee-attendance/more-options.svg"
                             alt="" />
                        <div class="options">
                            <button class="option-item"
                                    type="button"
                                    onclick="OpenEditingBonus('${item.id}')">
                                <img src="/Assets/crm/images/employee-attendance/gift.svg"
                                     alt="" />
                                <span>Thưởng khác</span>
                            </button>
                        </div>
                    </div>
                </td>
            </tr>
            `
        );
    });
}

//<button class="option-item"
//        type="button"
//        onclick="OpenEditingModal('${item.id}')">
//    <img src="/Assets/crm/images/employee-attendance/edit.svg"
//         alt="" />
//    <span>Chỉnh sửa</span>
//</button>