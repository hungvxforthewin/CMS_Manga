using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface IContractInvestorInstallments : IBaseServices<ContractInvestorInstallments, int>
    {
        ContractInvestorInstallments GetLastStatus(string CodeContract);
        bool UpdatePayDone(string CodeContract);
        bool DeleteByContract(string CodeContract);
        bool DeleteByCodeDeposit(string CodeDeposit);
        bool UpdateCodeContract(string CodeDeposit, string CodeContract);
        bool UpdateNewCodeContract(string CodeContract, string NewCodeContract);

    }
}
