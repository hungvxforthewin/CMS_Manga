using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMBussiness.ViewModel;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace CRMSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TypeCallController : BaseController
    {
        private ITypeCall _iType;
        public TypeCallController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
           : base(httpContextAccessor, logger)
        {
            _iType = new TypeCallImp();
            LogModel.ItemName = "call type(s)";
        }

        public IActionResult Index()
        {
            //write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return View();
        }

        #region GetList - khanhkk
        public IActionResult GetList(string key, int page = 1, int size = SiteConst.PageSize)
        {
            // trace log
            LogModel.Data = (new { key = key, page = page, size = size }).ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            var data = _iType.GetList(key, page, size, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Data = data.Result, Total = total });
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            LogModel.Action = ActionType.Create;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Create");
        }

        [HttpPost]
        public IActionResult Create(TypeCallViewModel model)
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

            var createResult = _iType.CreateNewCallType(model);
            if (createResult.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.CreateFailed;
                LogModel.Message = "Thêm loại cuộc gọi không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Thêm loại cuộc gọi không thành công!" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.CreateSuccess;
            LogModel.Message = "Thêm loại cuộc gọi thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Thêm loại cuộc gọi thành công!" });
        }
        #endregion

        #region Update
        public IActionResult Update(int id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();

            var bonus = _iType.GetById(id);
            var handleResult = HandleGetResult(bonus);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Update", bonus.Result.First());
        }

        [HttpPost]
        public IActionResult Update(TypeCallViewModel model)
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

            var createResult = _iType.Update(model);
            if (createResult.Error)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.UpdateFailed;
                LogModel.Message = "Cập nhật loại cuộc gọi không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Cập nhật loại cuộc gọi không thành công!" } });
            }

            //write trace log 
            LogModel.Result = ActionResultValue.UpdateSuccess;
            LogModel.Message = "Cập nhật loại cuộc gọi thành công!";
            Logger.LogError(LogModel.ToString());

            return Json(new { Result = 200, Message = "Cập nhật loại cuộc gọi thành công!" });
        }
        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            //trace log
            LogModel.Data = (new { id = id }).ToDataString();
            LogModel.Action = ActionType.Delete;

            var bonus = _iType.GetById(id);
            var handleResult = HandleGetResult(bonus);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Delete", bonus.Result.First());
        }

        public IActionResult ConfirmDelete(int id)
        {
            //trace log
            LogModel.Data = (new { id = id }).ToDataString();
            LogModel.Action = ActionType.Delete;

            var createResult = _iType.Delete(id);
            if (createResult.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.DeleteFailed;
                LogModel.Message = "Xóa loại cuộc gọi không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Xóa loại cuộc gọi không thành công!" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.DeleteSuccess;
            LogModel.Message = "Xóa loại cuộc gọi thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Xóa loại cuộc gọi thành công!" });
        }
        #endregion
    }
}
