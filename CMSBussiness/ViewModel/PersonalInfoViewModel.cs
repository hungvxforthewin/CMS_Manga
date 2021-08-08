using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class PersonalInfoViewModel
    {
        public virtual long Id { get; set; }
        public virtual long IdConstracStaff { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập tên tài khoản")]
        [StringLength(50, ErrorMessage = "Tên tài khoản vượt quá 50 ký tự")]
        public virtual string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập mật khẩu")]
        [StringLength(50, ErrorMessage = "Mật khẩu vượt quá 50 ký tự")]
        public virtual string Pass { get; set; }

        [StringLength(10)]
        public virtual string CodeStaff { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập họ và tên")]
        [StringLength(100, ErrorMessage = " Họ và tên vượt quá 100 ký tự")]
        public virtual string FullName { get; set; }

        public virtual string Birthday { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn ngày sinh nhật")]
        public virtual string BirthdayString { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập email")]
        [StringLength(50, ErrorMessage = "Email vượt quá 50 ký tự")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public virtual string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập số điện thoại")]
        [StringLength(20, ErrorMessage = "Số điện thoại vượt quá 20 ký tự")]
        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
        public virtual string Phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn chức vụ")]
        [StringLength(50, ErrorMessage = "Mã chức vụ vượt quá 50 ký tự")]
        public virtual string PositionCode { get; set; }

        public virtual string PositionName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn phòng ban")]
        [StringLength(50, ErrorMessage = "Mã phòng ban vượt quá 50 ký tự")]
        public virtual string DepartmentCode { get; set; }

        public virtual string DepartmentName { get; set; }

        public virtual string OfficeCode { get; set; }

        public virtual string OfficeName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập nơi sinh")]
        [StringLength(250, ErrorMessage = "Nơi sinh vượt quá 250 ký tự")]
        public virtual string BirthPlace { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn nhóm")]
        [StringLength(50, ErrorMessage = "Mã nhóm vượt quá 50 ký tự")]
        public virtual string TeamCode { get; set; }

        public virtual string TeamName { get; set; }

        public virtual string StartDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn ngày bắt đầu")]
        public virtual string StartDateString { get; set; }

        //public virtual DateTime? EndDate { get; set; }

        public virtual string EndDateString { get; set; }

        public virtual decimal SalaryAmountBasic { get; set; }

        public virtual decimal SalaryAmountCapacity { get; set; }

        public virtual DateTime SalaryStartDate { get; set; }

        public virtual DateTime? SalaryEndDate { get; set; }

        public virtual decimal IncomeTax { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn mức bảo hiểm xã hội")]
        //[StringLength(15, ErrorMessage = "Mã mức bảo hiểm xã hội vượt quá 15 ký tự")]
        //public virtual string FeeSocialInsurance { get; set; }

        [StringLength(25)]
        public virtual string SocialInsuaranceNo { get; set; }

        /// <summary>
        /// 0 : Đã nghỉ việc ,
        /// 1 : Đang làm việc/Công ty đã thanh toán,
        /// 2 : Tạm nghỉ không lương(nghỉ sau sinh, nghỉ dài hạn vì lý do nào đó..)
        /// Mặc định = 1.(status of account)
        /// </summary>
        public virtual byte Status { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập số CMND/CCCD")]
        [StringLength(15, ErrorMessage = "Số CMND/CCCD vượt quá 15 ký tự")]
        public virtual string IdCard { get; set; }

        public virtual string IdCardIssuedDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn ngày cấp CMND/CCCD")]
        public virtual string IdCardIssuedDateString { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập nơi cấp CMND/CCCD")]
        [StringLength(250, ErrorMessage = "Nơi cấp CMND/CCCD vượt quá 250 ký tự")]
        public virtual string IdCardIssuedPlace { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập thông tin tôn giáo")]
        [StringLength(20, ErrorMessage = "Tôn giáo không vượt quá 20 ký tự")]
        public virtual string Religion { get; set; }

        public virtual bool? Gender { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập quốc tịch")]
        [StringLength(50, ErrorMessage = "Quốc tịch vượt quá 50 ký tự")]
        public virtual string Nationality { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập địa chỉ hiện tại")]
        [StringLength(250, ErrorMessage = "Địa chỉ hiện tại vượt quá 250 ký tự")]
        public virtual string CurrentAddress { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn chi nhánh công ty")]
        [StringLength(50, ErrorMessage = "Mã chi nhánh công ty vượt quá 50 ký tự")]
        public virtual string BranchCode { get; set; }

        public virtual string BranchCompany { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn mức thâm niên")]
        [StringLength(50, ErrorMessage = "Mã mức thâm niên không quá 50 ký tự")]
        public virtual string AllowanceCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn mức bảo hiểm xã hội")]
        [StringLength(50, ErrorMessage = "Mã mức bảo hiểm xã hội vượt quá 50 ký tự")]
        public virtual string SocialInsuaranceType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn mức thưởng thâm niên")]
        [StringLength(50, ErrorMessage = "Mã mức thưởng thâm niên vượt quá 50 ký tự")]
        public virtual string SeniorityBonus { get; set; }
        public string Role { get; set; }
        public string StartDateOfProbation { get; set; }
        public string StartDateOffical { get; set; }
        public string EndDate { get; set; }
        public string Avatar { get; set; }
    }

    public class DisplayPersonalTableViewModel
    {
        public virtual long Id { get; set; }

        public virtual string CodeStaff { get; set; }

        public virtual string UserName { get; set; }

        public virtual string FullName { get; set; }

        public virtual string Phone { get; set; }

        public virtual string PositionName { get; set; }

        public virtual string TeamName { get; set; }

        public virtual string DepartmentName { get; set; }

        public virtual byte Status { get; set; }

        public int Role { get; set; }

        public virtual string BranchName { get; set; }

        public string Email { get; set; }

        public float? Share { get; set; }

    }
    public class RoleViewModel
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
    public class EmployeeInfoModel
    {
        public virtual long Id { get; set; }

        public virtual string CodeStaff { get; set; }

        public virtual byte RoleAccount { get; set; }

        public virtual byte Role { get; set; }

        public virtual string FullName { get; set; }

        public virtual string BranchCode { get; set; }
    }

    public class SearchPersonalInfoModel
    {
        public string Key { get; set; }

        public string Position { get; set; }

        public string Branch { get; set; }

        public string Office { get; set; }

        public string Department { get; set; }

        public string Team { get; set; }

        public string Status { get; set; }

        public int Size { get; set; } = 10;

        public int Page { get; set; } = 1;

        public int Start { get; set; } = 1;
    }
    public class PersonViewModel
    {
        public virtual long Id { get; set; }
        public virtual long IdConstracStaff { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập tên tài khoản")]
        [StringLength(50, ErrorMessage = "Tên tài khoản vượt quá 50 ký tự")]
        public virtual string UserName { get; set; }

        public virtual string Pass { get; set; }

        [StringLength(10)]
        public virtual string CodeStaff { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập họ và tên")]
        [StringLength(100, ErrorMessage = " Họ và tên vượt quá 100 ký tự")]
        public virtual string FullName { get; set; }
        [Required]
        public virtual string Birthday { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập email")]
        [StringLength(50, ErrorMessage = "Email vượt quá 50 ký tự")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public virtual string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập số điện thoại")]
        [StringLength(20, ErrorMessage = "Số điện thoại vượt quá 20 ký tự")]
        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
        public virtual string Phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn chức vụ")]
        [StringLength(50, ErrorMessage = "Mã chức vụ vượt quá 50 ký tự")]
        public virtual string PositionCode { get; set; }

        public virtual string PositionName { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn phòng ban")]
        //[StringLength(50, ErrorMessage = "Mã phòng ban vượt quá 50 ký tự")]
        public virtual string DepartmentCode { get; set; }

        public virtual string DepartmentName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập nơi sinh")]
        [StringLength(250, ErrorMessage = "Nơi sinh vượt quá 250 ký tự")]
        public virtual string BirthPlace { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn nhóm")]
        //[StringLength(50, ErrorMessage = "Mã nhóm vượt quá 50 ký tự")]
        public virtual string TeamCode { get; set; }

        public virtual string TeamName { get; set; }
        //[Required(ErrorMessage = "Ngày thử việc không được trống")]
        public virtual string StartDateOfProbation { get; set; }

        [Required(ErrorMessage = "Ngày làm việc chính thức không được trống")]
        public virtual string StartDateOffical { get; set; }

        public virtual string EndDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập số CMND/CCCD")]
        [StringLength(15, ErrorMessage = "Số CMND/CCCD vượt quá 15 ký tự")]
        public virtual string IdCard { get; set; }
        [Required]
        public virtual string IdCardIssuedDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập nơi cấp CMND/CCCD")]
        [StringLength(250, ErrorMessage = "Nơi cấp CMND/CCCD vượt quá 250 ký tự")]
        public virtual string IdCardIssuedPlace { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập thông tin tôn giáo")]
        [StringLength(20, ErrorMessage = "Tôn giáo không vượt quá 20 ký tự")]
        public virtual string Religion { get; set; }

        public virtual bool? Gender { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập quốc tịch")]
        [StringLength(50, ErrorMessage = "Quốc tịch vượt quá 50 ký tự")]
        public virtual string Nationality { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập địa chỉ hiện tại")]
        [StringLength(250, ErrorMessage = "Địa chỉ hiện tại vượt quá 250 ký tự")]
        public virtual string CurrentAddress { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn chi nhánh công ty")]
        [StringLength(50, ErrorMessage = "Mã chi nhánh công ty vượt quá 50 ký tự")]
        public virtual string BranchCode { get; set; }

        public virtual string BranchCompany { get; set; }
        public virtual string OfficeCode { get; set; }

        public string Avatar { get; set; }
      
    }
}
