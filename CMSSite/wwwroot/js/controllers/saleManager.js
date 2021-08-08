function numberFormart(n) {
    n = n.replace(/[^0-9]+/g, '');
    return n;
}
function delay(callback, ms) {
    let timer = 5000;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 5000);
    };
}
function RemoveSalaryForSaleManager(e) {
    let body = $(e).closest('tbody');
    $(e).closest('tr').remove();

    body.find('.label-level').each(function (index, item) {
        $(item).text('Level ' + (index));
    });
}
function AddSalaryForSaleManager(e) {
    let rows = $(e).closest('table').find('tr.managerSaleAdd');
    $(e).before(
        `
                   <tr class="managerSaleAdd">
                        <td class="label-level">Level ${rows.length}</td>
                                <td>
                                <div>
                                    <input type="text" name="" class="Remuneration_Admin_Manager" />
                                </div>
                                </td>
                                <td>
                                    <div>
                                        <input type="text" class="SharePercent_Admin_Manager" />
                                    </div>
                                </td>
                                <td>
                                    <div>
                                        <input type="text" name="" class="Salary_Admin_Manager" />
                                    </div>
                                </td>
                                <td class="resize">
                                    <div>
                                        <input type="text" name="MinMaxGroup" class="isMinMax" />
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
var RemoveKpiForSaleManager = function (e) {
    $(e).closest('tr').remove();
}

var AddKpiForSaleManager6F = function (e) {
    $(e).closest('tr').before(
        `
            <tr class="managerSale6F">
                        <td>
                            <input type="text" class="isPercentSM" name="PercentKpiMin6F" min="0" value="" placeholder="Min" />
                        </td>
                        <td>
                            <input type="text" class="isPercentSM" name="PercentKpiMax6F" min="0" value="" placeholder="Max" />
                        </td>
                        <td>
                            <input type="number" class="isPercentSM" name="SalaryPercentLv16F" min="0" value="" />
                        </td>
                        <td>
                            <img onclick="RemoveKpiForSale(this)" src="/Assets/crm/images/mechanism/delete.svg"
                                    alt="" />
                        </td>
            </tr>
                `
    );
}
var AddKpiForSaleManager6L = function (e) {
    $(e).closest('tr').before(
        `
            <tr class="managerSale6L">
                        <td>
                            <input type="text" class="isPercentSM" name="PercentKpiMin6L" min="0" value="" placeholder="Min" />
                        </td>
                        <td>
                            <input type="text" class="isPercentSM" name="PercentKpiMax6L" min="0" value="" placeholder="Max" />
                        </td>
                        <td>
                            <input type="number" class="isPercentSM" name="SalaryPercentLv16L" min="0" value="" />
                        </td>
                        <td>
                            <img onclick="RemoveKpiForSale(this)" src="/Assets/crm/images/mechanism/delete.svg"
                                    alt="" />
                        </td>
            </tr>
                `
    );
}
var saleManager = {
    load: function () {
        $(document).on('keyup', '.isMinMax', delayMinMax(function (e) {
            let v1 = $(this).val();
            v1 = v1.replace(/[^0-9-]+/g, '');
            $(this).val(commaSeparateNumber(v1));
        }, 0))
        $(document).on('keyup', '.Salary_Admin_Manager', delayMinMax(function (e) {
            let v1 = $(this).val();
            v1 = v1.replace(/[^0-9-]+/g, '');
            $(this).val(commaSeparateNumber(v1));
        }, 0))
        $(document).on('keyup', '.Remuneration_Admin_Manager', delayMinMax(function (e) {
            let v1 = $(this).val();
            v1 = v1.replace(/[^0-9.]+/g, '');
            $(this).val(commaSeparateNumber(v1));
        }, 0))
        //khanhkk added
        $(document).on('keyup', '.SharePercent_Admin_Manager', delayMinMax(function (e) {
            let v1 = $(this).val();
            v1 = v1.replace(/[^0-9.]+/g, '');
            $(this).val(commaSeparateNumber(v1));
        }, 0))
        //khanhkk added
        $(document).on('keyup', '.isPercentSM', delayMinMax(function (e) {
            let v1 = $(this).val();
            v1 = v1.replace(/[^0-9]+/g, '');
            $(this).val(commaSeparateNumber(v1));
        }, 0))
        $(window).click(function (e) {
            $('.managerSaleAdd').each(function (item, index) {
                let v = $(this).find('input[name="MinMaxGroup"]').val();
                let checkV = v.replaceAll(',', '').replaceAll('.', '');
                console.log(v);
                let result = checkV.match(/(^[0-9]{1,}-[0-9]{1,}$)|(^[0-9]{1,}$)+/g);
                console.log(result);
                if (result) {
                    $(this).find('.isMinMax').val(v);
                } else {
                    $(this).find('.isMinMax').val('');
                }
            });
        });
        $('#btnSaleManagerUpdate').on('click', function () {
            app.component.Loading.Show();
            let SaleManagerRemunerations = [];
            let SaleManagerKpi6Firsts = [];
            let SaleManagerKpi6Lasts = [];
            let flagManager = true;
            $('.managerSaleAdd').each(function () {
                if ($(this).find('input[class="Remuneration_Admin_Manager"]').val() === '') {
                    toastr.error('Hoa hồng không được trống.', 'Gặp lỗi!');
                    flagManager = false;
                    app.component.Loading.Hide();

                }
                // khanhkk added
                if ($(this).find('input[class="SharePercent_Admin_Manager"]').val() === '') {
                    toastr.error('Phần trăm tính cổ phần không được trống.', 'Gặp lỗi!');
                    flagManager = false;
                    app.component.Loading.Hide();

                }
                // khanhkk added
                if ($(this).find('input[class="Salary_Admin_Manager"]').val() === '') {
                    toastr.error('Lương không được trống.', 'Gặp lỗi!');
                    flagManager = false;
                    app.component.Loading.Hide();

                }
                if ($(this).find('input[name="MinMaxGroup"]').val() === '') {
                    toastr.error('Doanh số không được trống.', 'Gặp lỗi!');
                    flagManager = false;
                    app.component.Loading.Hide();

                }
                SaleManagerRemunerations.push({
                    Percent: $(this).find('input[class="Remuneration_Admin_Manager"]').val().replaceAll(',', ''),
                    //khanhkk added
                    SharePercent: $(this).find('input[class="SharePercent_Admin_Manager"]').val().replaceAll(',', ''),
                    //khanhkk added
                    Salary: $(this).find('input[class="Salary_Admin_Manager"]').val().replaceAll('.', '').replaceAll(',', ''),
                    MinMaxRevenueSM: $(this).find('input[name="MinMaxGroup"]').val().replaceAll('.', '').replaceAll(',', ''),
                    Id: $(this).find('input[name="Id"]').val(),
                    RemunerationId: $(this).find('input[name="RemunerationId"]').val(),
                    CodeRemuneration: $(this).find('input[name="CodeRemuneration"]').val()
                });
            })
            $('.managerSale6F').each(function () {
                SaleManagerKpi6Firsts.push({
                    PercentKpiMin6F: $(this).find('input[name="PercentKpiMin6F"]').val().replaceAll('.', '').replaceAll(',', ''),
                    PercentKpiMax6F: $(this).find('input[name="PercentKpiMax6F"]').val().replaceAll('.', '').replaceAll(',', ''),
                    SalaryPercentLv16F: $(this).find('input[name="SalaryPercentLv16F"]').val().replaceAll('.', '').replaceAll(',', ''),
                });
            })
            $('.managerSale6L').each(function () {
                SaleManagerKpi6Lasts.push({
                    PercentKpiMin6L: $(this).find('input[name="PercentKpiMin6L"]').val().replaceAll('.', '').replaceAll(',', ''),
                    PercentKpiMax6L: $(this).find('input[name="PercentKpiMax6L"]').val().replaceAll('.', '').replaceAll(',', ''),
                    SalaryPercentLv16L: $(this).find('input[name="SalaryPercentLv16L"]').val().replaceAll('.', '').replaceAll(',', ''),
                });
            })
            let model = {
                PercentKpiRoot6F: $('input[name="PercentKpiRoot6F"]').val().replaceAll('.', '').replaceAll(',', ''),
                SalaryPercentRoot6F: $('input[name="SalaryPercentRoot6F"]').val().replaceAll('.', '').replaceAll(',', ''),
                PercentKpiRoot6L: $('input[name="PercentKpiRoot6L"]').val().replaceAll('.', '').replaceAll(',', ''),
                SalaryPercentRoot6L: $('input[name="SalaryPercentRoot6L"]').val(),
                SaleManagerRemunerations : SaleManagerRemunerations,
                SaleManagerKpi6Firsts : SaleManagerKpi6Firsts,
                SaleManagerKpi6Lasts: SaleManagerKpi6Lasts
            }
            if (flagManager) {
                $.ajax({
                    url: '/Admin/SaleManager/Setup',
                    method: 'POST',
                    data: {
                        model: model
                    },
                    success: function (rs) {
                        app.component.Loading.Hide();
                        if (rs.status) {
                            toastr.success('Update thành công.', 'Note!')
                        } else {
                            toastr.error(rs.mess, 'Gặp lỗi!')
                        }
                    }
                })
            }
           
        })
        //$('.isNumber').keyup(delay(function (e) {
        //    let v = $(this).val();
        //    v = v.replace(/[^0-9]+/g, '');
        //    $(this).val(v);
        //}, 0));
        //$('.isMinMax').keyup(delay(function (e) {
        //    let v = $(this).val();
        //    let result = v.match(/(^[0-9]{1,7}-[0-9]{1,7}$)|(^[0-9]{1,7}$)+/g);
        //    if (result) {
        //        $(this).val(v);
        //    } else {
        //        $(this).val('');
        //    }
        //}, 0));
        let count = $('.managerSaleAdd').length;
        //if (count >= 2) {
        //    console.log('xxx');
        //    for (var i = count; i > 0; i--) {
        //        var old = $('.managerSaleAdd')[i].find('input[name="MinMaxOwn"]').val();
        //        var input = $('.managerSaleAdd')[i - 1].find('input[name="MinMaxOwn"]');
        //        input.val(input.val() + "-" + old);
        //    }
        //}
    }
}
$(document).ready(function () {
    saleManager.load();
});