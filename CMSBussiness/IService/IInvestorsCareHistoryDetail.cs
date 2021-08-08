using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface IInvestorsCareHistoryDetail : IBaseServices<InvestorsCareHistoryDetail, int>
    {
        DataResult<DetailCallCareHistory> GetByHistoryCode(string code);
        bool DeleteByHistoryCode(string historyCode);
    }
}
