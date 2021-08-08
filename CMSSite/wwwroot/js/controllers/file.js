let baseUrl = '/Accountant/UploadFile/';
$(function () {
    // search
    $('#search-file').on('click', function () {
        $('.loading-wrapper').show();
        SetupPagination();
    });

    $('.module-content .search input').keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            $('#search-file').trigger('click');
        }
    });

    // event for button add
    $('#Add-File').on('click', function () {
        $('#admin_file-manage_add-modal input').val('');
    });

    $('#admin_file-manage_add-modal #create-file').click(function () {
        $('.loading-wrapper').show();
        var data = new FormData($('#create-form')[0]);
        $.ajax({
            url: baseUrl + 'Create',
            type: 'POST',
            data: data,
            contentType: false,
            processData: false
        }).done(function (res) {
            if (res.result == 200) {
                $('#search-file').trigger('click');
                CloseModal('#admin_file-manage_add-modal');
                toastr.success(res.message);
            }
            else {
                toastr.error(res.errors.join('<br />'));
                $('.loading-wrapper').hide();
            }
        });
    });

    //event for button update
    $('#Update-File').on('click', function () {
        $('.loading-wrapper').show();
        let selectedRow = $('input[name="select-row"]:checked');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một file hệ thống để cập nhật');
            $('.loading-wrapper').hide();
            return;
        }
        let id = selectedRow.data('id');
        if (id) {
            $.get(baseUrl + 'Update?id=' + id, function (res) {
                //console.log(res);
                if (res.result == 400) {
                    toastr.error(res.errors.join('<br />'));
                    $('.loading-wrapper').hide();
                }
                else {
                    $('#admin_file-manage_edit-modal .content-modal').html(res);
                    ShowModal('#admin_file-manage_edit-modal');
                    $('.loading-wrapper').hide();

                    $('#admin_file-manage_edit-modal #update-file').click(function () {
                        $('.loading-wrapper').show();
                        var data = new FormData($('#update-form')[0]);
                        $.ajax({
                            url: baseUrl + 'Update',
                            type: 'POST',
                            data: data,
                            contentType: false,
                            processData: false
                        }).done(function (res) {
                            if (res.result == 200) {
                                $('#search-file').trigger('click');
                                CloseModal('#admin_file-manage_edit-modal');
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

    //event for button delete
    $('#Delete-File').on('click', function () {
        $('.loading-wrapper').show();
        let selectedRow = $('input[name="select-row"]:checked');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một file hệ thống để xóa');
            $('.loading-wrapper').hide();
            return;
        }
        let id = selectedRow.data('id');
        if (id) {
            $.get(baseUrl + 'Delete?id=' + id, function (res) {
                //console.log(res);
                if (res.result == 400) {
                    toastr.error(res.errors.join('<br />'));
                    $('.loading-wrapper').hide();
                }
                else {
                    $('#admin_file-manage_delete-modal .content-modal').html(res);
                    ShowModal('#admin_file-manage_delete-modal');
                    $('.loading-wrapper').hide();

                    $('#admin_file-manage_delete-modal #delete-file').click(function () {
                        $('.loading-wrapper').show();
                        //var data = $('#update-form').serializeArray();
                        //let id = $('#admin_file-manage_delete-modal #hidden-id').val();
                        $.get(baseUrl + 'ConfirmDelete?id=' + id, function (res) {
                            if (res.result == 200) {
                                $('#search-file').trigger('click');
                                CloseModal('#admin_file-manage_delete-modal');
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

    // update table data by size page
    $('#size-page select').on('change', function () {
        $('.loading-wrapper').show();
        SetupPagination();
    });

    //$('body').on('click', '.view-info', function () {
    //    console.log('view');
    //});

    $('.loading-wrapper').show();

    // first loading
    SetupPagination();
});

var ViewFile = function (id) {
    $('.loading-wrapper').show();
    $.get(baseUrl + 'View?id=' + id, function (res) {
        //console.log(res);
        if (res.result == 400) {
            toastr.error(res.errors.join('<br />'));
            $('.loading-wrapper').hide();
        }
        else {
            $('#admin_file-manage_view-modal .content-modal').html(res);
            ShowModal('#admin_file-manage_view-modal');
            $('.loading-wrapper').hide();
        }
    });
}

// setup pagination
var SetupPagination = function () {
    $('#pagination').pagination({
        ajax: function (options, refresh, $target) {
            $.ajax({
                url: baseUrl + "GetList",
                data: {
                    page: options.current,
                    size: $('#size-page select').val(),
                    key: $('#search-key').val(),
                },
                method: 'get',
                dataType: 'json'
            }).done(function (res) {
                //console.log(res);
                let div = $('#table-body');
                div.html(`
                    <tr>
                        <th>
                            <div class="custom-checkbox">
                                <input type="checkbox" />
                                <span class="checkmark"></span>
                            </div>
                        </th>
                        <th>STT</th>
                        <th>File</th>
                        <th style="max-width: 300px;">Mô tả</th>
                        <th>Ngày tải lên</th>
                        <th>Người tải lên</th>
                        <th>Ngày cập nhật</th>
                        <th>Người cập nhật</th>
                    </tr>
                `);
                if (res.result != 400) {
                    $.each(res.data, function (index, item) {
                        div.append(`
                                <tr>
                                    <td>
                                        <div class="custom-checkbox">
                                            <input type="checkbox" name="select-row" data-id="${item.id}" />
                                            <span class="checkmark"></span>
                                        </div>
                                    </td>
                                    <td>${($('#size-page select').val() * (options.current - 1)) + index + 1}</td>
                                    <td><button type="button" onclick="ViewFile('${item.id}')" class="view-info">${item.linksUpload} </button></td>
                                    <td>${item.reason}</td>
                                    <td>${item.createDateString}</td>
                                    <td>${item.userUpload}</td>
                                    <td>${item.updateDateString ?? ''}</td>
                                    <td>${item.userUpdate ?? ''}</td>
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