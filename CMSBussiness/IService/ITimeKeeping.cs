using CRMBussiness.ViewModel;
using System.Collections.Generic;

namespace CRMBussiness.IService
{
    public interface ITimeKeeping
    {
        DataResult<DisplayTimeKeepingViewmodel> GetInfoByMonthAndKey(string key, string month);

        DataResult<TimeKeepingViewModel> AddTimeKeeping(List<TimeKeepingViewModel> models);

        DataResult<TimeKeepingViewModel> AddNewTimeKeeping(TimeKeepingViewModel model);

        DataResult<TimeKeepingViewModel> UpdateTimeKeeping(TimeKeepingViewModel model);

        DataResult<TimeKeepingViewModel> GetTimeKeepingOfEmployee(long id);

        DataResult<TimeKeepingViewModel> UpdateTimeKeepingOfEmployee(TimeKeepingViewModel model);

        DataResult<bool> CheckExistedTimekeepingInMonth(string month, string staffCode);

        DataResult<TimeKeepingViewModel> UpdateOtherBonus(decimal? bonus, long id);

        DataResult<TimeKeepingViewModel> ImportTimeKeeping(List<TimeKeepingViewModel> models);
    }
}
