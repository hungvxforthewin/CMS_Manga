using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface IContractInvester : IBaseServices<ContractInvestor, int>
    {
        DataResult<DisplayContractInvesterTableViewModel> GetList(SearchContractInvesterViewModel model, out int total);
        DataResult<DisplayContractInvesterTableViewModel> GetListByWaitPayDone(SearchContractInvestorInstallmentsViewModel model, out int total);
        DataResult<ContractInvesterViewModel> GetInfoById(int id);
        ContractInvesterViewModel GetInfoByCodeContract(string code);
        DataResult<InforBill> GetListBill(int id);
        ContractInvestor GetByCode(string CodeContract);
        bool UpdatePayDone(int id, DateTime? approvedDate);
        bool UpdateNewCodeContract(int id, string newCodeContract);
        bool UpdateDepositCode(int id, string depositCode);
        int CountDateContract(string CodeContract);
        int CountDateContractForAmber(string CodeContract, string Code);
        int CountInntermediariesContract(string CodeIntermediaries);
        bool UpdateSignature(int id, string signaturePath);
    }
}
