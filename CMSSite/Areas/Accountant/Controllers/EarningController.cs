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

namespace CRMSite.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    [Authorize]
    public class EarningController : BaseController
    {
        private IEarning _iEarning;
        public EarningController(IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            _iEarning = new EarningImp();
            LogModel.ItemName = "payslip(s)";
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
        public IActionResult GetList(string month, int page, int size)
        {
            // trace log
            LogModel.Data = (new { month = month, page = page, size = size }).ToDataString();
            LogModel.Action = ActionType.GetInfo;

            //string[] strs = month.Split(SiteConst.SubstractChar);
            var lst = _iEarning.GetSalaries(month);
            var handleResult = HandleGetResult(lst);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = lst.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = lst.Result.ToList().Skip((page - 1) * size).Take(size), Total = lst.Result.Count });
        }
        #endregion

        //#region GetList - khanhkk
        //[HttpPost]
        //public IActionResult GetList(SearchSalaryModel model)
        //{
        //    // trace log
        //    LogModel.Data = model.ToDataString();
        //    LogModel.Action = ActionType.GetInfo;

        //    int total;
        //    var data = _iEarning.GetSalaries(model, out total);
        //    var handleResult = HandleGetResult(data);
        //    if (handleResult != null) return handleResult;

        //    //if (data.Error)
        //    //{
        //    //    return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
        //    //}

        //    //if (data.Result == null || data.Result.Count == 0)
        //    //{
        //    //    return Json(new { Result = 400, Errors = new List<string> { SiteConst.NotFoundError } });
        //    //}

        //    //write trace log
        //    LogModel.Result = ActionResultValue.GetInfoSuccess;
        //    LogModel.Data = data.Result.ToDataString();
        //    Logger.LogInformation(LogModel.ToString());

        //    return Json(new { Data = data.Result, Total = total });
        //}
        //#endregion
    }
}
