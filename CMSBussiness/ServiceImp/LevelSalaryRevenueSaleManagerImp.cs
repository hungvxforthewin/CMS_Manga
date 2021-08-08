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
    public class LevelSalaryRevenueSaleManagerImp : BaseService<LevelSalaryRevenue, short>, ILevelSalaryRevenueSaleManager
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

        public LevelSalaryRevenue GetFirstByRoleAndTimeKpi(string kpiCode, int timeKpi)
        {
            try
            {
                return this.Raw_Query<LevelSalaryRevenue>("SELECT * FROM tblLevelSalaryRevenue WHERE CodeKpi = @kpiCode AND TimeKpi = @timeKpi", param: new Dictionary<string, object>() {
                    {"kpiCode", kpiCode },
                    {"timeKpi", timeKpi }
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<LevelSalaryRevenue> GetLevelSalaryRevenueByKpiCode(string kpiCode)
        {
            try
            {
                return this.Raw_Query<LevelSalaryRevenue>("SELECT * FROM tblLevelSalaryRevenue WHERE CodeKpi = @kpiCode", param: new Dictionary<string, object>() {
                    {"kpiCode", kpiCode }
                }).ToList();
                
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<LevelSalaryRevenue> GetLevelSalaryRevenueByRole(int role)
        {
            try
            {
                return this.Raw_Query<LevelSalaryRevenue>("SELECT * FROM tblLevelSalaryRevenue WHERE RoleAccount = @role", param: new Dictionary<string, object>() {
                    {"role", role }
                }).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
