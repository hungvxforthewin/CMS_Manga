using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface IBranch : IBaseServices<Branch, long>
    {
        DataResult<BranchViewModel> GetBranchList(string key, int start = 1, int size = 10, int pages = 5);
        DataResult<BranchViewModel> GetAllBranches();
        DataResult<BranchViewModel> GetLastBranch();
        DataResult<BranchViewModel> GetList(SearchBranchViewModel model, out int total);
        DataResult<BranchViewModel> GetById(int id);
        DataResult<BranchViewModel> GetByCode(string code);
        DataResult<BranchViewModel> GetByName(string name);
        bool DeleteById(int id);
        bool DeleteAll();

    }
}
