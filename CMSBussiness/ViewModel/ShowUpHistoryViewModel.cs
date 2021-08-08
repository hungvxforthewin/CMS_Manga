using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class ShowUpHistoryTableViewModel
    {
        public long Id { get; set; }

        //[Required(AllowEmptyStrings = false)]
        //[StringLength(50)]
        //public string CodeShow { get; set; }

        //[StringLength(50)]
        //public string CodeEvent { get; set; }

        public string EventName { get; set; }

        //public string ProductName { get; set; }

        //[Required(AllowEmptyStrings = false)]
        //[StringLength(50)]
        public string CodeInvestor { get; set; }

        public string InvestorName { get; set; }

        public string PhoneNumber { get; set; }

        public string TimeInString { get; set; }

        public string TimeOutString { get; set; }

        public string Sale { get; set; }

        public string SaleTO { get; set; }

        public string TeleSale { get; set; }

        //public string UserUpdate { get; set; }

        public List<GoWithInvestorModel> Group { get; set; }
    }

    public class ShowUpHistoryCreateViewModel
    {
        public long Id { get; set; }

        [StringLength(50, ErrorMessage = "Mã checkin vượt quá 50 ký tự")]
        public string CodeShow { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn show up")]
        [StringLength(50, ErrorMessage = "Mã show up vượt quá 50 ký tự")]
        public string CodeEvent { get; set; }

        [StringLength(50, ErrorMessage = "Mã khách hàng vượt quá 50 ký tự")]
        public string CodeInvestor { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập tên khách hàng")]
        [StringLength(100, ErrorMessage = "Họ tên khách vượt quá 100 ký tự")]
        public string InvestorName { get; set; }

        [StringLength(50)]
        public string ProductCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập số điện thoại khách hàng")]
        [StringLength(15, ErrorMessage = "Số điện thoại vượt quá 15 ký tự")]
        public string PhoneNumber { get; set; }

        [StringLength(15, ErrorMessage = "CMND/CCCD vượt quá 15 ký tự")]
        public string IdCard { get; set; }

        public DateTime Birthday { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập ngày sinh của khách hàng")]
        [StringLength(15, ErrorMessage = "Ngày sinh khách hàng vượt quá 15 ký tự")]
        public string BirthdayString { get; set; }

        //public string Status { get; set; }

        [StringLength(50, ErrorMessage = "Tên bàn vượt quá 50 ký tự")]
        public string Table { get; set; }

        public string Sale { get; set; }

        public string SaleTO { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn telesale")]
        [StringLength(50, ErrorMessage = "Mã nhân viên telesale vượt quá 50 ký tự")]
        public string TeleSale { get; set; }

        public decimal? ContractValue { get; set; }

        public decimal? Deposit { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa chọn nguồn khách hàng")]
        [StringLength(50, ErrorMessage = "Mã nguồn khách vượt quá 50 ký tự")]
        public string InvestorResourceCode { get; set; }

        public short? JoinedObject { get; set; }

        //[StringLength(50, ErrorMessage = "Mã nhân viên telesale vượt quá 50 ký tự")]
        public string CodeShowUpWithGroup { get; set; }

        [StringLength(250, ErrorMessage = "Ghi chú vượt quá 50 ký tự")]
        public string Note { get; set; }

        public string UserUpdate { get; set; }

        [StringLength(100, ErrorMessage = "Quà tặng vượt quá 100 ký tự")]
        public string Gift { get; set; }

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }

        public string CreatedBy { get; set; }

        public List<GoWithInvestorModel> Group { get; set; }
    }

    public class CheckinInfoModel
    {
        public long Id { get; set; }

        public string CodeEvent { get; set; }

        public string CodeShow { get; set; }

        public string CodeInvestor { get; set; }

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }

        public DateTime ShowStartTime { get; set; }

        public DateTime ShowEndTime { get; set; }
    }

    public class GoWithInvestorModel
    {
        public long Id { get; set; }

        [StringLength(50)]
        public string CodeInvestor { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập tên khách đi theo nhóm")]
        [StringLength(100, ErrorMessage = "Họ tên khách đi theo nhóm vượt quá 100 ký tự")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập số điện thoại khách đi theo nhóm")]
        [StringLength(15, ErrorMessage = "Số điện thoại khách đi theo nhóm vượt quá 15 ký tự")]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string CodeShowUpWithGroup { get; set; }
    }

    public class SearchShowUpHistoryModel
    {
        public string ShowUp { get; set; }

        public string Product { get; set; }

        public string InvestorResource { get; set; }

        public string SaleTO { get; set; }

        public string Sale { get; set; }

        public string TeleSale { get; set; }

        public string CheckIn { get; set; }

        public DateTime? CheckInDate { get; set; }

        public string CheckOut { get; set; }

        public DateTime? CheckOutDate { get; set; }

        public string Key { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }

        public string Branch { get; set; }

        public string CreatedBy { get; set; }
    }

    public class StatisticShowUpViewModel
    {
        public int CheckedInPersonNumber { get; set; }

        //public int GoWithPersonNumber { get; set; }

        public int ExpectedCheckinPersonNumber { get; set; }

        public float CheckedinPercent { get; set; }
    }
}
