using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class TypeCallViewModel
    {
        public virtual int Id { get; set; }

        [StringLength(50)]
        public virtual string TypeCallCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập tên loại cuộc gọi")]
        [StringLength(100, ErrorMessage = "Loại cuộc gọi không vượt quá 100 ký tự")]
        public virtual string NameTypeCall { get; set; }
    }
}
