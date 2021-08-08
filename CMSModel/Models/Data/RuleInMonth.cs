using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    public class RuleInMonth
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual short Id { get; set; }

        /// <summary>
        /// format ‘yyyy/MM’
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(25)]
        public virtual string Month { get; set; }

        public virtual byte TotalWorkingDays { get; set; }

        public virtual decimal? OtherBonus { get; set; }
    }
}
