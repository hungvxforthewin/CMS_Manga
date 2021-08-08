using CRMSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMModel.Models.Data;
using Microsoft.Extensions.Logging;
using CRMSite.Models;
using CRMSite.Common;
using System.Security.Claims;

namespace CRMSite.Areas.Admin.Components
{
    public class SaleManagerViewComponent : ViewComponent
    {
        private readonly IRemunerationSaleManager remunerationSaleManagerService;
        //private readonly ISalaryRealWithRuleKpiSaleManager salaryRealWithRuleKpiSaleManagerService;
        private readonly IKpiSaleManager kpiSaleManagerService;
        private readonly ISalaryWithRoleStaffSaleManager salaryWithRoleStaffSaleManagerService;
        private readonly ILevelSalaryRevenueSaleManager levelSalaryRevenueSaleManager;
        private readonly ILogger<SaleManagerViewComponent> _logger;

        public SaleManagerViewComponent(IKpiSaleManager KpiSaleManagerService, ILogger<SaleManagerViewComponent> logger)
        {
            remunerationSaleManagerService = new RemunerationSaleManagerImp();
            //salaryRealWithRuleKpiSaleManagerService = new SalaryRealWithRuleKpiSaleManagerImp();
            kpiSaleManagerService = KpiSaleManagerService;
            salaryWithRoleStaffSaleManagerService = new SalaryWithRoleStaffSaleManagerImp();
            levelSalaryRevenueSaleManager = new LevelSalaryRevenueSaleManagerImp();
            _logger = logger;
        }
        public IViewComponentResult Invoke()
        {
            SaleManagerViewModel model = new SaleManagerViewModel();
            var kpi = kpiSaleManagerService.GetByRole(6).FirstOrDefault() ?? new Kpi();
            var remuneration = remunerationSaleManagerService.GetByRole(6);
            var salary = salaryWithRoleStaffSaleManagerService.GetByRole(6).ToArray();
            var lstData = new List<LevelSalaryRevenue> ();
            if(kpi != null)
            {
                lstData = levelSalaryRevenueSaleManager.GetLevelSalaryRevenueByKpiCode(kpi.CodeKpi);
            }
            //var stt = 0;
            var data6MFirst = levelSalaryRevenueSaleManager.GetFirstByRoleAndTimeKpi(kpi.CodeKpi, 1);
            var data6MLast = levelSalaryRevenueSaleManager.GetFirstByRoleAndTimeKpi(kpi.CodeKpi, 2);
            if(data6MFirst != null)
            {
                model.PercentKpiRoot6F = (decimal)data6MFirst.PercentKpiMax;
                model.SalaryPercentRoot6F = (decimal)data6MFirst.SalaryPercentLv1;
            }
            if(data6MLast != null)
            {
                model.PercentKpiRoot6L = (decimal)data6MLast.PercentKpiMax;
                model.SalaryPercentRoot6L = (decimal)data6MLast.SalaryPercentLv1;
            }
            
            foreach (var item in lstData)
            {
                if(item.PercentKpiMin == 0 && item.TimeKpi == 1)
                {
                    model.SaleManagerRemunerations.Add(new SaleManagerRemuneration()
                    {
                        Id = item.Id,
                        Percent = (decimal)item.PercentRemuneration,
                        //khanhkk added
                        SharePercent = (decimal)(item.SharePercent ?? 0),
                        //khanhkk added
                        Salary = item.Salary,
                        MinMaxRevenueSM = string.Concat(string.Format("{0:#,0}", item.RevenueMin), "-", string.Format("{0:#,0}", item.RevenueMax))
                    });
                }
                if(item.SalaryPercentLv1 == null)
                {
                    model.SaleManagerRemunerations.Add(new SaleManagerRemuneration()
                    {
                        Id = item.Id,
                        Percent = (decimal)item.PercentRemuneration,
                        //khanhkk added
                        SharePercent = (decimal)(item.SharePercent ?? 0),
                        //khanhkk added
                        Salary = item.Salary,
                        MinMaxRevenueSM = item.RevenueMax != null ? string.Concat(string.Format("{0:#,0}", item.RevenueMin), "-", string.Format("{0:#,0}", item.RevenueMax)) : string.Format("{0:#,0}", item.RevenueMin)
                    });
                }
                if(item.PercentKpiMin != 0 && item.TimeKpi == 1)
                {
                    model.SaleManagerKpi6Firsts.Add(new SaleManagerKpi6First()
                    {
                        PercentKpiMin6F = (decimal)item.PercentKpiMin,
                        PercentKpiMax6F = (decimal)item.PercentKpiMax,
                        SalaryPercentLv16F = (decimal)item.SalaryPercentLv1
                    });
                }
                if (item.PercentKpiMin != 0 && item.TimeKpi == 2)
                {
                    model.SaleManagerKpi6Lasts.Add(new SaleManagerKpi6Last()
                    {
                        PercentKpiMin6L = (decimal)item.PercentKpiMin,
                        PercentKpiMax6L = (decimal)item.PercentKpiMax,
                        SalaryPercentLv16L = (decimal)item.SalaryPercentLv1
                    });
                }
            }
            //foreach (var item in remuneration)
            //{
            //    model.Add(new SaleManagerViewModel()
            //    {
            //        Salary = salary[stt].Salary,
            //        Id = salary[stt].Id,
            //        RemunerationId = item.Id,
            //        AmountMinMaxInMonth = item.AmountMinInMonth.ToString(),
            //        MinMaxRevenueSM = string.Concat(item.MinRevenueSM, "-", item.MaxRevenueSM),
            //        CodeRemuneration = item.CodeRemuneration,
            //        Percent = item.Percent.Value
            //    }); 
            //    stt = stt + 1;
            //}

            // write trace log
            LogModel log = new LogModel
            {
                AccessTarget = HttpContext.Request.Path.Value,
                Action = ActionType.GetInfo,
                ItemName = "SALE ADMIN mechanism",
                Data = model.ToDataString(),
                Result = ActionResultValue.GetInfoSuccess,
                Role = Helper.GetRoleName(byte.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Role).Value)),
                Username = HttpContext.User.Claims.First(x => x.Type == SiteConst.TokenKey.USERNAME).Value,
            };
            _logger.LogInformation(log.ToString());

            return View("SaleManager", model);
        }
    }
}
