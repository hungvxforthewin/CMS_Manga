using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class AllowanceOrDeductViewModel
    {
        public virtual long Id { get; set; }

        [StringLength(50)]
        public virtual string AllowanceCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập tên")]
        [StringLength(50, ErrorMessage = "Tên vượt quá 50 ký tự")]
        public virtual string AllowanceName { get; set; }

        /// <summary>
        /// Trường này có thể để null.
        ///1 : các loại thưởng thâm niên
        ///2: Thưởng chuyên cần
        ///3: BHXH
        ///4: Phí công đoàn
        ///5 : xăng xe
        /// </summary>
        public virtual byte? Type { get; set; }

        /// <summary>
        /// true : tăng, cộng vào thu nhập hàng tháng
        /// false : trừ, giảm thu nhập hàng tháng

        /// </summary>
        public virtual bool UpOrDown { get; set; }

        /// <summary>
        /// true : tinh bằng tiền (sử dụng trường AllowanceAmount)
        /// false : tính theo kiểu % (AllwancePercent)
        /// </summary>
        public virtual bool Calculation { get; set; }

        public virtual decimal? AllowanceAmount { get; set; }

        public virtual byte? AllwancePercent { get; set; }

        [StringLength(250)]
        public virtual string Note { get; set; }

        /// <summary>
        /// false: đã hủy bỏ
        /// true: đang được áp dụng
        /// Mặc định : true
        /// </summary>
        public virtual bool Status { get; set; }
    }

    public class SearchAllowanceInfoModel
    {
        public byte Type { get; set; }

        public string Key { get; set; }

        public int Size { get; set; } = 10;

        //public int Pages { get; set; } = 5;

        public int Start { get; set; } = 1;
    }
}
