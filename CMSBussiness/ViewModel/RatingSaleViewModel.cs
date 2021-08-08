using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CRMBussiness.ViewModel
{
    public class RatingSaleViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Sale { get; set; }
        [Required]
        public decimal RevenueSale { get; set; }
        [Required]
        public string RevenueDate { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        /// <summary>
        /// (1: theo ngày(mặc định), 2: theo tháng, 3: theo năm)
        /// </summary>
        public int IsByTime { get; set; }
        public virtual string BranchCode { get; set; }
        public virtual string OfficeCode { get; set; }
        public virtual string DepartmentCode { get; set; }
        public virtual string TeamCode { get; set; }
        public virtual string StaffCode { get; set; }
    }
    public class SearchRattingViewModel
    {
        /// <summary>
        /// NGÀY VÀO DOANH SỐ CỦA SALE
        /// </summary>
        public string Key { get; set; }
        public string DateRevenue { get; set; }

        public string Sale { get; set; }

        public string Branch { get; set; }

        public int Size { get; set; } = 10;

        public int Page { get; set; } = 1;
    }
    public class DisplayRatingSaleTableViewModel
    {
        public virtual int Id { get; set; }

        public virtual string BranchName { get; set; }

        public virtual string OfficeName { get; set; }

        public virtual string DepartmentName { get; set; }

        public virtual string TeamName { get; set; }

        public decimal RevenueSale { get; set; }

        public virtual string CMT { get; set; }

        public virtual string Phone { get; set; }

        public virtual string Sale { get; set; }

        public virtual string SaleName { get; set; }      
        public virtual string DateRevenue { get; set; }      
     
    }
    public class SaleChart
    {
        public string FullName { get; set; }
        public decimal RevenueSale { get; set; }
        public string RevenueDate { get; set; }
        public decimal Percent { get; set; }
    }
    public class SaleTop10
    {
        public string FullName { get; set; }
        public decimal RevenueSale { get; set; }
        public string RevenueDate { get; set; }
        public decimal Percent { get; set; }
        public string Avatar { get; set; }
        public string DepartmentName { get; set; }
    }
}
