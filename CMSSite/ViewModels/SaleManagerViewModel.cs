using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRMSite.ViewModels
{
    public class SaleManagerViewModel
    {
        public SaleManagerViewModel()
        {
            SaleManagerRemunerations = new List<SaleManagerRemuneration>();
            SaleManagerKpi6Firsts = new List<SaleManagerKpi6First>();
            SaleManagerKpi6Lasts = new List<SaleManagerKpi6Last>();
        }
        public decimal PercentKpiRoot6F { get; set; }
        public decimal SalaryPercentRoot6F { get; set; }
        public decimal PercentKpiRoot6L { get; set; }
        public decimal SalaryPercentRoot6L { get; set; }
        public List<SaleManagerRemuneration> SaleManagerRemunerations { get; set; }
        public List<SaleManagerKpi6First> SaleManagerKpi6Firsts { get; set; }
        public List<SaleManagerKpi6Last> SaleManagerKpi6Lasts { get; set; }
    }
    public class SaleManagerRemuneration
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Hoa hồng không được trống")]
        public decimal Percent { get; set; }
        //khanhkk added
        public decimal SharePercent { get; set; }
        //khanhkk added
        [Required(ErrorMessage = "Lương cứng không được trống")]
        public decimal Salary { get; set; }
        public string AmountMinMaxInMonth { get; set; }
        //public decimal AmountMaxInMonth { get; set; }
        public string MinMaxRevenueSM { get; set; }
        //public decimal MaxRevenueSM { get; set; }
        public short RemunerationId { get; set; }
        public string CodeRemuneration { get; set; }
    }
    public class SaleManagerKpi6First
    {
        public decimal PercentKpiMin6F { get; set; }
        public decimal PercentKpiMax6F { get; set; }
        public decimal SalaryPercentLv16F { get; set; }
    }
    public class SaleManagerKpi6Last
    {
        public decimal PercentKpiMin6L { get; set; }
        public decimal PercentKpiMax6L { get; set; }
        public decimal SalaryPercentLv16L { get; set; }
    }
}
