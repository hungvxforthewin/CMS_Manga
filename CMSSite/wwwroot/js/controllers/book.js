let baseUrl = '/Admin/Book/';
$(function () {
    LoadCategoriesToFormPage('#search-book');
    LoadSexsToForm('#search-book');
    LoadStatusToForm('#search-book');

    LoadCategoriesToForm('#book-add');
    LoadSexsToForm('#book-add');
    LoadStatusToForm('#book-add');
    LoadAuthorToForm('#book-add');
    LoadCommentAllowedToForm('#book-add');

    $('#btn-search-book').on('click', function () {
        app.component.Loading.Show();
        let data = $('#search-book-form').serializeObject();
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
                        <th class="text-center">Tên truyện</th>
                        <th class="text-center">Tác giả</th>
                        <th class="text-center">Danh mục</th>
                        <th class="text-center">Hình ảnh</th>
                        <th class="text-center">Đối tượng</th>
                        <th class="text-center">Điểm đánh giá</th>
                        <th class="text-center">Trạng thái</th>
                        <th class="text-center">Action</th>
                      
                    </tr>
                `);

                    if (res.result != 400) {
                        $.each(res.data, function (index, item) {
                            div.append(`
                        <tr>
                            <td>
                                <button type="button" class="view-info" style="text-align: left" data-id="${item.bookId}">
                                    ${item.bookName}
                                </button>
                            </td>
                            <td class="text-center">${item.author}</td>
                            <td>${item.categoryName}</td>

                            <td class="text-center"><img height="100" src="/media/${item.imgUrl}" /></td>
                            <td class="text-center">${item.sex}</td>
                            <td class="text-center">${item.rating}</td>
                            <td class="text-center">${item.status}</td>
                            <td class="text-center">
                                <button type="button" class="delete-book" data-id="${item.bookId}"><img src="/Assets/crm/images/employee-manage/delete.svg" alt="" /></button>
                                ${item.isApprove == false ? `` :
                                `<div class='radiotext status'>
                                    <button data-id="${item.bookId}" class="button_lit button2 accept-book" type="button" modal-show="show"
                                    modal-data="#book-accept" >
                                        <img src='/Assets/SA/images/accountant_contact-manage/done_all_24px_rounded.svg' alt=''/>
                                        <p>Duyệt</p>
                                    </button>
                                </div>`}
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

    $('body').on('click', 'button.accept-book', function () {
        $('#book-accept input').val('');
        let btn = $(this);
        let id = $(btn).data('id');
        if (id === "" || id === undefined) {
            toastr.error('Truyện không tồn tại');
            return;
        }
        $('#id-book-confirm').val(id);
    })

    $('body').on('click', '#btn-update-confirm', function () {
        let id = $('#id-book-confirm').val();
        $.ajax({
            type: 'POST',
            url: baseUrl + 'UpdateStatus',
            data: {
                id: id,
            },
            success: function (res) {
                if (res.status) {
                    toastr.success('Duyệt truyện thành công', 'Thông báo !');
                    $('.close__modal').trigger('click');
                    SetupPagination();
                    $('#book-accept input').val('');
                } else {
                    toastr.error(res.mess, 'Thông báo !');
                }
            }
        });
    })

    $('.btn-add-book').on('click', function () {
        let data = $('#frm-book-add').serializeObject();
        data.CategoryIds = $(".select-category").select2("val");
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
                    toastr.success('Thêm mới truyện thành công!', 'Thông báo');
                    ResetValueInForm('#frm-book-add');
                    $('.close__modal').trigger('click');
                    SetupPagination();
                    app.component.Loading.Hide();
                    /*$('.close__btn').trigger('click');*/

                } else {
                    //rs.lst.map(function (item) {
                    //    toastr.error(`${item.field}: ${item.errorMessage}`, 'Thông báo');
                    //});
                    toastr.error(`có lỗi xảy ra`, 'Thông báo');
                    app.component.Loading.Hide();
                }
            }
        });
    });

    $('body').on('click', 'button.view-info', function () {
        EditBookInfo($(this).data('id'));
    })

    $('body').on('click', 'button.delete-book', function () {
        app.component.Loading.Show();
        let id = $(this).data('id');
        $.get(baseUrl + 'IsDelete?id=' + id, function (res) {
            //console.log(res);
            if (res.status == false) {
                toastr.error(res.mess);
                app.component.Loading.Hide();
            }
            else {
                ShowModal('#book-delete');
                $('#txt-del-book').html(`Bạn có chắc muốn xóa ?`)
                app.component.Loading.Hide();

                $('#delete-book').click(function () {
                    app.component.Loading.Show();
                    $.get(baseUrl + 'Delete?id=' + id, function (res) {
                        if (res.status) {
                            CloseModal('#book-delete');
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
    });

    $('body').on('click', '#btn-update-book', function () {
        debugger;
        let data = $('#frm-book-edit').serializeObject();
        data.CategoryIds = $(".select-category").select2("val");
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
                    toastr.success('Cập nhật truyện thành công!', 'Thông báo');
                    ResetValueInForm('#frm-book-edit');
                    $('#book-edit').modal('hide');
                    SetupPagination();
                    app.component.Loading.Hide();
                    $('.close__modal').trigger('click');
                } else {
                    //rs.lst.map(function (item) {
                    //    toastr.error(`${item.field}: ${item.errorMessage}`, 'Thông báo');
                    //});
                    toastr.error(`có lỗi xảy ra`, 'Thông báo');
                    app.component.Loading.Hide();
                }
            }
        });
    });

    $(".autofill_today").datetimepicker(
        {
            format: 'dd-mm-yyyy',
            minView: 2,
            maxView: 4,
            autoclose: true
        });
    $('.isNumberF').keyup(delaySystem(function (e) {
        let v = $(this).val();
        v = v.replace(/[^0-9]+/g, '');
        $(this).val(numberFormartAdmin(v));
    }, 0));

    $('#size-page select').on('change', function () {
        $('.loading-wrapper').show();
        SetupPagination();
        $('.loading-wrapper').hide();
    });
    SetupPagination();
});

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
                },
                method: 'POST',
                dataType: 'json'
            }).done(function (res) {
                //console.log(res);
                let div = $('#table-body');
                div.html('');
                div.html(`
                     <tr>
                        <th class="text-center">Tên truyện</th>
                        <th class="text-center">Tác giả</th>
                        <th class="text-center">Danh mục</th>
                        <th class="text-center">Hình ảnh</th>
                        <th class="text-center">Đối tượng</th>
                        <th class="text-center">Điểm đánh giá</th>
                        <th class="text-center">Trạng thái</th>
                        <th class="text-center">Action</th>
                       
                    </tr>
                `);

                if (res.result != 400) {
                    $.each(res.data, function (index, item) {
                        div.append(`
                            <tr>
                                <td>
                                    <button type="button" class="view-info" style="text-align: left" data-id="${item.bookId}">
                                        ${item.bookName}
                                    </button>
                                </td>
                                <td class="text-center">${item.author}</td>
                                <td>${item.categoryName}</td>

                                <td class="text-center"><img height="100" src="/media/${item.imgUrl}" /></td>
                                <td class="text-center">${item.sex}</td>
                                <td class="text-center">${item.rating}</td>
                                <td class="text-center">${item.status}</td>
                                <td class="text-center">
                                    <button type="button" class="delete-book" data-id="${item.bookId}"><img src="/Assets/crm/images/employee-manage/delete.svg" alt="" /></button>
                                    ${item.isApprove == false ? `` :
                                    `<div class='radiotext status'>
                                        <button data-id="${item.bookId}" class="button_lit button2 accept-book" type="button" modal-show="show"
                                        modal-data="#book-accept" >
                                            <img src='/Assets/SA/images/accountant_contact-manage/done_all_24px_rounded.svg' alt=''/>
                                            <p>Duyệt</p>
                                        </button>
                                    </div>`}
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

var EditBookInfo = function (id) {
    $.get(baseUrl + "Edit?id=" + id, function (res) {
        if (res.result == 400) {
            toastr.error(res.errors.join('<br />'));
        }
        else {
            $('#book-edit .content-modal').html(res);
            $('#book-edit').addClass('show-modal');
            $('#book-edit .content-modal').addClass('show-modal');
        }
    }).done(function () {
        let selectedCategory = $('#book-edit .hidden-category').val();
        LoadCategoriesToForm('#book-edit', selectedCategory);
        let selectedAuthorAccountId = $('#book-edit .hidden-authorAccountId').val();
        LoadAuthorToForm('#book-edit', selectedAuthorAccountId);
        let selectedBookSexId = $('#book-edit .hidden-bookSexId').val();
        LoadSexsToForm('#book-edit', selectedBookSexId);
        let selectedIsEnable = $('#book-edit .hidden-isEnable').val();
        LoadStatusToForm('#book-edit', selectedIsEnable);
        let selectedCommentAllowed = $('#book-edit .hidden-commentAllowed').val();
        LoadCommentAllowedToForm('#book-edit', selectedCommentAllowed);


        $('#book-edit select').select2();
        $(".autofill_today").datetimepicker(
            {
                format: 'dd-mm-yyyy',
                minView: 2,
                maxView: 4,
                autoclose: true
            });
        $('.isNumberF').keyup(delaySystem(function (e) {
            let v = $(this).val();
            v = v.replace(/[^0-9]+/g, '');
            $(this).val(numberFormartAdmin(v));
        }, 0));
    });
}

var LoadCategoriesToFormPage = function (target, selected = null) {
    $.get(baseUrl + "GetAllCategories", function (res) {
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            var el = $(target).find('.select-category');
            el.html('');
            el.append(`<option value="">-- Danh mục --</option>`);
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.categoryId}">${item.categoryName}</option>`);
            });
            if (selected) {
                if (selected.split(';').length > 0) {
                    $(el).val(selected.split(';'));
                }
            }
        }
    });
}

var LoadCategoriesToForm = function (target, selected = null) {
    $.get(baseUrl + "GetAllCategories", function (res) {
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            var el = $(target).find('.select-category');
            el.html('');
            //el.append(`<option value="">-- Danh mục --</option>`);
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.categoryId}">${item.categoryName}</option>`);
            });
            if (selected) {
                if (selected.split(';').length > 0) {
                    $(el).val(selected.split(';'));
                }
            }
        }
    });
}

var LoadSexsToForm = function (target, selected = null) {
    $.get(baseUrl + "GetAllSexs", function (res) {
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            var el = $(target).find('.select-sex');
            el.html('');
            el.append(`<option value="">-- Đối tượng --</option>`);
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.bookSexId}">${item.bookSexName}</option>`);
            });
            if (selected) {
                $(el).val(selected);
                /*$(el).trigger('change');*/
            }
        }
    });
}

var LoadStatusToForm = function (target, selected = null) {
    var el = $(target).find('.select-status');
    el.html('');
    el.append(`<option value="">Trạng thái</option>`);
    el.append(`<option value="false">Khóa</option>`);
    el.append(`<option value="true">Kích hoạt</option>`);
    if (selected) {
        $(el).val(selected);
        /*$(el).trigger('change');*/
    }
}

var LoadAuthorToForm = function (target, selected = null) {
    $.get(baseUrl + "GetAllAuthors", function (res) {
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            var el = $(target).find('.select-author');
            el.html('');
            el.append(`<option value="">-- Tác giả --</option>`);
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.accountId}">${item.nickname}</option>`);
            });
            if (selected) {
                $(el).val(selected);
                /*$(el).trigger('change');*/
            }
        }
    });
}

var LoadCommentAllowedToForm = function (target, selected = null) {
    var el = $(target).find('.select-commentAllowed');
    el.html('');
    el.append(`<option value="false">Không cho phép</option>`);
    el.append(`<option value="true">Cho phép</option>`);
    if (selected) {
        $(el).val(selected);
        /*$(el).trigger('change');*/
    }
}