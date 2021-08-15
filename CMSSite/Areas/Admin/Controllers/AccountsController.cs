using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMBussiness.ViewModel;
using CRMModel.Models.Data;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EncodeByMd5;

namespace CMSSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : BaseController
    {
        private readonly IAccount _account;
        private EncodeImp _eCode;
        public AccountsController(IConfiguration configuration, IAccount account, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
           : base(httpContextAccessor, logger)
        {
            _account = account;
            _eCode = new EncodeImp();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetList(SearchAccountViewModel model)
        {
            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            var data = _account.GetList(model, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;
            foreach (var item in data.Result)
            {
                item.NameUserCreate = tokenModel.Username;
            }
            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Data = data.Result, Total = total });
        }
        public IActionResult Edit(int id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();
            if (id <= 0)
            {
                return BadRequest();
            }
            var data = _account.Raw_Get(id);
            var result = new AccountViewModel()
            {
                AccountID = data.AccountID,
                AccountName = data.AccountName,
                AccountFullName = data.AccountFullName,
                isEnable = data.isEnable
            };
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;
            return PartialView("Edit", result);
        }
        public IActionResult View(int id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();
            if (id <= 0)
            {
                return BadRequest();
            }
            var data = _account.Raw_Get(id);
            var result = new AccountViewModel()
            {
                AccountID = data.AccountID,
                AccountName = data.AccountName,
                AccountFullName = data.AccountFullName,
                isEnable = data.isEnable
            };
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;
            return PartialView("View", result);
        }
        public IActionResult IsDelete(int id)
        {
            //trace log
            LogModel.Action = ActionType.Delete;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _account.Raw_Get(id);
            if (data == null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.NotFoundData;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, mess = "Nhân sự không tồn tại", name = "" });
            }
            else
            {
                //write trace log
                LogModel.Result = ActionResultValue.DeleteSuccess;
                LogModel.Message = "Xóa doanh số thành công";
                Logger.LogInformation(LogModel.ToString());

                return Json(new { status = true, name = data.AccountFullName });
            }
        }
        public IActionResult Delete(int id)
        {
            //trace log
            LogModel.Action = ActionType.Delete;
            LogModel.Data = (new { id = id }).ToDataString();
            if (id <= 0)
            {
                return BadRequest();
            }
            var data = _account.Raw_Delete(id);
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;
            return Ok(new { status = true, mess = "Xóa nhân sự thành công" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertOrUpdate(AccountViewModel model)
        {            
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

            if (model.AccountID <= 0)
            {
                var status = 0;
                try
                {
                    model.AccountPassword = _eCode.EncodeMD5(model.AccountPassword);
                    _account.InsertAccount(model, out status);
                    if(status != 1)
                    {
                        return Json(new { status = false, mess = "Lỗi hệ thống !" });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, mess = "Lỗi hệ thống !" });
                }

            }
            else
            {
                var oldData = _account.Raw_Get(model.AccountID);
                var data = new Account()
                {
                    AccountID = model.AccountID,
                    AccountName = model.AccountName,
                    AccountFullName = model.AccountFullName,
                    isEnable = model.isEnable,
                    CreateDate = oldData.CreateDate,
                    AccountPassword = string.IsNullOrEmpty(model.AccountPassword) ? oldData.AccountPassword : _eCode.EncodeMD5(model.AccountPassword)
                };
                try
                {
                    _account.Raw_Update(data);
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, mess = "Lỗi hệ thống !" });
                }
            }
            return Json(new { status = true });
        }
        #region HungVX Validate
        private List<ErrorResult> validform(AccountViewModel entity)
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
