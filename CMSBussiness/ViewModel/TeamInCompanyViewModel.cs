using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class TeamInCompanyViewModel
    {
        public virtual long Id { get; set; }

        [StringLength(50, ErrorMessage = "")]
        public virtual string TeamCode { get; set; }

        public virtual string DepartmentCode { get; set; }

        public virtual string DepartmentName { get; set; }

        [StringLength(200, ErrorMessage = "")]
        public virtual string Name { get; set; }

        public virtual string BranchCode { get; set; }
        public virtual string OfficeCode { get; set; }

        public virtual string BranchName { get; set; }
        public virtual string OfficeName { get; set; }
        public bool Status { get; set; }
        public string StatusTeam { get; set; }
        public string StatusDepartment { get; set; }
        public string StatusOffice { get; set; }
        public string StatusBranch { get; set; }
        public string CodeStaffLeader { get; set; }
        public string NameStaffLeader { get; set; }
    }
    public class SearchTeamViewModel
    {
        public string Key { get; set; }
        public string Status { get; set; }
        public string BranchCode { get; set; }
        public string OfficeCode { get; set; }
        public string DepartmentCode { get; set; }
        public int Size { get; set; } = 10;
        public int Page { get; set; } = 1;
    }
}
