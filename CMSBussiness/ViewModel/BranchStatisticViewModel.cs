using System.Collections.Generic;

namespace CRMBussiness.ViewModel
{
    #region BranchAggregateStatisticViewModel
    public class BranchAggregateStatisticViewModel
    {
        public List<BranchStatisticTableModel> ListData { get; set; }

        public decimal TotalRevenue { get; set; }

        public decimal TotalRemuneration { get; set; }
    }

    public class BranchStatisticTableModel
    {
        public string BranchCode { get; set; }

        public string BranchName { get; set; }

        public decimal BranchRevenue { get; set; }

        public decimal BranchRemuneration { get; set; }
    }
    #endregion

    #region OfficeAggregateStatisticViewModel
    public class OfficeAggregateStatisticViewModel
    {
        public List<OfficeStatisticTableModel> ListData { get; set; }

        public decimal TotalRevenue { get; set; }

        public decimal TotalRemuneration { get; set; }
    }

    public class OfficeStatisticTableModel
    {
        public string OfficeCode { get; set; }

        public string OfficeName { get; set; }

        public string BranchName { get; set; }

        public decimal OfficeRevenue { get; set; }

        public decimal OfficeRemuneration { get; set; }
    }
    #endregion

    #region DepartmentAggregateStatisticViewModel
    public class DepartmentAggregateStatisticViewModel
    {
        public List<DepartmentStatisticTableModel> ListData { get; set; }

        public decimal TotalRevenue { get; set; }

        public decimal TotalRemuneration { get; set; }
    }

    public class DepartmentStatisticTableModel
    {
        public string DepartmentCode { get; set; }

        public string DepartmentName { get; set; }

        public string OfficeName { get; set; }

        public string BranchName { get; set; }

        public decimal DepartmentRevenue { get; set; }

        public decimal DepartmentRemuneration { get; set; }
    }
    #endregion

    #region TeamAggregateStatisticViewModel
    public class TeamAggregateStatisticViewModel
    {
        public List<TeamStatisticTableModel> ListData { get; set; }

        public decimal TotalRevenue { get; set; }

        public decimal TotalRemuneration { get; set; }
    }

    public class TeamStatisticTableModel
    {
        public string TeamCode { get; set; }

        public string TeamName { get; set; }

        public string DepartmentName { get; set; }

        public string OfficeName { get; set; }

        public string BranchName { get; set; }

        public decimal TeamRevenue { get; set; }

        public decimal TeamRemuneration { get; set; }
    }
    #endregion

    #region PersonalAggregateStatisticViewModel
    public class PersonalAggregateStatisticViewModel
    {
        public List<PersonalStatisticTableModel> ListData { get; set; }

        public decimal TotalRevenue { get; set; }

        public decimal TotalRemuneration { get; set; }
    }

    public class PersonalStatisticTableModel
    {
        public string CodeStaff { get; set; }

        public byte Role { get; set; }

        public string FullName { get; set; }

        public string TeamName { get; set; }

        public string DepartmentName { get; set; }

        public string OfficeName { get; set; }

        public string BranchName { get; set; }

        public decimal PersonalRevenueS { get; set; }

        public decimal PersonalRevenueTO { get; set; }

        public decimal PersonalRevenue { get; set; }

        public decimal PersonalRemuneration { get; set; }
    }
    #endregion

    #region AllLevelsRevenueStatistics
    public class AllLevelsRevenueStatistics
    {
        public string StartTime { get; set; }

        public decimal Revenue { get; set; }
    }
    #endregion

    #region ALevelRevenueStatistics
    public class ALevelRevenueStatistics
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Revenue { get; set; }
    }
    #endregion

    #region ProductCompnentStatistics
    public class ProductCompnentStatistics
    {
        public string ProductName { get; set; }

        public decimal Percent { get; set; }
    }
    #endregion

    #region RevenueStatisticViewModel
    public class RevenueStatisticViewModel
    {
        public List<AllLevelsRevenueStatistics> AllLevelsRevenueInDuration { get; set; }

        public List<ALevelRevenueStatistics> CurrentALevelRevenue { get; set; }

        public List<ProductCompnentStatistics> ProductComponents { get; set; }

        public List<RevenueTarget> Targets { get; set; }

        public decimal CurrentRevenue { get; set; }

        public decimal ProportionPercent { get; set; }

        public float FinishedLevel { get; set; }
    }
    #endregion
}
