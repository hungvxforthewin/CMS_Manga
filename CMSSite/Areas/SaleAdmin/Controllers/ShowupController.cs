using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMBussiness.ViewModel;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CRMSite.Areas.HR.Controllers
{
    [Area("SaleAdmin")]
    [Authorize]
    public class ShowupController : BaseController
    {
        private IEvent _iEvent;
        public ShowupController(IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            _iEvent = new EventImp();
            LogModel.ItemName = "event(s)";
        }

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            //write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return View();
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            LogModel.Action = ActionType.Create;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Create");
        }

        [HttpPost]
        public IActionResult Create(EventViewModel model)
        {
            model.CreatedBy = tokenModel.BranchCode;

            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            if (string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("EventViewModel.Name", "Chưa nhập tên sự kiện");
            }

            if (!ModelState.IsValid)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = Helper.GetErrors(ModelState).ToMessageString();
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = Helper.GetErrors(ModelState) });
            }

            var checkInputResult = CheckInput(model);
            if (checkInputResult != null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = checkInputResult.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return checkInputResult;
            }

            model.CreatedForCC = false;
            var create = _iEvent.Create(model);
            if (create.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.CreateFailed;
                LogModel.Message = "Tạo mới sự kiện không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Tạo mới sự kiện không thành công" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.CreateSuccess;
            LogModel.Message = "Tạo mới sự kiện thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Tạo mới sự kiện thành công" });
        }

        [HttpPost]
        public IActionResult CreateForCC(EventViewModel model)
        {
            model.CreatedBy = tokenModel.BranchCode;

            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            if (!ModelState.IsValid)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = Helper.GetErrors(ModelState).ToMessageString();
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = Helper.GetErrors(ModelState) });
            }

            var checkInputResult = CheckInput(model);
            if (checkInputResult != null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = checkInputResult.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return checkInputResult;
            }

            //check exist event
            var getResult = _iEvent.GetByShowUpCode(model.EventTime?.ToString("ddMM") + "EVCC");
            if (getResult.Error)
            {
                //trace log
                LogModel.Action = ActionType.GetInfo;
                LogModel.Data = (new { Code = model.EventTime?.ToString("ddMM") + "EVCC" }).ToDataString();

                //write trace log
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            if (getResult.Result != null && getResult.Result.Count > 0)
            {
                //write trace log
                LogModel.Result = ActionResultValue.CreateFailed;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Đã tồn tại sự kiện cho khách hàng của capital consultant trong ngày đã chọn" } });
            }

            model.CreatedForCC = true;
            var create = _iEvent.Create(model);
            if (create.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.CreateFailed;
                LogModel.Message = "Tạo mới sự kiện không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Tạo mới sự kiện không thành công" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.CreateSuccess;
            LogModel.Message = "Tạo mới sự kiện thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Tạo mới sự kiện thành công" });
        }
        #endregion

        #region Update
        [HttpGet]
        public IActionResult Update(long id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();

            var getResult = _iEvent.Get(id);
            var handleResult = HandleGetResult(getResult);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Update", getResult.Result.First());
        }

        [HttpPost]
        public IActionResult Update(EventViewModel model)
        {
            model.CreatedBy = tokenModel.BranchCode;

            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = model.ToDataString();

            var getResult = _iEvent.Get(model.Id);
            var handleResult = HandleGetResult(getResult);
            if (handleResult != null) return handleResult;
            var currentEvent = getResult.Result.First();

            IShowUpHistory iShowUp = new ShowUpHistoryImp();
            var getShowups = iShowUp.GetCheckinInByShowUpCode(model.CodeEvent);
            if (getShowups.Error)
            {
                //write trace log
                LogModel.ItemName = "expected investor";
                LogModel.Action = ActionType.GetInfo;
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            if (getShowups.Result.Count > 0)
            {
                //write trace log
                LogModel.Result = ActionResultValue.CreateFailed;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Sự kiện đã có khách hàng đăng ký tham gia nên không thể chỉnh sửa được thông tin" } });
            }

            //check input
            if (!ModelState.IsValid)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = Helper.GetErrors(ModelState).ToMessageString();
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = Helper.GetErrors(ModelState) });
            }

            var checkInputResult = CheckInput(model);
            if (checkInputResult != null)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = checkInputResult.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return checkInputResult;
            }

            //check existed event for cc
            if (model.CodeEvent.Contains("CC"))
            {
                //check exist event
                var getCCEventResult = _iEvent.GetByShowUpCode(model.EventTime?.ToString("ddMM") + "EVCC");
                if (getCCEventResult.Error)
                {
                    //trace log
                    LogModel.Action = ActionType.GetInfo;
                    LogModel.Data = (new { Code = model.EventTime?.ToString("ddMM") + "EVCC" }).ToDataString();

                    //write trace log
                    LogModel.Result = ActionResultValue.ThrowException;
                    Logger.LogError(LogModel.ToString());

                    return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
                }

                if (getCCEventResult.Result != null && getCCEventResult.Result.Count > 0 && getCCEventResult.Result.First().Id != model.Id)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.CreateFailed;
                    Logger.LogError(LogModel.ToString());

                    return Json(new { Result = 400, Errors = new List<string> { "Đã tồn tại sự kiện cho khách hàng của capital consultant trong ngày đã chọn" } });
                }
                model.CodeEvent = model.EventTime?.ToString("ddMM") + "EVCC";
                model.Name = "Sự kiện " + model.EventTime?.ToString("dd/MM/yyyy");
            }
            else
            {
                if (model.EventTime.Value.Date != currentEvent.EventTime.Value.Date)
                {
                    string newEventCode = _iEvent.GetEventCode(false, model.EventTime.Value);
                    if (string.IsNullOrEmpty(newEventCode))
                    {
                        //write trace log 
                        LogModel.Result = ActionResultValue.UpdateFailed;
                        LogModel.Message = "Cập nhật sự kiện không thành công!";
                        Logger.LogError(LogModel.ToString());

                        return Json(new { Result = 400, Errors = new List<string> { "Cập nhật sự kiện không thành công" } });
                    }
                    model.CodeEvent = newEventCode;
                }

                if (string.IsNullOrEmpty(model.Name))
                {
                    ModelState.AddModelError("EventViewModel.Name", "Chưa nhập tên sự kiện");
                }
            }

            //reset name for cc event
            //if (model.CodeEvent.Contains("CC"))
            //{
            //    model.Name = "Sự kiện " + model.EventTime.Value.ToString("dd/MM/yyyy");
            //}

            var create = _iEvent.Update(model);
            if (create.Error)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.UpdateFailed;
                LogModel.Message = "Cập nhật sự kiện không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Cập nhật sự kiện không thành công" } });
            }

            //write trace log 
            LogModel.Result = ActionResultValue.UpdateSuccess;
            LogModel.Message = "Cập nhật sự kiện thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Cập nhật sự kiện thành công" });
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(long id)
        {
            //trace log
            LogModel.Data = (new { id = id }).ToDataString();
            LogModel.Action = ActionType.Delete;

            ViewBag.Id = id;
            var bonus = _iEvent.Get(id);
            var handleResult = HandleGetResult(bonus);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Delete");
        }

        [HttpGet]
        public IActionResult ConfirmDelete(long id)
        {
            //trace log
            LogModel.Data = (new { id = id }).ToDataString();
            LogModel.Action = ActionType.Delete;

            var bonus = _iEvent.Get(id);
            var handleResult = HandleGetResult(bonus);
            if (handleResult != null) return handleResult;

            var deleteResult = _iEvent.Delete(id);
            if (deleteResult.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.DeleteFailed;
                LogModel.Message = "Xóa sự kiện không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Xóa sự kiện không thành công" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.DeleteSuccess;
            LogModel.Message = "Xóa sự kiện thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Xóa sự kiện thành công" });
        }
        #endregion

        #region GetList
        [HttpPost]
        public IActionResult GetList(SearchEventModel model)
        {
            // trace log
            model.Branch = tokenModel.BranchCode;
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            if (!string.IsNullOrEmpty(model.StartTimeString) && !string.IsNullOrEmpty(model.StartDateString))
            {
                DateTime startDate;
                bool validStartDate = DateTime.TryParseExact(model.StartDateString, SiteConst.Format.DateFormat, 
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
                if (!validStartDate)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Ngày bắt đầu không đúng định dạng giờ:phút";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { Result = 400, Errors = new List<string> { "Ngày bắt đầu không đúng định dạng giờ:phút" } });
                }
                model.StartDate = startDate;

                TimeSpan startTime;
                bool validStartTime = TimeSpan.TryParseExact(model.StartTimeString, SiteConst.Format.TimeFormat, 
                    CultureInfo.InvariantCulture, TimeSpanStyles.None, out startTime);
                if (!validStartTime)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Thời gian bắt đầu không đúng định dạng ngày/tháng/năm";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { Result = 400, Errors = new List<string> { "Thời gian bắt đầu không đúng định dạng ngày/tháng/năm" } });
                }
                model.StartDate = model.StartDate.Value.Add(startTime);
            }

            if (!string.IsNullOrEmpty(model.EndTimeString) && !string.IsNullOrEmpty(model.EndDateString))
            {
                //end date
                DateTime endDate;
                bool validEndDate = DateTime.TryParseExact(model.EndDateString, SiteConst.Format.DateFormat, 
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
                if (!validEndDate)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Ngày kết thúc không đúng định dạng giờ:phút";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { Result = 400, Errors = new List<string> { "Ngày kết thúc không đúng định dạng giờ:phút" } });
                }
                model.EndDate = endDate;

                TimeSpan endTime;
                bool validEndTime = TimeSpan.TryParseExact(model.EndTimeString, SiteConst.Format.TimeFormat, 
                    CultureInfo.InvariantCulture, TimeSpanStyles.None, out endTime);
                if (!validEndTime)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Thời gian kết thúc không đúng định dạng ngày/tháng/năm";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { Result = 400, Errors = new List<string> { "Thời gian kết thúc không đúng định dạng ngày/tháng/năm" } });
                }
                model.EndDate = model.EndDate.Value.Add(endTime);
            }

            if(model.StartDate != null && model.EndDate != null &&  model.StartDate >= model.EndDate)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Thời gian kết thúc phải lớn hơn thời gian bắt đầu";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Thời gian kết thúc phải lớn hơn thời gian bắt đầu" } });
            }

            var data = _iEvent.GetList(model, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            //if (data.Error)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            //}

            //if (data.Result == null || data.Result.Count == 0)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { SiteConst.NotFoundError } });
            //}

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data.Result, Total = total });
        }
        #endregion

        #region CheckInput
        private IActionResult CheckInput(EventViewModel model)
        {
            //start date
            DateTime startDate;
            bool validStartDate = DateTime.TryParseExact(model.EventDateString, SiteConst.Format.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
            if (!validStartDate)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Ngày bắt đầu không đúng định dạng giờ:phút" } });
            }
            model.EventTime = startDate;

            TimeSpan startTime;
            bool validStartTime = TimeSpan.TryParseExact(model.EventTimeString, SiteConst.Format.TimeFormat, CultureInfo.InvariantCulture, TimeSpanStyles.None, out startTime);
            if (!validStartTime)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Thời gian bắt đầu không đúng định dạng ngày/tháng/năm" } });
            }
            model.EventTime = model.EventTime.Value.Add(startTime);

            //end date
            DateTime endDate;
            bool validEndDate = DateTime.TryParseExact(model.EndDateString, SiteConst.Format.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
            if (!validEndDate)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Ngày kết thúc không đúng định dạng giờ:phút" } });
            }
            model.EndTime = endDate;

            TimeSpan endTime;
            bool validEndTime = TimeSpan.TryParseExact(model.EndTimeString, SiteConst.Format.TimeFormat, CultureInfo.InvariantCulture, TimeSpanStyles.None, out endTime);
            if (!validEndTime)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Thời gian kết thúc không đúng định dạng ngày/tháng/năm" } });
            }
            model.EndTime = model.EndTime.Value.Add(endTime);

            if (model.EventTime >= model.EndTime)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Thời gian kết thúc phải lớn hơn thời gian bắt đầu" } });
            }

            return null;
        }
        #endregion
    }
}
