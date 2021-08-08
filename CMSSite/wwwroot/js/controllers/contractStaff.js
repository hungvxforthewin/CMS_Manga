let baseUrl = '/SaleAdmin/ContractStaffs/';

$(function () {
    LoadResources('#search-contracStaff .select-resource');
    LoadSales('#search-contracStaff .select-sale');
    LoadTeleSales('#search-contracStaff .select-telesale');
    LoadStatus('#search-contracStaff .select-status');
    LoadSales('#add-contract_modal .select-saleref');
    LoadSales('#add-contract_modal .select-sale');
    LoadTeleSales('#add-contract_modal .select-telesale');
    let elefrm = '#add-contract-investor';
    $(elefrm).SetupSerializeJson({
        pattern: 'auto', url: baseUrl + "getmv", containerid: '#error_message', tokenid: '#tokenid'
    });
    $('body').on('click', '#btn-show-add', function () {
        $(elefrm).ResetInputs().ClearErrors(null, function () {

        });
        let elefrmAdd = '#add-contract-investor';
        $(elefrmAdd).SetupSerializeJson({
            pattern: 'auto', url: baseUrl + "getmv", containerid: '#error_message', tokenid: '#tokenid'
        });
        $('#btn-hiid-show').trigger('click');
        $('#add-contract_step-4 .bs-row').remove();
        LoadSales('#add-contract_modal .select-saleref');
        LoadSales('#add-contract_modal .select-sale');
        LoadTeleSales('#add-contract_modal .select-telesale');
        $("#add-contract_modal .id-type").val(0);
        const dataId2 = $("#add-contract_modal .id-type").val();
        const addressSelect2 = $(`#add-contract_modal .address-select`);
        for (let i = 0; i < addressSelect2.length; i++) {
            let s = addressSelect2[i]
            if ($(s).attr('data-id') === dataId2) {
                $(s).addClass('active');
            }
            else if ($(s).hasClass('active')) {
                $(s).removeClass('active')
            }
        }
    });
    $('body').on('click', '#btn-addContract-close', function () {
        /*$('.close__modal').trigger('click');*/
        let id = $(this).attr('data-id');
        /*$('#btn-hiid-show-preview').trigger('click');*/
        PreviewViewPersonalInfo(id);
        //$('#contract-preview_modal').addClass('show-modal');
        //$('#contract-preview_modal .content-modal').addClass('show-modal');
    });
    //$('body').on('keyup', '#checkEmail', function () {
    //    let email = $('#checkEmail').val();
    //    _AjaxPostForm(baseUrl + "CheckEmailInvestor", { email: email }, function (res) {
    //        console.log(res);
    //    });
    //});

    var typingTimer;                //timer identifier
    var doneTypingInterval = 3000;  //time in ms, 5 second for example
    var $input = $('#checkPhone');

    //on keyup, start the countdown
    $input.on('keyup', function () {
        clearTimeout(typingTimer);
        typingTimer = setTimeout(doneTyping, doneTypingInterval);
    });

    //on keydown, clear the countdown 
    $input.on('keydown', function () {
        clearTimeout(typingTimer);
    });

    //user is "finished typing," do something
    function doneTyping() {
        app.component.Loading.Show();
        let phone = $('#checkPhone').val();
        _AjaxPostForm(baseUrl + "CheckPhoneInvestor", { phone: phone }, function (res) {
            app.component.Loading.Hide();
            if (res.status) {
                /*toastr.success('Khách hàng đã tồn tại', 'Thông báo');*/
                $('#add-contract_modal').find('input[name = "idCard"]').val(res.data.idCard);
                if (res.data.isCMT == 1) {
                    $('#add-contract_modal').find('input[name = "addressIssuance"]').val(res.data.addressIssuance);
                    $('#add-contract_modal .id-type').val(0).change();
                } else {
                    $('#add-contract_modal .id-type').val(1).change();
                    $('#add-contract_modal select[name="AddressIssuanceCC"]').val(+res.data.addressIssuance).change();
                }

                $('#add-contract_modal').find('input[name = "email"]').val(res.data.email);
                $('#add-contract_modal').find('input[name = "birthday"]').val(res.data.birthdayString);
                $('#add-contract_modal').find('input[name = "dateOfIssuance"]').val(res.data.dateOfIssuanceString);
                $('#add-contract_modal').find('input[name = "idInvestor"]').val(res.data.id);
                $('#add-contract_modal').find('input[name = "name"]').val(res.data.name);
                $('#add-contract_modal').find('input[name = "personalTaxCode"]').val(res.data.personalTaxCode);
                $('#add-contract_modal').find('input[name = "accountBank"]').val(res.data.accountBank);
                $('#add-contract_modal').find('input[name = "bank"]').val(res.data.bank);
                $('#add-contract_modal').find('input[name = "address"]').val(res.data.address);
                $('#add-contract_modal').find('input[name = "codeDeposit"]').val(res.data.codeDeposit);
                $('#add-contract_modal').find('input[name = "depositAmount"]').val((+res.data.sumPaymentAmount).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
            }
        });
    }
    // Add
    $('body').on('click', '#btn-addContract-done', function () {
        //REMOVE DATA

        var dictprefix = {
            name: "Họ và tên",
            email: "Email",
            birthday: "Ngày sinh",
            idCard: "Chứng minh thư",
            dateOfIssuance: "Ngày cấp",
            addressIssuance: "Nơi cấp",
            phone: "Số điện thoại",
            address: "Địa chỉ",
            accountBank: "Số tài khoản",
            codeDeposit: "Số phiếu đặt cọc",
            depositAmount: "Số tiền đặt cọc",
            depositDate: "Ngày đặt cọc",
            depositForm: "Hình thức đặt cọc",
            /*codeContract: "Mã hợp đồng",*/
            investmentAmount: "Gía trị HĐ",
            createDate: "Ngày vào hợp đồng",
            sale: "Sale",
            //teleSale: "TeleSale",
            saleRep: "Sale chốt hộ",
            //THÔNG TIN THANH TOÁN
            billMoney: "Thông tin thanh toán",
            dateBill: "Ngày thanh toán",
            formBill: "Hình thức thanh toán",
            /*stock: "Số cổ phần",*/
            personalTaxCode: "Mã số thuế cá nhân"
        };
        $('#add-contract-investor').Validate({ texterrors: dictprefix, errtype: 2 }, function (errs) {
            if (errs.length > 0) {
                toastr.error('Có lỗi xảy ra', 'Thông báo!');
                //LoadSales('#add-contract_modal .select-saleref');
                //LoadSales('#add-contract_modal .select-sale');
                //LoadTeleSales('#add-contract_modal .select-telesale');
            } else {
                let model = $('#add-contract-investor').serializeObject();
                let infoBill = [];
                $('#add-contract_step-4 .bs-row').each(function () {
                    infoBill.push(
                        {
                            BillMoney: $(this).find('input[name="billMoney"]').val(),
                            DateBill: $(this).find('input[name="dateBill"]').val(),
                            FormBill: $(this).find('select[name="formBill"]').val()
                        }
                    );
                });
                model.ListInforBill = infoBill;
                let data = model;
                _AjaxPostForm(baseUrl + "InsertOrUpdate", data, function (res) {
                    $(elefrm).ClearErrors(null, function () {

                    });
                    if (res.status) {
                        toastr.success('Tạo hợp đồng thành công', 'Thông báo');
                        $('.close__modal').trigger('click');
                        SetupPagination();
                    } else {
                        $(elefrm).ShowError(res.data, { errtype: 2 }, function (error) {

                        }, function (errs) {
                            errs.forEach(function (item, index) {
                                toastr.error(item.ErrorMessage, 'Thông báo');
                            })
                        });
                    }
                });
            }
        })

    });
    var typingTimerEmber;                //timer identifier
    var doneTypingIntervalEmber = 3000;  //time in ms, 5 second for example
    var $inputEmber = $('#taxCodeIntermediaries');

    //on keyup, start the countdown
    $inputEmber.on('keyup', function () {
        clearTimeout(typingTimerEmber);
        typingTimerEmber = setTimeout(doneTypingEmber, doneTypingIntervalEmber);
    });

    //on keydown, clear the countdown 
    $inputEmber.on('keydown', function () {
        clearTimeout(typingTimerEmber);
    });

    //user is "finished typing," do something
    function doneTypingEmber() {
        app.component.Loading.Show();
        let taxCode = $('#taxCodeIntermediaries').val();
        _AjaxPostForm(baseUrl + "CheckTaxCodeEmber", { taxCode: taxCode }, function (res) {
            app.component.Loading.Hide();
            if (res.status) {
                /*toastr.success('Khách hàng đã tồn tại', 'Thông báo');*/
                $('#add-contract_modal').find('input[name = "codeIntermediaries"]').val(res.data.codeIntermediaries);
                $('#add-contract_modal').find('input[name = "nameIntermediaries"]').val(res.data.name);
                $('#add-contract_modal').find('input[name = "phoneIntermediaries"]').val(res.data.phone);
                $('#add-contract_modal').find('input[name = "addressIntermediaries"]').val(res.data.address);
                $('#add-contract_modal').find('input[name = "idIntermediaries"]').val(res.data.id);
            }
        });
    }
    //View info
    $('body').on('click', 'button.view-info', function () {
        ViewPersonalInfo($(this).data('id'));
    })
    $('body').on('click', 'button.view-info-tab', function () {
        let selectedRow = $('#table-body tr.selected');
        let id = selectedRow.data('id');
        OpenPrintModel(id);
        //PrintTab(id);
    })
    //EditContractInfo
    $('body').on('click', 'button#btn-show-edit', function () {
        /* e.preventDefault();*/
        let selectedRow = $('#table-body tr.selected');
        let id = selectedRow.data('id');
        EditContractInfo(id);
    });
    $('body').on('click', '#add-bill-edit', function () {
        let rows = $('#edit-contract_step-4').find('input[name="billMoney"]');
        let rawHtml = `
        <div class="bs-row">
            <!-- item -->
            <div class="bs-col sm-50">
                <div class="content-item input-group">
                    <label class="txtBill" for="">Thanh toán lần ${rows.length + 2}</label>
                    <input type="text" data-required="true" name="billMoney" class="isNumberF">
                </div>
            </div>
            <!-- item -->
            <div class="bs-col sm-50">
                <div class="content-item input-group">
                    <label for="">Ngày</label>
                    <div class="">
                        <input type="text" class="search__input form_datetime" name="dateBill" data-required="true">
                    </div>
                </div>
            </div>
            <!-- item -->
            <div class="bs-col sm-50">
                <div class="content-item input-group">
                    <label for="">Hình thức</label>
                    <select class="select2" name="formBill" data-required="true" data-ignore-reset="true">
                                                        <option value="0">Chuyển khoản</option>
                                                        <option value="1">Tiền mặt</option>
                                                    </select>
                </div>
            </div>
            <div class="bs-col sm-10">
                  <img id="remove-bill-edit" src="/Assets/crm/images/mechanism/delete.svg"
                       alt="" />
            </div>
         </div>
    `;
        $('#info-bill-edit').before(rawHtml);
        $(".form_datetime").datetimepicker({
            startView: 2,
            minView: 2,
            format: "dd/mm/yyyy",
            autoclose: true
        });
        $('.isNumberF').keyup(delaySystem(function (e) {
            let v = $(this).val();
            v = v.replace(/[^0-9]+/g, '');
            $(this).val(numberFormartAdmin(v));
        }, 0));
        $(".select2").select2();
    });
    $('body').on('click', '#remove-bill-edit', function (e) {
        $(this).closest('.bs-row').remove();
        $('#edit-contract_step-4').find('.txtBill').each(function (index, item) {
            $(item).text(`Thanh toán lần ${index + 2}`)
        });
    });
    $('body').on('click', '#btn-updateContract-edit', function () {
        var dictprefix = {
            name: "Họ và tên",
            email: "Email",
            birthday: "Ngày sinh",
            idCard: "Chứng minh thư",
            dateOfIssuance: "Ngày cấp",
            addressIssuance: "Nơi cấp",
            phone: "Số điện thoại",
            address: "Địa chỉ",
            accountBank: "Số tài khoản",
            codeDeposit: "Số phiếu đặt cọc",
            depositAmount: "Số tiền đặt cọc",
            depositDate: "Ngày đặt cọc",
            depositForm: "Hình thức đặt cọc",
            codeContract: "Mã hợp đồng",
            investmentAmount: "Gía trị HĐ",
            createDate: "Ngày vào hợp đồng",
            sale: "Sale",
            //teleSale: "TeleSale",
            saleRep: "Sale chốt hộ",
            //THÔNG TIN THANH TOÁN
            billMoney: "Thông tin thanh toán",
            dateBill: "Ngày thanh toán",
            formBill: "Hình thức thanh toán"
        };
        $('#edit-contract-investor').Validate({ texterrors: dictprefix, errtype: 2 }, function (errs) {
            if (errs.length > 0) {
                toastr.error('Có lỗi xảy ra', 'Thông báo!');
                //LoadSales('#add-contract_modal .select-saleref');
                //LoadSales('#add-contract_modal .select-sale');
                //LoadTeleSales('#add-contract_modal .select-telesale');
            } else {
                let model = $('#edit-contract-investor').serializeObject();
                let infoBill = [];
                $('#edit-contract_step-4 .bs-row').each(function () {
                    infoBill.push(
                        {
                            BillMoney: $(this).find('input[name="billMoney"]').val(),
                            DateBill: $(this).find('input[name="dateBill"]').val(),
                            FormBill: $(this).find('select[name="formBill"]').val(),
                            IdBill: $(this).find('select[name="idBill"]').val()
                        }
                    );
                });
                model.ListInforBill = infoBill;
                let data = model;
                _AjaxPostForm(baseUrl + "InsertOrUpdate", data, function (res) {
                    $(elefrm).ClearErrors(null, function () {

                    });
                    if (res.status) {
                        toastr.success('Cập nhật hợp đồng thành công', 'Thông báo');
                        $('.close__modal').trigger('click');
                        SetupPagination();
                    } else {
                        $(elefrm).ShowError(res.data, { errtype: 2 }, function (error) {

                        }, function (errs) {
                            errs.forEach(function (item, index) {
                                toastr.error(item.ErrorMessage, 'Thông báo');
                            })
                        });
                    }
                });
            }
        })
    });

    //List, Search, Pageing
    $('#btn-search-contract-aaaa').on('click', function (e) {
        /* e.preventDefault();*/
        app.component.Loading.Show();
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
                              <tr>                      
                                <th>Mã hợp đồng</th>
                                <th>Tên khách hàng</th>
                                <th>Số CMT</th>
                                <th>Số điện thoại</th>
                                <th>Sale</th>
                                <th>TeleSale</th>
                                <th>Trạng thái hợp đồng</th>
                            </tr>
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
                                    <td>
                                       ${item.contractCode}
                                    </td>
                                    <td>
                                        <button type="button"
                                                data-id="${item.id}"
                                                class="view-info">
                                                ${item.nameInvestor}
                                         </button>
                                    </td>
                                    <td>${item.cmt}</td>
                                    <td>${item.phone}</td>
                                    <td>${item.saleName}</td>
                                    <td>${item.teleSaleName}</td>
                                    <td>
                                        <span class="status working">${item.nameStatus}</span>
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
                        $('#table-body').html(`<span>Không có dữ liệu</span>`);
                        $('.select_pagination').hide();
                        $('#size-page .from').text(0);
                        $('#size-page .to').text(0);
                        $('#size-page .total').text(0);
                    }
                    app.component.Loading.Hide();
                }).fail(function (error) {
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

var LoadResources = function (target, selected) {
    $(target).html('');
    $(target).append(`<option value="">-Chọn nguồn-</option>`);
    $.get(baseUrl + 'GetInvestorResource', function (res) {
        if (res.result == 200) {
            $.each(res.data, function (index, item) {
                $(target).append(`<option value="${item.investorResourceCode}">${item.addressFind}</option>`)
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
                div.html(`
                             <tr>                      
                                <th>Mã hợp đồng</th>
                                <th>Tên khách hàng</th>
                                <th style=" min-width: 150px;">Số CMT</th>
                                <th>Số điện thoại</th>
                                <th>Sale</th>
                                <th>TeleSale</th>
                                <th>Trạng thái hợp đồng</th>
                            </tr>
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
                                    <td>
                                       ${item.contractCode}
                                    </td>
                                    <td>
                                        <button type="button"
                                                data-id="${item.id}"
                                                class="view-info"
                                                >
                                                ${item.nameInvestor}
                                         </button>
                                    </td>
                                    <td>${item.cmt}</td>
                                    <td>${item.phone}</td>
                                    <td>${item.saleName}</td>
                                    <td>${item.teleSaleName}</td>
                                    <td>
                                        <span class="status ${item.idStatusContract.trim().toLowerCase() === "PayDone".toLowerCase() ? "done" : "working"}">${item.nameStatus}</span>
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
            //modal-show="show"
            //modal-data="#detail-customer-contract_modal"
        }
    });
}
//function PrintTab(id) {
//    app.component.Loading.Show();
//    $.ajax({
//        url: "/PrintContract/Print",
//        method: 'POST',
//        data: {
//            id: id
//        },
//        success: function (res) {
//            app.component.Loading.Hide();
//            PrintData(res.datahtml);
//        }
//    });
//}
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
        const dataId1 = $("#detail-customer-contract_modal .id-type").val();
        const addressSelect1 = $(`#detail-customer-contract_modal .address-select`)
        for (let i = 0; i < addressSelect1.length; i++) {
            let s = addressSelect1[i]
            if ($(s).attr('data-id') === dataId1) {
                $(s).addClass('active');
            }
            else if ($(s).hasClass('active')) {
                $(s).removeClass('active')
            }
        }
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
var EditContractInfo = function (id) {
    $.get(baseUrl + "Edit?id=" + id, function (res) {
        if (res.result == 400) {
            toastr.error(res.errors.join('<br />'));
        }
        else {
            if (res.status == false) {
                toastr.error("HĐ đã được duyệt nên không thể chỉnh sửa", 'Thông báo');
                return false;
            } else {
                $('#edit-contract_modal .content-modal').html(res);
                $('#edit-contract_modal').addClass('show-modal');
                $('#edit-contract_modal .content-modal').addClass('show-modal');
            }

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

        let selectedSale = $('#edit-contract_modal .hidden-sale').val();
        LoadSalesView('#edit-contract_modal', selectedSale);

        let selectedSaleRep = $('#edit-contract_modal .hidden-saleRep').val();
        LoadSalesRefView('#edit-contract_modal', selectedSaleRep);

        let selectedTeleSale = $('#edit-contract_modal .hidden-telesale').val();
        LoadTeleSalesView('#edit-contract_modal', selectedTeleSale);

        $('#edit-contract_modal select').select2();
        const dataId1 = $("#edit-contract_modal .id-type").val();
        const addressSelect1 = $(`#edit-contract_modal .address-select`)
        for (let i = 0; i < addressSelect1.length; i++) {
            let s = addressSelect1[i]
            if ($(s).attr('data-id') === dataId1) {
                $(s).addClass('active');
            }
            else if ($(s).hasClass('active')) {
                $(s).removeClass('active')
            }
        }
        $(".form_datetime").datetimepicker(
            {
                format: 'dd/mm/yyyy',
                minView: 2,
                maxView: 4,
                autoclose: true
            });
        let elefrmEdit = '#edit-contract-investor';
        $(elefrmEdit).SetupSerializeJson({
            pattern: 'auto', url: baseUrl + "getmv", containerid: '#error_message', tokenid: '#tokenid'
        });
        var typingTimerEdit;                //timer identifier
        var doneTypingIntervalEdit = 3000;  //time in ms, 5 second for example
        var $inputEdit = $('#checkPhoneEdit');

        //on keyup, start the countdown
        $inputEdit.on('keyup', function () {
            clearTimeout(typingTimerEdit);
            typingTimerEdit = setTimeout(doneTypingEdit, doneTypingIntervalEdit);
        });

        //on keydown, clear the countdown 
        $inputEdit.on('keydown', function () {
            clearTimeout(typingTimer);
        });

        //user is "finished typing," do something
        function doneTypingEdit() {
            app.component.Loading.Show();
            let phone = $('#checkPhoneEdit').val();
            _AjaxPostForm(baseUrl + "CheckPhoneInvestor", { phone: phone }, function (res) {
                app.component.Loading.Hide();
                if (res.status) {
                    /*toastr.success('Khách hàng đã tồn tại', 'Thông báo');*/
                    $('#edit-contract_modal').find('input[name = "idCard"]').val(res.data.idCard);
                    if (res.data.isCMT == 1) {
                        $('#edit-contract_modal').find('input[name = "addressIssuance"]').val(res.data.addressIssuance);
                        $('#edit-contract_modal .id-type').val(0).change();
                    } else {
                        $('#edit-contract_modal .id-type').val(1).change();
                        $('#edit-contract_modal select[name="AddressIssuanceCC"]').val(+res.data.addressIssuance).change();
                    }
                    $('#edit-contract_modal').find('input[name = "email"]').val(res.data.email);
                    $('#edit-contract_modal').find('input[name = "birthday"]').val(res.data.birthdayString);
                    $('#edit-contract_modal').find('input[name = "dateOfIssuance"]').val(res.data.dateOfIssuanceString);
                    $('#edit-contract_modal').find('input[name = "idInvestor"]').val(res.data.id);
                    $('#edit-contract_modal').find('input[name = "name"]').val(res.data.name);
                    $('#edit-contract_modal').find('input[name = "personalTaxCode"]').val(res.data.personalTaxCode);
                    $('#edit-contract_modal').find('input[name = "accountBank"]').val(res.data.accountBank);
                    $('#edit-contract_modal').find('input[name = "bank"]').val(res.data.bank);
                }
            });
        }
        //
        var typingTimerEmberEdit;                //timer identifier
        var doneTypingIntervalEmberEdit = 3000;  //time in ms, 5 second for example
        var $inputEmberEdit = $('#taxCodeIntermediariesEdit');

        //on keyup, start the countdown
        $inputEmberEdit.on('keyup', function () {
            clearTimeout(typingTimerEmberEdit);
            typingTimerEmberEdit = setTimeout(doneTypingEmberEdit, doneTypingIntervalEmberEdit);
        });

        //on keydown, clear the countdown 
        $inputEmberEdit.on('keydown', function () {
            clearTimeout(typingTimerEmberEdit);
        });

        //user is "finished typing," do something
        function doneTypingEmberEdit() {
            app.component.Loading.Show();
            let taxCode = $('#taxCodeIntermediariesEdit').val();
            _AjaxPostForm(baseUrl + "CheckTaxCodeEmber", { taxCode: taxCode }, function (res) {
                app.component.Loading.Hide();
                if (res.status) {
                    /*toastr.success('Khách hàng đã tồn tại', 'Thông báo');*/
                    $('#edit-contract_modal').find('input[name = "codeIntermediaries"]').val(res.data.codeIntermediaries);
                    $('#edit-contract_modal').find('input[name = "nameIntermediaries"]').val(res.data.name);
                    $('#edit-contract_modal').find('input[name = "phoneIntermediaries"]').val(res.data.phone);
                    $('#edit-contract_modal').find('input[name = "addressIntermediaries"]').val(res.data.address);
                    $('#edit-contract_modal').find('input[name = "idIntermediaries"]').val(res.data.id);
                }
            });
        }
        //$('#detail-customer-contract_modal input').attr('disabled', 'disabled');
        //$('#detail-customer-contract_modal select').attr('disabled', 'disabled');
        /*$('#view-edit').hide();*/
        $('.isNumberF').keyup(delaySystem(function (e) {
            let v = $(this).val();
            v = v.replace(/[^0-9]+/g, '');
            $(this).val(numberFormartAdmin(v));
        }, 0));
    });
}
//Append Bill
var addViewBill = function (e) {
    let rows = $('#add-contract_step-4').find('input[name="billMoney"]');
    let rawHtml = `
        <div class="bs-row">
            <!-- item -->
            <div class="bs-col sm-50">
                <div class="content-item input-group">
                    <label class="txtBill" for="">Thanh toán lần ${rows.length + 2}</label>
                    <input type="text" data-required="true" name="billMoney" class="isNumberF">
                </div>
            </div>
            <!-- item -->
            <div class="bs-col sm-50">
                <div class="content-item input-group">
                    <label for="">Ngày</label>
                    <div class="">
                        <input type="text" class="search__input form_datetime" name="dateBill" data-required="true">
                    </div>
                </div>
            </div>
            <!-- item -->
            <div class="bs-col sm-50">
                <div class="content-item input-group">
                    <label for="">Hình thức</label>
                    <select class="select2" name="formBill" data-required="true" data-ignore-reset="true">
                                                        <option value="0">Chuyển khoản</option>
                                                        <option value="1">Tiền mặt</option>
                                                    </select>
                </div>
            </div>
            <div class="bs-col sm-10">
                  <img onclick="RemoveBill(this)" src="/Assets/crm/images/mechanism/delete.svg"
                       alt="" />
            </div>
         </div>
    `;
    $('#info-bill').before(rawHtml);
    $(".form_datetime").datetimepicker({
        startView: 2,
        minView: 2,
        format: "dd/mm/yyyy",
        autoclose: true
    });
    $('.isNumberF').keyup(delaySystem(function (e) {
        let v = $(this).val();
        v = v.replace(/[^0-9]+/g, '');
        $(this).val(numberFormartAdmin(v));
    }, 0));
    $(".select2").select2();
}
var RemoveBill = function (e) {
    //let body = $(e).closest('#add-contract_step-4');
    $(e).closest('.bs-row').remove();
    $('#add-contract_step-4').find('.txtBill').each(function (index, item) {
        $(item).text(`Thanh toán lần ${index + 2}`)
    });
}

function OpenPrintModel(id) {
    $('.loading-wrapper').show();
    $.ajax({
        url: "/PrintContract/SelectContractTemplate",
        method: 'GET',
        data: {
        },
        success: function (res) {
            $('#select-contract-type_modal .content-modal').html(res);
            ShowModal('#select-contract-type_modal');
            $('#select-contract-type_modal select').select2();
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
        url: printUrl,
        method: 'POST',
        data: {
            id: id
        },
        success: function (res) {
            var left = ($(window).width() / 2) - (900 / 2);
            top = ($(window).height() / 2) - (900 / 2);

            var mywindow = window.open('', 'report', "width=900,height=900, top=" + top + ", left=" + left);

            mywindow.document.write('<html><head>');
            mywindow.document.write('</head><body>');
            if (printUrl === '/PrintContract/Print') {
                mywindow.document.write(`<div id="tools" style="position: fixed;">
                <button type="button" onClick="$('#tools').hide(); PreviewPrint(); $('#tools').show();">In</button>
                <button type="button" onClick="Export2Doc('print', 'hợp đồng')">Tải về</button>
                </div>`);
            } else if (printUrl === '/PrintContract/PrintDMDT') {
                mywindow.document.write(`<div id="tools" style="position: fixed;">
                <button type="button" onClick="$('#tools').hide(); PreviewPrint(); $('#tools').show();">In</button>
                <button type="button" onClick="Export2Doc('print', 'hợp đồng QLDM')">Tải về</button>
                </div>`);
            } else if (printUrl === '/PrintContract/PrintTransferAgreement') {
                mywindow.document.write(`<div id="tools" style="position: fixed;">
                <button type="button" onClick="$('#tools').hide(); PreviewPrint(); $('#tools').show();">In</button>
                <button type="button" onClick="Export2Doc('print', 'hợp đồng chuyên nhượng')">Tải về</button>
                </div>`);
            } else if (printUrl === '/PrintContract/PrintSupplementAgreement') {
                mywindow.document.write(`<div id="tools" style="position: fixed;">
                <button type="button" onClick="$('#tools').hide(); PreviewPrint(); $('#tools').show();">In</button>
                <button type="button" onClick="Export2Doc('print', 'hợp đồng thỏa thuận bổ sung')">Tải về</button>
                </div>`);
            }
            
            mywindow.document.write(res.datahtml);
            mywindow.document.write('<script src="/lib/jquery/dist/jquery.min.js"></script>');
            mywindow.document.write('</body></html>');
            mywindow.document.close();
            $('.loading-wrapper').hide();
        }
    });
}