using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblContractStaff")]

    public class ContractStaff
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string LaborContractCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(200)]
        public virtual string LaborContractName { get; set; }

        [StringLength(50)]
        public virtual string AllowanceCode { get; set; }

        /// <summary>
        /// 1 : hợp đồng 2 tháng kể từ ngày bắt đầu làm việc (thử việc..)
        /// 2: hợp đồng 1 năm
        /// 3 : hợp động 3 năm
        /// 4: hợp đồng 5 năm
        /// 5 : hợp đồng vô thời hạn
        /// 6 : khác(loại hợp đồng thuê chuyên gia xử lý theo ngày hay nhiều loại khác..)
        /// </summary>
        public virtual byte Duration { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// 1: Toàn thời gian
        /// 2: Bán thời gian
        /// 3: CTV
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string LaborContractType { get; set; }

        public virtual decimal SalaryAmountBasic { get; set; }

        public virtual decimal SalaryAmountCapacity { get; set; }

        public virtual DateTime? SalaryStartDate { get; set; }

        public virtual DateTime? SalaryEndDate { get; set; }

        /// <summary>
        /// 1- lương net
        /// 2- lương gross
        /// </summary>
        public virtual byte? SalaryType { get; set; }

        //public virtual decimal? IncomeTax { get; set; }

        //public virtual decimal? FeeSocialInsurance { get; set; }

        [StringLength(25)]
        public virtual string SocialInsuaranceNo { get; set; }

        [StringLength(50)]
        public virtual string CompanyCode { get; set; }

        [StringLength(10)]
        public virtual string CodeStaff { get; set; }

        [StringLength(50)]
        public virtual string NameStaff { get; set; }

        /// <summary>
        /// 1 - Còn hiệu lực
        /// 2 - Hết hiệu lực
        /// 3 - Tạm dừng
        /// Default : 1
        /// </summary>
        public virtual byte Status { get; set; }

        [StringLength(50)]
        public virtual string PositionCode { get; set; }

        [StringLength(50)]
        public virtual string DepartmentCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(15)]
        public virtual string IdCard { get; set; }

        public virtual DateTime? IdCardIssuedDate { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(250)]
        public virtual string IdCardIssuedPlace { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(20)]
        public virtual string Religion { get; set; }

        public virtual bool? Gender { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string Nationality { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(250)]
        public virtual string CurrentAddress { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string BranchCompany { get; set; }

        [StringLength(50)]
        public virtual string SocialInsuaranceType { get; set; }

        [StringLength(50)]
        public virtual string SeniorityBonus { get; set; }
    }
}
