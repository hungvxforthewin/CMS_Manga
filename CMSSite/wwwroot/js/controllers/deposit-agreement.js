let baseUrl = '/SaleAdmin/DepositAgreement/';

$(function () {
    let elefrm = '#add-deposit-investor';
    $(elefrm).SetupSerializeJson({
        pattern: 'auto', url: baseUrl + "getmv", containerid: '#error_message', tokenid: '#tokenid'
    });
    $('body').on('click', '#btn-show-add', function () {
        $(elefrm).ResetInputs().ClearErrors(null, function () {

        });
        $('#btn-hiid-show').trigger('click');
    });
    $('body').on('click', '#btn-addContract-close', function () {
        let id = $(this).attr('data-id');
        PreviewViewPersonalInfo(id);
    });
    
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
            console.log(res);
            if (res.status) {
                /*toastr.success('Khách hàng đã tồn tại', 'Thông báo');*/
                $('#deposit-agreement_modal').find('input[name = "idCard"]').val(res.data.idCard);
                $('#deposit-agreement_modal').find('input[name = "addressIssuance"]').val(res.data.addressIssuance);
                $('#deposit-agreement_modal').find('input[name = "email"]').val(res.data.email);
                $('#deposit-agreement_modal').find('input[name = "birthday"]').val(res.data.birthdayString);
                $('#deposit-agreement_modal').find('input[name = "dateOfIssuance"]').val(res.data.dateOfIssuanceString);
                $('#deposit-agreement_modal').find('input[name = "idInvestor"]').val(res.data.id);
                $('#deposit-agreement_modal').find('input[name = "name"]').val(res.data.name);
                $('#deposit-agreement_modal').find('input[name = "personalTaxCode"]').val(res.data.personalTaxCode);
                $('#deposit-agreement_modal').find('input[name = "accountBank"]').val(res.data.accountBank);
                $('#deposit-agreement_modal').find('input[name = "bank"]').val(res.data.bank);
                $('#deposit-agreement_modal').find('input[name = "address"]').val(res.data.address);
            }
        });
    }
    //check contractInvestor
    var $inputCode = $('#checkContract');
    $inputCode.on('keyup', function () {
        clearTimeout(typingTimer);
        typingTimer = setTimeout(doneTypingCheck, doneTypingInterval);
    });

    //on keydown, clear the countdown 
    $inputCode.on('keydown', function () {
        clearTimeout(typingTimer);
    });
    function doneTypingCheck() {
        app.component.Loading.Show();
        let CodeContract = $('#checkContract').val();
        _AjaxPostForm(baseUrl + "CheckContract", { CodeContract: CodeContract }, function (res) {
            app.component.Loading.Hide();
            console.log(res);
            if (res.status && res.data.id != null) {
                /*toastr.success('Hợp đồng đã tồn tại', 'Thông báo');*/
                $('#deposit-agreement_modal').find('input[name = "currencyContract"]').val((+res.data.investmentAmount).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
                $('#deposit-agreement_modal').find('input[name = "idContractInvestor"]').val(res.data.id);
            }
        });
    }
    // Add
    $('body').on('click', '#btn-addContract-done', function () {

        var dictprefix = {
            name: "Họ và tên",
            email: "Email",
            idCard: "Chứng minh thư",
            phone: "Số điện thoại",
            //THÔNG TIN ĐẶT CỌC
            billMoney: "Thông tin đặt cọc",
            dateBill: "Ngày cọc",
            formBill: "Hình thức thanh toán",
        };
        $('#add-deposit-investor').Validate({ texterrors: dictprefix, errtype: 2 }, function (errs) {
            if (errs.length > 0) {
                toastr.error('Có lỗi xảy ra', 'Thông báo!');
               
            } else {
                let model = $('#add-deposit-investor').serializeObject();
                let infoBill = [];
                $('#deposit-agreement_step-4 .pay-item1').each(function () {
                    infoBill.push(
                        {
                            BillMoney: $(this).find('input[name="billMoney"]').val(),
                            DateBill: $(this).find('input[name="dateBill"]').val(),
                            DescriptionBill: $(this).find('input[name="descriptionDeposit"]').val()
                        }
                    );
                });
                model.ListInforDepositBill = infoBill;
                let data = model;
                console.log(data);
                _AjaxPostForm(baseUrl + "InsertOrUpdate", data, function (res) {
                    console.log(res);
                    $(elefrm).ClearErrors(null, function () {

                    });
                    if (res.status) {
                        toastr.success('Đặt cọc thành công', 'Thông báo');
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
    //View info
    //$('body').on('click', 'button.view-info', function () {
    //    ViewPersonalInfo($(this).data('id'));
    //})
    //$('body').on('click', 'button.view-info-tab', function () {
    //    let selectedRow = $('#table-body tr.selected');
    //    let id = selectedRow.data('id');
    //    PrintTab(id);
    //})
    // Edit info
    $('body').on('click', 'button.edit-info', function () {
        EditDpositInfo($(this).data('id'));
    })
    $('body').on('click', '#add-bill-edit-deposit', function () {
        let rows = $('#deposit-agreement_edit_step-4').find('input[name="billMoney"]');
        let rawHtml = `
        <div class="pay-item1">
                                        <div class="bs-row">
                                            <!-- item -->
                                            <div class="bs-col sm-50">
                                                <div class="content-item input-group">
                                                    <label class="txtBill" for="">Thanh toán lần ${rows.length + 1}</label>
                                                    <div class="">
                                                            <input type="text" name="billMoney" data-required="true" class="isNumberF" />
                                                            
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- item -->
                                            <div class="bs-col sm-50">
                                                <div class="content-item input-group">
                                                    <label for="">Ngày thanh toán</label>
                                                    <div class="">
                                                        <input type="text"
                                                                   class="search__input form_datetime" name="dateBill" data-required="true" />
                                                        
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- item -->
                                            <div class="bs-col sm-50">
                                                <div class="content-item input-group">
                                                    <label for="">Ghi chú</label>
                                                    <input type="text" name="descriptionDeposit" />
                                                </div>
                                            </div>
                                            <!-- item -->
                                            <div class="bs-col sm-50">
                                                <div class="content-item input-group">
                                                    <label for="" style="color:white">Tương ứng</label>
                                                    <div class="input_currency">
                                                       
                                                    </div>
                                                </div>
                                            </div>
                                           
                                        </div>
                                        <img onclick="RemoveBill(this)" class="delete-icon"
                                             src="/Assets/SA/images/layout/delete.svg"
                                             alt="delete"
                                             width="25"
                                             height="25" />
                                    </div>
    `;
        $('#add-bill-edit-deposit').before(rawHtml);
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
    //EditContractInfo
    //$('body').on('click', 'button#btn-show-edit', function () {
    //    /* e.preventDefault();*/
    //    let selectedRow = $('#table-body tr.selected');
    //    let id = selectedRow.data('id');
    //    EditContractInfo(id);
    //});
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
            idCard: "Chứng minh thư",
            phone: "Số điện thoại",
            //THÔNG TIN ĐẶT CỌC
            billMoney: "Thông tin đặt cọc",
            dateBill: "Ngày cọc",
            formBill: "Hình thức thanh toán",
        };
        $('#edit-deposit-investor').Validate({ texterrors: dictprefix, errtype: 2 }, function (errs) {
            if (errs.length > 0) {
                toastr.error('Có lỗi xảy ra', 'Thông báo!');
            } else {
                let model = $('#edit-deposit-investor').serializeObject();
                let infoBill = [];
                $('#deposit-agreement_edit_step-4 .pay-item1').each(function () {
                    infoBill.push(
                        {
                            BillMoney: $(this).find('input[name="billMoney"]').val(),
                            DateBill: $(this).find('input[name="dateBill"]').val(),
                            DescriptionBill: $(this).find('input[name="descriptionDeposit"]').val()
                        }
                    );
                });
                model.ListInforDepositBill = infoBill;
                let data = model;
                console.log(data);
                _AjaxPostForm(baseUrl + "InsertOrUpdate", data, function (res) {
                    console.log(res);
                    $(elefrm).ClearErrors(null, function () {

                    });
                    if (res.status) {
                        toastr.success('Cập nhật đặt cọc thành công', 'Thông báo');
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
    $('#btn-search-deposit-confirm').on('click', function (e) {
        /* e.preventDefault();*/
        console.log('click');
        app.component.Loading.Show();
        let data = $('#search-form').serializeObject();
        console.log(data);
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
                    console.log(res);
                    let div = $('#table-body');
                    div.html('');
                    div.html(`
                             <tr>                      
                                <th>Mã hợp đồng</th>
                                <th>Tên khách hàng</th>
                                <th>Số CMT</th>
                                <th>Số điện thoại</th>
                                <th>Description</th>
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
                            if (item.contractCode == null || item.contractCode == undefined) {
                                item.contractCode = 'Không xác định';
                            }
                            if (item.description == null || item.description == undefined) {
                                item.description = 'Không có';
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
                                                class="edit-info"
                                                >
                                                ${item.nameInvestor}
                                         </button>
                                    </td>
                                    <td>${item.cmt}</td>
                                    <td>${item.phone}</td>
                                    <td>
                                        <span class="status working">${item.description}</span>
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
                    key: $('#search-key').val()
                },
                method: 'POST',
                dataType: 'json'
            }).done(function (res) {
                console.log(res);

                let div = $('#table-body');
                div.html('');
                div.html(`
                             <tr>                      
                                <th>Mã hợp đồng</th>
                                <th>Tên khách hàng</th>
                                <th>Số CMT</th>
                                <th>Số điện thoại</th>
                                <th>Description</th>
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
                        if (item.contractCode == null || item.contractCode == undefined) {
                            item.contractCode = 'Không xác định';
                        }
                        if (item.description == null || item.description == undefined) {
                            item.description = 'Không có';
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
                                                class="edit-info"
                                                >
                                                ${item.nameInvestor}
                                         </button>
                                    </td>
                                    <td>${item.cmt}</td>
                                    <td>${item.phone}</td>
                                    <td>
                                        <span class="status working">${item.description}</span>
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
function PrintTab(id) {
    app.component.Loading.Show();
    $.ajax({
        url: baseUrl + "Print",
        method: 'POST',
        data: {
            id: id
        },
        success: function (res) {
            app.component.Loading.Hide();
            PrintData(res.datahtml);
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
var EditContractInfo = function (id) {
    $.get(baseUrl + "Edit?id=" + id, function (res) {
        if (res.result == 400) {
            toastr.error(res.errors.join('<br />'));
        }
        else {
            $('#edit-contract_modal .content-modal').html(res);
            $('#edit-contract_modal').addClass('show-modal');
            $('#edit-contract_modal .content-modal').addClass('show-modal');
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
                console.log(res);
                if (res.status) {
                    /*toastr.success('Khách hàng đã tồn tại', 'Thông báo');*/
                    $('#edit-contract_modal').find('input[name = "idCard"]').val(res.data.idCard);
                    $('#edit-contract_modal').find('input[name = "addressIssuance"]').val(res.data.addressIssuance);
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
var EditDpositInfo = function (id) {
    $.get(baseUrl + "Edit?id=" + id, function (res) {
        if (res.result == 400) {
            toastr.error(res.errors.join('<br />'));
        }
        else {
            $('#deposit-agreement_modal_edit .content-modal').html(res);
            $('#deposit-agreement_modal_edit').addClass('show-modal');
            $('#deposit-agreement_modal_edit .content-modal').addClass('show-modal');
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

        //let selectedSale = $('#edit-contract_modal .hidden-sale').val();
        //LoadSalesView('#edit-contract_modal', selectedSale);

        //let selectedSaleRep = $('#edit-contract_modal .hidden-saleRep').val();
        //LoadSalesRefView('#edit-contract_modal', selectedSaleRep);

        //let selectedTeleSale = $('#edit-contract_modal .hidden-telesale').val();
        //LoadTeleSalesView('#edit-contract_modal', selectedTeleSale);

        $('#edit-contract_modal select').select2();
        $(".form_datetime").datetimepicker(
            {
                format: 'dd/mm/yyyy',
                minView: 2,
                maxView: 4,
                autoclose: true
            });
        let elefrmEdit = '#edit-deposit-investor';
        $(elefrmEdit).SetupSerializeJson({
            pattern: 'auto', url: baseUrl + "getmv", containerid: '#error_message', tokenid: '#tokenid'
        });
        var typingTimerEditDP;                //timer identifier
        var doneTypingIntervalEdit = 3000;  //time in ms, 5 second for example
        var $inputEdit = $('#checkPhoneEdit');

        //on keyup, start the countdown
        $inputEdit.on('keyup', function () {
            clearTimeout(typingTimerEditDP);
            typingTimerEditDP = setTimeout(doneTypingEdit, doneTypingIntervalEdit);
        });

        //on keydown, clear the countdown 
        $inputEdit.on('keydown', function () {
            clearTimeout(typingTimerEditDP);
        });

        //user is "finished typing," do something
        function doneTypingEdit() {
            app.component.Loading.Show();
            let phone = $('#checkPhoneEdit').val();
            _AjaxPostForm(baseUrl + "CheckPhoneInvestor", { phone: phone }, function (res) {
                app.component.Loading.Hide();
                console.log(res);
                if (res.status) {
                    /*toastr.success('Khách hàng đã tồn tại', 'Thông báo');*/
                    $('#deposit-agreement_modal_edit').find('input[name = "idCard"]').val(res.data.idCard);
                    $('#deposit-agreement_modal_edit').find('input[name = "addressIssuance"]').val(res.data.addressIssuance);
                    $('#deposit-agreement_modal_edit').find('input[name = "email"]').val(res.data.email);
                    $('#deposit-agreement_modal_edit').find('input[name = "birthday"]').val(res.data.birthdayString);
                    $('#deposit-agreement_modal_edit').find('input[name = "dateOfIssuance"]').val(res.data.dateOfIssuanceString);
                    $('#deposit-agreement_modal_edit').find('input[name = "idInvestor"]').val(res.data.id);
                    $('#deposit-agreement_modal_edit').find('input[name = "name"]').val(res.data.name);
                    $('#deposit-agreement_modal_edit').find('input[name = "personalTaxCode"]').val(res.data.personalTaxCode);
                    $('#deposit-agreement_modal_edit').find('input[name = "accountBank"]').val(res.data.accountBank);
                    $('#deposit-agreement_modal_edit').find('input[name = "bank"]').val(res.data.bank);
                    $('#deposit-agreement_modal_edit').find('input[name = "address"]').val(res.data.address);
                }
            });
        }
        var typingTimerEditDPC; //timer identifier
        var doneTypingIntervalEditCode = 3000; 
        var $inputCodeEdit = $('#checkContractEdit');

        $inputCodeEdit.on('keyup', function () {
            clearTimeout(typingTimerEditDPC);
            typingTimer = setTimeout(doneTypingCheck, doneTypingIntervalEditCode);
        });

        //on keydown, clear the countdown 
        $inputCodeEdit.on('keydown', function () {
            clearTimeout(typingTimerEditDPC);
        });
        function doneTypingCheck() {
            app.component.Loading.Show();
            let CodeContract = $('#checkContractEdit').val();
            _AjaxPostForm(baseUrl + "CheckContract", { CodeContract: CodeContract }, function (res) {
                app.component.Loading.Hide();
                console.log(res);
                if (res.status && res.data.id != null) {
                    /*toastr.success('Hợp đồng đã tồn tại', 'Thông báo');*/
                    $('#deposit-agreement_modal_edit').find('input[name = "currencyContract"]').val((+res.data.investmentAmount).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
                    $('#deposit-agreement_modal_edit').find('input[name = "idContractInvestor"]').val(res.data.id);
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
    let rows = $('#deposit-agreement_step-4').find('input[name="billMoney"]');
    let rawHtml = `
        <div class="pay-item1">
                                        <div class="bs-row">
                                            <!-- item -->
                                            <div class="bs-col sm-50">
                                                <div class="content-item input-group">
                                                    <label class="txtBill" for="">Thanh toán lần ${rows.length + 1}</label>
                                                    <div class="">
                                                            <input type="text" name="billMoney" data-required="true" class="isNumberF" />
                                                            
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- item -->
                                            <div class="bs-col sm-50">
                                                <div class="content-item input-group">
                                                    <label for="">Ngày thanh toán</label>
                                                    <div class="">
                                                        <input type="text"
                                                                   class="search__input form_datetime" name="dateBill" data-required="true" />
                                                        
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- item -->
                                            <div class="bs-col sm-50">
                                                <div class="content-item input-group">
                                                    <label for="">Ghi chú</label>
                                                    <input type="text" name="descriptionDeposit" />
                                                </div>
                                            </div>
                                            <!-- item -->
                                            <div class="bs-col sm-50">
                                                <div class="content-item input-group">
                                                    <label for="" style="color:white">Tương ứng</label>
                                                    <div class="input_currency">
                                                       
                                                    </div>
                                                </div>
                                            </div>
                                           
                                        </div>
                                        <img onclick="RemoveBill(this)" class="delete-icon"
                                             src="/Assets/SA/images/layout/delete.svg"
                                             alt="delete"
                                             width="25"
                                             height="25" />
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
    $(e).closest('.pay-item1').remove();
    $('#deposit-agreement_step-4').find('.txtBill').each(function (index, item) {
        $(item).text(`Thanh toán lần ${index + 1}`)
    });
}
function ShowPopupPrint(data) {
    var left = ($(window).width() / 2) - (900 / 2);
    top = ($(window).height() / 2) - (900 / 2);

    var mywindow = window.open('', 'report', "width=900,height=900, top=" + top + ", left=" + left);

    mywindow.document.write('<html><head>');
    mywindow.document.write('<link rel="stylesheet" href="/Templates/css/main.min.css" type="text/css" />');
    mywindow.document.write('<link rel="stylesheet" href="/Templates/css/tool.min.css" type="text/css" />');
    mywindow.document.write('<link rel="stylesheet" href="/Templates/css/select2.min.css" type="text/css" />');

    mywindow.document.write('<script src="/Scripts/jquery-3.3.1.min.js"></script>');
    mywindow.document.write('<script src="/Areas/GetApplication/Scripts/word.js"></script>');

    mywindow.document.write('</head><body>');
    mywindow.document.write(data);
    mywindow.document.write('</body></html>');
    mywindow.document.close();
}

function PrintData(datahtml) {
    return ShowPopupPrint(datahtml);
}