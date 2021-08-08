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
    public class DepartmentImp : BaseService<Department, long>, IDepartment
    {
        public DataResult<DepartmentViewModel> CheckByOfficeAndBranch(string departName, string officeCode, string branchCode)
        {
            try
            {
                DepartmentViewModel data = new DepartmentViewModel();
                data = this.Raw_Query<DepartmentViewModel>("SELECT DepartmentName FROM tblDepartMent WHERE DepartmentName = @DepartmentName AND OfficeCode = @OfficeCode AND BranchCode = @BranchCode", new Dictionary<string, object>() {
                    {"DepartmentName", departName.ToUpper() },
                    {"OfficeCode", officeCode },
                    {"BranchCode", branchCode }
                }).FirstOrDefault();
                return new DataResult<DepartmentViewModel>() { DataItem = data ?? new DepartmentViewModel(), Error = false };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataResult<DepartmentViewModel> GetByCode(string code)
        {
            try
            {
                DepartmentViewModel data = new DepartmentViewModel();
                data = this.Raw_Query<DepartmentViewModel>("SELECT DepartmentName FROM tblDepartMent WHERE DepartmentCode = @DepartmentCode", new Dictionary<string, object>() {
                    {"DepartmentCode", code }
                }).FirstOrDefault();
                return new DataResult<DepartmentViewModel>() { DataItem = data, Error = false };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataResult<DepartmentViewModel> GetById(int id)
        {
            try
            {
                var data = new Department();
                data = this.Raw_Get(id);
                var result = new List<DepartmentViewModel>()
                {
                    new DepartmentViewModel()
                {
                    Id = data.Id,
                    OfficeCode = data.OfficeCode,
                    DepartmentCode = data.DepartmentCode,
                    BranchCode = data.BranchCode,
                    DepartmentName = data.DepartmentName,
                    Status = data.Status,
                    CodeStaffSaleManage = data.CodeStaffSaleManage
                }
                };
                return new DataResult<DepartmentViewModel>() { Result = result };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataResult<DepartmentViewModel> GetDepartmentList(string branch, string key, int start = 1, int size = 10, int pages = 5)
        {
            throw new NotImplementedException();
        }

        #region GetDepartmentsInOffice
        /// <summary>
        /// GET DEPARTMENT BY OFFICE
        /// </summary>
        /// <param name="office"></param>
        /// <returns></returns>
        public DataResult<DepartmentViewModel> GetDepartmentsInOffice(string office)
        {
            List<DepartmentViewModel> data = new List<DepartmentViewModel>();
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("sp_tblDepartment_GetDeptInBranch",
                        new { @office = office }, commandType: CommandType.StoredProcedure))
                    {
                        data = multipleresult.Read<DepartmentViewModel>().ToList();
                    }
                }

                return new DataResult<DepartmentViewModel> { Error = false, Result = data };
            }
            catch
            {
                return new DataResult<DepartmentViewModel> { Error = true };
            }
        }
        #endregion

        #region GetDepartmentsInBranch
        /// <summary>
        /// GET DEPARTMENT BY Branch
        /// </summary>
        /// <param name="branch">branch</param>
        /// <returns></returns>
        public DataResult<DepartmentViewModel> GetDepartmentListInBranch(string branch)
        {
            List<DepartmentViewModel> data = new List<DepartmentViewModel>();
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("sp_tblDepartment_GetDepartmentListInBranch",
                        new { @branch = branch }, commandType: CommandType.StoredProcedure))
                    {
                        data = multipleresult.Read<DepartmentViewModel>().ToList();
                    }
                }

                return new DataResult<DepartmentViewModel> { Result = data };
            }
            catch
            {
                return new DataResult<DepartmentViewModel> { Error = true };
            }
        }

        public DataResult<DepartmentViewModel> GetList(SearchDepartmentViewModel model, out int total)
        {
            List<DepartmentViewModel> data = new List<DepartmentViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", model.Key ?? string.Empty);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@Status", model.Status);
            param.Add("@BranchCode", model.BranchCode);
            param.Add("@OfficeCode", model.OfficeCode);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    data = this.Procedure<DepartmentViewModel>("sp_tblDepartment_GetList", param).ToList();
                    total = param.Get<int>("Total");
                }
                return new DataResult<DepartmentViewModel> { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<DepartmentViewModel> { Error = true };
            }
        }
        #endregion
    }
}
