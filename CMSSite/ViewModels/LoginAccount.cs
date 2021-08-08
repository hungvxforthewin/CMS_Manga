using System.ComponentModel.DataAnnotations;

namespace CRMSite.ViewModels
{
    public class LoginAccount
    {
        //[Required(AllowEmptyStrings = false, ErrorMessage = "ACCOUNT_REQUIRED_PHONE")]
        //[StringLength(20)]
        //public virtual string Phone { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập tên tài khoản")]
        [StringLength(50, ErrorMessage = "Tên tài khoản không vượt quá 50 ký tự")]
        public virtual string UserName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập mật khẩu")]
        [StringLength(50, ErrorMessage = "Mật khẩu không vượt quá 50 ký tự")]
        public virtual string Pass { get; set; }
    }
}
