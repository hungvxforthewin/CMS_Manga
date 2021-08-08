var allowance = {
    load: function () {
        function delay(callback, ms) {
            let timer = 0;
            return function () {
                var context = this, args = arguments;
                clearTimeout(timer);
                timer = setTimeout(function () {
                    callback.apply(context, args);
                }, ms || 0);
            };
        }
        function numberFormart(n) {
            n = n.replace(/[^0-9]+/g, '');
            return commaSeparateNumber(n);
        }
        function commaSeparateNumber(val) {
            while (/(\d+)(\d{3})/.test(val.toString())) {
                val = val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
            }
            return val;
        }

        $('.isNumber').keyup(delay(function (e) {
            let v = $(this).val();
            v = v.replace(/[^0-9]+/g, '');
            $(this).val(commaSeparateNumber(v));
        }, 0));
        //$('#chuyen_can').val(commaSeparateNumber($('#chuyen_can').attr('data-id')));
        //$('#cong_doan').val(commaSeparateNumber($('#cong_doan').attr('data-id')));
       
        $('#btnSubmit').on('click', function () {
            debugger;
            let lstmodel = [];
            var chuyen_can = {
                Id: $('.id_chuyen_can').val(),
                AllowanceAmount: $('#chuyen_can').val().replaceAll(',', '').replaceAll('.', ''),
                Type: 2
            }
            lstmodel.push(chuyen_can);
            var cong_doan = {
                Id: $('.id_cong_doan').val(),
                AllowanceAmount: $('#cong_doan').val().replaceAll(',', '').replaceAll('.', ''),
                Type: 4
            }
            lstmodel.push(cong_doan);
            $.ajax({
                url: '/Allowance/Save',
                type: 'POST',
                data: {
                    lstmodel: JSON.stringify(lstmodel)
                },
                success: function (re) {
                    if (re.success) {
                        toastr.success('Cập nhật thành công', 'Thông báo');
                    } else {
                        toastr.success('Cập nhật không thành công', 'Thông báo');
                    }
                }
            });
        });
    }
}
$(document).ready(function () {
    allowance.load();
});