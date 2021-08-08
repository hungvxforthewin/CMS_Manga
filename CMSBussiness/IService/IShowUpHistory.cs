using CRMBussiness.ViewModel;
using System.Collections.Generic;

namespace CRMBussiness.IService
{
    public interface IShowUpHistory
    {
        DataResult<ShowUpHistoryTableViewModel> GetList(SearchShowUpHistoryModel model, out int total);

        DataResult<ShowUpHistoryTableViewModel> CheckIn(long id);

        DataResult<ShowUpHistoryTableViewModel> CheckOut(long id);

        DataResult<ShowUpHistoryTableViewModel> CreateCheckIn(ShowUpHistoryCreateViewModel model);

        DataResult<ShowUpHistoryTableViewModel> UpdateCheckIn(ShowUpHistoryCreateViewModel model);

        DataResult<ShowUpHistoryCreateViewModel> GetById(long id);

        DataResult<CheckinInfoModel> GetCheckinInfo(long id);

        //DataResult<InvestorViewModel> IsExistedInvestorInfo(string phoneNumber, long id);

        DataResult<StatisticShowUpViewModel> GetStaticalInfo(string branch);

        DataResult<CheckinInfoModel> GetInfoByPhoneNumber(string phoneNumber);

        DataResult<ShowUpHistoryCreateViewModel> ImportCheckin(List<ShowUpHistoryCreateViewModel> model);

        DataResult<ShowUpHistoryCreateViewModel> GetCheckinInByShowUpCode(string showUpCode);

        DataResult<ShowUpHistoryTableViewModel> RemoveCheckin(long id);

        DataResult<ShowUpHistoryTableViewModel> Delete(long id);
    }
}
