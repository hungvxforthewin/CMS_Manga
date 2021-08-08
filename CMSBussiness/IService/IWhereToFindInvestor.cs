using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface IWhereToFindInvestor
    {
        DataResult<WhereToFindInvestorViewModel> GetList(string key, int page, int size, out int total);

        DataResult<WhereToFindInvestorViewModel> CreateNewResource(WhereToFindInvestorViewModel model);

        DataResult<WhereToFindInvestorViewModel> GetById(int id);

        DataResult<WhereToFindInvestorViewModel> Update(WhereToFindInvestorViewModel model);

        DataResult<WhereToFindInvestorViewModel> Delete(int id);

        DataResult<WhereToFindInvestorViewModel> GetInvestResourceList();

        DataResult<WhereToFindInvestorViewModel> GetByName(string name);
    }
}
