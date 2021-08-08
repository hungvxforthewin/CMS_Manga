using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface IInvestorsCareHistory : IBaseServices<InvestorsCareHistory, int>
    {
        DataResult<DisplayInvestorsCareHistoryViewModel> GetList(SearchInvestorsCareHistoryViewModel model, out int total);
        DataResult<InvestorsCareHistoryViewModel> GetById(int id);
    }
}
