jQuery.fn.extend({
    reset: function () {
        try {
            this.each(function () {
                this.reset();
            });
            jQuery(jQuery(this).attr("data-html-reset")).html("");
        } catch (e) {
        }
    },
    getData: function () {
        var data = {};
        try {
            this.each(function () {
                jQuery.each(this.attributes, function () {
                    var name = this.name.toLowerCase();
                    if (name.indexOf("data-") === 0) {
                        var k = "";
                        var args = name.split("-");
                        for (var n = 0; n < args.length; n++) {
                            if (n == 0) continue;
                            if (n == 1) {
                                k += args[n];
                            }
                            else {
                                k += args[n].capitalize()
                            }
                        }
                        data[k] = this.value;
                    }
                });
            });
        } catch (e) {
        }
        return data;
    },
    getDataUppername: function () {
        var data = {};
        try {
            this.each(function () {
                jQuery.each(this.attributes, function () {
                    var name = this.name.toLowerCase();
                    if (name.indexOf("data-") === 0) {
                        var k = "";
                        var args = name.split("-");
                        for (var n = 0; n < args.length; n++) {
                            if (n == 0) continue;
                            var v = args[n];
                            if (v == "id") {
                                k += v.toUpperCase();
                            }
                            else {
                                k += v.capitalize()
                            }
                        }
                        data[k] = this.value;
                    }
                });
            });
        } catch (e) {
        }
        return data;
    }
});
var Utils = {
    rmSpace: function (val) {
        try {
            while (val.indexOf(" ") !== -1) {
                val = val.replace(" ", "");
            }
        } catch (e) { }
        return val;
    },
    refreshCountTable: function (tbody) {
        var childs = tbody.children();
        for (var i = 0; i <= childs.length; i++) {
            tbody.find("tr:nth-child(" + i + ") td:first-child").html((i));
        }
    },
    notEmpty: function (val) {
        return !Utils.isEmpty(val);
    },
    isGet: function (form) {
        return form.attr("method").toLowerCase() == "get";
    },
    isPost: function (form) {
        return form.attr("method").toLowerCase() == "post";
    },
    isEmpty: function (val) {

        if (typeof val == "object")
            return false;
        if (typeof val == "function")
            return false;

        return val === undefined || jQuery.trim(val).length === 0;
    },
    isInteger: function (val) {

        return !isNaN(val) && !Utils.isEmpty(val);
    },
    getSerialize: function (form, event) {
        var keys = {};
        var buttons = {};
        var checkboxs = {};
        form.find("input, select, textarea,button").each(function () {
            var el = jQuery(this);
            var name = el.prop("name");
            if (!Utils.isEmpty(name)) {
                var tagName = el.prop("tagName").toLowerCase();
                if (tagName == "input") {
                    var type = el.prop("type").toLowerCase();
                    if (type == "text" || type == "password" || type == "hidden" || type == "tel" || type == "email") {
                        if (!keys.hasOwnProperty(name)) {
                            keys[name] = [];
                        }
                        keys[name].push(el.val());
                    } else if (type == "checkbox" || type == "radio") {
                        if (el.prop("checked")) {
                            if (!keys.hasOwnProperty(name)) {
                                keys[name] = [];
                            }
                            keys[name].push(el.val());
                        }
                        if (!checkboxs.hasOwnProperty(name)) {
                            checkboxs[name] = 0;
                        }
                        checkboxs[name]++;
                    }
                } else if (tagName != "button") {
                    if (!keys.hasOwnProperty(name)) {
                        keys[name] = [];
                    }
                    keys[name].push(el.val());
                }
            }
        });

        for (var k in keys) {
            var vals = keys[k];
            if (vals.length == 1 || buttons.hasOwnProperty(k)) { //|| !checkboxs.hasOwnProperty(k)
                keys[k] = vals.join(",");
            } else {
                keys[k] = JSON.stringify(vals);
            }
        }
        return keys;
    },

};
var app = {
    config: {
        Date: {
            Moment: "DD/MM/YYYY"
        },
        DateTime: {
            Moment: 'DD/MM/YYYY hh:mm:ss a'
        },
        Currency: {
            Suffix: " VNĐ",
            DecimalPlaces: 0,
            DecimalSeparator: ",",
            ThousandsSeparator: "."
        }
    },
    localStorage: {
        name: "",
        Data: null,
        init: function () {
            var _this = this;
            _this.name = window.location.host + appSetting.User.UserId;
            _this.Data = $.extend(true, {
                ChatBox: {
                    Current: []
                }
            }, JSON.parse(window.localStorage.getItem(_this.name) || "{}"));

            _this.Save();
        },
        Save: function () {
            var _this = this;
            window.localStorage.setItem(_this.name, JSON.stringify(_this.Data));
        }
    },
    documentReady: [],
    init: function () {
        //$.fn.datepicker.dates['vi'] = {
        //    days: ['Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'],
        //    daysShort: ['C.Nhật', 'T.Hai', 'T.Ba', 'T.Tư', 'T.Năm', 'T.Sáu', 'T.Bảy'],
        //    daysMin: ['CN', 'T.Hai', 'T.Ba', 'T.Tư', 'T.Năm', 'T.Sáu', 'T.Bảy'],
        //    months: ['Tháng Một', 'Tháng Hai', 'Tháng Ba', 'Tháng Tư', 'Tháng Năm', 'Tháng Sáu', 'Tháng Bảy', 'Tháng Tám', 'Tháng Chín', 'Tháng Mười', 'Tháng Mười Một', 'Tháng Mười Hai'],
        //    monthsShort: ['Một', 'Hai', 'Ba', 'Tư', 'Năm', 'Sáu', 'Bảy', 'Tám', 'Chín', 'Mười', 'M.Một', 'M.Hai'],
        //    today: "Hôm nay",
        //    clear: "Xóa",
        //    format: "dd/mm/yyyy",
        //    titleFormat: "MM yyyy", /* Leverages same syntax as 'format' */
        //    weekStart: 0
        //};

        this.component.init();

        for (var i = 0; i < this.documentReady.length; i++) {
            this.documentReady[i]();
        }

        moment.locale("vi");
        this.component.registerHandleInput();
    },
    component: {
        init: function () {
            this.load();
        },
        load: function () {

            //this.select2Default();
            //this.InputCurrency();
            this.FormatText();
            this.FormatInput();
            //this.ValidateForm.init();
            //this.ValidateForm.defaultRegistry();
            //this.DatePicker();
            //this.FormatInputNumber();
            //this.UploadCopperImg();
            //this.ImageView.Single();
            //this.ImgError();

        },
        FormatInput: function () {
            //https://github.com/autoNumeric/autoNumeric
            $('[ipPosInt]').each(function (index, elm) {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    new AutoNumeric(this, opPosInt);
                }
            });
            $('[ipPosInt2]').each(function (index, elm) {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    new AutoNumeric(this, opPosInt2);
                }
            });
            $('[ipNumber]').each(function (index, elm) {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    new AutoNumeric(this, opNumber);
                }
            });
            $('[ipMoney]').each(function (index, elm) {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    new AutoNumeric(this, opMoney);
                }
            });
            $('[ipPercent]').each(function (index, elm) {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    new AutoNumeric(this, opPercent);
                }
            });
        },
        FormattxtMore: function () {
            $('[txtMoneyMore]').each(function (index, elm) {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    ///Define_Autonumeric.js
                    $(this).text(formatMoney($(this).text()));
                }
            });
            $('[txtNumberMore]').each(function () {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    ///Define_Autonumeric.js
                    $(this).text(formatNumber($(this).text()));
                }
            });
        },
        ValidateInputPhone: function () {
            $("input[type='tel']").keydown(function (e) {
                // Allow: backspace, delete, tab, escape, enter and .
                if ($(this).val().substring(0, 2) == '01') { $(this).attr("minlength", 11); $(this).attr("maxlength", 11); }
                else { $(this).attr("minlength", 10); $(this).attr("maxlength", 10); }
                var check = true;
                if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||
                    // Allow: Ctrl+A, Command+A
                    (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                    // Allow: home, end, left, right, down, up
                    (e.keyCode >= 35 && e.keyCode <= 40)) {
                    check = false;
                    // let it happen, don't do anything
                    return;
                }
                // Ensure that it is a number and stop the keypress
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    check = false;
                    e.preventDefault();
                }
                if (check) {
                    if ($(this).val().substring(0, 1) != '0') {
                        var valp = $(this).val();
                        if (valp == '00')
                            valp = '0';
                        else valp = '0' + valp;
                        $(this).val(valp);
                        return false;
                    }
                }
            });
        },
        ValidateEmail: function () {
            $('input[type="email"]').on('keypress', function (event) {
                var regex = new RegExp("^[a-zA-Z0-9@._]+$");
                var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                if (!regex.test(key)) {
                    event.preventDefault();
                    return false;
                }
            });
        },
        FormatText: function (index, elm) {
            $('[txtNumber]').each(function () {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    ///Define_Autonumeric.js
                    $(this).text(formatNumber($(this).text()));
                }
            });
            $('[txtMoney]').each(function (index, elm) {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    ///Define_Autonumeric.js
                    $(this).text(formatMoney($(this).text()));
                }
            });
            $('[txtDateTime]').each(function (index, elm) {
                if (AutoNumeric.getAutoNumericElement(this) === null) {
                    ///method.js
                    $(this).text(formatToDateTime($(this).text()));
                }
            });
        },
        InputCurrency: function () {
            //money format
            $('[InputNumber]').each(function (index, elm) {
                elm = $(elm);
                options = elm.data();
                options.currencyMaskMap = options.currencyMaskMap || "";
                if (!elm.data("numFormat")) {
                    var DecimalPlaces = 0;
                    var DecimalSeparator = ",";
                    var ThousandsSeparator = ".";
                    if ((elm.data("percent") || false) == true) {
                        DecimalPlaces = 0;
                        DecimalSeparator = ",";
                        ThousandsSeparator = "";
                    }
                    elm.number(true, DecimalPlaces, DecimalSeparator, ThousandsSeparator);
                    //elm.number(true, 0, ",", ".");
                    if (options.currencyMaskMap != "" && options.currencyMaskMap != null && options.currencyMaskMap == true) {
                        var name = elm.prop("name");
                        elm.attr("name", name + "Mask");
                        $('<input type="hidden" name="{0}" value="{1}"/>'.format(name, elm.val())).insertAfter(elm);
                        var _event = function () {
                            var elm = $(this);
                            var a = elm.val();
                            $('[name="' + name + '"]').val(a);
                        }
                        var _event1 = function () {
                            var elm = $(this);
                            var a = elm.val();
                            $('[name="' + name + "Mask" + '"]').val(a);
                        }
                        elm.on("keyup", _event).on("change", _event);
                        $('[name="' + name + '"]').on("keyup", _event1).on("change", _event1);
                    }
                }
            });

        },
        select2Default: function () {
            var elements = $('[select2Search]');
            for (var i = 0; i < elements.length; i++) {
                var elem = $(elements[i]);
                if (!elem.data('select2')) {
                    elem.select2(elem.data());
                }
            }
            this.select2FixPosition();
        },
        select2FixPosition: function () {
            //select2 fix position
            $(".select2-hidden-accessible").map(function (index, elm) {
                $(elm).data("select2").on("results:message", function () {
                    this.dropdown._positionDropdown();
                });
            });

            $("select").on("select2:close", function (e) {
                var _this = $(this);
                var form = _this.closest("form");
                if (form.length > 0 && form.data("validator"))
                    _this.valid();
            });
        },
        modalEvent: function () {
            var modelCheck = function (show) {
                if ($('.modal').hasClass('in') && !$('body').hasClass('modal-open')) {
                    $('body').addClass('modal-open');
                }
            }
            $('.modal').on('hidden.bs.modal', function () {
                modelCheck(false);
            });
            $('.modal').on('shown.bs.modal', function () {
                modelCheck(true);
            });
            modelCheck(true);
        },
        ValidateForm: {
            init: function () {
                var _this = this;
                var getOptionForm = function (element) {
                    var form = $(element).closest("form");
                    var optionsData = form.data();
                    optionsData.tooltip = optionsData.tooltip == null ? false : optionsData.tooltip;
                    optionsData.tooltipPlacement = optionsData.tooltipPlacement == null ? "top" : optionsData.tooltipPlacement;
                    return optionsData;
                }
                $.validator.setDefaults({
                    errorElement: "span", // contain the error msg in a small tag
                    errorClass: 'form-text danger',
                    errorPlacement: function (error, element) {// render error placement for each input type
                        //if (element.attr("type") == "radio" || element.attr("type") == "checkbox") {// for chosen elements, need to insert the error after the chosen container
                        //    error.insertAfter($(element).closest('.form-group').children('div').children().last());
                        //} else if (element.attr("name") == "card_expiry_mm" || element.attr("name") == "card_expiry_yyyy") {
                        //    error.appendTo($(element).closest('.form-group').children('div'));
                        //} else {
                        //    if (error.text() != "") {
                        //        error.insertAfter(element);
                        //    }
                        //    // for other inputs, just perform default behavior
                        //}

                        var form = $(element).closest("form");

                        var optionsData = getOptionForm(element);
                        if (optionsData.tooltip) {

                            var addTooltip = function (element) {
                                var mess = error.text();
                                var elm = $(element);
                                elm.data("message-error", mess);
                                elm.tooltip({
                                    placement: optionsData.tooltipPlacement,
                                    title: mess,
                                    trigger: "hover focus"
                                })
                                    .attr('data-original-title', mess)
                                    .tooltip('show');
                            }

                            if (element.parent('.input-group').length) {
                                addTooltip(element);    // radio/checkbox?
                            } else if (element.hasClass('select2-hidden-accessible')) {
                                addTooltip(element.next('span'));
                            } else {
                                addTooltip(element);
                            }
                        }
                        else {
                            if (element.parent('.input-group').length) {
                                error.insertAfter(element.parent());      // radio/checkbox?
                            } else if (element.hasClass('select2-hidden-accessible')) {
                                error.insertAfter(element.next('span'));  // select2
                            } else {
                                error.insertAfter(element);//default
                            }
                        }

                        form.find("validation-summary-errors").hide();
                    },
                    //ignore: '',
                    ignore: ':hidden, table input',
                    success: function (label, element) {
                        var elm = $(element);
                        var optionsData = getOptionForm(element);
                        if (optionsData.tooltip) {
                            var element1 = elm;
                            if (elm.hasClass('select2-hidden-accessible')) {
                                element1 = $(elm.next('span'));
                            }
                            if (typeof ($(element1).tooltip) == "function" && $(element1).data("bs.tooltip") != null) {
                                $(element1).tooltip('destroy');
                            }
                        }

                        label.addClass('help-block valid');
                        $(element).closest('.form-group').removeClass('has-danger');

                        _this.override.success(label, element);
                    },
                    highlight: function (element) {
                        var elm = $(element);
                        var optionsData = getOptionForm(element);
                        if (optionsData.tooltip) {
                            var mess = $(element).data("message-error") || "";
                            if (mess != "") {
                                if (typeof (elm.tooltip) == "function" && elm.data("bs.tooltip") != null) {
                                    elm.tooltip('show');
                                    //$(element).tooltip('destroy')
                                }
                                else {
                                    elm.tooltip({
                                        placement: optionsData.tooltipPlacement,
                                        title: mess,
                                        trigger: "hover focus"
                                    })
                                        .attr('data-original-title', mess)
                                        .tooltip('show');
                                }

                                //elm.tooltip({
                                //    placement: "top",
                                //    title: mess,
                                //    trigger: "hover focus"
                                //});
                                //$(element).tooltip("show");
                            }
                        }

                        $(element).closest('.help-block').removeClass('valid');
                        $(element).closest('.form-group').removeClass('has-danger');
                        // setTimeout(function () {
                        // }, 00);
                        $(element).closest('.form-group').addClass('has-danger');

                        _this.override.highlight(element);
                    },
                    unhighlight: function (element) {
                        var elm = $(element);
                        var optionsData = getOptionForm(element);
                        if (optionsData.tooltip) {
                            var element1 = elm;
                            if (elm.hasClass('select2-hidden-accessible')) {
                                element1 = $(elm.next('span'));
                            }
                            if (typeof (element1.tooltip) == "function" && element1.data("bs.tooltip") != null) {
                                element1.tooltip('destroy')
                            }
                        }
                        $(element).closest('.form-group').removeClass('has-danger');

                        _this.override.unhighlight(element);
                    }
                });
                // setDefaults jquery validate ==============================================================================
                $.validator.addMethod("selectNotNull", function (value, element) {
                    value = value || "";
                    if (isNaN(value + "")) {
                        return (value + "").trim() != "";
                    }
                    value = value.replace("number:", "");
                    return value > 0;
                }, "Hãy nhập.");
                //}, "This field is required.");

                $.validator.methods.date = function (value, element) {
                    return this.optional(element) || moment(value, 'DD/MM/YYYY').toDate() !== null;
                };


            },
            defaultRegistry: function () {
                $("form.form_validate").each(function (index, elm) {
                    elm = $(elm);
                    if (!elm.data("validator")) {
                        var validOption = {
                            invalidHandler: function (event, validator) {
                            },
                            validateOnInit: true
                        };

                        var dataOptions = elm.data();
                        dataOptions.autoSubmit = dataOptions.autoSubmit == null ? (dataOptions.onSubmit == "" || dataOptions.onSubmit == null) : dataOptions.autoSubmit;
                        dataOptions.validateExtend = dataOptions.validateExtend == null ? true : dataOptions.validateExtend;

                        if (!dataOptions.autoSubmit || true) {
                            validOption.submitHandler = function (form) {
                                if (event != null) {
                                    event.stopPropagation();
                                    event.preventDefault();
                                }
                                form = $(form);

                                var dataOptions = form.data();
                                dataOptions.autoSubmit = dataOptions.autoSubmit == null ? true : dataOptions.autoSubmit;
                                dataOptions.onSubmit = dataOptions.onSubmit == null ? "" : dataOptions.onSubmit;
                                dataOptions.validateExtend = dataOptions.validateExtend == null || dataOptions.validateExtend == "" ? function () { return true; } : dataOptions.validateExtend;

                                var validateExtend = eval(dataOptions.validateExtend);
                                if (validateExtend == false || ($.isFunction(validateExtend) && !validateExtend())) {
                                    return;
                                }


                                var enctype = form.attr("enctype") || "";
                                var action = form.attr("action");

                                var data = form.serializeObject();

                                try {
                                    if (enctype.toLowerCase() == "multipart/form-data".toLowerCase()) {
                                        var data1 = new FormData(form[0]);
                                        form.find('input[type="file"]').map(function (index, elm) {
                                            data1.append(elm.name, elm.files);
                                        });
                                        for (var i in data) {
                                            data1.append(i, data[i]);
                                        }
                                        data = data1;

                                        if (!dataOptions.autoSubmit) {
                                            if (dataOptions.onSubmit != "") {
                                                eval(dataOptions.onSubmit)(form, data);
                                            }
                                            else {
                                                _AjaxAPI.formData(action, data, function (data) {
                                                    //console.log(data);
                                                });
                                            }

                                        }
                                        else {
                                            var validator = form.validate();
                                            validator.destroy();
                                            form.submit();
                                        }
                                    }
                                    else {
                                        if (!dataOptions.autoSubmit) {
                                            if (dataOptions.onSubmit != "") {
                                                eval(dataOptions.onSubmit)(form, data);
                                            }
                                            else {
                                                _AjaxAPI.post(action, data, function (data) {
                                                  /*  console.log(data)*/;
                                                });
                                            }
                                        }
                                        else {
                                            var validator = form.validate();
                                            validator.destroy();
                                            form.submit();
                                        }
                                    }
                                } catch (ex) { }
                            };
                        }
                        else {

                        }

                        elm.validate(validOption);
                    }
                });
            },
            override: {
                success: function (label, element) {
                },
                highlight: function (element) {
                    var _element = $(element);

                    // show tab
                    var show_tab = function (e) {
                        var tab_content = e.closest(".tab-pane");
                        if (tab_content.length == 0) {
                            return;
                        }
                        var tab_content_id = tab_content.attr("id");
                        var find_btn_tab = $('[href="#' + tab_content_id + '"][data-toggle="tab"]');
                        if (find_btn_tab.length != 0) {
                            find_btn_tab.click();
                            show_tab(find_btn_tab);
                        }
                    }
                    show_tab(_element);
                    // ==================================
                },
                unhighlight: function (element) {
                }
            }
        },
        DatePicker: function () {
            //bootstrap-datepicker.js
            //bootstrap-datepicker.min.js
            //https://bootstrap-datepicker.readthedocs.io/en/stable/
            $('[datepicker]').each(function (index, elm) {
                var datenow = new Date();
                var yearnow = datenow.getFullYear();

                $(this).datepicker({
                    format: "dd/mm/yyyy",
                    todayBtn: true,
                    clearBtn: true,
                    language: "vi",
                    calendarWeeks: true,
                    autoclose: true,
                    todayHighlight: true,
                    startDate: new Date(yearnow - 100 + '-01-01'),
                    endDate: new Date(yearnow + 100 + '-01-01')
                });
                //console.log($(this).datepicker('getStartDate'));
            });
        },
        EventCalculate: function () {
            $('[data-oncalculate]').each(function (index, elm) {
                var run = function (elm) {
                    var func = elm.data("oncalculate") || ""
                    func = eval(func);
                    if ($.isFunction(func)) {
                        func();
                    }
                }
                elm = $(elm);
                elm.on('keyup change blur', function () {
                    var elm = $(this);
                    if (elm.data("numFormat")) {
                        setTimeout(function () {
                            run(elm);
                        }, 100);
                    }
                    else {
                        run(elm);
                    }
                });
                run(elm);
            });
        },
        TextAreaAutoFitContent: function () {
            $('textarea[auto-fitcontent]').each(function (index, elm) {
                elm = $(elm);
                elm.height(elm[0].scrollHeight);
                //elm.on('keyup change blur', function () {
                //    elm.height( elm[0].scrollHeight );
                //});
            });
        },
        ImgError: function () {
            $('img').on('error', function () {
                if (!$(this).hasClass('broken-image')) {
                    $(this).prop('src', '/App_Assets/images/thumbnail-default.jpg').addClass('broken-image');
                }
            });
        },
        SingleUpload: function () {
            $('[data-upload-single]').each(function (index, elm) {
                elm = $(elm);
                if (!elm.data("UploadSingle")) {
                    elm.UploadSingle(elm.data());
                }
            });
        },
        Select2: function () {
            $('.select2').each(function () {
                $(this).select2({
                    width: '100%'
                });
            });

        },
        UpfileImage: function () {
            jQuery(document).on("change", ".ipAvatar", function () {
                var obj = $(this);
                var filename = obj.attr('data-FileName');
                var filepath = obj.attr('data-FilePath');
                var Src = obj.attr('data-Src');
                var files = $(this).get(0).files;
                var fileData = new FormData();
                for (var i = 0; i < files.length; i++) {
                    fileData.append("fileInput", files[i]);
                }
                $.ajax({
                    type: "POST",
                    url: "/Uploader/UploadFile",
                    dataType: "json",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (result, status, xhr) {
                        $(Src).attr('src', result.data.Url);
                        $(filename).attr('title', result.data.Name);
                        $(filepath).val(result.data.Url);
                    },
                    error: function (xhr, status, error) {
                        notifyError("Có lỗi xảy ra!");
                    }
                });
            });
        },
        UploadCopperImg: function () {
            $('[data-provider="UploadCopperImg"]').each(function (index, elm) {
                elm = $(elm);
                if (!elm.data("UploadCopperImg")) {
                    elm.css('cursor', 'pointer');
                    elm.click(function (event) {
                        event.stopPropagation();
                        event.preventDefault();
                        if (!elm.data("UploadCopperImg")) {
                            elm.UploadCopperImg(elm.data());
                        }
                        else {
                            elm.UploadCopperImg("Load");
                        }
                    });
                }
            });
        },
        ImageView: {
            Single: function () {
                $('[data-provider="image-viewer-single"]').each(function (index, elm) {
                    elm = $(elm);
                    elm.css('cursor', 'pointer');
                    elm.click(function (event) {
                        event.stopPropagation();
                        event.preventDefault();
                        var options = elm.data();
                        options = $.extend(true, {
                            target: "",
                        }, options);

                        var url = "";
                        if (elm[0].nodeName == "IMG") {
                            url = elm.attr('src');
                        }
                        else if (options.target != "") {
                            var target;
                            target = $(options.target);
                            url = target.attr('src');
                        }
                        else {
                            url = elm.find("img").attr("src");
                        }
                        if (url == "") {
                            return;
                        }
                        var body = $("body");
                        body.css("overflow", "hidden");

                        var data_caption = target.data("caption") || "";
                        var modal = $("#modal-images-viewer-single");
                        var img = modal.find("#img");
                        var caption = modal.find("#caption");
                        var close = modal.find("#close");
                        close[0].onclick = function () {
                            modal.hide();
                            body.css("overflow", "auto");
                        }
                        img.attr("src", url);
                        img.css("max-height", window.innerHeight - 40 + "px");
                        caption.text(data_caption);
                        modal.show();
                    });
                });
            }
        },
        registerHandleInput: function () {

        },
        Loading: {
            html: '<div class="showLoading"><div class="loader"></div></div>',
            Show: function () {
                if ($(".showLoading").length > 0) {
                    $(".showLoading").each(function () {
                        $(this).remove();
                    });
                }
                $("body").append(app.component.Loading.html);
            },
            Hide: function () {
                if ($(".showLoading").length > 0) {
                    $(".showLoading").each(function () {
                        $(this).remove();
                    });
                }
            }
        }
    }
};
$(document).ready(function () {
    Main.init();
});
var Main = {
    init: function () {
        Main.onEvent();
    },
    onEvent: function () {
        $(document).on('change', '.select_changeType', function (e) {
            var select = jQuery(this);
            var id = select.val();
            var url = select.attr('data-url');
            var target = select.attr('data-target');
            var type = select.attr('data-type');
            app.component.Loading.Show();
            jQuery.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                url: url,
                data: JSON.stringify({
                    'id': id,
                    'type': type
                }),
                dataType: "json",
                success: function (data) {
                    app.component.Loading.Hide();
                    jQuery(target).html(data.data);
                    app.component.DatePicker();
                    app.component.FormatInput();
                    $('.select2').select2({
                        width: '100%'
                    });
                },
            });
        });
        $(document).on("click", ".delete_itemtb", function () {
            try {
                var object = jQuery(this);
                var tr = object.closest("tr");
                var tbody = tr.parent();
                tr.remove();
                Utils.refreshCountTable(tbody);
            } catch (e) {
                console.log(e);
            }
            return false;
        });
        $(document).on("click", ".add_itemtb", function () {
            try {
                var obj = jQuery(this);
                var target = jQuery(this).attr("data-target");
                var template = $(obj.attr("data-template")).html();
                var tbody = $(obj.attr("data-tbody"));
                tbody.append(template);
                $('.select2').select2({
                    width: '100%'
                });
                Utils.refreshCountTable(tbody);
            } catch (e) {
                console.log(e);
            }
            return false;
        });
        $(document).on('change', '.select_change', function (e) {
            var select = jQuery(this);
            var id = select.val();
            var url = select.attr('data-url');
            var target = select.attr('data-target');
            app.component.Loading.Show();
            jQuery.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                url: url,
                data: JSON.stringify({
                    'id': id
                }),
                dataType: "json",
                success: function (data) {
                    app.component.Loading.Hide();
                    jQuery(target).html(data.data);
                    app.component.DatePicker();
                    app.component.FormatInput();
                    $('.select2').select2({
                        width: '100%'
                    });
                },
            });
        });
        jQuery(document).on('click', '#dataFilter_Dropdown', function () {
            jQuery(this).parents(".dataFilter_Dropdown").toggleClass("open");
            jQuery(this).parents(".panel-body ").find(".dataFilter_Dropdown_target").toggleClass("open");
        });
        jQuery(document).on('click', '.dataFilter_Dropdown_close', function (e) {
            e.preventDefault();
            jQuery(this).parents(".panel-body").find(".dataFilter_Dropdown").toggleClass("open");
            jQuery(this).parents(".panel-body").find(".dataFilter_Dropdown_target").toggleClass("open");
        });
        $('.select2').select2({
            width: '100%'
        });
        $(document).on('change', '.upfilesP', function () {
            var obj = $(this);
            var id = obj.attr('data-id');
            var fileName = this.files[0].name;
            var oFReader = new FileReader();
            oFReader.readAsDataURL(this.files[0]);
            oFReader.onload = function (oFREvent) {
                $(id).attr('src', this.result);
                $(id).attr('title', fileName);
            }
        });
        $(document).on('click', '#btnPrint', function () {
            var obj = $(this);
            var id = obj.attr('data-id');
            var url = obj.attr('data-url');
            app.component.Loading.Show();
            _AjaxPost(url, {
                Id: id
            }, function (rs) {
                var frame1 = document.createElement('iframe');
                frame1.name = "frame1";
                frame1.style.position = "absolute";
                frame1.style.top = "-1000000px";
                document.body.appendChild(frame1);
                var newWin = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
                newWin.document.open();
                newWin.document.write(rs.data);
                newWin.document.close();
                setTimeout(function () {
                    window.frames["frame1"].focus();
                    window.frames["frame1"].print();
                    document.body.removeChild(frame1);
                    newWin.close();
                    app.component.Loading.Hide();
                }, 10);

            });
        });
        $(document).on('click', '.downloadFile', function () {
            var obj = $(this);
            var id = obj.attr('data-id');
            var fileName = obj.attr('data-filename');
            var url = obj.attr('data-url');
            app.component.Loading.Show();
            _AjaxPost(url, {
                id: id,
                fileName: fileName
            }, function (rs) {
                app.component.Loading.Hide();
                if (rs.success) {
                    notifySuccess(rs.message);
                    SaveFileAs(rs.urlFile, rs.fileName);
                }
                else {
                    notifyError(rs.message);
                    console.log(rs.message);
                }

            });
        });
        $(document).on('click', '#btnPrintBill', function () {
            var obj = $(this);
            var id = obj.attr('data-id');
            var url = obj.attr('data-url');
            var target = obj.attr('data-target');
            app.component.Loading.Show();
            _AjaxPost(url, {
                Id: id
            }, function (rs) {
                $(target).html(rs.data);
                //app.component.FormattxtMore();
                $("#barcodecontainer #barcode").html(DrawHTMLBarcode_Code128A($("#barcodecontainer #barcode").text(), "yes", "in", 0, 2.5, 1, "bottom", "center", "", "black", "white"));
                var frame1 = document.createElement('iframe');
                frame1.name = "frame1";
                frame1.style.position = "absolute";
                frame1.style.top = "-1000000px";
                document.body.appendChild(frame1);
                var newWin = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
                newWin.document.open();
                newWin.document.write('<html><head><title>In Hóa Đơn</title><style type="text/css" media="print">@page { size: auto;margin: 0mm;} </style></head><body>' + $(target).html() + '</body></html>');
                newWin.document.close();
                setTimeout(function () {
                    window.frames["frame1"].focus();
                    window.frames["frame1"].print();
                    document.body.removeChild(frame1);
                    newWin.close();
                    app.component.Loading.Hide();

                }, 300);
            });
        });
        $(document).on('change', '.select_change2', function (e) {
            var select = jQuery(this);
            var id = select.val();
            var url = select.attr('data-url');
            var target = select.attr('data-target');
            app.component.Loading.Show();
            jQuery.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                async: false,
                url: url,
                data: JSON.stringify({
                    'id': id
                }),
                dataType: "json",
                success: function (data) {
                    app.component.Loading.Hide();
                    if (data.hasOwnProperty("isCust")) {
                        jQuery(target).html(data.htCust);
                    }
                    app.component.DatePicker();
                    app.component.FormatInput();
                    $('.select2').select2({
                        width: '100%'
                    });
                }
            });
        });
        jQuery(document).on("submit", ".quickSubmit", function (e) {
            e.preventDefault();
            try {
                var form = jQuery(this);
                var url = form.attr("action");
                var table = form.attr("data-table");
                var data = Utils.getSerialize(form);
                if (Utils.isEmpty(url)) {
                    $(form).removeClass('submiting');
                    return false;
                }
                _AjaxPost(url, { data }, function (rs) {
                    if (rs.success) {
                        $(table).bootstrapTable("refresh");
                        $("#mdlCustom").modal("hide");
                        notifySuccess(rs.message);
                    }
                    else {
                        notifyError(rs.message);
                        console.log(rs.message);
                    }
                })
            } catch (e) {

            }
            return false;
        });

        jQuery(document).on("click", ".quickUpdate", function () {
            try {
                var obj = jQuery(this);
                var target = jQuery(this).attr("data-target");
                var url = jQuery(this).attr("href");
                app.component.Loading.Show();
                cusmodal.ShowView(url, function () {
                    app.component.Loading.Hide();
                    app.component.FormatInput();

                    app.component.UpfileImage();
                    app.component.ValidateInputPhone();
                    app.component.DatePicker();

                    app.component.Select2();

                });

            } catch (e) {

            }
            return false;
        });

        jQuery(document).on("click", ".quickDelete", function () {
            try {
                var data = jQuery(this).getDataUppername();
                if (typeof data.RedirectPath == "undefined")
                    data.RedirectPath = Utils.getRedirect();

                jQuery.ajax({
                    type: "POST",
                    async: true,
                    url: jQuery(this).attr("href"),
                    data: data,
                    beforeSend: function () {
                        Utils.openOverlay();
                    },
                    complete: function () {
                        Utils.openOverlay();
                    },
                    error: function () {
                        Utils.openOverlay();
                    },
                    success: function (response) {
                        Utils.sectionBuilder(response);
                        if (response.hasOwnProperty("isCust")) {
                            Utils.closeOverlay();
                            jQuery(data.Target).html(response.htCust);
                        }
                        if (!Utils.isEmpty(data.TargetDeleteClick)) {
                            jQuery(data.TargetDeleteClick).fadeOut("fast", function () {
                                jQuery(this).remove();
                            });
                        }
                        Utils.updateFormState(jQuery(data.Target));
                        Utils.updateScrollBar(jQuery(data.Target));
                    }
                });
            } catch (e) {

            }
            return false;
        });
    },
};
