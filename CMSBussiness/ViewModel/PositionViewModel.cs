using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class PositionViewModel
    {
        public virtual long Id { get; set; }

        [StringLength(50, ErrorMessage = "")]
        public virtual string PositionCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "")]
        [StringLength(50, ErrorMessage = "")]
        public virtual string PositionName { get; set; }

        [StringLength(50, ErrorMessage = "")]
        public virtual string CompanyCode { get; set; }

        [StringLength(50, ErrorMessage = "")]
        public virtual string DepartmentCode { get; set; }

        /// <summary>
        /// true tồn tại, false: bị hủy bỏ
        /// Default: true
        /// </summary>
        public virtual bool Status { get; set; }
    }
}
