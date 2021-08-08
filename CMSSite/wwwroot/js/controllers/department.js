let baseUrl = '/HR/Departments/';

$(function () {
    LoadStatus('#search-depart-set .select-status');
    LoadBranchesToForm('#search-depart-set');
    LoadBranchesToForm('#add-depart_modal');
    //add
    $('#add-depart_modal .select-branch').on('change', function () {
        $('#add-depart_modal .select-dept').html(`<option value="">Phòng</option>`);

        let selectedBranch = $(this).val();
        if (selectedBranch) {
            LoadOfficesToForm('#add-depart_modal', selectedBranch);
        }
    });
    //$('#add-depart_modal .select-office').on('change', function () {
    //    $('#add-depart_modal .select-team').html(`<option value="">Nhóm</option>`);

    //    let selectedOffice = $(this).val();
    //    if (selectedOffice) {
    //        LoadDeptsToForm('#add-depart_modal', selectedOffice);
    //    }
    //});
    //view-edit
    $('body').on('change', '#edit-depart_modal .select-branch', function () {
        $('#edit-depart_modal .select-dept').html(`<option value="">Phòng</option>`);

        let selectedBranch = $(this).val();
        if (selectedBranch) {
            LoadOfficesToForm('#edit-depart_modal', selectedBranch);
        }
    });
    //$('body').on('change', '#edit-depart_modal .select-office', function () {
    //    let selectedOffice = $(this).val();
    //    if (selectedOffice) {
    //        LoadDeptsToForm('#edit-depart_modal', selectedOffice);
    //    }
    //});
    //index
    $('#search-depart-set .select-branch').on('change', function () {
        $('#search-depart-set .select-dept').html(`<option value="">Phòng</option>`);

        let selectedBranch = $(this).val();
        if (selectedBranch) {
            LoadOfficesToForm('#search-depart-set', selectedBranch);
        }
    });
    $('#search-depart-set .select-office').on('change', function () {

        let selectedOffice = $(this).val();
        if (selectedOffice) {
            LoadDeptsToForm('#search-depart-set', selectedOffice);
        }
    });
    $('body').on('click', '#btn-show-add', function () {
        $('#btn-hiid-show').trigger('click');
        $('#add-depart_modal select').select2();
    });
    //Insert
    $('body').on('click', '.btn-add-depart', function () {
        let model = $('#form-model-add').serializeObject();
        if (model.DepartmentName === null || model.DepartmentName === '') {
            toastr.error('Nhập tên phòng ban', 'Thông báo');
            return false;
        }
        if (model.BranchCode === null || model.BranchCode === '') {
            toastr.error('Chọn chi nhánh', 'Thông báo');
            return false;
        }
        if (model.OfficeCode === null || model.OfficeCode === '') {
            toastr.error('Chọn khối', 'Thông báo');
            return false;
        }
        let data = model;
        _AjaxPostForm(baseUrl + "InsertOrUpdate", data, function (res) {
            if (res.status) {
                toastr.success('Tạo phòng ban thành công', 'Thông báo');
                $('.close__modal').trigger('click');
                SetupPagination();
            } else {
                toastr.error(res.mess, 'Thông báo');
            }
        });
    });
    $('body').on('click', '.btn-update-depart', function () {
        let model = $('#form-model-edit').serializeObject();
        if (model.DepartmentName === null || model.DepartmentName === '') {
            toastr.error('Nhập tên phòng ban', 'Thông báo');
            return false;
        }
        if (model.BranchCode === null || model.BranchCode === '') {
            toastr.error('Chọn chi nhánh', 'Thông báo');
            return false;
        }
        if (model.OfficeCode === null || model.OfficeCode === '') {
            toastr.error('Chọn khối', 'Thông báo');
            return false;
        }
        let data = model;
        _AjaxPostForm(baseUrl + "InsertOrUpdate", data, function (res) {
            if (res.status) {
                toastr.success('Cập nhật phòng ban thành công', 'Thông báo');
                $('.close__modal').trigger('click');
                SetupPagination();
            } else {
                toastr.error(res.mess, 'Thông báo');
            }
        });
    });
    //View 
    $('body').on('click', 'button.view-info', function () {
        ViewPersonalInfo($(this).data('id'));
    })
    //Edit 
    $('body').on('click', '#Edit-Depart', function () {
        let selectedRow = $('input[name="select-row"]:checked');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một phòng ban để chỉnh sửa');
            return;
        }
        let id = selectedRow.data('id');
        if (id) {
            $.get(baseUrl + "Edit?id=" + id, function (res) {
                if (res.result == 400) {
                    toastr.error(res.errors.join('<br />'));
                }
                else {
                    $('#edit-depart_modal .content-modal').html(res);
                    $('#edit-depart_modal').addClass('show-modal');
                    $('#edit-depart_modal .content-modal').addClass('show-modal');
                }
            }).done(function () {
                $('.loading-wrapper').hide();
                let selectedBranch = $('#edit-depart_modal .hidden-branch').val();
                if (selectedBranch) {
                    LoadBranchesToForm('#edit-depart_modal', selectedBranch);
                    let selectedOffice = $('#edit-depart_modal .hidden-office').val();
                    LoadOfficesToForm('#edit-depart_modal', selectedBranch, selectedOffice);
                    $('#view-info').trigger('click');
                }
                let selectedDepart = $('#edit-depart_modal .hidden-staff').val();
                LoadStaffDepart('#edit-depart_modal .select-staff', selectedDepart);
                $('#edit-depart_modal select').select2();
                $(".form_datetime").datetimepicker(
                    {
                        format: 'dd/mm/yyyy',
                        minView: 2,
                        maxView: 4,
                        autoclose: true
                    });
            });
        }
    })
    //List, Search, Pageing
    $('#search-depart').on('click', function (e) {
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
                                <th>Tên phòng ban</th>
                                <th>Capital Manager</th>
                                <th>Tên chi nhánh</th>
                                <th>Tên khối</th>
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
                                            ${item.departmentName}
                                        </button>
                                    </td>
                                    <td>
                                      ${item.nameStaffSaleManage}
                                    </td>
                                    <td>
                                      ${item.branchName}
                                    </td>
                                   
                                    <td>
                                      ${item.officeName}
                                    </td>
                                    <td>
                                        ${item.status === true ? `<div class="radiotext status"> <label>Đang hoạt động</label> </div>` : `<div class='radiotext status'>
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
                //console.log(item);
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
var LoadStaffDepart = function (target, selected = null) {
    $(target).html('');
    $(target).append(`<option value="">-Chọn trưởng phòng-</option>`);
    let codeDepart = $('#edit-depart_modal .departmentCode-value').val();
    $.get(baseUrl + 'GetStaffOfDepart?codeDepart=' + codeDepart, function (res) {
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
//View Detail
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
                    status: $('.select-status').val(),
                    branchCode: $('.select-branch').val(),
                    officeCode: $('.select-office').val(),
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
                                <th>Tên phòng ban</th>
                                <th>Capital Manager</th>
                                <th>Tên chi nhánh</th>
                                <th>Tên khối</th>
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
                                            ${item.departmentName}
                                        </button>
                                    </td>
                                    <td>
                                      ${item.nameStaffSaleManage}
                                    </td>
                                    <td>
                                      ${item.branchName}
                                    </td>
                                    <td>
                                      ${item.officeName}
                                    </td>
                                    <td>
                                        ${item.status === true ? `<div class="radiotext status"> <label>Đang hoạt động</label> </div>` : `<div class='radiotext status'>
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