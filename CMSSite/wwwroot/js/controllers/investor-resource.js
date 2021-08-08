let baseUrl = '/Admin/WhereToFindInvestor/';
$(function () {
    // search
    $('#search-investor-resource').on('click', function () {
        $('.loading-wrapper').show();
        SetupPagination();
    });

    $('.module-content .search input').keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
            $('#search-investor-resource').trigger('click');
        }
    });

    // event for button add
    $('#Add-Investor-Resource').on('click', function () {
        $('#admin_customer-source_add-modal input').val('');
    });

    $('#admin_customer-source_add-modal #create-investor-resource').click(function () {
        $('.loading-wrapper').show();
        var data = $('#create-form').serializeArray();
        data[data.length - 1].value = data[data.length - 1].value.replaceAll(',', '');
        $.post(baseUrl + 'Create', data, function (res) {
            if (res.result == 200) {
                $('#search-investor-resource').trigger('click');
                CloseModal('#admin_customer-source_add-modal');
                toastr.success(res.message);
            }
            else {
                toastr.error(res.errors.join('<br />'));
                $('.loading-wrapper').hide();
            }
        });
    });

    //event for button update
    $('#Edit-Investor-Resource').on('click', function () {
        $('.loading-wrapper').show();
        let selectedRow = $('input[name="select-row"]:checked');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một nguồn khách hàng để cập nhật');
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
                    $('#admin_customer-source_edit-modal .content-modal').html(res);
                    ShowModal('#admin_customer-source_edit-modal');
                    $('.loading-wrapper').hide();

                    $('#admin_customer-source_edit-modal #update-investor-resource').click(function () {
                        $('.loading-wrapper').show();
                        var data = $('#update-form').serializeArray();
                        $.post(baseUrl + 'Update', data, function (res) {
                            if (res.result == 200) {
                                $('#search-investor-resource').trigger('click');
                                CloseModal('#admin_customer-source_edit-modal');
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
    $('#Delete-Investor-Resource').on('click', function () {
        $('.loading-wrapper').show();
        let selectedRow = $('input[name="select-row"]:checked');
        if (selectedRow.length != 1) {
            toastr.error('Chọn một nguồn khách hàng để xóa');
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
                    $('#admin_customer-source_delete-modal .content-modal').html(res);
                    ShowModal('#admin_customer-source_delete-modal');
                    $('.loading-wrapper').hide();

                    $('#admin_customer-source_delete-modal #delete-investor-resource').click(function () {
                        $('.loading-wrapper').show();
                        $.get(baseUrl + 'ConfirmDelete?id=' + id, function (res) {
                            if (res.result == 200) {
                                $('#search-investor-resource').trigger('click');
                                CloseModal('#admin_customer-source_delete-modal');
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

    // first loading
    $('.loading-wrapper').show();
    SetupPagination();
});

// setup pagination
var SetupPagination = function () {
    //console.log('load');
    $('#pagination').pagination({
        ajax: function (options, refresh, $target) {
            $.ajax({
                url: baseUrl + "GetList",
                data: {
                    page: options.current,
                    size: $('#size-page select').val(),
                    key: $('#search-key').val(),
                },
                method: 'GET',
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
                        <th>Nguồn khách hàng</th>
                    </tr>
                `);
                if (res.result != 400) {
                    $.each(res.data, function (index, item) {
                        div.append(`
                        <tr>
                            <td>
                                <div class="custom-checkbox">
                                    <input type="checkbox" name="select-row" data-id="${item.id}"/>
                                    <span class="checkmark"></span>
                                </div>
                            </td>
                            <td>${item.investorResourceCode}</td>
                            <td>${item.addressFind}</td>
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