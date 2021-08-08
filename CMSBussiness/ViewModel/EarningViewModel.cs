using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class EarningViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string CodeStaff { get; set; }

        public string FullName { get; set; }

        public string BranchName { get; set; }

        public string DepartmentName { get; set; }

        public string TeamName { get; set; }

        public string PositionName { get; set; }

        public decimal? AmountSocialInsuarance { get; set; }

        public decimal? AmountSeniority { get; set; }

        public decimal? AmountAttendance { get; set; }

        public decimal? AmountUnion { get; set; }

        public decimal? AmountPunish { get; set; }

        public decimal? OtherBonus { get; set; }

        public decimal? Salary { get; set; }

        public decimal? TotalReal { get; set; }

        public decimal? Bonus { get; set; }

        public float? TotalShowUpInMonth { get; set; }

        public decimal? RevenueInMonth { get; set; }

        public decimal? IncomeTax { get; set; }

        public float? TotalWorkingDays { get; set; }

        public float? TotalWorkingDaysRule { get; set; }

        public byte? TotalLates { get; set; }

        public byte? TotalEarlyOuts { get; set; }

        public byte? TotalWithoutReason { get; set; }

        public byte? ForgetCheckOutIn { get; set; }

        public byte? TotalTakeLeaveInMonth { get; set; }

        public decimal? RevenueTeam{ get; set; }

        public decimal? RevenueDepartment { get; set; }

        public decimal? RevenueOffice { get; set; }

        public decimal? RevenueBranch { get; set; }

        public decimal? SaleHoldingStock { get; set; }

        public decimal? LeaderSaleHoldingStock { get; set; }

        public decimal? ManageSaleHoldingStock { get; set; }

        public decimal? OfficeManageHoldingStock { get; set; }

        public decimal? SumHoldingStocksInMonth { get; set; }
    }


    public class SearchSalaryModel
    {
        public string Month { get; set; }

        public string Position { get; set; }

        public string Branch { get; set; }

        public string Department { get; set; }

        public string Team { get; set; }

        public bool Status { get; set; }

        public string Key { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }
    }
}
