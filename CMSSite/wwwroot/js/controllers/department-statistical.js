let baseUrl = '/Admin/DepartmentStatistics/';
let selectUrl = '/SelectionData/';

$(function () {
    let branch = $('#hidden-branch').val();
    let office = $('#hidden-office').val();

    LoadDepartmentToForm('.depart-select');
    $('.loading-wrapper').show();
    GetData(branch, office);

    $('#search-statistic').on('click', function () {
        $('.loading-wrapper').show();
        let dept = $('.depart-select').val();
        let month = $('#selected-month').val();
        let selectedMonth;
        if (month) {
            selectedMonth = month.split("-").reverse().join('/');
        }
        GetData(branch, office, selectedMonth, dept);
    });

    $('#search-team-data').on('click', function () {
        $('.loading-wrapper').show();
        let dept = $('#search-team-data').data('hiddenDepartment');
        let team = $('#admin_branch-sale_detail-modal .team-sale-select').val();
        let month = $('#admin_branch-sale_detail-modal .only-month.sale-team').val();
        let selectedMonth;
        if (month) {
            selectedMonth = month.split("-").reverse().join('/');
        }
        GetTeamData(branch, office, dept, selectedMonth, team);
    });

    $('#search-sale-data').on('click', function () {
        $('.loading-wrapper').show();
        let dept = $('#search-sale-data').data('hiddenDepart');
        let team = $('#search-sale-data').data('hiddenTeam');
        let staff = $('#admin_branch-sale_sale-modal #staff-key').val();
        let month = $('#admin_branch-sale_sale-modal .only-month.sale').val();
        let selectedMonth;
        if (month) {
            selectedMonth = month.split("-").reverse().join('/');
        }
        GetPersonalData(branch, office, dept, team, selectedMonth, staff);
    });
});

var GetData = function (branch, office, month = null, depart = null) {
    $('#table-body').html(`
                    <tr>
                        <th>ID</th>
                        <th>Tên phòng</th>
                        <th>Chi nhánh</th>
                        <th>Doanh số phòng</th>
                        <th>Hoa hồng phòng</th>
                    </tr>
                `);
    $.get(baseUrl + 'GetList', { month: month, branch: branch, office: office, department: depart }, function (res) {
        //console.log(res);
        if (res.result == 200) {
            $.each(res.data.listData, function (index, item) {
                $('#table-body').append(
                    `
                                <tr>
                                    <td>
                                        <button type="button"
                                                onclick="DisplayTeamStatisticalData('${branch}', '${office}', '${item.departmentCode}')"
                                            >
                                            ${item.departmentCode}
                                        </button>
                                    </td>
                                    <td>${item.departmentName}</td>
                                    <td>${item.branchName}</td>
                                    <td>${commaSeparateNumber(item.departmentRevenue)}</td>
                                    <td>${commaSeparateNumber(item.departmentRemuneration)}</td>
                                </tr>
                            `
                )
            });

            if (!depart) {
                $('#total-revenue').text(res.data.totalRevenue / 1000000000);
                $('#total-remuneration').text(commaSeparateNumber(res.data.totalRemuneration));
            }
            $('.loading-wrapper').hide();
        }
        else {
            toastr.error(res.errors);
            $('.loading-wrapper').hide();
        }
    });
}

var LoadDepartmentToForm = function (target, selected = null) {
    $.get(selectUrl + "GetDepartmentsInBranch?branch=" + $('#hidden-branch').val(), function (res) {
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
            var el = $(target);
            el.html('');
            el.append(`<option value="">--</option>`);
        }
        else {
            var el = $(target);
            el.html('');
            el.append(`<option value="">Chọn phòng ban</option>`);
            $.each(res.data, function (index, item) {
                //console.log(item);
                el.append(`<option value="${item.departmentCode}">${item.departmentName}</option>`);
            });
            if (selected) {
                $(el).val(selected);
                /*$(el).trigger('change');*/
            }
        }
    });
}

var LoadTeamToForm = function (target, department, selected = null) {
    $.get(selectUrl + "GetTeams?department=" + department, function (res) {
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
            var el = $(target);
            el.html('');
            el.append(`<option value="">--</option>`);
        }
        else {
            var el = $(target);
            el.html('');
            el.append(`<option value="">Chọn nhóm</option>`);
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.teamCode}">${item.name}</option>`);
            });
            if (selected) {
                $(el).val(selected);
            }
        }
    });
}

var GetTeamData = function (branch, office, department, month = null, team = null) {
    $('.loading-wrapper').show();
    $('#sale-team-table-body').html(`
                <tr>
                    <th>ID</th>
                    <th>Tên nhóm</th>
                    <th>Phòng ban</th>
                    <th>Chi nhánh</th>
                    <th>Doanh số</th>
                    <th>Hoa hồng</th>
                </tr>
            `);
    $.get(baseUrl + 'GetStatisticalDataForTeam',
        {
            month: month,
            branch: branch,
            office: office,
            department: department,
            team: team
        }, function (res) {
            //console.log(res);
            if (res.result == 200) {
                $.each(res.data.listData, function (index, item) {
                    $('#sale-team-table-body').append(
                        `
                                <tr>
                                    <td>
                                        <button type="button"
                                                onclick="DisplayPersonalStatisticalData('${branch}', '${office}', '${department}','${item.teamCode}')"
                                            >
                                            ${item.teamCode}
                                        </button>
                                    </td>
                                    <td>${item.teamName}</td>
                                    <td>${item.departmentName}</td>
                                    <td>${item.branchName}</td>
                                    <td>${commaSeparateNumber(item.teamRevenue)}</td>
                                    <td>${commaSeparateNumber(item.teamRemuneration)}</td>
                                </tr>
                            `
                    )
                });

                //if (!depart) {
                //    $('#total-revenue').text(res.data.totalRevenue / 1000000000);
                //    $('#total-remuneration').text(commaSeparateNumber(res.data.totalRemuneration));
                //}
                $('.loading-wrapper').hide();
            }
            else {
                toastr.error(res.errors);
                $('.loading-wrapper').hide();
            }
        });
}

var DisplayTeamStatisticalData = function (branch, office, department) {
    $('#search-team-data').data('hiddenDepartment', department);
    GetTeamData(branch, office, department);
    LoadTeamToForm($('#admin_branch-sale_detail-modal').find('.team-sale-select'), department);
    $('#admin_branch-sale_detail-modal .only-month').val('');
    ShowModal('#admin_branch-sale_detail-modal');
}

var GetPersonalData = function (branch, office, dept, team, month = null, staff = null) {
    $('.loading-wrapper').show();
    $('#sale-staff-table-body').html(`
                <tr>
                    <th>ID</th>
                    <th>Họ và tên</th>
                    <th>Team</th>
                    <th>Phòng ban</th>
                    <th>Chi nhánh</th>
                    <th>Doanh số cá nhân</th>
                    <th>Doanh số chốt hộ</th>
                    <th>Tổng doanh số</th>
                    <th>Hoa hồng</th>
                    <th>Chức vụ</th>
                </tr>
            `);
    $.get(baseUrl + 'GetStatisticalDataForIndividual',
        {
            month: month,
            branch: branch,
            office: office,
            department: dept,
            team: team,
            staff: staff,
        }, function (res) {
            //console.log(res);
            if (res.result == 200) {
                $.each(res.data.listData, function (index, item) {
                    $('#sale-staff-table-body').append(
                        `
                            <tr>
                                <td>${item.codeStaff}</td>
                                <td>${item.fullName}</td>
                                <td>${item.teamName}</td>
                                <td>${item.departmentName}</td>
                                <td>${item.branchName}</td>
                                <td>${commaSeparateNumber(item.personalRevenueS)}</td>
                                <td>${commaSeparateNumber(item.personalRevenueTO)}</td>
                                <td>${commaSeparateNumber(item.personalRevenue)}</td>
                                <td>${commaSeparateNumber(item.personalRemuneration)}</td>
                                <td>${GetRoleName(item.role)}</td>
                            </tr>
                        `
                    )
                });

                $('.loading-wrapper').hide();
            }
            else {
                toastr.error(res.errors);
                $('.loading-wrapper').hide();
            }
        });
}

var DisplayPersonalStatisticalData = function (branch, office, depart, team) {
    $('#search-sale-data').data('hiddenDepart', depart);
    $('#search-sale-data').data('hiddenTeam', team);
    GetPersonalData(branch, office, depart, team);
    //GetPersonalData($('#admin_branch-sale_detail-modal').find('.team-sale-select'), team);
    $('#admin_branch-sale_sale-modal .only-month.sale').val('');
    ShowModal('#admin_branch-sale_sale-modal');
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