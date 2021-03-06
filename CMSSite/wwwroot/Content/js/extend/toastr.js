function notifySuccess(message, title, position) {
    if (position == null) {
        position = "bottom";
    }
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-" + position + "-left",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "800",
        "timeOut": "3000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    if (title == null) {
        title = "Thành công";
    }
    toastr.success(message, title)
}

function notifyInfo(message, title, position) {
    if (position == null) {
        position = "bottom";
    }
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-" + position + "-left",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "800",
        "timeOut": "3000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    if (title == null) {
        title = "Thông báo";
    }
    toastr.info(message, title)
}

function notifyWarning(message, title, position) {
    if (position == null) {
        position = "bottom";
    }
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-" + position + "-left",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "800",
        "timeOut": "3000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    if (title == null) {
        title = "Cảnh báo";
    }
    toastr.warning(message, title)
}

function notifyError(message, title, position) {
    if (position == null) {
        position = "bottom";
    }
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-" + position + "-left",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "800",
        "timeOut": "3000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    if (title == null) {
        title = "Lỗi";
    }
    toastr.error(message, title)
}