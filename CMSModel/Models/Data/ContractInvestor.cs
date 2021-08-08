using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblContractInvestors")]
    public class ContractInvestor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual int Id { get; set; }
        public virtual string IdStatusContract { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string CodeContract { get; set; }

        [StringLength(100)]
        public virtual string NameContract { get; set; }

        [StringLength(50)]
        public virtual string CodeInvestor { get; set; }

        public virtual string CodeDeposit { get; set; }

        public virtual decimal? InvestmentAmount { get; set; }

        [StringLength(50)]
        public virtual string TeleSale { get; set; }

        [StringLength(50)]
        public virtual string SaleRepresent { get; set; }

        public virtual string Sale { get; set; }

        public virtual DateTime CreateDate { get; set; }

        public virtual DateTime? PayOffDate { get; set; }
        public virtual DateTime? DatePaydone { get; set; }

        /// <summary>
        /// Mặc định : đã duyệt.
        /// </summary>
        //public virtual byte Status { get; set; }
        public virtual string CodeIntermediaries { get; set; }
        /// <summary>
        /// HĐ CHUYỂN NHƯỢNG
        /// </summary>
        public virtual string CodeTransferAgreement { get; set; }
        /// <summary>
        /// HĐ QUẢN LÝ DANH MỤC ĐẦU TƯ
        /// </summary>
        public virtual string CodeManagementCatalog { get; set; }
        /// <summary>
        /// HĐ THỎA THUẬN BỔ SUNG
        /// </summary>
        public virtual string CodeSupplementAgreement { get; set; }
    }
}
