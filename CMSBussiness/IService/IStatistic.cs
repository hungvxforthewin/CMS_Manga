using CRMBussiness.ViewModel;
using System;

namespace CRMBussiness.IService
{
    public interface IStatistic
    {
        //DataResult<BranchAggregateStatisticViewModel> GetBranchStatisticInfo(string month, string branch);

        //DataResult<OfficeAggregateStatisticViewModel> GetOfficeStatisticInfo(string month, string branch
        //    , string office);

        //DataResult<DepartmentAggregateStatisticViewModel> GetDepartmentStatisticInfo(string month, string branch
        //    , string office, string department);

        //DataResult<TeamAggregateStatisticViewModel> GetTeamStatisticInfo(string month, string branch
        //    , string office, string department, string team);

        //DataResult<PersonalAggregateStatisticViewModel> GetPersonalStatisticInfo(string month, string branch
        //   , string office, string department, string team, string staff);

        DataResult<AllLevelsRevenueStatistics> GetRevenueStatisticsInDurations(DateTime time, int timeOption
            , string branch = null, string office = null, string department = null
            , string team = null, string staff = null);

        DataResult<ProductCompnentStatistics> GetProductComponents(DateTime time, int timeOption
           , string branch = null, string office = null, string department = null
           , string team = null, string staff = null);

        DataResult<ALevelRevenueStatistics> GetBranchesRevenueStatistics(DateTime time, int timeOption);

        DataResult<ALevelRevenueStatistics> GetOfficesRevenueStatistics(DateTime time, int timeOption, string branch);

        DataResult<ALevelRevenueStatistics> GetDepartmentsRevenueStatistics(DateTime time, int timeOption, string branch
            , string office);

        DataResult<ALevelRevenueStatistics> GetTeamsRevenueStatistics(DateTime time, int timeOption, string branch
           , string office, string department);

        DataResult<ALevelRevenueStatistics> GetPersonalRevenueStatistics(DateTime time, int timeOption, string branch
           , string office, string department, string team);

        DataResult<Tuple<decimal, decimal>> GetCurrentRevenueAndProportion(DateTime time, int timeOption
            , string branch = null, string office = null, string department = null
            , string team = null, string staff = null);
    }
}
