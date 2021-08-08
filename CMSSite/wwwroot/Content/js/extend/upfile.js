var UpfilePage = {

    init: function () {

        UpfilePage.onEvent();
        UpfilePage.upEvent();
    },

    onEvent: function () {

        jQuery(document).on("click", ".attachFile", function () {
            var data = jQuery(this).getData();
            jQuery(data.rel).attr("data-target", data.target);
            jQuery(data.rel).attr("data-filename", data.filename);
            jQuery(data.rel).attr("data-filepath", data.filepath);
            jQuery(data.rel).attr("data-ischeck", data.ischeck);
            jQuery(data.rel).attr("data-isprocess", data.isprocess);
            jQuery(data.rel).attr("data-isonly", data.isonly);
            jQuery(data.rel).attr("data-deleteclass", data.deleteclass);
            jQuery(data.rel).val("").trigger("click");

            return false;
        });
        jQuery(document).on("click", ".delMember", function () {
            jQuery(this).closest(".member").slideUp("slow", function () {
                var parent = jQuery(this).parent();
                if (parent.find(".member").length == 1) {
                    parent.addClass("hidden");
                }
                jQuery(this).remove();
            });
        });
        jQuery(document).on("click", ".delMemberhd", function () {
            jQuery(this).closest(".member").slideUp("slow", function () {
                var parent = jQuery(this).parent();
                if (parent.find(".member").length == 1) {
                    parent.addClass("hidden");
                }
                jQuery(this).removeClass("loading").html(
                    "<span class='fileitem member'>" +
                    "<input name='FileName' class='fileNames' type='text' value='' />" +
                    "<input name='Path' class='filePaths' type='hidden' value=''/>" +
                    "</span>"
                );
            });
        });
        jQuery(document).on("change", ".inputUpFile", function () {
            var obj = jQuery(this);
            if (!obj.hasClass("setUpFiled")) {
                obj.hasClass("setUpFiled");
                var files = $(this).get(0).files;
                var fileData = new FormData();
                for (var i = 0; i < files.length; i++) {
                    fileData.append("fileInput", files[i]);
                }
                $.ajax({
                    type: "POST",
                    url: "/Uploader/UpFile",
                    dataType: "json",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (rs) {
                        if (rs.Status) {
                            var dataObj = obj.getData();
                            var targetObj = jQuery(dataObj.target);
                            var deleteClass = dataObj.deleteClass;
                            deleteClass = Utils.isEmpty(deleteClass) ? "delMember" : deleteClass;
                            targetObj.closest(".hidden").removeClass("hidden");
                            rs.datas.forEach(function (data) {
                                if (dataObj.isOnly) {
                                    targetObj.removeClass("loading").html(
                                        "<span class='fileitem member'>" +
                                        "<img class='img-thumbnail' src='" + data.FilePath + "' title='" + data.FileName + "' />" +
                                        "<input readonly name='" + dataObj.filename + "' class='fileNames' type='text' value='" + data.FileName + "' />" +
                                        "<input name='" + dataObj.filepath + "' class='filePaths' type='hidden' value='" + data.FilePath + "'/>" +
                                        "<button type='button' class='btn btn-xs btn-link close " + deleteClass + "'>x</button>" +
                                        "</span>"
                                    );
                                } else {
                                    targetObj.removeClass("loading").append(
                                        "<span class='fileitem member'>" +
                                        "<img class='img-thumbnail' src='" + data.FilePath + "' title='" + data.FileName + "' />" +
                                        "<input readonly name='" + dataObj.filename + "' class='fileNames' type='text' value='" + data.FileName + "' />" +
                                        "<input name='" + dataObj.filepath + "' class='filePaths' type='hidden' value='" + data.FilePath + "'/>" +
                                        "<button type='button' class='btn btn-xs btn-link close " + deleteClass + "'>x</button>" +
                                        "</span>"
                                    );
                                }
                            });
                           
                        }
                        else {
                            Utils.setError('File không hợp lệ');
                        }
                    },
                    error: function (xhr, status, error) {
                       
                    }
                });
            }
        });

    },
    upEvent: function (container) {
       
    },
    upEventRow: function (row) {
        row.find(".datetime").datetimepicker({
            format: "d-m-Y H:i",
            lang: "vi",
            scrollInput: false
        });
    },
    onProgress: function (e, file) {
        var pc = Math.floor(100 * (e.loaded / e.total));
        var rowId = UpfilePage.getRowId(file.id, true);
        jQuery(rowId).find(".progress-bar").css("width", pc + "%");
        jQuery(rowId).find(".progress-label").text(pc + "%");
    },
    onAbort: function (e, file) {
        jQuery(UpfilePage.getRowId(file.id, true))
            .find(".upStatus")
            .html("<a href='#' class='loadfail' title='Tải tài liệu lên không thành công' ></a>");
    },
    getRowId: function (fileId, isSelector) {
        return (isSelector ? "#" : "") + "DocUploadR" + fileId;
    }
};
jQuery(document).ready(function () {
    UpfilePage.init();
})