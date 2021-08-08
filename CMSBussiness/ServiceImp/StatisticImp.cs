using CRMBussiness.IService;
using CRMBussiness.ViewModel;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CRMBussiness.ServiceImp
{
    public class StatisticImp : IStatistic
    {
        //#region GetBranchStatisticInfo
        //public DataResult<BranchAggregateStatisticViewModel> GetBranchStatisticInfo(string month, string branch)
        //{
        //    BranchAggregateStatisticViewModel statisticalData = new BranchAggregateStatisticViewModel();
        //    DynamicParameters paramObject = new DynamicParameters();
        //    paramObject.Add("@Month", month);
        //    paramObject.Add("@Branch", branch);
        //    paramObject.Add("@TotalRevenue", dbType: DbType.Decimal, direction: ParameterDirection.Output);
        //    paramObject.Add("@TotalRemuneration", dbType: DbType.Decimal, direction: ParameterDirection.Output);

        //    try
        //    {
        //        using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
        //        {
        //            db.Open();
        //            using (var getResult = db.QueryMultiple("sp_GetRemuneration_By_Branch", paramObject, commandType: CommandType.StoredProcedure))
        //            {
        //                statisticalData.ListData = getResult.Read<BranchStatisticTableModel>().ToList();
        //            }
        //        }
        //        statisticalData.TotalRevenue = paramObject.Get<decimal>("TotalRevenue");
        //        statisticalData.TotalRemuneration = paramObject.Get<decimal>("TotalRemuneration");

        //        return new DataResult<BranchAggregateStatisticViewModel> { DataItem = statisticalData };
        //    }
        //    catch
        //    {
        //        return new DataResult<BranchAggregateStatisticViewModel> { Error = true };
        //    }
        //}
        //#endregion

        //#region GetOfficeStatisticInfo
        //public DataResult<OfficeAggregateStatisticViewModel> GetOfficeStatisticInfo(string month, string branch
        //    , string office)
        //{
        //    OfficeAggregateStatisticViewModel statisticalData = new OfficeAggregateStatisticViewModel();
        //    DynamicParameters paramObject = new DynamicParameters();
        //    paramObject.Add("@Month", month);
        //    paramObject.Add("@Branch", branch);
        //    paramObject.Add("@Office", office);
        //    paramObject.Add("@TotalRevenue", dbType: DbType.Decimal, direction: ParameterDirection.Output);
        //    paramObject.Add("@TotalRemuneration", dbType: DbType.Decimal, direction: ParameterDirection.Output);

        //    try
        //    {
        //        using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
        //        {
        //            db.Open();
        //            using (var getResult = db.QueryMultiple("sp_GetRemuneration_By_Office", paramObject, commandType: CommandType.StoredProcedure))
        //            {
        //                statisticalData.ListData = getResult.Read<OfficeStatisticTableModel>().ToList();
        //            }
        //        }
        //        statisticalData.TotalRevenue = paramObject.Get<decimal>("TotalRevenue");
        //        statisticalData.TotalRemuneration = paramObject.Get<decimal>("TotalRemuneration");

        //        return new DataResult<OfficeAggregateStatisticViewModel> { DataItem = statisticalData };
        //    }
        //    catch
        //    {
        //        return new DataResult<OfficeAggregateStatisticViewModel> { Error = true };
        //    }
        //}
        //#endregion

        //#region GetDepartmentStatisticInfo
        //public DataResult<DepartmentAggregateStatisticViewModel> GetDepartmentStatisticInfo(string month, string branch, string office, string department)
        //{
        //    DepartmentAggregateStatisticViewModel statisticalData = new DepartmentAggregateStatisticViewModel();
        //    DynamicParameters paramObject = new DynamicParameters();
        //    paramObject.Add("@Month", month);
        //    paramObject.Add("@Branch", branch);
        //    paramObject.Add("@Office", office);
        //    paramObject.Add("@Department", department);
        //    paramObject.Add("@TotalRevenue", dbType: DbType.Decimal, direction: ParameterDirection.Output);
        //    paramObject.Add("@TotalRemuneration", dbType: DbType.Decimal, direction: ParameterDirection.Output);

        //    try
        //    {
        //        using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
        //        {
        //            db.Open();
        //            using (var getResult = db.QueryMultiple("sp_GetRemuneration_By_Department", paramObject, commandType: CommandType.StoredProcedure))
        //            {
        //                statisticalData.ListData = getResult.Read<DepartmentStatisticTableModel>().ToList();
        //            }
        //        }
        //        statisticalData.TotalRevenue = paramObject.Get<decimal>("TotalRevenue");
        //        statisticalData.TotalRemuneration = paramObject.Get<decimal>("TotalRemuneration");

        //        return new DataResult<DepartmentAggregateStatisticViewModel> { DataItem = statisticalData };
        //    }
        //    catch
        //    {
        //        return new DataResult<DepartmentAggregateStatisticViewModel> { Error = true };
        //    }
        //}
        //#endregion

        //#region GetTeamStatisticInfo
        //public DataResult<TeamAggregateStatisticViewModel> GetTeamStatisticInfo(string month, string branch
        //    , string office, string department, string team)
        //{
        //    TeamAggregateStatisticViewModel statisticalData = new TeamAggregateStatisticViewModel();
        //    DynamicParameters paramObject = new DynamicParameters();
        //    paramObject.Add("@Month", month);
        //    paramObject.Add("@Branch", branch);
        //    paramObject.Add("@Office", office);
        //    paramObject.Add("@Department", department);
        //    paramObject.Add("@Team", team);
        //    paramObject.Add("@TotalRevenue", dbType: DbType.Decimal, direction: ParameterDirection.Output);
        //    paramObject.Add("@TotalRemuneration", dbType: DbType.Decimal, direction: ParameterDirection.Output);

        //    try
        //    {
        //        using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
        //        {
        //            db.Open();
        //            using (var getResult = db.QueryMultiple("sp_GetRemuneration_By_Team", paramObject, commandType: CommandType.StoredProcedure))
        //            {
        //                statisticalData.ListData = getResult.Read<TeamStatisticTableModel>().ToList();
        //            }
        //        }
        //        statisticalData.TotalRevenue = paramObject.Get<decimal>("TotalRevenue");
        //        statisticalData.TotalRemuneration = paramObject.Get<decimal>("TotalRemuneration");

        //        return new DataResult<TeamAggregateStatisticViewModel> { DataItem = statisticalData };
        //    }
        //    catch
        //    {
        //        return new DataResult<TeamAggregateStatisticViewModel> { Error = true };
        //    }
        //}
        //#endregion

        //#region GetPersonalStatisticInfo
        //public DataResult<PersonalAggregateStatisticViewModel> GetPersonalStatisticInfo(string month, string branch
        //    , string office, string department, string team, string staff)
        //{
        //    PersonalAggregateStatisticViewModel statisticalData = new PersonalAggregateStatisticViewModel();
        //    DynamicParameters paramObject = new DynamicParameters();
        //    paramObject.Add("@Month", month);
        //    paramObject.Add("@Branch", branch);
        //    paramObject.Add("@Office", office);
        //    paramObject.Add("@Department", department);
        //    paramObject.Add("@Team", team);
        //    paramObject.Add("@Staff", staff);
        //    paramObject.Add("@TotalRevenue", dbType: DbType.Decimal, direction: ParameterDirection.Output);
        //    paramObject.Add("@TotalRemuneration", dbType: DbType.Decimal, direction: ParameterDirection.Output);

        //    try
        //    {
        //        using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
        //        {
        //            db.Open();
        //            using (var getResult = db.QueryMultiple("sp_GetRemuneration_By_StaffCode", paramObject, commandType: CommandType.StoredProcedure))
        //            {
        //                statisticalData.ListData = getResult.Read<PersonalStatisticTableModel>().ToList();
        //            }
        //        }
        //        //statisticalData.TotalRevenue = paramObject.Get<decimal>("TotalRevenue");
        //        //statisticalData.TotalRemuneration = paramObject.Get<decimal>("TotalRemuneration");

        //        return new DataResult<PersonalAggregateStatisticViewModel> { DataItem = statisticalData };
        //    }
        //    catch
        //    {
        //        return new DataResult<PersonalAggregateStatisticViewModel> { Error = true };
        //    }
        //}
        //#endregion





        #region GetRevenueStatisticsInDurations
        /// <summary>
        /// get revenue of selected level in past durations
        /// </summary>
        /// <param name="time">Timeline</param>
        /// <param name="timeOption">day/week/month</param>
        /// <param name="branch">branch code</param>
        /// <param name="office">office code</param>
        /// <param name="department">department code</param>
        /// <param name="team">team code</param>
        /// <param name="staff">staff code or fullname</param>
        /// <returns>statistics data</returns>
        public DataResult<AllLevelsRevenueStatistics> GetRevenueStatisticsInDurations(DateTime time, int timeOption
            , string branch = null, string office = null, string department = null
            , string team = null, string staff = null)
        {
            DynamicParameters paramObject = new DynamicParameters();
            paramObject.Add("@Start", time);
            paramObject.Add("@TimeOption", timeOption);
            paramObject.Add("@Branch", branch);
            paramObject.Add("@Office", office);
            paramObject.Add("@Department", department);
            paramObject.Add("@Team", team);
            paramObject.Add("@Staff", staff ?? string.Empty);

            try
            {
                List<AllLevelsRevenueStatistics> data = new List<AllLevelsRevenueStatistics>();
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (var getResult = db.QueryMultiple("sp_GetRevenue_By_Duration", paramObject, commandType: CommandType.StoredProcedure))
                    {
                        data = getResult.Read<AllLevelsRevenueStatistics>().ToList();
                    }
                }

                return new DataResult<AllLevelsRevenueStatistics> { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<AllLevelsRevenueStatistics> { Error = true };
            }
        }
        #endregion

        #region GetBranchesStatisticInfo
        /// <summary>
        /// get revenue of each branch
        /// </summary>
        /// <param name="time">timeline</param>
        /// <param name="timeOption">day/week/month</param>
        /// <returns>statistics data</returns>
        public DataResult<ALevelRevenueStatistics> GetBranchesRevenueStatistics(DateTime time, int timeOption)
        {
            DynamicParameters paramObject = new DynamicParameters();
            paramObject.Add("@Start", time);
            paramObject.Add("@TimeOption", timeOption);

            try
            {
                List<ALevelRevenueStatistics> data = new List<ALevelRevenueStatistics>();
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (var getResult = db.QueryMultiple("sp_GetRevenue_By_Branches", paramObject, commandType: CommandType.StoredProcedure))
                    {
                        data = getResult.Read<ALevelRevenueStatistics>().ToList();
                    }
                }

                return new DataResult<ALevelRevenueStatistics> { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<ALevelRevenueStatistics> { Error = true };
            }
        }
        #endregion

        #region GetOfficesRevenueStatistics
        /// <summary>
        /// get revenue of each office
        /// </summary>
        /// <param name="time">timeline</param>
        /// <param name="timeOption">day/week/month</param>
        /// <param name="branch">branch code</param>
        /// <returns>statistics data</returns>
        public DataResult<ALevelRevenueStatistics> GetOfficesRevenueStatistics(DateTime time, int timeOption
            , string branch)
        {
            DynamicParameters paramObject = new DynamicParameters();
            paramObject.Add("@Start", time);
            paramObject.Add("@TimeOption", timeOption);
            paramObject.Add("@Branch", branch);

            try
            {
                List<ALevelRevenueStatistics> data = new List<ALevelRevenueStatistics>();
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (var getResult = db.QueryMultiple("sp_GetRevenue_By_Offices", paramObject, commandType: CommandType.StoredProcedure))
                    {
                        data = getResult.Read<ALevelRevenueStatistics>().ToList();
                    }
                }

                return new DataResult<ALevelRevenueStatistics> { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<ALevelRevenueStatistics> { Error = true };
            }
        }
        #endregion

        #region GetDepartmentsRevenueStatistics
        /// <summary>
        /// get revenue of each department
        /// </summary>
        /// <param name="time">timeline</param>
        /// <param name="timeOption">day/week/month</param>
        /// <param name="branch">branch code</param>
        /// <param name="office">office code</param>
        /// <returns>statistics data</returns>
        public DataResult<ALevelRevenueStatistics> GetDepartmentsRevenueStatistics(DateTime time, int timeOption
            , string branch, string office)
        {
            DynamicParameters paramObject = new DynamicParameters();
            paramObject.Add("@Start", time);
            paramObject.Add("@TimeOption", timeOption);
            paramObject.Add("@Branch", branch);
            paramObject.Add("@Office", office);

            try
            {
                List<ALevelRevenueStatistics> data = new List<ALevelRevenueStatistics>();
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (var getResult = db.QueryMultiple("sp_GetRevenue_By_Departments", paramObject, commandType: CommandType.StoredProcedure))
                    {
                        data = getResult.Read<ALevelRevenueStatistics>().ToList();
                    }
                }

                return new DataResult<ALevelRevenueStatistics> { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<ALevelRevenueStatistics> { Error = true };
            }
        }
        #endregion

        #region GetTeamsRevenueStatistics
        /// <summary>
        /// get revenue of each team
        /// </summary>
        /// <param name="time">timeline</param>
        /// <param name="timeOption">day/week/month</param>
        /// <param name="branch">branch code</param>
        /// <param name="office">office code</param>
        /// <param name="department">department code</param>
        /// <returns>statistics data</returns>
        public DataResult<ALevelRevenueStatistics> GetTeamsRevenueStatistics(DateTime time, int timeOption
            , string branch, string office, string department)
        {
            DynamicParameters paramObject = new DynamicParameters();
            paramObject.Add("@Start", time);
            paramObject.Add("@TimeOption", timeOption);
            paramObject.Add("@Branch", branch);
            paramObject.Add("@Office", office);
            paramObject.Add("@Department", department);

            try
            {
                List<ALevelRevenueStatistics> data = new List<ALevelRevenueStatistics>();
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (var getResult = db.QueryMultiple("sp_GetRevenue_By_Teams", paramObject, commandType: CommandType.StoredProcedure))
                    {
                        data = getResult.Read<ALevelRevenueStatistics>().ToList();
                    }
                }

                return new DataResult<ALevelRevenueStatistics> { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<ALevelRevenueStatistics> { Error = true };
            }
        }
        #endregion

        #region GetPersonalRevenueStatistics
        /// <summary>
        /// get revenue of each staff
        /// </summary>
        /// <param name="time">timeline</param>
        /// <param name="timeOption">day/week/month</param>
        /// <param name="branch">branch code</param>
        /// <param name="office">office code</param>
        /// <param name="department">department code</param>
        /// <param name="team">team code</param>
        /// <returns>statistics data</returns>
        public DataResult<ALevelRevenueStatistics> GetPersonalRevenueStatistics(DateTime time, int timeOption
            , string branch, string office, string department, string team)
        {
            DynamicParameters paramObject = new DynamicParameters();
            paramObject.Add("@Start", time);
            paramObject.Add("@TimeOption", timeOption);
            paramObject.Add("@Branch", branch);
            paramObject.Add("@Office", office);
            paramObject.Add("@Department", department);
            paramObject.Add("@Team", team);

            try
            {
                List<ALevelRevenueStatistics> data = new List<ALevelRevenueStatistics>();
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (var getResult = db.QueryMultiple("sp_GetRevenue_By_Staffs", paramObject, commandType: CommandType.StoredProcedure))
                    {
                        data = getResult.Read<ALevelRevenueStatistics>().ToList();
                    }
                }

                return new DataResult<ALevelRevenueStatistics> { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<ALevelRevenueStatistics> { Error = true };
            }
        }
        #endregion

        #region GetCurrentRevenueAndProportion
        /// <summary>
        /// get current revenue and proportion of selected level
        /// </summary>
        /// <param name="time">timeline</param>
        /// <param name="timeOption">day/week/month</param>
        /// <param name="branch">branch code</param>
        /// <param name="office">office code</param>
        /// <param name="department">department code</param>
        /// <param name="team">team code</param>
        /// <param name="staff">satff code or fullname</param>
        /// <returns>statistics data</returns>
        public DataResult<Tuple<decimal, decimal>> GetCurrentRevenueAndProportion(DateTime time, int timeOption
            , string branch = null, string office = null, string department = null
            , string team = null, string staff = null)
        {
            DynamicParameters paramObject = new DynamicParameters();
            paramObject.Add("@Start", time);
            paramObject.Add("@TimeOption", timeOption);
            paramObject.Add("@Branch", branch);
            paramObject.Add("@Office", office);
            paramObject.Add("@Department", department);
            paramObject.Add("@Team", team);
            paramObject.Add("@Staff", staff??string.Empty);
            paramObject.Add("@TotalCurrentRevenue", dbType: DbType.Decimal, direction: ParameterDirection.Output);
            paramObject.Add("@IncrementPercent", dbType: DbType.Decimal, scale: 2, direction: ParameterDirection.Output);

            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (var getResult = db.QueryMultiple("sp_GetRevenueAndProportion_By_CurrentTime", paramObject, commandType: CommandType.StoredProcedure))
                    {
                    }
                }
                var totalRevenue = paramObject.Get<decimal>("TotalCurrentRevenue");
                var percent = paramObject.Get<decimal>("IncrementPercent");

                return new DataResult<Tuple<decimal, decimal>>
                {
                    Result = new List<Tuple<decimal, decimal>>
                    {
                        new Tuple<decimal, decimal>(totalRevenue, percent)
                    } 
                };
            }
            catch(Exception ex)
            {
                return new DataResult<Tuple<decimal, decimal>> { Error = true };
            }
        }
        #endregion

        #region GetProductComponents
        /// <summary>
        /// get revenue of selected level in past durations
        /// </summary>
        /// <param name="time">Timeline</param>
        /// <param name="timeOption">day/week/month</param>
        /// <param name="branch">branch code</param>
        /// <param name="office">office code</param>
        /// <param name="department">department code</param>
        /// <param name="team">team code</param>
        /// <param name="staff">staff code or fullname</param>
        /// <returns>statistics data</returns>
        public DataResult<ProductCompnentStatistics> GetProductComponents(DateTime time, int timeOption
            , string branch = null, string office = null, string department = null
            , string team = null, string staff = null)
        {
            DynamicParameters paramObject = new DynamicParameters();
            paramObject.Add("@Start", time);
            paramObject.Add("@TimeOption", timeOption);
            paramObject.Add("@Branch", branch);
            paramObject.Add("@Office", office);
            paramObject.Add("@Department", department);
            paramObject.Add("@Team", team);
            paramObject.Add("@Staff", staff ?? string.Empty);

            try
            {
                List<ProductCompnentStatistics> data = new List<ProductCompnentStatistics>();
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (var getResult = db.QueryMultiple("sp_GetRevenueComponents_By_Duration", paramObject, commandType: CommandType.StoredProcedure))
                    {
                        data = getResult.Read<ProductCompnentStatistics>().ToList();
                    }
                }

                return new DataResult<ProductCompnentStatistics> { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<ProductCompnentStatistics> { Error = true };
            }
        }
        #endregion
    }
}
