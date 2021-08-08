using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using System.Data;
using System.IO;
using CRMModel.Models.Data;
using Microsoft.AspNetCore.Hosting;
using static CRMBussiness.ExcelExtension;

namespace CRMSite.Controllers
{
    [Route("Employees")]
    public class EmployeesController : BaseController
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IRemunerationSaleAdmin remunerationSaleAdminService;
        private readonly ISalaryRealWithRuleKpiSaleAdmin salaryRealWithRuleKpiSaleAdminService;
        private readonly IKpiSaleAdmin kpiSaleAdminService;
        private readonly ISalaryWithRoleStaffSaleAdmin salaryWithRoleStaffSaleAdminService;
        private readonly ILevelSalaryRevenueSaleAdmin levelSalaryRevenueService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public readonly IConfiguration _configuration;
        public readonly IAccount _accountService;
        public readonly IContractStaff _contractStaff;

        public EmployeesController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment, IAccount accountService, IContractStaff contractStaff)
          : base(httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            remunerationSaleAdminService = new RemunerationSaleAdminImp();
            salaryRealWithRuleKpiSaleAdminService = new SalaryRealWithRuleKpiSaleAdminImp();
            kpiSaleAdminService = new KpiSaleAdminImp();
            salaryWithRoleStaffSaleAdminService = new SalaryWithRoleStaffSaleAdminImp();
            levelSalaryRevenueService = new LevelSalaryRevenueSaleAdminImp();
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _accountService = accountService;
            _contractStaff = contractStaff;
        }
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("ReadFileImportExcel")]
        [HttpPost]
        public JsonResult ReadFileImportExcel(IFormFile fileImportExcel)
        {
            try
            {
                if (fileImportExcel == null)
                {
                    return Json(new { success = false, message = "Tập tin không tồn tại!" });
                }
                else if (fileImportExcel.FileName.Split('.')[1] != "xlsx" && fileImportExcel.FileName.Split('.')[1] != "xls")
                {
                    return Json(new { success = false, message = "Chọn file excel!" });
                }
                else
                {
                    //save file
                    string pathSave = _configuration["PathSaveFileImport"].ToString();
                    string pathFile = _hostingEnvironment.WebRootPath + pathSave + fileImportExcel.FileName;
                    DirectoryInfo dir = new DirectoryInfo(_hostingEnvironment.WebRootPath + pathSave);
                    foreach (FileInfo file in dir.GetFiles())
                    {
                        file.Delete();
                    }
                    using (FileStream stream = new FileStream(pathFile, FileMode.Create))
                    {
                        fileImportExcel.CopyTo(stream);
                        stream.Close();
                    }
                    //read file to list model
                    List<string> listError = new List<string>();
                    var list = new List<AccountStaffViewModel>();
                    list = ReadtoList<AccountStaffViewModel>(pathFile, ref listError).Select(t => { t.ImportMessage = ""; return t; }).ToList();
                    int stt = 0;
                    List<string> lstMaNV = new List<string>();
                    List<string> lstSDT = new List<string>();
                    List<string> lstEmail = new List<string>();
                    list.ForEach(t =>
                    {
                        stt++;
                        t.rownumber = stt;
                        if (string.IsNullOrEmpty(t.EmployeeID))
                        {
                            t.ImportMessage += "*MaNV không được bỏ trống.";
                        }
                        else if (_accountService.GetByCodeStaff(t.EmployeeID) != null)
                        {
                            t.ImportMessage += "*MaNV này đã tồn tại";
                        }
                        else
                        {
                            if (lstMaNV.Where(n => n.Contains(t.EmployeeID)).Any())
                            {
                                t.ImportMessage += "*MaNV này đã trùng với mã NV trong bảng";
                            }
                            lstMaNV.Add(t.EmployeeID);
                        }
                        //check 
                        if (string.IsNullOrEmpty(t.Email))
                        {
                            t.ImportMessage += "*Email không được bỏ trống";
                        }
                        if (string.IsNullOrEmpty(t.FullName))
                        {
                            t.ImportMessage += "*FullName không được bỏ trống.";
                        }
                        if (string.IsNullOrEmpty(t.Phone))
                        {
                            t.ImportMessage += "*Phone không được trống";
                        }
                        else
                        {
                            if (lstSDT.Where(n => n.Contains(t.Phone)).Any())
                            {
                                t.ImportMessage += "*Phone này đã trùng với phone trong bảng";
                            }
                            lstSDT.Add(t.Phone);
                        }
                        if (string.IsNullOrEmpty(t.CMT))
                        {
                            t.ImportMessage += "*CMT không được trống";
                        }
                    });
                    return Json(new { success = true, lstCus = list, listError });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex });
            }
        }

        [Route("UpdateImportExcel")]
        public JsonResult UpdateImportExcel(List<AccountStaffViewModel> data)
        {
            data = data.Where(x => x.Position.ToUpper().Trim() != "Lễ tân".ToUpper().Trim() && x.Position.ToUpper().Trim() != "Front-end".ToUpper().Trim()).ToList();
            List<AccountStaffViewModel> dataFilter = new List<AccountStaffViewModel>();
            foreach (var item in data)
            {
                if (item.Position.Trim().ToUpper() == "Chuyên viên Nguồn vốn Doanh nghiệp".Trim().ToUpper())
                {
                    item.Role = 4;//SALE
                }
                else if (item.Position.Trim().ToUpper() == "Telesale".Trim().ToUpper())
                {
                    item.Role = 8;//TELESALE
                }
                else if (item.Position.Trim().ToUpper() == "SM (Sale Manager)".Trim().ToUpper())
                {
                    item.Role = 6;//SALE MANAGER
                }
                else if (item.Position.Trim().ToUpper() == "Leader Telesale".Trim().ToUpper())
                {
                    item.Role = 9;//LEADER TELESALE
                }
                else if (item.Position.Trim().ToUpper() == "Admin".Trim().ToUpper())
                {
                    item.Role = 7;//SALE ADMIN
                }
                else if (item.Position.Trim().ToUpper() == "Trưởng phòng Nguồn vốn Doanh nghiệp".Trim().ToUpper())
                {
                    item.Role = 6;//SALE MANAGER
                }

            }
            foreach (var item in data)
            {
                if (item.TeamDepart.Trim().ToUpper() == "Team Telesales 2".Trim().ToUpper())
                {
                    item.TeamCode = "team_telesale_2";
                }
                else if (item.TeamDepart.Trim().ToUpper() == "Team Telesales 1".Trim().ToUpper())
                {
                    item.TeamCode = "team_telesale_1";
                }
                else if (item.TeamDepart.Trim().ToUpper() == "Team Nguồn vốn Doanh nghiệp 1".Trim().ToUpper())
                {
                    item.TeamCode = "team_nvdn_1";

                }
                else if (item.TeamDepart.Trim().ToUpper() == "Team Nguồn vốn Doanh nghiệp 2".Trim().ToUpper())
                {
                    item.TeamCode = "team_nvdn_2";

                }
                else if (item.TeamDepart.Trim().ToUpper() == "Team Nguồn vốn Doanh nghiệp 3".Trim().ToUpper())
                {
                    item.TeamCode = "team_nvdn_3";

                }
                else if (item.TeamDepart.Trim().ToUpper() == "Team Nguồn vốn Doanh nghiệp 4".Trim().ToUpper())
                {
                    item.TeamCode = "team_nvdn_4";

                }
                else if (item.TeamDepart.Trim().ToUpper() == "Khối Nguồn vốn Doanh nghiệp".Trim().ToUpper())
                {
                    item.TeamCode = "";

                }
            }
            foreach (var item in data)
            {
                var check = (DateTime.Now - DateTime.ParseExact(item.StartDate, "dd/MM/yyyy", null)).Days;
                if ((DateTime.Now - DateTime.ParseExact(item.StartDate, "dd/MM/yyyy", null)).Days <= 60)
                {
                    item.Duration = 1;//HỢP ĐỒNG THỬ VIỆC
                    item.LaborContractName = String.Concat("Hợp đồng thử việc - ", item.FullName);
                }
                else
                {
                    item.Duration = 5;//HỢP ĐỒNG VÔ THỜI HẠN
                    item.LaborContractName = String.Concat("Hợp đồng vô thời hạn - ", item.FullName);
                }
            }
            List<Account> lst = new List<Account>();
            List<ContractStaff> lstContract = new List<ContractStaff>();
            DateTime? valueDate = null;
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    var account = new Account()
                    {
                        AccountName = item.Email.Split('\n')[0].Split('@')[0],
                        AccountPassword = "abc123",
                        AccountFullName = item.FullName,
                        isEnable = true,
                        CreateDate = DateTime.Now
                    };
                    _accountService.Raw_Insert(account);
                    var contractStaff = new ContractStaff()
                    {
                        LaborContractCode = Guid.NewGuid().ToString(),
                        LaborContractName = item.LaborContractName,
                        AllowanceCode = "",
                        Duration = (byte)item.Duration,
                        CreateDate = (item.DateOfBirth != null) ? DateTime.ParseExact(item.StartDate, "dd/MM/yyyy", null) : valueDate,
                        StartDate = DateTime.ParseExact(item.StartDate, "dd/MM/yyyy", null),
                        EndDate = valueDate,
                        LaborContractType = "Toan Thoi Gian",
                        IdCard = item.CMT,
                        CodeStaff = item.EmployeeID,
                        NameStaff = item.FullName,
                        Status = 1,
                        Nationality = "Việt Nam",
                        Religion = "Không",
                        IdCardIssuedDate = DateTime.ParseExact(item.DateIssued, "dd/MM/yyyy", null),
                        IdCardIssuedPlace = (item.AddIssued != null) ? item.AddIssued : "Hà Nội",
                        SalaryStartDate = DateTime.ParseExact(item.StartDate, "dd/MM/yyyy", null),
                        DepartmentCode = "nvdn",
                        CurrentAddress = item.CurrentResidence
                    };
                    _contractStaff.Raw_Insert(contractStaff);
                }
            }
            return Json(new { data });
        }
    }
}
