using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface ITeamInCompany : IBaseServices<TeamInCompany, long>
    {
        DataResult<TeamInCompanyViewModel> GetTeamList(string branch, string department, string key, int start = 1, int size = 10, int pages = 5);
        DataResult<TeamInCompanyViewModel> GetAllTeamsInDepartment(string department);
        DataResult<TeamInCompanyViewModel> GetList(SearchTeamViewModel model, out int total);
        DataResult<TeamInCompanyViewModel> GetById(int id);
        DataResult<TeamInCompanyViewModel> GetByCode(string code);
    }
}
