using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRMSite.ViewModels
{
    public class SaleAdminViewModel
    {
        public SaleAdminViewModel()
        {
            SaleAdminKpiWithBranches = new List<SaleAdminKpiWithBranch>();
            SaleAdminLevelSalaryRevenues = new List<SaleAdminLevelSalaryRevenue>();
        }
        public int IdRoleSalaryStaff { get; set; }
        [Required(ErrorMessage = "Role Acount không được trống")]
        public int RoleAccount { get; set; }
        [Required(ErrorMessage = "Lương cứng không được trống")]
        public decimal Salary { get; set; }
        [Required(ErrorMessage = "Lương cứng tối thiểu không được trống")]
        public decimal SalaryMin { get; set; }
        [Required(ErrorMessage = "Thời gian thử việc không được trống")]
        public int TimeProbationary { get; set; }
        [Required(ErrorMessage = "Lương thử việc không được trống")]
        public decimal ProbationarySalary { get; set; }
        [Required(ErrorMessage = "Trạng thái không được trống")]
        public bool Status { get; set; }
        //KPI
        public List<SaleAdminKpiWithBranch> SaleAdminKpiWithBranches { get; set; }
        public List<SaleAdminLevelSalaryRevenue> SaleAdminLevelSalaryRevenues { get; set; }

    }
    public class SaleAdminLevelSalaryRevenue
    {
        public short Id { get; set; }
        public short RoleAccount { get; set; }
        public decimal Salary { get; set; }
        public byte ProbationaryTime { get; set; }
        public decimal ProbationarySalary { get; set; }
        public float PercentRemuneration { get; set; }
        //khanhkk added
        public float SharePercent { get; set; }
        //khanhkk added
        public decimal RevenueMin { get; set; }
        public decimal RevenueMax { get; set; }
        public string MinMaxRevenueBranch { get; set; }
        [Required(ErrorMessage = "CodeKpi không được để trống")]
        public string CodeKpi { get; set; }
        public string SalaryMin { get; set; }
        public byte TeamSize { get; set; }
    }
    public class SaleAdminKpiWithBranch
    {
        public long KpiId { get; set; }
        [Required(ErrorMessage = "CodeKpi không được để trống")]
        public string CodeKpi { get; set; }
        [Required(ErrorMessage = "KpiName không được để trống")]
        public string KpiName { get; set; }
        [Required(ErrorMessage = "BranchCode không được trống")]
        public string BranchCode { get; set; }
        [Required(ErrorMessage = "TypeKpi không được trống")]
        public int TypeKpi { get; set; }
        [Required(ErrorMessage = "Doanh số không được trống")]
        public decimal Revenue { get; set; }
        [Required(ErrorMessage = "Trạng thái không được trống")]
        public bool Status { get; set; }
        //Remuneration, add
        [Required(ErrorMessage = "% Hoa hồng không được trống")]
        public decimal Percent { get; set; }
        public string MinMaxRevenueBranch { get; set; }
        public string MinRevenueBranch { get; set; }
        public string MaxRevenueBranch { get; set; }
        public string CodeRemuneration { get; set; }
        public byte RemunerationId { get; set; }
    }
}
