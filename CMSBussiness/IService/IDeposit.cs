using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface IDeposit : IBaseServices<Deposit, int>
    {
        Deposit GetByCode(string codeDeposit);
        DataResult<DepositAgreementTableViewModel> GetList(SearchDepositAAgreementViewModel model, out int total);
        DataResult<DepositAgreementViewModel> GetInfoById(int id);
        DataResult<InforDepositBill> GetListBill(int id);
    }
}
