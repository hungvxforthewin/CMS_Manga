using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblRemuneration")]
    public class Remuneration
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual byte Id { get; set; }

        [StringLength(250)]
        public virtual string CodeRemuneration { get; set; }

        [StringLength(250)]
        public virtual string CodeKpi { get; set; }

        public virtual byte PercentKpiMin { get; set; }

        public virtual byte PercentKpiMax { get; set; }

        public virtual byte RoleAccount { get; set; }

        public virtual decimal? Percent { get; set; }

        public virtual decimal? AmountContractTele { get; set; }

        public virtual decimal? AmountShowupTele { get; set; }

        public virtual decimal? AmountMinInMonth { get; set; }

        public virtual decimal? AmountMaxInMonth { get; set; }

        public virtual decimal? MinRevenueTeam { get; set; }


        public virtual decimal? MaxRevenueTeam { get; set; } 

        public virtual decimal? MinRevenueSM { get; set; }

        public virtual decimal? MaxRevenueSM { get; set; }
        // THÊM MỚI 2 COLUMN TRONG DB
        [CrudField(CrudFieldType.DontUse)]
        public string MinMaxRevenueBranch { get; set; }
        public virtual decimal? MinRevenueBranch { get; set; }
        public virtual decimal? MaxRevenueBranch { get; set; }

    }
}
