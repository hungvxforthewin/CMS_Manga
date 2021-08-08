using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblKpi")]
    public class Kpi
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string CodeKpi { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public virtual string KpiName { get; set; }

        [StringLength(50)]
        public virtual string BranchCode { get; set; }

        [StringLength(50)]
        public virtual string CompanyCode { get; set; }

        [StringLength(50)]
        public virtual string DepartmentCode { get; set; }

        [StringLength(50)]
        public virtual string TeamCode { get; set; }

        [StringLength(50)]
        public virtual string PositionCode { get; set; }

        [Required]
        public virtual byte TypeKpi { get; set; }

        //public virtual short? TotalInvestors { get; set; }

        public virtual decimal? Revenue { get; set; }

        public virtual float? TotalShowUp { get; set; }

        public virtual byte? RoleAccount { get; set; }

        [StringLength(10)]
        public virtual string CodeStaff { get; set; }
        /// <summary>
        /// true : KPI tồn tại
        /// false : KPI bị hủy bỏ
        /// Default: true
        /// </summary>
        public virtual bool Status { get; set; }

        public virtual byte TypeContract { get; set; }
    }
}
