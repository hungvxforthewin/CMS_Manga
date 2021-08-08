using CRMBussiness.IService;
using CRMBussiness.ViewModel;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CRMBussiness.ServiceImp
{
    public class RuleInMonthImp : IRuleInMonth
    {
        #region Setup
        public DataResult<RuleInMonthViewModel> Setup(RuleInMonthViewModel model)
        {
            try
            {
                RuleInMonthViewModel existedRule = null;
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("sp_tblRuleInMonth_ByMonth",
                        new { @month = model.Month },
                        commandType: CommandType.StoredProcedure))
                    {
                        existedRule = multipleresult.Read<RuleInMonthViewModel>().FirstOrDefault();
                    }

                    if (existedRule == null)
                    {
                        string processInsertQuery = @"INSERT INTO tblRuleInMonth(Month, TotalWorkingDays, OtherBonus) VALUES 
                        (@Month, @TotalWorkingDays, @OtherBonus)";
                        db.Execute(processInsertQuery, model);
                    }
                    else
                    {
                        string processUpdateQuery = @"UPDATE tblRuleInMonth SET TotalWorkingDays = @TotalWorkingDays, 
                            OtherBonus = @OtherBonus WHERE Month = @Month";
                        db.Execute(processUpdateQuery, model);
                    }
                }

                return new DataResult<RuleInMonthViewModel> { Error = false };
            }
            catch (Exception ex)
            {
                return new DataResult<RuleInMonthViewModel> { Error = true };
            }
        }
        #endregion

        #region GetByMonth
        public DataResult<RuleInMonthViewModel> GetByMonth(string month)
        {
            try
            {
                List<RuleInMonthViewModel> data = new List<RuleInMonthViewModel>();
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("sp_tblRuleInMonth_ByMonth",
                        new { @month = month },
                        commandType: CommandType.StoredProcedure))
                    {
                        data = multipleresult.Read<RuleInMonthViewModel>().ToList();
                    }
                }

                return new DataResult<RuleInMonthViewModel> { Result = data };
            }
            catch
            {
                return new DataResult<RuleInMonthViewModel> { Error = true };
            }
        }
        #endregion
    }
}
