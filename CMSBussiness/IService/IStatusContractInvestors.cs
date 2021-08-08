using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using CRMModel.Models.Data;
using System.Collections.Generic;

namespace CRMBussiness.IService
{
    public interface IStatusContractInvestors : IBaseServices<StatusContractInvestor, string>
    {
        StatusContractInvestor InsertData(StatusContractInvestor model, ref string statusContractId);
    }
}
