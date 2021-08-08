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
    public class OfficeImp : BaseService<Office, int>, IOffice
    {
        public DataResult<OfficeViewModel> CheckOffficeInBranch(string branch, string nameOffice)
        {
            try
            {
                OfficeViewModel data = new OfficeViewModel();
                data = this.Raw_Query<OfficeViewModel>("SELECT OfficeName FROM tblOffice WHERE BranchCode = @BranchCode AND UPPER(OfficeName) = @OfficeName", new Dictionary<string, object>() {
                    {"BranchCode", branch },
                    {"OfficeName", nameOffice.ToUpper() }
                }).FirstOrDefault();
                return new DataResult<OfficeViewModel>() { DataItem = data ?? new OfficeViewModel(), Error = false };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataResult<OfficeViewModel> GetByCode(string code)
        {
            try
            {
                OfficeViewModel data = new OfficeViewModel();
                data = this.Raw_Query<OfficeViewModel>("SELECT OfficeName FROM tblOffice WHERE OfficeCode = @OfficeCode", new Dictionary<string, object>() {
                    {"OfficeCode", code }
                }).FirstOrDefault();
                return new DataResult<OfficeViewModel>() { DataItem = data, Error = false };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataResult<OfficeViewModel> GetById(int id)
        {
            try
            {
                var data = new Office();
                data = this.Raw_Get(id);
                var result = new List<OfficeViewModel>()
                {
                    new OfficeViewModel()
                {
                    Id = data.Id,
                    OfficeCode = data.OfficeCode,
                    OfficeName = data.OfficeName,
                    BranchCode = data.BranchCode,
                    Status = data.Status,
                    CodeStaffOffice = data.CodeStaffOffice
                }
                };
                return new DataResult<OfficeViewModel>() { Result = result };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataResult<OfficeViewModel> GetList(SerachOfficeViewModel model, out int total)
        {
            List<OfficeViewModel> data = new List<OfficeViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", model.Key ?? string.Empty);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@Status", model.Status);
            param.Add("@BranchCode", model.BranchCode);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    data = this.Procedure<OfficeViewModel>("sp_tblOffice_GetList", param).ToList();
                    total = param.Get<int>("Total");
                }
                return new DataResult<OfficeViewModel> { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<OfficeViewModel> { Error = true };
            }
        }

        public DataResult<OfficeViewModel> GetOfficeList(string branch, string key, int start = 1, int size = 10, int pages = 5)
        {
            throw new NotImplementedException();
        }

        public DataResult<OfficeViewModel> GetOfficesInBranch(string branch)
        {
            List<OfficeViewModel> data = new List<OfficeViewModel>();
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("sp_tblOffice_GetOfficeInBranch",
                        new { @branch = branch }, commandType: CommandType.StoredProcedure))
                    {
                        data = multipleresult.Read<OfficeViewModel>().ToList();
                    }
                }

                return new DataResult<OfficeViewModel> { Error = false, Result = data };
            }
            catch
            {
                return new DataResult<OfficeViewModel> { Error = true };
            }
        }
    }
}
