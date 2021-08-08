using CRMBussiness.ViewModel;
using System;

namespace CRMBussiness.IService
{
    public interface ITarget
    {
        DataResult<SaleTargetViewModel> SetRevenueTargets(SaleTargetViewModel model);

        DataResult<decimal> GetRevenueTarget(byte role, string targetFor, DateTime date,
            int timeOption);

        DataResult<RevenueTarget> GetBranchRevenueTargetList(DateTime date);

        DataResult<RevenueTarget> GetOfficeRevenueTargetList(DateTime date, string branch);

        DataResult<RevenueTarget> GetDepartmentRevenueTargetList(DateTime date, string office);

        DataResult<RevenueTarget> GetTeamRevenueTargetList(DateTime date, string department);

        DataResult<RevenueTarget> GetSaleRevenueTargetList(DateTime date, string team);
    }
}
