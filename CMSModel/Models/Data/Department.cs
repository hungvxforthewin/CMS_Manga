using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblDepartMent")]
    public class Department
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual long Id { get; set; }

        public virtual string DepartmentCode { get; set; }

     
        public virtual string DepartmentName { get; set; }

        [StringLength(50, ErrorMessage = "")]
        public virtual string CompanyCode { get; set; }

        [StringLength(50, ErrorMessage = "")]
        public virtual string BranchCode { get; set; }
        public virtual string OfficeCode { get; set; }
        public virtual string CodeStaffSaleManage { get; set; }

        /// <summary>
        /// Phòng ban tồn tại : true
        /// Phòng ban tạm đóng, phòng ban bị hủy bỏ : false
        /// Default : true
        /// </summary>
        public virtual bool Status { get; set; } 
    }
}
