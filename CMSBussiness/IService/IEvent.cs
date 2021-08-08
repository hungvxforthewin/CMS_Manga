using CRMBussiness.ViewModel;
using System;

namespace CRMBussiness.IService
{
    public interface IEvent
    {
        DataResult<EventViewModel> GetList(SearchEventModel model, out int total);

        DataResult<EventViewModel> Create(EventViewModel model);

        DataResult<EventViewModel> Update(EventViewModel model);

        DataResult<EventViewModel> Delete(long id);

        DataResult<EventViewModel> Get(long id);

        DataResult<EventViewModel> GetByShowUpCode(string code);

        DataResult<EventViewModel> GetEventList(string branch);

        DataResult<EventViewModel> GetByName(string name);

        DataResult<EventViewModel> GetNearestEvent(bool? forSale, string branch);

        DataResult<EventViewModel> GetEventByProductCode(string ProductCode);


        string GetEventCode(bool isForCC, DateTime date);

    }
}
