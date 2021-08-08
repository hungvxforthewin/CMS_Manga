using System;
using System.Collections.Generic;
using System.Text;

namespace CRMBussiness.ViewModel
{
    public class OfficeViewModel
    {
        public virtual long Id { get; set; }
        public virtual string OfficeCode { get; set; }
        public virtual string OfficeName { get; set; }
        public virtual string BranchCode { get; set; }
        public virtual string BranchName { get; set; }
        public virtual string BranchStatus { get; set; }
        public virtual string CompanyCode { get; set; }
        public virtual string CodeStaffOffice { get; set; }
        public virtual string NameStaffOffice { get; set; }
        public virtual bool Status { get; set; }
        public virtual string StatusOfiice { get; set; }
    }
    public class SerachOfficeViewModel
    {
        public string Key { get; set; }
        public string Status { get; set; }
        public string BranchCode { get; set; }
        public int Size { get; set; } = 10;
        public int Page { get; set; } = 1;
    }
}
