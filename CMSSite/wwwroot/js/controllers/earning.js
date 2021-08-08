//let actionUrl = '/HR/Earning/';
//let actionUrl = '/Accountant/Earning/';
let selectUrl = '/SelectionData/';
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

    LoadBranchesToForm('.search-box');

    $("body").on('click', 'tr', function () {
        $(this).parent().children().removeClass("selected"), $(this).addClass("selected");
    });

    $('.search-box .select-branch').on('change', function () {
        $('.search-box .select-team').html(`<option value="">Nhóm</option>`);

        let selectedBranch = $(this).val();
        if (selectedBranch) {
            LoadDeptsToForm('.search-box', selectedBranch);
        }
    });

    $('.search-box .select-dept').on('change', function () {
        let selectedDept = $(this).val();
        if (selectedDept) {
            LoadTeamsToForm('.search-box', selectedDept);
        }
    });

    $('#search-earning').on('click', function () {
        $('.loading-wrapper').show();
        //let month = $('.search-box').find('input[name="Month"]').val();
        //$.get(actionUrl + "GetList?month=" + month, function (res) {
        //    if (res.result == 400) {
        //        $('#body-table').html('');
        //        toastr.error(res.errors.join('<br />'));
        //    }
        //    else {
        //        GenerateTable('#body-table', res.data);
        //    }
        //    $('.loading-wrapper').hide();
        //});

        //SetupPagination();
        SetupPaginationSingleCondition();
    });

    $('.module-content .select input').keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            $('#search-earning').trigger('click');
        }
    });

    var d = new Date();
    let month = d.getMonth();
    let year = d.getFullYear();
    $('.search-box .only-month').val((month <= 9 ? '0' + month : month) + '/' + year);

    $('.search-box select').select2();


    // update table data by size page
    $('#size-page select').on('change', function () {
        $('.loading-wrapper').show();
        SetupPaginationSingleCondition();
    });
    //// first loading
    //SetupPagination();

    $('body').on('click', 'tr', function () {
        $(this).parent().children().removeClass("selected");
        $(this).addClass("selected");
    });

    SetupPaginationSingleCondition();
});

var LoadBranchesToForm = function (target, selected = null) {
    $.get(selectUrl + "GetAllBranches?", function (res) {
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            var el = $(target).find('.select-branch');
            el.html('');
            el.append(`<option value="">Chọn chi nhánh</option>`);
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.branchCode}">${item.branchName}</option>`);
            });
            if (selected) {
                $(el).val(selected);
                $(el).trigger('change');
            }
        }
    });
}

var LoadDeptsToForm = function (target, branch, selected = null) {
    $.get(selectUrl + "GetDepartments?branch=" + branch, function (res) {
        var el = $(target).find('.select-dept');
        el.html('');
        el.append(`<option value="">Chọn phòng ban</option>`);
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.departmentCode}">${item.departmentName}</option>`);
            });
            if (selected) {
                $(el).val(selected);
                $(el).trigger('change');
            }
        }
    });
}

var LoadTeamsToForm = function (target, dept, selected = null) {
    $.get(selectUrl + "GetTeams?department=" + dept, function (res) {
        var el = $(target).find('.select-team');
        el.html('');
        el.append(`<option value="">Chọn nhóm</option>`);
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.teamCode}">${item.name}</option>`);
            });
            if (selected) {
                $(el).val(selected);
                $(el).trigger('change');
            }
        }
    });
}

var SetupPaginationSingleCondition = function () {
    //console.log('load');
    $('#pagination').pagination({
        ajax: function (options, refresh, $target) {
            $.ajax({
                url: actionUrl + "GetList",
                data: {
                    month: $('.search-box').find('input[name="Month"]').val().split('/').reverse().join('/'),
                    size: $('#size-page select').val(),
                    page: options.current
                },
                method: 'GET',
                dataType: 'json'
            }).done(function (res) {
                let div = $('#table-body');
                div.html('');
                if (res.result == 200) {
                    $.each(res.data, function (index, item) {
                        if (item.positionName == 'ADMIN') {
                            div.append(`
                                <tr>
                                    <td>${item.codeStaff}</td>
                                    <td>${item.fullName}</td>
                                    <td>${item.teamName ?? '_'}</td>
                                    <td>${item.departmentName ?? '_'}</td>
                                    <td>${item.branchName ?? '_'}</td>
                                    <td>${item.positionName ?? '_'}</td>
                                    <td>${item.totalWorkingDaysRule ?? 0}</td>
                                    <td>${item.totalWorkingDays ?? 0}</td>
                                    <td>${item.totalEarlyOuts ?? 0}</td>
                                    <td>${item.totalLates ?? 0}</td>
                                    <td>${item.forgetCheckOutIn ?? 0}</td>
                                    <td>${item.totalWithoutReason ?? 0}</td>
                                    <td>${item.totalTakeLeaveInMonth ?? 0}</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>${commaSeparateNumber(item.revenueBranch ?? 0)}</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>${commaSeparateNumber(item.salary)}</td>
                                    <td>${commaSeparateNumber(item.amountUnion)}</td>
                                    <td>${commaSeparateNumber(item.amountAttendance)}</td>
                                    <td>${commaSeparateNumber(item.bonus)}</td>
                                    <td>${commaSeparateNumber(item.amountPunish)}</td>
                                    <td>${commaSeparateNumber(item.otherBonus)}</td>
                                    <td>${commaSeparateNumber(item.totalReal)}</td>
                                </tr>`);
                        }
                        else if (item.positionName == 'TELESALE' || item.positionName == 'TELESALE LEADER') {
                            div.append(`
                                <tr>
                                    <td>${item.codeStaff}</td>
                                    <td>${item.fullName}</td>
                                    <td>${item.teamName ?? '_'}</td>
                                    <td>${item.departmentName ?? '_'}</td>
                                    <td>${item.branchName ?? '_'}</td>
                                    <td>${item.positionName ?? '_'}</td>
                                    <td>${item.totalWorkingDaysRule ?? 0}</td>
                                    <td>${item.totalWorkingDays ?? 0}</td>
                                    <td>${item.totalEarlyOuts ?? 0}</td>
                                    <td>${item.totalLates ?? 0}</td>
                                    <td>${item.forgetCheckOutIn ?? 0}</td>
                                    <td>${item.totalWithoutReason ?? 0}</td>
                                    <td>${item.totalTakeLeaveInMonth ?? 0}</td>
                                    <td>${item.totalShowUpInMonth ?? 0}</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>${commaSeparateNumber(item.salary)}</td>
                                    <td>${commaSeparateNumber(item.amountUnion)}</td>
                                    <td>${commaSeparateNumber(item.amountAttendance)}</td>
                                    <td>${commaSeparateNumber(item.bonus)}</td>
                                    <td>${commaSeparateNumber(item.amountPunish)}</td>
                                    <td>${commaSeparateNumber(item.otherBonus)}</td>
                                    <td>${commaSeparateNumber(item.totalReal)}</td>
                                </tr>
                            `);
                        }
                        else {
                            div.append(`
                                <tr>
                                    <td>${item.codeStaff}</td>
                                    <td>${item.fullName}</td>
                                    <td>${item.teamName ?? '_'}</td>
                                    <td>${item.departmentName ?? '_'}</td>
                                    <td>${item.branchName ?? '_'}</td>
                                    <td>${item.positionName ?? '_'}</td>
                                    <td>${item.totalWorkingDaysRule ?? 0}</td>
                                    <td>${item.totalWorkingDays ?? 0}</td>
                                    <td>${item.totalEarlyOuts ?? 0}</td>
                                    <td>${item.totalLates ?? 0}</td>
                                    <td>${item.forgetCheckOutIn ?? 0}</td>
                                    <td>${item.totalWithoutReason ?? 0}</td>
                                    <td>${item.totalTakeLeaveInMonth ?? 0}</td>
                                    <td>_</td>
                                    <td>${commaSeparateNumber(item.revenueInMonth ?? 0)}</td>
                                    <td>${item.positionName.indexOf('LEADER') != -1 ? commaSeparateNumber(item.revenueTeam ?? 0) : '_'}</td>
                                    <td>${item.positionName.indexOf('MANAGER') != -1 ? commaSeparateNumber(item.revenueDepartment ?? 0) : '_'}</td>
                                    <td>${item.positionName.indexOf('DIRECTOR') != -1 ? commaSeparateNumber(item.revenueOffice ?? 0) : '_'}</td>
                                    <td>_</td>
                                    <td>${commaSeparateNumber(item.saleHoldingStock)}</td>
                                    <td>${item.positionName.indexOf('LEADER') != -1 ? commaSeparateNumber(item.leaderSaleHoldingStock) : '_'}</td>
                                    <td>${item.positionName.indexOf('MANAGER') != -1 ? commaSeparateNumber(item.manageSaleHoldingStock) : '_'}</td>
                                    <td>${item.positionName.indexOf('DIRECTOR') != -1 ? commaSeparateNumber(item.officeManageHoldingStock) : '_'}</td>
                                    <td>${commaSeparateNumber(item.sumHoldingStocksInMonth)}</td>
                                    <td>${commaSeparateNumber(item.salary)}</td>
                                    <td>${commaSeparateNumber(item.amountUnion)}</td>
                                    <td>${commaSeparateNumber(item.amountAttendance)}</td>
                                    <td>${commaSeparateNumber(item.bonus)}</td>
                                    <td>${commaSeparateNumber(item.amountPunish)}</td>
                                    <td>${commaSeparateNumber(item.otherBonus)}</td>
                                    <td>${commaSeparateNumber(item.totalReal)}</td>
                                </tr>
                            `);
                        }
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

// setup pagination having filter many conditions
var SetupPagination = function () {
    //console.log('load');
    $('#pagination').pagination({
        ajax: function (options, refresh, $target) {
            let data = $('#search-form').serializeArray();
            data.push({ name: "Page", value: options.current });
            data.push({ name: "Size", value: $('#size-page select').val().split('/').reverse().join('/') });
            $.ajax({
                url: actionUrl + "GetList",
                data: data,
                method: 'POST',
                dataType: 'json'
            }).done(function (res) {
                //console.log(res);
                let div = $('#table-body');
                div.html('');
                if (res.result == 200) {
                    $.each(res.data, function (index, item) {
                        if (item.positionName == 'ADMIN') {
                            div.append(`
                                <tr>
                                    <td>${item.codeStaff}</td>
                                    <td>${item.fullName}</td>
                                    <td>${item.teamName ?? '_'}</td>
                                    <td>${item.departmentName ?? '_'}</td>
                                    <td>${item.branchName ?? '_'}</td>
                                    <td>${item.positionName ?? '_'}</td>
                                    <td>${item.totalWorkingDaysRule ?? 0}</td>
                                    <td>${item.totalWorkingDays ?? 0}</td>
                                    <td>${item.totalEarlyOuts ?? 0}</td>
                                    <td>${item.totalLates ?? 0}</td>
                                    <td>${item.forgetCheckOutIn ?? 0}</td>
                                    <td>${item.totalWithoutReason ?? 0}</td>
                                    <td>${item.totalTakeLeaveInMonth ?? 0}</td>
                                    <td>${item.totalShowUpInMonth ?? '_'}</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>${commaSeparateNumber(item.revenueBranch ?? 0)}</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>${commaSeparateNumber(item.salary)}</td>
                                    <td>${commaSeparateNumber(item.amountUnion)}</td>
                                    <td>${commaSeparateNumber(item.amountAttendance)}</td>
                                    <td>${commaSeparateNumber(item.bonus)}</td>
                                    <td>${commaSeparateNumber(item.amountPunish)}</td>
                                    <td>${commaSeparateNumber(item.otherBonus)}</td>
                                    <td>${commaSeparateNumber(item.totalReal)}</td>
                                </tr>`);
                        }
                        else if (item.positionName == 'TELESALE' || item.positionName == 'TELESALE LEADER') {
                            div.append(`
                                <tr>
                                    <td>${item.codeStaff}</td>
                                    <td>${item.fullName}</td>
                                    <td>${item.teamName ?? '_'}</td>
                                    <td>${item.departmentName ?? '_'}</td>
                                    <td>${item.branchName ?? '_'}</td>
                                    <td>${item.positionName ?? '_'}</td>
                                    <td>${item.totalWorkingDaysRule ?? 0}</td>
                                    <td>${item.totalWorkingDays ?? 0}</td>
                                    <td>${item.totalEarlyOuts ?? 0}</td>
                                    <td>${item.totalLates ?? 0}</td>
                                    <td>${item.forgetCheckOutIn ?? 0}</td>
                                    <td>${item.totalWithoutReason ?? 0}</td>
                                    <td>${item.totalTakeLeaveInMonth ?? 0}</td>
                                    <td>${item.totalShowUpInMonth ?? 0}</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>_</td>
                                    <td>${commaSeparateNumber(item.salary)}</td>
                                    <td>${commaSeparateNumber(item.amountUnion)}</td>
                                    <td>${commaSeparateNumber(item.amountAttendance)}</td>
                                    <td>${commaSeparateNumber(item.bonus)}</td>
                                    <td>${commaSeparateNumber(item.amountPunish)}</td>
                                    <td>${commaSeparateNumber(item.otherBonus)}</td>
                                    <td>${commaSeparateNumber(item.totalReal)}</td>
                                </tr>
                            `);
                        }
                        else {
                            div.append(`
                                <tr>
                                    <td>${item.codeStaff}</td>
                                    <td>${item.fullName}</td>
                                    <td>${item.teamName ?? '_'}</td>
                                    <td>${item.departmentName ?? '_'}</td>
                                    <td>${item.branchName ?? '_'}</td>
                                    <td>${item.positionName ?? '_'}</td>
                                    <td>${item.totalWorkingDaysRule ?? 0}</td>
                                    <td>${item.totalWorkingDays ?? 0}</td>
                                    <td>${item.totalEarlyOuts ?? 0}</td>
                                    <td>${item.totalLates ?? 0}</td>
                                    <td>${item.forgetCheckOutIn ?? 0}</td>
                                    <td>${item.totalWithoutReason ?? 0}</td>
                                    <td>${item.totalTakeLeaveInMonth ?? 0}</td>
                                    <td>_</td>
                                    <td>${commaSeparateNumber(item.revenueInMonth ?? 0)}</td>
                                    <td>${item.positionName.indexOf('LEADER') != -1 ? commaSeparateNumber(item.revenueTeam ?? 0) : '_'}</td>
                                    <td>${item.positionName.indexOf('MANAGER') != -1 ? commaSeparateNumber(item.revenueDepartment ?? 0) : '_'}</td>
                                    <td>${item.positionName.indexOf('DIRECTOR') != -1 ? commaSeparateNumber(item.revenueOffice ?? 0) : '_'}</td>
                                    <td>_</td>
                                    <td>${commaSeparateNumber(item.saleHoldingStock)}</td>
                                    <td>${item.positionName.indexOf('LEADER') != -1 ? commaSeparateNumber(item.leaderSaleHoldingStock) : '_'}</td>
                                    <td>${item.positionName.indexOf('MANAGER') != -1 ? commaSeparateNumber(item.manageSaleHoldingStock) : '_'}</td>
                                    <td>${item.positionName.indexOf('DIRECTOR') != -1 ? commaSeparateNumber(item.officeManageHoldingStock) : '_'}</td>
                                    <td>${commaSeparateNumber(item.sumHoldingStocksInMonth)}</td>
                                    <td>${commaSeparateNumber(item.salary)}</td>
                                    <td>${commaSeparateNumber(item.amountUnion)}</td>
                                    <td>${commaSeparateNumber(item.amountAttendance)}</td>
                                    <td>${commaSeparateNumber(item.bonus)}</td>
                                    <td>${commaSeparateNumber(item.amountPunish)}</td>
                                    <td>${commaSeparateNumber(item.otherBonus)}</td>
                                    <td>${commaSeparateNumber(item.totalReal)}</td>
                                </tr>
                            `);
                        }
                    });
                    refresh({
                        total: res.total, // optional
                        length: $('#size-page select').val()// optional
                    });
                    $('#size-page .from').text($('#size-page select').val() * (options.current - 1) + 1);
                    $('#size-page .to').text($('#size-page select').val() * options.current);
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