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
    public class KpiSaleAdminImp : BaseService<Kpi, long>, IKpiSaleAdmin
    {
        public List<RevenuePercentSaleAdmin> RevenuePercentSaleAdminsWithRole(byte roleAccount)
        {
            try
            {
                return this.Raw_Query<RevenuePercentSaleAdmin>("SELECT b.MinRevenueBranch, b.MaxRevenueBranch, b.[Percent], b.CodeRemuneration, b.Id AS RemunerationId FROM  tblRemuneration b  " +
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
