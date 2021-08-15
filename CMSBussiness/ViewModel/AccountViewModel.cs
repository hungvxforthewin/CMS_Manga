using System;
using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class AccountViewModel
    {
        public int AccountID { get; set; }

        [Required(ErrorMessage = "Tên không được trống")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Password không được trống")]
        public string AccountPassword { get; set; }

        public string AccountFullName { get; set; }

        public string NameUserCreate { get; set; }

        public bool? isEnable { get; set; } = true;

        public string Status { get; set; }

        public DateTime? CreateDate { get; set; }

        public string CreateDateString { get; set; }

        public int Role { get; set; }

        //public int ResponseStatus { get; set; }
    }
    public class SearchAccountViewModel
    {
      
        public string Key { get; set; }

        public int Size { get; set; } = 10;

        public int Page { get; set; } = 1;
    }
}
