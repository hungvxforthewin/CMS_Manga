using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface IOffice : IBaseServices<Office, long>
    {
        DataResult<OfficeViewModel> GetOfficeList(string branch, string key, int start = 1, int size = 10, int pages = 5);
        DataResult<OfficeViewModel> GetOfficesInBranch(string branch);
        DataResult<OfficeViewModel> GetList(SerachOfficeViewModel model, out int total);
        DataResult<OfficeViewModel> GetById(int id);
        DataResult<OfficeViewModel> GetByCode(string code);
        DataResult<OfficeViewModel> CheckOffficeInBranch(string branch, string nameOffice);
    }
}
