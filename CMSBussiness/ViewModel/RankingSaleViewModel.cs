using System;
using System.Collections.Generic;
using System.Text;

namespace CRMBussiness.ViewModel
{
    public class RankingSaleViewModel
    {
        public int Week { get; set; }
        public int Month { get; set; }
        public string Timmer { get; set; }
        public List<SaleTop10> LstSaleTop10ByDay { get; set; }
        public List<SaleTop10> LstSaleTop10ByWeek { get; set; }
        public List<SaleTop10> LstSaleTop10ByMonth { get; set; }
    }
}
