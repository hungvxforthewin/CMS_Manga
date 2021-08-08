using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMSModel.Models.Data
{
    [Table("BookChapter")]
    public class BookChapter
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int BookId { get; set; }
        [Key]
        public Guid ChapterId { get; set; }

        public string ChapterName { get; set; }

        public byte adultLimit { get; set; }

        public int LikeNo { get; set; }

        public int CommentNo { get; set; }

        public byte ChapterStatus { get; set; }

        public DateTime PublishDate { get; set; }

        public DateTime lastUpdateTime { get; set; }

        public bool isPremium { get; set; }
    }
}
