let actionUrl = '/Sale/TemporaryPaySlip/';
$(function () {

    $('#search-earning').on('click', function () {
        $('.loading-wrapper').show();

        //SetupPagination();
        SetupPaginationSingleCondition();
    });

    var d = new Date();
    let month = d.getMonth() + 1;
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

    SetupPaginationSingleCondition();
});

var SetupPaginationSingleCondition = function () {
    //console.log('load');
    $('#pagination').pagination({
        ajax: function (options, refresh, $target) {
            $.ajax({
                url: actionUrl + "GetList",
                //data: {
                //    month: $('.search-box').find('input[name="Month"]').val(),
                //    size: $('#size-page select').val(),
                //    page: options.current
                //},
                method: 'GET',
                dataType: 'json'
            }).done(function (res) {
                let div = $('#table-body');
                div.html('');
                if (res.result == 200) {
                    $.each(res.data, function (index, item) {
                        div.append(`
                            <tr>
                                <td>${item.codeStaff}</td>
                                <td>${item.fullName}</td>
                                <td>${item.teamName ?? '_'}</td>
                                <td>${item.departmentName ?? '_'}</td>
                                <td>${item.branchName ?? '_'}</td>
                                <td>${item.positionName ?? '_'}</td>
                                <td>${item.totalWorkingDaysRule ?? 0}</td>
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
                                <td>${commaSeparateNumber(item.otherBonus)}</td>
                                <td>${commaSeparateNumber(item.totalReal)}</td>
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
            data.push({ name: "Size", value: $('#size-page select').val() });
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
                        //console.log(item);
                        //<td>${commaSeparateNumber(item.amountSeniority)}</td>
                        //        <td>${commaSeparateNumber(item.amountSocialInsuarance)}</td>
                        div.append(`
                        <tr>
                            <td>${item.codeStaff}</td>
                            <td>${item.fullName}</td>
                            <td>${item.teamName ?? ''}</td>
                            <td>${item.departmentName ?? ''}</td>
                            <td>${item.branchName ?? ''}</td>
                            <td>${item.positionName ?? ''}</td>
                            <td>${item.totalWorkingDaysRule ?? 0}</td>
                            <td>${item.totalWorkingDays ?? 0}</td>
                            <td>${item.totalEarlyOuts ?? 0}</td>
                            <td>${item.totalLates ?? 0}</td>
                            <td>${item.forgetCheckOutIn ?? 0}</td>
                            <td>${item.totalWithoutReason ?? 0}</td>
                            <td>${item.totalTakeLeaveInMonth ?? 0}</td>
                            <td>${item.totalShowUpInMonth ?? ''}</td>
                            <td>${commaSeparateNumber(item.revenueInMonth)}</td>
                            <td>${commaSeparateNumber(item.revenueTeam)}</td>
                            <td>${commaSeparateNumber(item.revenueDepartment)}</td>
                            <td>${commaSeparateNumber(item.revenueBranch)}</td>
                            <td>${commaSeparateNumber(item.saleHoldingStock)}</td>
                            <td>${commaSeparateNumber(item.leaderSaleHoldingStock)}</td>
                            <td>${commaSeparateNumber(item.manageSaleHoldingStock)}</td>
                            <td>${commaSeparateNumber(item.officeManageHoldingStock)}</td>
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
                    });
                    refresh({
                        total: res.total, // optional
                        length: $('#size-page select').val()// optional
                    });
                    $('#size-page .from').text($('#size-page select').val() * (options.current - 1) + 1);
                    $('#size-page .to').text($('#size-page select').val() * options.current);
                    $('#size-page .total').text(res.total);
                }
                $('.loading-wrapper').hide();
            }).fail(function (error) {
                $('.loading-wrapper').hide();
            });
        }
    });
}