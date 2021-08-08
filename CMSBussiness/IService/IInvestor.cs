using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using CRMModel.Models.Data;
using System.Collections.Generic;

namespace CRMBussiness.IService
{
    public interface IInvestor : IBaseServices<Investor, long>
    {
        List<Investor> GetDatas(BootstrapTableParam obj, ref int totalRow, int status);
        DataResult<GoWithInvestorModel> GetGoWithPersons(string checkinCode);
        InvestorViewModel GetByPhone(string phone);
        InvestorViewModel CheckSumAmountDeposit(string phone);
        InvestorViewModel CheckSumAmountDepositWithContract(string phone, string codeContract);
        bool UpdateByPhone(string phone, string name);
        bool UpdateAccountBank2(string bank, string investorCode);
    }
}
