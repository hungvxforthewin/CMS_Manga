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
using Kpi = CRMModel.Models.Data.Kpi;

namespace CRMBussiness.ServiceImp
{
    public class KpiSaleManagerImp : BaseService<Kpi, long>, IKpiSaleManager
    {
        public bool DeleteAllByRole(byte role)
        {
            try
            {
                this.Raw_Query<Kpi>("DELETE tblKpi WHERE RoleAccount = @role", param: new Dictionary<string, object>() {
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

        public List<Kpi> GetByRole(int roleAccount)
        {
            try
            {
                return this.Raw_Query<Kpi>("SELECT b.RoleAccount, b.CodeKpi, b.KpiName, b.Revenue, b.Id AS RemunerationId FROM  tblKpi b  " +
                    "WHERE b.RoleAccount = @role", param: new Dictionary<string, object>()
                {
                    { "role", roleAccount }
                }).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
