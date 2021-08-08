using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMBussiness.ViewModel;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CRMSite.Areas.Sale.Controllers
{
    [Area("Sale")]
    [Route("[area]/[controller]")]
    public class SaleRevenueStatisticsController : BaseController
    {
        private IStatistic _iStatis;
        private ITarget _iTarget;
        private IOffice _iOffice;
        private IDepartment _iDept;
        private ITeamInCompany _iTeam;
        private IAccount _iAc;
        private string _DateFormat = "dd-MM-yyyy";

        public SaleRevenueStatisticsController(IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IStatistic iStatis, 
            ITarget iTarget, IOffice iOffice, IDepartment iDept, ITeamInCompany iTeam, IAccount iAc) : base(httpContextAccessor, logger)
        {
            _iStatis = iStatis;
            _iTarget = iTarget;
            _iOffice = iOffice;
            _iDept = iDept;
            _iTeam = iTeam;
            _iAc = iAc;

            LogModel.ItemName = "revenue statistics";
        }

        #region Index
        [Route("Index")]
        public IActionResult Index()
        {
            ViewBag.Role = tokenModel.Role;
            ViewBag.BranchCode = tokenModel.BranchCode;
            ViewBag.OfficeCode = tokenModel.Role == 1 ? string.Empty : tokenModel.OfficeCode;
            ViewBag.DepartmentCode = tokenModel.Role == 10 ? string.Empty : tokenModel.DepartmentCode;
            ViewBag.TeamCode = tokenModel.Role == 6 ? string.Empty : tokenModel.TeamCode;
            ViewBag.StaffCode = tokenModel.Role == 5 ? string.Empty : tokenModel.StaffCode;

            //write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return View();
        }
        #endregion

        #region GetStatisticsData-Departments
        [HttpGet]
        [Route("GetStatisticsData/{time}/{timeOption}/{branch}/{office}")]
        public IActionResult GetStatisticsData(string time, int timeOption, string branch, string office)
        {
            //trace log
            LogModel.Data = new { Time = time, TimeOption = timeOption }.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            //valid selected date
            DateTime statisticalDate = DateTime.Now;
            if (!string.IsNullOrEmpty(time))
            {
                bool validDate = DateTime.TryParseExact(time, _DateFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out statisticalDate);
                if (!validDate)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Ngày thống kê không đúng định dạng ngày/tháng/năm";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { Result = 400, Errors = new List<string> { "Ngày thống kê không đúng định dạng ngày/tháng/năm" } });
                }
            }

            if (statisticalDate.Date > DateTime.Now)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Ngày thống kê không được lớn hơn ngày hiện tại";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Ngày thống kê không được lớn hơn ngày hiện tại" } });
            }

            // get revenue in durations
            var getRevenueInDurationsResult =
                _iStatis.GetRevenueStatisticsInDurations(statisticalDate, timeOption, branch, office);
            var handleDurationResult = HandleGetResult(getRevenueInDurationsResult);
            if (handleDurationResult != null) return handleDurationResult;

            // get revenue of one level
            var getRevenueOfALevelResult =
                _iStatis.GetDepartmentsRevenueStatistics(statisticalDate, timeOption, branch, office);
            if (getRevenueOfALevelResult.Error)
            {
                //write trace log
                LogModel.ItemName = "revenue of some departments";
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            // get revenue of one level
            var getCurrentTotalRevenueResult =
                _iStatis.GetCurrentRevenueAndProportion(statisticalDate, timeOption, branch, office);
            var handleCurrentTotalRevenueResult = HandleGetResult(getCurrentTotalRevenueResult);
            if (handleCurrentTotalRevenueResult != null) return handleCurrentTotalRevenueResult;

            //get revenue target
            var getTarget = _iTarget.GetRevenueTarget(10, office, statisticalDate, timeOption);
            if (getTarget.Error)
            {
                //write trace log
                LogModel.ItemName = "revenue target";
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            //get product component data
            var getProductComponents =
                _iStatis.GetProductComponents(statisticalDate, timeOption, branch, office);
            if (getProductComponents.Error)
            {
                //write trace log
                LogModel.ItemName = "product components";
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            //get revenue target of departments at the specified time
            var getDepartTargets =
                _iTarget.GetDepartmentRevenueTargetList(statisticalDate, office);
            if (getDepartTargets.Error)
            {
                //write trace log
                LogModel.ItemName = "department revenue target";
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            Logger.LogInformation(LogModel.ToString());

            return Json(new
            {
                Result = 200,
                Data = new RevenueStatisticViewModel
                {
                    AllLevelsRevenueInDuration = getRevenueInDurationsResult.Result.ToList(),
                    CurrentALevelRevenue = getRevenueOfALevelResult.Result.ToList(),
                    CurrentRevenue = getCurrentTotalRevenueResult.Result.First().Item1,
                    ProportionPercent = getCurrentTotalRevenueResult.Result.First().Item2,
                    FinishedLevel = getTarget.Result.First() == 0 ? 0 :
                    (float)Math.Round(getCurrentTotalRevenueResult.Result.First().Item1 / getTarget.Result.First() * 100, 2),
                    ProductComponents = getProductComponents.Result.ToList(),
                    Targets = getDepartTargets.Result.ToList()
                }
            });
        }
        #endregion

        #region GetStatisticsData-Teams
        [HttpGet]
        [Route("GetStatisticsData/{time}/{timeOption}/{branch}/{office}/{department}")]
        public IActionResult GetStatisticsData(string time, int timeOption, string branch, string office
            , string department)
        {
            //trace log
            LogModel.Data = new { Time = time, TimeOption = timeOption }.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            //valid selected date
            DateTime statisticalDate = DateTime.Now;
            if (!string.IsNullOrEmpty(time))
            {
                bool validDate = DateTime.TryParseExact(time, _DateFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out statisticalDate);
                if (!validDate)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Ngày thống kê không đúng định dạng ngày/tháng/năm";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { Result = 400, Errors = new List<string> { "Ngày thống kê không đúng định dạng ngày/tháng/năm" } });
                }
            }

            if (statisticalDate.Date > DateTime.Now)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Ngày thống kê không được lớn hơn ngày hiện tại";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Ngày thống kê không được lớn hơn ngày hiện tại" } });
            }

            // get revenue in durations
            var getRevenueInDurationsResult =
                _iStatis.GetRevenueStatisticsInDurations(statisticalDate, timeOption, branch, office, department);
            var handleDurationResult = HandleGetResult(getRevenueInDurationsResult);
            if (handleDurationResult != null) return handleDurationResult;

            // get revenue of one level
            var getRevenueOfALevelResult =
                _iStatis.GetTeamsRevenueStatistics(statisticalDate, timeOption, branch, office, department);
            if (getRevenueOfALevelResult.Error)
            {
                //write trace log
                LogModel.ItemName = "revenue of some teams";
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            // get revenue of one level
            var getCurrentTotalRevenueResult =
                _iStatis.GetCurrentRevenueAndProportion(statisticalDate, timeOption, branch, office, department);
            var handleCurrentTotalRevenueResult = HandleGetResult(getCurrentTotalRevenueResult);
            if (handleCurrentTotalRevenueResult != null) return handleCurrentTotalRevenueResult;

            //get revenue target
            var getTarget = _iTarget.GetRevenueTarget(6, department, statisticalDate, timeOption);
            if (getTarget.Error)
            {
                //write trace log
                LogModel.ItemName = "revenue target";
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            //get product component data
            var getProductComponents =
                _iStatis.GetProductComponents(statisticalDate, timeOption, branch, office, department);
            if (getProductComponents.Error)
            {
                //write trace log
                LogModel.ItemName = "product components";
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            //get revenue target of teams at the specified time
            var getTeamTargets =
                _iTarget.GetTeamRevenueTargetList(statisticalDate, department);
            if (getTeamTargets.Error)
            {
                //write trace log
                LogModel.ItemName = "team revenue target";
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            Logger.LogInformation(LogModel.ToString());

            return Json(new
            {
                Result = 200,
                Data = new RevenueStatisticViewModel
                {
                    AllLevelsRevenueInDuration = getRevenueInDurationsResult.Result.ToList(),
                    CurrentALevelRevenue = getRevenueOfALevelResult.Result.ToList(),
                    CurrentRevenue = getCurrentTotalRevenueResult.Result.First().Item1,
                    ProportionPercent = getCurrentTotalRevenueResult.Result.First().Item2,
                    FinishedLevel = getTarget.Result.First() == 0 ? 0 :
                    (float)Math.Round(getCurrentTotalRevenueResult.Result.First().Item1 / getTarget.Result.First() * 100, 2),
                    ProductComponents = getProductComponents.Result.ToList(),
                    Targets = getTeamTargets.Result.ToList()
                }
            });
        }
        #endregion

        #region GetStatisticsData-Staffs
        [HttpGet]
        [Route("GetStatisticsData/{time}/{timeOption}/{branch}/{office}/{department}/{team}")]
        public IActionResult GetStatisticsData(string time, int timeOption, string branch, string office
            , string department, string team)
        {
            //trace log
            LogModel.Data = new { Time = time, TimeOption = timeOption }.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            //valid selected date
            DateTime statisticalDate = DateTime.Now;
            if (!string.IsNullOrEmpty(time))
            {
                bool validDate = DateTime.TryParseExact(time, _DateFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out statisticalDate);
                if (!validDate)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Ngày thống kê không đúng định dạng ngày/tháng/năm";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { Result = 400, Errors = new List<string> { "Ngày thống kê không đúng định dạng ngày/tháng/năm" } });
                }
            }

            if (statisticalDate.Date > DateTime.Now)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Ngày thống kê không được lớn hơn ngày hiện tại";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Ngày thống kê không được lớn hơn ngày hiện tại" } });
            }

            // get revenue in durations
            var getRevenueInDurationsResult =
                _iStatis.GetRevenueStatisticsInDurations(statisticalDate, timeOption, branch, office, department, team);
            var handleDurationResult = HandleGetResult(getRevenueInDurationsResult);
            if (handleDurationResult != null) return handleDurationResult;

            // get revenue of one level
            var getRevenueOfALevelResult =
                _iStatis.GetPersonalRevenueStatistics(statisticalDate, timeOption, branch, office, department, team);
            if (getRevenueOfALevelResult.Error)
            {
                //write trace log
                LogModel.ItemName = "revenue of some staffs";
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            // get revenue of one level
            var getCurrentTotalRevenueResult =
                _iStatis.GetCurrentRevenueAndProportion(statisticalDate, timeOption, branch, office, department, team);
            var handleCurrentTotalRevenueResult = HandleGetResult(getCurrentTotalRevenueResult);
            if (handleCurrentTotalRevenueResult != null) return handleCurrentTotalRevenueResult;

            //get revenue target
            var getTarget = _iTarget.GetRevenueTarget(5, team, statisticalDate, timeOption);
            if (getTarget.Error)
            {
                //write trace log
                LogModel.ItemName = "revenue target";
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            //get product component data
            var getProductComponents =
                _iStatis.GetProductComponents(statisticalDate, timeOption, branch, office, department, team);
            if (getProductComponents.Error)
            {
                //write trace log
                LogModel.ItemName = "product components";
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            //get revenue target of sales at the specified time
            var getSaleTargets =
                _iTarget.GetSaleRevenueTargetList(statisticalDate, team);
            if (getSaleTargets.Error)
            {
                //write trace log
                LogModel.ItemName = "sale revenue target";
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            Logger.LogInformation(LogModel.ToString());

            return Json(new
            {
                Result = 200,
                Data = new RevenueStatisticViewModel
                {
                    AllLevelsRevenueInDuration = getRevenueInDurationsResult.Result.ToList(),
                    CurrentALevelRevenue = getRevenueOfALevelResult.Result.ToList(),
                    CurrentRevenue = getCurrentTotalRevenueResult.Result.First().Item1,
                    ProportionPercent = getCurrentTotalRevenueResult.Result.First().Item2,
                    FinishedLevel = getTarget.Result.First() == 0 ? 0 :
                    (float)Math.Round(getCurrentTotalRevenueResult.Result.First().Item1 / getTarget.Result.First() * 100, 2),
                    ProductComponents = getProductComponents.Result.ToList(),
                    Targets = getSaleTargets.Result.ToList()
                }
            });
        }
        #endregion

        #region GetStatisticsData-A Staff
        [HttpGet]
        [Route("GetStatisticsData/{time}/{timeOption}/{branch}/{office}/{department}/{team}/{staff}")]
        public IActionResult GetStatisticsData(string time, int timeOption, string branch, string office
            , string department, string team, string staff)
        {
            //trace log
            LogModel.Data = new { Time = time, TimeOption = timeOption }.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            //valid selected date
            DateTime statisticalDate = DateTime.Now;
            if (!string.IsNullOrEmpty(time))
            {
                bool validDate = DateTime.TryParseExact(time, _DateFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out statisticalDate);
                if (!validDate)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Ngày thống kê không đúng định dạng ngày/tháng/năm";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { Result = 400, Errors = new List<string> { "Ngày thống kê không đúng định dạng ngày/tháng/năm" } });
                }
            }

            if (statisticalDate.Date > DateTime.Now)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Ngày thống kê không được lớn hơn ngày hiện tại";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Ngày thống kê không được lớn hơn ngày hiện tại" } });
            }

            // get revenue in durations
            var getRevenueInDurationsResult =
                _iStatis.GetRevenueStatisticsInDurations(statisticalDate, timeOption, branch, office, department,
                    team, staff);
            var handleDurationResult = HandleGetResult(getRevenueInDurationsResult);
            if (handleDurationResult != null) return handleDurationResult;

            // get revenue of one level
            var getRevenueOfALevelResult =
                _iStatis.GetPersonalRevenueStatistics(statisticalDate, timeOption, branch, office, department, team);
            var handleALevelResult = HandleGetResult(getRevenueOfALevelResult);
            if (handleALevelResult != null) return handleALevelResult;

            // get revenue of one level
            var getCurrentTotalRevenueResult =
                _iStatis.GetCurrentRevenueAndProportion(statisticalDate, timeOption, branch, office, department,
                    team, staff);
            var handleCurrentTotalRevenueResult = HandleGetResult(getCurrentTotalRevenueResult);
            if (handleCurrentTotalRevenueResult != null) return handleCurrentTotalRevenueResult;

            //get revenue target
            var getTarget = _iTarget.GetRevenueTarget(4, staff, statisticalDate, timeOption);
            if (getTarget.Error)
            {
                //write trace log
                LogModel.ItemName = "revenue target";
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            //get product component data
            var getProductComponents =
                _iStatis.GetProductComponents(statisticalDate, timeOption, branch, office, department, team, staff);
            if (getProductComponents.Error)
            {
                //write trace log
                LogModel.ItemName = "product components";
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            //get revenue target of sales at the specified time
            var getSaleTargets =
                _iTarget.GetSaleRevenueTargetList(statisticalDate, team);
            if (getSaleTargets.Error)
            {
                //write trace log
                LogModel.ItemName = "sale revenue target";
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            Logger.LogInformation(LogModel.ToString());

            return Json(new
            {
                Result = 200,
                Data = new RevenueStatisticViewModel
                {
                    AllLevelsRevenueInDuration = getRevenueInDurationsResult.Result.ToList(),
                    CurrentALevelRevenue = getRevenueOfALevelResult.Result.ToList(),
                    CurrentRevenue = getCurrentTotalRevenueResult.Result.First().Item1,
                    ProportionPercent = getCurrentTotalRevenueResult.Result.First().Item2,
                    FinishedLevel = getTarget.Result.First() == 0 ? 0 :
                    (float)Math.Round(getCurrentTotalRevenueResult.Result.First().Item1 / getTarget.Result.First() * 100, 2),
                    ProductComponents = getProductComponents.Result.ToList(),
                    Targets = getSaleTargets.Result.ToList()
                }
            });
        }
        #endregion

        #region SetRevenueTarget
        [HttpPost]
        [Route("SetRevenueTarget")]
        public IActionResult SetRevenueTarget(SaleTargetViewModel model)
        {
            //trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.Create;
            LogModel.ItemName = "revenue target";

            //check input
            if (model != null && model.Targets != null)
            {
                foreach (var item in model.Targets)
                {
                    if (string.IsNullOrEmpty(item.SetTargetFor))
                    {
                        //write trace log
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = "Hệ thống không tìm thấy đối tượng sét chỉ tiêu";
                        Logger.LogWarning(LogModel.ToString());

                        return Json(new { Result = 400,
                            Errors = new List<string> { "Hệ thống không tìm thấy đối tượng sét chỉ tiêu" } });
                    }

                    //valid target object
                    try
                    {
                        switch (tokenModel.Role)
                        {
                            case 10:
                                LogModel.ItemName = "department";
                                LogModel.Action = ActionType.GetInfo;
                                var getDeptResult = _iDept.GetByCode(item.SetTargetFor);

                                if (getDeptResult == null || string.IsNullOrEmpty(getDeptResult.DataItem?.DepartmentName))
                                {
                                    //write trace log
                                    LogModel.Data = new { DepartmentCode = item.SetTargetFor }.ToDataString();
                                    LogModel.Result = ActionResultValue.NotFoundData;
                                    Logger.LogWarning(LogModel.ToString());

                                    return Json(new { Result = 400, Errors = new List<string> { "Không tìm thấy thông tin phòng ban" } });
                                }
                                break;

                            case 6:
                                LogModel.ItemName = "team";
                                LogModel.Action = ActionType.GetInfo;
                                var getTeamResult = _iTeam.GetByCode(item.SetTargetFor);

                                if (getTeamResult == null || string.IsNullOrEmpty(getTeamResult.DataItem?.Name))
                                {
                                    //write trace log
                                    LogModel.Data = new { TeamCode = item.SetTargetFor }.ToDataString();
                                    LogModel.Result = ActionResultValue.NotFoundData;
                                    Logger.LogWarning(LogModel.ToString());

                                    return Json(new { Result = 400, Errors = new List<string> { "Không tìm thấy thông tin nhóm" } });
                                }
                                break;

                            case 5:
                                LogModel.ItemName = "sale";
                                LogModel.Action = ActionType.GetInfo;
                                var getSaleResult = _iAc.GetByCodeStaff(item.SetTargetFor);

                                if (getSaleResult == null || string.IsNullOrEmpty(getSaleResult?.AccountFullName))
                                {
                                    //write trace log
                                    LogModel.Data = new { StaffCode = item.SetTargetFor }.ToDataString();
                                    LogModel.Result = ActionResultValue.NotFoundData;
                                    Logger.LogWarning(LogModel.ToString());

                                    return Json(new { Result = 400, Errors = new List<string> { "Không tìm thấy thông tin sale" } });
                                }
                                break;
                        }
                        
                    }
                    catch 
                    {
                        //write trace log
                        LogModel.Data = new { DepartmentCode = item.SetTargetFor }.ToDataString();
                        LogModel.Result = ActionResultValue.ThrowException;
                        Logger.LogError(LogModel.ToString());

                        return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
                    }

                    LogModel.Action = ActionType.Create;
                    LogModel.ItemName = "revenue target";
                    //valid revenue target
                    if (item.Revenue <= 0 || item.Revenue >= 10000)
                    {
                        //write trace log
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = "Chỉ tiêu phải lớn hơn 0 và nhỏ hơn 10000";
                        Logger.LogWarning(LogModel.ToString());

                        return Json(new { Result = 400, 
                            Errors = new List<string> { "Chỉ tiêu phải lớn hơn 0 và nhỏ hơn 10000" }});
                    }
                }
            }
            else 
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Hệ thống không nhận được dữ liệu chỉ tiêu doanh số";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, 
                    Errors = new List<string> { "Hệ thống không nhận được dữ liệu chỉ tiêu doanh số" }});
            }

            model.CreatedBy = tokenModel.StaffCode;
            switch(tokenModel.Role)
            {
                case 10:
                    model.Role = 6;
                    break;

                case 6:
                    model.Role = 5;
                    break;

                case 5:
                    model.Role = 4;
                    break;
            }
            var setRetsult = _iTarget.SetRevenueTargets(model);
            if (setRetsult.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.CreateFailed;
                LogModel.Message = "Tạo chỉ tiêu doanh thu không thành công";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, 
                    Errors = new List<string> { "Tạo chỉ tiêu doanh thu không thành công!" }});
            }

            //write trace log
            LogModel.Result = ActionResultValue.CreateSuccess;
            LogModel.Message = "Tạo chỉ tiêu doanh thu thành công";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Tạo chỉ tiêu doanh thu thành công!" });
        }
        #endregion
    }
}
