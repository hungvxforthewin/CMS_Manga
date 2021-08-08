using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CRMBussiness.ViewModel
{
    public class DepositAgreementViewModel
    {
        public virtual int Id { get; set; } //Deposit
        public virtual int IdInvestor { get; set; } //Investor
        public virtual string IdContractInvestor { get; set; } //ContractInvestor
        //Invester
        [Required]
        public virtual string Name { get; set; }
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
        [Required]
        public virtual string Phone { get; set; }
        //[Required]
        public virtual string Address { get; set; }
        //[Required]
        public virtual string AccountBank { get; set; }
        //[Required]
        public virtual string Bank { get; set; }
        //[Required]
        public virtual string EmailAccount { get; set; }
        // THÔNG TIN CÁC LẦN ĐẶT CỌC
        public List<InforDepositBill> ListInforDepositBill { get; set; }
        public string ContractCode { get; set; }
        public string InvestmentAmount { get; set; }
        public string InvestorName { get; set; }
        public string Sale { get; set; }
        public string LeaderSale { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        // CHECK
        public virtual decimal BillMoney { get; set; }
        public virtual string DateBill { get; set; }
    }
    //tblContractInvestorInstallments
    public class InforDepositBill
    {
        public virtual int IdBill { get; set; }
        //[Required]
        public virtual decimal BillMoney { get; set; } //ĐẶT CỌC LẦN 1, SỐ TIỀN ĐẶT LẦN 1
        //[Required]
        public virtual string DateBill { get; set; } //NGÀY ĐẶT CỌC LẦN 1
        //[Required]
        public virtual string DescriptionBill { get; set; } //GHI CHÚ ĐẶT CỌC
    }
    public class SearchDepositAAgreementViewModel
    {
        public string Key { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }

        public int Size { get; set; } = 10;

        public int Page { get; set; } = 1;
    }
    public class DepositAgreementTableViewModel
    {
        public int Id { get; set; }
        public string  ContractCode { get; set; }
        public string NameInvestor { get; set; }
        public string  CMT { get; set; }
        public string Phone { get; set; }
        public string CreateDate { get; set; }
        public string Description { get; set; }
        public string LeaderSale { get; set; }
        public string Status  { get; set; }
    }
}
