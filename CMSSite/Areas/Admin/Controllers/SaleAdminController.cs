using CRMSite.Controllers;
using CRMSite.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMModel.Models.Data;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using CRMSite.Models;
using CRMSite.Common;

namespace CRMSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SaleAdminController : BaseController
    {
        //private IHttpContextAccessor _httpContextAccessor;
        //private readonly IRemunerationSaleAdmin remunerationSaleAdminService;
        //private readonly ISalaryRealWithRuleKpiSaleAdmin salaryRealWithRuleKpiSaleAdminService;
        //private readonly IKpiSaleAdmin kpiSaleAdminService;
        //private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ISalaryWithRoleStaffSaleAdmin salaryWithRoleStaffSaleAdminService;
        private readonly ILevelSalaryRevenueSaleAdmin levelSalaryRevenueService;
        public readonly IConfiguration _configuration;

        public SaleAdminController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment, ILogger<BaseController> logger)
          : base(httpContextAccessor, logger)
        {
            //_httpContextAccessor = httpContextAccessor;
            //remunerationSaleAdminService = new RemunerationSaleAdminImp();
            //salaryRealWithRuleKpiSaleAdminService = new SalaryRealWithRuleKpiSaleAdminImp();
            //kpiSaleAdminService = new KpiSaleAdminImp();
            //_hostingEnvironment = hostingEnvironment;
            salaryWithRoleStaffSaleAdminService = new SalaryWithRoleStaffSaleAdminImp();
            levelSalaryRevenueService = new LevelSalaryRevenueSaleAdminImp();
            _configuration = configuration;
        }
        public IActionResult GetAdminSetup()
        {
            // write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "sale admin mechanism";
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());


            SaleAdminViewModel model = new SaleAdminViewModel();
            var salaryWithRole = salaryWithRoleStaffSaleAdminService.GetByRole(7);
            return PartialView("~/Areas/Admin/Views/Mechanism/_SaleAdmin.cshtml", model);
        }
        [HttpPost]
        public IActionResult Setup(SaleAdminViewModel model)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.ItemName = "sale admin mechanism";
            LogModel.Data = model.ToDataString();

            var branchCode = tokenModel.BranchCode;
            var role = tokenModel.Role;
            string pattern = @"(^[0-9]{1,}-[0-9]{1,}$)";//|(^[0-9]{1,7}$)
            Regex rg = new Regex(pattern);
            //VALIDATE MODEL
            if (model.Salary <= 0 || model.TimeProbationary <= 0 || model.ProbationarySalary <= 0)
            {
                // write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Setup cơ chế lương không đúng";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, mess = "Setup không thành công" });
            }
            long checkFinal = 0;
            var flag = 0;
            var stt = 1;
            foreach (var item in model.SaleAdminLevelSalaryRevenues)
            {
                if (item.PercentRemuneration <= 0 || string.IsNullOrEmpty(item.MinMaxRevenueBranch))
                {
                    // write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Setup cơ chế hoa hồng không đúng";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, mess = "Setup không thành công" });
                }
                ////khanhkk added
                //if (item.SharePercent <= 0)
                //{
                //    return Json(new { status = false, mess = "Phần trăm tính hoa hồng không được để trống" });
                //}
                ////khanhkk added
                if (!rg.IsMatch(item.MinMaxRevenueBranch) && flag == 0)
                {
                    // write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Setup không thành công, sai định dạng";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, mess = "Setup không thành công, sai định dạng" });
                }
                if (!rg.IsMatch(item.MinMaxRevenueBranch) && !(flag == (model.SaleAdminLevelSalaryRevenues.Count - 1)))
                {
                    // write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Setup không thành công, sai định dạng";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, mess = "Setup không thành công, sai định dạng" });
                }
                if (item.MinMaxRevenueBranch.Contains('-'))
                {
                    if (long.Parse(item.MinMaxRevenueBranch.Split('-')[0]) > long.Parse(item.MinMaxRevenueBranch.Split('-')[1]))
                    {
                        // write trace log
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = "Setup không thành công, chia lại khoảng doanh thu";
                        Logger.LogWarning(LogModel.ToString());

                        return Json(new { status = false, mess = "Setup không thành công, chia lại khoảng doanh thu" });
                    }
                    if (!(checkFinal == long.Parse(item.MinMaxRevenueBranch.Split('-')[0])) && checkFinal != 0)
                    {
                        // write trace log
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = "Setup không thành công, chia lại khoảng doanh thu";
                        Logger.LogWarning(LogModel.ToString());

                        return Json(new { status = false, mess = "Setup không thành công, chia lại khoảng doanh thu" });
                    }
                    checkFinal = long.Parse(item.MinMaxRevenueBranch.Split('-')[1]);
                }
                else if (flag == (model.SaleAdminLevelSalaryRevenues.Count - 1))
                {
                    if (checkFinal > long.Parse(item.MinMaxRevenueBranch.Split('-')[0]) && checkFinal != 0)
                    {
                        // write trace log
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = "Khoảng chia không hợp lệ";
                        Logger.LogWarning(LogModel.ToString());

                        return Json(new { status = false, mess = "Khoảng chia không hợp lệ" });
                    }
                }
                flag = flag + 1;
            }
            levelSalaryRevenueService.DeleteAllByRole(7);
            decimal? nulLVAL = null;
            model.SaleAdminLevelSalaryRevenues.ForEach(item =>
            {
                // KHÔNG LIÊN QUAN ĐẾN BẢNG tblKPI
                //var kpi = new Kpi()
                //{
                //    Id = item.KpiId,
                //    //CodeKpi = Guid.NewGuid().ToString(),
                //    KpiName = "SALE_ADMIN",
                //    BranchCode = branchCode,
                //    RoleAccount = 7,
                //    TypeKpi = 2, //THEO DOANH SỐ
                //    Revenue = item.Revenue, //DOANH SỐ CHI NHÁNH
                //    Status = true //MẶC ĐỊNH TỒN TẠI
                //};
                //var kpi_result = new Kpi();
                //if (item.KpiId > 0)
                //{
                //     kpi.CodeKpi = item.CodeKpi;
                //     kpi_result = kpiSaleAdminService.Raw_Update(kpi);
                //}
                //else
                //{
                //    kpi.CodeKpi = Guid.NewGuid().ToString();
                //    kpi_result = kpiSaleAdminService.Raw_Insert(kpi);
                //}
                var remuneration = new LevelSalaryRevenue()
                {
                    CodeKpi = "Sale_Admin",
                    RoleAccount = 7,
                    Salary = model.Salary,
                    ProbationaryTime = (byte)model.TimeProbationary,
                    ProbationarySalary = model.ProbationarySalary,
                    PercentRemuneration =  item.PercentRemuneration,
                    RevenueMin = (stt != model.SaleAdminLevelSalaryRevenues.Count || stt == 1) ? decimal.Parse(item.MinMaxRevenueBranch.Split('-')[0]) : decimal.Parse(item.MinMaxRevenueBranch),
                    RevenueMax = (stt != model.SaleAdminLevelSalaryRevenues.Count || stt == 1) ? decimal.Parse(item.MinMaxRevenueBranch.Split('-')[1]) : nulLVAL,
                    ////khanhkk added
                    //SharePercent = item.SharePercent,
                    ////khanhkk added
                };
                var remuneration_result = new LevelSalaryRevenue();
                remuneration.CreateDate = DateTime.Now;
                remuneration.Status = true;
                remuneration_result = levelSalaryRevenueService.Raw_Insert(remuneration);
                stt = stt + 1;
            });

            // write trace log
            LogModel.Result = ActionResultValue.UpdateSuccess;
            LogModel.Message = "Setup cơ chế thành công";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { status = true });
        }
        
    }
}
