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
    public class WhereToFindInvestorController : BaseController
    {
        private IWhereToFindInvestor _iWhereFind;
        public WhereToFindInvestorController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
           : base(httpContextAccessor, logger)
        {
            _iWhereFind = new WhereToFindInvestorImp();
            LogModel.ItemName = "investor resource(s)";
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
            var data = _iWhereFind.GetList(key ?? string.Empty, page, size, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            // write trace log
            LogModel.Data = data.Result.ToDataString();
            LogModel.Result = ActionResultValue.GetInfoSuccess;
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
        public IActionResult Create(WhereToFindInvestorViewModel model)
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

            var createResult = _iWhereFind.CreateNewResource(model);
            if (createResult.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.CreateFailed;
                LogModel.Message = "Thêm nguồn khách hàng không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Thêm nguồn khách hàng không thành công!" } });
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

            var data = _iWhereFind.GetById(id);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Update", data.Result.First());
        }

        [HttpPost]
        public IActionResult Update(WhereToFindInvestorViewModel model)
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

            var createResult = _iWhereFind.Update(model);
            if (createResult.Error)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.UpdateFailed;
                LogModel.Message = "Cập nhật nguồn khách hàng không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Cập nhật nguồn khách hàng không thành công!" } });
            }

            //write trace log 
            LogModel.Result = ActionResultValue.UpdateSuccess;
            LogModel.Message = "Cập nhật nguồn khách hàng thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Cập nhật nguồn khách hàng thành công!" });
        }
        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            //trace log
            LogModel.Data = (new { id = id }).ToDataString();
            LogModel.Action = ActionType.Delete;

            var data = _iWhereFind.GetById(id);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;


            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Delete", data.Result.First());
        }

        public IActionResult ConfirmDelete(int id)
        {
            //trace log
            LogModel.Data = (new { id = id }).ToDataString();
            LogModel.Action = ActionType.Delete;

            var createResult = _iWhereFind.Delete(id);
            if (createResult.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.DeleteFailed;
                LogModel.Message = "Xóa nguồn khách hàng không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Xóa nguồn khách hàng không thành công!" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.DeleteSuccess;
            LogModel.Message = "Xóa nguồn khách hàng thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Xóa nguồn khách hàng thành công!" });
        }
        #endregion
    }
}
