using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMSModel.Models.Data
{
    [Table("tbl_MediaServer_Config")]
    public class MediaServerConfig
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int MediaId { get; set; }

        public string MediaDesc { get; set; }

        public string MediaPath { get; set; }
    }
}
