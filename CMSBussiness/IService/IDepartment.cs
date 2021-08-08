using CRMModel.Models.Data;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface IDepartment : IBaseServices<Department, long>
    {
        DataResult<DepartmentViewModel> GetDepartmentList(string branch, string key, int start = 1, int size = 10, int pages = 5);
        DataResult<DepartmentViewModel> GetDepartmentsInOffice(string office); 
        DataResult<DepartmentViewModel> GetDepartmentListInBranch(string branch); 
        DataResult<DepartmentViewModel> GetList(SearchDepartmentViewModel model, out int total);
        DataResult<DepartmentViewModel> GetById(int id);
        DataResult<DepartmentViewModel> GetByCode(string code);
        DataResult<DepartmentViewModel> CheckByOfficeAndBranch(string departName, string officeCode, string branchCode);
    }
}
