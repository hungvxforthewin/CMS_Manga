using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class RuleInMonthViewModel
    {
        public virtual short Id { get; set; }

        /// <summary>
        /// format ‘yyyy/MM’
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn tháng trong năm")]
        [StringLength(25, ErrorMessage = "Tháng đã chọn vượt quá 25 ký tự")]
        public virtual string Month { get; set; }

        public virtual byte TotalWorkingDays { get; set; }

        public virtual decimal? OtherBonus { get; set; }
    }
}
