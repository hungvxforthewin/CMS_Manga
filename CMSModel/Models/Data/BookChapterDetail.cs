using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMSModel.Models.Data
{
    [Table("BookChapterDetail")]
    public class BookChapterDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public Guid ChapterId { get; set; }

        public int Page { get; set; }

        public string PageImgUrl { get; set; }
    }
}
