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
    public class RemunerationSaleManagerImp : BaseService<Remuneration, long>, IRemunerationSaleManager
    {
        public List<Remuneration> GetByRole(int roleAccount)
        {
            try
            {
                return this.Raw_Query<Remuneration>("SELECT * FROM tblRemuneration WHERE RoleAccount = @role", param: new Dictionary<string, object>() {
                    {"role", roleAccount }
                }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
