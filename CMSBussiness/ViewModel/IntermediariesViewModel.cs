using System;
using System.Collections.Generic;
using System.Text;

namespace CRMBussiness.ViewModel
{
    public class IntermediariesViewModel
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public string UserCreate { get; set; }
        public string UserUpdate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CodeIntermediaries { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string TaxCode { get; set; }
        public bool Status { get; set; }
    }
}
