using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class TargetModel
    {
        public long Id { get; set; }

        public decimal Revenue { get; set; }

        //public short ShowUp { get; set; }
        
        //public bool Status { get; set; }

        public DateTime StartDate { get; set; }
    }

    public class RevenueTarget
    {
        [StringLength(50)]
        public string SetTargetFor { get; set; }

        public string Name { get; set; }

        public decimal Revenue { get; set; }
    }

    public class SaleTargetViewModel
    {
        public byte Role { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public List<RevenueTarget> Targets { get; set; }
    }
}
