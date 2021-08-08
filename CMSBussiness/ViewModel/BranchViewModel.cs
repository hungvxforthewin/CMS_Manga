using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class BranchViewModel
    {
        public virtual long Id { get; set; }

        [StringLength(50)]
        public virtual string BranchCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(250)]
        public virtual string BranchName { get; set; }

        [StringLength(250)]
        public virtual string Address { get; set; }
        public virtual string Status { get; set; }
        public virtual string CodeStaffAdminSale { get; set; }
        public virtual string NameStaffAdminSale { get; set; }
    }
    public class SearchBranchViewModel
    {
        public string Key { get; set; }
        public string Status { get; set; }
        public int Size { get; set; } = 10;
        public int Page { get; set; } = 1;
    }
}
