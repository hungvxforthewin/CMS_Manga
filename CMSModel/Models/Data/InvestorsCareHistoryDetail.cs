using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblInvestorsCareHistoryDetail")]
    public class InvestorsCareHistoryDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public string HistoryCode { get; set; }

        [Required]
        public string CodeInvestor { get; set; }

        [Required]
        public string CodeStaff { get; set; }

        public string CompanyCode { get; set; }

        [Required]
        public string SupportTime { get; set; }

        public string Content { get; set; }

        public string StatusFollow { get; set; }

        public string Note { get; set; }

        public DateTime? DateCall { get; set; }
    }
}
