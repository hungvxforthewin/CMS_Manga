using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface ISetupSalaryElements
    {
        DataResult<SalaryElementViewModel> GetSetupSalaryForTele();

        DataResult<SalaryElementViewModel> GetSetupSalaryForSale();

        DataResult<SalaryElementViewModel> SetupSalaryElementsForTele(SalaryElementViewModel model);

        DataResult<SalaryElementViewModel> SetupSalaryElementsForSale(SalaryElementViewModel model);

        DataResult<SalaryElementViewModel> UpdateSetupSalaryElements(SalaryElementViewModel model);
    }
}
