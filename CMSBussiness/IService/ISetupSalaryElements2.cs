using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface ISetupSalaryElements2
    {
        DataResult<SalaryMechanismViewModel> GetSetupSalaryForTele();

        DataResult<SalaryMechanismViewModel> GetSetupSalaryForSale();

        DataResult<SalaryMechanismViewModel> GetSetupSalaryForMinister();

        DataResult<SalaryMechanismViewModel> SetupSalaryElementsForTele(SalaryMechanismViewModel model);

        DataResult<SalaryMechanismViewModel> SetupSalaryElementsForSale(SalaryMechanismViewModel model);

        DataResult<SalaryMechanismViewModel> SetupSalaryElementsForMinister(SalaryMechanismViewModel model);
    }
}
