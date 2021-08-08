using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMSModel.Models.Data
{
    [Table("tbl_BookSex_Info")]
    public class BookSexInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public byte bookSexId { get; set; }

        public string bookSexName { get; set; }
    }
}
