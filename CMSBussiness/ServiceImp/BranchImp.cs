using System;
using System.Linq;
using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using CRMBussiness.IService;
using CRMBussiness.LIB;
using CRMModel.Models.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CRMBussiness.ViewModel;
using System.Data.SqlClient;

namespace CRMBussiness.ServiceImp
{
    public class BranchImp : BaseService<Branch, int>, IBranch
    {
        public bool DeleteAll()
        {
            try
            {
                int id = 0;
                this.Raw_Query<BranchViewModel>("UPDATE tblBranch SET Status = 0", new Dictionary<string, object>() {
                    {"Id", id }
                }).SingleOrDefault();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool DeleteById(int id)
        {
            try
            {
                this.Raw_Query<BranchViewModel>("UPDATE tblBranch SET Status = 0 WHERE Id = @Id", new Dictionary<string, object>() {
                    {"Id", id }
                }).SingleOrDefault();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #region GetAllBranches
        public DataResult<BranchViewModel> GetAllBranches()
        {
            List<BranchViewModel> data = new List<BranchViewModel>();
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("sp_tblBranch_GetAll",
                        new { }, commandType: CommandType.StoredProcedure))
                    {
                        data = multipleresult.Read<BranchViewModel>().ToList();
                    }
                }

                return new DataResult<BranchViewModel> { Error = false, Result = data };
            }
            catch
            {
                return new DataResult<BranchViewModel> { Error = true };
            }
        }
        #endregion

        public DataResult<BranchViewModel> GetBranchList(string key, int start = 1, int size = 10, int pages = 5)
        {
            throw new NotImplementedException();
        }

        public DataResult<BranchViewModel> GetByCode(string code)
        {
            try
            {
                BranchViewModel data = new BranchViewModel();
                data = this.Raw_Query<BranchViewModel>("SELECT BranchName FROM tblBranch WHERE BranchCode = @BranchCode", new Dictionary<string, object>() {
                    {"BranchCode", code }
                }).FirstOrDefault();
                return new DataResult<BranchViewModel>() { DataItem = data ?? new BranchViewModel(), Error = false };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataResult<BranchViewModel> GetById(int id)
        {
            try
            {
                var data = new Branch();
                data = this.Raw_Get(id);
                var result = new List<BranchViewModel>()
                {
                    new BranchViewModel()
                {
                    Id = data.Id,
                    Address = data.Address,
                    BranchCode = data.BranchCode,
                    BranchName = data.BranchName,
                    Status = data.Status.ToString(),
                    CodeStaffAdminSale = data.CodeStaffAdminSale
                }
                };
                return new DataResult<BranchViewModel>() { Result = result };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataResult<BranchViewModel> GetByName(string name)
        {
            try
            {
                BranchViewModel data = new BranchViewModel();
                data = this.Raw_Query<BranchViewModel>("SELECT BranchName FROM tblBranch WHERE UPPER(BranchName) = @BranchName", new Dictionary<string, object>() {
                    {"BranchName", name.ToUpper() }
                }).FirstOrDefault();              
                return new DataResult<BranchViewModel>() { DataItem = data ?? new BranchViewModel(), Error = false };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataResult<BranchViewModel> GetLastBranch()
        {
            try
            {
                BranchViewModel data = new BranchViewModel();
                data = this.Raw_Query<BranchViewModel>("SELECT TOP 1 BranchCode FROM tblBranch ORDER BY Id DESC").FirstOrDefault();
                return new DataResult<BranchViewModel>() { DataItem = data, Error = false };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataResult<BranchViewModel> GetList(SearchBranchViewModel model, out int total)
        {
            List<BranchViewModel> data = new List<BranchViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", model.Key ?? string.Empty);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@Status", model.Status);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    data = this.Procedure<BranchViewModel>("sp_tblBranch_GetList", param).ToList();
                    total = param.Get<int>("Total");
                }
                return new DataResult<BranchViewModel> { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<BranchViewModel> { Error = true };
            }
        }
    }
}
