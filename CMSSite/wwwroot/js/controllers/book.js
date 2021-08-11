let baseUrl = '/Admin/Book/';
$(function () {

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
                        <th>Action</th>
                        <th>Tên truyện</th>
                        <th>Danh mục</th>
                        <th>Hình ảnh</th>
                        <th>Giới tính</th>
                        <th>Điểm đánh giá</th>
                        <th>Trạng thái</th>
                      
                    </tr>
                `);

                    if (res.result != 400) {
                        $.each(res.data, function (index, item) {
                            div.append(`
                        <tr>
                            <td>
                                <button type="button" class="delete-staff-sale" data-id="${item.BookId}"><img src="/Assets/crm/images/employee-manage/delete.svg" alt="" /></button>
                            </td>
                            <td>
                                <button type="button" class="view-info" style="text-align: left" data-id="${item.id}">
                                    ${item.BookName}
                                </button>
                            </td>
                            <td>${item.CategoryName}</td>
                            
                            <td><img src="${item.ImgUrl}" /></td>
                            <td>${item.Sex}</td>
                            <td>${item.Rating}</td>
                            <td>${item.isEnable ?? ''}</td>
                           
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
                        <th>Action</th>
                        <th>Tên truyện</th>
                        <th>Danh mục</th>
                        <th>Hình ảnh</th>
                        <th>Giới tính</th>
                        <th>Điểm đánh giá</th>
                        <th>Trạng thái</th>
                       
                    </tr>
                `);

                if (res.result != 400) {
                    $.each(res.data, function (index, item) {
                        div.append(`
                        <tr>
                            <td>
                                <button type="button" class="delete-staff-sale" data-id="${item.bookId}"><img src="/Assets/crm/images/employee-manage/delete.svg" alt="" /></button>
                            </td>
                            <td>
                                <button type="button" class="view-info" style="text-align: left" data-id="${item.bookId}">
                                    ${item.bookName}
                                </button>
                            </td>
                            <td>${item.categoryName}</td>

                            <td><img src="${item.ImgUrl}" /></td>
                            <td>${item.sex}</td>
                            <td>${item.rating}</td>
                            <td>${item.isEnable ?? ''}</td>
                           
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