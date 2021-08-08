using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class FileUploadViewModel
    {
        public long Id { get; set; }

        public IFormFile File { get; set; }

        [StringLength(250, ErrorMessage = "Đường dẫn lưu file không vượt quá 250 ký tự")]
        public string LinksUpload { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập lý do tải hình lên")]
        [StringLength(150, ErrorMessage = "Lý do không vượt quá 250 ký tự")]
        public string Reason { get; set; }

        public DateTime? CreateDate { get; set; }

        public string CreateDateString { get; set; }

        public string UserUpload { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string UpdateDateString { get; set; }

        public string UserUpdate { get; set; }

        public int IdContractInvestor { get; set; }
    }
}
