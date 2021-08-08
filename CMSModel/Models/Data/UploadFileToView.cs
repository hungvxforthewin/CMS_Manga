using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblUploadFileToView")]
    public class UploadFileToView
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(250)]
        public string LinksUpload { get; set; }

        [StringLength(150)]
        public string Reason { get; set; }

        public DateTime? CreateDate { get; set; }

        public string UserUpload { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string UserUpdate { get; set; }

        public int  IdContractInvestor { get; set; }
    }
}
