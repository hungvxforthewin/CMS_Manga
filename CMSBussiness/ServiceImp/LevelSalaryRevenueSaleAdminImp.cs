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
    public class LevelSalaryRevenueSaleAdminImp : BaseService<LevelSalaryRevenue, short>, ILevelSalaryRevenueSaleAdmin
    {
        public bool DeleteAllByRole(byte role)
        {
            try
            {
                this.Raw_Query<LevelSalaryRevenue>("DELETE tblLevelSalaryRevenue WHERE RoleAccount = @role", param: new Dictionary<string, object>() {
                    {"role", role }
                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public short InsertWithSaleAdmin(byte RoleAccount, decimal Salary, byte ProbationaryTime, decimal ProbationarySalary, float PercentRemuneration, decimal RevenueMin, decimal RevenueMax, ref short id)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@RoleAccount", RoleAccount);
                param.Add("@Salary", Salary);
                param.Add("@ProbationaryTime", ProbationaryTime);
                param.Add("@ProbationarySalary", ProbationarySalary);
                param.Add("@PercentRemuneration", PercentRemuneration);
                param.Add("@RevenueMin", RevenueMin);
                param.Add("@RevenueMax", RevenueMax);
                param.Add("@id", dbType: DbType.Int16, direction: ParameterDirection.Output);
                var lst = this.Procedure<LevelSalaryRevenue>("sp_tblLevelSalaryRevenue_InsertBySaleAdmin", param).ToList();
                id = param.Get<short>("@id");
                return id;
            }
            catch (Exception ex)
            {
                return -1;
                throw;
            }
        }

        public List<LevelSalaryRevenueSaleAdmin> LevelSalaryRevenueSaleAdminWithRole(byte roleAccount)
        {
            try
            {
                return this.Raw_Query<LevelSalaryRevenueSaleAdmin>("SELECT b.RoleAccount, b.Salary, b.ProbationaryTime, b.ProbationarySalary, b.Id, b.PercentRemuneration, b.RevenueMin, b.RevenueMax, b.SharePercent FROM  tblLevelSalaryRevenue b  " +
                    "WHERE b.RoleAccount = @role", param: new Dictionary<string, object>()
                {
                    { "role", roleAccount }
                }).ToList();
            }
            catch (Exception ex)
            {
                return new List<LevelSalaryRevenueSaleAdmin>();
                throw;
            }
        }
    }
}
