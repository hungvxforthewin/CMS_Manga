using CRMBussiness.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMBussiness.IService
{
    public interface ITypeContract
    {
        DataResult<TypeContractViewModel> GetAll();

        DataResult<TypeContractViewModel> GetList(SearchTypeContractModel model, out int total);

        DataResult<TypeContractViewModel> CreateNewContractType(TypeContractViewModel model);

        DataResult<TypeContractViewModel> GetById(int id);

        DataResult<TypeContractViewModel> Update(TypeContractViewModel model);

        DataResult<TypeContractViewModel> Delete(int id);
    }
}
