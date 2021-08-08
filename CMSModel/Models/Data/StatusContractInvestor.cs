using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblStatusContractInvestors")]
    public class StatusContractInvestor
    {
        //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //    [Key]
        //    public virtual int Id { get; set; }
        //public virtual string ContractInvestorCode { get; set; }
        public virtual string IdStatusContract { get; set; }

        [StringLength(250)]
        public virtual string NameStatus { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        [StringLength(250)]
        public virtual string Description { get; set; }

        /// <summary>
        /// true: Loại trạng thái hợp đồng còn sử dụng
        /// false : Loại trạng thái hợp đồng k còn dc sử dụng
        /// </summary>
        public virtual bool Status { get; set; }
        /// <summary>
        /// Hình thức thanh toán
        /// 0: Chuyển khoản
        /// 1: Tiền mặt
        /// </summary>
        //public virtual byte PaymentFormat { get; set; }
        /// <summary>
        /// Tiền thanh toán
        /// </summary>
        //public virtual decimal PaymentAmount { get; set; }
    }
}
