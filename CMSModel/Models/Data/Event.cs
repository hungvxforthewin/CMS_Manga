using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblEvent")]
    public class Event
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string CodeEvent { get; set; }

        [StringLength(50)]
        public virtual string ProductCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(250)]
        public virtual string Name { get; set; }

        [StringLength(250)]
        public virtual string Address { get; set; }

        public virtual DateTime? EventTime { get; set; }

        public virtual DateTime? EndTime { get; set; }

        [StringLength(50)]
        public virtual string CreatedBy { get; set; }
        
        /// <summary>
        /// 0: sự kiện đã hủy bỏ
        /// 1: sự kiện chưa diễn ra, sắp diễn ra, đang diễn ra
        /// 2 : sự kiện đã diễn ra
        /// default : 1
        /// </summary>
        public virtual byte Status { get; set; }
    }
}
