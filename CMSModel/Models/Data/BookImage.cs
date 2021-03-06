using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMSModel.Models.Data
{
    [Table("BookImage")]
    public class BookImage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }
        [Key]
        public int imgId { get; set; }

        public string imgUrl { get; set; }
        public bool IsBanner { get; set; }
    }
}
