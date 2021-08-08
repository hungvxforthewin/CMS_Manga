let baseUrl = '/Admin/Seniority/';
$(function () {
    // search
    $('#search-seniority').on('click', function () {
        $('.loading-wrapper').show();
        SetupPagination();
    });

    $('.module-content .search input').keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            $('#search-seniority').trigger('click');
        }
    });

    // event for button add
    $('#Add-Bonus').on('click', function () {
        $('#admin_seniority-manage_add-modal input').val('');
        $('#admin_seniority-manage_add-modal select').val('1 Năm');
        $('#admin_seniority-manage_add-modal select').change();
    });

    $('#admin_seniority-manage_add-modal #create-seniority').click(function () {
        $('.loading-wrapper').show();
        var data = $('#create-form').serializeArray();
        data[data.length - 1].value = data[data.length - 1].value.replaceAll(',', '');
        $.post(baseUrl + 'Create', data, function (res) {
            if (res.result == 200) {
                $('#search-seniority').trigger('click');
                CloseModal('#admin_seniority-manage_add-modal');
                toastr.success(res.message);
            }
            else {
                toastr.error(res.errors.join('<br />'));
                $('.loading-wrapper').hide();
            }
        });
    });

    //event for button update
    $('#Update-Bonus').on('click', function () {
        $('.loading-wrapper').show();
        let selectedRow = $('input[name="select-row"]:checked');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một mức thưởng thâm niên để cập nhật');
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
                    $('#admin_seniority-manage_edit-modal .content-modal').html(res);
                    $('#admin_seniority-manage_edit-modal select').select2();
                    ShowModal('#admin_seniority-manage_edit-modal');
                    $('.loading-wrapper').hide();

                    $('#admin_seniority-manage_edit-modal #update-seniority').click(function () {
                        $('.loading-wrapper').show();
                        var data = $('#update-form').serializeArray();
                        data[data.length - 1].value = data[data.length - 1].value.replaceAll(',', '');
                        $.post(baseUrl + 'Update', data, function (res) {
                            if (res.result == 200) {
                                $('#search-seniority').trigger('click');
                                CloseModal('#admin_seniority-manage_edit-modal');
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
    $('#Delete-Bonus').on('click', function () {
        $('.loading-wrapper').show();
        let selectedRow = $('input[name="select-row"]:checked');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một mức thưởng thâm niên để xóa');
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
                    $('#admin_seniority-manage_delete-modal .content-modal').html(res);
                    $('#admin_seniority-manage_delete-modal select').select2();
                    ShowModal('#admin_seniority-manage_delete-modal');
                    $('.loading-wrapper').hide();

                    $('#admin_seniority-manage_delete-modal #delete-seniority').click(function () {
                        $('.loading-wrapper').show();
                        //var data = $('#update-form').serializeArray();
                        //let id = $('#admin_seniority-manage_delete-modal #hidden-id').val();
                        $.get(baseUrl + 'ConfirmDelete?id=' + id, function (res) {
                            if (res.result == 200) {
                                $('#search-seniority').trigger('click');
                                CloseModal('#admin_seniority-manage_delete-modal');
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

    $('.loading-wrapper').show();
    $('#admin_seniority-manage_add-modal select').select2();

    // first loading
    SetupPagination();
});

// setup pagination
var SetupPagination = function () {
    $('#pagination').pagination({
        ajax: function (options, refresh, $target) {
            $.ajax({
                url: baseUrl + "GetList",
                data: {
                    start: options.current,
                    size: $('#size-page select').val(),
                    key: $('#search-key').val(),
                },
                method: 'post',
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
                        <th>ID</th>
                        <th>Tên mức thâm niên</th>
                        <th>Thời gian làm việc</th>
                        <th>Số tiền</th>
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
                                    <td>${item.allowanceCode}</td>
                                    <td>${item.allowanceName}</td>
                                    <td>${item.note}</td>
                                    <td>${commaSeparateNumber(item.allowanceAmount)}</td>
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