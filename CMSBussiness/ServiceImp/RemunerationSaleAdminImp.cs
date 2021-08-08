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
using Remuneration = CRMModel.Models.Data.Remuneration;

namespace CRMBussiness.ServiceImp
{
    public class RemunerationSaleAdminImp : BaseService<Remuneration, long>, IRemunerationSaleAdmin
    {
        public bool DeleteAllByRole(byte role)
        {
            try
            {
                this.Raw_Query<Remuneration>("DELETE tblRemuneration WHERE RoleAccount = @role", param: new Dictionary<string, object>() {
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
    }
}
