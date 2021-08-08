using CRMBussiness.IService;
using CRMBussiness.ViewModel;
using CRMModel.Models.Data;
using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CRMBussiness.ServiceImp
{
    public class TargetImp : ITarget
    {
        private const string InsertTargetQuery = "INSERT INTO tblTargets(Role, SetTargetFor, Revenue, StartDate, Status, CreatedBy) VALUES(@Role, @SetTargetFor, @Revenue, @StartDate, 1, @CreatedBy)";
        private const string DisableTargetQuery = "UPDATE tblTargets SET EndDate = @EndDate, Status = 0 WHERE Id = @Id";
        private const string UpdateRevenueQuery = "UPDATE tblTargets SET Revenue = @Revenue WHERE Id = @Id";

        #region GetRevenueTarget
        public DataResult<decimal> GetRevenueTarget(byte role, string targetFor, DateTime date,
            int timeOption)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var getResult = db.QueryMultiple("sp_tblTarget_GetInfo_By_TimeOption",
                                new
                                {
                                    @Role = role,
                                    @Date = date,
                                    @TargetFor = targetFor,
                                    @TimeOption = timeOption
                                }, commandType: CommandType.StoredProcedure))
                    {
                        var target = getResult.Read<decimal>().ToList();
                        return new DataResult<decimal> { Result = target };
                    }
                }
            }
            catch (Exception ex)
            {
                return new DataResult<decimal> { Error = true };
            }
        }
        #endregion

        #region SetTargets
        public DataResult<SaleTargetViewModel> SetRevenueTargets(SaleTargetViewModel model)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        foreach (var item in model.Targets)
                        {
                            Target targetModel = new Target
                            {
                                Revenue = item.Revenue * 1000000000,
                                StartDate = DateTime.Now.AddDays(-1 * DateTime.Now.Day + 1).Date,
                                CreatedBy = model.CreatedBy,
                                Role = model.Role,
                                SetTargetFor = item.SetTargetFor,

                            };

                            using (var getResult = db.QueryMultiple("sp_tblTarget_GetInfo_By_Role",
                                new {
                                    @Role = model.Role,
                                    @Date = DateTime.Now,
                                    @TargetFor = item.SetTargetFor
                                }, transaction, commandType: CommandType.StoredProcedure))
                            {
                                var target = getResult.Read<TargetModel>().FirstOrDefault();
                                if (target is null)
                                {
                                    db.Execute(InsertTargetQuery, targetModel, transaction);
                                }
                                else
                                {
                                    if (target.StartDate.Year == DateTime.Now.Year &&
                                        target.StartDate.Month == DateTime.Now.Month)
                                    {
                                        if (target.Revenue * 1000000000 != item.Revenue)
                                        {
                                            db.Execute(UpdateRevenueQuery, new
                                            {
                                                @Revenue = item.Revenue * 1000000000,
                                                @Id = target.Id
                                            }, transaction);
                                        }
                                    }
                                    else
                                    {
                                        db.Execute(InsertTargetQuery, targetModel, transaction);
                                        db.Execute(DisableTargetQuery, new
                                        {
                                            @EndDate = targetModel.StartDate.AddDays(-1).Date,
                                            @Id = target.Id
                                        }, transaction);
                                    }
                                }
                            }
                        }
                        transaction.Commit();
                    }
                }

                return new DataResult<SaleTargetViewModel>();
            }
            catch (Exception ex)
            {
                return new DataResult<SaleTargetViewModel> { Error = true };
            }
        }
        #endregion

        #region GetBranchRevenueTargetList
        public DataResult<RevenueTarget> GetBranchRevenueTargetList(DateTime date) 
        {
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var getResult = db.QueryMultiple("sp_tblTarget_GetRevenueTarget_For_Branches",
                                new
                                {
                                    @Date = date,
                                }, commandType: CommandType.StoredProcedure))
                    {
                        var target = getResult.Read<RevenueTarget>().ToList();
                        return new DataResult<RevenueTarget> { Result = target };
                    }
                }
            }
            catch (Exception ex)
            {
                return new DataResult<RevenueTarget> { Error = true };
            }
        }
        #endregion

        #region GetOfficeRevenueTargetList
        public DataResult<RevenueTarget> GetOfficeRevenueTargetList(DateTime date, string branch)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var getResult = db.QueryMultiple("sp_tblTarget_GetRevenueTarget_For_Offices",
                                new
                                {
                                    @Date = date,
                                    @BranchCode = branch
                                }, commandType: CommandType.StoredProcedure))
                    {
                        var target = getResult.Read<RevenueTarget>().ToList();
                        return new DataResult<RevenueTarget> { Result = target };
                    }
                }
            }
            catch (Exception ex)
            {
                return new DataResult<RevenueTarget> { Error = true };
            }
        }
        #endregion

        #region GetDepartmentRevenueTargetList
        public DataResult<RevenueTarget> GetDepartmentRevenueTargetList(DateTime date, string office)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var getResult = db.QueryMultiple("sp_tblTarget_GetRevenueTarget_For_Departments",
                                new
                                {
                                    @Date = date,
                                    @OfficeCode = office
                                }, commandType: CommandType.StoredProcedure))
                    {
                        var target = getResult.Read<RevenueTarget>().ToList();
                        return new DataResult<RevenueTarget> { Result = target };
                    }
                }
            }
            catch (Exception ex)
            {
                return new DataResult<RevenueTarget> { Error = true };
            }
        }
        #endregion

        #region GetTeamRevenueTargetList
        public DataResult<RevenueTarget> GetTeamRevenueTargetList(DateTime date, string department)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var getResult = db.QueryMultiple("sp_tblTarget_GetRevenueTarget_For_Teams",
                                new
                                {
                                    @Date = date,
                                    @DepartmentCode = department
                                }, commandType: CommandType.StoredProcedure))
                    {
                        var target = getResult.Read<RevenueTarget>().ToList();
                        return new DataResult<RevenueTarget> { Result = target };
                    }
                }
            }
            catch (Exception ex)
            {
                return new DataResult<RevenueTarget> { Error = true };
            }
        }
        #endregion

        #region GetSaleRevenueTargetList
        public DataResult<RevenueTarget> GetSaleRevenueTargetList(DateTime date, string team)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var getResult = db.QueryMultiple("sp_tblTarget_GetRevenueTarget_For_Sales",
                                new
                                {
                                    @Date = date,
                                    @TeamCode = team
                                }, commandType: CommandType.StoredProcedure))
                    {
                        var target = getResult.Read<RevenueTarget>().ToList();
                        return new DataResult<RevenueTarget> { Result = target };
                    }
                }
            }
            catch (Exception ex)
            {
                return new DataResult<RevenueTarget> { Error = true };
            }
        }
        #endregion
    }
}