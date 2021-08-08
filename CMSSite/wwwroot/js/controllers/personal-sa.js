let baseUrl = '/SaleAdmin/PersonalInfo/';
$(function () {
    (function ($) {
        $.fn.serializeObject = function (options) {
            options = jQuery.extend(true, {
                validateName: true
            }, options);
            var self = this,
                json = {},
                push_counters = {},
                patterns = {
                    "validate": /^[a-zA-Z][a-zA-Z0-9_]*(?:\[(?:\d*|[a-zA-Z0-9_]+)\])*$/,
                    "key": /[a-zA-Z0-9_]+|(?=\[\])/g,
                    "push": /^$/,
                    "fixed": /^\d+$/,
                    "named": /^[a-zA-Z0-9_]+$/
                };
            this.build = function (base, key, value) {
                base[key] = value;
                return base;
            };
            this.push_counter = function (key) {
                if (push_counters[key] === undefined) {
                    push_counters[key] = 0;
                }
                return push_counters[key]++;
            };
            $.each($(this).serializeArray(), function () {

                // skip invalid keys
                if (!patterns.validate.test(this.name) && options.validateName) {
                    return;
                }
                var k,
                    keys = this.name.match(patterns.key),
                    merge = this.value,
                    reverse_key = this.name;
                while ((k = keys.pop()) !== undefined) {
                    // adjust reverse_key
                    reverse_key = reverse_key.replace(new RegExp("\\[" + k + "\\]$"), '');
                    // push
                    if (k.match(patterns.push)) {
                        merge = self.build([], self.push_counter(reverse_key), merge);
                    }
                    // fixed
                    else if (k.match(patterns.fixed)) {
                        merge = self.build([], k, merge);
                    }
                    // named
                    else if (k.match(patterns.named)) {
                        merge = self.build({}, k, merge);
                    }
                }
                json = $.extend(true, json, merge);
            });
            return json;
        };
    })(jQuery);
    /* LoadPositionsToForm('.search-box');*/
    LoadBranchesToForm('.search-box');
    LoadBranchesToForm('#add-employee_modal');
    LoadBranchesToForm('#detail-employee_modal');
    //view add
    $('#add-employee_modal .select-branch').on('change', function () {
        $('#add-employee_modal .select-dept').html(`<option value="">Phòng</option>`);
        $('#add-employee_modal .select-team').html(`<option value="">Nhóm</option>`);

        let selectedBranch = $(this).val();
        if (selectedBranch) {
            LoadOfficesToForm('#add-employee_modal', selectedBranch);
        }
    });
    $('#add-employee_modal .select-office').on('change', function () {
        $('#add-employee_modal .select-team').html(`<option value="">Nhóm</option>`);

        let selectedOffice = $(this).val();
        if (selectedOffice) {
            LoadDeptsToForm('#add-employee_modal', selectedOffice);
        }
    });

    $('#add-employee_modal .select-dept').on('change', function () {
        let selectedDept = $(this).val();
        if (selectedDept) {
            LoadTeamsToForm('#add-employee_modal', selectedDept);
        }
    });
    //view edit
    $('body').on('change', '#detail-employee_modal .select-branch', function () {
        $('#detail-employee_modal .select-dept').html(`<option value="">Phòng</option>`);
        $('#detail-employee_modal .select-team').html(`<option value="">Nhóm</option>`);

        let selectedBranch = $(this).val();
        if (selectedBranch) {
            LoadOfficesToForm('#detail-employee_modal', selectedBranch);
        }
    });
    $('body').on('change', '#detail-employee_modal .select-office', function () {
        $('#detail-employee_modal .select-team').html(`<option value="">Nhóm</option>`);

        let selectedOffice = $(this).val();
        if (selectedOffice) {
            LoadDeptsToForm('#detail-employee_modal', selectedOffice);
        }
    });

    $('body').on('change', '#detail-employee_modal .select-dept', function () {
        let selectedDept = $(this).val();
        if (selectedDept) {
            LoadTeamsToForm('#detail-employee_modal', selectedDept);
        }
    });
    //view index
    $('.search-box .select-branch').on('change', function () {
        $('.search-box .select-dept').html(`<option value="">Phòng</option>`);
        $('.search-box .select-team').html(`<option value="">Nhóm</option>`);

        let selectedBranch = $(this).val();
        if (selectedBranch) {
            LoadOfficesToForm('.search-box', selectedBranch);
        }
    });
    $('.search-box .select-office').on('change', function () {
        $('.search-box .select-team').html(`<option value="">Nhóm</option>`);

        let selectedOffice = $(this).val();
        if (selectedOffice) {
            LoadDeptsToForm('.search-box', selectedOffice);
        }
    });
    $('.search-box .select-dept').on('change', function () {
        let selectedDept = $(this).val();
        if (selectedDept) {
            LoadTeamsToForm('.search-box', selectedDept);
        }
    });

    $('#search-personal').on('click', function () {
        $('.loading-wrapper').show();
        let data = $('#search-form').serializeObject();
        //console.log(data);
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
                        <th>ID</th>
                        <th>Tên tài khoản</th>
                        <th>Họ và tên</th>
                        <th>Số điện thoại</th>
                        <th>Chức vụ</th>
                        <th>Team</th>
                        <th>Phòng</th>
                        <th>Trạng thái</th>
                    </tr>
                `);

                    if (res.result != 400) {
                        $('.select_pagination').show();
                        $.each(res.data, function (index, item) {
                            div.append(`
                        <tr>
                            <td>
                                <div class="custom-checkbox check_all">
                                    <input type="checkbox" name="select-row" data-id="${item.id}"/>
                                    <span class="checkmark"></span>
                                </div>
                            </td>
                            <td>
                                <button type="button" class="view-info" data-id="${item.id}">
                                    ${item.codeStaff}
                                </button>
                            </td>
                            <td>${item.userName}</td>
                            <td>${item.fullName}</td>
                            <td>${item.phone}</td>
                            <td>${item.positionName}</td>
                            <td>${item.teamName}</td>
                            <td>${item.departmentName}</td>
                            <td>
                                <span class="status ${(item.status == 1) ? "working" : "quit"}">${(item.status == 1) ? "Đang làm việc" : "Đã thôi việc"}</span>
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
                        $('#table-body').html(`<span>Không có dữ liệu</span>`);
                        $('.select_pagination').hide();
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
        //$.post(baseUrl + "GetList", data, function (res) {
        //    if (res.result == 400) {
        //        $('div.table-data').html('');
        //        $('#table-body').html(`<span>Không có dữ liệu</span>`);
        //        $('.select_pagination').hide();
        //        toastr.error(res.errors.join('<br />'));
        //        $('.loading-wrapper').hide();
        //    }
        //    else {
        //        $('#table-body').html('');
        //        $('div.table-data').html(res);
        //        $('.loading-wrapper').hide();
        //        $('.select_pagination').show();

        //    }
        //});
    });

    $('body').on('click', 'button.view-info', function () {
        ViewPersonalInfo($(this).data('id'));
    })
    $('#Edit-Employ').on('click', function () {
        $('.loading-wrapper').show();
        $('#view-edit').show();

        let selectedRow = $('input[name="select-row"]:checked');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một nhân viên tâm để chỉnh sửa');
            $('.loading-wrapper').hide();
            return;
        }
        let id = selectedRow.data('id');
        //console.log(id);
        if (id) {
            /*ShowModal('#edit-employee_modal');*/
            $.get(baseUrl + "View?id=" + id, function (res) {
                if (res.result == 400) {
                    toastr.error(res.errors.join('<br />'));
                }
                else {
                    //console.log(res);
                    $('#detail-employee_modal .content-modal').html(res);
                    $('#detail-employee_modal').addClass('show-modal');
                    $('#detail-employee_modal .content-modal').addClass('show-modal');
                }
            }).done(function () {
                $('.loading-wrapper').hide();
                let selectedBranch = $('#detail-employee_modal .hidden-branch').val();
                if (selectedBranch) {
                    LoadBranchesToForm('#detail-employee_modal', selectedBranch);

                    let selectedOffice = $('#detail-employee_modal .hidden-office').val();
                    LoadOfficesToForm('#detail-employee_modal', selectedBranch, selectedOffice);

                    let selectedDept = $('#detail-employee_modal .hidden-dept').val();
                    LoadDeptsToForm('#detail-employee_modal', selectedOffice, selectedDept);

                    let selectedTeam = $('#detail-employee_modal .hidden-team').val();
                    LoadTeamsToForm('#detail-employee_modal', selectedDept, selectedTeam);

                    $('#view-info').trigger('click');
                }

                let selectedRole = $('#detail-employee_modal .hidden-role').val();
                LoadRolesToForm('#detail-employee_modal', selectedRole);

                let selectedInsurance = $('#detail-employee_modal .hidden-insurance').val();
                LoadSocialInsuranceToForm('#detail-employee_modal', selectedInsurance);

                let selectedBonus = $('#detail-employee_modal .hidden-bonus').val();
                LoadSeniorBonusToForm('#detail-employee_modal', selectedBonus);

                $('#detail-employee_modal select').select2();
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
    $('#Del-Employ').on('click', function () {
        $('.loading-wrapper').show();
        let selectedRow = $('input[name="select-row"]:checked');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một nhân viên tâm để xóa');
            $('.loading-wrapper').hide();
            return;
        }
        let id = selectedRow.data('id');
        //console.log(id);

        if (id) {
            $.get(baseUrl + 'Delete?id=' + id, function (res) {
                //console.log(res);
                if (res.status == false) {
                    toastr.error(res.mess);
                    $('.loading-wrapper').hide();
                }
                else {
                    ShowModal('#delete-employee_modal');
                    $('#txt-del-emp').html(`Bạn có chắc muốn xóa nhân sự <span>${res.name}</span> không?`);
                    $('.loading-wrapper').hide();

                    $('#delete-empl').click(function () {
                        $('.loading-wrapper').show();
                        $.get(baseUrl + 'ConfirmDelete?id=' + id, function (res) {
                            if (res.status == true) {
                                $('#search-personal').trigger('click');
                                CloseModal('#delete-employee_modal');
                                toastr.success(res.message);
                            }
                            else {
                                toastr.error(res.message);
                                $('.loading-wrapper').hide();
                            }
                        });
                    });
                }
            });
        }
    });
    $('body').on('change', '#myFile', function () {
        var files = $('#myFile').prop("files");
        formData = new FormData();
        formData.append("file", files[0]);
        $.ajax({
            method: 'POST',
            url: baseUrl + 'upFile',
            data: formData,
            processData: false,  // tell jQuery not to process the data
            contentType: false,  // tell jQuery not to set contentType
            //headers: {
            //    "Content-Type": "application/x-www-form-urlencoded"
            //},
            success: function (rs) {
                //console.log(rs);
                if (rs.status) {
                    $('#myFileName').val(rs.data);
                    $('.loading-wrapper').hide();

                } else {

                    $('.loading-wrapper').hide();
                }
            }
        })
    })
    $('body').on('change', '#myFileEdit', function () {
        var files = $('#myFileEdit').prop("files");
        formData = new FormData();
        formData.append("file", files[0]);
        $.ajax({
            method: 'POST',
            url: baseUrl + 'upFile',
            data: formData,
            processData: false,  // tell jQuery not to process the data
            contentType: false,  // tell jQuery not to set contentType
            //headers: {
            //    "Content-Type": "application/x-www-form-urlencoded"
            //},
            success: function (rs) {
                //console.log(rs);
                if (rs.status) {
                    $('#myFileNameEdit').val(rs.data);
                    $('.loading-wrapper').hide();

                } else {

                    $('.loading-wrapper').hide();
                }
            }
        })
    })
    $('body').on('change', '#myFile', function () {
        if (typeof (FileReader) != "undefined") {
            var dvPreview = $("#divImageMediaPreview");
            dvPreview.html("");
            $($(this)[0].files).each(function () {
                var file = $(this);
                var reader = new FileReader();
                reader.onload = function (e) {
                    var img = $("<img />");
                    img.attr("style", "width: 150px; height:100px; padding: 10px");
                    img.attr("src", e.target.result);
                    dvPreview.append(img);
                }
                reader.readAsDataURL(file[0]);
            });
        } else {
            alert("This browser does not support HTML5 FileReader.");
        }
    });
    $('body').on('change', '#myFileEdit', function () {
        if (typeof (FileReader) != "undefined") {
            var dvPreview = $("#divImageMediaPreviewEdit");
            dvPreview.html("");
            $($(this)[0].files).each(function () {
                var file = $(this);
                var reader = new FileReader();
                reader.onload = function (e) {
                    var img = $("<img />");
                    img.attr("style", "width: 150px; height:100px; padding: 10px");
                    img.attr("src", e.target.result);
                    dvPreview.append(img);
                }
                reader.readAsDataURL(file[0]);
            });
        } else {
            alert("This browser does not support HTML5 FileReader.");
        }
    });
    $('.btn-add-person').on('click', function () {
        let data = $('#form-model-add').serializeObject();
        $('.loading-wrapper').show();
        $.ajax({
            method: 'POST',
            url: baseUrl + 'InsertOrUpdate',
            data: {
                model: data
            },
            success: function (rs) {
                //console.log(rs);
                if (rs.success) {
                    toastr.success('Thêm mới nhân sự thành công!', 'Thông báo');
                    ResetValueInForm('#form-model-add');
                    $('.close__btn_fix').trigger('click');
                    $('#add-employee_modal').modal('hide');
                    SetupPagination();
                    $('.loading-wrapper').hide();
                } else {
                    rs.lst.map(function (item) {
                        toastr.error(`${item.field}: ${item.errorMessage}`, 'Thông báo');
                    });
                    $('.loading-wrapper').hide();
                }
            }
        });
    });
    $('body').on('click', '#btn-update-person', function () {
        let data = $('#form-model-edit').serializeObject();
        $('.loading-wrapper').show();
        $.ajax({
            method: 'POST',
            url: baseUrl + 'InsertOrUpdate',
            data: {
                model: data
            },
            success: function (rs) {
                //console.log(rs);
                if (rs.success) {
                    toastr.success('Cập nhật nhân sự thành công!', 'Thông báo');
                    ResetValueInForm('#form-model-edit');
                    $('#detail-employee_modal').modal('hide');
                    SetupPagination();
                    $('.loading-wrapper').hide();
                    $('.close__btn_update').trigger('click');
                } else {
                    rs.lst.map(function (item) {
                        toastr.error(`${item.field}: ${item.errorMessage}`, 'Thông báo');
                    });
                    $('.loading-wrapper').hide();
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

var ViewPersonalInfo = function (id) {
    $.get(baseUrl + "View?id=" + id, function (res) {
        if (res.result == 400) {
            toastr.error(res.errors.join('<br />'));
        }
        else {
            $('#detail-employee_modal .content-modal').html(res);
            $('#detail-employee_modal').addClass('show-modal');
            $('#detail-employee_modal .content-modal').addClass('show-modal');
        }
    }).done(function () {
        let selectedBranch = $('#detail-employee_modal .hidden-branch').val();
        if (selectedBranch) {
            LoadBranchesToForm('#detail-employee_modal', selectedBranch);

            let selectedOffice = $('#detail-employee_modal .hidden-office').val();
            LoadOfficesToForm('#detail-employee_modal', selectedBranch, selectedOffice);

            let selectedDept = $('#detail-employee_modal .hidden-dept').val();
            LoadDeptsToForm('#detail-employee_modal', selectedOffice, selectedDept);

            let selectedTeam = $('#detail-employee_modal .hidden-team').val();
            LoadTeamsToForm('#detail-employee_modal', selectedDept, selectedTeam);

            $('#view-info').trigger('click');
        }

        let selectedRole = $('#detail-employee_modal .hidden-role').val();
        LoadRolesToForm('#detail-employee_modal', selectedRole);

        let selectedInsurance = $('#detail-employee_modal .hidden-insurance').val();
        LoadSocialInsuranceToForm('#detail-employee_modal', selectedInsurance);

        let selectedBonus = $('#detail-employee_modal .hidden-bonus').val();
        LoadSeniorBonusToForm('#detail-employee_modal', selectedBonus);

        $('#detail-employee_modal select').select2();
        $(".form_datetime").datetimepicker(
            {
                format: 'dd/mm/yyyy',
                minView: 2,
                maxView: 4,
                autoclose: true
            });

        $('#detail-employee_modal input').attr('disabled', 'disabled');
        $('#detail-employee_modal select').attr('disabled', 'disabled');
        $('#view-edit').hide();
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


var LoadDeptsToForm = function (target, office, selected = null) {
    $.get(baseUrl + "GetDepartments?office=" + office, function (res) {
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

var LoadPositionsToForm = function (target, selected = null) {
    $.get(baseUrl + "GetPositions", function (res) {
        var el = $(target).find('.select-position');
        el.html('');
        el.append(`<option value="">Chọn chức vụ</option>`);
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.positionCode}">${item.positionName}</option>`);
            });
            if (selected) {
                $(el).val(selected);
                $(el).trigger('change');
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

var LoadSeniorBonusToForm = function (target, selected = null) {
    $.get(baseUrl + "GetAllowanceOrDeductByType?type=1", function (res) {
        var el = $(target).find('.select-bonus');
        el.html('');
        el.append(`<option value="">Chọn thâm niên</option>`);
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

var GenerateTable = function (target, data) {
    $(target).html('');
    $.each(data.timeKeepings, function (index, item) {
        $(target).append(
            `<tr>
                        <td>
                            <input type="radio" name="rb-check" data-id="${item.id}" />
                        </td>
                        <td>${item.codeStaff}</td>
                        <td>${item.userName}</td>
                        <td>${item.fullName}</td>
                        <td${item.phone}</td>
                        <td>${item.positionName}</td>
                        <td>${item.teamName}</td>
                        <td>${item.departmentName}</td>
                        <td>${item.status == '1' ? 'Đang làm việc' : 'Đã thôi việc'}</td>
                    </tr>`
        );
    });
}

//SetupPagination = function () {
//    $('#pagination a').attr('href', '#');
//    $('#pagination .btn').on('click', function () {
//        var page = $(this).attr('data-path');
//        $.get(baseUrl + 'ClickPage?page=' + page, function (res) {
//            console.log(res);
//            $('div.table-data').html(res);
//            SetupPagination();
//        });
//    });

//    $('#pagination .page-link').on('click', function () {
//        var page = $(this).attr('data-path');

//        let data = $('#search-form').serializeArray();
//        data.push({ name: 'Start', value: page });
//        $.post(baseUrl + "GetList", data, function (res) {
//            console.log(res);
//            $('#error-list').html('');
//            if (res.result == 400) {
//                $.each(res.errors, function (index, item) {
//                    $('#error-list').append(`<p class="error-message">${item}</p>`);
//                });
//            }
//            else {
//                $('div.table-data').html(res);
//                SetupPagination();
//            }
//        });
//    });
//}

function ResetValueInForm(form) {
    //reset error;
    //showErrors(null, $(form).find(NameDivShowError));
    //showErrors2($(form), null, prefixName);
    //reset value;
    var fields = $(form).find('[name]');
    $.each(fields, function (index, field) {
        var tagname = $(field).prop("tagName");
        var name = $(field).attr("name");
        if (name !== "__RequestVerificationToken") {
            if (tagname === "INPUT") {
                //check radio
                var type = $(field).attr("type");
                if (type !== "radio") {
                    $(field).val("");
                }
            }
            else if (tagname === "TEXTAREA") {
                $(field).val("");
            } else if (tagname === "SELECT") {
                $(field).val("").trigger("change");
            }
        }

    });
}
let branchSearch = $('#search-form select.select-branch').val();
// setup pagination
var SetupPagination = function () {
    //console.log('load');
    $('.loading-wrapper').show();
    $('#pagination').pagination({
        ajax: function (options, refresh, $target) {
            $.ajax({
                url: baseUrl + "GetList",
                data: {
                    page: options.current,
                    size: $('#size-page select').val(),
                    key: $('#search-key').val(),
                    position: $('.select-position').val(),
                    branch: $('#search-form select#select-branch').val(),
                    department: $('.select-dept').val(),
                    team: $('.select-team').val(),
                    status: $('.select-status').val()
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
                        <th>ID</th>
                        <th>Tên tài khoản</th>
                        <th>Họ và tên</th>
                        <th>Số điện thoại</th>
                        <th>Chức vụ</th>
                        <th>Team</th>
                        <th>Phòng</th>
                        <th>Trạng thái</th>
                    </tr>
                `);

                if (res.result != 400) {
                    $.each(res.data, function (index, item) {
                        div.append(`
                        <tr>
                            <td style="min-width: 50px;">
                                <div class="custom-checkbox check_all">
                                    <input type="checkbox" name="select-row" data-id="${item.id}"/>
                                    <span class="checkmark"></span>
                                </div>
                            </td>
                            <td>
                                <button type="button" class="view-info" data-id="${item.id}">
                                    ${item.codeStaff}
                                </button>
                            </td>
                            <td>${item.userName}</td>
                            <td>${item.fullName}</td>
                            <td>${item.phone}</td>
                            <td>${item.positionName}</td>
                            <td>${item.teamName ?? ''}</td>
                            <td>${item.departmentName ?? ''}</td>
                            <td>
                                <span class="status ${(item.status == 1) ? "working" : "quit"}">${(item.status == 1) ? "Đang làm việc" : "Đã thôi việc"}</span>
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
                $('.loading-wrapper').hide();
            }).fail(function (error) {
                $('.loading-wrapper').hide();
            });
        }
    });
}