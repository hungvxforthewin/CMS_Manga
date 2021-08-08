using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblTypeContract")]
    public class TypeContract
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string TypeContractCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(250)]
        public virtual string NameType { get; set; }

        [StringLength(250)]
        public virtual string Content { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        /// <summary>
        /// true : đang sử dụng
        /// false : không còn sử dụng
        /// </summary>
        public virtual bool Status { get; set; }
    }
}
