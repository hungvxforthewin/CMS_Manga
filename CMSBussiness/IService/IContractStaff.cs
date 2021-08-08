using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface IContractStaff : IBaseServices<ContractStaff, long>
    {
        //DataResult<DisplayContractStaffTableViewModel> GetList(SearchContractStaffViewModel model, out int total);

        DataResult<DisplayPersonalTableViewModel> GetShareList(string key, out int total, int page = 1, int size = 10);

        DataResult<DisplayPersonalTableViewModel> DeleteAnEmployee(string staffCode);
    }
}
