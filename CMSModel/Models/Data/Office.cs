using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblOffice")]
    public class Office
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public  virtual long  Id { get; set; }
        public  virtual string OfficeCode { get; set; }
        public  virtual string  OfficeName { get; set; }
        public  virtual string BranchCode { get; set; }
        public  virtual string  CompanyCode { get; set; }
        public  virtual string  CodeStaffOffice { get; set; }
        public  virtual bool Status { get; set; }
    }
}
