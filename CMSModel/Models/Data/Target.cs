using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblTargets")]
    public class Target
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// 1: branches
        /// 7: offices
        /// 10: departments
        /// 6: teams
        /// 5: sales
        /// </summary>
        public byte Role { get; set; }

        [StringLength(50)]
        public string SetTargetFor { get; set; }

        public decimal? Revenue { get; set; }

        public short? ShowUp { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool Status { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }
    }
}
