using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblShowUpHistory")]
    public class ShowUpHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string CodeShow { get; set; }

        [StringLength(50)]
        public virtual string CodeEvent { get; set; }

        [StringLength(100)]
        public virtual string ShowContent { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string CodeInvestor { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string CodeStaff { get; set; }

        public virtual float? CountShowUp { get; set; }

        public virtual DateTime? TimeIn { get; set; }

        public virtual DateTime? TimeOut { get; set; }

        [StringLength(50)]
        public virtual string Table { get; set; }

        [StringLength(50)]
        public virtual string UserUpdate { get; set; }

        [StringLength(250)]
        public virtual string Note { get; set; }

        [StringLength(50)]
        public virtual string InvestorResourceCode { get; set; }

        [StringLength(50)]
        public virtual string Sale { get; set; }

        [StringLength(50)]
        public virtual string SaleTO { get; set; }

        public virtual decimal? ContractValue { get; set; }

        public virtual decimal? Deposit { get; set; }

        public virtual short? JoinedObject { get; set; }

        [StringLength(100)]
        public virtual string Gift { get; set; }

        /// <summary>
        /// false: gói đầu tư không còn hiệu lực
        /// true: gói đầu tư còn hiệu lực
        /// Mặc định : true
        /// </summary>
        public virtual bool Status { get; set; }

        public virtual bool? IsDirectAddition { get; set; }

        public virtual string CreatedBy { get; set; }
    }
}
