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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CRMSite.Areas.Admin.Controllers
{
    //[Area("Admin")]
    public class MechanismController : BaseController
    {
        private Regex Regex = new Regex(SiteConst.RangeTemplate);
        private Regex NumberRegex = new Regex(SiteConst.NumberTemplate);
        public ISetupSalaryElements2 _iSetup;

        public MechanismController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
           : base(httpContextAccessor, logger)
        {
            _iSetup = new SetupSalaryElementsImp2();
        }

        public IActionResult Index()
        {
            //write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "mechanisms";
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return View();
        }

        #region TeleSetup - khanhkk
        [HttpPost]
        public IActionResult TeleSetup(MechanismForTele model)
        {
            //Trace log
            LogModel.Action = ActionType.Create;
            LogModel.ItemName = "TELESALE mechanism";
            LogModel.Data = model.ToDataString();

            if (model.TeleSaleMachanism.Remunerations == null || model.TeleSaleMachanism.Remunerations.Count == 0)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Chưa thiết lập cơ chế hoa hồng cho tele sale!";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Chưa thiết lập cơ chế hoa hồng cho tele sale!" } });
            }

            if (model.LeaderTeleSaleMachanism.Remunerations == null || model.LeaderTeleSaleMachanism.Remunerations.Count == 0)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Chưa thiết lập cơ chế hoa hồng cho leader tele sale!";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Chưa thiết lập cơ chế hoa hồng cho leader tele sale!" } });
            }

            //check common info
            model.TeleSaleMachanism.Common.RoleAccount = 8;
            var commonTele = CommonCheckInput(model.TeleSaleMachanism.Common);
            if (commonTele != null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = commonTele.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return commonTele;
            }

            model.LeaderTeleSaleMachanism.Common.RoleAccount = 9;
            var commonLeaderTele = CommonCheckInput(model.LeaderTeleSaleMachanism.Common);
            if (commonLeaderTele != null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = commonLeaderTele.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return commonLeaderTele;
            }

            //check kpi salary info
            if (model.TeleSaleMachanism.ProbationaryCondition != null)
            {
                var kpiSalaryTele = KpiSalaryCheckInput(model.TeleSaleMachanism.ProbationaryCondition);
                if (kpiSalaryTele != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = kpiSalaryTele.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return kpiSalaryTele;
                }
            }

            if (model.LeaderTeleSaleMachanism.ProbationaryCondition != null)
            {
                var kpiSalaryLeaderTele = KpiSalaryCheckInput(model.LeaderTeleSaleMachanism.ProbationaryCondition);
                if (kpiSalaryLeaderTele != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = kpiSalaryLeaderTele.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return kpiSalaryLeaderTele;
                }
            }

            //check remuneration info
            if (model.TeleSaleMachanism.Remunerations != null)
            {
                foreach (var item in model.TeleSaleMachanism.Remunerations)
                {
                    var remunerationTele = TeleRemunerationCheckInput(item);
                    if (remunerationTele != null)
                    {
                        //write trace log
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = remunerationTele.ToDataString();
                        Logger.LogWarning(LogModel.ToString());

                        return remunerationTele;
                    }
                }

                // check remuneration range
                var remunerationRangeCheck = CheckTeleRemunerationRangeInput(model.TeleSaleMachanism.Remunerations);
                if (remunerationRangeCheck != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = remunerationRangeCheck.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return remunerationRangeCheck;
                }
            }

            if (model.LeaderTeleSaleMachanism.Remunerations != null)
            {
                foreach (var item in model.LeaderTeleSaleMachanism.Remunerations)
                {
                    var remunerationLeaderTele = TeleRemunerationCheckInput(item);
                    if (remunerationLeaderTele != null)
                    {
                        //write trace log
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = remunerationLeaderTele.ToDataString();
                        Logger.LogWarning(LogModel.ToString());

                        return remunerationLeaderTele;
                    }
                }

                var remunerationRangeLeaderCheck = CheckTeleRemunerationRangeInput(model.LeaderTeleSaleMachanism.Remunerations);
                if (remunerationRangeLeaderCheck != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = remunerationRangeLeaderCheck.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return remunerationRangeLeaderCheck;
                }
            }

            var createResult = _iSetup.SetupSalaryElementsForTele(model);
            if (createResult.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.UpdateFailed;
                LogModel.Message = "Thiết lập cơ chế cho telesale không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Thiết lập cơ chế cho telesale không thành công!" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.UpdateSuccess;
            LogModel.Message = "Thiết lập cơ chế cho telesale thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Thiết lập cơ chế cho telesale thành công!" });
        }
        #endregion

        #region SaleSetup - khanhkk
        [HttpPost]
        public IActionResult SaleSetup(MechanismForSale model)
        {
            //Trace log
            LogModel.Action = ActionType.Create;
            LogModel.ItemName = "SALE mechanism";
            LogModel.Data = model.ToDataString();

            if (model.SaleMechanism.Remunerations == null || model.SaleMechanism.Remunerations.Count == 0)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Chưa thiết lập cơ chế hoa hồng và lương cứng cho sale!";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Chưa thiết lập cơ chế hoa hồng và lương cứng cho sale!" } });
            }

            if (model.LeaderSaleMechanism.Remunerations == null || model.LeaderSaleMechanism.Remunerations.Count == 0)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Chưa thiết lập cơ chế hoa hồng và lương cứng cho leader sale!";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Chưa thiết lập cơ chế hoa hồng và lương cứng cho leader sale!" } });
            }

            //check kpi salary info
            // first 6 months
            if (model.SaleMechanism.FirstMonthsCondition != null)
            {
                var kpiSalarySale = KpiSalaryCheckInput(model.SaleMechanism.FirstMonthsCondition);
                if (kpiSalarySale != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = kpiSalarySale.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return kpiSalarySale;
                }
            }

            if (model.LeaderSaleMechanism.FirstMonthsCondition != null)
            {
                var kpiSalaryLeaderSale = KpiSalaryCheckInput(model.LeaderSaleMechanism.FirstMonthsCondition);
                if (kpiSalaryLeaderSale != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = kpiSalaryLeaderSale.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return kpiSalaryLeaderSale;
                }
            }

            // later 6 months
            if (model.SaleMechanism.LaterMonthsCondition != null)
            {
                var kpiSalarySale = KpiSalaryCheckInput(model.SaleMechanism.LaterMonthsCondition);
                if (kpiSalarySale != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = kpiSalarySale.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return kpiSalarySale;
                }
            }

            if (model.LeaderSaleMechanism.LaterMonthsCondition != null)
            {
                var kpiSalaryLeaderSale = KpiSalaryCheckInput(model.LeaderSaleMechanism.LaterMonthsCondition);
                if (kpiSalaryLeaderSale != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = kpiSalaryLeaderSale.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return kpiSalaryLeaderSale;
                }
            }

            //
            if (model.SaleMechanism.FirstMonthsSalary != null)
            {
                foreach (var item in model.SaleMechanism.FirstMonthsSalary)
                {
                    var checkResult = CheckKpiSalaryInput(item, model.SaleMechanism.FirstMonthsCondition);
                    if (checkResult != null)
                    {
                        //write trace log
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = checkResult.ToDataString();
                        Logger.LogWarning(LogModel.ToString());

                        return checkResult;
                    }
                }
                // check remuneration range
                var checkRangeResult = CheckKPIRangeInput(model.SaleMechanism.FirstMonthsSalary, model.SaleMechanism.FirstMonthsCondition);
                if (checkRangeResult != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = checkRangeResult.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return checkRangeResult;
                }
            }

            //
            if (model.SaleMechanism.LaterMonthsSalary != null)
            {
                foreach (var item in model.SaleMechanism.LaterMonthsSalary)
                {
                    var checkResult = CheckKpiSalaryInput(item, model.SaleMechanism.LaterMonthsCondition);
                    if (checkResult != null)
                    {
                        //write trace log
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = checkResult.ToDataString();
                        Logger.LogWarning(LogModel.ToString());

                        return checkResult;
                    }
                }
                // check remuneration range
                var checkRangeResult = CheckKPIRangeInput(model.SaleMechanism.LaterMonthsSalary, model.SaleMechanism.LaterMonthsCondition);
                if (checkRangeResult != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = checkRangeResult.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return checkRangeResult;
                }
            }

            //
            if (model.LeaderSaleMechanism.FirstMonthsSalary != null)
            {
                foreach (var item in model.LeaderSaleMechanism.FirstMonthsSalary)
                {
                    var checkResult = CheckKpiSalaryInput(item, model.LeaderSaleMechanism.FirstMonthsCondition);
                    if (checkResult != null)
                    {
                        //write trace log
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = checkResult.ToDataString();
                        Logger.LogWarning(LogModel.ToString());

                        return checkResult;
                    }
                }
                // check remuneration range
                var checkRangeResult = CheckKPIRangeInput(model.LeaderSaleMechanism.FirstMonthsSalary, model.LeaderSaleMechanism.FirstMonthsCondition);
                if (checkRangeResult != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = checkRangeResult.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return checkRangeResult;
                }
            }

            //
            if (model.LeaderSaleMechanism.LaterMonthsSalary != null)
            {
                foreach (var item in model.LeaderSaleMechanism.LaterMonthsSalary)
                {
                    var checkResult = CheckKpiSalaryInput(item, model.LeaderSaleMechanism.LaterMonthsCondition);
                    if (checkResult != null)
                    {
                        //write trace log
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = checkResult.ToDataString();
                        Logger.LogWarning(LogModel.ToString());

                        return checkResult;
                    }
                }
                // check remuneration range
                var checkRangeResult = CheckKPIRangeInput(model.LeaderSaleMechanism.LaterMonthsSalary, model.LeaderSaleMechanism.LaterMonthsCondition);
                if (checkRangeResult != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = checkRangeResult.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return checkRangeResult;
                }
            }

            //check remuneration info
            //if (model.SaleMechanism.Remunerations != null)
            //{
                foreach (var item in model.SaleMechanism.Remunerations)
                {
                    var remunerationSale = SaleRemunerationCheckInput(item);
                if (remunerationSale != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = remunerationSale.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return remunerationSale;
                }
                }
                // check remuneration range
                var remunerationRangeCheck = CheckSaleRemunerationRangeInput(model.SaleMechanism.Remunerations);
            if (remunerationRangeCheck != null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = remunerationRangeCheck.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return remunerationRangeCheck;
            }
            //}

            //if (model.LeaderSaleMechanism.Remunerations != null)
            //{
                foreach (var item in model.LeaderSaleMechanism.Remunerations)
                {
                    var remunerationLeaderSale = SaleRemunerationCheckInput(item);
                if (remunerationLeaderSale != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = remunerationLeaderSale.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return remunerationLeaderSale;
                }
                }

                var remunerationRangeLeaderCheck = CheckSaleRemunerationRangeInput(model.LeaderSaleMechanism.Remunerations);
            if (remunerationRangeLeaderCheck != null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = remunerationRangeLeaderCheck.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return remunerationRangeLeaderCheck;
            }
            //}

            var createResult = _iSetup.SetupSalaryElementsForSale(model);
            if (createResult.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.UpdateFailed;
                LogModel.Message = "Thiết lập cơ chế cho sale không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Thiết lập cơ chế cho sale không thành công!" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.UpdateSuccess;
            LogModel.Message = "Thiết lập cơ chế cho sale thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Thiết lập cơ chế cho sale thành công!" });
        }
        #endregion

        #region MinisterSetup - khanhkk
        [HttpPost]
        public IActionResult MinisterSetup(MechanismForMinister model)
        {
            //Trace log
            LogModel.Action = ActionType.Create;
            LogModel.ItemName = "OFFICE MANAGER mechanism";
            LogModel.Data = model.ToDataString();

            if (model.MinisterMechanism.Remunerations == null || model.MinisterMechanism.Remunerations.Count == 0)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Chưa thiết lập cơ chế hoa hồng và lương cứng cho trưởng khối!";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Chưa thiết lập cơ chế hoa hồng và lương cứng cho trưởng khối!" } });
            }

            //check kpi salary info
            // first 6 months
            if (model.MinisterMechanism.FirstMonthsCondition != null)
            {
                var kpiSalarySale = KpiSalaryCheckInput(model.MinisterMechanism.FirstMonthsCondition);
                if (kpiSalarySale != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = kpiSalarySale.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return kpiSalarySale;
                }
            }

            // later 6 months
            if (model.MinisterMechanism.LaterMonthsCondition != null)
            {
                var kpiSalarySale = KpiSalaryCheckInput(model.MinisterMechanism.LaterMonthsCondition);
                if (kpiSalarySale != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = kpiSalarySale.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return kpiSalarySale;
                }
            }

            //
            if (model.MinisterMechanism.FirstMonthsSalary != null)
            {
                foreach (var item in model.MinisterMechanism.FirstMonthsSalary)
                {
                    var checkResult = CheckKpiSalaryInput(item, model.MinisterMechanism.FirstMonthsCondition);
                    if (checkResult != null)
                    {
                        //write trace log
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = checkResult.ToDataString();
                        Logger.LogWarning(LogModel.ToString());

                        return checkResult;
                    }
                }
                // check remuneration range
                var checkRangeResult = CheckKPIRangeInput(model.MinisterMechanism.FirstMonthsSalary, model.MinisterMechanism.FirstMonthsCondition);
                if (checkRangeResult != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = checkRangeResult.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return checkRangeResult;
                }
            }

            //
            if (model.MinisterMechanism.LaterMonthsSalary != null)
            {
                foreach (var item in model.MinisterMechanism.LaterMonthsSalary)
                {
                    var checkResult = CheckKpiSalaryInput(item, model.MinisterMechanism.LaterMonthsCondition);
                    if (checkResult != null)
                    {
                        //write trace log
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = checkResult.ToDataString();
                        Logger.LogWarning(LogModel.ToString());

                        return checkResult;
                    }
                }
                // check remuneration range
                var checkRangeResult = CheckKPIRangeInput(model.MinisterMechanism.LaterMonthsSalary, model.MinisterMechanism.LaterMonthsCondition);
                if (checkRangeResult != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = checkRangeResult.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return checkRangeResult;
                }
            }

            //check remuneration info
            //if (model.MinisterMechanism.Remunerations != null)
            //{
            foreach (var item in model.MinisterMechanism.Remunerations)
            {
                var remunerationSale = SaleRemunerationCheckInput(item);
                if (remunerationSale != null)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = remunerationSale.ToDataString();
                    Logger.LogWarning(LogModel.ToString());

                    return remunerationSale;
                }
            }
            // check remuneration range
            var remunerationRangeCheck = CheckSaleRemunerationRangeInput(model.MinisterMechanism.Remunerations);
            if (remunerationRangeCheck != null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = remunerationRangeCheck.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return remunerationRangeCheck;
            }
            //}

            var createResult = _iSetup.SetupSalaryElementsForMinister(model);
            if (createResult.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.UpdateFailed;
                LogModel.Message = "Thiết lập cơ chế cho trưởng khối không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Thiết lập cơ chế cho trưởng khối không thành công!" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.UpdateSuccess;
            LogModel.Message = "Thiết lập cơ chế cho trưởng khối thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Thiết lập cơ chế cho trưởng khối thành công!" });
        }
        #endregion

        #region GetTeleSaleMechanism - khanhkk
        public IActionResult GetTeleSaleMechanism()
        {
            //Trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "TELESALE mechanism";

            var teleMechanismInfo = _iSetup.GetSetupSalaryForTele();
            var handleResult = HandleGetResult(teleMechanismInfo);
            if (handleResult != null) return PartialView("_TeleSale_View", new MechanismForTele());

            //if (teleMechanismInfo.Error)
            //{
            //    //write trace log
            //    Logger.LogError(LogModel.ToString());

            //    return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            //}

            //if (teleMechanismInfo.Result == null || teleMechanismInfo.Result.Count == 0)
            //{
            //    //write trace log
            //    LogModel.Result = ActionResultValue.NotFoundData;
            //    Logger.LogInformation(LogModel.ToString());

            //    return PartialView("_TeleSale_View", new MechanismForTele());
            //}

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = teleMechanismInfo.Result.First().ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_TeleSale_View", teleMechanismInfo.Result.First());
        }
        #endregion

        #region GetSaleMechanism - khanhkk
        public IActionResult GetSaleMechanism()
        {
            //Trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "SALE mechanism";

            var saleMechanismInfo = _iSetup.GetSetupSalaryForSale();
            var handleResult = HandleGetResult(saleMechanismInfo);
            if (handleResult != null) return PartialView("_Sale_View", new MechanismForSale());

            //if (saleMechanismInfo.Error)
            //{
            //    //write trace log
            //    Logger.LogError(LogModel.ToString());

            //    return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            //}

            //if (saleMechanismInfo.Result == null || saleMechanismInfo.Result.Count == 0)
            //{
            //    //write trace log
            //    LogModel.Result = ActionResultValue.NotFoundData;
            //    Logger.LogInformation(LogModel.ToString());

            //    return PartialView("_Sale_View", new MechanismForSale());
            //}

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = saleMechanismInfo.Result.First().ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Sale_View", saleMechanismInfo.Result.First());
        }
        #endregion

        #region GetMinisterMechanism - khanhkk
        public IActionResult GetMinisterMechanism()
        {
            //Trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "OFFICE MANAGER mechanism";

            var saleMechanismInfo = _iSetup.GetSetupSalaryForMinister();
            var handleResult = HandleGetResult(saleMechanismInfo);
            if (handleResult != null) return PartialView("_Minister_View", new MechanismForMinister());

            //if (saleMechanismInfo.Error)
            //{
            //    //write trace log
            //    Logger.LogError(LogModel.ToString());

            //    return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            //}

            //if (saleMechanismInfo.Result == null || saleMechanismInfo.Result.Count == 0)
            //{
            //    //write trace log
            //    LogModel.Result = ActionResultValue.NotFoundData;
            //    Logger.LogInformation(LogModel.ToString());

            //    return PartialView("_Minister_View", new MechanismForMinister());
            //}

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = saleMechanismInfo.Result.First().ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Minister_View", saleMechanismInfo.Result.First());
        }
        #endregion

        #region GetCTVMechanism - khanhkk
        public IActionResult GetCTVMechanism()
        {
            //Trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "COLLABORATOR mechanism";

            var saleMechanismInfo = _iSetup.GetSetupSalaryForSale();
            var handleResult = HandleGetResult(saleMechanismInfo);
            if (handleResult != null) return PartialView("_CTV_View", new MechanismForSale());
            //if (saleMechanismInfo.Error)
            //{
            //    //write trace log
            //    Logger.LogError(LogModel.ToString());

            //    return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            //}

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = saleMechanismInfo.Result.First().ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_CTV_View", saleMechanismInfo.Result?.FirstOrDefault());
        }
        #endregion

        #region CheckInput

        private IActionResult KpiSalaryCheckInput(Level1Condition model)
        {
            if (!model.KpiPercent.HasValue || (model.KpiPercent.HasValue && model.KpiPercent < 0))
            {
                return Json(new { Result = 400, Errors = new List<string> { "Phần trăm kpi đạt được trong thời gian thử việc không thể nhỏ hơn 0!" } });
            }
            model.KpiPercent = (float)Math.Round(model.KpiPercent.Value, 2);

            if (!model.SalaryPercent.HasValue || (model.SalaryPercent.HasValue && model.SalaryPercent < 0))
            {
                return Json(new { Result = 400, Errors = new List<string> { "Phần trăm lương cứng theo kpi không thể nhỏ hơn 0!" } });
            }
            return null;
        }

        private IActionResult TeleRemunerationCheckInput(TeleSaleRemuneration model)
        {
            byte min;
            byte? max;
            var checkRange = CheckPercentRange(model.PercentRange, out min, out max);
            if (checkRange != null) return checkRange;
            model.MinPercent = min;
            model.MaxPercent = max;

            //check remunerations
            if ( model.ShowRemuneration < 0)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Hoa hồng showup không thể nhỏ hơn 0!" } });
            }

            if (model.ContractRemuneration < 0)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Hoa hồng hợp đồng không thể nhỏ hơn 0!" } });
            }

            return null;
        }

        private IActionResult SaleRemunerationCheckInput(SaleRemurationLevel model)
        {
            if (model.PercentRemuneration <= 0)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Hoa hồng phải lớn hơn 0!" } });
            }
            model.PercentRemuneration = (float)Math.Round(model.PercentRemuneration, 2);

            if (model.CalculatingSharePercent <= 0)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Phần trăm tính cổ phần phải lớn hơn 0!" } });
            }
            model.CalculatingSharePercent = (float)Math.Round(model.CalculatingSharePercent, 2);

            if (model.Salary <= 0)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Lương cứng phải lớn hơn 0!" } });
            }

            decimal min;
            decimal? max;
            var checkRange = CheckAmountRange(model.RevenueRange, out min, out max);
            if (checkRange != null) return checkRange;
            model.RevenueMin = min;
            model.RevenueMax = max;
            
            return null;
        }

        private IActionResult CommonCheckInput(TeleSaleCommon model)
        {
            if (model.ProbationaryTime < 0 || model.ProbationaryTime > 2)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Thời gian thử việc không thể nhỏ hơn 0 và tối đa là 2 tháng!" } });
            }

            if (model.ProbationarySalary < 0 || model.Salary <= 0)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Lương không thể nhỏ hơn 0!" } });
            }

            if (model.ProbationarySalary > model.Salary)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Lương thử việc phải nhỏ hơn hoăc bằng lương chính thức!" } });
            }

            if (model.Kpi <= 0 && model.RoleAccount == 8)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Kpi phải lớn hơn 0!" } });
            }    

            return null;
        }

        private IActionResult CheckKpiSalaryInput(KpiSalary model, Level1Condition condition)
        {
            if (model.MinKpiPercent < condition.KpiPercent)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Phần trăm KPI nhỏ nhất trong khoảng không thể nhỏ hơn phần trăm KPI tối thiểu phải đạt được trong level 1!" } });
            }

            if (model.MinKpiPercent >= model.MaxKpiPercent)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Phần trăm KPI lớn nhất phải lớn hơn phần trăm KPI nhỏ nhất trong khoảng!" } });
            }
            model.MinKpiPercent = (float)Math.Round(model.MinKpiPercent, 2);
            model.MaxKpiPercent = (float)Math.Round(model.MaxKpiPercent, 2);

            return null;
        }

        private IActionResult CheckKPIRangeInput(IList<KpiSalary> list, Level1Condition condition)
        {
            var listData = list.OrderBy(x => x.MinKpiPercent).ToList();
            if (listData.First().MinKpiPercent != condition.KpiPercent)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Giá trị kpi nhỏ nhất trong bảng phải bằng giá trị kpi tối thiểu phải đạt được trong level 1!" } });
            }

            for (int i = 0; i < listData.Count - 1; i++)
            {
                if (listData[i + 1].MinKpiPercent != listData[i].MaxKpiPercent)
                {
                    return Json(new { Result = 400, Errors = new List<string> { "Các khoảng phần trăm KPI chia không hợp lý!" } });
                }
            }

            var lastLevel = listData.Last();
            if (lastLevel.SalaryPercent < 100)
            {
                list.Add(new KpiSalary
                {
                    MinKpiPercent = lastLevel.MaxKpiPercent,
                    MaxKpiPercent = lastLevel.MaxKpiPercent,
                    SalaryPercent = 100,
                });
            }

            return null;
        }

        private IActionResult CheckTeleRemunerationRangeInput(IList<TeleSaleRemuneration> list)
        {
            var listData = list.OrderByDescending(x => x.MinPercent).ToList();
            for (int i = 0; i < listData.Count - 1; i++)
            {
                if (listData[i + 1].MinPercent is null)
                {
                    return Json(new { Result = 400, Errors = new List<string> { "Chưa khai báo phần trăm KPI nhỏ nhất trong khoảng!" } });
                }

                if (listData[i + 1].MaxPercent != listData[i].MinPercent)
                {
                    return Json(new { Result = 400, Errors = new List<string> { "Các khoảng phần trăm KPI đạt được chia không hợp lý!" } });
                }    
            }  
            
            return null;
        }

        private IActionResult CheckSaleRemunerationRangeInput(IList<SaleRemurationLevel> list)
        {
            var listData = list.OrderByDescending(x => x.RevenueMin).ToList();
            for (int i = 0; i < listData.Count - 1; i++)
            {
                if (listData[i + 1].RevenueMin is null)
                {
                    return Json(new { Result = 400, Errors = new List<string> { "Chưa khai báo doanh số nhỏ nhất trong khoảng!" } });
                }

                if (listData[i + 1].RevenueMax != listData[i].RevenueMin)
                {
                    return Json(new { Result = 400, Errors = new List<string> { "Các khoảng doanh số đạt được chia không hợp lý!" } });
                }
            }

            return null;
        }

        private IActionResult CheckPercentRange(string range, out byte minValue, out byte? maxValue)
        {
            minValue = 0;
            maxValue = null;

            //check input kpi to cal remuneration
            if (string.IsNullOrEmpty(range) || (!Regex.IsMatch(range) && !NumberRegex.IsMatch(range)))
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Khoảng phần trăm KPI đạt được chưa nhập hoặc không đúng format!" }
                });
            }

            string[] values = range.Trim().Split(SiteConst.SubstractChar);
            if (values.Count() == 2)
            {
                byte min = byte.Parse(values[0]);
                byte max = byte.Parse(values[1]);
                if (min < 0)
                {
                    return Json(new
                    {
                        Result = 400,
                        Errors =
                        new List<string> { "Phần trăm KPI nhỏ nhất không thể nhỏ hơn 0!" }
                    });
                }
                if (min >= max)
                {
                    return Json(new
                    {
                        Result = 400,
                        Errors =
                        new List<string> { "Phần trăm KPI lớn nhất phải lớn hơn phần trăm PKI nhỏ nhất!" }
                    });
                }
                minValue = min;
                maxValue = max;
            }
            else
            {
                byte min = byte.Parse(values[0]);
                if (min < 0)
                {
                    return Json(new
                    {
                        Result = 400,
                        Errors =
                        new List<string> { "Phần trăm KPI nhỏ nhất phải lớn hơn 0!" }
                    });
                }
                minValue = min;
                maxValue = min;
            }
            return null;
        }

        private IActionResult CheckAmountRange(string range, out decimal minValue, out decimal? maxValue)
        {
            minValue = 0;
            maxValue = null;

            //check input kpi to cal remuneration
            if (string.IsNullOrEmpty(range) || (!Regex.IsMatch(range) && !NumberRegex.IsMatch(range)))
            {
                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Khoảng doanh số đạt được chưa nhập hoặc không đúng format!" }
                });
            }

            string[] values = range.Trim().Split(SiteConst.SubstractChar);
            if (values.Count() == 2)
            {
                decimal min = decimal.Parse(values[0]);
                decimal max = decimal.Parse(values[1]);
                if (min < 0)
                {
                    return Json(new
                    {
                        Result = 400,
                        Errors =
                        new List<string> { "Doanh nhỏ nhất số đạt được không thể nhỏ hơn 0!" }
                    });
                }
                if (min >= max)
                {
                    return Json(new
                    {
                        Result = 400,
                        Errors =
                        new List<string> { "Doanh số tối đa phải lớn hơn doanh số nhỏ nhất!" }
                    });
                }
                minValue = min;
                maxValue = max;
            }
            else
            {
                decimal min = decimal.Parse(values[0]);
                if (min < 0)
                {
                    return Json(new
                    {
                        Result = 400,
                        Errors =
                        new List<string> { "Doanh số đạt được thấp nhất phải lớn hơn 0!" }
                    });
                }
                minValue = min;
                maxValue = min;
            }
            return null;
        }
        #endregion
    }
}
