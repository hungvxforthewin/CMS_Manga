using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class TypeContractViewModel
    {
        public virtual int Id { get; set; }

        [StringLength(50, ErrorMessage = "Mã loại hợp đồng vượt qúa 50 ký tự")]
        public virtual string TypeContractCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập tên hợp đồng")]
        [StringLength(250, ErrorMessage = "Tên loại hợp đồng vượt quá 250 ký tự")]
        public virtual string NameType { get; set; }

        [StringLength(250, ErrorMessage = "Nội dung loại hợp đồng vượt quá 250 ký tự")]
        public virtual string Content { get; set; }
    }

    public class SearchTypeContractModel
    {
        public string Key { get; set; }

        public int Size { get; set; } = 10;

        public int Page { get; set; } = 1;
    }
}
