using CRMModel.Models.Data;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface IAccount : IBaseServices<Account, long>
    {
        DataResult<Account> GetAll();

        DataResult<AccountViewModel> Login(string UserName);

        Account GetByCodeStaff(string codeStaff);
        Account GetAccountLast();

        DataResult<EmployeeViewModel> GetEmployeeList();

        DataResult<EmployeeInfoModel> GetEmployeeInfoByStaffCode(string staffCode);
        DataResult<EmployeeInfoModel> GetEmployeeInfoByBranch(string branchCode);
        DataResult<EmployeeInfoModel> GetEmployeeInfoByOffice(string officeCode);
        DataResult<EmployeeInfoModel> GetEmployeeInfoByDepart(string departCode);
        DataResult<EmployeeInfoModel> GetEmployeeInfoByTeam(string teamCode);

        DataResult<EmployeeViewModel> GetEmployeeListByType(bool isTele);

        DataResult<EmployeeViewModel> GetEmployeeListByTypeAndName(bool isTele, string fullName);
        bool UpdateShareForSale();
    }
}
