using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblLevelConcern")]
    public class LevelConcern
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual int Id { get; set; }

        public virtual string LevelConcernCode { get; set; }

        [StringLength(250)]
        public virtual string NameConcern { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        /// <summary>
        /// true : đang sử dụng
        /// false : không còn sử dụng
        /// </summary>
        public virtual bool Status { get; set; }
    }
}
