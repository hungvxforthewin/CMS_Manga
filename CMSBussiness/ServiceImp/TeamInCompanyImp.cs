using System;
using System.Linq;
using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using CRMBussiness.IService;
using CRMBussiness.LIB;
using CRMModel.Models.Data;
using CRMBussiness.ViewModel;

namespace CRMBussiness.ServiceImp
{
    public class TeamInCompanyImp : BaseService<TeamInCompany, long>, ITeamInCompany
    {
        #region GetAllTeamsInDepartment
        public DataResult<TeamInCompanyViewModel> GetAllTeamsInDepartment(string department)
        {
            List<TeamInCompanyViewModel> data = new List<TeamInCompanyViewModel>();
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("sp_tblTeamInCompany_GetTeamsInDepartment",
                        new { @department = department }, commandType: CommandType.StoredProcedure))
                    {
                        data = multipleresult.Read<TeamInCompanyViewModel>().ToList();
                    }
                }

                return new DataResult<TeamInCompanyViewModel> { Error = false, Result = data };
            }
            catch
            {
                return new DataResult<TeamInCompanyViewModel> { Error = true };
            }
        }
        #endregion

        #region GetById
        public DataResult<TeamInCompanyViewModel> GetById(int id)
        {
            try
            {
                var data = new List<TeamInCompanyViewModel>();
                DynamicParameters p = new DynamicParameters();
                p.Add("@Id", id);

                data = this.Procedure<TeamInCompanyViewModel>("sp_tblTeam_GetById", p).ToList();
                return new DataResult<TeamInCompanyViewModel>() { Result = data };
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region GetList
        public DataResult<TeamInCompanyViewModel> GetList(SearchTeamViewModel model, out int total)
        {
            List<TeamInCompanyViewModel> data = new List<TeamInCompanyViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", model.Key ?? string.Empty);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@Status", model.Status);
            param.Add("@BranchCode", model.BranchCode);
            param.Add("@OfficeCode", model.OfficeCode);
            param.Add("@DepartmentCode", model.DepartmentCode);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    data = this.Procedure<TeamInCompanyViewModel>("sp_tblTeam_GetList", param).ToList();
                    total = param.Get<int>("Total");
                }
                return new DataResult<TeamInCompanyViewModel> { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<TeamInCompanyViewModel> { Error = true };
            }
        }
        #endregion

        public DataResult<TeamInCompanyViewModel> GetTeamList(string branch, string department, string key, int start = 1, int size = 10, int pages = 5)
        {
            throw new NotImplementedException();
        }

        public DataResult<TeamInCompanyViewModel> GetByCode(string code)
        {
            try
            {
                TeamInCompanyViewModel data = new TeamInCompanyViewModel();
                data = this.Raw_Query<TeamInCompanyViewModel>("SELECT Name FROM tblTeamInCompany WHERE TeamCode = @TeamCode", new Dictionary<string, object>() {
                    {"TeamCode", code }
                }).FirstOrDefault();
                return new DataResult<TeamInCompanyViewModel>() { DataItem = data };
            }
            catch (Exception ex)
            {
                return new DataResult<TeamInCompanyViewModel>() { Error = true };
            }
        }
    }
}