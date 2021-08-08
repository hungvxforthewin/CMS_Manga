using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CRMBussiness.ViewModel
{
    public class InvestorsCareHistoryViewModel
    {
        public InvestorsCareHistoryViewModel()
        {
            detailCallCareHistories = new List<DetailCallCareHistory>();
        }
        public long Id { get; set; }

        [Required]
        public string NameInvestor { get; set; }
        [Required]
        public string PhoneInvestor { get; set; }

        
        public string HistoryCode { get; set; }

        [Required]
        public string ProductCode { get; set; }

        [Required]
        public string EventCode { get; set; }

        public int StatusCode { get; set; }

        [Required]
        public string LevelConcernCode { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedDateString { get; set; }

       
        public string CodeStaff { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedDateString { get; set; }

        //DETAIL CALL CARE HISTORY
        public List<DetailCallCareHistory> detailCallCareHistories { get; set; }
    }
    public class DetailCallCareHistory
    {
        public string HistoryCode { get; set; }
        public string SupportTime { get; set; }
        public string DateCallString { get; set; }
        public string Note { get; set; }
        public string StatusFollow { get; set; }
        public string StatusFollowName { get; set; }
    }
    public class TableInvestorsCareHistoryViewModel
    {

    }
    public class DisplayInvestorsCareHistoryViewModel
    {
        public int Id { get; set; }
        public string STT { get; set; }
        public string NameInvestor { get; set; }
        public string PhoneInvestor { get; set; }
        public string ProductName { get; set; }
        public string EventName { get; set; }
        public string StatusName { get; set; }
        public string LevelconcernName { get; set; }
        public string DateCreateString { get; set; }
    }
    public class SearchInvestorsCareHistoryViewModel
    {
        public string Key { get; set; }

        public int Size { get; set; } = 10;

        public int Page { get; set; } = 1;

        public string DateFrom { get; set; }
                                            
        public string DateTo { get; set; }

        public string ProductCode { get; set; }

        public string EventCode { get; set; }

        public string LevelconcernCode { get; set; }

        public string StatusCode { get; set; }
    }
    public class CareHistoryStatusViewModel
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}
