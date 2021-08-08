using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using Kpi = CRMModel.Models.Data.Kpi;

namespace CRMBussiness.IService
{
    public interface IKpiSaleAdmin : IBaseServices<Kpi, long>
    {
        List<RevenuePercentSaleAdmin> RevenuePercentSaleAdminsWithRole(byte roleAccount);
    }
    public class RevenuePercentSaleAdmin
    {
        public decimal Percent { get; set; }
        public decimal Revenue { get; set; }
        public string CodeKpi { get; set; }
        public string CodeRemuneration { get; set; }
        public long KpiId { get; set; }
        public byte RemunerationId { get; set; }
        public string MinRevenueBranch { get; set; }
        public string MaxRevenueBranch { get; set; }
    }
}
