using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMSModel.Models.Data
{
    [Table("Book")]
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int BookId { get; set; }

        public Guid BookUUID { get; set; }

        public string BookName { get; set; }

        public string BookDescription { get; set; }

        public byte adultLimit { get; set; }

        public byte bookSexId { get; set; }

        public decimal Rating { get; set; }

        public int LikeNo { get; set; }

        public int FollowNo { get; set; }

        public bool isEnable { get; set; }

        public bool commentAllowed { get; set; }

        public int authorAccountId { get; set; }

        public byte updateStatus { get; set; }

        public DateTime lastUpdateTime { get; set; }
    } // class Book
}
