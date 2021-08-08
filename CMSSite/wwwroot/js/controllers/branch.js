let baseUrl = '/HR/Branchs/';

$(function () {
    LoadStatus('#search-branch-set .select-status-branch');
    $('body').on('click', '#btn-show-add', function () {
        $('#btn-hiid-show').trigger('click');
        $('#add-branch_modal select').select2();
        ResetValueInForm('#add-branch_modal');
    });
    //Insert
    $('body').on('click', '.btn-add-branch', function () {
        let model = $('#form-model-add').serializeObject();
        if (model.BranchName === null || model.BranchName === '') {
            toastr.error('Nhập tên chi nhánh', 'Thông báo');
            return false;
        }
        if (model.Address === null || model.Address === '') {
            toastr.error('Nhập địa chỉ', 'Thông báo');
            return false;
        }
        let data = model;
        _AjaxPostForm(baseUrl + "InsertOrUpdate", data, function (res) {
            if (res.status) {
                toastr.success('Tạo chi nhánh thành công', 'Thông báo');
                $('.close__modal').trigger('click');
                SetupPagination();
            } else {
                toastr.error(res.mess, 'Thông báo');
            }
        });
    });
    $('body').on('click', '.btn-update-branch', function () {
        let model = $('#form-model-edit').serializeObject();
        if (model.BranchName === null || model.BranchName === '') {
            toastr.error('Nhập tên chi nhánh', 'Thông báo');
            return false;
        }
        if (model.Address === null || model.Address === '') {
            toastr.error('Nhập địa chỉ', 'Thông báo');
            return false;
        }
        let data = model;
        _AjaxPostForm(baseUrl + "InsertOrUpdate", data, function (res) {
            if (res.status) {
                toastr.success('Cập nhật chi nhánh thành công', 'Thông báo');
                $('.close__modal').trigger('click');
                SetupPagination();
            } else {
                toastr.error(res.mess, 'Thông báo');
            }
        });
    });
    //View Branch
    $('body').on('click', 'button.view-info', function () {
        ViewPersonalInfo($(this).data('id'));
    })
    //Edit Branch
    $('body').on('click', '#Edit-Branch', function () {
        let selectedRow = $('input[name="select-row"]:checked');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một chi nhánh để chỉnh sửa');
            return;
        }
        let id = selectedRow.data('id');
        if (id) {
            $.get(baseUrl + "Edit?id=" + id, function (res) {
                if (res.result == 400) {
                    toastr.error(res.errors.join('<br />'));
                }
                else {
                    $('#edit-branch_modal .content-modal').html(res);
                    $('#edit-branch_modal').addClass('show-modal');
                    $('#edit-branch_modal .content-modal').addClass('show-modal');
                }
            }).done(function () {
                $('.loading-wrapper').hide();
                //let selectedStatus = $('#edit-branch_modal .hidden-status').val();
                //if (selectedBranch) {
                //    LoadBranchesToForm('#detail-employee_modal', selectedBranch);

                //    let selectedOffice = $('#detail-employee_modal .hidden-office').val();
                //    LoadOfficesToForm('#detail-employee_modal', selectedBranch, selectedOffice);

                //    let selectedDept = $('#detail-employee_modal .hidden-dept').val();
                //    LoadDeptsToForm('#detail-employee_modal', selectedOffice, selectedDept);

                //    let selectedTeam = $('#detail-employee_modal .hidden-team').val();
                //    LoadTeamsToForm('#detail-employee_modal', selectedDept, selectedTeam);

                //    $('#view-info').trigger('click');
                //}

                //let selectedRole = $('#detail-employee_modal .hidden-role').val();
                //LoadRolesToForm('#detail-employee_modal', selectedRole);

                //let selectedInsurance = $('#detail-employee_modal .hidden-insurance').val();
                //LoadSocialInsuranceToForm('#detail-employee_modal', selectedInsurance);

                //let selectedBonus = $('#detail-employee_modal .hidden-bonus').val();
                //LoadSeniorBonusToForm('#detail-employee_modal', selectedBonus);
                let selectedStaff = $('#edit-branch_modal .hidden-staff').val();
                LoadStaffBranch('#edit-branch_modal .select-staff', selectedStaff);
                $('#edit-branch_modal select').select2();
                $(".form_datetime").datetimepicker(
                    {
                        format: 'dd/mm/yyyy',
                        minView: 2,
                        maxView: 4,
                        autoclose: true
                    });

                //$('#detail-employee_modal input').attr('disabled', 'disabled');
                //$('#detail-employee_modal select').attr('disabled', 'disabled');
            });
        }
    })
    //Delete Branh
    $('body').on('click', '#Del-Employ', function () {
        let selectedRow = $('input[name="select-row"]:checked');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một chi nhánh để xóa');
            return;
        }
        let id = selectedRow.data('id');
        if (id) {
            $.get(baseUrl + "Delete?id=" + id, function (res) {
                if (res.result == 400) {
                    toastr.error(res.errors.join('<br />'));
                }
                else {
                    $('#delete-employee_modal .content-modal').html(res);
                    $('#delete-employee_modal').addClass('show-modal');
                    $('#delete-employee_modal .content-modal').addClass('show-modal');
                }
            }).done(function () {
                $('.loading-wrapper').hide();
            });
        }
    })
    //List, Search, Pageing
    $('#search-branch').on('click', function (e) {
        /* e.preventDefault();*/
        app.component.Loading.Show();
        let stt = 1;
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
                                <th>STT</th>
                                <th>Tên chi nhánh</th>
                                <th>Sale Admin</th>
                                <th>Địa chỉ</th>
                                <th>Trạng thái</th>
                           </tr>
                        `);

                    if (res.data != null) {
                        let newData = res.data.map((item, index) => {
                            return item;
                        });

                        $.each(newData, function (index, item) {
                            div.append(`
                                <tr data-id="${item.id}">
                                    <th>
                                        <div class="custom-checkbox check_all">
                                            <input type="checkbox" name="select-row" data-id="${item.id}" />
                                            <span class="checkmark"></span>
                                        </div>
                                    </th>
                                    <td>${stt}</td>
                                    <td>
                                         <button data-id="${item.id}" class="preview-contract">
                                            ${item.branchName}
                                        </button>
                                    </td>
                                    <td>
                                      ${item.nameStaffAdminSale}
                                    </td>
                                    <td>
                                      ${item.address}
                                    </td>
                                    <td>
                                        ${item.status === 'True' ? `<div class="radiotext status"> <label>Đang hoạt động</label> </div>` : `<div class='radiotext status'>
                                            <label>Đang khóa</label>
                                        </div>`}
                                    </td>
                                </tr>`);
                            stt = stt + 1;
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
                        $('#table-body').html(`<span>Không có dữ liệu</span>`);
                        $('.select_pagination').hide();
                        $('#size-page .from').text(0);
                        $('#size-page .to').text(0);
                        $('#size-page .total').text(0);
                    }
                    app.component.Loading.Hide();
                }).fail(function (error) {
                    console.log('false');
                    app.component.Loading.Hide();
                });
            }
        });
    })
    SetupPagination();
    $('#size-page select').on('change', function () {
        app.component.Loading.Show();
        SetupPagination();
    });

});
var LoadSales = function (target, selected) {
    $(target).html('');
    $(target).append(`<option value="">-Chọn sale-</option>`);
    $.get(baseUrl + 'GetSale', function (res) {
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
    $(target).append(`<option value="">-Chọn Telesale-</option>`);
    $.get(baseUrl + 'GetTeleSale', function (res) {
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
var LoadStatus = function (target, selected) {
    $(target).html('');
    $(target).append(`<option value="">-Chọn Trạng thái-</option>`);
    $.get(baseUrl + 'GetContractStaffStatus', function (res) {
        if (res.status == true) {
            $.each(res.data, function (index, item) {
                $(target).append(`<option value="${item.key}">${item.value}</option>`);
                console.log(target);

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
//View Detail
var LoadStaffBranch = function (target, selected = null) {
    $(target).html('');
    $(target).append(`<option value="">-Chọn Sale Admin-</option>`);
    let codeBranch = $('#edit-branch_modal .branchCode-value').val();
    $.get(baseUrl + 'GetStaffOfBranch?codeBranch=' + codeBranch, function (res) {
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
var LoadSalesView = function (target, selected) {
    var el = $(target).find('.select-sale');
    el.html('');
    el.append(`<option value="">-Chọn sale-</option>`);
    $.get(baseUrl + 'GetSale', function (res) {
        if (res.result == 200) {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.codeStaff}">${item.fullName}</option>`)
            });

            if (selected) {
                el.val(selected);
                el.trigger('change');
            }
        }
        else {
            //toastr.error(res.errors.join('<br />'));
        }
    });
}
var LoadTeleSalesView = function (target, selected) {
    var el = $(target).find('.select-telesale');
    el.html('');
    el.append(`<option value="">-Chọn Telesale-</option>`);
    $.get(baseUrl + 'GetTeleSale', function (res) {
        if (res.result == 200) {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.codeStaff}">${item.fullName}</option>`)
            });

            if (selected) {
                el.val(selected);
                el.trigger('change');
            }
        }
        else {
            //toastr.error(res.errors.join('<br />'));
        }
    });
}
var LoadSalesRefView = function (target, selected) {
    var el = $(target).find('.select-saleref');
    el.html('');
    el.append(`<option value="">-Chọn sale-</option>`);
    $.get(baseUrl + 'GetSale', function (res) {
        if (res.result == 200) {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.codeStaff}">${item.fullName}</option>`)
            });

            if (selected) {
                el.val(selected);
                el.trigger('change');
            }
        }
        else {
            //toastr.error(res.errors.join('<br />'));
        }
    });
}
var SetupPagination = function () {
    app.component.Loading.Show();
    let stt = 1;
    $('#pagination').pagination({
        ajax: function (options, refresh, $target) {
            $.ajax({
                url: baseUrl + "GetList",
                data: {
                    page: options.current,
                    size: $('#size-page select').val(),
                    key: $('#search-key').val(),
                    status: $('.select-status').val()
                },
                method: 'POST',
                dataType: 'json'
            }).done(function (res) {
                let div = $('#table-body');
                div.html('');
                div.html(`<tr>
                                <th>
                                    <div class="custom-checkbox check_all">
                                        <input type="checkbox" />
                                        <span class="checkmark"></span>
                                    </div>
                                </th>
                                <th>STT</th>
                                <th>Tên chi nhánh</th>
                                <th>Sale Admin</th>
                                <th>Địa chỉ</th>
                                <th>Trạng thái</th>
                           </tr>
                `);

                if (res.data != null) {
                    let newData = res.data.map((item, index) => {
                        return item;
                    });

                    $.each(newData, function (index, item) {
                        div.append(`
                                <tr data-id="${item.id}">
                                    <th>
                                        <div class="custom-checkbox check_all">
                                            <input type="checkbox" name="select-row" data-id="${item.id}" />
                                            <span class="checkmark"></span>
                                        </div>
                                    </th>
                                    <td>${stt}</td>
                                    <td>
                                         <button data-id="${item.id}" class="preview-contract">
                                            ${item.branchName}
                                        </button>
                                    </td>
                                    <td>
                                      ${item.nameStaffAdminSale}
                                    </td>
                                    <td>
                                      ${item.address}
                                    </td>
                                    <td>
                                        ${item.status === 'True' ? `<div class="radiotext status"> <label>Đang hoạt động</label> </div>` : `<div class='radiotext status'>
                                            <label>Đang khóa</label>
                                        </div>`}
                                    </td>
                                </tr>`);
                        stt = stt + 1;
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
            //modal-show="show"
            //modal-data="#detail-customer-contract_modal"
        }
    });
}
var ViewPersonalInfo = function (id) {
    $.get(baseUrl + "View?id=" + id, function (res) {
        if (res.result == 400) {
            toastr.error(res.errors.join('<br />'));
        }
        else {
            $('#detail-customer-contract_modal .content-modal').html(res);
            $('#detail-customer-contract_modal').addClass('show-modal');
            $('#detail-customer-contract_modal .content-modal').addClass('show-modal');
        }
    }).done(function () {
        //let selectedBranch = $('#detail-employee_modal .hidden-branch').val();
        //if (selectedBranch) {
        //    LoadBranchesToForm('#detail-employee_modal', selectedBranch);

        //    let selectedDept = $('#detail-employee_modal .hidden-dept').val();
        //    LoadDeptsToForm('#detail-employee_modal', selectedBranch, selectedDept);

        //    let selectedTeam = $('#detail-employee_modal .hidden-team').val();
        //    LoadTeamsToForm('#detail-employee_modal', selectedDept, selectedTeam);

        //    $('#view-info').trigger('click');
        //}

        let selectedSale = $('#detail-customer-contract_modal .hidden-sale').val();
        LoadSalesView('#detail-customer-contract_modal', selectedSale);

        let selectedSaleRep = $('#detail-customer-contract_modal .hidden-saleRep').val();
        LoadSalesRefView('#detail-customer-contract_modal', selectedSaleRep);

        let selectedTeleSale = $('#detail-customer-contract_modal .hidden-telesale').val();
        LoadTeleSalesView('#detail-customer-contract_modal', selectedTeleSale);

        $('#detail-customer-contract_modal select').select2();
        $(".form_datetime").datetimepicker(
            {
                format: 'dd/mm/yyyy',
                minView: 2,
                maxView: 4,
                autoclose: true
            });

        $('#detail-customer-contract_modal input').attr('disabled', 'disabled');
        $('#detail-customer-contract_modal select').attr('disabled', 'disabled');
        /*$('#view-edit').hide();*/
    });
}
var PreviewViewPersonalInfo = function (id) {
    $.get(baseUrl + "Preview?id=" + id, function (res) {
        if (res.result == 400) {
            toastr.error(res.errors.join('<br />'));
        }
        else {
            $('#contract-preview_modal .content-modal').html(res);
            $('#contract-preview_modal').addClass('show-modal');
            $('#contract-preview_modal .content-modal').addClass('show-modal');
        }
    }).done(function () {
        //let selectedBranch = $('#detail-employee_modal .hidden-branch').val();
        //if (selectedBranch) {
        //    LoadBranchesToForm('#detail-employee_modal', selectedBranch);

        //    let selectedDept = $('#detail-employee_modal .hidden-dept').val();
        //    LoadDeptsToForm('#detail-employee_modal', selectedBranch, selectedDept);

        //    let selectedTeam = $('#detail-employee_modal .hidden-team').val();
        //    LoadTeamsToForm('#detail-employee_modal', selectedDept, selectedTeam);

        //    $('#view-info').trigger('click');
        //}

        let selectedSale = $('#contract-preview_modal .hidden-sale').val();
        LoadSalesView('#contract-preview_modal', selectedSale);

        let selectedSaleRep = $('#contract-preview_modal .hidden-saleRep').val();
        LoadSalesRefView('#contract-preview_modal', selectedSaleRep);

        let selectedTeleSale = $('#contract-preview_modal .hidden-telesale').val();
        LoadTeleSalesView('#contract-preview_modal', selectedTeleSale);

        $('#contract-preview_modal select').select2();
        $(".form_datetime").datetimepicker(
            {
                format: 'dd/mm/yyyy',
                minView: 2,
                maxView: 4,
                autoclose: true
            });

        $('#contract-preview_modal input').attr('disabled', 'disabled');
        $('#contract-preview_modal select').attr('disabled', 'disabled');
        /*$('#view-edit').hide();*/
    });
}