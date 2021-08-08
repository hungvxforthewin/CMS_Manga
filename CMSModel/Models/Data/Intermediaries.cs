using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace CRMModel.Models.Data
{
    [Table("tblIntermediaries")]
    public class Intermediaries
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public string UserCreate { get; set; }
        public string UserUpdate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        [Required(ErrorMessage = "CodeIntermediaries không được trống")]
        public string CodeIntermediaries { get; set; }
        public string Address { get; set; }
        //[Required]
        public string Phone { get; set; }
        [Required]
        public string TaxCode { get; set; }
        public bool Status { get; set; }
    }
}
