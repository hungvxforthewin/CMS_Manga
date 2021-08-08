using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class WhereToFindInvestorViewModel
    {
        public virtual int Id { get; set; }

        [StringLength(50)]
        public virtual string InvestorResourceCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập tên nguồn khách hàng")]
        [StringLength(250, ErrorMessage = "Tên nguồn khách hàng vượt không vượt quá 250 ký tự")]
        public virtual string AddressFind { get; set; }
    }
}
