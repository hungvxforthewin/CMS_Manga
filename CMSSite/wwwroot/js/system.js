/**
 * System.Js
 * Github: none
 * Date created: Ngày 25 tháng 3 năm 2021.
 * HungVX
 * Một thư viên javascript dựa trên theme ByteSoft
 *
 * Licensed under the MIT license ( http://www.opensource.org/licenses/MIT )
 */
let ResetValueInForm = function (form) {
    //reset error;
    //showErrors(null, $(form).find(NameDivShowError));
    //showErrors2($(form), null, prefixName);
    //reset value;
    var fields = $(form).find('[name]');
    $.each(fields, function (index, field) {
        var tagname = $(field).prop("tagName");
        var name = $(field).attr("name");
        if (name !== "__RequestVerificationToken") {
            if (tagname === "INPUT") {
                //check radio
                var type = $(field).attr("type");
                if (type !== "radio") {
                    $(field).val("");
                }
            }
            else if (tagname === "TEXTAREA") {
                $(field).val("");
            } else if (tagname === "SELECT") {
                $(field).val("").trigger("change");
            }
        }

    });
}
$(function () {
    $('.isNumberF').keyup(delaySystem(function (e) {
        let v = $(this).val();
        v = v.replace(/[^0-9]+/g, '');
        $(this).val(numberFormartAdmin(v));
    }, 0));
});
(function ($) {
    var __DataPost = null;
    var ___ModelView = null;;
    var __eleerr = $('<div id="__err">Lỗi</div>');
    var __containererrorID = null;
    var __ModelForm = null;
    var __dataget = null;
    var __TokenID = null;

    $.fn.serializeObject = function (options) {
        options = jQuery.extend(true, {
            validateName: true
        }, options);
        var self = this,
            json = {},
            push_counters = {},
            patterns = {
                "validate": /^[a-zA-Z][a-zA-Z0-9_]*(?:\[(?:\d*|[a-zA-Z0-9_]+)\])*$/,
                "key": /[a-zA-Z0-9_]+|(?=\[\])/g,
                "push": /^$/,
                "fixed": /^\d+$/,
                "named": /^[a-zA-Z0-9_]+$/
            };
        this.build = function (base, key, value) {
            base[key] = value;
            return base;
        };
        this.push_counter = function (key) {
            if (push_counters[key] === undefined) {
                push_counters[key] = 0;
            }
            return push_counters[key]++;
        };
        $.each($(this).serializeArray(), function () {

            // skip invalid keys
            if (!patterns.validate.test(this.name) && options.validateName) {
                return;
            }
            var k,
                keys = this.name.match(patterns.key),
                merge = this.value,
                reverse_key = this.name;
            while ((k = keys.pop()) !== undefined) {
                // adjust reverse_key
                reverse_key = reverse_key.replace(new RegExp("\\[" + k + "\\]$"), '');
                // push
                if (k.match(patterns.push)) {
                    merge = self.build([], self.push_counter(reverse_key), merge);
                }
                // fixed
                else if (k.match(patterns.fixed)) {
                    merge = self.build([], k, merge);
                }
                // named
                else if (k.match(patterns.named)) {
                    merge = self.build({}, k, merge);
                }
            }
            json = $.extend(true, json, merge);
        });
        return json;
    };
    $.fn.SetupSerializeJson = function (options) {
        if ($(this).attr('id') == "__err") return __eleerr;

        var args = options || {};
        __containererrorID = null;
        ___ModelView = null;
        __ModelForm = null;
        __dataget = null;
        __TokenID = null;

        var bsuccess = __getpattern(args, "SetupSerializeJson");
        if (!bsuccess) return __eleerr;
        bsuccess = __getcontainerError(args, "SetupSerializeJson");
        if (!bsuccess) return __eleerr;
        __getModelForm($(this), ___ModelView);

        //var idhtmltoken = args['tokenid'];
        //if (idhtmltoken == null || idhtmltoken == undefined) {
        //    alert("Lỗi: chưa chỉ định {tokenid:string} trong SetupSerializeJson");
        //    return __eleerr;
        //}
        //__TokenID = idhtmltoken;
        return $(this);
    };
    $.fn.Validate = function (options = null, callback = null) {
        if ($(this).attr('id') == "__err") return __eleerr;

        var args = options || {};
        var bsuccess = __getpattern(args, "Validate");
        if (!bsuccess) return __eleerr;
        bsuccess = __getcontainerError(args, "Validate");
        if (!bsuccess) return __eleerr;

        var textError = args['texterrors'];
        var showerrs = args['showerrors'];
        var errtype = args['errtype'];

        if (showerrs == null || showerrs == undefined) showerrs = true;
        if (textError == null || textError == undefined) textError = null;
        if (errtype == null || errtype == undefined) errtype = 1;
        /////////////////////////////////////////////////////////
        var errs = __validateform($(this), textError);
        //reset lại các lỗi nếu có
        resetRedBorders($(this));
        if (errs.length > 0 && showerrs) {
            if (errtype == 2)
                __showerrors2(errs);
            else
                __showerrors(errs, $(__containererrorID));
        }
        if (typeof callback == 'function') callback(errs);

        return errs.length > 0 ? __eleerr : $(this);
    };
    $.fn.ShowError = function (response, options = null, callback = null, callback2 = null) {
        if ($(this).attr('id') == "__err") return __eleerr;
        var args = options || {};
        var errtype = args['errtype'];
        if (errtype == null || errtype == undefined) errtype = 1;
        var bsuccess = __getcontainerError(args, "ShowError");
        if (!bsuccess) return __eleerr;
        var flag = false;
        try {
            var errs = $.parseJSON(response);
            if (typeof errs == 'object') {
                if (errtype == 2)
                    __showerrors2(errs);
                else
                    __showerrors(errs, $(__containererrorID));

                if (typeof callback2 == 'function') callback2(errs);
            }
            else
                if (typeof callback == 'function') {
                    flag = true; callback(response);
                }
        } catch {
            if (typeof callback == 'function' && flag == false) callback(response);
        }

    };
    //reset error 
    $.fn.ClearErrors = function (options = null, AfterClear = null,) {
        if ($(this).attr('id') == "__err") return __eleerr;

        var args = options || {};
        var bsuccess = __getpattern(args, "ClearErrors");
        if (!bsuccess) return __eleerr;
        bsuccess = __getcontainerError(args, "ClearErrors");
        if (!bsuccess) return __eleerr;
        //clear các viền đỏ
        resetRedBorders($(this));
        //clear container lỗi
        var elecontainer = $(__containererrorID);
        elecontainer.hide();
        if (!elecontainer.hasClass('alert')) {
            elecontainer.addClass('alert');
            elecontainer.addClass('alert-danger');
            elecontainer.css({ "width": "100%", "height": "100%" });
        }
        elecontainer.empty();
        elecontainer.append(' <strong style="font-size: 16px;">Có lỗi khi lưu:</strong>');
        elecontainer.append('<ul></ul>');
        //clear các text màu đỏ bên dưới thành phần khi errtype=2
        $('.err').remove();
        if (typeof AfterClear == 'function') AfterClear();
        return $(this);
    };
    //reset input 
    $.fn.ResetInputs = function (options = null, AfterReset = null,) {
        if ($(this).attr('id') == "__err") return __eleerr;

        var args = options || {};
        var bsuccess = __getpattern(args, "ClearErrors");
        if (!bsuccess) return __eleerr;

        resetInputs($(this));

        if (typeof AfterReset == 'function') AfterReset();

        return $(this);
    };

    function __validateform($element, texterrors = null) {
        __getModelForm($element, ___ModelView);
        if (texterrors == null) texterrors = {};
        var dictname = {};
        var errs = new Array();
        $.each(__ModelForm, function (index, val) {
            var ele = val.Element;// $("[name=" + val + "]");
            var nameele = val.Name;

            if (ele.is(':disabled') == false && dictname[nameele] == undefined) { //nếu thành phần disable thì bỏ qua, không thẩm định, nếu có nhiều thành phần trùng tên thì chỉ xử lý một lần
                dictname[nameele] = nameele;
                var textlabel = (texterrors[nameele] == undefined ? nameele : texterrors[nameele]);
                if (ele.is("input")) {
                    var typeele = ele.attr('type').toLowerCase();

                    if (ele.val() != '') {
                        if (ele.data('type') == "date") {
                            if (!isDate(ele.val())) { var SS = { ErrorMessage: textlabel + " không hợp lệ.", Field: nameele }; errs.push(SS); }
                        } else if (ele.data('type') == "int") {
                            var intRegex = /^\d+$/;
                            if (!intRegex.test(ele.val())) { var SS = { ErrorMessage: textlabel + " không hợp lệ.", Field: nameele }; errs.push(SS); }

                        } else if (ele.data('type') == "float") {
                            var floatRegex = /^((\d+(\.\d *)?)|((\d*\.)?\d+))$/;
                            if (!floatRegex.test(ele.val().replace(/,/g, ''))) { var SS = { ErrorMessage: textlabel + " không hợp lệ.", Field: nameele }; errs.push(SS); }

                        }
                    }

                    if (ele.data('required') == true) {
                        if (typeele == "radio" || typeele == "checkbox") {

                            var ii = -1;
                            var ele1 = $("[name=" + nameele + "]");
                            ele1.each(function (index, _) {
                                if (ele1.eq(index).prop("checked")) { ii = index; }
                            });

                            if (ii == -1) { var SS = { ErrorMessage: textlabel + " chưa lựa chọn.", Field: nameele }; errs.push(SS); }


                        } else {
                            if (ele.val() == '') { var SS = { ErrorMessage: textlabel + " không để trống.", Field: nameele }; errs.push(SS); }
                        }
                    }

                    //var maxlenght = ele.attr('maxlength');
                    //if (maxlenght != undefined) {
                    //    if (ele.val().length > maxlenght) { var SS = { ErrorMessage: textlabel + " không lớn hơn " + maxlenght + " kí tự.", Field: nameele }; errs.push(SS); }
                    //}

                } else if (ele.is("textarea")) {
                    if (ele.val() === "" && ele.data('required') == true) { var SS = { ErrorMessage: textlabel + " không để trống.", Field: nameele }; errs.push(SS); }
                } else if (ele.is("select") && ele.data('required') == true) { //truong select2 dropdownlist

                    if ((ele.val() == undefined || ele.val() == "")) { var SS = { ErrorMessage: textlabel + " chưa lựa chọn.", Field: nameele }; errs.push(SS); }

                }
            }
        });
        return errs;
    }
    function resetInputs($element) {
        __getModelForm($element, ___ModelView);
        $.each(__ModelForm, function (index, val) {
            var ele = val.Element;// $("[name=" + val + "]");
            //bỏ qua nếu thành phần có data('ignore') ==true

            if (ele.data('ignore-reset') == false || ele.data('ignore-reset') == undefined) {
                if (ele.is("input")) {
                    var typeele = ele.attr('type').toLowerCase();
                    if (typeele == "radio") {
                        ele.prop("checked", false);
                    } else if (typeele == "checkbox") {
                        ele.prop("checked", false);
                    } else {
                        ele.val('');
                    }
                    //ele.css('border-color', '');
                } else if (ele.is("select")) {
                    var attr = ele.attr('multiple');
                    ele.empty();
                    if (typeof attr !== typeof undefined && attr !== false) {
                        //reset lại attr data==false, có tác dụng trong hàm BindingMultiDropdownlist trong Common.cs
                        ele.data('firstloaded', false);
                    } else {
                        var textholder = ele.data('placeholder');
                        textholder = textholder == undefined ? "Lựa chọn" : textholder;
                        ele.append($("<option></option>").attr("value", '').text(textholder));
                    }
                    //  ele.siblings(".select2-container").css('border', '');
                } else if (ele.is("textarea")) {
                    ele.val('');
                    // ele.css('border-color', '');
                }
            }
        });
    }
    function resetRedBorders($element) {
        __getModelForm($element, ___ModelView);
        $.each(__ModelForm, function (index, val) {
            //console.log(index, val);
            var ele = val.Element;// $("[name=" + val + "]");
            // var nameele = val;
            if (ele.is("select")) {
                ele.siblings(".select2-container").css('border', '');
            } else ele.css('border-color', '');
        });

        //$element.children().each(function () {
        //    var ele = $(this);
        //    var nameele = ele.attr('name');
        //    if (nameele !== undefined && pattern[nameele] !== undefined) {
        //        if (ele.is("select")) {
        //            ele.siblings(".select2-container").css('border', '');
        //        } else ele.css('border-color', '');
        //    }
        //    resetRedBorders(ele, pattern);
        //});
    }

    function __getModelForm($element, pattern) {
        debugger
        if (__ModelForm == null) {
            var result = new Array();
            _recursiveModelForm($element, pattern, result);

            __ModelForm = result;
        }
    }
    function _recursiveModelForm($element, pattern, arrResult) {
        debugger
        //lặp qua tất cả các thành phần trong div
        $element.children().each(function () {
            var ele = $(this);
            var nameele = ele.attr('name');
            var eleValue = ele.val();
            if (nameele !== undefined && pattern[nameele] !== undefined) {
                //nếu thành phần co name giống với thuộc tính modelview
                var ss = { "Name": nameele, "Element": ele };
                arrResult.push(ss);
            }
            // Loop her children
            _recursiveModelForm(ele, pattern, arrResult);
        });
    }
    function __getcontainerError(args, name) {
        debugger
        if (__containererrorID == null) {
            var containererror = args['containerid'];
            if (containererror == null || containererror == undefined) {
                alert("Lỗi : chưa chỉ định {containerid:string} trong " + name);
                return false;
            }
            __containererrorID = containererror;
        }
        return true;
    }
    function GetModelView(surl) {
        var theresponse = null;
        $.ajax({
            method: 'POST',
            url: surl,
            async: false,
            success: function (response) {
                console.log(response);
                if (response.status === 200) {
                    theresponse = response.modelView;
                }
            }
        })
        //AjaxProvider.Post({
        //    url: surl,
        //    async: false,
        //    success: function (response) {
        //        if (response.status === 200) {
        //            theresponse = response.data.ModelView;
        //        }
        //    }
        //});
        return theresponse;
    }
    function __getpattern(args, name) {
        if (___ModelView == null) {
            var pattern = args['pattern'];
            if (pattern == undefined) {
                alert("Lỗi:  Chưa chỉ định {pattern:object} trong " + name);
                return false;
            }
            if (Object.prototype.toString.call(pattern) == '[object String]') {
                // a string
                if (pattern == "auto") {
                    var surl = args['url'];
                    if (surl == '' || surl == undefined) {
                        alert("Lỗi: chưa chỉ định {url:string} trong " + name);
                        return false;
                    }
                    pattern = GetModelView(surl);
                } else {
                    alert("Lỗi: chỉ định {pattern:'auto',url:string} trong " + name);
                    return false;
                }
            }
            if (pattern == null) {
                return false;
            }
            ___ModelView = pattern
        }
        return true;
    }
    function __showerrors2(errs) {
        $('.err').remove();
        $.each(errs, function (_, data) {
            var ele = $("[name=" + data.Field + "]");
            ele.closest('div').append("<label class='err' style='color:red;' > " + data.ErrorMessage + "</label>");
            var type = ele.prop('nodeName');
            if (type == "INPUT") //textbox
                ele.css('border-color', 'red');
            else if (type == "TEXTAREA")//text area
                ele.css('border-color', 'red');
            else if (type == "SELECT")//select2
                ele.siblings(".select2-container").css('border', '2px solid red');
        });
    }
    function __showerrors(errs, eleContainer) {
        eleContainer.hide();
        if (!eleContainer.hasClass('alert')) {
            eleContainer.addClass('alert');
            eleContainer.addClass('alert-danger');
            eleContainer.css({ "width": "100%", "height": "100%" });
        }
        eleContainer.empty();
        eleContainer.append(' <strong style="font-size: 16px;">Có lỗi khi lưu:</strong>');
        eleContainer.append('<ul></ul>');
        $.each(errs, function (_, data) {
            eleContainer.append('<li> ' + data.ErrorMessage + '</li>');
            var ele = $("[name=" + data.Field + "]");
            var type = ele.prop('nodeName');
            if (type == "INPUT") //textbox
                ele.css('border-color', 'red');
            else if (type == "TEXTAREA")//text area
                ele.css('border-color', 'red');
            else if (type == "SELECT")//select2
                ele.siblings(".select2-container").css('border', '2px solid red');
        });
        eleContainer.show();
    }
})(jQuery);
function numberFormartAdmin(n) {
    n = n.replace(/[^0-9]+/g, '');
    return commaSeparateNumberSystem(n);
}
function delaySystem(callback, ms) {
    let timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}
function delayMinMaxSystem(callback, ms) {
    let timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}
function commaSeparateNumberSystem(val) {
    while (/(\d+)(\d{3})/.test(val.toString())) {
        val = val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
    }
    return val;
}
function showErrors(errs, divShowError) {
    divShowError = $(divShowError);
    divShowError.addClass('alert');
    divShowError.addClass('alert-danger');
    divShowError.css({ "width": "100%", "height": "100%" });

    divShowError.hide();
    divShowError.empty();

    if (errs != null && errs.length > 0) {
        divShowError.append(' <strong style="font-size: 16px;">Có lỗi khi lưu:</strong>');
        divShowError.append('<ul></ul>');
        $.each(errs, function (_, err) {
            divShowError.append('<li> ' + err + '</li>');
        });
        divShowError.show();
    }
}
function showErrors2($form, listErrors, prefixName) {
    $($form).find(".show-error-name").remove()
    if (isValue(listErrors) == true) {
        $.each(listErrors, function (i, error) {
            var namefield = "";
            if (isValue(prefixName) == true) {
                namefield = prefixName[error.Name];
            }
            if (isValue(namefield) == false) namefield = error.Name;

            var parrent = $($form).find("[name={0}]".format(error.Name)).parent();
            var divError = parrent.find(".show-error-name");
            if (divError.length == 0) {
                var html = "<div class ='show-error-name'  style='color:red;text-align:left;font-weight: bold;'>{0} {1} </div>".format(namefield, error.Error);
                parrent.append(html);
            } else {

            }
        });
    }
}
function Validate2($form, model, divShowError, prefixName, type) {
    //test check find input type = file, number, date, ... select, radio,checkbok ==> chua check
    var elements = $form.find('[name]');
    var listErrors = [];

    var requireChooseMessage = "phải lựa chọn";
    var requireFillMessage = "không được bỏ trống";

    var maxNumberMessgae = "phải là số nhỏ hơn hoặc bằng {0}";
    var minNumberMessgae = "phải là số lớn hơn hoặc bằng {0}";
    var intNumberMessage = "phải là số nguyên";
    var intByteMessage = "phải là số nguyên từ 0 đến 255";

    $.each(elements, function (index, _element) {
        var element = $(_element);
        // no valid element display none , disabled ,readonly
        if (element.is("[disabled]") !== false || element.is('[readonly]') == false) {

            var elementName = element.attr("name");
            var value = element.val();
            var type = element.attr("type");
            var isrequire = element.is(":required");

            //check require;=========================
            if (isrequire == true) {
                if (type == "radio") {
                    //  console.log(model[elementName]);
                    if (isValue(model[elementName]) == true) {
                        var isExits = listErrors.filter(x => (x.Name === elementName)).length > 0;
                        if (isExits == false) listErrors.push({ Name: elementName, Error: requireChooseMessage });
                    }
                } else if (type == "checkbox") {
                    var _checkboxs = elements.find("[type=checkbox][checked=true]");
                } else {
                    if (isValue(value) == false || value.trim() == "") {
                        if (element.is("INPUT") == true) {
                            listErrors.push({ Name: elementName, Error: requireFillMessage });
                        }
                        //is select 
                        else if (element.is("SELECT") == true) {
                            listErrors.push({ Name: elementName, Error: requireChooseMessage });
                        }
                        else {
                            listErrors.push({ Name: elementName, Error: requireFillMessage });
                        }

                    }
                }
            }

            //data - type =======================
            if (isValue(value) == true) {
                // is input ==> text, radio, checkbox,number,...
                if (element.is("INPUT") == true) {
                    if (type == "number") {
                        var number = parseFloat(element.val());
                        var maxNumber = element.attr('max');
                        var minNumber = element.attr('min');

                        var dataType = element.attr('data-type');
                        if (dataType == "int") {
                            if (Number.isInteger(number) == false) {
                                listErrors.push({ Name: elementName, Error: intNumberMessage });
                            } else {
                                if (typeof maxNumber != 'undefined' && number > maxNumber) {
                                    listErrors.push({ Name: elementName, Error: maxNumberMessgae.format(maxNumber) });
                                } else if (typeof minNumber != 'undefined' && number < minNumber) {
                                    listErrors.push({ Name: elementName, Error: minNumberMessgae.format(minNumber) });
                                }
                            }
                        } else if (dataType == "byte") {
                            if (number > 255 || number < 0 || Number.isInteger(number) == false) {
                                listErrors.push({ Name: elementName, Error: intByteMessage });
                            }
                        } else {
                            if (typeof maxNumber != 'undefined' && number > maxNumber) {
                                listErrors.push({ Name: elementName, Error: maxNumberMessgae.format(maxNumber) });
                            } else if (typeof minNumber != 'undefined' && number < minNumber) {
                                listErrors.push({ Name: elementName, Error: minNumberMessgae.format(minNumber) });
                            }
                        }
                    }
                    else if (type == "text") {
                        var maxLength = element.attr('maxlength');
                        var minLength = element.attr('minlength');
                        var validLength = element.val().length;
                        if (isValue(maxNumber) == true && isValue(minLength) == true) {
                            if (maxLength == minLength && validLength != maxLength) {
                                listErrors.push({ Name: elementName, Error: "số ký tự phải  bằng {0}".format(maxLength) });
                            } else if (maxLength < validLength || minLength > validLength) {
                                listErrors.push({ Name: elementName, Error: "số ký tự phải nằm trong khoảng từ {0} đến {1}".format(minLength, maxLength) });
                            }
                        } else if (isValue(maxNumber) == true && isValue(minLength) == false) {
                            if (maxLength < validLength) {
                                listErrors.push({ Name: elementName, Error: "số ký tự phải ít hơn hoặc bằng {0}".format(maxLength) });
                            }
                        } else if (isValue(maxNumber) == false && isValue(minLength) == true) {
                            if (minLength > validLength) {
                                listErrors.push({ Name: elementName, Error: "số ký tự phải nhiều hơn hoặc bằng {0}".format(minLength) });
                            }
                        }
                    }

                }

            }

        }
    });

    if (type == 1) {
        var listshowError = [];
        for (var i = 0; i < listErrors.length; i++) {
            var error = listErrors[i];
            if (isValue(prefixName) && isValue(prefixName[error.Name])) {
                listshowError.push(prefixName[error.Name] + ' ' + error.Error);
            } else {
                listshowError.push(error.Name + ' ' + error.Error);
            }
        }
        showErrors(listshowError, divShowError);
    } else {
        showErrors2($form, listErrors, prefixName);
    }
    return listErrors.length === 0;
}
$(document).ajaxSend(function (e, xhr, options) {
    if (options.type.toUpperCase() == "POST" || options.type.toUpperCase() == "PUT") {
        var token = $('form').find("input[name='__RequestVerificationToken']").val();
        xhr.setRequestHeader("RequestVerificationToken", token);
    }
});