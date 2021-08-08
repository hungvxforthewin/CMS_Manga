var allowanceOrDeducts = {
    ui: {

    },
    load: function () {
        allowanceOrDeducts.component.table();
        $("#btnSearch").click(function () {
            $("#tblCustomer").bootstrapTable("refresh");
        });
        $(document).on("keyup", function (e) {
            if (e.which == 13) {
                $("#tblCustomer").bootstrapTable("refresh");
            }
        });
        $("body").on('change', '#drdView', function () {
            if ($(this).val() == '1') {
                let id = $("input[type='radio'][name='alowanceid']:checked").val();
                if (id == undefined) {
                    toastr.error('Chưa chọn mức phạt', 'Thông báo');
                    $("#drdView")[0].selectedIndex = 0;
                } else {
                    app.component.Loading.Show();
                    cusmodal.ShowView("/AllowanceOrDeducts/Detail/" + id, function () {
                        app.component.Loading.Hide();
                        app.component.Select2();
                        app.component.ValidateInputPhone();
                    });
                }
            } else if ($(this).val() == '2') {
                let id = $("input[type='radio'][name='alowanceid']:checked").val();
                if (id == undefined) {
                    toastr.error('Chưa chọn mức phạt', 'Thông báo');
                    $("#drdView")[0].selectedIndex = 0;
                } else {
                    app.component.Loading.Show();
                    cusmodal.ShowView("/AllowanceOrDeducts/Edit/" + id, function () {
                        app.component.Loading.Hide();
                        app.component.Select2();
                        app.component.ValidateInputPhone();
                        $("#frmAdd").on("submit", function (event) {
                            event.preventDefault();
                            var data = $(this).serializeObject();
                            _AjaxPostForm("/AllowanceOrDeducts/Insert_Update", { data }, function (rs) {
                                if (rs.success) {
                                    $("#tblCustomer").bootstrapTable("refresh");
                                    $("#mdlCustom").modal("hide");
                                    notifySuccess(rs.message);
                                }
                                else {
                                    notifyError(rs.message);
                                    console.log(rs.message);
                                }
                            })
                        });
                    });
                }
            } else if ($(this).val() == '3') {
                let id = $("input[type='radio'][name='alowanceid']:checked").val();
                let code = $("input[type='radio'][name='alowanceid']:checked").attr('data-code');
                if (id == undefined) {
                    toastr.error('Chưa chọn mức phạt', 'Thông báo');
                    $("#drdView")[0].selectedIndex = 0;
                } else {
                    modal.DeleteComfirm({
                        tilte: "Xóa",
                        message: `Bạn có chắc chắn muốn xóa tiền phạt ${code}`,
                        callback: function () {
                            _AjaxPostForm('/AllowanceOrDeducts/Delete/', { id: id }, function (rs) {
                                if (rs.success) {
                                    notifySuccess(rs.message);
                                    $("#tblCustomer").bootstrapTable("refresh");
                                }
                                else notifyError(rs.message);
                            });
                        }
                    });
                }
            } else {
                toastr.error('Chưa chọn mức phạt', 'Thông báo');
            }
        });
        $("#btnDoneExportExcel").on("click", function () {
            _AjaxPostForm("/AllowanceOrDeducts/WriteDataToExcel", {
                pageNumber: $('#tblCustomer').bootstrapTable('getOptions').pageNumber,
                pageSize: $('#tblCustomer').bootstrapTable('getOptions').pageSize,
            }, function (rs) {
                if (rs.success) {
                    notifySuccess("Xuất báo cáo thành công!");
                    SaveFileAs(rs.urlFile, rs.fileName);
                }
                else {
                    console.log(rs.message);
                    notifyError("Xuất báo cáo thất bại!");
                }
            });
        });
        $('#PreviewInvestor').on('click', function () {
            alert($("input[type='radio'][name='alowanceid']:checked").val());
        });
        $("#btnAdd").click(function () {
            app.component.Loading.Show();
            cusmodal.ShowView("/AllowanceOrDeducts/Create", function () {
                app.component.Loading.Hide();
                //app.component.UpfileImage();
                //app.component.DatePicker();
                //app.component.Select2();
                //app.component.ValidateInputPhone();
                $("#frmAdd").on("submit", function (event) {
                    event.preventDefault();
                    var data = $(this).serializeObject();
                    console.log(data);
                    _AjaxPostForm("/AllowanceOrDeducts/Insert_Update", { data }, function (rs) {
                        console.log(data);
                        if (rs.success) {
                            $("#tblCustomer").bootstrapTable("refresh");
                            $("#mdlCustom").modal("hide");
                            notifySuccess(rs.message);
                        }
                        else {
                            notifyError(rs.message);
                            console.log(rs.message);
                        }
                    })
                });
            });
        });
    },
    component: {
        table: function () {
            $("#tblCustomer").bootstrapTable({
                url: "/AllowanceOrDeducts/GetData",
                method: "POST",
                ajax: function (config) {
                    app.component.Loading.Show();
                    config.data.search = $("#txtsearch").val();
                    _AjaxPostForm(config.url, {
                        obj: config.data
                    }, function (rs) {
                        app.component.Loading.Hide();
                        if (rs.success) {
                            console.log(rs);
                            console.log(config);
                            config.success({
                                total: rs.total,
                                rows: rs.datas
                            });
                        }
                        else {
                            notifyError("Lấy dữ liệu thất bại!");
                            console.log(rs.error);
                        }
                    });
                },
                striped: true,
                sidePagination: 'server',
                pagination: true,
                paginationVAlign: 'both',
                limit: 10,
                pageSize: 10,
                pageList: [10, 25, 50, 100, 200],
                search: false,
                showColumns: false,
                showRefresh: false,
                minimumCountColumns: 2,
                toolbar: "#toolbar",
                columns: [
                    {
                        field: 'id',
                        align: 'center',
                        valign: 'middle',
                        formatter: function (value, row, index) {
                            var htmlCell = '<input type="radio" data-code="' + row.allowanceCode + '" name="alowanceid" value=' + value + '>';
                            return htmlCell;
                        },
                    },
                    {
                        field: 'rownumber',
                        title: 'STT',
                        align: 'center',
                        valign: 'middle',
                        sortable: false,
                        width: '50px'
                    },
                    {
                        field: 'allowanceCode',
                        title: 'ID/Code',
                        align: 'center',
                        valign: 'middle',
                        formatter: function (value, row, index) {
                            return '<div class="text-left">' + value + '</div>';
                        }
                    },
                    {
                        field: 'allowanceName',
                        title: 'Tên',
                        align: 'center',
                        valign: 'middle',
                        formatter: function (value, row, index) {
                            return '<div class="text-left">' + value + '</div>';
                        }
                    },
                    {
                        field: 'allowanceAmount',
                        title: 'Số tiền phạt/lần',
                        align: 'center',
                        valign: 'middle',
                        formatter: function (value, row, index) {
                            return '<div class="text-left">' + formatMoney(value) + ' VNĐ</div>';
                        }
                    }
                ]
            });
        },
    }
};
$(document).ready(function () {
    allowanceOrDeducts.load();
})