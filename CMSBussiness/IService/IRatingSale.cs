using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface IRatingSale : IBaseServices<RatingSale, int>
    {
        DataResult<DisplayRatingSaleTableViewModel> GetList(SearchRattingViewModel model, out int total);
        DataResult<RatingSaleViewModel> GetById(int id);
        DataResult<SaleChart> GetTop(string DateSale);
        DataResult<SaleTop10> GetTop10Day(DateTime today);
        DataResult<SaleTop10> GetTop10Week(DateTime today, out int week);
        DataResult<SaleTop10> GetTop10Month(DateTime today, out int month);
    }
}
