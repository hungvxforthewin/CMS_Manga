using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMBussiness.ViewModel
{
    public class InvestorViewModel
    {
        public virtual long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string CodeInvestor { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        [Display(Name = "Họ và tên")]
        public virtual string Name { get; set; }

        [StringLength(15)]
        [Display(Name = "Mã thẻ")]
        public virtual string IdCard { get; set; }

        [Display(Name = "Ngày cấp")]
        public virtual DateTime? DateOfIssuance { get; set; }
        public virtual string DateOfIssuanceString { get; set; }

        [StringLength(250)]
        [Display(Name = "Nơi cấp")]
        public virtual string AddressIssuance { get; set; }

        [StringLength(50)]
        public virtual string AccountBank { get; set; }
        /// <summary>
        /// TÀI KHOẢN LƯU KÝ KHI KÊ TOÁN DUYỆT
        /// </summary>
        public virtual string AccountBank2 { get; set; }

        [StringLength(250)]
        public virtual string Bank { get; set; }

        [StringLength(50)]
        public virtual string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(15)]
        [Display(Name = "SĐT")]
        public virtual string PhoneNumber { get; set; }

        [StringLength(15)]
        public virtual string PhoneNumber2 { get; set; }

        public virtual DateTime? Birthday { get; set; }
        public virtual string BirthdayString { get; set; }

        [StringLength(200)]
        [Display(Name = "Địa chỉ")]
        public virtual string Address { get; set; }

        [StringLength(200)]
        public virtual string Address2 { get; set; }

        [StringLength(50)]
        public virtual string CompanyCode { get; set; }

        [StringLength(500)]
        public virtual string LstProductsOfInterest { get; set; }

        /// <summary>
        /// 0 : Khách hàng chưa đầu tư
        /// 1 : Khách hàng đã và đang đầu tư
        /// 2 : Khách hàng đã đầu tư và hiện tại thì đã rút hết khoản đầu tư
        /// Default: 0
        /// </summary>
        public virtual byte Status { get; set; }
        
        [StringLength(50)]
        public virtual string CodeShowUpWithGroup { get; set; }
        [Required]
        public string PersonalTaxCode { get; set; }
        /// <summary>
        /// TỔNG SỐ TIỀN CỌC QUA CÁC ĐỢT TRẢ TRƯỚC
        /// </summary>
        public string SumPaymentAmount { get; set; }
        public string CodeDeposit { get; set; }
        public bool IsCMT { get; set; }
    }
}
