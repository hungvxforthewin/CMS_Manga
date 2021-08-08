let baseUrl = '/Tele/CallManagements/';
$(function () {
    /* LoadPositionsToForm('.search-box');*/ 
    //LoadProductToForm('#employee-sales-add');
    //LoadProductToForm('#employee-sales-edit');
    //search
    LoadProductToForm('#search-form');
    $('#search-form .select-product').on('change', function () {
        let selectedProduct = $(this).val();
        if (selectedProduct) {
            LoadEventToForm('#search-form', selectedProduct);
        }
    });
    LoadStatusToForm('#search-form');
    LoadLevelConcernToForm('#search-form');
    //create
    LoadProductToForm('#add-call_modal');
    $('#add-call_modal .select-product').on('change', function () {
        let selectedProduct = $(this).val();
        if (selectedProduct) {
            LoadEventToForm('#add-call_modal', selectedProduct);
        }
    });
    LoadStatusToForm('#add-call_modal');
    LoadLevelConcernToForm('#add-call_modal');
    $('#btn-show-add').on('click', function () {
        $('#btn-hiid-show').trigger('click');
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
    //edit item
    $('body').on('click', '.btn-del-remove-sale-detail', function (e) {
        $(this).closest('tr').remove();
        $('#edit-call_modal').find('input[name="billCall"]').each(function (index, item) {
            $(item).attr("placeholder", `Lần ${index + 1}`);
        });
    });
    $('body').on('click', '#btn-add-sale-detail', function (e) {
        let rows = $('#edit-call_modal').find('input[name="billCall"]');
        let temp = `

            <tr>
                    <td>
                        <input name="billCall" class="text-show"  placeholder="Lần ${rows.length + 1}" />
                    </td>
                    <td>
                        <input class="autofill_today" name="DateCallString" />
                    </td>
                    <td>
                        <input type="text" name="SupportTime" />
                    </td>
                    <td>
                        <select class="select2 select-level-detail" name="StatusFollow">
                            <option value="">--</option>
                            
                        </select>
                    </td>
                    <td>
                        <input type="text" class="note" name="Note" />
                        <img src="/Assets/SA/images/layout/delete.svg"
                             alt="delete"
                             width="16"
                             height="16"
                             class="delete-button btn-del-remove-sale-detail" />
                    </td>

                </tr>

        `;
        $('#table-detail').append(temp);
        $('#edit-call_modal select').select2();
        $(".autofill_today").datetimepicker(
            {
                format: 'dd/mm/yyyy',
                minView: 2,
                maxView: 4,
                autoclose: true
            });
        LoadLevelConcernToFormDetail('#edit-call_modal');
    });
    $('#btn-search-call').on('click', function () {
        app.component.Loading.Show();
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
                            <th></th>
                            <th>STT</th>
                            <th>Họ và tên</th>
                            <th>Điện thoại</th>
                            <th>Sản phẩm</th>
                            <th>Show up</th>
                            <th>Trạng thái</th>
                            <th>Quan tâm</th>
                            <th>Ngày cập nhật</th>
                     </tr>
                `);

                    if (res.result != 400) {
                        $.each(res.data, function (index, item) {
                            div.append(`
                        <tr>
                            <td>
                                <label class="custom-radio">
                                    <input type="radio" data-id="${item.id}" class="radio" name="call" />
                                    <span class="checkmark"></span>
                                </label>
                            </td>
                            <td>
                                ${item.stt}
                            </td>
                            <td>
                                ${item.nameInvestor}
                            </td>
                            
                            <td>${item.phoneInvestor}</td>
                            <td>${item.productName}</td>
                            <td>${item.eventName}</td>
                            <td>${item.statusName}</td>
                            <td>${item.levelconcernName}</td>
                            <td>${item.dateCreateString}</td>
                            
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

    $('body').on('click', 'button#btn-edit-call-sale', function () {
        let selectedRow = $('input[name="call"]:checked');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một cuộc để chỉnh sửa');
            $('.loading-wrapper').hide();
            return;
        }
        let id = selectedRow.data('id');
        EditCallManagementInfo(id);
    })
    $('body').on('click', 'button.delete-staff-sale', function () {
        app.component.Loading.Show();
        let id = $(this).data('id');
        $.get(baseUrl + 'IsDelete?id=' + id, function (res) {
            //console.log(res);
            if (res.status == false) {
                toastr.error(res.mess);
                app.component.Loading.Hide();
            }
            else {
                ShowModal('#employee-sales-delete');
                $('#txt-del-sale').html(`Bạn có chắc muốn xóa ?`)
                app.component.Loading.Hide();

                $('#delete-empl-sale').click(function () {
                    app.component.Loading.Show();
                    $.get(baseUrl + 'Delete?id=' + id, function (res) {
                        if (res.status) {
                            CloseModal('#employee-sales-delete');
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
    $('#del-call-sale').on('click', function () {
        $('.loading-wrapper').show();
        let selectedRow = $('input[name="call"]:checked');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một cuộc gọi để xóa');
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
                    ShowModal('#delete-call_modal');
                    //$('#txt-del-emp').html(`Bạn có chắc muốn xóa cuộc gọi ${res.name}`)
                    $('.loading-wrapper').hide();

                    $('#delete-empl-call').click(function () {
                        $('.loading-wrapper').show();
                        $.get(baseUrl + 'ConfirmDelete?id=' + id, function (res) {
                            if (res.status) {
                                SetupPagination();
                                CloseModal('#delete-call_modal');
                                toastr.success('Xóa cuộc gọi thành công !');
                                $('.loading-wrapper').hide();
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
    $('.btn-add-call').on('click', function () {
        let data = $('#add-call-sale').serializeObject();
        app.component.Loading.Show();
        $.ajax({
            method: 'POST',
            url: baseUrl + 'InsertOrUpdate',
            data: {
                model: data
            },
            success: function (rs) {
                console.log(rs);
                if (rs.status) {
                    toastr.success('Thêm mới cuộc gọi thành công!', 'Thông báo');
                    ResetValueInForm('#add-call-sale');
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
    $('body').on('click', '#btn-update-call-sale', function () {
        let data = $('#edit-call-sale').serializeObject();
        let infoCall = [];
        $('#table-detail tr').each(function () {
            infoCall.push(
                {
                    Note: $(this).find('input[name="Note"]').val(),
                    SupportTime: $(this).find('input[name="SupportTime"]').val(),
                    StatusFollow: $(this).find('select[name="StatusFollow"]').val(),
                    DateCallString: $(this).find('input[name="DateCallString"]').val()
                }
            );
        });
        data.detailCallCareHistories = infoCall;
        app.component.Loading.Show();
        $.ajax({
            method: 'POST',
            url: baseUrl + 'InsertOrUpdate',
            data: {
                model: data
            },
            success: function (rs) {
                //console.log(rs);
                if (rs.status) {
                    toastr.success('Cập nhật cuộc gọi thành công!', 'Thông báo');
                    ResetValueInForm('#edit-call-sale');
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

var EditCallManagementInfo = function (id) {
    $.get(baseUrl + "Edit?id=" + id, function (res) {
        if (res.result == 400) {
            toastr.error(res.errors.join('<br />'));
        }
        else {
            $('#edit-call_modal .content-modal').html(res);
            $('#edit-call_modal').addClass('show-modal');
            $('#edit-call_modal .content-modal').addClass('show-modal');
        }
    }).done(function () {
        let selectedProduct = $('#edit-call_modal .hidden-productCode').val();
        if (selectedProduct) {
            LoadProductToForm('#edit-call_modal', selectedProduct);

            let selectedEvent = $('#edit-call_modal .hidden-eventCode').val();
            LoadEventToForm('#edit-call_modal', selectedProduct, selectedEvent);

            let selectedLevelConcernCode = $('#edit-call_modal .hidden-levelConcernCode').val();
            LoadLevelConcernToForm('#edit-call_modal', selectedLevelConcernCode);

            let selectedStatus = $('#edit-call_modal .hidden-statusCode').val();
            LoadStatusToForm('#edit-call_modal', selectedStatus);

            //table
            $('#table-detail tr').each(function (index, item) {
                console.log(item);
                let selectedStatusFollow = $(item).find('.hidden-statusFollowCode').val();
                console.log(selectedStatusFollow)
                //let selectedStatusFollow = $(`#edit-call_modal ${item} td .hidden-statusFollowCode`).val();
                LoadLevelConcernToFormDetail($(item), selectedStatusFollow);
            });
            
        }

        $('#edit-call_modal select').select2();
        $(".autofill_today").datetimepicker(
            {
                format: 'dd/mm/yyyy',
                minView: 2,
                maxView: 4,
                autoclose: true
            });
        $('.isNumberF').keyup(delaySystem(function (e) {
            let v = $(this).val();
            v = v.replace(/[^0-9]+/g, '');
            $(this).val(numberFormartAdmin(v));
        }, 0));
        //$('#employee-sales-edit input').attr('disabled', 'disabled');
        //$('#employee-sales-edit select').attr('disabled', 'disabled');
        //$('#view-edit').hide();
    });
}

var LoadProductToForm = function (target, selected = null) {
    $.get(baseUrl + "GetProduct?", function (res) {
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            var el = $(target).find('.select-product');
            el.html('');
            el.append(`<option value="">Chọn sản phẩm</option>`);
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.productCode}">${item.name}</option>`);
            });
            if (selected) {
                $(el).val(selected);
                /*$(el).trigger('change');*/
            }
        }
    });
}

var LoadEventToForm = function (target, product, selected = null) {
    $.get(baseUrl + "GetEvent?CodeProduct=" + product, function (res) {
        var el = $(target).find('.select-event');
        el.html('');
        el.append(`<option value="">Chọn sự kiện</option>`);
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.codeEvent}">${item.name}</option>`);
            });
            if (selected) {
                $(el).val(selected);
                /*$(el).trigger('change');*/
            }
        }
    });
}


var LoadStatusToForm = function (target, selected = null) {
    $.get(baseUrl + "GetStatusData", function (res) {
        var el = $(target).find('.select-status');
        el.html('');
        el.append(`<option value="">Chọn trạng thái khai thác</option>`);
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.key}">${item.value}</option>`);
            });
            if (selected) {
                $(el).val(selected);
                /*$(el).trigger('change');*/
            }
        }
    });
}

var LoadLevelConcernToForm = function (target, selected = null) {
    $.get(baseUrl + "GetLevelOfConcern", function (res) {
        var el = $(target).find('.select-level');
        el.html('');
        el.append(`<option value="">Chọn mức độ quan tâm</option>`);
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.levelConcernCode}">${item.nameConcern}</option>`);
            });
            if (selected) {
                $(el).val(selected);
                //$(el).trigger('change');
            }
        }
    });
}
var LoadLevelConcernToFormDetail = function (target, selected = null) {
    $.get(baseUrl + "GetLevelOfConcern", function (res) {
        var el = $(target).find('.select-level-detail');
        el.html('');
        el.append(`<option value="">Chọn mức độ quan tâm</option>`);
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.levelConcernCode}">${item.nameConcern}</option>`);
            });
            if (selected) {
                $(el).val(selected);
                //$(el).trigger('change');
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
                    Key: $('#search-key').val(),
                    DateRevenue: $('#search-sale input[name="DateRevenue"]').val()
                },
                method: 'POST',
                dataType: 'json'
            }).done(function (res) {
                //console.log(res);
                let div = $('#table-body');
                div.html('');
                div.html(`
                    <tr>
                            <th></th>
                            <th>STT</th>
                            <th>Họ và tên</th>
                            <th>Điện thoại</th>
                            <th>Sản phẩm</th>
                            <th>Show up</th>
                            <th>Trạng thái</th>
                            <th>Quan tâm</th>
                            <th>Ngày cập nhật</th>
                     </tr>
                `);

                if (res.result != 400) {
                    $.each(res.data, function (index, item) {
                        div.append(`
                         <tr>
                            <td>
                                <label class="custom-radio">
                                    <input type="radio" data-id="${item.id}" class="radio" name="call" />
                                    <span class="checkmark"></span>
                                </label>
                            </td>
                            <td>
                                ${item.stt}
                            </td>
                            <td>
                                ${item.nameInvestor}
                            </td>

                            <td>${item.phoneInvestor}</td>
                            <td>${item.productName}</td>
                            <td>${item.eventName}</td>
                            <td>${item.statusName}</td>
                            <td>${item.levelconcernName}</td>
                            <td>${item.dateCreateString}</td>

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