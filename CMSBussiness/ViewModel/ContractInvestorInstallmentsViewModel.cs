using System;
using System.Collections.Generic;
using System.Text;

namespace CRMBussiness.ViewModel
{
    public class ContractInvestorInstallmentsViewModel
    {

    }
    public class SearchContractInvestorInstallmentsViewModel
    {
        public string Date { get; set; }

        public string Key { get; set; }

        public string Sale { get; set; }

        public string TeleSale { get; set; }

        public string Status { get; set; }

        public string  Amber { get; set; }

        public int Size { get; set; } = 10;

        public int Page { get; set; } = 1;
    }
    public class DisplayContractInvestorInstallmentsTableViewModel
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
    }
}
