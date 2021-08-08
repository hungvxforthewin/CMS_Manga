using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblLevelSalaryRevenue")]

    public class LevelSalaryRevenue
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [CrudField(CrudFieldType.Update | CrudFieldType.Delete)]
        public short Id { get; set; }

        public byte RoleAccount { get; set; }

        public decimal Salary { get; set; }


        public float? PercentRemuneration { get; set; }

        public decimal? RemunerationShowUp { get; set; }

        public decimal? RemunerationContractTele { get; set; }

        public float? PercentKpiMin { get; set; }

        public float? PercentKpiMax { get; set; }

		[StringLength(50)]
        public string CodeKpi { get; set; }

        public string SalaryMin { get; set; }

        public byte? TeamSize { get; set; }

        public byte? TotalTeams { get; set; }

        public decimal? RevenueMin { get; set; }

        public decimal? RevenueMax { get; set; }

        public DateTime? CreateDate { get; set; }

        public bool Status { get; set; }

        public byte? TimeKpi { get; set; }

        public decimal? ProbationarySalary { get; set; }

        public float? SalaryPercentLv1 { get; set; }

        public byte? ProbationaryTime { get; set; }

        public float? SharePercent { get; set; }
    }
}
