using CRMSite.Controllers;
using CRMSite.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMModel.Models.Data;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using CRMSite.Models;
using CRMSite.Common;

namespace CRMSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SaleManagerController : BaseController
    {
        //private IHttpContextAccessor _httpContextAccessor;
        //private readonly IRemunerationSaleManager remunerationSaleManagerService;
        //private readonly ISalaryRealWithRuleKpiSaleManager salaryRealWithRuleKpiSaleManagerService;
        //private readonly ISalaryWithRoleStaffSaleManager salaryWithRoleStaffSaleManagerService;
        private readonly IKpiSaleManager kpiSaleManagerService;
        private readonly ILevelSalaryRevenueSaleManager levelSalaryRevenueService;

        public SaleManagerController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            //_httpContextAccessor = httpContextAccessor;
            //remunerationSaleManagerService = new RemunerationSaleManagerImp();
            //salaryRealWithRuleKpiSaleManagerService = new SalaryRealWithRuleKpiSaleManagerImp();
            //salaryWithRoleStaffSaleManagerService = new SalaryWithRoleStaffSaleManagerImp();
            kpiSaleManagerService = new KpiSaleManagerImp();
            levelSalaryRevenueService = new LevelSalaryRevenueSaleManagerImp();

        }
        [HttpPost]
        public IActionResult Setup(SaleManagerViewModel model)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.ItemName = "sale manager mechanism";
            LogModel.Data = model.ToDataString();

            string pattern = @"(^[0-9]{1,}-[0-9]{1,}$)";//|(^[0-9]{1,}$)
            Regex rg = new Regex(pattern);
            long checkFinal = 0;
            decimal checkFinal6F = 0;
            decimal checkFinal6L = 0;
            var stt = 1;
            var flag = 0;
            var Kpi_Result = new Kpi();
            foreach (var item in model.SaleManagerKpi6Firsts)
            {
                if(item.PercentKpiMax6F == 0 || item.PercentKpiMin6F == 0 || item.SalaryPercentLv16F == 0)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Các Kpi không được để trống";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, mess = "Các Kpi không được để trống" });
                }
                if (item.PercentKpiMin6F < model.PercentKpiRoot6F)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Không thể nhỏ hơn Kpi cấu hình 6 tháng đầu";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, mess = "Không thể nhỏ hơn Kpi cấu hình 6 tháng đầu" });
                }
                if (item.PercentKpiMin6F > item.PercentKpiMax6F)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Khoảng chia không hợp lệ";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, mess = "Khoảng chia không hợp lệ" });
                }
                if (!(checkFinal6F == item.PercentKpiMin6F) && checkFinal6F != 0)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Khoảng chia không hợp lệ";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, mess = "Khoảng chia không hợp lệ" });
                }
                checkFinal6F = item.PercentKpiMax6F;
            }
            foreach (var item in model.SaleManagerKpi6Lasts)
            {
                if (item.PercentKpiMax6L == 0 || item.PercentKpiMin6L == 0 || item.SalaryPercentLv16L == 0)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Các Kpi không được để trống";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, mess = "Các Kpi không được để trống" });
                }
                if (item.PercentKpiMin6L < model.PercentKpiRoot6F)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Không thể nhỏ hơn Kpi cấu hình 6 tháng sau";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, mess = "Không thể nhỏ hơn Kpi cấu hình 6 tháng sau" });
                }
                if (item.PercentKpiMin6L > item.PercentKpiMax6L)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Khoảng chia không hợp lệ";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, mess = "Khoảng chia không hợp lệ" });
                }
                if (!(checkFinal6L == item.PercentKpiMin6L) && checkFinal6L != 0)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Khoảng chia không hợp lệ";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, mess = "Khoảng chia không hợp lệ" });
                }
                checkFinal6L = item.PercentKpiMax6L;
            }
            foreach (var item in model.SaleManagerRemunerations)
            {
                if (item.Percent <= 0 || item.Salary <= 0)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Hoa hồng hoặc lương không được trống";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, mess = "Hoa hồng hoặc lương không được trống" });
                }
                //khanhkk added
                if (item.SharePercent <= 0)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Phần trăm tính cổ phần không được trống";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, mess = "Phần trăm tính cổ phần không được trống" });
                }
                //khanhkk added
                if (!rg.IsMatch(item.MinMaxRevenueSM) && flag == 0)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Setup không thành công, không đúng định dạng";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, mess = "Setup không thành công, không đúng định dạng" });
                }
                if (!rg.IsMatch(item.MinMaxRevenueSM) && !(flag == (model.SaleManagerRemunerations.Count -1)) )
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Setup không thành công, không đúng định dạng";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, mess = "Setup không thành công, không đúng định dạng" });
                }
                if (item.MinMaxRevenueSM.Contains('-'))
                {
                    if (long.Parse(item.MinMaxRevenueSM.Split('-')[0]) > long.Parse(item.MinMaxRevenueSM.Split('-')[1]))
                    {
                        //write trace log
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = "Khoảng chia không hợp lệ";
                        Logger.LogWarning(LogModel.ToString());

                        return Json(new { status = false, mess = "Khoảng chia không hợp lệ" });
                    }
                    if (checkFinal > long.Parse(item.MinMaxRevenueSM.Split('-')[0]) && checkFinal != 0)
                    {
                        //write trace log
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = "Khoảng chia không hợp lệ";
                        Logger.LogWarning(LogModel.ToString());

                        return Json(new { status = false, mess = "Khoảng chia không hợp lệ" });
                    }
                    checkFinal = long.Parse(item.MinMaxRevenueSM.Split('-')[1]);
                }
                else if(flag == (model.SaleManagerRemunerations.Count - 1))
                {
                    if (checkFinal > long.Parse(item.MinMaxRevenueSM.Split('-')[0]) && checkFinal != 0)
                    {
                        //write trace log
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = "Khoảng chia không hợp lệ";
                        Logger.LogWarning(LogModel.ToString());

                        return Json(new { status = false, mess = "Khoảng chia không hợp lệ" });
                    }
                }
                flag = flag + 1;
            }
            kpiSaleManagerService.DeleteAllByRole(6);
            levelSalaryRevenueService.DeleteAllByRole(6);
            foreach (var item in model.SaleManagerRemunerations)
            {
                
                if (stt == 1)
                {
                    //INSERT LEVEL 1
                    var dataKpi = new Kpi()
                    {
                        CodeKpi = Guid.NewGuid().ToString(),
                        KpiName = "Sale_Manager",
                        TypeKpi = 1,
                        Status = true,
                        RoleAccount = 6,
                        Revenue = decimal.Parse(item.MinMaxRevenueSM.Split('-')[0])
                    };
                    //6 THÁNG ĐẦU
                    Kpi_Result = kpiSaleManagerService.Raw_Insert(dataKpi);
                    var dataLevelSalaryRemuration6FFirst = new LevelSalaryRevenue()
                    {
                        RoleAccount = 6,
                        PercentRemuneration = (float)item.Percent,
                        Salary = item.Salary,
                        PercentKpiMin = 0,
                        PercentKpiMax = (float)model.PercentKpiRoot6F,
                        Status = true,
                        CodeKpi = Kpi_Result.CodeKpi,
                        RevenueMin = decimal.Parse(item.MinMaxRevenueSM.Split('-')[0]),
                        RevenueMax = decimal.Parse(item.MinMaxRevenueSM.Split('-')[1]),
                        SalaryPercentLv1 = (float)model.SalaryPercentRoot6F,
                        TimeKpi = 1,
                        CreateDate = DateTime.Now,
                        //khanhkk added
                        SharePercent = (float)item.SharePercent,
                        //khanhkk added

                    };
                    levelSalaryRevenueService.Raw_Insert(dataLevelSalaryRemuration6FFirst);
                    foreach (var kpiF in model.SaleManagerKpi6Firsts)
                    {
                        var dataLevelSalaryRemuration6F = new LevelSalaryRevenue()
                        {
                            RoleAccount = 6,
                            PercentRemuneration = (float)item.Percent,
                            Salary = item.Salary,
                            PercentKpiMin = (float)kpiF.PercentKpiMin6F,
                            PercentKpiMax = (float)kpiF.PercentKpiMax6F,
                            Status = true,
                            CodeKpi = Kpi_Result.CodeKpi,
                            RevenueMin = decimal.Parse(item.MinMaxRevenueSM.Split('-')[0]),
                            RevenueMax = decimal.Parse(item.MinMaxRevenueSM.Split('-')[1]),
                            SalaryPercentLv1 = (float)kpiF.SalaryPercentLv16F,
                            TimeKpi = 1,
                            CreateDate = DateTime.Now,
                            //khanhkk added
                            SharePercent = (float)item.SharePercent,
                            //khanhkk added

                        };
                        levelSalaryRevenueService.Raw_Insert(dataLevelSalaryRemuration6F);
                    }
                    //6 THÁNG SAU
                    var dataLevelSalaryRemuration6LFirst = new LevelSalaryRevenue()
                    {
                        RoleAccount = 6,
                        PercentRemuneration = (float)item.Percent,
                        Salary = item.Salary,
                        PercentKpiMin = 0,
                        PercentKpiMax = (float)model.PercentKpiRoot6L,
                        Status = true,
                        CodeKpi = Kpi_Result.CodeKpi,
                        RevenueMin = decimal.Parse(item.MinMaxRevenueSM.Split('-')[0]),
                        RevenueMax = decimal.Parse(item.MinMaxRevenueSM.Split('-')[1]),
                        SalaryPercentLv1 = (float)model.SalaryPercentRoot6L,
                        TimeKpi = 2,
                        CreateDate = DateTime.Now,
                        //khanhkk added
                        SharePercent = (float)item.SharePercent,
                        //khanhkk added
                    };
                    levelSalaryRevenueService.Raw_Insert(dataLevelSalaryRemuration6LFirst);
                    foreach (var kpiL in model.SaleManagerKpi6Lasts)
                    {
                        var dataLevelSalaryRemuration6L = new LevelSalaryRevenue()
                        {
                            RoleAccount = 6,
                            PercentRemuneration = (float)item.Percent,
                            Salary = item.Salary,
                            PercentKpiMin = (float)kpiL.PercentKpiMin6L,
                            PercentKpiMax = (float)kpiL.PercentKpiMax6L,
                            Status = true,
                            CodeKpi = Kpi_Result.CodeKpi,
                            RevenueMin = decimal.Parse(item.MinMaxRevenueSM.Split('-')[0]),
                            RevenueMax = decimal.Parse(item.MinMaxRevenueSM.Split('-')[1]),
                            SalaryPercentLv1 = (float)kpiL.SalaryPercentLv16L,
                            TimeKpi = 2,
                            CreateDate = DateTime.Now,
                            //khanhkk added
                            SharePercent = (float)item.SharePercent,
                            //khanhkk added
                        };
                        levelSalaryRevenueService.Raw_Insert(dataLevelSalaryRemuration6L);
                    }
                }
                //else if(stt == model.SaleManagerRemunerations.Count && stt != 1)
                //{
                //    var dataLevelSalaryRemuration = new LevelSalaryRevenue()
                //    {
                //        RoleAccount = 6,
                //        PercentRemuneration = (float)item.Percent,
                //        Salary = item.Salary,
                //        PercentKpiMin = 100,
                //        PercentKpiMax = 100,
                //        Status = true,
                //        CodeKpi = Kpi_Result.CodeKpi,
                //        RevenueMin = decimal.Parse(item.MinMaxRevenueSM),
                //        RevenueMax = null,
                //        CreateDate = DateTime.Now,
                //        //khanhkk added
                //        SharePercent = (float)item.SharePercent,
                //        //khanhkk added
                //    };
                //    levelSalaryRevenueService.Raw_Insert(dataLevelSalaryRemuration);
                //}
                else
                {
                    var dataLevelSalaryRemuration = new LevelSalaryRevenue()
                    {
                        RoleAccount = 6,
                        PercentRemuneration = (float)item.Percent,
                        Salary = item.Salary,
                        PercentKpiMin = 100,
                        PercentKpiMax = 100,
                        Status = true,
                        CodeKpi = Kpi_Result.CodeKpi,
                        RevenueMin = decimal.Parse(item.MinMaxRevenueSM.Split('-')[0]),
                        RevenueMax = decimal.Parse(item.MinMaxRevenueSM.Split('-')[1]),
                        CreateDate = DateTime.Now,
                        //khanhkk added
                        SharePercent = (float)item.SharePercent,
                        //khanhkk added
                    };
                    levelSalaryRevenueService.Raw_Insert(dataLevelSalaryRemuration);
                }
                stt = stt + 1;
            }

            //write trace log
            LogModel.Result = ActionResultValue.UpdateSuccess;
            LogModel.Message = "Setup cơ chế sale manager thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { status = true, mess = "Setup thành công!" });
        }
    }
}
