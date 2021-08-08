using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblTeamInCompany")]
    public class TeamInCompany
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual long Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "")]
        public virtual string TeamCode { get; set; }

        [StringLength(50, ErrorMessage = "")]
        public virtual string DepartmentCode { get; set; }

        [StringLength(200, ErrorMessage = "")]
        public virtual string Name { get; set; }

        [StringLength(50, ErrorMessage = "")]
        public virtual string CompanyCode { get; set; }

        [StringLength(500, ErrorMessage = "")]
        public virtual string lstCodeStaff { get; set; }

        [StringLength(10, ErrorMessage = "")]
        public virtual string CodeStaffLeader { get; set; }

        /// <summary>
        /// team tồn tại : true, team bị hủy bỏ : false
        /// Default: true
        /// </summary>
        [StringLength(10, ErrorMessage = "")]
        public virtual bool Status { get; set; }
    }
}
