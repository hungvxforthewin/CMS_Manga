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
    public class LevelConcernController : BaseController
    {
        //private const string _itemName = "level concern";
        private ILevelConcern _iLevel;
        public LevelConcernController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
           : base(httpContextAccessor, logger)
        {
            _iLevel = new LevelConcernImp();
            LogModel.ItemName = "level concern(s)";
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
            //trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Data = (new { key = key, page = page, size = size }).ToDataString();

            int total;
            var data = _iLevel.GetList(key, page, size, out total);
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
            LogModel.Action = ActionType.Create;
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Create");
        }

        [HttpPost]
        public IActionResult Create(LevelConcernViewModel model)
        {
            //trace log
            LogModel.Action = ActionType.Create;
            LogModel.Data = model.ToDataString();

            if (!ModelState.IsValid)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = Helper.GetErrors(ModelState).ToMessageString();
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = Helper.GetErrors(ModelState) });
            }

            var createResult = _iLevel.CreateNewLevel(model);
            if (createResult.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.CreateFailed;
                LogModel.Message = "Thêm mức quan tâm không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Thêm mức quan tâm không thành công!" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.CreateSuccess;
            LogModel.Message = "Thêm mức quan tâm thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Thêm mức quan tâm thành công!" });
        }
        #endregion

        #region Update
        public IActionResult Update(int id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();

            var bonus = _iLevel.GetById(id);
            var handleResult = HandleGetResult(bonus);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Update", bonus.Result.First());
        }

        [HttpPost]
        public IActionResult Update(LevelConcernViewModel model)
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

            var createResult = _iLevel.Update(model);
            if (createResult.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.UpdateFailed;
                LogModel.Message = "Cập nhật mức độ quan tâm không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Cập nhật mức độ quan tâm không thành công!" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.UpdateSuccess;
            LogModel.Message = "Cập nhật mức độ quan tâm thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Cập nhật mức độ quan tâm thành công!" });
        }
        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            //trace log
            LogModel.Action = ActionType.Delete;
            LogModel.Data = (new { id = id }).ToDataString();

            var bonus = _iLevel.GetById(id);
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
            LogModel.Action = ActionType.Delete;
            LogModel.Data = (new { id = id }).ToDataString();

            var createResult = _iLevel.Delete(id);
            if (createResult.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.DeleteFailed;
                LogModel.Message = "Xóa mức độ quan tâm không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Xóa mức độ quan tâm không thành công!" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.DeleteSuccess;
            LogModel.Message = "Xóa mức độ quan tâm thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Xóa mức độ quan tâm thành công!" });
        }
        #endregion
    }
}
