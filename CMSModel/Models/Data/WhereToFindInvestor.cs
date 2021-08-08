using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblWhereToFindInvestor")]
    public class WhereToFindInvestor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual int Id { get; set; }

        [StringLength(50)]
        public virtual string InvestorResourceCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(250)]
        public virtual string AddressFind { get; set; }

        public bool? Bytele { get; set; }
    }
}
