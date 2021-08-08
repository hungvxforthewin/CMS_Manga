using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CRMSite.Areas.Sale.Controllers
{
    [Area("Sale")]
    public class TemporaryPaySlipController : BaseController
    {
        private IEarning _iEarning;

        public TemporaryPaySlipController(IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            _iEarning = new EarningImp();
            LogModel.ItemName = "temporary payslip(s)";
        }

        #region Index
        public IActionResult Index()
        {
            //write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            //var lst = _iEarning.GetSalaries("2021/04");
            //return View(lst.Result);
            return View();
        }
        #endregion

        #region GetList
        public IActionResult GetList()
        {
            // trace log
            LogModel.Action = ActionType.GetInfo;

            var lst = _iEarning.GetTemporarySalary(null, tokenModel.StaffCode);
            var handleResult = HandleGetResult(lst);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = lst.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = lst.Result.ToList(), Total = lst.Result.Count });
        }
        #endregion
    }
}
