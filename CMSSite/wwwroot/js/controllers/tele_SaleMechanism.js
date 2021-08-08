let baseUrl = '/Admin/Mechanism/';

function openCity(evt, cityName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
}

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
function commaSeparateNumber(val) {
    while (/(\d+)(\d{3})/.test(val.toString())) {
        val = val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
    }
    return val;
}

$(function () {
    $('.loading-wrapper').show();
    //$('#Sale').show();
    $.get(baseUrl + 'GetTeleSaleMechanism', function (res) {
        $('#tab1').html(res);
    });

    $.get(baseUrl + 'GetSaleMechanism', function (res) {
        $('#tab3').html(res);
    });

    $.get(baseUrl + 'GetCTVMechanism', function (res) {
        $('#tab6').html(res);
        $('#tab6 input').attr('disabled', 'disabled');
    });

    $.get(baseUrl + 'GetMinisterMechanism', function (res) {
        $('#tab7').html(res);
    });

    setTimeout(function () {
        $('.loading-wrapper').hide();
    }, 1500);
    $(document).on('keyup', '.isNumber', delayAdmin(function (e) {
        let v = $(this).val();
        v = v.replace(/[^0-9]+/g, '');
        $(this).val(numberFormartAdmin(v));
    }, 0));
});

var Cancel = function () {
    location.reload();
}

var RemoveLevelForTele = function (e) {
    $(e).parents('div.table-item').remove();
}

var RemoveKpiForSale = function (e) {
    $(e).closest('tr').remove();
}

var RemoveSalaryForSale = function (e) {
    let table = $(e).closest('table');
    $(e).closest('tr').remove();

    table.find('td.label-level').each(function (index, item) {
        $(item).text('Level ' + (index + 1));
    });
}

var AddLevelForTele = function (e) {
    $(e).before(
        `<div class="table-item">
            <img onclick="RemoveLevelForTele(this)" src="/Assets/crm/images/mechanism/delete.svg"
                    alt="" />
            <div class="table-item_row first_row">
                <input type="text" name="PercentKpiRange" placeholder="Min-Max" />
            </div>
            <div class="table-item_row">
                <input type="text" class="isNumber" name="AmountShowupTele" />
            </div>
            <div class="table-item_row">
                <input type="text" class="isNumber" name="AmountContractTele" />
            </div>
        </div>`
    );
}

var AddKpiForSale = function (e) {
    $(e).closest('tr').before(
        `
<tr>
            <td>
                <input type="text" class="isPercent" name="MinKpiPercent" min="0" value="" placeholder="Min" />
            </td>
            <td>
                <input type="text" class="isPercent" name="MaxKpiPercent" min="0" value="" placeholder="Max" />
            </td>
            <td>
                <input type="number" class="isPercent" name="SalaryPercent" min="0" value="" />
            </td>
            <td>
                <img onclick="RemoveKpiForSale(this)" src="/Assets/crm/images/mechanism/delete.svg"
                        alt="" />
            </td>
</tr>
                `
    );
}

var AddSalaryForSale = function (e) {
    let rows = $(e).closest('table').find('tr.value');
    $(e).closest('tr').before(
        `
            <tr class="value">
                <td class="label-level">Level ${rows.length + 1}</td>
               <td>
                    <input type="number" class="isPercent" name="PercentRemuneration" min="0" value="" />
                </td>
                <td>
                    <input type="number" class="isPercent" name="CalculatingSharePercent" min="0" value="" />
                </td>
                <td>
                    <input type="text" class="isNumber" name="Salary" value="" />
                </td>
                <td class="resize">
                    <input type="text" class="revenue-range" name="RevenueRange" value="" />
                </td>
                <td>
                    <img onclick="RemoveSalaryForSale(this)" src="/Assets/crm/images/mechanism/delete.svg"
                            alt="" />
                </td>
            </tr>
            `
    );
}

function AddSalaryForMinister(e) {
    let rows = $(e).closest('table').find('tr.adminSaleAdd');
    
    $(e).before(
        `
                   <tr class="adminSaleAdd">
                       <td class="label-level">Level ${rows.length + 1}</td>
                        <td>
                            <div>
                                <input type="text" class="Remuneration_Admin_Sale" />
                            </div>
                        </td>
                        <td>
                            <div>
                                <input type="text" class="isPercent SharePercent_Admin_Sale" />
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

var SetupMechanismForTele = function () {
    $('.loading-wrapper').show();
    var Model = {};
    ////////////////////////////////TELE SALE
    let teleData = $('#tab1').find('#tele-sale').find('form').serializeArray();
    //console.log(teleData);

    //common info
    var TeleSaleModel = {
        Common: {
            //RoleAccount: 8,
            Salary: teleData.find(({name}) => name === 'Salary').value.replaceAll(',', ''),
            ProbationaryTime: teleData.find(({ name }) => name === 'ProbationaryTime').value,
            ProbationarySalary: teleData.find(({ name }) => name === 'ProbationarySalary').value.replaceAll(',', ''),
            Id: teleData.find(({ name }) => name === 'Id').value,
            Kpi: teleData.find(({ name }) => name === 'TotalShowUp').value,
            CodeKpi: teleData.find(({ name }) => name === 'CodeKpi').value
        },
        ProbationaryCondition: {
            KpiPercent: teleData.find(({ name }) => name === 'RatioKpiMax').value,
            SalaryPercent: teleData.find(({ name }) => name === 'PercentSalary').value,
        }
    };

    //kpi remuneration
    var RemunerationList = [];
    for (let i = 8; i < teleData.length - 1; i = i + 3) {
        var remuneration = {
            PercentRange: teleData[i].value,
            ShowRemuneration: teleData[i + 1].value.replaceAll(',', ''),
            ContractRemuneration: teleData[i + 2].value.replaceAll(',', ''),
            //Id: teleData[i + 3].value,
        };
        RemunerationList.push(remuneration);
    }
    TeleSaleModel.Remunerations = RemunerationList;
    Model.TeleSaleMachanism = TeleSaleModel;

    ////////////////////////////////LEADER TELE
    let LeaderTeleData = $('#tab1').find('#leader-tele').find('form').serializeArray();
    //console.log(LeaderTeleData);

    //common info
    var LeaderTeleSaleModel = {
        Common: {
            //RoleAccount: 8,
            Salary: LeaderTeleData.find(({ name }) => name === 'Salary').value.replaceAll(',', ''),
            ProbationaryTime: LeaderTeleData.find(({ name }) => name === 'ProbationaryTime').value,
            ProbationarySalary: LeaderTeleData.find(({ name }) => name === 'ProbationarySalary').value.replaceAll(',', ''),
            Id: LeaderTeleData.find(({ name }) => name === 'Id').value,
            Kpi: LeaderTeleData.find(({ name }) => name === 'TotalShowUp').value,
            CodeKpi: LeaderTeleData.find(({ name }) => name === 'CodeKpi').value
        },
        ProbationaryCondition: {
            KpiPercent: LeaderTeleData.find(({ name }) => name === 'RatioKpiMax').value,
            SalaryPercent: LeaderTeleData.find(({ name }) => name === 'PercentSalary').value,
        }
    };

    //kpi remuneration
    var RemunerationList = [];
    for (let i = 8; i < LeaderTeleData.length - 1; i = i + 3) {
        var remuneration = {
            PercentRange: LeaderTeleData[i].value,
            ShowRemuneration: LeaderTeleData[i + 1].value.replaceAll(',', ''),
            ContractRemuneration: LeaderTeleData[i + 2].value.replaceAll(',', ''),
            //Id: LeaderTeleData[i + 3].value,
        };
        RemunerationList.push(remuneration);
    }
    LeaderTeleSaleModel.Remunerations = RemunerationList;
    Model.LeaderTeleSaleMachanism = LeaderTeleSaleModel;

    $.ajax({
        type: "POST",
        url: baseUrl + 'TeleSetup',
        data: { model: Model },
        success: function (res) {
            if (res.result == 400) {
                toastr.error(res.errors.join('<br />'));
            }
            else {
                toastr.success(res.message);
                location.reload();
            }
            $('.loading-wrapper').hide();
        }
    });
}

var SetupMechanismForSale = function () {
    $('.loading-wrapper').show();
    var Model = {};

    ////////////////////////////////SALE
    //common info
    var SaleModel = {
        Id: $('#sale .sale-id').val(),
        CodeKpi: $('#sale .sale-kpi-code').val(),
    };

    //
    let FistMonthsConditionData = $('#tab3').find('#sale .kpi .kpi_left').find('form.condition').serializeArray();
    //console.log(FistMonthsConditionData);

    let firstMonthsCondition = {
        KpiPercent: FistMonthsConditionData.find(({ name }) => name === 'KpiPercent').value,
        SalaryPercent: FistMonthsConditionData.find(({ name }) => name === 'SalaryPercent').value,
    }
    SaleModel.FirstMonthsCondition = firstMonthsCondition;

    //
    let LaterMonthsConditionData = $('#tab3').find('#sale .kpi .kpi_right').find('form.condition').serializeArray();
    //console.log(LaterMonthsConditionData);

    let laterMonthsCondition = {
        KpiPercent: LaterMonthsConditionData.find(({ name }) => name === 'KpiPercent').value,
        SalaryPercent: LaterMonthsConditionData.find(({ name }) => name === 'SalaryPercent').value,
    }
    SaleModel.LaterMonthsCondition = laterMonthsCondition;

    //
    let FirstMonthsSalaryData = $('#tab3').find('#sale .kpi .kpi_left').find('form.kpi-salary-rules').serializeArray();
    //console.log(FirstMonthsSalaryData);
    var firstMonthsSalaryList = [];
    for (let i = 0; i < FirstMonthsSalaryData.length; i = i + 3) {
        var kpi = {
            MinKpiPercent: FirstMonthsSalaryData[i].value,
            MaxKpiPercent: FirstMonthsSalaryData[i + 1].value,
            SalaryPercent: FirstMonthsSalaryData[i + 2].value,
        };
        firstMonthsSalaryList.push(kpi);
    }
    SaleModel.FirstMonthsSalary = firstMonthsSalaryList;

    //
    let LaterMonthsSalaryData = $('#tab3').find('#sale .kpi .kpi_right').find('form.kpi-salary-rules').serializeArray();
    //console.log(LaterMonthsSalaryData);
    var laterMonthsSalaryList = [];
    for (let i = 0; i < LaterMonthsSalaryData.length; i = i + 3) {
        var kpi = {
            MinKpiPercent: LaterMonthsSalaryData[i].value,
            MaxKpiPercent: LaterMonthsSalaryData[i + 1].value,
            SalaryPercent: LaterMonthsSalaryData[i + 2].value,
        };
        laterMonthsSalaryList.push(kpi);
    }
    SaleModel.LaterMonthsSalary = laterMonthsSalaryList;

    let remunerationList = $('#tab3').find('.sale').find('form.remunerations').serializeArray();
    //console.log(remunerationList);
    var remunerations = [];
    for (let i = 0; i < remunerationList.length; i = i + 4) {
        var remu = {
            PercentRemuneration: remunerationList[i].value,
            CalculatingSharePercent: remunerationList[i+1].value,
            Salary: remunerationList[i + 2].value.replaceAll(',', ''),
            RevenueRange: remunerationList[i + 3].value.replaceAll(',', ''),
        };
        remunerations.push(remu);
    }
    SaleModel.Remunerations = remunerations;
    Model.SaleMechanism = SaleModel;

    ////////////////////////////////LEADER SALE
    //common info
    var LeaderSaleModel = {
        Id: $('#leader-sale .leader-sale-id').val(),
        CodeKpi: $('#leader-sale .leader-kpi-code').val(),
    };

    //
    let LeaderFistMonthsConditionData = $('#tab3').find('#leader-sale .kpi .kpi_left').find('form.condition').serializeArray();
    //console.log(LeaderFistMonthsConditionData);

    let leaderFirstMonthsCondition = {
        KpiPercent: LeaderFistMonthsConditionData.find(({ name }) => name === 'KpiPercent').value,
        SalaryPercent: LeaderFistMonthsConditionData.find(({ name }) => name === 'SalaryPercent').value,
    }
    LeaderSaleModel.FirstMonthsCondition = leaderFirstMonthsCondition;

    //
    let LeaderLaterMonthsConditionData = $('#tab3').find('#leader-sale .kpi .kpi_right').find('form.condition').serializeArray();
    //console.log(LeaderLaterMonthsConditionData);

    let LeaderLaterMonthsCondition = {
        KpiPercent: LeaderLaterMonthsConditionData.find(({ name }) => name === 'KpiPercent').value,
        SalaryPercent: LeaderLaterMonthsConditionData.find(({ name }) => name === 'SalaryPercent').value,
    }
    LeaderSaleModel.LaterMonthsCondition = LeaderLaterMonthsCondition;

    //
    let LeaderFirstMonthsSalaryData = $('#tab3').find('#leader-sale .kpi .kpi_left').find('form.kpi-salary-rules').serializeArray();
    //console.log(LeaderFirstMonthsSalaryData);
    var leaderFirstMonthsSalaryList = [];
    for (let i = 0; i < LeaderFirstMonthsSalaryData.length; i = i + 3) {
        var kpi = {
            MinKpiPercent: LeaderFirstMonthsSalaryData[i].value,
            MaxKpiPercent: LeaderFirstMonthsSalaryData[i + 1].value,
            SalaryPercent: LeaderFirstMonthsSalaryData[i + 2].value,
        };
        leaderFirstMonthsSalaryList.push(kpi);
    }
    LeaderSaleModel.FirstMonthsSalary = leaderFirstMonthsSalaryList;

    //
    let LeaderLaterMonthsSalaryData = $('#tab3').find('#leader-sale .kpi .kpi_right').find('form.kpi-salary-rules').serializeArray();
    //console.log(LeaderLaterMonthsSalaryData);
    var leaderLaterMonthsSalaryList = [];
    for (let i = 0; i < LeaderLaterMonthsSalaryData.length; i = i + 3) {
        var kpi = {
            MinKpiPercent: LeaderLaterMonthsSalaryData[i].value,
            MaxKpiPercent: LeaderLaterMonthsSalaryData[i + 1].value,
            SalaryPercent: LeaderLaterMonthsSalaryData[i + 2].value,
        };
        leaderLaterMonthsSalaryList.push(kpi);
    }
    LeaderSaleModel.LaterMonthsSalary = leaderLaterMonthsSalaryList;

    let LeaderremunerationList = $('#tab3').find('.leader-sale').find('form.remunerations').serializeArray();
    //console.log(LeaderremunerationList);
    var leaderRemunerations = [];
    for (let i = 0; i < LeaderremunerationList.length; i = i + 4) {
        var remu = {
            PercentRemuneration: LeaderremunerationList[i].value,
            CalculatingSharePercent: LeaderremunerationList[i + 1].value,
            Salary: LeaderremunerationList[i + 2].value.replaceAll(',', ''),
            RevenueRange: LeaderremunerationList[i + 3].value.replaceAll(',', ''),
        };
        leaderRemunerations.push(remu);
    }
    LeaderSaleModel.Remunerations = leaderRemunerations;
    Model.LeaderSaleMechanism = LeaderSaleModel;

    $.ajax({
        type: "POST",
        url: baseUrl + 'SaleSetup',
        data: { model: Model },
        success: function (res) {
            $('#error-list').html('');
            if (res.result == 400) {
                toastr.error(res.errors.join('<br />'));
            }
            else {
                toastr.success(res.message);
                $.get(baseUrl + 'GetCTVMechanism', function (res) {
                    $('#tab6').html(res);
                    $('#tab6 input').attr('disabled', 'disabled');
                });
            }
            $('.loading-wrapper').hide();
        }
    });
}

var SetupMechanismForMinister = function () {
    $('.loading-wrapper').show();
    var Model = {};

    ////////////////////////////////SALE
    //common info
    var MinisterModel = {
        Id: $('#tab7 .sale-id').val(),
        CodeKpi: $('#tab7 .sale-kpi-code').val(),
    };

    //
    let FistMonthsConditionData = $('#tab7').find('#sale .kpi .kpi_left').find('form.condition').serializeArray();
    //console.log(FistMonthsConditionData);

    let firstMonthsCondition = {
        KpiPercent: FistMonthsConditionData.find(({ name }) => name === 'KpiPercent').value,
        SalaryPercent: FistMonthsConditionData.find(({ name }) => name === 'SalaryPercent').value,
    }
    MinisterModel.FirstMonthsCondition = firstMonthsCondition;

    //
    let LaterMonthsConditionData = $('#tab7').find('#sale .kpi .kpi_right').find('form.condition').serializeArray();
    //console.log(LaterMonthsConditionData);

    let laterMonthsCondition = {
        KpiPercent: LaterMonthsConditionData.find(({ name }) => name === 'KpiPercent').value,
        SalaryPercent: LaterMonthsConditionData.find(({ name }) => name === 'SalaryPercent').value,
    }
    MinisterModel.LaterMonthsCondition = laterMonthsCondition;

    //
    let FirstMonthsSalaryData = $('#tab7').find('#sale .kpi .kpi_left').find('form.kpi-salary-rules').serializeArray();
    //console.log(FirstMonthsSalaryData);
    var firstMonthsSalaryList = [];
    for (let i = 0; i < FirstMonthsSalaryData.length; i = i + 3) {
        var kpi = {
            MinKpiPercent: FirstMonthsSalaryData[i].value,
            MaxKpiPercent: FirstMonthsSalaryData[i + 1].value,
            SalaryPercent: FirstMonthsSalaryData[i + 2].value,
        };
        firstMonthsSalaryList.push(kpi);
    }
    MinisterModel.FirstMonthsSalary = firstMonthsSalaryList;

    //
    let LaterMonthsSalaryData = $('#tab7').find('#sale .kpi .kpi_right').find('form.kpi-salary-rules').serializeArray();
    //console.log(LaterMonthsSalaryData);
    var laterMonthsSalaryList = [];
    for (let i = 0; i < LaterMonthsSalaryData.length; i = i + 3) {
        var kpi = {
            MinKpiPercent: LaterMonthsSalaryData[i].value,
            MaxKpiPercent: LaterMonthsSalaryData[i + 1].value,
            SalaryPercent: LaterMonthsSalaryData[i + 2].value,
        };
        laterMonthsSalaryList.push(kpi);
    }
    MinisterModel.LaterMonthsSalary = laterMonthsSalaryList;

    let remunerationList = $('#tab7').find('.sale').find('form.remunerations').serializeArray();
    //console.log(remunerationList);
    var remunerations = [];
    for (let i = 0; i < remunerationList.length; i = i + 4) {
        var remu = {
            PercentRemuneration: remunerationList[i].value,
            CalculatingSharePercent: remunerationList[i + 1].value,
            Salary: remunerationList[i + 2].value.replaceAll(',', ''),
            RevenueRange: remunerationList[i + 3].value.replaceAll(',', ''),
        };
        remunerations.push(remu);
    }
    MinisterModel.Remunerations = remunerations;
    Model.MinisterMechanism = MinisterModel;

    $.ajax({
        type: "POST",
        url: baseUrl + 'MinisterSetup',
        data: { model: Model },
        success: function (res) {
            $('#error-list').html('');
            if (res.result == 400) {
                toastr.error(res.errors.join('<br />'));
            }
            else {
                toastr.success(res.message);
            }
            $('.loading-wrapper').hide();
        }
    });
}