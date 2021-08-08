using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface IEarning
    {
        DataResult<EarningViewModel> GetSalaries(string month);

        DataResult<EarningViewModel> GetTemporarySalary(string month, string codeStaff);

        DataResult<EarningViewModel> GetSalaries(SearchSalaryModel model, out int total);
    }
}
