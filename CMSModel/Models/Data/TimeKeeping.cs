using System.ComponentModel.DataAnnotations;

namespace CRMModel.Models.Data
{
    public class TimeKeeping
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string CodeKeeping { get; set; }

        /// <summary>
        /// format ‘yyyy/MM’
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(8)]
        public virtual string Month { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(20)]
        public virtual string CodeStaff { get; set; }

        /// <summary>
        /// Mặc định : 0
        /// </summary>
        public virtual float TotalWorkingDays { get; set; }

        public virtual byte? TotalLates { get; set; }

        public virtual byte? TotalEarlyOuts { get; set; }

        public virtual byte? TotalWithoutReason { get; set; }

        public virtual byte? ForgetCheckOutIn { get; set; }

        public virtual float? TotalTakeLeaveInMonth{ get; set; }

        public virtual float? TotalShowupInMonth { get; set; }

        public virtual float? TotalContract { get; set; }

        public virtual decimal? RevenueInMonth { get; set; }
    }
}
