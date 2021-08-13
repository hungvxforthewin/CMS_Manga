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

        [Required]
        public string BookName { get; set; }

        [Required]
        public string BookDescription { get; set; }

        [Required]
        public byte adultLimit { get; set; }

        [Required]
        public byte bookSexId { get; set; }

        public decimal Rating { get; set; }

        public int LikeNo { get; set; }

        public int FollowNo { get; set; }

        [Required]
        public bool isEnable { get; set; }

        [Required]
        public bool commentAllowed { get; set; }

        [Required]
        public int authorAccountId { get; set; }

        public byte updateStatus { get; set; }

        public DateTime lastUpdateTime { get; set; }
    } // class Book
}
