using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using SalaryWithRoleStaff = CRMModel.Models.Data.SalaryWithRoleStaff;

namespace CRMBussiness.IService
{
    public interface ISalaryWithRoleStaffSaleAdmin : IBaseServices<SalaryWithRoleStaff, short>
    {
        SalaryWithRoleStaff GetByRole(byte roleAccount);
    }
}
