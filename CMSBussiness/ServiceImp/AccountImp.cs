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
    public class AccountImp : BaseService<Account, long>, IAccount
    {
        #region GetList
        public DataResult<AccountViewModel> GetList(SearchAccountViewModel model, out int total)
        {
            List<AccountViewModel> data = new List<AccountViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", model.Key);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    data = this.Procedure<AccountViewModel>("SP_CMS_Account_GetList", param).ToList();
                    total = param.Get<int>("Total");
                }
                return new DataResult<AccountViewModel> { Result = data ?? new List<AccountViewModel>() };
            }
            catch (Exception ex)
            {
                return new DataResult<AccountViewModel> { Error = true };
            }
        }

        public Account GetByCodeStaff(string codeStaff)
        {
            try
            {
                return this.Raw_Query<Account>("SELECT * FROM tblAccount WHERE CodeStaff = @codeStaff", param: new Dictionary<string, object>() {
                    {"codeStaff", codeStaff }
                }).SingleOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Login
        public DataResult<AccountViewModel> Login(string UserName)
        {
            try
            {
                List<AccountViewModel> Ac = new List<AccountViewModel>();
                string connectionStr = OpenDapper.connectionStr;
                using (IDbConnection db = new SqlConnection(connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("CMS_Account_Search_By_AccountName",
                        new { @AccountName = UserName }, commandType: CommandType.StoredProcedure))
                    {
                        Ac = multipleresult.Read<AccountViewModel>().ToList();
                    }
                }

                return new DataResult<AccountViewModel> { Result = Ac };
            }
            catch (Exception ex)
            {
                return new DataResult<AccountViewModel> { Error = true };
            }
        }
        #endregion



        public Account GetAccountLast()
        {
            try
            {
                return this.Raw_Query<Account>("SELECT TOP 1 * FROM tblAccount ORDER BY Id DESC").FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new Account();
                throw;
            }
        }

        public bool InsertAccount(AccountViewModel model, out int statusInsert)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@AccountName", model.AccountName);
                param.Add("@AccountPassword", model.AccountPassword);
                param.Add("@AccountFullName", model.AccountFullName);
                param.Add("@isEnable", model.isEnable);
                param.Add("@ResponseStatus", dbType: DbType.Int32, direction: ParameterDirection.Output);
                statusInsert = 0;
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    this.Procedure<AccountViewModel>("CMS_CreateAccount", param).ToList();
                    statusInsert = param.Get<int>("ResponseStatus");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}