using Microsoft.AspNetCore.Mvc;
using CRMSite.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMSite.Models;
using CRMSite.Common;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;

namespace CRMSite.Areas.Admin.Components
{
    public class SaleAdminViewComponent : ViewComponent
    {
        //private IHttpContextAccessor _httpContextAccessor;
        //private readonly IRemunerationSaleAdmin remunerationSaleAdminService;
        //private readonly ISalaryRealWithRuleKpiSaleAdmin salaryRealWithRuleKpiSaleAdminService;
        //private readonly IKpiSaleAdmin kpiSaleAdminService;
        //private readonly ISalaryWithRoleStaffSaleAdmin salaryWithRoleStaffSaleAdminService;
        private readonly ILevelSalaryRevenueSaleAdmin levelSalaryRevenueService;
        private readonly ILogger<SaleAdminViewComponent> _logger;

        public SaleAdminViewComponent(ILogger<SaleAdminViewComponent> logger)
        {
            //remunerationSaleAdminService = new RemunerationSaleAdminImp();
            //salaryRealWithRuleKpiSaleAdminService = new SalaryRealWithRuleKpiSaleAdminImp();
            //kpiSaleAdminService = new KpiSaleAdminImp();
            //salaryWithRoleStaffSaleAdminService = new SalaryWithRoleStaffSaleAdminImp();
            levelSalaryRevenueService = new LevelSalaryRevenueSaleAdminImp();
            _logger = logger;
        }
        public IViewComponentResult Invoke()
        {
            SaleAdminViewModel model = new SaleAdminViewModel();
            var lstRevenue = levelSalaryRevenueService.LevelSalaryRevenueSaleAdminWithRole(7) ?? new List<LevelSalaryRevenueSaleAdmin>();
            foreach (var item in lstRevenue)
            {
                model.Salary = item.Salary;
                model.ProbationarySalary = item.ProbationarySalary;
                model.TimeProbationary = item.ProbationaryTime;
                model.SaleAdminLevelSalaryRevenues.Add(new SaleAdminLevelSalaryRevenue() { 
                    Salary = item.Salary,
                    ProbationaryTime = item.ProbationaryTime,
                    ProbationarySalary = item.ProbationarySalary,
                    PercentRemuneration = item.PercentRemuneration,
                    ////khanhkk added
                    //SharePercent = item.SharePercent,
                    ////khanhkk added
                    MinMaxRevenueBranch = item.RevenueMax != null ? string.Concat(string.Format("{0:#,0}", decimal.Parse(item.RevenueMin)) , "-", string.Format("{0:#,0}", decimal.Parse(item.RevenueMax))) : string.Format("{0:#,0}", decimal.Parse(item.RevenueMin)),
                    CodeKpi = item.CodeKpi,
                });
            }

            // write trace log
            LogModel log = new LogModel
            {
                AccessTarget = HttpContext.Request.Path.Value,
                Action = ActionType.GetInfo,
                ItemName = "SALE MANAGER mechanism",
                Data = model.ToDataString(),
                Result = ActionResultValue.GetInfoSuccess,
                Role = Helper.GetRoleName(byte.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Role).Value)),
                Username = HttpContext.User.Claims.First(x => x.Type == SiteConst.TokenKey.USERNAME).Value,
            };
            _logger.LogInformation(log.ToString());

            return View("SaleAdmin", model);
        }
    }
}
