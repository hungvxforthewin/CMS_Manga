using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblBranch")]
    public class Branch
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string BranchCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(250)]
        public virtual string BranchName { get; set; }

        [StringLength(250)]
        public virtual string Address { get; set; }

        [StringLength(50)]
        public virtual string CompanyCode { get; set; }
        public virtual string CodeStaffAdminSale { get; set; }

        /// <summary>
        /// Chi nhánh tồn tại : true
        /// Chi nhánh không còn tồn tại : false
        /// </summary>
        public virtual bool Status { get; set; }
    }
}
