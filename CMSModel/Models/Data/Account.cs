using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("CMS_Account")]
    public class Account // CMS_Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AccountID { get; set; }

        public string AccountName { get; set; }

        public string AccountPassword { get; set; }

        public string AccountFullName { get; set; }

        public bool? isEnable { get; set; }

        public DateTime? CreateDate { get; set; }

    } // class CMS_Account
}