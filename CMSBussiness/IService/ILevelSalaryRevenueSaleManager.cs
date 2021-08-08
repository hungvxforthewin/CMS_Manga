using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface ILevelSalaryRevenueSaleManager : IBaseServices<LevelSalaryRevenue, short>
    {
        List<LevelSalaryRevenue> GetLevelSalaryRevenueByKpiCode(string kpiCode);
        LevelSalaryRevenue GetFirstByRoleAndTimeKpi(string kpiCode, int timeKpi);
        bool DeleteAllByRole(byte role);
        List<LevelSalaryRevenue> GetLevelSalaryRevenueByRole(int role);

    }
}
