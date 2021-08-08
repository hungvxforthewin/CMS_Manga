using ClosedXML.Excel;
using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMBussiness.ViewModel;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CRMSite.Areas.HR.Controllers
{
    [Area("HR")]
    public class TimeKeepingController : BaseController
    {
        private ITimeKeeping _iTimeKeeping;
        private IWebHostEnvironment _env;

        public TimeKeepingController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env, ILogger<BaseController> logger)
            : base(httpContextAccessor, logger)
        {
            _iTimeKeeping = new TimeKeepingImp();
            _env = env;
            LogModel.ItemName = "timekeeping data";
        }

        #region Index - khanhkk
        [HttpGet]
        public IActionResult Index(string month = "", string key = "")
        {
            if (tokenModel == null)
            {
                return Redirect("/Login/LoginAccount");
            }

            var data = _iTimeKeeping.GetInfoByMonthAndKey(month, key);

            //write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Result = ActionResultValue.AccessSuccess;
            LogModel.Data = data.DataItem?.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return View(data.DataItem);
        }
        #endregion

        #region GetList - khanhkk
        public IActionResult GetList(string month, string key)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { month = month, key = key }).ToDataString();

            //string[] strs = month.Split(SiteConst.SlashChar);

            var data = _iTimeKeeping.GetInfoByMonthAndKey(month, key);
            if (data.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            if (data.DataItem == null || data.DataItem.TimeKeepings == null || data.DataItem.TimeKeepings.Count == 0)
            {
                //write trace log
                LogModel.Result = ActionResultValue.NotFoundData;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.NotFoundError } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.DataItem.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data.DataItem });
        }
        #endregion

        //#region UploadTimeKeeping - khanhkk
        //[HttpPost]
        //[RequestFormSizeLimit(valueCountLimit: 4096)]
        //public IActionResult UploadTimeKeeping(List<TimeKeepingViewModel> data)
        //{
        //    //trace log
        //    LogModel.Action = ActionType.Create;
        //    LogModel.Data = data.ToDataString();

        //    if (!ModelState.IsValid)
        //    {
        //        //write trace log 
        //        LogModel.Result = ActionResultValue.InvalidInput;
        //        LogModel.Message = Helper.GetErrors(ModelState).ToMessageString();
        //        Logger.LogWarning(LogModel.ToString());

        //        return Json(new { Result = 400, Errors = Helper.GetErrors(ModelState) });
        //    }

        //    data = data.Where(x => x.Id == 0 && x.TotalWorkingDays > 0).ToList();

        //    if (data == null || data.Count == 0)
        //    {
        //        //write trace log 
        //        LogModel.Result = ActionResultValue.InvalidInput;
        //        LogModel.Message = "Dữ liệu khai báo chấm công của nhân viên đang trống!";
        //        Logger.LogWarning(LogModel.ToString());

        //        return Json(new
        //        {
        //            Result = 400,
        //            Errors =
        //            new List<string> { "Dữ liệu khai báo chấm công của nhân viên đang trống!" }
        //        });
        //    }

        //    //check input data
        //    DateTime selectedMonth = DateTime.Now;
        //    foreach (var item in data)
        //    {
        //        var checkInputResult = CheckInput(item, out selectedMonth);
        //        if (checkInputResult != null)
        //        {
        //            //write trace log 
        //            LogModel.Result = ActionResultValue.InvalidInput;
        //            LogModel.Message = checkInputResult.ToDataString();
        //            Logger.LogWarning(LogModel.ToString());

        //            return checkInputResult;
        //        }
        //    }

        //    // check current selected month
        //    //DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    //if (selectedMonth >= currentMonth)
        //    //{
        //    //    return Json(new
        //    //    {
        //    //        Result = 400,
        //    //        Errors =
        //    //        new List<string> { "Chỉ khai báo chấm công cho các tháng trước đây!" }
        //    //    });
        //    //}

        //    //check common setup this month
        //    IRuleInMonth iRule = new RuleInMonthImp();
        //    var getRule = iRule.GetByMonth(data.First().Month);
        //    if (getRule.Error)
        //    {
        //        //write trace log
        //        LogModel.Result = ActionResultValue.ThrowException;
        //        Logger.LogError(LogModel.ToString());

        //        return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
        //    }

        //    if (getRule.Result == null || getRule.Result.Count == 0)
        //    {
        //        //write trace log
        //        LogModel.Result = ActionResultValue.NotFoundData;
        //        LogModel.Message = "Chưa setup chung cho tháng khai báo chấm công";
        //        Logger.LogWarning(LogModel.ToString());

        //        return Json(new { Result = 400, Errors = new List<string> { "Chưa setup chung cho tháng khai báo chấm công" } });
        //    }

        //    // save timekeeping for all employees in month
        //    var resultUpload = _iTimeKeeping.AddTimeKeeping(data);
        //    if (resultUpload.Error)
        //    {
        //        //write trace log 
        //        LogModel.Result = ActionResultValue.CreateFailed;
        //        LogModel.Message = "Khai báo chấm công của nhân viên không thành công!";
        //        Logger.LogError(LogModel.ToString());

        //        return Json(new { Result = 400, Errors =
        //            new List<string> { "Khai báo chấm công của nhân viên không thành công!" } });
        //    }

        //    //write trace log 
        //    LogModel.Result = ActionResultValue.CreateSuccess;
        //    LogModel.Message = "Khai báo chấm công của nhân viên thành công!";
        //    Logger.LogInformation(LogModel.ToString());

        //    return Json(new { Result = 200, Message = "Khai báo chấm công của nhân viên thành công!" });
        //}
        //#endregion

        #region GetById - khanhkk
        public IActionResult GetById(long id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _iTimeKeeping.GetTimeKeepingOfEmployee(id);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data.Result });
        }
        #endregion 

        //#region UpdateTimeKeeping - khanhkk
        //public IActionResult Update(long id)
        //{
        //    //trace log
        //    LogModel.Action = ActionType.Update;
        //    LogModel.Data = (new { id = id }).ToDataString();

        //    var data = _iTimeKeeping.GetTimeKeepingOfEmployee(id);
        //    var handleResult = HandleGetResult(data);
        //    if (handleResult != null) return handleResult;

        //    //write trace log
        //    LogModel.Result = ActionResultValue.AccessSuccess;
        //    Logger.LogInformation(LogModel.ToString());

        //    return PartialView("_Update", data.Result.First());
        //}

        //[HttpPost]
        //public IActionResult UpdateTimeKeeping(TimeKeepingViewModel data)
        //{
        //    //trace log
        //    LogModel.Data = data.ToDataString();
        //    LogModel.Action = ActionType.Update;

        //    if (!ModelState.IsValid)
        //    {
        //        //write trace log 
        //        LogModel.Result = ActionResultValue.InvalidInput;
        //        LogModel.Message = Helper.GetErrors(ModelState).ToMessageString();
        //        Logger.LogWarning(LogModel.ToString());

        //        return Json(new { Result = 400, Errors = Helper.GetErrors(ModelState) });
        //    }

        //    //check input data
        //    DateTime selectedMonth = DateTime.Now;
        //    var checkInputResult = CheckInput(data, out selectedMonth);
        //    if (checkInputResult != null)
        //    {
        //        //write trace log 
        //        LogModel.Result = ActionResultValue.InvalidInput;
        //        LogModel.Message = checkInputResult.ToDataString();
        //        Logger.LogWarning(LogModel.ToString());

        //        return checkInputResult;
        //    }


        //    // check current selected month
        //    //DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    //if (selectedMonth >= currentMonth)
        //    //{
        //    //    return Json(new
        //    //    {
        //    //        Result = 400,
        //    //        Errors =
        //    //        new List<string> { "Chỉ khai báo chấm công cho các tháng trước đây!" }
        //    //    });
        //    //}

        //    // update timekeeping for an emloyee
        //    var resultUpload = _iTimeKeeping.UpdateTimeKeepingOfEmployee(data);
        //    if (resultUpload.Error)
        //    {
        //        //write trace log 
        //        LogModel.Result = ActionResultValue.UpdateFailed;
        //        LogModel.Message = "Cập nhật chấm công của nhân viên không thành công!";
        //        Logger.LogError(LogModel.ToString());

        //        return Json(new
        //        {
        //            Result = 400,
        //            Errors =
        //            new List<string> { "Cập nhật chấm công của nhân viên không thành công!" }
        //        });
        //    }

        //    //write trace log 
        //    LogModel.Result = ActionResultValue.UpdateSuccess;
        //    LogModel.Message = "Cập nhật chấm công của nhân viên thành công!";
        //    Logger.LogInformation(LogModel.ToString());

        //    return Json(new { Result = 200, Message = "Cập nhật chấm công của nhân viên thành công!" });
        //}
        //#endregion

        #region SetupRuleInMonth - khanhkk
        [HttpPost]
        public IActionResult SetupRuleInMonth(RuleInMonthViewModel data)
        {
            //trace log
            LogModel.Action = ActionType.Create;
            LogModel.ItemName = "rule in month";
            LogModel.Data = data.ToDataString();

            if (!ModelState.IsValid)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = Helper.GetErrors(ModelState).ToMessageString();
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = Helper.GetErrors(ModelState) });
            }

            //string[] strs = data.Month.Split(SiteConst.SubstractChar);
            //data.Month = strs[1] + SiteConst.SlashChar + strs[0];
            // check the working days in month for all
            if (data.TotalWorkingDays > 31)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Số công chuẩn không thể lớn hơn 31!";
                Logger.LogWarning(LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Số công chuẩn không thể lớn hơn 31!" }
                });
            }

            // check other bonus for all
            if (data.OtherBonus.HasValue && data.OtherBonus < 0) 
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Thưởng khác không thể nhỏ hơn 0!";
                Logger.LogWarning(LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Thưởng khác không thể nhỏ hơn 0!" }
                });
            }

            data.Month = data.Month.Replace(SiteConst.SubstractChar, SiteConst.SlashChar);

            // check format of selected month
            DateTime selectedMonth;
            bool checkMonthValue = DateTime.TryParseExact(data.Month, SiteConst.Format.MonthYearFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedMonth);
            if (!checkMonthValue)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Tháng đã chọn không đúng format năm tháng!";
                Logger.LogWarning(LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Tháng đã chọn không đúng format năm tháng!" }
                });
            }
            // check current selected month 
            //DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //if (selectedMonth >= currentMonth)
            //{
            //    return Json(new
            //    {
            //        Result = 400,
            //        Errors =
            //        new List<string> { "Khai báo chung chỉ dành cho các tháng tới!" }
            //    });
            //}

            //save rule in month
            IRuleInMonth iRule = new RuleInMonthImp();
            var resultUpload = iRule.Setup(data);
            if (resultUpload.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.CreateFailed;
                LogModel.Message = "Khai báo chung cho nhân viên không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Khai báo chung cho nhân viên không thành công!" }
                });
            }
            //write trace log
            LogModel.Result = ActionResultValue.CreateSuccess;
            LogModel.Message = "Khai báo chung cho nhân viên thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Khai báo chung cho nhân viên thành công!" });
        }
        #endregion

        #region UpdateBonus - khanhkk
        [HttpPost]
        public IActionResult UpdateBonus(decimal? otherBonus, long id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id, OtherBonus = otherBonus }).ToDataString();
            LogModel.ItemName = "other bonus";

            // check other bonus for all
            if (otherBonus.HasValue && otherBonus < 0)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Thưởng khác không thể nhỏ hơn 0!";
                Logger.LogWarning(LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Thưởng khác không thể nhỏ hơn 0!" }
                });
            }

            //// check existed info
            //var data = _iTimeKeeping.GetTimeKeepingOfEmployee(id);
            //if (data.Error)
            //{
            //    return Json(new { Result = 400, Errors = "Lỗi hệ thống" });
            //}

            //if (data.Result == null || data.Result.Count == 0)
            //{
            //    return Json(new { Result = 400, Errors = "Không tìm thấy dữ liệu chấm công" });
            //}

            //save rule in month
            IRuleInMonth iRule = new RuleInMonthImp();
            var resultUpload = _iTimeKeeping.UpdateOtherBonus(otherBonus, id);
            if (resultUpload.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.UpdateFailed;
                LogModel.Message = "Cập nhật thưởng khác cho nhân viên không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Cập nhật thưởng khác cho nhân viên không thành công!" }
                });
            }

            //write trace log
            LogModel.Result = ActionResultValue.UpdateSuccess;
            LogModel.Message = "Cập nhật thưởng khác cho nhân viên thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Cập nhật thưởng khác cho nhân viên thành công!" });
        }
        #endregion

        #region Import
        public IActionResult GetEmployeeListFile()
        {
            //trace log
            LogModel.Action = ActionType.Export;

            IAccount iAcc = new AccountImp();
            var data = iAcc.GetEmployeeList();
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;


            using (var workbook = new XLWorkbook(Path.Combine(_env.WebRootPath, "Content/MauChamCongHoangNgan.xlsx")))
            {
                var worksheet = workbook.Worksheets.ElementAt(0);
                var currentRow = 4;
                foreach (var user in data.Result)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 2).Value = user.STT;
                    worksheet.Cell(currentRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(currentRow, 3).Value = user.CodeStaff;
                    worksheet.Cell(currentRow, 4).Value = user.FullName;
                    worksheet.Cell(currentRow, 5).Value = Helper.GetRoleName(user.RoleAccount);
                    worksheet.Cell(currentRow, 6).Value = string.Empty;
                    worksheet.Cell(currentRow, 7).Value = string.Empty;
                    worksheet.Cell(currentRow, 8).Value = string.Empty;
                    worksheet.Cell(currentRow, 9).Value = string.Empty;
                    worksheet.Cell(currentRow, 10).Value = string.Empty;
                    worksheet.Cell(currentRow, 11).Value = string.Empty;
                    worksheet.Cell(currentRow, 12).Value = string.Empty;
                    worksheet.Cell(currentRow, 13).Value = string.Empty;
                    worksheet.Cell(currentRow, 14).Value = string.Empty;
                }
                worksheet.Range("B5:N" + currentRow).Cells().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    //write trace log
                    LogModel.Result = ActionResultValue.ImportSuccess;
                    Logger.LogInformation(LogModel.ToString());

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "TimeKeeping.xlsx");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file, string month)
        {
            //trace log
            LogModel.Action = ActionType.Import;
            LogModel.Data = (new { file = file, month = month }).ToDataString();

            //valid model data
            if (file == null)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Chưa chọn file chứa dữ liệu chấm công!";
                Logger.LogWarning(LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Chưa chọn file chứa dữ liệu chấm công!" }
                });
            }

            //check file type
            if (Path.GetExtension(file.FileName).ToLower() != ".xlsx" && Path.GetExtension(file.FileName).ToLower() != ".xls")
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Chức năng nhập dữ liệu chỉ nhận file excel!";
                Logger.LogWarning(LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Chức năng nhập dữ liệu chỉ nhận file excel!" }
                });
            }

            //check select month
            if (string.IsNullOrEmpty(month))
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Chưa chọn tháng chấm công!";
                Logger.LogWarning(LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Chưa chọn tháng chấm công!" }
                });
            }

            //string[] strs = month.Split(SiteConst.SubstractChar);
            //month = strs[1] + SiteConst.SlashChar + strs[0];
            //check format of month
            DateTime selectedMonth;
            bool checkMonthValue = DateTime.TryParseExact(month, SiteConst.Format.MonthYearFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedMonth);
            if (!checkMonthValue)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Thông tin tháng chấm công không đúng format năm tháng!";
                Logger.LogWarning(LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Thông tin tháng chấm công không đúng format năm tháng!" }
                });
            }

            // check current selected month
            DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (selectedMonth >= currentMonth)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Chỉ khai báo chấm công cho các tháng trước đây!";
                Logger.LogWarning(LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Chỉ khai báo chấm công cho các tháng trước đây!" }
                });
            }

            var filePath = Path.Combine(_env.WebRootPath, "Content",
            Path.GetRandomFileName() + Path.GetExtension(file.FileName));

            try
            {
                //save temp file
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }

                List<TimeKeepingViewModel> listData = new List<TimeKeepingViewModel>();
                //import data timekeeping
                using (var excelWorkbook = new XLWorkbook(filePath))
                {
                    int stt = 0;
                    var nonEmptyDataRows = excelWorkbook.Worksheet(1).RowsUsed().Skip(2);
                    foreach (var dataRow in nonEmptyDataRows)
                    {
                        stt++;
                        string staffCode = dataRow.Cell(3).Value.ToString();
                        if (string.IsNullOrEmpty(staffCode))
                        {
                            //write trace log 
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Mã nhân viên không được để trống ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Mã nhân viên không được để trống ở dòng " + stt }
                            });
                        }

                        string fullName = dataRow.Cell(4).Value.ToString();
                        if (string.IsNullOrEmpty(fullName))
                        {
                            //write trace log 
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Họ tên nhân viên không được để trống ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Họ tên nhân viên không được để trống ở dòng " + stt }
                            });
                        }

                        byte role = Helper.GetRole(dataRow.Cell(5).Value.ToString());
                        if (role == 0)
                        {
                            //write trace log 
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Không tìm thấy chức vụ của nhân viên ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Không tìm thấy chức vụ của nhân viên ở dòng " + stt }
                            });
                        }

                        // valid upon info
                        IAccount iAcc = new AccountImp();
                        var getStaffInfo = iAcc.GetEmployeeInfoByStaffCode(staffCode);
                        if (getStaffInfo.Error)
                        {
                            //write trace log
                            LogModel.Result = ActionResultValue.ThrowException;
                            Logger.LogError(LogModel.ToString());

                            return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
                        }

                        if (getStaffInfo.Result == null || getStaffInfo.Result.Count == 0)
                        {
                            //write trace log
                            LogModel.Result = ActionResultValue.NotFoundData;
                            LogModel.Message = "Không tìm thấy thông tin nhân viên với mã nhân viên là: " + staffCode;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new { Result = 400, Errors = new List<string> { "Không tìm thấy thông tin nhân viên với mã nhân viên là: " + staffCode } });
                        }

                        if (getStaffInfo.Result.First().RoleAccount != role || getStaffInfo.Result.First().FullName.Trim().ToLower() != fullName.Trim().ToLower())
                        {
                            //write trace log
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Thông tin nhân viên với mã nhân viên là: " + staffCode + " không khớp với thông tin trên hệ thống";
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new { Result = 400, Errors = new List<string> { "Thông tin nhân viên với mã nhân viên là: " + staffCode + " không khớp với thông tin trên hệ thống" } });
                        }

                        //check total working days
                        if (string.IsNullOrEmpty(dataRow.Cell(6).Value.ToString()))
                        {
                            continue;
                        }
                        float totalWorking;
                        bool validTotalWorkingDays = float.TryParse(dataRow.Cell(6).Value.ToString(), out totalWorking);
                        if (!validTotalWorkingDays)
                        {
                            //write trace log
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Số ngày làm việc thực tế không đúng định dạng ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Số ngày làm việc thực tế không đúng định dạng ở dòng " + stt }
                            });
                        }
                        if (totalWorking <= 0) {
                            continue;
                        }

                        //check total late
                        byte totalLate;
                        bool validTotalLate = byte.TryParse(dataRow.Cell(7).Value.ToString(), out totalLate);
                        if (!validTotalLate)
                        {
                            //write trace log
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Số lần đi muộn không đúng định dạng ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Số lần đi muộn không đúng định dạng ở dòng " + stt }
                            });
                        }

                        if (totalLate > totalWorking)
                        {
                            //write trace log
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Số lần đi muộn không lớn hơn số ngày công thực tế ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Số lần đi muộn không lớn hơn số ngày công thực tế ở dòng " + stt }
                            });
                        }

                        //check total leave early
                        byte totalEarlyOuts;
                        bool validTotalEarlyOuts = byte.TryParse(dataRow.Cell(8).Value.ToString(), out totalEarlyOuts);
                        if (!validTotalEarlyOuts)
                        {
                            //write trace log
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Số lần về sớm không đúng định dạng ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Số lần về sớm không đúng định dạng ở dòng " + stt }
                            });
                        }

                        if (totalEarlyOuts > totalWorking)
                        {
                            //write trace log
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Số lần về sớm không lớn hơn số ngày công thực tế ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Số lần về sớm không lớn hơn số ngày công thực tế ở dòng " + stt }
                            });
                        }

                        //check total off without reason
                        byte totalOffWithoutReason;
                        bool validTotalOffWithoutReason = byte.TryParse(dataRow.Cell(9).Value.ToString(), out totalOffWithoutReason);
                        if (!validTotalOffWithoutReason)
                        {
                            //write trace log
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Số ngày nghỉ không lý do không đúng định dạng ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Số ngày nghỉ không lý do không đúng định dạng ở dòng " + stt }
                            });
                        }

                        //check total off without reason
                        byte totalForgetCheckIn;
                        bool validTotalForgetCheckIn = byte.TryParse(dataRow.Cell(10).Value.ToString(), out totalForgetCheckIn);
                        if (!validTotalForgetCheckIn)
                        {
                            //write trace log
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Số lần quên check in/check out không đúng định dạng ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Số lần quên check in/check out không đúng định dạng ở dòng " + stt }
                            });
                        }

                        //check total take leave days
                        float totalTakeLeave;
                        bool validTotalTakeLeave = float.TryParse(dataRow.Cell(11).Value.ToString(), out totalTakeLeave);
                        if (!validTotalTakeLeave)
                        {
                            //write trace log
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Số ngày nghỉ phép không đúng định dạng ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Số ngày nghỉ phép không đúng định dạng ở dòng " + stt }
                            });
                        }

                        //check total show up
                        float totalShowUp;
                        bool validTotalShowUp = float.TryParse(dataRow.Cell(12).Value.ToString(), out totalShowUp);
                        if (!validTotalShowUp)
                        {
                            //write trace log
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Số showup đạt được không đúng định dạng ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Số showup đạt được không đúng định dạng ở dòng " + stt }
                            });
                        }

                        //check total contract
                        float totalContract;
                        bool validTotalContract = float.TryParse(dataRow.Cell(13).Value.ToString(), out totalContract);
                        if (!validTotalContract)
                        {
                            //write trace log
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Số hợp đồng đạt được không đúng định dạng ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Số hợp đồng đạt được không đúng định dạng ở dòng " + stt }
                            });
                        }

                        //check total revenue
                        decimal totalRevenue;
                        bool validTotalRevenue = decimal.TryParse(dataRow.Cell(14).Value.ToString(), out totalRevenue);
                        if (!validTotalRevenue)
                        {
                            //write trace log
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Doanh số đạt được không đúng định dạng ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Doanh số đạt được không đúng định dạng ở dòng " + stt }
                            });
                        }

                        TimeKeepingViewModel model = new TimeKeepingViewModel
                        {
                            Month = month,
                            CodeStaff = staffCode,
                            FullName = fullName,
                            TotalWorkingDays = totalWorking,
                            TotalLates = totalLate,
                            TotalEarlyOuts = totalEarlyOuts,
                            TotalWithoutReason = totalOffWithoutReason,
                            ForgetCheckOutIn = totalForgetCheckIn,
                            TotalTakeLeaveInMonth = totalTakeLeave,
                            TotalShowupInMonth = totalShowUp,
                            TotalContract = totalContract,
                            RevenueInMonth = totalRevenue,
                            RoleAccount = role,
                        };

                        listData.Add(model);
                    }
                }

                //check common setup this month
                IRuleInMonth iRule = new RuleInMonthImp();
                var getRule = iRule.GetByMonth(listData.First().Month);
                if (getRule.Error)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.ThrowException;
                    Logger.LogError(LogModel.ToString());

                    return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
                }

                if (getRule.Result == null || getRule.Result.Count == 0)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.NotFoundData;
                    LogModel.Message = "Chưa setup chung cho tháng khai báo chấm công";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { Result = 400, Errors = new List<string> { "Chưa setup chung cho tháng khai báo chấm công" } });
                }

                //import data
                var resultUpload = _iTimeKeeping.ImportTimeKeeping(listData);
                if (resultUpload.Error)
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.ImportFailed;
                    LogModel.Message = "Khai báo chấm công của nhân viên không thành công!";
                    Logger.LogError(LogModel.ToString());

                    return Json(new
                    {
                        Result = 400,
                        Errors =
                        new List<string> { "Khai báo chấm công của nhân viên không thành công!" }
                    });
                }

            }
            catch (Exception ex)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.ImportFailed;
                LogModel.Message = "Khai báo chấm công của nhân viên không thành công!";
                Logger.LogError(ex, LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Khai báo chấm công cho nhân viên không thành công!" }
                });
            }
            finally
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            //write trace log 
            LogModel.Result = ActionResultValue.ImportSuccess;
            LogModel.Message = "Khai báo chấm công của nhân viên thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message="Nhập dữ liệu bảng chấm công thành công!" });
        }
        #endregion

        #region CheckInput
        private IActionResult CheckInput(TimeKeepingViewModel data, out DateTime month)
        {
            month = DateTime.Now;
            // check total working days
            if (data.TotalWorkingDays == null || data.TotalWorkingDays == 0)
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Số ngày làm việc không được để trống!" }
                });
            }

            if (data.TotalWorkingDays > 31)
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Số ngày làm việc không thể lớn hơn 31!" }
                });
            }

            if (data.TotalWorkingDays < 0)
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Số ngày làm việc không thể nhỏ hơn 0!" }
                });
            }

            // check total early outs
            if (data.TotalEarlyOuts < 0)
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Số lần về sớm không thể nhỏ hơn 0!" }
                });
            }

            // check total lates
            if (data.TotalLates < 0)
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Số lần đi muộn không thể nhỏ hơn 0!" }
                });
            }

            //check total widthout reason
            if (data.TotalWithoutReason < 0)
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Số lần nghỉ không lý do không thể nhỏ hơn 0!" }
                });
            }

            //check forget check out in
            if (data.ForgetCheckOutIn < 0)
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Số lần quên checkin, checkout không thể nhỏ hơn 0!" }
                });
            }

            // check allow leave days
            if (data.TotalTakeLeaveInMonth < 0)
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Số ngày nghỉ có phép không thể nhỏ hơn 0!" }
                });
            }

            // check total show up
            if ((data.RoleAccount == 8 || data.RoleAccount == 9) && !data.TotalShowupInMonth.HasValue)
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Chưa nhập số show up!" }
                });
            }
            if (data.TotalShowupInMonth < 0)
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Số show up không thể nhỏ hơn 0!" }
                });
            }

            // check total 
            if ((data.RoleAccount == 8 || data.RoleAccount == 9) && !data.TotalContract.HasValue)
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Chưa nhập số hợp đồng!" }
                });
            }
            if (data.TotalContract < 0)
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Số hợp đồng không thể nhỏ hơn 0!" }
                });
            }

            // check revenue
            if ((data.RoleAccount != 8 && data.RoleAccount != 9) && !data.RevenueInMonth.HasValue)
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Chưa nhập tổng doanh thu!" }
                });
            }
            if (data.RevenueInMonth < 0)
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Tổng doanh thu không thể nhỏ hơn 0!" }
                });
            }

            //check format of month
            //DateTime selectedMonth;
            bool checkMonthValue = DateTime.TryParseExact(data.Month, SiteConst.Format.MonthYearFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out month);
            if (!checkMonthValue)
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Thông tin tháng chấm công không đúng format năm tháng!" }
                });
            }

            return null;
        }
        #endregion

        #region UpdateNewTimeKeeping - khanhkk
        [HttpPost]
        public IActionResult UpdateNewTimeKeeping(TimeKeepingViewModel data)
        {
            //trace log
            LogModel.Action = ActionType.Create;
            LogModel.Data = data.ToDataString();

            if (!ModelState.IsValid)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = Helper.GetErrors(ModelState).ToMessageString();
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = Helper.GetErrors(ModelState) });
            }

            //check input data
            DateTime selectedMonth = DateTime.Now;
            var checkInputResult = CheckInput(data, out selectedMonth);
            if (checkInputResult != null)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = checkInputResult.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return checkInputResult;
            }

            //check current selected month
            DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (selectedMonth >= currentMonth)
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Chỉ khai báo chấm công cho các tháng trước đây!" }
                });
            }

            //check existed data
            var checkExistedData = _iTimeKeeping.CheckExistedTimekeepingInMonth(data.Month, data.CodeStaff);
            if (checkExistedData.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            //check existed timekeeping info
            if (checkExistedData.Result.First())
            {
                //write trace log
                LogModel.Result = ActionResultValue.UpdateFailed;
                LogModel.Message = "Thông tin chấm công đã tồn tại trên hệ thống!";
                Logger.LogError(LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Thông tin chấm công đã tồn tại trên hệ thống!" }
                });
            }

            //check common setup this month
            IRuleInMonth iRule = new RuleInMonthImp();
            var getRule = iRule.GetByMonth(data.Month);
            if (getRule.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            if (getRule.Result == null || getRule.Result.Count == 0)
            {
                //write trace log
                LogModel.Result = ActionResultValue.NotFoundData;
                LogModel.Message = "Chưa setup chung cho tháng khai báo chấm công";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Chưa setup chung cho tháng khai báo chấm công" } });
            }

            // save timekeeping for all employees in month
            data.CodeKeeping = Guid.NewGuid().ToString();
            var resultUpload = _iTimeKeeping.AddNewTimeKeeping(data);
            if (resultUpload.Error)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.CreateFailed;
                LogModel.Message = "Khai báo chấm công của nhân viên không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Khai báo chấm công của nhân viên không thành công!" }
                });
            }

            //write trace log 
            LogModel.Result = ActionResultValue.CreateSuccess;
            LogModel.Message = "Khai báo chấm công của nhân viên thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = new { CodeKeeping = data.CodeKeeping }, Message = "Khai báo chấm công của nhân viên thành công!" });
        }

        [HttpPost]
        public IActionResult UpdateExistedTimeKeeping(TimeKeepingViewModel data)
        {
            //trace log
            LogModel.Data = data.ToDataString();
            LogModel.Action = ActionType.Update;

            if (!ModelState.IsValid)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = Helper.GetErrors(ModelState).ToMessageString();
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = Helper.GetErrors(ModelState) });
            }

            //check input data
            DateTime selectedMonth = DateTime.Now;
            var checkInputResult = CheckInput(data, out selectedMonth);
            if (checkInputResult != null)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = checkInputResult.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return checkInputResult;
            }

            //check current selected month
            DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (selectedMonth >= currentMonth)
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Chỉ khai báo chấm công cho các tháng trước đây!" }
                });
            }

            // update timekeeping for an emloyee
            var resultUpload = _iTimeKeeping.UpdateTimeKeeping(data);
            if (resultUpload.Error)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.UpdateFailed;
                LogModel.Message = "Cập nhật chấm công của nhân viên không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Cập nhật chấm công của nhân viên không thành công!" }
                });
            }

            //write trace log 
            LogModel.Result = ActionResultValue.UpdateSuccess;
            LogModel.Message = "Cập nhật chấm công của nhân viên thành công";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Cập nhật chấm công của nhân viên thành công" });
        }
        #endregion
    }
}
