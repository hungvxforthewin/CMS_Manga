using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{

    [Table("tblAllowanceOrDeduct")]
    public partial class AllowanceOrDeduct
    {
        [CrudField(UsedFor = CrudFieldType.DontUse)]
        public int rownumber { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [CrudField(CrudFieldType.Update | CrudFieldType.Delete)]
        public virtual long Id { get; set; }
        [Required]
        [Display(Name = "Mã nhập tay")]
        public virtual string AllowanceCode { get; set; }
        [Display(Name = "Tên gói")]
        [Required(ErrorMessage = "Không được trống")]
        public virtual string AllowanceName { get; set; }
        [Display(Name = "Loại")]
        public byte? Type { get; set; }
        [Required(ErrorMessage = "Cộng, trừ vào thu nhập hàng tháng không được trống")]
        [Display(Name = "Cộng/Trừ")]
        public bool UpOrDown { get; set; }
        [Required(ErrorMessage = "Cách tính không được trống")]
        public bool Calculation { get; set; }
        [Required(ErrorMessage = "Số tiền không được trống")]
        [Display(Name = "Số tiền")]
        public virtual decimal AllowanceAmount { get; set; }
        [Display(Name = "Số phần trăm")]
        public byte? AllwancePercent { get; set; }
        [StringLength(250)]
        [Display(Name = "Ghi chú")]
        public virtual string Note { get; set; }

        /// <summary>
        /// true : gói hỗ trợ đang được áp dụng gói hỗ trợ đang được áp dụng
        /// false : gói hỗ trợ hiện k còn được áp dụng
        /// </summary>
        //[Required(ErrorMessage = "Trạng thái")]
        [Display(Name = "Trạng thái")]

        public virtual bool Status { get; set; }
    }
}
