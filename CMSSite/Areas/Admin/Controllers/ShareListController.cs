using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CRMSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShareListController : BaseController
    {
        private IContractStaff _iAc;

        public ShareListController(IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            _iAc = new ContractStaffImp();
            LogModel.ItemName = "shares";
        }

        public IActionResult Index()
        {
            //write trace log
            LogModel.Action = ActionType.ViewInfo;
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
            var data = _iAc.GetShareList(key, out total, page, size);
            var bonus = HandleGetResult(data);
            if (bonus != null) return bonus;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Data = data.Result, Total = total });
        }
        #endregion
    }
}
