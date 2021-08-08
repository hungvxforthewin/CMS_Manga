using CRMBussiness;
using CRMBussiness.IService;
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
    public class SeniorityController : BaseController
    {
        private IAllowanceOrDeduct _iAllowance;
        public SeniorityController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
           : base(httpContextAccessor, logger)
        {
            _iAllowance = new AllowanceOrDeductImp();
            LogModel.ItemName = "seniority level(s)";
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
        [HttpPost]
        public IActionResult GetList(SearchAllowanceInfoModel model)
        {
            model.Type = 1;

            //trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            var data = _iAllowance.GetAllowanceOrDeductByTypeHavingPagination(model, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            //wrtie trace log
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
        public IActionResult Create(AllowanceOrDeductViewModel model)
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

            if (model.AllowanceAmount == null || model.AllowanceAmount <= 0)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Số tiền thưởng phải lớn hơn 0";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Số tiền thưởng phải lớn hơn 0" } });
            }

            var createResult = _iAllowance.CreateSenoirityBonus(model);
            if (createResult.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.CreateFailed;
                LogModel.Message = "Thêm mức thưởng thâm niên không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Thêm mức thưởng thâm niên không thành công!" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.CreateSuccess;
            LogModel.Message = "Thêm mức thưởng thâm niên thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Thêm mức thưởng thâm niên thành công!" });
        }
        #endregion

        #region Update
        public IActionResult Update(long id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();

            var getResult = _iAllowance.GetAllowanceOrDeductById(id);
            var bonus = HandleGetResult(getResult);
            if (bonus != null) return bonus;

            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Update", getResult.Result.First());
        }

        [HttpPost]
        public IActionResult Update(AllowanceOrDeductViewModel model)
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

            if (model.AllowanceAmount == null || model.AllowanceAmount <= 0)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Số tiền thưởng phải lớn hơn 0";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Số tiền thưởng phải lớn hơn 0" } });
            }

            var createResult = _iAllowance.UpdateSenoirityBonus(model);
            if (createResult.Error)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.UpdateFailed;
                LogModel.Message = "Cập nhật mức thưởng thâm niên không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Cập nhật mức thưởng thâm niên không thành công!" } });
            }

            //write trace log 
            LogModel.Result = ActionResultValue.UpdateSuccess;
            LogModel.Message = "Cập nhật mức thưởng thâm niên thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Cập nhật mức thưởng thâm niên thành công!" });
        }
        #endregion

        #region Delete
        public IActionResult Delete(long id)
        {
            //trace log
            LogModel.Data = (new { id = id }).ToDataString();
            LogModel.Action = ActionType.Delete;

            var getResult = _iAllowance.GetAllowanceOrDeductById(id);
            var bonus = HandleGetResult(getResult);
            if (bonus != null) return bonus;

            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Delete", getResult.Result.First());
        }

        public IActionResult ConfirmDelete(long id)
        {
            //trace log
            LogModel.Data = (new { id = id }).ToDataString();
            LogModel.Action = ActionType.Delete;

            var createResult = _iAllowance.DeleteAllowanceOrDeduct(id);
            if (createResult.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.DeleteFailed;
                LogModel.Message = "Xóa mức thưởng thâm niên không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Xóa mức thưởng thâm niên không thành công!" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.DeleteSuccess;
            LogModel.Message = "Xóa mức thưởng thâm niên thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Xóa mức thưởng thâm niên thành công!" });
        }
        #endregion
    }
}
