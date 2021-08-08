using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    public class Menu
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual short Id { get; set; }

        [StringLength(250)]
        public virtual string NameMenu { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        /// <summary>
        /// true : đang sử dụng
        /// false : không còn sử dụng
        /// </summary>
        public virtual bool Status { get; set; }
    }
}
