using System.ComponentModel.DataAnnotations;

namespace CRMModel.Models.Data
{
    public class Punish
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual long IdPunish { get; set; }

        public virtual byte IdDeduct { get; set; }

        /// <summary>
        /// format ‘yyyy/MM’
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [StringLength(8)]
        public virtual string Month { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(10)]
        public virtual string CodeStaff { get; set; }

        public virtual byte? NoPenalties { get; set; }
    }
}
