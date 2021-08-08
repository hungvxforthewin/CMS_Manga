using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class LevelConcernViewModel
    {
        public virtual int Id { get; set; }

        public virtual string LevelConcernCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập tên mức độ quan tâm")]
        [StringLength(250, ErrorMessage = "Tên mức độ quan tâm không quá 250 ký tự")]
        public virtual string NameConcern { get; set; }
    }
}
