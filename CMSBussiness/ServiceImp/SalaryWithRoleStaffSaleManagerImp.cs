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
using SalaryWithRoleStaff = CRMModel.Models.Data.SalaryWithRoleStaff;

namespace CRMBussiness.ServiceImp
{
    public class SalaryWithRoleStaffSaleManagerImp : BaseService<SalaryWithRoleStaff, long>, ISalaryWithRoleStaffSaleManager
    {
        public List<SalaryWithRoleStaff> GetByRole(byte roleAccount)
        {
            try
            {
                return this.Raw_Query<SalaryWithRoleStaff>("SELECT * FROM tblSalaryWithRoleStaff WHERE RoleAccount = @Role", param: new Dictionary<string, object>(){
                    {"Role", roleAccount }
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
