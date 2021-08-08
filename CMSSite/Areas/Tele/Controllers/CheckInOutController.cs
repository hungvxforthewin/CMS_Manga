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

namespace CRMSite.Areas.Tele.Controllers
{
    [Area("Tele")]
    [Authorize]
    public class CheckInOutController : BaseController
    {
        private IShowUpHistory _iHis;
        public CheckInOutController(IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            _iHis = new ShowUpHistoryImp();
            LogModel.ItemName = "checkin(s)/checkout(s)";
        }

        #region Index
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
        [HttpPost]
        public IActionResult Create(ShowUpHistoryCreateViewModel model)
        {
            //trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.Create;

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

            //var checkInputResult = CheckInput(model);
            //if (checkInputResult != null) return checkInputResult;

            if (string.IsNullOrEmpty(model.TeleSale))
            {
                model.TeleSale = tokenModel.StaffCode;
            }

            var createResult = _iHis.CreateCheckIn(model);
            if (createResult.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.CreateFailed;
                LogModel.Message = "Tạo mới checkin không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Tạo mới checkin không thành công" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.CreateSuccess;
            LogModel.Message = "Tạo mới checkin thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Tạo mới checkin thành công" });
        }
        #endregion

        #region Update
        public IActionResult Update(long id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();

            var getResult = _iHis.GetById(id);
            var handleResult = HandleGetResult(getResult);
            if (handleResult != null) return handleResult;

            ShowUpHistoryCreateViewModel model = getResult.Result.First();

            IInvestor iInvestor = new InvestorImp();
            var getGoWithPersonList = iInvestor.GetGoWithPersons(model.CodeShow);
            if (getGoWithPersonList.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }
            model.Group = getGoWithPersonList.Result.ToList();

            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Update", model);
        }

        [HttpPost]
        public IActionResult Update(ShowUpHistoryCreateViewModel model)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = model.ToDataString();

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

            //var checkInputResult = CheckInput(model);
            //if (checkInputResult != null) return checkInputResult;

            if (string.IsNullOrEmpty(model.TeleSale))
            {
                model.TeleSale = tokenModel.StaffCode;
            }

            model.UserUpdate = tokenModel.StaffCode;
            var updateResult = _iHis.UpdateCheckIn(model);
            if (updateResult.Error)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.UpdateFailed;
                LogModel.Message = "Cập nhật checkin không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Cập nhật checkin không thành công" } });
            }

            //write trace log 
            LogModel.Result = ActionResultValue.UpdateSuccess;
            LogModel.Message = "Cập nhật checkin thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Cập nhật checkin thành công" });
        }
        #endregion

        #region CheckInput
        private IActionResult CheckInput(ShowUpHistoryCreateViewModel model)
        {
            if (!string.IsNullOrEmpty(model.BirthdayString))
            {
                DateTime dob;
                bool validDOBDate = DateTime.TryParseExact(model.BirthdayString, SiteConst.Format.DateFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dob);
                if (!validDOBDate)
                {
                    return Json(new { Result = 400, Errors = new List<string> { "Ngày sinh không đúng định dạng ngày/tháng/năm" } });
                }
                model.Birthday = dob;
            }

            //check phone number participate in show up
            List<string> phones = new List<string>();
            if (model.Group != null)
            {
                var goWithPhones = model.Group.Select(x => x.PhoneNumber).ToList();
                phones.AddRange(goWithPhones);
            }
            var checkSampePhoneNumber = phones.GroupBy(x => x).ToList();
            if (checkSampePhoneNumber.Count != phones.Count)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Số điện thoại không được trùng nhau" } });
            }

            //check show up info
            IEvent iEvent = new EventImp();
            var eventInfo = iEvent.GetByShowUpCode(model.CodeEvent);
            if (eventInfo.Error || eventInfo.Result == null)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Không tìm thấy thông tin show up" } });
            }

            if (eventInfo.Result.First().EndTime <= DateTime.Now)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Show up đã kết thúc. Vui lòng chọn show up khác" } });
            }


            //var checkPhoneNoOfInvestorResult = _iHis.IsExistedInvestorInfo(model.PhoneNumber, model.Id);
            //if (checkPhoneNoOfInvestorResult.Error)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            //}

            //if (checkPhoneNoOfInvestorResult.Result != null || checkPhoneNoOfInvestorResult.Result.Count > 0)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { "Số điện thoại của khách hàng đã tồn tại trên hệ thống" } });
            //}

            //foreach (var item in model.Group)
            //{
            //    var checkPhoneNoOfGoWithPersonResult = _iHis.IsExistedInvestorInfo(model.PhoneNumber, model.Id);
            //    if (checkPhoneNoOfGoWithPersonResult.Error)
            //    {
            //        return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            //    }

            //    if (checkPhoneNoOfGoWithPersonResult.Result != null || checkPhoneNoOfGoWithPersonResult.Result.Count > 0)
            //    {
            //        return Json(new { Result = 400, Errors = new List<string> { "Số điện thoại của khách hàng đi cùng đã tồn tại trên hệ thống" } });
            //    }
            //}

            return null;
        }
        #endregion

        #region View
        public IActionResult View(long id)
        {
            //trace log
            LogModel.Data = (new { id = id }).ToDataString();
            LogModel.Action = ActionType.ViewInfo;

            var getResult = _iHis.GetById(id);
            var handleResult = HandleGetResult(getResult);
            if (handleResult != null) return handleResult;

            ShowUpHistoryCreateViewModel model = getResult.Result.First();

            IInvestor iInvestor = new InvestorImp();
            var getGoWithPersonList = iInvestor.GetGoWithPersons(model.CodeShow);
            if (getGoWithPersonList.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }
            model.Group = getGoWithPersonList.Result.ToList();

            //write trace log
            LogModel.Result = ActionResultValue.ViewSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_View", model);
        }
        #endregion

        #region GetList
        [HttpPost]
        public IActionResult GetList(SearchShowUpHistoryModel model)
        {
            model.TeleSale = tokenModel.StaffCode;

            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            //validate checkin date
            if (!string.IsNullOrEmpty(model.CheckIn))
            {
                DateTime checkinDate;
                bool validCheckinDate = DateTime.TryParseExact(model.CheckIn, SiteConst.Format.DateFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out checkinDate);
                if (!validCheckinDate)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Ngày checkin không đúng định dạng ngày/tháng/năm";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { Result = 400, Errors = new List<string> { "Ngày checkin không đúng định dạng ngày/tháng/năm" } });
                }
                model.CheckInDate = checkinDate;
            }

            // validate checkout date
            if (!string.IsNullOrEmpty(model.CheckOut))
            {
                DateTime checkoutDate;
                bool validCheckoutDate = DateTime.TryParseExact(model.CheckOut, SiteConst.Format.DateFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out checkoutDate);
                if (!validCheckoutDate)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Ngày checkin không đúng định dạng ngày/tháng/năm";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { Result = 400, Errors = new List<string> { "Ngày checkout không đúng định dạng ngày/tháng/năm" } });
                }
                model.CheckOutDate = checkoutDate;
            }

            int total;
            var data = _iHis.GetList(model, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data.Result, Total = total });
        }
        #endregion
    }
}
