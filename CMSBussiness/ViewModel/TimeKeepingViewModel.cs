using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace CRMBussiness.ViewModel
{
    public class TimeKeepingViewModel
    {
        public virtual long Id { get; set; }

        //[Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string CodeKeeping { get; set; }

        /// <summary>
        /// format ‘yyyy/MM’
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(8, ErrorMessage = "Tháng được chọn quá 8 ký tự")]
        public virtual string Month { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(20, ErrorMessage = "Mã nhân viên quá 20 ký tự")]
        public virtual string CodeStaff { get; set; }

        /// <summary>
        /// Mặc định : 0
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập số ngày đi làm thực tế")]
        public virtual float? TotalWorkingDays { get; set; }

        //public virtual byte? TotalDaysInMonth { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập số ngày đi muộn")]
        [Range(0, 100, ErrorMessage = "Số lần đi muộn phải trong khoảng 0 đến 100")]
        public virtual byte? TotalLates { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập số ngày về sớm")]
        [Range(0, 100, ErrorMessage = "Số lần về sớm phải trong khoảng 0 đến 100")]
        public virtual byte? TotalEarlyOuts { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập số ngày nghỉ không lý do")]
        [Range(0, 100, ErrorMessage = "Số ngày nghỉ không lý do phải trong khoảng 0 đến 100")]
        public virtual byte? TotalWithoutReason { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập số lần quên check in/check out")]
        [Range(0, 100, ErrorMessage = "Số lần quên check in, check out phải trong khoảng 0 đến 100")]
        public virtual byte? ForgetCheckOutIn { get; set; }

        public virtual string FullName { get; set; }

        public virtual string Position { get; set; }

        public virtual string PositionName { get; set; }

        //public virtual decimal? IncomeTax { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập số ngày nghỉ phép")]
        [Range(0, 31, ErrorMessage = "Số ngày nghỉ phép phải trong khoảng 0 đến 31")]
        public virtual float? TotalTakeLeaveInMonth { get; set; }

        [Range(0, 1000, ErrorMessage = "Số show up đạt được phải trong khoảng 0 đến 1000")]
        public virtual float? TotalShowupInMonth { get; set; }

        [Range(0, 100000000000000, ErrorMessage = "Doanh số đạt được phải trong khoảng 0 đến 100,000,000,000,000")]
        public virtual decimal? RevenueInMonth { get; set; }

        public virtual byte RoleAccount { get; set; }

        [Range(0, 1000, ErrorMessage = "Số hợp đồng đạt được phải trong khoảng 0 đến 1000")]
        public virtual float? TotalContract { get; set; }
    }

    public class EmployeeViewModel
    {
        public virtual long STT { get; set; }

        public virtual string CodeStaff { get; set; }

        public virtual string FullName { get; set; }

        public virtual byte RoleAccount { get; set; }

        public virtual string BranchCode { get; set; }
    }

    public class DisplayTimeKeepingViewmodel
    {
        public virtual byte? TotalDaysInMonth { get; set; }

        public virtual decimal? Bonus { get; set; }

        public virtual List<TimeKeepingViewModel> TimeKeepings { get; set; }
    }
}
