using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CRMSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BranchStatisticsController : BaseController
    {
        private IStatistic _iStatistic;
        public BranchStatisticsController(IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            _iStatistic = new StatisticImp();
            LogModel.ItemName = "Branch Statistical Data";
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

        #region GetList
        public IActionResult GetList(string month, string branch)
        {
            //trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Data = (new { Month = month, Branch = branch }).ToDataString();

            //check selected month
            if (!string.IsNullOrEmpty(month))
            {
                var monthElements = month.Split(SiteConst.SlashChar);
                int selectedYear = int.Parse(monthElements[0]);
                int selectedMonth = int.Parse(monthElements[1]);
                if (selectedYear > DateTime.Now.Year || (selectedYear == DateTime.Now.Year && selectedMonth > DateTime.Now.Month))
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { Result = 400, Errors = new List<string> { "Chỉ có thể thống kê từ dữ liệu của các tháng trước" } });
                }
            }

            var data = _iStatistic.GetBranchStatisticInfo(month, branch);
            if (data.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            if (data.DataItem == null || data.DataItem.ListData == null || data.DataItem.ListData.Count == 0)
            {
                //write trace log
                LogModel.Result = ActionResultValue.NotFoundData;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.NotFoundError } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data.DataItem });

        }
        #endregion
    }
}