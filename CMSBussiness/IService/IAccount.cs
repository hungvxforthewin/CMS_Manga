using CRMModel.Models.Data;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface IAccount : IBaseServices<Account, long>
    {
        DataResult<AccountViewModel> GetList(SearchAccountViewModel model, out int total);
        DataResult<AccountViewModel> Login(string UserName);
        Account GetByCodeStaff(string codeStaff);
        Account GetAccountLast();
        bool InsertAccount(AccountViewModel model, out int statusInsert);
    }
}
