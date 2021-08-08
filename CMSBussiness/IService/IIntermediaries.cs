using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using CRMModel.Models.Data;
using System.Collections.Generic;

namespace CRMBussiness.IService
{
    public interface IIntermediaries : IBaseServices<Intermediaries, int>
    {
        DataResult<IntermediariesViewModel> CheckByPhone(string phone);
        DataResult<IntermediariesViewModel> CheckByTaxCode(string taxCode);
    }
}
