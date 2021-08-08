var customer = {
    ui: {

    },
    load: function () {
        customer.component.table();
        $("#btnSearch").click(function () {
            $("#tblCustomer").bootstrapTable("refresh");
        });
        $(document).on("keyup", function (e) {
            if (e.which == 13) {
                $("#tblCustomer").bootstrapTable("refresh");
            }
        });
        $("body").on('change','#drdView', function () {          
            if ($(this).val() == '1') {
                let id = $("input[type='radio'][name='investorid']:checked").val();
                if (id == undefined) {
                    toastr.error('Chưa chọn khách hàng', 'Thông báo');
                    //$('#drdView').empty(); //remove all child nodes
                    //var newOption = $(`<option value="0">-- Tác vụ --</option>
                    //                    <option value = "1" id = "PreviewInvestor"> Xem thông tin </option >
                    //                `);
                    //$('#drdView').append(newOption);
                    //$('#drdView').trigger("chosen:updated");
                    $("#drdView")[0].selectedIndex = 0;  
                   
                } else {
                    app.component.Loading.Show();
                    cusmodal.ShowView("/Investors/Detail/" + id, function () {
                        app.component.Loading.Hide();
                        app.component.UpfileImage();
                        app.component.ValidateInputPhone();
                        app.component.DatePicker();
                        app.component.Select2();
                        app.component.ValidateInputPhone();
                    });
                }
            } else {
                toastr.error('Chưa chọn khách hàng', 'Thông báo');
            }
        });
        $("#btnDoneExportExcel").on("click", function () {           
            _AjaxPostForm("/Investors/WriteDataToExcel", {
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
            alert($("input[type='radio'][name='investorid']:checked").val());
        });
    },
    component: {
        table: function () {
            $("#tblCustomer").bootstrapTable({
                url: "/Investors/GetData",
                method: "POST",
                ajax: function (config) {
                    app.component.Loading.Show();
                    config.data.search = $("#txtsearch").val();
                    _AjaxPostForm(config.url, {
                        obj: config.data,
                        Status: $('#drdStatus').val()
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
                            var htmlCell = '<input type="radio" id="male" name="investorid" value=' +value+ '>';
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
                        field:'idCard',
                        title: 'Mã thẻ',
                        align: 'center',
                        valign: 'middle',
                        formatter: function (value, row, index) {
                            return '<div class="text-left">' + value + '</div>';
                        }
                    },
                    {
                        field: 'name',
                        title: 'Họ và tên khách hàng',
                        align: 'center',
                        valign: 'middle',
                        formatter: function (value, row, index) {
                            return '<div class="text-left">' + value + '</div>';
                        }
                    },
                    {
                        field: 'phoneNumber',
                        title: 'Số điện thoại',
                        align: 'center',
                        valign: 'middle',
                        formatter: function (value, row, index) {
                            return '<div class="text-left">' + value + '</div>';
                        }
                    },
                    {
                        field: 'email',
                        title: 'Email',
                        align: 'center',
                        valign: 'middle',
                        formatter: function (value, row, index) {
                            return '<div class="text-left">' + value + '</div>';
                        }
                    },
                    {
                        field: 'email',
                        title: 'Số hợp đồng đã ký',
                        align: 'center',
                        valign: 'middle',
                        formatter: function (value, row, index) {
                            return '<div class="text-left">' + value + '</div>';
                        }
                    },
                    {
                        field: 'status',
                        title: 'Trạng thái sử dụng',
                        align: 'center',
                        valign: 'middle',
                        formatter: function (value, row, index) {
                            if (value === 1) {
                                return '<div class="text-left">' + 'Đang hoạt động' + '</div>';
                            } else if (value === 0) {
                                return '<div class="text-left">' + 'Ngừng hoạt động' + '</div>';
                            }
                        }
                    },
                ]
            });
        },
    }
};
$(document).ready(function () {
    customer.load();
})