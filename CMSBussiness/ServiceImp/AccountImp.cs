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
        #region Const
        //private const string _storeProcedureXml = "StoreProcedure/";
        //private const string _getByUsername = "st_tblAccount_Search_By_Username";
        private readonly List<byte> _saleRoles = new List<byte>() { 4, 5, 6 , 10, 11 };
        #endregion

        #region GetAll
        public DataResult<Account> GetAll()
        {
            try
            {
                return new DataResult<Account> { Result = { } };
            }
            catch
            {
                return new DataResult<Account> { Error = true };
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

                    //update share for sales, sale leader, sm, ctv, tk
                    //if (DateTime.Now.Day == 1 && Ac.FirstOrDefault() != null)
                    //{
                    //    string checkMonth = DateTime.Now.Year + "/" + (DateTime.Now.Month - 2 < 10 ? "0" + (DateTime.Now.Month - 2) : (DateTime.Now.Month - 2) + string.Empty);

                    //    string updatedMonth = DateTime.Now.Year + "/" + (DateTime.Now.Month - 1 < 10 ? "0" + (DateTime.Now.Month - 1) : (DateTime.Now.Month - 1) + string.Empty);

                    //    if (_saleRoles.Contains(Ac.First().Role))
                    //    {
                    //        if (Ac.First().MonthOfHoldingStocks == null || Ac.First().MonthOfHoldingStocks == checkMonth)
                    //        {
                    //            this.Procedure<AccountViewModel>("sp_tblContractStaff_UpdateShare", new { @month = updatedMonth });
                    //        }
                    //    }
                    //    else 
                    //    {
                    //        using (var multipleresult = db.QueryMultiple("sp_tblAccount_GetAnActiveSaleInfo",
                    //        new { }, commandType: CommandType.StoredProcedure))
                    //        {
                    //            var result = multipleresult.Read<AccountViewModel>().ToList();

                    //            if (result.FirstOrDefault() != null && (result.First().MonthOfHoldingStocks == null || result.First().MonthOfHoldingStocks == checkMonth))
                    //            {
                    //                this.Procedure<AccountViewModel>("sp_tblContractStaff_UpdateShare", new { @month = updatedMonth });
                    //            }
                    //        }
                    //    }
                    //}    
                }

                return new DataResult<AccountViewModel> { Result = Ac };
            }
            catch (Exception ex)
            {
                return new DataResult<AccountViewModel> { Error = true };
            }
        }
        #endregion

        #region UpdateShareForSale
        public bool UpdateShareForSale()
        {
            try
            {
                if (DateTime.Now.Day == 1)
                {
                    using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                    {
                        using (var multipleresult = db.QueryMultiple("sp_tblAccount_GetAnActiveSaleInfo",
                            new { }, commandType: CommandType.StoredProcedure))
                        {
                            string checkMonth = DateTime.Now.Year + "/" + (DateTime.Now.Month - 2 < 10 ? "0" + (DateTime.Now.Month - 2) : (DateTime.Now.Month - 2) + string.Empty);

                            string updatedMonth = DateTime.Now.Year + "/" + (DateTime.Now.Month - 1 < 10 ? "0" + (DateTime.Now.Month - 1) : (DateTime.Now.Month - 1) + string.Empty);

                            var result = multipleresult.Read<AccountViewModel>().ToList();

                            if (result.FirstOrDefault() != null && (result.First().MonthOfHoldingStocks == null || result.First().MonthOfHoldingStocks == checkMonth))
                            {
                                this.Procedure<AccountViewModel>("sp_tblContractStaff_UpdateShare", new { @month = updatedMonth });
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
            }

            return false;
        }
        #endregion

        #region GetEmployeeList
        public DataResult<EmployeeViewModel> GetEmployeeList()
        {
            List<EmployeeViewModel> data = new List<EmployeeViewModel>();
            string connectionStr = OpenDapper.connectionStr;
            try
            {
                using (IDbConnection db = new SqlConnection(connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("sp_tblAccount_GetList_ByRoles", new { }, commandType: CommandType.StoredProcedure))
                    {
                        data = multipleresult.Read<EmployeeViewModel>().ToList();
                    }
                }

                return new DataResult<EmployeeViewModel> { Result = data };
            }
            catch
            {
                return new DataResult<EmployeeViewModel> { Error = true };
            }
        }
        #endregion

        #region GetEmployeeInfoByStaffCode
        public DataResult<EmployeeInfoModel> GetEmployeeInfoByStaffCode(string staffCode)
        {
            List<EmployeeInfoModel> data = new List<EmployeeInfoModel>();
            string connectionStr = OpenDapper.connectionStr;
            try
            {
                using (IDbConnection db = new SqlConnection(connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("sp_tblContractStaff_GetPersonalInfo_ByStaffCode", new { @staffCode = staffCode }, commandType: CommandType.StoredProcedure))
                    {
                        data = multipleresult.Read<EmployeeInfoModel>().ToList();
                    }
                }

                return new DataResult<EmployeeInfoModel> { Result = data };
            }
            catch
            {
                return new DataResult<EmployeeInfoModel> { Error = true };
            }
        }
        #endregion

        #region GetEmployeeListByType
        public DataResult<EmployeeViewModel> GetEmployeeListByType(bool isTele)
        {
            try
            {
                var lst = this.Procedure<EmployeeViewModel>("sp_tblContractStaff_GetEmployeeList_ByType", new { @isTele = isTele }).ToList();
                return new DataResult<EmployeeViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<EmployeeViewModel> { Error = true };
            }
        }
        #endregion

        #region GetEmployeeListByTypeAndName
        public DataResult<EmployeeViewModel> GetEmployeeListByTypeAndName(bool isTele, string fullName)
        {
            try
            {
                var lst = this.Procedure<EmployeeViewModel>("sp_tblContractStaff_GetEmployeeList_ByNameAndType", new { @Name = fullName, @isTele = isTele }).ToList();
                return new DataResult<EmployeeViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<EmployeeViewModel> { Error = true };
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

        public DataResult<EmployeeInfoModel> GetEmployeeInfoByBranch(string branchCode)
        {
            try
            {
                List<EmployeeInfoModel> Ac = new List<EmployeeInfoModel>();
                Ac = this.Raw_Query<EmployeeInfoModel>("SELECT * FROM tblAccount WHERE BranchCode = @branchCode", param: new Dictionary<string, object>() {
                    {"branchCode", branchCode }
                }).ToList();
                return new DataResult<EmployeeInfoModel>() { Result = Ac };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataResult<EmployeeInfoModel> GetEmployeeInfoByOffice(string officeCode)
        {
            try
            {
                List<EmployeeInfoModel> Ac = new List<EmployeeInfoModel>();
                Ac = this.Raw_Query<EmployeeInfoModel>("SELECT * FROM tblAccount WHERE OfficeCode = @OfficeCode", param: new Dictionary<string, object>() {
                    {"OfficeCode", officeCode }
                }).ToList();
                return new DataResult<EmployeeInfoModel>() { Result = Ac };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataResult<EmployeeInfoModel> GetEmployeeInfoByDepart(string departCode)
        {
            try
            {
                List<EmployeeInfoModel> Ac = new List<EmployeeInfoModel>();
                Ac = this.Raw_Query<EmployeeInfoModel>("SELECT * FROM tblAccount WHERE DepartmentCode = @departmentCode", param: new Dictionary<string, object>() {
                    {"departmentCode", departCode }
                }).ToList();
                return new DataResult<EmployeeInfoModel>() { Result = Ac };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataResult<EmployeeInfoModel> GetEmployeeInfoByTeam(string teamCode)
        {
            try
            {
                List<EmployeeInfoModel> Ac = new List<EmployeeInfoModel>();
                Ac = this.Raw_Query<EmployeeInfoModel>("SELECT * FROM tblAccount WHERE TeamCode = @TeamCode", param: new Dictionary<string, object>() {
                    {"TeamCode", teamCode }
                }).ToList();
                return new DataResult<EmployeeInfoModel>() { Result = Ac ?? new List<EmployeeInfoModel>() };
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}