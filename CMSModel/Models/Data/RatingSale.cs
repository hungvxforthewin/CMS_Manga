using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblRatingSale")]
    public class RatingSale
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Sale { get; set; }
        [Required]
        public decimal RevenueSale { get; set; }
        [Required]
        public DateTime RevenueDate { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        /// <summary>
        /// (1: theo ngày(mặc định), 2: theo tháng, 3: theo năm)
        /// </summary>
        public int IsByTime { get; set; }
    }
}
