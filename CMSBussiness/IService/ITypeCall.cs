using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface ITypeCall
    {
        DataResult<TypeCallViewModel> GetList(string key, int page, int size, out int total);

        DataResult<TypeCallViewModel> CreateNewCallType(TypeCallViewModel model);

        DataResult<TypeCallViewModel> GetById(int id);

        DataResult<TypeCallViewModel> Update(TypeCallViewModel model);

        DataResult<TypeCallViewModel> Delete(int id);
    }
}
