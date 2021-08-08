using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblInvestorsCareHistory")]
    public class InvestorsCareHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        public string HistoryCode { get; set; }

        [Required]
        public string ProductCode { get; set; }

        [Required]
        public string EventCode { get; set; }

        public bool? StatusCode { get; set; }

        [Required]
        public string LevelConcernCode { get; set; }

        public DateTime? CreatedDate { get; set; }

        [Required]
        public string CodeStaff { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [Required]
        public string InvestorCode { get; set; }
    }
}
