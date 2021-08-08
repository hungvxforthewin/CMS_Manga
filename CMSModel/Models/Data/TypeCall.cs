using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblTypeCall")]
    public class TypeCall
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string TypeCallCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public virtual string NameTypeCall { get; set; }

        [StringLength(250)]
        public virtual string Note { get; set; }


        /// <summary>
        /// true : đang được áp dụng
        /// false : k dc áp dụng tại thời điểm hiện tại
        /// </summary>
        public virtual bool Status { get; set; }
    }
}
