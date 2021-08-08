function numberFormartAdmin(n) {
    n = n.replace(/[^0-9]+/g, '');
    return commaSeparateNumber(n);
}
function delayAdmin(callback, ms) {
    let timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}
function delayMinMax(callback, ms) {
    let timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}
function commaSeparateNumber(val) {
    while (/(\d+)(\d{3})/.test(val.toString())) {
        val = val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
    }
    return val;
}
function RemoveSalaryForSaleAdmin(e) {
    let body = $(e).closest('tbody');
    $(e).closest('tr').remove();

    body.find('.label-level').each(function (index, item) {
        $(item).text('Level ' + (index + 1));
    });
}
function AddSalaryForSaleAdmin(e) {
    let rows = $(e).closest('table').find('tr.adminSaleAdd');
                        //<td>
                        //    <div>
                        //        <input type="text" class="isPercent SharePercent_Admin_Sale" />
                        //    </div>
                        //</td>
    $(e).before(
        `
                   <tr class="adminSaleAdd">
                       <td class="label-level">Level ${rows.length + 1}</td>
                        <td>
                            <div>
                                <input type="text" class="Remuneration_Admin_Sale" />
                            </div>
                        </td>
                        <td class="resize">
                            <div>
                                <input type="text" name="isMinMaxAdmin" class="Revenue_Admin_Sale" />
                            </div>
                        </td>
                        <td>
                            <img onclick="RemoveSalaryForSaleAdmin(this)" src="/Assets/crm/images/mechanism/delete.svg"
                                 alt="" />
                        </td>
                    </tr>
                `
    );
}
var sale = {
    load: function () {
        $('.isNumber').keyup(delayAdmin(function (e) {
            let v = $(this).val();
            v = v.replace(/[^0-9]+/g, '');
            $(this).val(numberFormartAdmin(v));
        }, 0));
        $(document).on('keyup', '.Remuneration_Admin_Sale', delayAdmin(function (e) {
            let v = $(this).val();
            v = v.replace(/[^0-9]+/g, '');
            $(this).val(numberFormartAdmin(v));
        }, 0));
        ////khanhkk added
        //$(document).on('keyup', '.SharePercent_Admin_Sale', delayAdmin(function (e) {
        //    let v = $(this).val();
        //    v = v.replace(/[^0-9]+/g, '');
        //    $(this).val(numberFormartAdmin(v));
        //}, 0));
        ////khanhkk added

        //$('.Revenue_Admin_Sale').keyup(delayMinMax(function (e) {
        //    let v1 = $(this).val();
        //    v1 = v1.replace(/[^0-9-]+/g, '');
        //    $(this).val(commaSeparateNumber(v1));
        //}, 0));
        $(document).on('keyup', '.Revenue_Admin_Sale', delayMinMax(function (e) {
            let v1 = $(this).val();
            v1 = v1.replace(/[^0-9-]+/g, '');
            $(this).val(commaSeparateNumber(v1));
        }, 0));
        $(window).click(function (e) {
            $('.adminSaleAdd').each(function (item, index) {
                let v = $(this).find('input[name="isMinMaxAdmin"]').val();
                let checkV = v.replaceAll(',', '').replaceAll('.', '');
                console.log(v);
                let result = checkV.match(/(^[0-9]{1,}-[0-9]{1,}$)|(^[0-9]{1,}$)+/g);
                console.log(result);
                if (result) {
                    $(this).find('.Revenue_Admin_Sale').val(v);
                } else {
                    $(this).find('.Revenue_Admin_Sale').val('');
                }
            });
        });
        $('#btnSaleAdminUpdate').on('click', function () {
            app.component.Loading.Show();
            let SaleAdminLevelSalaryRevenues = [];
            let flag = true;
            $('.adminSaleAdd').each(function () {
                if ($(this).find('input[class="Remuneration_Admin_Sale"]').val() === '') {
                    toastr.error('Hoa hồng không được trống.', 'Gặp lỗi!');
                    flag = false;
                    app.component.Loading.Hide();

                }
                ////khanhkk added
                //if ($(this).find('input[class="isPercent SharePercent_Admin_Sale"]').val() === '') {
                //    toastr.error('Phần trăm tính cổ phần không được trống.', 'Gặp lỗi!');
                //    flag = false;
                //    app.component.Loading.Hide();
                //}
                ////khanhkk added

                if ($(this).find('input[class="Revenue_Admin_Sale"]').val() === '') {
                    toastr.error('Doanh số không được trống.', 'Gặp lỗi!');
                    flag = false;
                    app.component.Loading.Hide();

                }
                SaleAdminLevelSalaryRevenues.push({
                    PercentRemuneration: $(this).find('input[class="Remuneration_Admin_Sale"]').val(),
                    ////khanhkk added
                    //SharePercent: $(this).find('input[class="isPercent SharePercent_Admin_Sale"]').val(),
                    ////khanhkk added
                    MinMaxRevenueBranch: $(this).find('input[name="isMinMaxAdmin"]').val().replaceAll('.', '').replaceAll(',', ''),
                    CodeKpi: $(this).find('input[name="CodeKpi"]').val(),
                    CodeRemuneration: $(this).find('input[name="CodeRemuneration"]').val(),
                    KpiId: $(this).find('input[name="KpiId"]').val(),
                    RemunerationId: $(this).find('input[name="RemunerationId"]').val(),
                });
            })
            let Salary = $('.Salary_Sale_Admin').val().replaceAll('.', '').replaceAll(',', '');
            let TimeProbationary = $('.TimeProbationary_Sale_Admin').val().replaceAll('.', '').replaceAll(',', '');
            let ProbationarySalary = $('.ProbationarySalary_Sale_Admin').val().replaceAll('.', '').replaceAll(',', '');
            let IdRoleSalaryStaff = $('.IdRoleSalaryStaff').val();

            if (Salary === '' || TimeProbationary === '' || ProbationarySalary === '') {
                flag = false;
                toastr.error('Lương hoặc tháng thử việc trống.', 'Gặp lỗi!');
                app.component.Loading.Hide();

            }
            if (flag) {
                console.log('hehe');
                $.ajax({
                    url: '/Admin/SaleAdmin/Setup',
                    method: 'POST',
                    data: {
                        model: {
                            IdRoleSalaryStaff,
                            Salary,
                            TimeProbationary,
                            ProbationarySalary,
                            SaleAdminLevelSalaryRevenues
                        }
                    },
                    success: function (rs) {
                        app.component.Loading.Hide();
                        if (rs.status) {
                            console.log(Salary);
                            $('.Salary_Sale_Admin').val(commaSeparateNumber(Salary));
                            $('.TimeProbationary_Sale_Admin').val();
                            $('.ProbationarySalary_Sale_Admin').val();
                            toastr.success('Update thành công.', 'Note!');
                        } else {
                            toastr.error(rs.mess, 'Gặp lỗi!');
                        }
                    }
                })
            }
        })
    }
}
$(document).ready(function () {
    sale.load();
});