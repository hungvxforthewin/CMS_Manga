using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using SalaryWithRoleStaff = CRMModel.Models.Data.SalaryWithRoleStaff;

namespace CRMBussiness.IService
{
    public interface ISalaryWithRoleStaffSaleManager : IBaseServices<SalaryWithRoleStaff, short>
    {
        List<SalaryWithRoleStaff> GetByRole(byte roleAccount);
    }
}
