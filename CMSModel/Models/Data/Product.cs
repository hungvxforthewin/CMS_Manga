using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMModel.Models.Data
{
    [Table("tblProduct")]
    public class Product
    {
        [CrudField(UsedFor = CrudFieldType.DontUse)]
        public int rownumber { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [CrudField(CrudFieldType.Update | CrudFieldType.Delete)]
        public virtual long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public virtual string ProductCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(250)]
        public virtual string Name { get; set; }

        [StringLength(250)]
        public virtual string Infomation { get; set; } //Information

        [StringLength(50)]
        public virtual string CompanyCode { get; set; }

        /// <summary>
        /// 0: Sản phẩm đã ngừng hoạt động
        /// 1: Sản phẩm đã xây dựng xong, đang hoạt động
        /// 2: Sản phẩm đang trong quá trình xây dựng
        /// Default: 1
        /// </summary>
        public virtual byte Status { get; set; }
    }
}
