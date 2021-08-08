using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface ILevelConcern
    {
        DataResult<LevelConcernViewModel> GetList(string key, int page, int size, out int total);

        DataResult<LevelConcernViewModel> GetById(int id);

        DataResult<LevelConcernViewModel> CreateNewLevel(LevelConcernViewModel model);

        DataResult<LevelConcernViewModel> Update(LevelConcernViewModel model);

        DataResult<LevelConcernViewModel> Delete(int id);

        DataResult<LevelConcernViewModel> GetAll();
    }
}
