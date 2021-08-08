using ClosedXML.Excel;
using CRMBussiness;
using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMBussiness.ViewModel;
using CRMModel.Models.Data;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CRMSite.Areas.Tele.Controllers
{
    [Area("Tele")]
    [Authorize]
    public class CallManagementsController : BaseController
    {
        private readonly IShowUpHistory _iHis;
        private readonly IWebHostEnvironment _env;
        private readonly IProduct _productService;
        private readonly IEvent _eventService;
        private readonly ILevelConcern _levelConcernServicel;
        private readonly IInvestorsCareHistory _investorsCareHistory;
        private readonly IInvestorsCareHistoryDetail _investorsCareHistoryDetail;
        private readonly IInvestor _investorService; 

        public CallManagementsController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env, ILogger<BaseController> logger, IInvestor investor, IProduct product, IEvent eventSevice, ILevelConcern levelConcern, IInvestorsCareHistory investorsCareHistory, IInvestorsCareHistoryDetail investorsCareHistoryDetail) : base(httpContextAccessor, logger)
        {
            _iHis = new ExShowUpHistoryImp();
            _env = env;
            LogModel.ItemName = "call managements";
            _productService = product;
            _eventService = eventSevice;
            _levelConcernServicel = levelConcern;
            _investorsCareHistory = investorsCareHistory;
            _investorsCareHistoryDetail = investorsCareHistoryDetail;
            _investorService = investor;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetList(SearchInvestorsCareHistoryViewModel model)
        {
            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            var data = _investorsCareHistory.GetList(model, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Data = data.Result, Total = total });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertOrUpdate(InvestorsCareHistoryViewModel model)
        {
            //if (!Utils.CheckDateTime(model.CreatedDate.Value.ToString()))
            //{
            //    var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "RevenueDate", ErrorMessage = "Ngày tháng không đúng định dạng dd-MM-yyyy" } };
            //    var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

            //    //write trace log 
            //    LogModel.Result = ActionResultValue.InvalidInput;
            //    LogModel.Message = jsonerrs;
            //    Logger.LogWarning(LogModel.ToString());

            //    return Json(new { status = false, data = jsonerrs });
            //}
            if(model.detailCallCareHistories.Count > 0)
            {
                foreach (var item in model.detailCallCareHistories)
                {
                    if (item.DateCallString != null && !Utils.CheckDateTime(item.DateCallString))
                    {
                        var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "", ErrorMessage = "Ngày gọi không đúng định dạng dd/MM/yyyy" } };
                        var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                        //write trace log 
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = jsonerrs;
                        Logger.LogWarning(LogModel.ToString());

                        return Json(new { status = false, data = jsonerrs });
                    }
                }
            }
            var errs = validform(model);
            if (errs.Count > 0)
            {
                //var jsonSerialiser = new JavaScriptSerializer();
                var jsonerrs = JsonConvert.SerializeObject(errs, Formatting.Indented);

                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = jsonerrs;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, data = jsonerrs });
            }
            DateTime? dateNull = null;
            //UPDATE OR CREATE INESTOR
            var investorResult = new Investor();
            var dataInvestor = _investorService.GetByPhone(model.PhoneInvestor.Trim());
            if(dataInvestor != null)
            {
                try
                {
                    _investorService.UpdateByPhone(model.PhoneInvestor, model.NameInvestor);
                    investorResult.CodeInvestor = dataInvestor.CodeInvestor;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else
            {
                var resultInvestor = new Investor()
                {
                    CodeInvestor = Guid.NewGuid().ToString(),
                    Name = model.NameInvestor,
                    PhoneNumber = model.PhoneInvestor,
                    Status = 1
                };
                try
                {
                    investorResult = _investorService.Raw_Insert(resultInvestor);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            //CALL INVESTOR
            if(model.Id <= 0)
            {
                var callObject = new InvestorsCareHistory()
                {
                    CodeStaff = tokenModel.StaffCode,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = dateNull,
                    ProductCode = model.ProductCode,
                    EventCode = model.EventCode,
                    LevelConcernCode = model.LevelConcernCode,
                    StatusCode = model.StatusCode == 1 ? true : false,
                    HistoryCode = Guid.NewGuid().ToString(),
                    InvestorCode = investorResult.CodeInvestor
                };
                try
                {
                    _investorsCareHistory.Raw_Insert(callObject);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else
            {
                var callInvesOld = _investorsCareHistory.Raw_Get(model.Id);
                var callObject = new InvestorsCareHistory()
                {
                    Id = model.Id,
                    CodeStaff = tokenModel.StaffCode,
                    CreatedDate = callInvesOld.CreatedDate,
                    UpdatedDate = DateTime.Now,
                    ProductCode = model.ProductCode,
                    EventCode = model.EventCode,
                    LevelConcernCode = model.LevelConcernCode,
                    StatusCode = model.StatusCode == 1 ? true : false,
                    HistoryCode = callInvesOld.HistoryCode,
                    InvestorCode = investorResult.CodeInvestor
                };
                try
                {
                    _investorsCareHistory.Raw_Update(callObject);
                }
                catch (Exception ex)
                {

                    throw;
                }
                _investorsCareHistoryDetail.DeleteByHistoryCode(callInvesOld.HistoryCode);
                foreach (var item in model.detailCallCareHistories)
                {
                    var dataDetail = new InvestorsCareHistoryDetail()
                    {
                        HistoryCode = callInvesOld.HistoryCode,
                        Note = item.Note,
                        SupportTime = item.SupportTime,
                        DateCall = !string.IsNullOrEmpty(item.DateCallString) ? DateTime.ParseExact(item.DateCallString, SiteConst.Format.DateFormat, null) : DateTime.Now,
                        StatusFollow = item.StatusFollow,
                        CodeInvestor = investorResult.CodeInvestor,
                        CodeStaff = tokenModel.StaffCode
                        
                    };
                    try
                    {
                        _investorsCareHistoryDetail.Raw_Insert(dataDetail);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
            return Json(new { status = true });
        }
        #region
        public IActionResult Edit(int id)
        {
            //trace log
            LogModel.Action = ActionType.ViewInfo;
            LogModel.Data = (new { id = id }).ToDataString();

            if (id <= 0)
            {
                return BadRequest();
            }
            var data = _investorsCareHistory.GetById(id);
            data.DataItem.detailCallCareHistories = _investorsCareHistoryDetail.GetByHistoryCode(data.DataItem.HistoryCode).Result.ToList() ?? new List<DetailCallCareHistory>();

            return PartialView("Edit", data.DataItem);
        }
        #endregion
        #region HungVX-Delete
        public IActionResult Delete(int id)
        {
            //trace log
            LogModel.Action = ActionType.Delete;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _investorsCareHistory.Raw_Get(id);
            if (data == null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.NotFoundData;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, mess = "Cuộc gọi không tồn tại", name = "" });
            }
            else
            {
                //write trace log
                LogModel.Result = ActionResultValue.AccessSuccess;
                //LogModel.Message = "Xóa cuộc gọi thành công";
                Logger.LogInformation(LogModel.ToString());

                return Json(new { status = true, name = data.EventCode });
            }
        }

        public IActionResult ConfirmDelete(int id)
        {
            //trace log
            LogModel.Action = ActionType.Delete;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _investorsCareHistory.Raw_Get(id);
            if (data != null)
            {
                var createResult = _investorsCareHistory.Raw_Delete(id);
                _investorsCareHistoryDetail.DeleteByHistoryCode(data.HistoryCode);
                if (createResult)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.DeleteSuccess;
                    LogModel.Message = "Xóa nhân sự thành công";
                    Logger.LogInformation(LogModel.ToString());

                    return Json(new { status = true, Message = "Xóa nhân sự thành công!" });
                }
                else
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.DeleteFailed;
                    LogModel.Message = "Xóa cuộc gọi không thành công";
                    Logger.LogError(LogModel.ToString());

                    return Json(new { status = false, Message = "Xóa cuộc gọi không thành công!" });
                }
            }
            //write trace log
            LogModel.Result = ActionResultValue.NotFoundData;
            LogModel.Message = "Không tìm thấy thông tin cuộc gọi trên hệ thống";
            Logger.LogWarning(LogModel.ToString());

            return Json(new { status = false, Message = "Không tìm thấy thông tin cuộc gọi trên hệ thống!" });
        }
        #endregion
        #region HungVX - Get All Product
        public IActionResult GetProduct()
        {
            //trace log
            LogModel.ItemName = "products";
            LogModel.Action = ActionType.GetInfo;
            var data = _productService.GetProducts();
            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());
            return Json(new { Result = 200, Data = data.Result });
        }
        #endregion
        #region HungVX - Get Event(Show up)
        public IActionResult GetEvent(string CodeProduct)
        {
            //trace log
            LogModel.ItemName = "events";
            LogModel.Action = ActionType.GetInfo;
            var data = _eventService.GetEventByProductCode(CodeProduct);
            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());
            return Json(new { Result = 200, Data = data.Result });
        }
        #endregion
        #region HungVX - Get Level
        public IActionResult GetStatusData()
        {
            List<CareHistoryStatusViewModel> data = new List<CareHistoryStatusViewModel>()
            {
                new CareHistoryStatusViewModel(){ Key = 0, Value = "Đã khai thác"},
                new CareHistoryStatusViewModel(){ Key = 1, Value = "Đang khai thác"}
            };
            
            return Json(new { Result = 200, Data = data });
        }
        #endregion
        #region HungVX - Get Level
        public IActionResult GetLevelOfConcern()
        {
            //trace log
            LogModel.ItemName = "events";
            LogModel.Action = ActionType.GetInfo;
            var data = _levelConcernServicel.GetAll();
            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());
            return Json(new { Result = 200, Data = data.Result });
        }
        #endregion
        #region HungVX Validate
        private List<ErrorResult> validform(InvestorsCareHistoryViewModel entity)
        {

            Dictionary<string, ErrorResult> dictErrors = new Dictionary<string, ErrorResult>();
            //  List<ErrorResult> Errors = new List<ErrorResult>();
            if (!TryValidateModel(entity))
            {
                //Error while validating user info. Please review the errors below!";
                foreach (KeyValuePair<string, ModelStateEntry> modelStateDD in ViewData.ModelState)
                {
                    string key = modelStateDD.Key;
                    ModelStateEntry modelState = modelStateDD.Value;

                    foreach (ModelError error in modelState.Errors)
                    {
                        ErrorResult er = new ErrorResult();
                        er.ErrorMessage = error.ErrorMessage;
                        if (key.IndexOf('.') > -1) key = key.Split('.')[1]; //key sẽ có dạng model.property nếu để dạng này sẽ bị ,lỗi phân tích phía client
                        er.Field = key;
                        dictErrors[key] = er;
                    }
                }
            }

            return dictErrors.Values.GroupBy(x => x.ErrorMessage).Select(y => y.First()).ToList();
        }
        #endregion
    }
}
