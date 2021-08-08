using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class DepartmentViewModel
    {
        public virtual long Id { get; set; }

        [StringLength(50, ErrorMessage = "")]
        public virtual string DepartmentCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(200, ErrorMessage = "")]
        public virtual string DepartmentName { get; set; }

        [StringLength(50, ErrorMessage = "")]
        public virtual string BranchCode { get; set; }

        public virtual string BranchName { get; set; }

        public virtual string OfficeCode { get; set; }
        public virtual string OfficeName { get; set; }
        public virtual bool Status { get; set; }
        public virtual string StatusDepartment { get; set; }
        public virtual string StatusOfiice { get; set; }
        public virtual string StatusBranch { get; set; }
        public virtual string CodeStaffSaleManage { get; set; }
        public virtual string NameStaffSaleManage { get; set; }
    }
    public class SearchDepartmentViewModel
    {
        public string Key { get; set; }
        public string Status { get; set; }
        public string BranchCode { get; set; }
        public string OfficeCode { get; set; }
        public int Size { get; set; } = 10;
        public int Page { get; set; } = 1;
    }
}
