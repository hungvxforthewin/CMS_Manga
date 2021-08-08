let baseUrl = '/Accountant/ConfirmPayment/';

$(function () {
    LoadSales('#search-contract .select-sale');
    LoadTeleSales('#search-contract .select-telesale');
    let elefrm = '#add-contract-investor';

    $('body').on('click', '#btn-show-add', function () {
        $(elefrm).ResetInputs().ClearErrors(null, function () {

        });
        $('#btn-hiid-show').trigger('click');
        LoadSales('#add-contract_modal .select-saleref');
        LoadSales('#add-contract_modal .select-sale');
        LoadTeleSales('#add-contract_modal .select-telesale');
    });

    //View info
    $('body').on('click', 'button.view-info', function () {
        ViewPersonalInfo($(this).data('id'));
    })

    $('body').on('click', 'button.accept-contract', function () {
        $('#myModal1 input').val('');
        let btn = $(this);
        let id = $(btn).data('id');
        if (id === "" || id === undefined) {
            toastr.error('Hợp đồng không tồn tại');
            return;
        }
        $('#id-contract-confirm').val(id);
        $('#id-contract').val(id);
        $('#myBtn1').trigger('click');
        $.ajax({
            type: 'POST',
            url: baseUrl + 'GetInfoPay',
            data: {
                id: id
            },
            success: function (res) {
                if (res.result != 400) {
                    $('.f-amountContract').val(res.result.amountContract.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
                    $('.f-remainAmount').val(res.result.remainAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
                    $('.f-depositAmount').val(res.result.billDeposit.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
                    $('.f-nameInvestor').val(res.result.name);
                    $('.f-phone').val(res.result.phone);
                    $('.f-cmt').val(res.result.cmt);
                    $('.f-stock').val(res.result.amountContract / 50000);
                    $('input[name="CodeManagementCatalog"]').val(res.result.codeManagementCatalog);
                    $('input[name="CodeSupplementAgreement"]').val(res.result.codeSupplementAgreement);
                    $('input[name="CodeTransferAgreement"]').val(res.result.codeTransferAgreement);
                    $('#list-bill-amount').html(``);
                    res.result.listBills.forEach(function (item, index) {
                        $('#list-bill-amount').append(`
                            <div class="right">
                                <label for="fname">Đã thanh toán lần ${index + 2}</label><br>
                                <input type="text" disabled id="fname" name="fname" class="" value="${item.billMoney.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}"><br><br>
                            </div>
                        `);
                    })

                } else {
                    toastr.error('Có lỗi xảy ra', 'Thông báo !');
                }
            }
        });
    })
    $('body').on('click', '.update-code-new-contract', function () {
        $('#myModal1 input').val('');
        let btn = $(this);
        let id = $(btn).data('id');
        if (id === "" || id === undefined) {
            toastr.error('Hợp đồng không tồn tại');
            return;
        }
        $('#id-contract-confirm').val(id);
        $('#id-contract').val(id);
        $('#myBtn1').trigger('click');
        $.ajax({
            type: 'POST',
            url: baseUrl + 'GetInfoPay',
            data: {
                id: id
            },
            success: function (res) {
                if (res.result != 400) {
                    $('.f-amountContract').val(res.result.amountContract.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
                    $('.f-remainAmount').val(res.result.remainAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
                    $('.f-depositAmount').val(res.result.billDeposit.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
                    $('.f-nameInvestor').val(res.result.name);
                    $('.f-phone').val(res.result.phone);
                    $('.f-cmt').val(res.result.cmt);
                    $('.f-stock').val(res.result.amountContract / 50000);
                    $('input[name="CodeManagementCatalog"]').val(res.result.codeManagementCatalog);
                    $('input[name="CodeSupplementAgreement"]').val(res.result.codeSupplementAgreement);
                    $('input[name="CodeTransferAgreement"]').val(res.result.codeTransferAgreement);
                    $('input[name="PayDoneDate"]').val(res.result.datePaydoneString);
                    $('#list-bill-amount').html(``);
                    res.result.listBills.forEach(function (item, index) {
                        $('#list-bill-amount').append(`
                            <div class="right">
                                <label for="fname">Đã thanh toán lần ${index + 2}</label><br>
                                <input type="text" disabled id="fname" name="fname" class="" value="${item.billMoney.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}"><br><br>
                            </div>
                        `);
                    })

                } else {
                    toastr.error('Có lỗi xảy ra', 'Thông báo !');
                }
            }
        });
    })

    $('body').on('click', 'button.preview-contract', function () {
        let btn = $(this);
        let id = $(btn).data('id');
        if (id === "" || id === undefined) {
            toastr.error('Hợp đồng không tồn tại');
            return;
        }
        $('#id-contract-confirm').val(id);
       
        OpenPrintModel(id);
    })

    //$('body').on('click', '#btn-access', function () {
    //    let selectedRow = $('input[name="rb-check"]:checked');
    //    if (selectedRow.length != 1) {
    //        toastr.error('Chọn một hợp đồng ');
    //        $('.loading-wrapper').hide();
    //        return;
    //    }
    //    let id = selectedRow.data('id');
    //    $('#id-contract-confirm').val(id);
    //    $('#myBtn1').trigger('click');
    //    $.ajax({
    //        type: 'POST',
    //        url: baseUrl + 'GetInfoPay',
    //        data: {
    //            id: id
    //        },
    //        success: function (res) {
    //            if (res.result != 400) {
    //                $('.f-amountContract').val(res.result.amountContract.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
    //                $('.f-remainAmount').val(res.result.remainAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
    //                $('.f-depositAmount').val(res.result.billDeposit.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ","));
    //                $('#list-bill-amount').html(``);
    //                res.result.listBills.forEach(function (item, index) {
    //                    $('#list-bill-amount').append(`
    //                        <div class="right">
    //                            <label for="fname">Đã thanh toán lần ${index + 2}</label><br>
    //                            <input type="text" disabled id="fname" name="fname" class="" value="${item.billMoney.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}"><br><br>
    //                        </div>
    //                    `);
    //                })

    //            } else {
    //                toastr.error('Có lỗi xảy ra', 'Thông báo !');
    //            }
    //        }
    //    });
    //})

    $('body').on('click', '#btn-update-confirm', function () {
        let id = $('#id-contract-confirm').val();
        $.ajax({
            type: 'POST',
            url: baseUrl + 'UpdatePayDone',
            data: {
                id: id,
                payDoneDate: $('#myModal1 input[name="PayDoneDate"]').val(),
                createDateContract: $('#myModal1 input[name="CreateDateContract"]').val(),
                codeManagementCatalog: $('#myModal1 input[name="CodeManagementCatalog"]').val(),
                codeSupplementAgreement: $('#myModal1 input[name="CodeSupplementAgreement"]').val(),
                codeTransferAgreement: $('#myModal1 input[name="CodeTransferAgreement"]').val(),
                accountBank2: $('#myModal1 input[name="AccountBank2"]').val()
            },
            success: function (res) {
                if (res.status) {
                    toastr.success('Cập nhật trạng thái thành công', 'Thông báo !');
                    $('#close-confirm').trigger('click');
                    SetupPagination();
                    $('#myModal1 input').val('');
                } else {
                    toastr.error(res.mess, 'Thông báo !');
                }
            }
        });
    })

    //List, Search, Pageing
    $('#btn-search-contract-imp').on('click', function (e) {
        /* e.preventDefault();*/
        app.component.Loading.Show();
        let stt = 1;
        let data = $('#search-form').serializeObject();
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
                    let div = $('#table-body');
                    div.html('');
                    div.html(`
                        `);

                    if (res.data != null) {
                        let newData = res.data.map((item, index) => {
                            if (item.nameInvestor == null || item.nameInvestor == undefined) {
                                item.nameInvestor = 'Không xác định';
                            }
                            if (item.cmt == null || item.cmt == undefined) {
                                item.cmt = 'Không xác định';
                            }
                            if (item.phone == null || item.phone == undefined) {
                                item.phone = 'Không xác định';
                            }
                            if (item.teleSaleName == null || item.teleSaleName == undefined) {
                                item.teleSaleName = 'Không xác định';
                            }
                            return item;
                        });

                        $.each(newData, function (index, item) {
                            div.append(`
                                <tr data-id="${item.id}">
                                    <td>${stt}</td>
                                    <td>
                                        <button data-id="${item.id}" class="preview-contract">
                                            ${item.contractCode}
                                        </button>
                                    </td>
                                    <td>
                                         ${item.nameInvestor}
                                    </td>
                                    <td>${item.investmentAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}</td>
                                    <td>${item.phone}</td>
                                    <td>${item.cmt}</td>
                                    <td>${item.saleName}</td>
                                    <td>${item.teleSaleName}</td>
                                    <td>
                                        ${item.idStatusContract === 'PayDone' ? `<div class="radiotext status update-code-new-contract" data-id="${item.id}"> <label>${item.nameStatus}</label> </div>` : `<div class='radiotext status'>
                                            <button data-id="${item.id}" class="button_lit button2 accept-contract" type="button">
                                                <img src='/Assets/SA/images/accountant_contact-manage/done_all_24px_rounded.svg' alt='' />
                                                <p>Duyệt</p>
                                            </button>
                                        </div>`}
                                    </td>
                                </tr>`);
                            stt = stt + 1;
                        });
                        refresh({
                            total: res.total, // optional
                            length: $('#size-page select').val()// optional
                        });

                        $('#size-page .from').text($('#size-page select').val() * (options.current - 1) + 1);
                        $('#size-page .to').text($('#size-page select').val() * (options.current - 1) + res.data.length);
                        $('#size-page .total').text(res.total);
                        $('.select_pagination').show();
                    }
                    else {
                        toastr.error(res.errors.join('<br />'));
                        $('#table-body').html(`<span>Không có dữ liệu</span>`);
                        $('.select_pagination').hide();
                        $('#size-page .from').text(0);
                        $('#size-page .to').text(0);
                        $('#size-page .total').text(0);
                    }
                    app.component.Loading.Hide();
                }).fail(function (error) {
                    console.log('false');
                    app.component.Loading.Hide();
                });
            }
        });
    })

    SetupPagination();

    $('#size-page select').on('change', function () {
        app.component.Loading.Show();
        SetupPagination();
    });

});

function OpenPrintModel(id) {
    $('.loading-wrapper').show();
    $.ajax({
        url: "/PrintContract/SelectContractTemplate",
        method: 'GET',
        data: {
        },
        success: function (res) {
            $('#select-contract-type_modal .content-modal').html(res);
            $('#select-contract-type_modal select').select2();
            ShowModal('#select-contract-type_modal');
            $('#hidden-contract-id').val(id);
            $('.loading-wrapper').hide();
        }
    }).done(function () {
        
        $('#select-contract-template').click(function () {
            selectPrintTemplate(PrintTab);
        });
    });
}

function PrintTab(printUrl, id) {
    $('.loading-wrapper').show();
    CloseModal('#select-contract-type_modal');
    $.ajax({
        type: 'POST',
        url: printUrl,
        data: {
            id: id
        },
        success: function (res) {
            if (res.result != 400) {
                //$('#detail-contract-show .content-modal').html(res);
                //khanhkk update
                $('#detail-contract-show .content-modal .contact-content').html(res.datahtml);
                $('#detail-contract-show').addClass('show-modal');
                $('#detail-contract-show .content-modal').addClass('show-modal');
            } else {
                toastr.error('Có lỗi xảy ra', 'Thông báo !');
            }
            $('.loading-wrapper').hide();
        }
    });
}

var LoadSales = function (target, selected) {
    $(target).html('');
    $(target).append(`<option value="">-Chọn sale-</option>`);
    $.get(baseUrl + 'GetSale', function (res) {
        if (res.result == 200) {
            $.each(res.data, function (index, item) {
                $(target).append(`<option value="${item.codeStaff}">${item.fullName}</option>`)
            });

            if (selected) {
                $(target).val(selected);
                $(target).trigger('change');
            }
        }
        else {
            //toastr.error(res.errors.join('<br />'));
        }
    });
}

var LoadTeleSales = function (target, selected) {
    $(target).html('');
    $(target).append(`<option value="">-Chọn Telesale-</option>`);
    $.get(baseUrl + 'GetTeleSale', function (res) {
        if (res.result == 200) {
            $.each(res.data, function (index, item) {
                $(target).append(`<option value="${item.codeStaff}">${item.fullName}</option>`)
            });

            if (selected) {
                $(target).val(selected);
                $(target).trigger('change');
            }
        }
        else {
            //toastr.error(res.errors.join('<br />'));
        }
    });
}

var LoadStatus = function (target, selected) {
    $(target).html('');
    $(target).append(`<option value="">-Chọn Trạng thái-</option>`);
    $.get(baseUrl + 'GetContractStaffStatus', function (res) {
        if (res.status == true) {
            $.each(res.data, function (index, item) {
                $(target).append(`<option value="${item.key}">${item.value}</option>`)
            });

            if (selected) {
                $(target).val(selected);
                $(target).trigger('change');
            }
        }
        else {
            //toastr.error(res.errors.join('<br />'));
        }
    });
}

//View Detail
var LoadSalesView = function (target, selected) {
    var el = $(target).find('.select-sale');
    el.html('');
    el.append(`<option value="">-Chọn sale-</option>`);
    $.get(baseUrl + 'GetSale', function (res) {
        if (res.result == 200) {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.codeStaff}">${item.fullName}</option>`)
            });

            if (selected) {
                el.val(selected);
                el.trigger('change');
            }
        }
        else {
            //toastr.error(res.errors.join('<br />'));
        }
    });
}

var LoadTeleSalesView = function (target, selected) {
    var el = $(target).find('.select-telesale');
    el.html('');
    el.append(`<option value="">-Chọn Telesale-</option>`);
    $.get(baseUrl + 'GetTeleSale', function (res) {
        if (res.result == 200) {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.codeStaff}">${item.fullName}</option>`)
            });

            if (selected) {
                el.val(selected);
                el.trigger('change');
            }
        }
        else {
            //toastr.error(res.errors.join('<br />'));
        }
    });
}

var LoadSalesRefView = function (target, selected) {
    var el = $(target).find('.select-saleref');
    el.html('');
    el.append(`<option value="">-Chọn sale-</option>`);
    $.get(baseUrl + 'GetSale', function (res) {
        if (res.result == 200) {
            $.each(res.data, function (index, item) {
                el.append(`<option value="${item.codeStaff}">${item.fullName}</option>`)
            });

            if (selected) {
                el.val(selected);
                el.trigger('change');
            }
        }
        else {
            //toastr.error(res.errors.join('<br />'));
        }
    });
}

var SetupPagination = function () {
    app.component.Loading.Show();
    let stt = 1;
    $('#pagination').pagination({
        ajax: function (options, refresh, $target) {
            $.ajax({
                url: baseUrl + "GetList",
                data: {
                    page: options.current,
                    size: $('#size-page select').val(),
                    key: $('#search-key').val(),
                    position: $('.select-position').val(),
                    branch: $('#search-form select#select-branch').val(),
                    department: $('.select-dept').val(),
                    team: $('.select-team').val(),
                    status: $('.select-status').val()
                },
                method: 'POST',
                dataType: 'json'
            }).done(function (res) {
                let div = $('#table-body');
                div.html('');
                div.html(``);

                if (res.data != null) {
                    let newData = res.data.map((item, index) => {
                        if (item.nameInvestor == null || item.nameInvestor == undefined) {
                            item.nameInvestor = 'Không xác định';
                        }
                        if (item.cmt == null || item.cmt == undefined) {
                            item.cmt = 'Không xác định';
                        }
                        if (item.phone == null || item.phone == undefined) {
                            item.phone = 'Không xác định';
                        }
                        if (item.teleSaleName == null || item.teleSaleName == undefined) {
                            item.teleSaleName = 'Không xác định';
                        }
                        return item;
                    });

                    $.each(newData, function (index, item) {
                        div.append(`
                                <tr data-id="${item.id}">
                                    <td>${stt}</td>
                                    <td>
                                         <button data-id="${item.id}" class="preview-contract">
                                            ${item.contractCode}
                                        </button>
                                    </td>
                                    <td>
                                      ${item.nameInvestor}
                                    </td>
                                    <td>${item.investmentAmount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")}</td>
                                    <td>${item.phone}</td>
                                    <td>${item.cmt}</td>
                                    <td>${item.saleName}</td>
                                    <td>${item.teleSaleName}</td>
                                    <td>
                                        ${item.idStatusContract === 'PayDone' ? `<div class="radiotext status update-code-new-contract" data-id="${item.id}"> <label>${item.nameStatus}</label> </div>` : `<div class='radiotext status'>
                                            <button data-id="${item.id}" class="button_lit button2 accept-contract" type="button">
                                                <img src='/Assets/SA/images/accountant_contact-manage/done_all_24px_rounded.svg' alt='' />
                                                <p>Duyệt</p>
                                            </button>
                                        </div>`}
                                    </td>
                                </tr>`);
                        stt = stt + 1;
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
            //modal-show="show"
            //modal-data="#detail-customer-contract_modal"
        }
    });
}

var ViewPersonalInfo = function (id) {
    $.get(baseUrl + "View?id=" + id, function (res) {
        if (res.result == 400) {
            toastr.error(res.errors.join('<br />'));
        }
        else {
            $('#detail-customer-contract_modal .content-modal').html(res);
            $('#detail-customer-contract_modal').addClass('show-modal');
            $('#detail-customer-contract_modal .content-modal').addClass('show-modal');
        }
    }).done(function () {
        //let selectedBranch = $('#detail-employee_modal .hidden-branch').val();
        //if (selectedBranch) {
        //    LoadBranchesToForm('#detail-employee_modal', selectedBranch);

        //    let selectedDept = $('#detail-employee_modal .hidden-dept').val();
        //    LoadDeptsToForm('#detail-employee_modal', selectedBranch, selectedDept);

        //    let selectedTeam = $('#detail-employee_modal .hidden-team').val();
        //    LoadTeamsToForm('#detail-employee_modal', selectedDept, selectedTeam);

        //    $('#view-info').trigger('click');
        //}

        let selectedSale = $('#detail-customer-contract_modal .hidden-sale').val();
        LoadSalesView('#detail-customer-contract_modal', selectedSale);

        let selectedSaleRep = $('#detail-customer-contract_modal .hidden-saleRep').val();
        LoadSalesRefView('#detail-customer-contract_modal', selectedSaleRep);

        let selectedTeleSale = $('#detail-customer-contract_modal .hidden-telesale').val();
        LoadTeleSalesView('#detail-customer-contract_modal', selectedTeleSale);

        $('#detail-customer-contract_modal select').select2();
        $(".form_datetime").datetimepicker(
            {
                format: 'dd/mm/yyyy',
                minView: 2,
                maxView: 4,
                autoclose: true
            });

        $('#detail-customer-contract_modal input').attr('disabled', 'disabled');
        $('#detail-customer-contract_modal select').attr('disabled', 'disabled');
        /*$('#view-edit').hide();*/
    });
}

var PreviewViewPersonalInfo = function (id) {
    $.get(baseUrl + "Preview?id=" + id, function (res) {
        if (res.result == 400) {
            toastr.error(res.errors.join('<br />'));
        }
        else {
            $('#contract-preview_modal .content-modal').html(res);
            $('#contract-preview_modal').addClass('show-modal');
            $('#contract-preview_modal .content-modal').addClass('show-modal');
        }
    }).done(function () {
        //let selectedBranch = $('#detail-employee_modal .hidden-branch').val();
        //if (selectedBranch) {
        //    LoadBranchesToForm('#detail-employee_modal', selectedBranch);

        //    let selectedDept = $('#detail-employee_modal .hidden-dept').val();
        //    LoadDeptsToForm('#detail-employee_modal', selectedBranch, selectedDept);

        //    let selectedTeam = $('#detail-employee_modal .hidden-team').val();
        //    LoadTeamsToForm('#detail-employee_modal', selectedDept, selectedTeam);

        //    $('#view-info').trigger('click');
        //}

        let selectedSale = $('#contract-preview_modal .hidden-sale').val();
        LoadSalesView('#contract-preview_modal', selectedSale);

        let selectedSaleRep = $('#contract-preview_modal .hidden-saleRep').val();
        LoadSalesRefView('#contract-preview_modal', selectedSaleRep);

        let selectedTeleSale = $('#contract-preview_modal .hidden-telesale').val();
        LoadTeleSalesView('#contract-preview_modal', selectedTeleSale);

        $('#contract-preview_modal select').select2();
        $(".form_datetime").datetimepicker(
            {
                format: 'dd/mm/yyyy',
                minView: 2,
                maxView: 4,
                autoclose: true
            });

        $('#contract-preview_modal input').attr('disabled', 'disabled');
        $('#contract-preview_modal select').attr('disabled', 'disabled');
        /*$('#view-edit').hide();*/
    });
}