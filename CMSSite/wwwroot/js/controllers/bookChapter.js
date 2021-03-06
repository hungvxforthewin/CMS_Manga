let baseUrl = '/Admin/BookChapter/';
$(function () {
    LoadStatusToForm('#search-book-chapter');

    LoadStatusToForm('#book-chapter-add');
    LoadBooksToForm('#book-chapter-add');

    $('#btn-search-book-chapter').on('click', function () {
        app.component.Loading.Show();
        let data = $('#search-book-chapter-form').serializeObject();
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
                        <th class="text-center">Tên sách</th>
                        <th class="text-center">Tên Chapter</th>
                        <th class="text-center">Số lượng trang</th>
                        <th class="text-center">Tuổi giới hạn</th>
                        <th class="text-center">Ngày đăng</th>
                        <th class="text-center">Trạng thái</th>
                        <th class="text-center">Action</th>
                      
                    </tr>
                `);

                    if (res.result != 400) {
                        $.each(res.data, function (index, item) {
                            div.append(`
                            <tr>
                                <td>${item.bookName}</td>
                                <td>
                                    <button type="button" class="view-info" style="text-align: left" data-id="${item.chapterId}">
                                        ${item.chapterName}
                                    </button>
                                </td>
                                <td class="text-center">${item.numberPages}</td>

                                <td class="text-center">${item.adultLimit}</td>
                                <td class="text-center">${item.publishDate}</td>
                                <td class="text-center">${item.chapterStatus}</td>
                                <td class="text-center">
                                    <button type="button" class="delete-book-chapter" data-id="${item.chapterId}"><img src="/Assets/crm/images/employee-manage/delete.svg" alt="" /></button>
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

    $('.btn-add-book-chapter').on('click', function () {
        let data = $('#frm-book-chapter-add').serializeObject();
        debugger;
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
                    toastr.success('Thêm mới Chapter thành công!', 'Thông báo');
                    ResetValueInForm('#frm-book-chapter-add');
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
        EditBookChapterInfo($(this).data('id'));
    })

    $('body').on('click', 'button.delete-book-chapter', function () {
        app.component.Loading.Show();
        let id = $(this).data('id');
        $.get(baseUrl + 'IsDelete?id=' + id, function (res) {
            //console.log(res);
            if (res.status == false) {
                toastr.error(res.mess);
                app.component.Loading.Hide();
            }
            else {
                ShowModal('#book-chapter-delete');
                $('#txt-del-book-chapter').html(`Bạn có chắc muốn xóa ?`)
                app.component.Loading.Hide();

                $('#delete-book-chapter').click(function () {
                    app.component.Loading.Show();
                    $.get(baseUrl + 'Delete?id=' + id, function (res) {
                        if (res.status) {
                            CloseModal('#book-chapter-delete');
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

    $('body').on('click', '#btn-update-book-chapter', function () {
        debugger;
        let data = $('#frm-book-chapter-edit').serializeObject();
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

    $('body').on('change', '.myFile', function () {
        var files = $(this).prop("files");
        formData = new FormData();
        formData.append("file", files[0]);

        var item = $(this).closest('.item-chapter');
        var indexItem = $(".item-chapter").index(item);
        var indexCount = $(".item-chapter").length;
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
                    var value = $('#myFileName').val();

                    if (indexItem + 1 < indexCount) {
                        var valueArray = value.split(';');
                        for (var i = 0; i < valueArray.length; i++) {
                            if (i == indexItem) {
                                valueArray[i] = rs.data;
                            }
                        }
                        $('#myFileName').val(valueArray.join(';'));
                    }
                    else {
                        $('#myFileName').val(value == "" ? rs.data : value + ";" + rs.data);
                    }

                    $('.loading-wrapper').hide();

                } else {

                    $('.loading-wrapper').hide();
                }
            }
        })
    })

    $('body').on('change', '.myFile', function () {
        if (typeof (FileReader) != "undefined") {
            var divChapter = $('#div-chapter');
            var divClosest = $(this).closest('div');
            var dvPreview = divClosest.find(".divImageMedia");
            var item = $(this).closest('.item-chapter');
            var indexItem = $(".item-chapter").index(item);
            var indexCount = $(".item-chapter").length;
            //var dvPreview = $(".divImageMedia");
            dvPreview.html("");
            $($(this)[0].files).each(function () {
                var file = $(this);
                var reader = new FileReader();
                reader.onload = function (e) {
                    var img = $("<img />");
                    img.attr("style", "width: 150px; height:100px; padding: 10px");
                    img.attr("src", e.target.result);
                    dvPreview.append(img);
                    dvPreview.append(`
                        <span title="close" class="item-chapter-clear">
                            <img src="/Assets/SA/images/layout/close-modal.svg" width="20"/>
                        </span>
                    `);
                    if (indexItem + 1 == indexCount) {
                        divChapter.append(`
                        <div class="bs-col md-25 item-chapter">
                            <input type="file" class="form-control input-chapter myFile" accept="image/*"/>
                            <div class="divImageMedia">
                                <img />
                            </div>
                        </div>
                    `);
                    }
                }
                reader.readAsDataURL(file[0]);
            });
        } else {
            alert("This browser does not support HTML5 FileReader.");
        }
    });
    $('body').on('click', '.item-chapter-clear', function () {
        var countItem = $('.item-chapter').length;
        if (countItem > 1) {
            var item = $(this).closest('.item-chapter');
            var indexItem = $(".item-chapter").index(item);

            var value = $('#myFileName').val();
            var valueArray = value.split(';');
            valueArray.splice(indexItem, 1);

            $('#myFileName').val(valueArray.join(';'));
            item.remove();

        }
        else {
            var itemImage = $(this).closest('.divImageMedia');
            itemImage.html('');
        }
    });

    $('body').on('change', '.myFileEdit', function () {
        var files = $(this).prop("files");
        formData = new FormData();
        formData.append("file", files[0]);

        var item = $(this).closest('.item-chapter-edit');
        var indexItem = $(".item-chapter-edit").index(item);
        var indexCount = $(".item-chapter-edit").length;
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
                    var value = $('#myFileNameEdit').val();

                    if (indexItem + 1 < indexCount) {
                        var valueArray = value.split(';');
                        for (var i = 0; i < valueArray.length; i++) {
                            if (i == indexItem) {
                                valueArray[i] = rs.data;
                            }
                        }
                        $('#myFileNameEdit').val(valueArray.join(';'));
                    }
                    else {
                        $('#myFileNameEdit').val(value == "" ? rs.data : value + ";" + rs.data);
                    }

                    $('.loading-wrapper').hide();

                } else {

                    $('.loading-wrapper').hide();
                }
            }
        })
    })

    $('body').on('change', '.myFileEdit', function () {
        if (typeof (FileReader) != "undefined") {
            var divChapter = $('#div-chapter-edit');
            var divClosest = $(this).closest('div');
            var dvPreview = divClosest.find(".divImageMediaEdit");
            var item = $(this).closest('.item-chapter-edit');
            var indexItem = $(".item-chapter-edit").index(item);
            var indexCount = $(".item-chapter-edit").length;
            //var dvPreview = $(".divImageMedia");
            dvPreview.html("");
            $($(this)[0].files).each(function () {
                var file = $(this);
                var reader = new FileReader();
                reader.onload = function (e) {
                    var img = $("<img />");
                    img.attr("style", "width: 150px; height:100px; padding: 10px");
                    img.attr("src", e.target.result);
                    dvPreview.append(img);
                    dvPreview.append(`
                        <span title="close" class="item-chapter-clear-edit">
                            <img src="/Assets/SA/images/layout/close-modal.svg" width="20"/>
                        </span>
                    `);
                    if (indexItem + 1 == indexCount) {
                        divChapter.append(`
                        <div class="bs-col md-25 item-chapter-edit">
                            <input type="file" class="form-control input-chapter myFileEdit" accept="image/*"/>
                            <div class="divImageMediaEdit">
                                <img />
                            </div>
                        </div>
                    `);
                    }
                }
                reader.readAsDataURL(file[0]);
            });
        } else {
            alert("This browser does not support HTML5 FileReader.");
        }
    });
    $('body').on('click', '.item-chapter-clear-edit', function () {
        var countItem = $('.item-chapter-edit').length;
        if (countItem > 1) {
            var item = $(this).closest('.item-chapter-edit');
            var indexItem = $(".item-chapter-edit").index(item);

            var value = $('#myFileNameEdit').val();
            var valueArray = value.split(';');
            valueArray.splice(indexItem, 1);

            $('#myFileNameEdit').val(valueArray.join(';'));
            item.remove();

        }
        else {
            var itemImage = $(this).closest('.divImageMediaEdit');
            itemImage.html('');
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
                        <th class="text-center">Tên sách</th>
                        <th class="text-center">Tên Chapter</th>
                        <th class="text-center">Số lượng trang</th>
                        <th class="text-center">Tuổi giới hạn</th>
                        <th class="text-center">Ngày đăng</th>
                        <th class="text-center">Trạng thái</th>
                        <th class="text-center">Action</th>
                       
                    </tr>
                `);

                if (res.result != 400) {
                    $.each(res.data, function (index, item) {
                        div.append(`
                            <tr>
                                <td>${item.bookName}</td>
                                <td>
                                    <button type="button" class="view-info" style="text-align: left" data-id="${item.chapterId}">
                                        ${item.chapterName}
                                    </button>
                                </td>
                                <td class="text-center">${item.numberPages}</td>

                                <td class="text-center">${item.adultLimit}</td>
                                <td class="text-center">${item.publishDate}</td>
                                <td class="text-center">${item.chapterStatus}</td>
                                <td class="text-center">
                                    <button type="button" class="delete-book-chapter" data-id="${item.chapterId}"><img src="/Assets/crm/images/employee-manage/delete.svg" alt="" /></button>
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

var EditBookChapterInfo = function (id) {
    debugger;
    $.get(baseUrl + "Edit?id=" + id, function (res) {
        if (res.result == 400) {
            toastr.error(res.errors.join('<br />'));
        }
        else {
            $('#book-chapter-edit .content-modal').html(res);
            $('#book-chapter-edit').addClass('show-modal');
            $('#book-chapter-edit .content-modal').addClass('show-modal');
        }
    }).done(function () {
        let selectedBook = $('#book-chapter-edit .hidden-book').val();
        LoadBooksToForm('#book-chapter-edit', selectedBook);
        let selectedStatus = $('#book-chapter-edit .hidden-status').val();
        LoadStatusToForm('#book-chapter-edit', selectedStatus);


        $('#book-chapter-edit select').select2();
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


var LoadStatusToForm = function (target, selected = null) {
    var el = $(target).find('.select-status');
    el.html('');
    el.append(`<option value="">-- Trạng thái --</option>`);
    el.append(`<option value="0">Chương đang chờ duyệt</option>`);
    el.append(`<option value="1">Chương đã được duyệt</option>`);
    el.append(`<option value="2">Chương truyện bị hạ</option>`);
    if (selected) {
        $(el).val(selected);
        /*$(el).trigger('change');*/
    }
}



var LoadBooksToForm = function (target, selected = null) {
    $.get(baseUrl + "GetAllBooks", function (res) {
        if (res.result == 400) {
            //$('#error-list').append(`<p class="error-message">${res.errors}</p>`);
        }
        else {
            var el = $(target).find('.select-book');
            el.html('');
            el.append(`<option value="">-- Truyện --</option>`);
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.bookId}">${item.bookName}</option>`);
            });
            if (selected) {
                $(el).val(selected);
                /*$(el).trigger('change');*/
            }
        }
    });
}
