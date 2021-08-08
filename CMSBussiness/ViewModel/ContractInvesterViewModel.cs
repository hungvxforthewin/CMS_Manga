using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CRMBussiness.ViewModel
{
    public class ContractInvesterViewModel
    {
        public virtual int Id { get; set; } //ContractInvestor
        public virtual int IdInvestor { get; set; }
        public virtual int DepositId { get; set; }
        //Invester
        [Required]
        public virtual string Name { get; set; }

        public virtual string PrintName { get; set; }
        //[Required]
        public virtual string Email { get; set; }
        //[Required]
        public virtual string Birthday { get; set; }
        //[Required]
        public virtual string IdCard { get; set; }
        //[Required]
        public virtual string DateOfIssuance { get; set; }//NGAY CAP - NGAY PHAT HANH
        //[Required]
        public virtual string AddressIssuance { get; set; }
        public virtual string AddressIssuanceCC { get; set; }
        [Required]
        public virtual string Phone { get; set; }
        //[Required]
        public virtual string Address { get; set; }
        //[Required]
        public virtual string AccountBank { get; set; }
        public virtual string AccountBank2 { get; set; }
        //[Required]
        public virtual string Bank { get; set; }
        //[Required]
        public virtual string EmailAccount { get; set; }
        public virtual string IsCMT { get; set; }// CMT OR CCCD
        //Deposit
        //[Required]
        public virtual string CodeDeposit { get; set; } //SỐ PHIẾU ĐẶT CỌC
        //[Required]
        public virtual decimal? DepositAmount { get; set; } = 0; //SỐ TIỀN ĐẶT CỌC --> TÔNG TIỀN CỌC
        //[Required]
        public virtual string DepositDate { get; set; } //NGÀY ĐẶT CỌC
        //[Required]
        public virtual int DepositForm { get; set; } //HÌNH THỨC ĐẶT CỌC
        //
        //[Required]
        //[StringLength(50)]
        public virtual string CodeContract { get; set; } //MÃ HỢP ĐỒNG
        [Required]
        public virtual decimal? InvestmentAmount { get; set; } //GIÁ TRỊ HĐ
        [Required]
        public virtual string CreateDate { get; set; } //NGÀY VÀO HĐ
        //THÔNG TIN THANH TOÁN
        public List<InforBill> ListInforBill { get; set; }
        public virtual float BillMoney { get; set; } //THANH TOÁN LẦN 1, SỐ TIỀN THANH TOÁN LẦN 1
        public virtual string DateBill { get; set; } //NGÀY THANH TOÁN LẦN 1
        public virtual int FormBill { get; set; } //HÌNH THỨC THANH TOÁN LẦN 1
        //NGƯỜI THỰC HIỆN
        //[StringLength(50)]
        //[Required]
        public virtual string TeleSale { get; set; }
        [Required]
        public virtual string Sale { get; set; }
        //[StringLength(50)]
        public virtual string SaleRep { get; set; } //SALE CHỐT HỘ
        [StringLength(100)]
        public virtual string NameContract { get; set; }
        public virtual string CodeInvestor { get; set; }

        /// <summary>
        /// Mặc định : đã duyệt.
        /// </summary>
        public virtual string IdStatusContract { get; set; }
        /// <summary>
        /// Mã số thuế cá nhân
        /// </summary>
        //[Required]
        public virtual string PersonalTaxCode { get; set; }
        /// <summary>
        /// SỐ CỔ PHẦN
        /// </summary>
        //[Required]
        public virtual long Stock { get; set; }
        /// <summary>
        /// NGÀY HOÀN THÀNH HỢP ĐỒNG-HĐ ĐƯỢC DUYỆT
        /// </summary>
        public DateTime? DatePaydone { get; set; }
        public virtual string CodeTransferAgreement { get; set; }
        public virtual string CodeManagementCatalog { get; set; }
        public virtual string CodeSupplementAgreement { get; set; }
        //khanhkk added
        public string InvestorSignature { get; set; }
        //CONTRACT EMBER
        public string IdIntermediaries { get; set; }
        public string CodeIntermediaries { get; set; }
        public string PhoneIntermediaries { get; set; }
        public string TaxCodeIntermediaries { get; set; }
        public string NameIntermediaries { get; set; }
        public string AddressIntermediaries { get; set; }
        
    }
    public class InforBill
    {
        public virtual int IdBill { get; set; }
        //[Required]
        public virtual decimal BillMoney { get; set; } //THANH TOÁN LẦN 1, SỐ TIỀN THANH TOÁN LẦN 1
        //[Required]
        public virtual string DateBill { get; set; } //NGÀY THANH TOÁN LẦN 1
        //[Required]
        public virtual int FormBill { get; set; } //HÌNH THỨC THANH TOÁN LẦN 1
    }
    public class ContractStaffStatus
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
    public class SearchContractInvesterViewModel
    {
        public string Key { get; set; }

        public string Resource { get; set; }

        public string Sale { get; set; }

        public string TeleSale { get; set; }

        public string Status { get; set; }
        public string BranchCode { get; set; }

        public int Size { get; set; } = 10;

        public int Page { get; set; } = 1;
    }
    public class DisplayContractInvesterTableViewModel
    {
        public virtual int Id { get; set; }

        public virtual string ContractCode { get; set; }

        public virtual string NameInvestor { get; set; }

        public virtual string CMT { get; set; }
    
        public virtual string Phone { get; set; }

        public virtual string SaleName { get; set; }

        public virtual string TeleSaleName { get; set; }

        public virtual byte Status { get; set; }

        public virtual string IdStatusContract { get; set; }

        public virtual string NameStatus { get; set; }

        public virtual decimal InvestmentAmount { get; set; }

        public virtual decimal Stock { get; set; }

        public virtual string CodeIntermediaries { get; set; }
        //CODE CONTRACT FOR AMBER
        public virtual string CodeTransferAgreement { get; set; }
        /// <summary>
        /// HĐ QUẢN LÝ DANH MỤC ĐẦU TƯ
        /// </summary>
        public virtual string CodeManagementCatalog { get; set; }
        /// <summary>
        /// HĐ THỎA THUẬN BỔ SUNG
        /// </summary>
        public virtual string CodeSupplementAgreement { get; set; }
    }
    public class PaymentViewModel
    {
        public PaymentViewModel()
        {
            ListBills = new List<ListBill>();
        }
        /// <summary>
        /// GIÁ TRỊ CỦA HĐ
        /// </summary>
        public decimal AmountContract { get; set; }
        /// <summary>
        /// SỐ TIỀN CÒN LẠI TRONG HĐ
        /// </summary>
        public decimal RemainAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal BillDeposit { get; set; }
        /// <summary>
        /// TÊN KHÁCH HÀNG
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// SĐT 
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// CMT
        /// </summary>
        public string CMT { get; set; }
        /// <summary>
        /// NGÀY DUYỆT HĐ
        /// </summary>
        public DateTime? DatePaydone { get; set; }
        public string DatePaydoneString { get; set; }

        //CODE CONTRACT FOR AMBER
        public virtual string CodeTransferAgreement { get; set; }
        /// <summary>
        /// HĐ QUẢN LÝ DANH MỤC ĐẦU TƯ
        /// </summary>
        public virtual string CodeManagementCatalog { get; set; }
        /// <summary>
        /// HĐ THỎA THUẬN BỔ SUNG
        /// </summary>
        public virtual string CodeSupplementAgreement { get; set; }

        public List<ListBill> ListBills { get; set; }
    }
    public class ListBill
    {
        /// <summary>
        /// Số tiền mỗi lần thanh toán
        /// </summary>
        public decimal BillMoney { get; set; }
    }
    public class ContractInvestorPrintViewModel : ContractInvesterViewModel
    {
        public ContractInvestorPrintViewModel()
        {
            LstPayment = new List<ListPayment>();
        }
        public string DateOfIssuancePrint { get; set; }
        public string AddressIssuancePrint { get; set; }
        public string AddressPrint { get; set; }
        public string BankPrint { get; set; }
        public string InvestmentAmountPrint { get; set; }
        public string InvestmentAmountPrintEN { get; set; }
        public int CountShare { get; set; }
        public string CountShareVN { get; set; }
        public string CountShareEN { get; set; }
        public decimal AmountDeposit { get; set; }
        public decimal PercenDeposit { get; set; }
        public decimal RemainAmount { get; set; }
        public string RemainAmountVN { get; set; }
        public string RemainAmountEN { get; set; }
        public List<ListPayment> LstPayment { get; set; }

        public DateTime CreatedDateValue { get; set; }
    }
    // CÁC ĐỢT THANH TOÁN
    public class ListPayment
    {
        /// <summary>
        /// SỐ NGÀY KÝ KẾT HĐ ĐẾN NGÀY THANH TOÁN ĐỢT N
        /// </summary>
        public int day { get; set; }
        /// <summary>
        /// THỜI GIAN THANH TOÁN ĐỢT N
        /// </summary>
        public DateTime? BillTwo { get; set; }
        public decimal PriceBill { get; set; }
        public decimal PricePercent { get; set; }
        public string PriceBillVN { get; set; }
        public string PriceBillEN { get; set; }
    }
}
