var Customer = {
    ui: {
    },
    load: function () {
        $("#btnImportExcel").on("click", function () {
            $("input#fileImportExcelCustomer").trigger("click");
        });
        $(".closeModalImport").on('click', function () {
            $('#CustomersImport').modal('hide');
            app.component.Loading.Hide();
        });
        $("#btnExportExcel").on("click", function () {
            app.component.Loading.Show();
            $('#CustomersExport').modal('show');
        });
        function delayImport(callback, ms) {
            let timer = 5000;
            return function () {
                var context = this, args = arguments;
                clearTimeout(timer);
                timer = setTimeout(function () {
                    callback.apply(context, args);
                }, ms || 5000);
            };
        }
        $("#fileImportExcelCustomer").on("change", function () {
            app.component.Loading.Show();
            debugger
            var formData = new FormData($('form.form-inline')[0]);
            $.ajax({
                url: "/Employees/ReadFileImportExcel",
                method: "POST",
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                success: function (rs) {
                    $("#fileImportExcelCustomer").val(null);
                    console.log(rs);
                    app.component.Loading.Hide();
                    if (rs.success) {
                        if (rs.listError.length > 0) {
                            $("#txtLstError").text("Có " + rs.listError.length + " lỗi trong file Excel.");
                            var htmlError = "";
                            rs.listError.forEach(function (item) {
                                htmlError += "<tr><td>" + item + "</td></tr>";
                            });
                            $("#tblImportError").html(htmlError);
                        }
                        else {
                            $("#txtLstError").text("File excel không có lỗi!");
                        }
                        $("#tblCustomersImport").bootstrapTable({
                            striped: true,
                            pagination: true,
                            limit: 20,
                            pageSize: 20,
                            pageList: [20, 50, 100, 200, 500],
                            search: false,
                            showColumns: false,
                            showRefresh: false,
                            minimumCountColumns: 2,
                            columns: [
                                {
                                    field: 'rownumber',
                                    title: 'STT',
                                    align: 'center',
                                    valign: 'middle',
                                    width: '50px'
                                },
                                {
                                    field: 'employeeID',
                                    title: 'Mã Nhân Viên',
                                    align: 'center',
                                    valign: 'middle',
                                    sortable: true,
                                    formatter: function (value, row, index) {
                                        if (value === null)
                                            return '';
                                        else return '<div class="text-left">' + value + '</div>';;
                                    }
                                },
                                {
                                    field: 'fullName',
                                    title: 'Họ và Tên',
                                    align: 'center',
                                    valign: 'middle',
                                    sortable: true,
                                    formatter: function (value, row, index) {
                                        if (value === null)
                                            return '';
                                        else return '<div class="text-left">' + value + '</div>';;
                                    }
                                },
                                {
                                    field: 'teamDepart',
                                    title: 'Team-Phòng ban',
                                    align: 'center',
                                    valign: 'middle',
                                    formatter: function (value, row, index) {
                                        if (value === null)
                                            return '';
                                        else return '<div class="text-left">' + value + '</div>';;
                                    }
                                },
                                {
                                    field: 'position',
                                    title: 'Vị trí',
                                    align: 'center',
                                    valign: 'middle',
                                    formatter: function (value, row, index) {
                                        if (value === null)
                                            return '';
                                        else return '<div class="text-left">' + value + '</div>';;
                                    }
                                },
                                {
                                    field: 'phone',
                                    title: 'Số điện thoại',
                                    align: 'center',
                                    valign: 'middle',
                                    formatter: function (value, row, index) {
                                        if (value === null)
                                            return '';
                                        else return '<div class="text-left">' + value + '</div>';;
                                    }
                                },
                                {
                                    field: 'startDate',
                                    title: 'Ngày vào làm',
                                    align: 'center',
                                    valign: 'middle',
                                    formatter: function (value, row, index) {
                                        if (value === null)
                                            return '';
                                        else return '<div class="text-left">' + value + '</div>';;
                                    }
                                },
                                {
                                    field: 'email',
                                    title: 'Email',
                                    align: 'center',
                                    valign: 'middle',
                                    formatter: function (value, row, index) {
                                        if (value === null)
                                            return '';
                                        else return '<div class="text-left">' + value + '</div>';;
                                    }
                                },
                                {
                                    field: 'cmt',
                                    title: 'CMT/Căn cước',
                                    align: 'center',
                                    valign: 'middle',
                                    formatter: function (value, row, index) {
                                        if (value === null)
                                            return '';
                                        else return '<div class="text-left">' + value + '</div>';;
                                    }
                                },
                                {
                                    title: "Thông tin Import",
                                    field: "importMessage",
                                    align: 'center',
                                    valign: 'middle',
                                    formatter: function (value, row, index) {
                                        if (value == "") {
                                            return 'Chưa thực hiện';
                                        }
                                        else if (value == "1") {
                                            return "<i style='color:green'>Thành Công</i>";
                                        }
                                        else {
                                            return "<i style='color:red'>" + value + "</i>";
                                        }
                                    }
                                },
                                {
                                    title: "Xóa",
                                    align: 'center',
                                    valign: 'middle',
                                    formatter: function (value, row, index) {
                                        return '<a class="remove" href="javascript:void(0)" style="color:red" title="Xóa"><i class="fas fa-trash-alt"></i></a>';
                                    },
                                    events: {
                                        'click .remove': function (e, value, row, index) {
                                            if (row != null) {
                                                $("#tblCustomersImport").bootstrapTable("remove", { field: 'rownumber', values: [row.rownumber] });
                                            }
                                        }
                                    }
                                }
                            ]
                        });
                        $("#tblCustomer").bootstrapTable("refresh");
                        $("#tblCustomersImport").bootstrapTable('removeAll');
                        if (rs.lstCus.length > 0) {

                            $("#tblCustomersImport").bootstrapTable('load', rs.lstCus);
                        }
                        $('#btnAddCustomersImport').removeAttr('disabled');
                        $("#CustomersImport").modal("show");
                        $("#CustomersImport .modal-dialog").attr("style", "width:90%;");

                    }
                    else {
                        console.log(rs.message);
                        notifyError("Lỗi đọc file!");
                    }
                }
            });
        });
        $("#btnAddCustomersImport").on("click", function () {
            app.component.Loading.Show();
            var dataImport = $("#tblCustomersImport").bootstrapTable('getData');
            console.log(dataImport);
            var data = $.map(dataImport, function (e) {
                if (e.importMessage == "" && e.position != 'Lễ tân' && e.position != 'Front-end') {
                    return {
                        EmployeeID: e.employeeID,
                        FullName: e.fullName,
                        TeamDepart: e.teamDepart,
                        Position: e.position,
                        StartDate: e.startDate,
                        DateOfBirth: e.dateOfBirth,
                        CMT: e.cmt,
                        DateIssued: e.dateIssued,
                        AddIssued: e.addIssued,
                        PermanentAddress: e.permanentAddress,
                        CurrentResidence: e.currentResidence,
                        Phone: e.phone,
                        Email: e.email,
                        NumberBank: e.numberBank,
                        Bank: e.bank,
                    };
                }
            });
            console.log(dataImport);
            console.log(data);
            if (data.length > 0) {
                _AjaxPostForm("/Employees/UpdateImportExcel", { data }, function (rs) {
                    app.component.Loading.Hide();
                    notifySuccess("Import dữ liệu thành công");
                    $("#tblCustomersImport").bootstrapTable('load', rs.data);
                    $("#tblCustomer").bootstrapTable("refresh");
                });
            }
            else {
                app.component.Loading.Hide();
                notifyWarning("Không có dữ liệu đúng để Import");
            }
            $(this).attr('disabled', 'true');
        });
    },
    component: {
    }
}
$(document).ready(function () {
    Customer.load();
});
