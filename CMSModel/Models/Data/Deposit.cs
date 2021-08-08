using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblDeposit")]
    public class Deposit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string CodeDeposit { get; set; }

        [StringLength(100)]
        public virtual string NameDeposit { get; set; }

        [StringLength(50)]
        public virtual string CodeInvestor { get; set; }

        public virtual decimal DepositAmount { get; set; }

        [StringLength(50)]
        public virtual string TeleSale { get; set; }

        [StringLength(50)]
        public virtual string SaleRep { get; set; }

        /// <summary>
        /// 1: Đã đặt cọc
        /// 2 : đã chuyển sang trạng thái hợp đồng(không còn là đặt cọc nữa)
        /// 3: k đặt cọc nữa(chịu phạt hoặc k có phạt và rút tiền đi..)
        /// Default: 1
        /// </summary>
        public virtual byte Status { get; set; }
        public virtual int DepositForm { get; set; }
        public virtual DateTime? DepositDate { get; set; }
        public virtual DateTime? CreateDate { get; set; }
        public virtual DateTime? DestroyDate { get; set; }
        public virtual string Description { get; set; }
    }
}
