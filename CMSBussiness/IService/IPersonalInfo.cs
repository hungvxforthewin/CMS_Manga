using CRMBussiness.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMBussiness.IService
{
    public interface IPersonalInfo
    {
        DataResult<PersonalInfoViewModel> GetInfoById(long id);

        DataResult<DisplayPersonalTableViewModel> GetPersonalInformationList(SearchPersonalInfoModel model, out int total);
    }
}
