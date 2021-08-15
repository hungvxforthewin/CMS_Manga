let baseUrl = '/Admin/BookImage/';
$(function () {
    LoadIsBannerToForm('#search-book-image');

    LoadBooksToForm('#book-image-add');
    LoadIsBannerToForm('#book-image-add');

    $('#btn-search-book-image').on('click', function () {
        app.component.Loading.Show();
        let data = $('#search-book-image-form').serializeObject();
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
                        <th class="text-center">Hình ảnh</th>
                        <th class="text-center">Loại</th>
                        <th class="text-center">Action</th>
                    </tr>
                `);

                    if (res.result != 400) {
                        $.each(res.data, function (index, item) {
                            div.append(`
                        <tr>
                            <td>
                                <button type="button" class="view-info" style="text-align: left" data-id="${item.imgId}">
                                    ${item.bookName}
                                </button>
                            </td>

                            <td class="text-center"><img height="100" src="/media/${item.imgUrl}" /></td>
                            <td class="text-center">${item.isBanner}</td>
                            <td class="text-center">
                                <button type="button" class="delete-book-image" data-id="${item.imgId}"><img src="/Assets/crm/images/employee-manage/delete.svg" alt="" /></button>
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

    $('.btn-add-book-image').on('click', function () {
        let data = $('#frm-book-image-add').serializeObject();
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
                    toastr.success('Thêm mới Banner thành công!', 'Thông báo');
                    ResetValueInForm('#frm-book-image-add');
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
                        <th class="text-center">Hình ảnh</th>
                        <th class="text-center">Loại</th>
                        <th class="text-center">Action</th>
                    </tr>
                `);

                if (res.result != 400) {
                    $.each(res.data, function (index, item) {
                        div.append(`
                        <tr>
                            <td>
                                <button type="button" class="view-info" style="text-align: left" data-id="${item.imgId}">
                                    ${item.bookName}
                                </button>
                            </td>

                            <td class="text-center"><img height="100" src="/media/${item.imgUrl}" /></td>
                            <td class="text-center">${item.isBanner}</td>
                            <td class="text-center">
                                <button type="button" class="delete-book-image" data-id="${item.imgId}"><img src="/Assets/crm/images/employee-manage/delete.svg" alt="" /></button>
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

var LoadBooksToForm = function (target, selected = null) {
    $.get(baseUrl + "GetAllBooks", function (res) {
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            var el = $(target).find('.select-book');
            el.html('');
            //el.append(`<option value="">-- Danh mục --</option>`);
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.bookId}">${item.bookName}</option>`);
            });
            if (selected) {
                if (selected.split(';').length > 0) {
                    $(el).val(selected.split(';'));
                }
            }
        }
    });
}

var LoadIsBannerToForm = function (target, selected = null) {
    var el = $(target).find('.select-IsBanner');
    el.html('');
    el.append(`<option value="">-- Loại --</option>`);
    el.append(`<option value="false">Ảnh Thường</option>`);
    el.append(`<option value="true">Ảnh Banner</option>`);
    if (selected) {
        $(el).val(selected);
        /*$(el).trigger('change');*/
    }
}
