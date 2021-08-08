using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CRMModel.Models.Data
{
    [Table("tblSalaryRealWithRuleKpi")]
    public class SalaryRealWithRuleKpi
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual short Id { get; set; }

        public virtual byte RoleAccount { get; set; }

        public virtual byte RatioKpiMin { get; set; }

        public virtual byte RatioKpiMax { get; set; }

        public virtual byte PercentSalary { get; set; }

        public virtual decimal? RevenueMin { get; set; }

        public virtual decimal? RevenueMax { get; set; }

        public virtual decimal? SalaryReal { get; set; }

        public virtual bool StatusProbationary { get; set; }

        public virtual bool Status { get; set; }

        public virtual decimal? RevenuSmMin { get; set; }

        public virtual decimal? RevenuSmMax { get; set; }

    }
}
