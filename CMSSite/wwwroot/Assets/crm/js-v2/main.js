$(document).ready(function () {
  // detect when clicking outside menu
  $(document).mouseup(function (e) {
    var container = $(".attendance_more-options");
    if (!container.is(e.target) && container.has(e.target).length === 0) {
      container.children(".options").removeClass("popup");
    }
  });

  // menu
  $(".bars").click(function () {
    $(".main-left").toggleClass("slide");
    $(".main-web").toggleClass("slide");
  });

  // date picker
  $(".form_datetime").datetimepicker({
    format: "dd/mm/yyyy",
    startView: 2,
    minView: 2,
    autoclose: true
  });

  $(".only-month").datetimepicker({
    format: "mm-yyyy",
    startView: 3,
    minView: 3,
    autoclose: true
  });

  $(".date-select img").click(function () {
    $(this).prev("input").focus();
  });

  $(function () {
    var handleMatchMedia = function (md) {
      if (md.matches) {
        if ($(".main-left").hasClass("slide")) {
          $(".main-left").removeClass("slide");
          $(".main-web").removeClass("slide");
        }
      }
    };
    var md = window.matchMedia("(max-width: 991px)");
    handleMatchMedia(md);
    md.addEventListener("change", () => handleMatchMedia(md));
  });

  //employee-manage
  const employeeManageSelectPlaceholders = [
    "Chọn chức vụ",
    "Chọn team",
    "Chọn phòng ban",
    "Chọn chi nhánh",
    "Chọn trạng thái",
  ];

  $(".select2").select2();
  $(".empleyee-manage_select").select2();

  // $(".empleyee-manage_select").each((i, v) => {
  //   $(v)
  //     .prepend('<option selected=""></option>')
  //     .select2({ placeholder: employeeManageSelectPlaceholders[i] });
  // });

  $(".add-employee_body-modal .modal-item .select2").select2();

  $(".check_all input").click(function () {
    let that = $(this);
    let checkboxes = $(".employee-manage_data-table table tr td input");
    checkboxes.each((i, v) => {
      if ($(v).prop("checked") !== that.prop("checked")) {
        $(v).click();
      }
    });
  });
  $(".employee-manage_data-table table tr td input").click(function () {
    $(this).parent().parent().parent().toggleClass("chosen");
  });

  //attendance

  // attendance - timekeeping
  $(document).on("click", ".attendance_more-options", function () {
    $(this).children(".options").toggleClass("popup");

    let tr = $(this).parent().parent();
    tr.toggleClass("active");
  });

  // admin author manage
  $(".author-view input").click(function () {
    let viewTds = $(".author-view_td input");
    viewTds.each((i, v) => {
      if ($(v).prop("checked") !== $(this).prop("checked")) {
        $(v).click();
      }
    });
  });

  $(".author-add input").click(function () {
    let addTds = $(".author-add_td input");
    addTds.each((i, v) => {
      if ($(v).prop("checked") !== $(this).prop("checked")) {
        $(v).click();
      }
    });
  });

  $(".author-edit input").click(function () {
    let editTds = $(".author-edit_td input");
    editTds.each((i, v) => {
      if ($(v).prop("checked") !== $(this).prop("checked")) {
        $(v).click();
      }
    });
  });

  $(".author-delete input").click(function () {
    let deleteTds = $(".author-delete_td input");
    deleteTds.each((i, v) => {
      if ($(v).prop("checked") !== $(this).prop("checked")) {
        $(v).click();
      }
    });
  });

  $(".author-export input").click(function () {
    let exportTds = $(".author-export_td input");
    exportTds.each((i, v) => {
      if ($(v).prop("checked") !== $(this).prop("checked")) {
        $(v).click();
      }
    });
  });

  $(".author-import input").click(function () {
    let importTds = $(".author-import_td input");
    importTds.each((i, v) => {
      if ($(v).prop("checked") !== $(this).prop("checked")) {
        $(v).click();
      }
    });
  });

  $('.header-fixed').on('scroll', function(){
    var scroll = $(this).scrollTop();
    if(scroll > 40) {
      $('.header-fixed table tr th').addClass('fixed')
    }
    else{
      $('.header-fixed table tr th').removeClass('fixed')
    }
  })

});
