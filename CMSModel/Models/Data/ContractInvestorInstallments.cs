using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace CRMModel.Models.Data
{
    [Table("tblContractInvestorInstallments")]
    public class ContractInvestorInstallments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required]
        public string CodeContract { get; set; }
        public string DepositCode { get; set; }
        public int PaymentFormat { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime CreateDate { get; set; }
        public string IdStatusContract { get; set; }
        public string Description { get; set; }
    }
}
