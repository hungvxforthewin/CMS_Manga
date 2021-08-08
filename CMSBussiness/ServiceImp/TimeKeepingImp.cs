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
    public class TimeKeepingImp : ITimeKeeping
    {
        #region Const
        public const char Slash = '/';
        public static string _currentMonth = string.Empty + DateTime.Now.Year + Slash + (DateTime.Now.Month < 10 ? "0" : string.Empty) + DateTime.Now.Month;
        private const string InsertTimekeepingForSale = @"
                                DECLARE @@t TABLE (CodeStaff varchar(50), Amount decimal(18, 0));
                                INSERT INTO @@t(CodeStaff, Amount) 
                                EXEC sp_RevenueSaleWithCodeStaff @Month;

                                INSERT INTO tblTimeKeeping(CodeKeeping, Month, CodeStaff, TotalWorkingDays,  TotalLates, TotalEarlyOuts, TotalWithoutReason, ForgetCheckOutIn, TotalShowupInMonth, TotalContract, RevenueInMonth, TotalTakeLeaveInMonth) 
                              SELECT @CodeKeeping, @Month, @CodeStaff, @TotalWorkingDays, @TotalLates, @TotalEarlyOuts, @TotalWithoutReason, @ForgetCheckOutIn, NULL, NULL, Amount, @TotalTakeLeaveInMonth
                                FROM @@t
                                WHERE CodeStaff = @CodeStaff";

        private const string InsertTimekeepingForTele = @"
                                INSERT INTO tblTimeKeeping(CodeKeeping, Month, CodeStaff, TotalWorkingDays, TotalLates, TotalEarlyOuts, TotalWithoutReason, ForgetCheckOutIn, TotalShowupInMonth, TotalContract, RevenueInMonth, TotalTakeLeaveInMonth) VALUES (@CodeKeeping, @Month, @CodeStaff, @TotalWorkingDays, @TotalLates, @TotalEarlyOuts, @TotalWithoutReason, @ForgetCheckOutIn, dbo.Fc_TeleSale_CollectShowUp(@Month, @CodeStaff), dbo.Fc_TeleSale_CollectContract(@Month, @CodeStaff), NULL, @TotalTakeLeaveInMonth)";
        private const string InsertTimekeepingForOtherRole = @"
                                INSERT INTO tblTimeKeeping(CodeKeeping, Month, CodeStaff, TotalWorkingDays, TotalLates, TotalEarlyOuts, TotalWithoutReason, ForgetCheckOutIn, TotalShowupInMonth, TotalContract, RevenueInMonth, TotalTakeLeaveInMonth) VALUES (@CodeKeeping, @Month, @CodeStaff, @TotalWorkingDays, @TotalLates, @TotalEarlyOuts, @TotalWithoutReason, @ForgetCheckOutIn, NULL, NULL, NULL, @TotalTakeLeaveInMonth)";
        #endregion

        #region GetInfoByMonthAndKey - khanhkk
        /// <summary>
        /// Get timekeeping info by month or search key
        /// </summary>
        /// <param name="month">input mont</param>
        /// <param name="key">input search key</param>
        /// <returns>List of timekeepings</returns>
        public DataResult<DisplayTimeKeepingViewmodel> GetInfoByMonthAndKey(string month, string key/*, int size = 10, int pages = 5, int startRow = 1*/)
        {
            //set default value if user don't choose a month in year
            if (string.IsNullOrEmpty(month))
            {
                month = _currentMonth;
            }

            DisplayTimeKeepingViewmodel Data = new DisplayTimeKeepingViewmodel();
            var param = new DynamicParameters();
            param.Add("month", month);
            param.Add("key", key);
            param.Add("totalDays", dbType: DbType.Byte, direction: ParameterDirection.Output);
            param.Add("bonus", dbType: DbType.Decimal, direction: ParameterDirection.Output);
            // get info from db
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("sp_tblTimeKeeping_GetInfoByMonth",
                        param, commandType: CommandType.StoredProcedure))
                    {
                        Data.TimeKeepings = multipleresult.Read<TimeKeepingViewModel>().ToList();
                    }
                    Data.TotalDaysInMonth = param.Get<byte?>("totalDays");
                    Data.Bonus = param.Get<decimal?>("bonus");
                }

                return new DataResult<DisplayTimeKeepingViewmodel> { DataItem = Data };
            }
            catch
            {
                return new DataResult<DisplayTimeKeepingViewmodel> { Error = true };
            }
        }
        #endregion

        #region AddTimeKeeping - khanhkk
        /// <summary>
        /// Input timekeeping info for all employee
        /// </summary>
        /// <param name="models">timekeeping info</param>
        /// <returns>true or false</returns>
        public DataResult<TimeKeepingViewModel> AddTimeKeeping(List<TimeKeepingViewModel> model)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        //string insertMonthQuery = "INSERT INTO tblRuleInMonth(Month, TotalWorkingDays) VALUES (@Month, @TotalDaysInMonth)";
                        //db.Execute(insertMonthQuery, new { @Month = model.TimeKeepings.ElementAt(0).Month, @TotalDaysInMonth = model.TotalDaysInMonth }, transaction);

                        foreach (var item in model)
                        {
                            if (item.RoleAccount == 8 || item.RoleAccount == 9)
                            {
                                item.RevenueInMonth = null;
                            }
                            else
                            {
                                item.TotalContract = null;
                                item.TotalShowupInMonth = null;
                            }

                            item.CodeKeeping = Guid.NewGuid().ToString();
                            string processQuery = string.Empty;
                            switch (item.RoleAccount)
                            {
                                case 8:
                                case 9:
                                    processQuery = InsertTimekeepingForTele;
                                    break;

                                case 4:
                                case 5:
                                case 6:
                                case 10:
                                case 11:
                                    processQuery = InsertTimekeepingForSale;
                                    break;

                                case 7:
                                    processQuery = InsertTimekeepingForOtherRole;
                                    break;
                            }
                            db.Execute(processQuery, item, transaction);
                        }
                        transaction.Commit();
                    }

                }

                return new DataResult<TimeKeepingViewModel>();
            }
            catch (Exception ex)
            {
                return new DataResult<TimeKeepingViewModel> { Error = true };
            }
        }
        #endregion

        #region GetTimeKeepingOfEmployee - khanhkk
        /// <summary>
        /// Get timekeeping info by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>timekeeping info</returns>
        public DataResult<TimeKeepingViewModel> GetTimeKeepingOfEmployee(long id)
        {
            // get info from db
            List<TimeKeepingViewModel> Ac = new List<TimeKeepingViewModel>();
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("sp_tblTimeKeeping_GetInfoById",
                        new { @id = id },
                        commandType: CommandType.StoredProcedure))
                    {
                        Ac = multipleresult.Read<TimeKeepingViewModel>().ToList();
                    }
                }

                return new DataResult<TimeKeepingViewModel> { Result = Ac };
            }
            catch
            {
                return new DataResult<TimeKeepingViewModel> { Error = true };
            }
        }
        #endregion

        #region UpdateTimeKeepingOfEmployee - khanhkk
        /// <summary>
        /// update timekeeping for an employee
        /// </summary>
        /// <param name="model">timekeeping info</param>
        /// <returns>true or false</returns>
        public DataResult<TimeKeepingViewModel> UpdateTimeKeepingOfEmployee(TimeKeepingViewModel model)
        {
            if (model.RoleAccount == 8 || model.RoleAccount == 9)
            {
                model.RevenueInMonth = null;
            }
            else
            {
                model.TotalContract = null;
                model.TotalShowupInMonth = null;
            }

            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    string processQuery = @"UPDATE tblTimeKeeping SET TotalWorkingDays = @TotalWorkingDays, TotalLates = @TotalLates, TotalEarlyOuts = @TotalEarlyOuts, TotalWithoutReason = @TotalWithoutReason, ForgetCheckOutIn = @ForgetCheckOutIn, TotalShowupInMonth = @TotalShowupInMonth, TotalContract = @TotalContract, RevenueInMonth = @RevenueInMonth, TotalTakeLeaveInMonth = @TotalTakeLeaveInMonth WHERE Id = @Id";
                    db.Execute(processQuery, model);
                }

                return new DataResult<TimeKeepingViewModel>();
            }
            catch
            {
                return new DataResult<TimeKeepingViewModel> { Error = true };
            }
        }
        #endregion

        #region CheckExistedTimekeepingInMonth - khanhkk
        public DataResult<bool> CheckExistedTimekeepingInMonth(string month, string staffCode)
        {
            bool result = false;
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("sp_tblTimeKeeping_CheckExistedInfo",
                        new { @month = month, @staffCode = staffCode },
                        commandType: CommandType.StoredProcedure))
                    {
                        TimeKeepingViewModel data = multipleresult.Read<TimeKeepingViewModel>().FirstOrDefault();
                        if (data != null)
                        {
                            result = true;
                        }
                    }
                }

                return new DataResult<bool> { Result = new List<bool> { result } };
            }
            catch
            {
                return new DataResult<bool> { Error = true };
            }
        }
        #endregion

        #region UpdateOtherBonus - khanhkk
        public DataResult<TimeKeepingViewModel> UpdateOtherBonus(decimal? bonus, long id)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    string processQuery = "UPDATE tblTimeKeeping SET OtherBonus = @Bonus WHERE Id = @Id";
                    db.Execute(processQuery, new { @Bonus = bonus, @Id = id });
                }

                return new DataResult<TimeKeepingViewModel>();
            }
            catch (Exception ex)
            {
                return new DataResult<TimeKeepingViewModel> { Error = true };
            }
        }
        #endregion

        #region ImportTimeKeeping - khanhkk
        /// <summary>
        /// Import timekeeping info for all employee
        /// </summary>
        /// <param name="model">timekeeping info</param>
        /// <returns>true or false</returns>
        public DataResult<TimeKeepingViewModel> ImportTimeKeeping(List<TimeKeepingViewModel> model)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        foreach (var item in model)
                        {
                            if (item.RoleAccount == 8 || item.RoleAccount == 9)
                            {
                                item.RevenueInMonth = null;
                            }
                            else
                            {
                                item.TotalContract = null;
                                item.TotalShowupInMonth = null;
                            }

                            TimeKeepingViewModel data = null;
                            using (var multipleresult = db.QueryMultiple("sp_tblTimeKeeping_GetInfoByMonthAndStaffCode",
                            new { @month = item.Month, @staffCode = item.CodeStaff }, transaction,
                            commandType: CommandType.StoredProcedure))
                            {
                                data = multipleresult.Read<TimeKeepingViewModel>().FirstOrDefault();
                            }
                            if (data == null)
                            {
                                item.CodeKeeping = Guid.NewGuid().ToString();
                                string processQuery = string.Empty;
                                switch (item.RoleAccount)
                                {
                                    case 8:
                                    case 9:
                                        processQuery = InsertTimekeepingForTele;
                                        break;

                                    case 4:
                                    case 5:
                                    case 6:
                                    case 10:
                                    case 11:
                                        processQuery = InsertTimekeepingForSale;
                                        break;

                                    case 7:
                                        processQuery = InsertTimekeepingForOtherRole;
                                        break;
                                }

                                db.Execute(processQuery, item, transaction);
                            }
                            //else
                            //{
                            //    item.Id = data.Id;
                            //    string processQuery = "UPDATE tblTimeKeeping SET TotalWorkingDays = @TotalWorkingDays, TotalLates = @TotalLates, TotalEarlyOuts = @TotalEarlyOuts, TotalWithoutReason = @TotalWithoutReason, ForgetCheckOutIn = @ForgetCheckOutIn, TotalShowupInMonth = @TotalShowupInMonth, TotalContract = @TotalContract, RevenueInMonth = @RevenueInMonth, TotalTakeLeaveInMonth = @TotalTakeLeaveInMonth WHERE Id = @Id";
                            //    db.Execute(processQuery, item, transaction);
                            //}

                        }
                        transaction.Commit();
                    }

                }

                return new DataResult<TimeKeepingViewModel>();
            }
            catch (Exception ex)
            {
                return new DataResult<TimeKeepingViewModel> { Error = true };
            }
        }
        #endregion

        #region AddNewTimeKeeping
        public DataResult<TimeKeepingViewModel> AddNewTimeKeeping(TimeKeepingViewModel model)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        if (model.RoleAccount == 8 || model.RoleAccount == 9)
                        {
                            model.RevenueInMonth = null;
                        }
                        else
                        {
                            model.TotalContract = null;
                            model.TotalShowupInMonth = null;
                        }

                        //model.CodeKeeping = Guid.NewGuid().ToString();
                        string processQuery = string.Empty;
                        switch (model.RoleAccount)
                        {
                            case 8:
                            case 9:
                                processQuery = InsertTimekeepingForTele;
                                break;

                            case 4:
                            case 5:
                            case 6:
                            case 10:
                            case 11:
                                processQuery = InsertTimekeepingForSale;
                                break;

                            case 7:
                                processQuery = InsertTimekeepingForOtherRole;
                                break;
                        }
                        db.Execute(processQuery, model, transaction);
                        transaction.Commit();
                    }
                }

                return new DataResult<TimeKeepingViewModel>();
            }
            catch (Exception ex)
            {
                return new DataResult<TimeKeepingViewModel> { Error = true };
            }
        }
        #endregion

        #region UpdateTimeKeeping - khanhkk
        /// <summary>
        /// update timekeeping for an employee
        /// </summary>
        /// <param name="model">timekeeping info</param>
        /// <returns>true or false</returns>
        public DataResult<TimeKeepingViewModel> UpdateTimeKeeping(TimeKeepingViewModel model)
        {
            if (model.RoleAccount == 8 || model.RoleAccount == 9)
            {
                model.RevenueInMonth = null;
            }
            else
            {
                model.TotalContract = null;
                model.TotalShowupInMonth = null;
            }

            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    string processQuery = @"UPDATE tblTimeKeeping SET TotalWorkingDays = @TotalWorkingDays, TotalLates = @TotalLates, TotalEarlyOuts = @TotalEarlyOuts, TotalWithoutReason = @TotalWithoutReason, ForgetCheckOutIn = @ForgetCheckOutIn, TotalShowupInMonth = @TotalShowupInMonth, TotalContract = @TotalContract, RevenueInMonth = @RevenueInMonth, TotalTakeLeaveInMonth = @TotalTakeLeaveInMonth WHERE CodeKeeping = @CodeKeeping";
                    db.Execute(processQuery, model);
                }

                return new DataResult<TimeKeepingViewModel>();
            }
            catch
            {
                return new DataResult<TimeKeepingViewModel> { Error = true };
            }
        }
        #endregion
    }
}