let baseUrl = '/Admin/Accounts/';
$(function () {
    //search
    $('#search-sale .select-branch').on('change', function () {
        $('#search-sale .select-department').html(`<option value="">Phòng</option>`);
        $('#search-sale .select-team').html(`<option value="">Nhóm</option>`);
        $('#search-sale .select-staff').html(`<option value="">Nhân viên Sale</option>`);

        let selectedBranch = $(this).val();
        if (selectedBranch) {
            LoadOfficesToForm('#search-sale', selectedBranch);
        }
    });
    $('#search-sale .select-office').on('change', function () {
        $('#search-sale .select-team').html(`<option value="">Nhóm</option>`);
        $('#search-sale .select-staff').html(`<option value="">Nhân viên Sale</option>`);

        let selectedOffice = $(this).val();
        if (selectedOffice) {
            LoadDeptsToForm('#search-sale', selectedOffice);
        }
    });

    $('#search-sale .select-department').on('change', function () {
        $('#search-sale .select-staff').html(`<option value="">Nhân viên Sale</option>`);

        let selectedDept = $(this).val();
        if (selectedDept) {
            LoadTeamsToForm('#search-sale', selectedDept);
        }
    });
    $('#search-sale .select-team').on('change', function () {
        let selectedTeam = $(this).val();
        if (selectedTeam) {
            LoadStaffsToForm('#search-sale', selectedTeam);
        }
    });
    //create
    $('#employee-sales-add .select-branch').on('change', function () {
        $('#employee-sales-add .select-department').html(`<option value="">Phòng</option>`);
        $('#employee-sales-add .select-team').html(`<option value="">Nhóm</option>`);
        $('#employee-sales-add .select-staff').html(`<option value="">Nhân viên Sale</option>`);

        let selectedBranch = $(this).val();
        if (selectedBranch) {
            LoadOfficesToForm('#employee-sales-add', selectedBranch);
        }
    });
    $('#employee-sales-add .select-office').on('change', function () {
        $('#employee-sales-add .select-team').html(`<option value="">Nhóm</option>`);
        $('#employee-sales-add .select-staff').html(`<option value="">Nhân viên Sale</option>`);

        let selectedOffice = $(this).val();
        if (selectedOffice) {
            LoadDeptsToForm('#employee-sales-add', selectedOffice);
        }
    });

    $('#employee-sales-add .select-department').on('change', function () {
        $('#employee-sales-add .select-staff').html(`<option value="">Nhân viên Sale</option>`);
        let selectedDept = $(this).val();
        if (selectedDept) {
            LoadTeamsToForm('#employee-sales-add', selectedDept);
        }
    });
    $('#employee-sales-add .select-team').on('change', function () {
        let selectedTeam = $(this).val();
        if (selectedTeam) {
            LoadStaffsToForm('#employee-sales-add', selectedTeam);
        }
    });
    //edit index
    $('body').on('change', '#employee-sales-edit .select-branch', function () {
        $('#employee-sales-edit .select-department').html(`<option value="">Phòng</option>`);
        $('#employee-sales-edit .select-team').html(`<option value="">Nhóm</option>`);
        $('#employee-sales-edit .select-staff').html(`<option value="">Nhân viên Sale</option>`);

        let selectedBranch = $(this).val();
        if (selectedBranch) {
            LoadOfficesToForm('#employee-sales-edit', selectedBranch);
        }
    });
    $('body').on('change', '#employee-sales-edit .select-office', function () {
        $('#employee-sales-edit .select-team').html(`<option value="">Nhóm</option>`);
        $('#employee-sales-edit .select-staff').html(`<option value="">Nhân viên Sale</option>`);

        let selectedOffice = $(this).val();
        if (selectedOffice) {
            LoadDeptsToForm('#employee-sales-edit', selectedOffice);
        }
    });

    $('body').on('change', '#employee-sales-edit .select-department', function () {
        $('#employee-sales-edit .select-staff').html(`<option value="">Nhân viên Sale</option>`);
        let selectedDept = $(this).val();
        if (selectedDept) {
            LoadTeamsToForm('#employee-sales-edit', selectedDept);
        }
    });
    $('body').on('change', '#employee-sales-edit .select-team', function () {
        let selectedTeam = $(this).val();
        if (selectedTeam) {
            LoadStaffsToForm('#employee-sales-edit', selectedTeam);
        }
    });

    $('#search-personal').on('click', function () {
        app.component.Loading.Show();
        let data = $('#search-form').serializeObject();
        $('#pagination').pagination({
            ajax: function (options, refresh, $target) {
                data.page = options.current;

                $.ajax({
                    url: baseUrl + "GetList",
                    data: {
                        model: data
                    },
                    method: 'POST',
                    dataType: 'json'
                }).done(function (res) {
                    //console.log(res);
                    let div = $('#table-body');
                    div.html('');
                    div.html(`
                     <tr>
                          <th>
                                <div class="custom-checkbox check_all">
                                    <input type="checkbox" />
                                    <span class="checkmark"></span>
                                </div>
                            </th>
                            <th>Mã ID</th>
                            <th>UserName</th>
                            <th>Họ và tên</th>
                            <th>Trạng thái</th>
                            <th>Người tạo</th>
                            <th>Ngày tạo</th>
                            <th>Action</th>
                    </tr>
                `);

                    if (res.result != 400) {
                        $.each(res.data, function (index, item) {
                            div.append(`
                        <tr>
                             <td>
                                <div class="custom-checkbox check_all">
                                    <input type="checkbox" name="checkAccount" data-id="${item.accountID}" />
                                    <span class="checkmark"></span>
                                </div>
                            </td>
                            <td>${item.accountID}</td>
                            <td>
                                <button type="button" class="view-info" data-id="${item.accountID}">
                                    ${item.accountName}
                                </button>
                            </td>
                           <td>${item.accountFullName}</td>
                            <td>${item.status}</td>
                            <td>${item.nameUserCreate}</td>
                            <td>${item.createDateString}</td>
                            <td>
                                <button type="button" class="delete-cms-account" data-id="${item.accountID}">
                                    <img src="/Assets/crm/images/employee-manage/delete.svg" alt="">
                                </button>
                            </td>
                        </tr>`);
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
                    app.component.Loading.Hide();
                }).fail(function (error) {
                    app.component.Loading.Hide();
                });
            }
        });
    });

    $('body').on('click', 'button.view-info', function () {
        ViewAccount($(this).data('id'));
    })

    $('body').on('click', 'button#Del-Employ', function () {
        app.component.Loading.Show();
        let selectedRow = $('input[name="checkAccount"]:checked');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một nhân viên để xóa');
            $('.loading-wrapper').hide();
            return;
        }
        let id = selectedRow.data('id');
        $.get(baseUrl + 'IsDelete?id=' + id, function (res) {
            //console.log(res);
            if (res.status == false) {
                toastr.error(res.mess);
                app.component.Loading.Hide();
            }
            else {
                ShowModal('#delete-employee_modal');
                $('#txt-del-account').html(`Bạn có chắc muốn xóa ?`)
                app.component.Loading.Hide();

                $('#delete-empl').click(function () {
                    app.component.Loading.Show();
                    $.get(baseUrl + 'Delete?id=' + id, function (res) {
                        if (res.status) {
                            CloseModal('#delete-employee_modal');
                            SetupPagination();
                            app.component.Loading.Hide();
                            toastr.success('xóa thành công', 'Thông báo');
                        }
                        else {
                            toastr.error(res.mess, 'Thông báo');
                            app.component.Loading.Hide();
                        }
                    });
                });
            }
        });
    })   
   
    $('.btn-add-person').on('click', function () {
        let data = $('#form-model-add').serializeObject();
        app.component.Loading.Show();
        $.ajax({
            method: 'POST',
            url: baseUrl + 'InsertOrUpdate',
            data: {
                model: data
            },
            success: function (rs) {
                if (rs.status) {
                    toastr.success('Thêm mới nhân sự thành công!', 'Thông báo');
                    ResetValueInForm('#form-model-add');
                    $('.close__modal').trigger('click');
                    SetupPagination();
                    app.component.Loading.Hide();
                    /*$('.close__btn').trigger('click');*/

                } else {
                    const objResult = JSON.parse(rs.data);
                    objResult.map(function (item) {
                        toastr.error(`${item.ErrorMessage}`, 'Thông báo');
                    });
                    toastr.error(`có lỗi xảy ra`, 'Thông báo');
                    app.component.Loading.Hide();
                }
            }
        });
    });

    $('body').on('click', 'button#Edit-Employ', function () {
        let selectedRow = $('input[name="checkAccount"]:checked');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một tài khoản để chỉnh sửa');
            $('.loading-wrapper').hide();
            return;
        }
        let id = selectedRow.data('id');
        EditAccount(id);
    })

    $('body').on('click', '.btn-edit-account', function () {
        let data = $('#form-model-edit').serializeObject();
        data.isEnable = $('#edit-employee_modal input[name="isEnable"]').is(":checked");
        app.component.Loading.Show();
        $.ajax({
            method: 'POST',
            url: baseUrl + 'InsertOrUpdate',
            data: {
                model: data
            },
            success: function (rs) {
                if (rs.status) {
                    toastr.success('Cập nhật thông tin thành công!', 'Thông báo');
                    ResetValueInForm('#form-model-edit');
                    $('#form-model-edit').modal('hide');
                    SetupPagination();
                    app.component.Loading.Hide();
                    $('.close__modal').trigger('click');
                } else {
                    const objResult = JSON.parse(rs.data);
                    objResult.map(function (item) {
                        toastr.error(`${item.ErrorMessage}`, 'Thông báo');
                    });
                    toastr.error(`có lỗi xảy ra`, 'Thông báo');
                    app.component.Loading.Hide();
                }
            }
        });
    });

    SetupPagination();

    $('#size-page select').on('change', function () {
        $('.loading-wrapper').show();
        SetupPagination();
    });

    //khanhkk added
    $('#search-form select').select2();
    //khanhkk added
});

var ViewAccount = function (id) {
    $.get(baseUrl + "View?id=" + id, function (res) {
        if (res.result == 400) {
            toastr.error(res.errors.join('<br />'));
        }
        else {
            $('#detail-employee_modal .content-modal').html(res);
            $('#detail-employee_modal').addClass('show-modal');
            $('#detail-employee_modal .content-modal').addClass('show-modal');
        }
    }).done(function (res) {
        let checked = $('#detail-employee_modal .isEnable-hidden').val();
        if (checked) {
            $('#detail-employee_modal input[name="isEnable"]').prop('checked', true);
        }
        $('#detail-employee_modal input').attr('disabled', 'disabled');
        $('#detail-employee_modal select').attr('disabled', 'disabled');
    });
}

var EditAccount = function (id) {
    $.get(baseUrl + "Edit?id=" + id, function (res) {
        if (res.result == 400) {
            toastr.error(res.errors.join('<br />'));
        }
        else {
            $('#edit-employee_modal .content-modal').html(res);
            $('#edit-employee_modal').addClass('show-modal');
            $('#edit-employee_modal .content-modal').addClass('show-modal');
        }
    }).done(function () {
        let checked = $('#edit-employee_modal .isEnable-hidden').val();
        if (checked) {
            $('#edit-employee_modal input[name="isEnable"]').prop('checked', true);
        }
    });
}

var LoadBranchesToForm = function (target, selected = null) {
    $.get(baseUrl + "GetAllBranches?", function (res) {
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
                /*$(el).trigger('change');*/
            }
        }
    });
}

var LoadOfficesToForm = function (target, branch, selected = null) {
    $.get(baseUrl + "GetAllOffice?branch=" + branch, function (res) {
        var el = $(target).find('.select-office');
        el.html('');
        el.append(`<option value="">Chọn khối</option>`);
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.officeCode}">${item.officeName}</option>`);
            });
            if (selected) {
                $(el).val(selected);
                /*$(el).trigger('change');*/
            }
        }
    });
}


var LoadDeptsToForm = function (target, office, selected = null) {
    $.get(baseUrl + "GetDepartments?office=" + office, function (res) {
        var el = $(target).find('.select-department');
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
                /*$(el).trigger('change');*/
            }
        }
    });
}

var LoadTeamsToForm = function (target, dept, selected = null) {
    $.get(baseUrl + "GetTeams?department=" + dept, function (res) {
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
                //$(el).trigger('change');
            }
        }
    });
}

var LoadStaffsToForm = function (target, team, selected = null) {
    $.get(baseUrl + "GetStaffSale?teamCode=" + team, function (res) {
        var el = $(target).find('.select-staff');
        el.html('');
        el.append(`<option value="">Chọn nhân viên</option>`);
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.codeStaff}">${item.fullName}</option>`);
            });
            if (selected) {
                $(el).val(selected);
                //$(el).trigger('change');
            }
        }
    });
}

var LoadRolesToForm = function (target, selected = null) {
    $.get(baseUrl + "GetRoles", function (res) {
        //console.log(res);
        var el = $(target).find('.select-role');
        el.html('');
        el.append(`<option value="">Chọn chức vụ</option>`);
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.key}">${item.value}</option>`);
            });
            if (selected) {
                $(el).val(selected);
                $(el).trigger('change');
            }
        }
    });
}

var LoadSocialInsuranceToForm = function (target, selected = null) {
    $.get(baseUrl + "GetAllowanceOrDeductByType?type=3", function (res) {
        var el = $(target).find('.select-insurance');
        el.html('');
        el.append(`<option value="">Chọn bảo hiểm</option>`);
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.allowanceCode}">${item.allowanceName}</option>`);
            });
            if (selected) {
                $(el).val(selected);
                $(el).trigger('change');
            }
        }
    });
}

let branchSearch = $('#search-form select.select-branch').val();
// setup pagination
var SetupPagination = function () {
    //console.log('load');
    app.component.Loading.Show();
    $('#pagination').pagination({
        ajax: function (options, refresh, $target) {
            $.ajax({
                url: baseUrl + "GetList",
                data: {
                    Page: options.current,
                    Size: $('#size-page select').val(),
                    Key: $('#search-key').val()
                },
                method: 'POST',
                dataType: 'json'
            }).done(function (res) {
                //console.log(res);
                let div = $('#table-body');
                div.html('');
                div.html(`
                    <tr>
                          <th>
                                <div class="custom-checkbox check_all">
                                    <input type="checkbox" />
                                    <span class="checkmark"></span>
                                </div>
                            </th>
                            <th>Mã ID</th>
                            <th>UserName</th>
                            <th>Họ và tên</th>
                            <th>Trạng thái</th>
                            <th>Người tạo</th>
                            <th>Ngày tạo</th>
                            <th>Action</th>
                    </tr>
                `);

                if (res.result != 400) {
                    $.each(res.data, function (index, item) {
                        div.append(`
                        <tr>
                            <td>
                                <div class="custom-checkbox check_all">
                                    <input type="checkbox" name="checkAccount" data-id="${item.accountID}" />
                                    <span class="checkmark"></span>
                                </div>
                            </td>
                            <td>${item.accountID}</td>
                            <td>
                                <button type="button" class="view-info" data-id="${item.accountID}">
                                    ${item.accountName}
                                </button>
                            </td>
                           <td>${item.accountFullName}</td>
                            <td>${item.status}</td>
                            <td>${item.nameUserCreate}</td>
                            <td>${item.createDateString}</td>
                            <td>
                                <button type="button" class="delete-cms-account" data-id="${item.accountID}">
                                    <img src="/Assets/crm/images/employee-manage/delete.svg" alt="">
                                </button>
                            </td>
                        </tr>`);
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
                app.component.Loading.Hide();
            }).fail(function (error) {
                app.component.Loading.Hide();
            });
        }
    });
}