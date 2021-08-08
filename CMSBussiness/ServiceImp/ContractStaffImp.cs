using System.Linq;
using Dapper;
using System.Data;
using CRMBussiness.IService;
using CRMBussiness.LIB;
using CRMModel.Models.Data;
using CRMBussiness.ViewModel;
using System.Data.SqlClient;
using System;

namespace CRMBussiness.ServiceImp
{
    public class ContractStaffImp : BaseService<ContractStaff, long>, IContractStaff
    {
        #region GetShareList
        public DataResult<DisplayPersonalTableViewModel> GetShareList(string key, out int total, int page = 1, int size = 10)
        {
            total = 0;

            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", key ?? string.Empty);
            param.Add("@Page", page);
            param.Add("@Size", size);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            try
            {
                var lst = this.Procedure<DisplayPersonalTableViewModel>("sp_tblContractStaff_GetShareList", param).ToList();
                total = param.Get<int>("@Total");
                return new DataResult<DisplayPersonalTableViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<DisplayPersonalTableViewModel> { Error = true };
            }
        }
        #endregion

        #region DeleteAnEmployee 
        public DataResult<DisplayPersonalTableViewModel> DeleteAnEmployee(string staffCode)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (IDbTransaction transaction = db.BeginTransaction())
                    {
                        int ac = db.Execute("Update tblAccount SET Status = 0 WHERE CodeStaff = @CodeStaff", 
                            new { @CodeStaff = staffCode }, transaction);

                        int ct = db.Execute("Update tblContractStaff SET Status = 0 WHERE CodeStaff = @CodeStaff", 
                            new { @CodeStaff = staffCode }, transaction);
                        if (ac == 1 && ct == 1)
                        {
                            transaction.Commit();
                        }
                    }
                }

                return new DataResult<DisplayPersonalTableViewModel>();
            }
            catch
            {
                return new DataResult<DisplayPersonalTableViewModel>() { Error = true };
            }
        }
        #endregion
    }
}
