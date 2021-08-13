using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMSModel.Models.Data
{
    [Table("Account")]
    public class Author
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public string AccountPassword { get; set; }

        public string Nickname { get; set; }

        public string AvatarImg { get; set; }

        public DateTime CreateTime { get; set; }

        public bool isActive { get; set; }

        public short OAuthSystemID { get; set; }

        public string OAuthAccount { get; set; }
    }
}
