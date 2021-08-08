using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    public class Earning
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string CodeEarning { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string CodeStaff { get; set; }

        public virtual decimal SalaryAmountBasicReal { get; set; }

        public virtual decimal SalaryAmountCapacityReal { get; set; }

        /// <summary>
        /// format ‘yyyy/MM’
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(8)]
        public virtual string Month { get; set; }

        [StringLength(50)]
        public virtual string DepartmentCode { get; set; }

        [StringLength(50)]
        public virtual string PositionCode { get; set; }

        [StringLength(50)]
        public virtual string CodeKpi { get; set; }

        public virtual float PercentKpi { get; set; }

        public virtual decimal? AmountSocialInsuarance { get; set; }

        public virtual decimal? AmountAttendance { get; set; }

        public virtual decimal? AmountUnion { get; set; }

        public virtual decimal? Remuneration { get; set; }

        public virtual decimal? AmountPunish { get; set; }

        public virtual short? SumShowup { get; set; }

        public virtual decimal? RevenueInMonth { get; set; }

        [StringLength(50)]
        public virtual string AllowanceCode { get; set; }

        public virtual decimal? ExtraSalary { get; set; }

        public virtual decimal? DecimalBonus { get; set; }

        public virtual decimal? AmountDeduct { get; set; }

        public virtual decimal? OtherBonus { get; set; }

        public virtual decimal? IncomeTax { get; set; }

        [StringLength(200)]
        public virtual string Note { get; set; }
    }
}
