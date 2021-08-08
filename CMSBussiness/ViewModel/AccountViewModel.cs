using System;
using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class AccountViewModel
    {
        public virtual long Id { get; set; }

        [StringLength(50)]
        public virtual string UserName { get; set; }

        [StringLength(50)]
        public virtual string Pass { get; set; }

        [StringLength(10)]
        public virtual string CodeStaff { get; set; }

        [StringLength(100)]
        public virtual string FullName { get; set; }

        /// <summary>
        ///Role = 1 – Admin
        ///2: Kế toán
        ///3: Hr
        ///Role = 4 – SaleAdmin
        ///Role = 5 – Sale trong công ty.
        ///Role = 6 – Sale manage
        ///7 – Sale admin
        ///Role = 8 – TeleSale
        ///9 – Leader tele
        /// </summary>
        public virtual byte Role { get; set; }

        [StringLength(50)]
        public virtual string CompanyCode { get; set; }

        public virtual DateTime? Birthday { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string Email { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "ACCOUNT_REQUIRED_PHONE")]
        [StringLength(20)]
        public virtual string Phone { get; set; }

        [StringLength(50)]
        public virtual string PositionCode { get; set; }

        [StringLength(50)]
        public virtual string DepartmentCode { get; set; }

        [StringLength(1000)]
        public virtual string ImgUrlAvartar { get; set; }

        [StringLength(1000)]
        public virtual string ImgUrlCover { get; set; }

        /// <summary>
        /// 0 : Đã nghỉ việc ,
        /// 1 : Đang làm việc/Công ty đã thanh toán,
        /// 2 : Tạm nghỉ không lương(nghỉ sau sinh, nghỉ dài hạn vì lý do nào đó..)
        /// Mặc định = 1.
        /// </summary>
        public virtual byte Status { get; set; }

        [StringLength(250)]
        public virtual string BirthPlace { get; set; }

        [StringLength(50)]
        public virtual string TeamCode { get; set; }
        [Display(Name = "Mã chi nhánh")]
        public string BranchCode { get; set; }

        public string OfficeCode { get; set; }

        public int? Share { get; set; }

        public string MonthOfHoldingStocks{ get; set; }
    }
}
