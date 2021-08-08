using System;
using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class EventViewModel
    {
        public long Id { get; set; }

        [StringLength(50, ErrorMessage = "Mã show up vượt quá 50 ký tự")]
        public string CodeEvent { get; set; }

        [StringLength(50, ErrorMessage = "Mã sản phẩm vượt quá 50 ký tự")]
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập tên showup")]
        [StringLength(250, ErrorMessage = "Tên showup vượt quá 250 ký tự")]
        public string Name { get; set; }

        public DateTime? EventTime { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn thời gian bắt đầu")]
        public string EventTimeString { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn ngày bắt đầu")]
        public string EventDateString { get; set; }

        public DateTime? EndTime { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn thời gian kết thúc")]
        public string EndTimeString { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn ngày kết thúc")]
        public string EndDateString { get; set; }

        [StringLength(50, ErrorMessage = "Mã nhân viên tạo show up không vượt quá 50 ký tự")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 0: sự kiện đã hủy bỏ
        /// 1: sự kiện chưa diễn ra, sắp diễn ra, đang diễn ra
        /// 2 : sự kiện đã diễn ra
        /// default : 1
        /// </summary>
        public byte Status { get; set; }

        public bool CreatedForCC { get; set; }
    }

    public class SearchEventModel
    {
        public string StartDateString { get; set; }

        public string StartTimeString { get; set; }

        public DateTime? StartDate { get; set; }

        public string EndDateString { get; set; }

        public string EndTimeString { get; set; }

        public DateTime? EndDate { get; set; }

        public byte? Status { get; set; }

        public string Key { get; set; }

        public int Page { get; set; } = 1;

        public int Size { get; set; } = 10; 

        public string Branch { get; set; }
    }
}
