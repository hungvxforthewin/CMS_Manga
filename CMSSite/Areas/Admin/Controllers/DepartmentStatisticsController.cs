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
    public class DepartmentStatisticsController : BaseController
    {
        private IStatistic _iStatistic;
        public DepartmentStatisticsController(IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            _iStatistic = new StatisticImp();
            LogModel.ItemName = "Department Statistical Data";
        }

        #region Index
        public IActionResult Index(string branch, string office)
        {
            //HttpContext.Session.SetString(SiteConst.SessionKey.STATISTICAL_BRANCH, branch);
            ViewBag.Branch = branch;
            ViewBag.Office = office;
            //write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return View();
        }
        #endregion

        #region GetList
        public IActionResult GetList(string month, string branch, string office, string department)
        {
            //trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Data = (new { Month = month, Branch = branch, Office = office
                , Deparment = department }).ToDataString();

            // check selected month
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

            var data = _iStatistic.GetDepartmentStatisticInfo(month, branch, office, department);
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

        #region GetStatisticalDataForTeam
        public IActionResult GetStatisticalDataForTeam(string month, string branch, string office, string department
            , string team)
        {
            //trace log
            LogModel.ItemName = "Team Statistical Data";
            LogModel.Action = ActionType.GetInfo;
            LogModel.Data = (new { Month = month, Branch = branch, Office = office, Department = department
                , Team = team }).ToDataString();

            // check selected month
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

            var data = _iStatistic.GetTeamStatisticInfo(month, branch, office, department, team);
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

        #region GetStatisticalDataForIndividual
        public IActionResult GetStatisticalDataForIndividual(string month, string branch, string office
            , string department , string team, string staff)
        {
            //trace log
            LogModel.ItemName = "Personal Statistical Data";
            LogModel.Action = ActionType.GetInfo;
            LogModel.Data = (new { Month = month, Branch = branch, Office = office, Department = department
                , Team = team, Staff = staff }).ToDataString();

            // check selected month
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

            var data = _iStatistic.GetPersonalStatisticInfo(month, branch, office, department, team, staff);
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