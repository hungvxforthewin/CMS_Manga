using CRMBussiness.IService;
using CRMBussiness.ViewModel;
using CRMModel.Models.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CRMBussiness.ServiceImp
{
    public class SetupSalaryElementsImp2 :  ISetupSalaryElements2
    {
        #region Const
        private const byte SaleRole = 4; 
        private const byte LeaderSaleRole = 5;
        private const byte TeleRole = 8; 
        private const byte LeaderTeleRole = 9; 
        private const byte MinisterRole = 10; 

        private const string TeleInsertQuery = "INSERT INTO tblLevelSalaryRevenue(RoleAccount, Salary, PercentRemuneration, RemunerationShowUp, RemunerationContractTele, PercentKpiMin, PercentKpiMax, CodeKpi, CreateDate, Status, TimeKpi, ProbationarySalary, SalaryPercentLv1, ProbationaryTime) VALUES (@RoleAccount, @Salary, @PercentRemuneration, @RemunerationShowUp, @RemunerationContractTele, @PercentKpiMin, @PercentKpiMax, @CodeKpi, @CreateDate, @Status, @TimeKpi, @ProbationarySalary, @SalaryPercentLv1, @ProbationaryTime)";

        private const string SaleInsertQuery = "INSERT INTO tblLevelSalaryRevenue(RoleAccount, Salary, PercentRemuneration,  PercentKpiMin, PercentKpiMax, CodeKpi, CreateDate, Status, TimeKpi, ProbationarySalary, SalaryPercentLv1, ProbationaryTime, RevenueMin, RevenueMax, SharePercent) VALUES (@RoleAccount, @Salary, @PercentRemuneration, @PercentKpiMin, @PercentKpiMax, @CodeKpi, @CreateDate, @Status, @TimeKpi, @ProbationarySalary, @SalaryPercentLv1, @ProbationaryTime, @RevenueMin, @RevenueMax, @SharePercent)";

        private const string InsertKpiQuery = "INSERT INTO tblKpi(CodeKpi, KpiName, RoleAccount, TypeKpi, TotalShowUp, Revenue, TypeContract, Status) VALUES (@CodeKpi, @KpiName, @RoleAccount, @TypeKpi, @TotalShowUp, @Revenue, 0, 1)";

        private const string UpdateKpiQuery = "UPDATE tblKpi SET TotalShowUp = @TotalShowUp, Revenue = @Revenue WHERE CodeKpi = @CodeKpi";

        private const string DeleteDataQuery = "DELETE FROM tblLevelSalaryRevenue WHERE RoleAccount = @RoleAccount";
        #endregion

        #region GetSetupSalaryForTele - khanhkk
        public DataResult<SalaryMechanismViewModel> GetSetupSalaryForTele()
        {
            List<SalaryMechanismViewModel> data = new List<SalaryMechanismViewModel>();
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    //TeleSale
                    //common info
                    TeleSaleCommon common = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetCommonInfo_ForTeleSale",
                        new { @role = TeleRole }, commandType: CommandType.StoredProcedure))
                    {
                        common = multipleresult.Read<TeleSaleCommon>().FirstOrDefault();
                    }
                    if (common == null) return new DataResult<SalaryMechanismViewModel>();

                    // kpi salary
                    Level1Condition probationary = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetProbationaryCondition_ForTeleSale",
                        new { @role = TeleRole }, commandType: CommandType.StoredProcedure))
                    {
                        probationary = multipleresult.Read<Level1Condition>().FirstOrDefault();
                    }

                    //Remuneration
                    List<TeleSaleRemuneration> remunerations = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetRemunerations_ForTeleSale",
                        new { @role = TeleRole }, commandType: CommandType.StoredProcedure))
                    {
                        remunerations = multipleresult.Read<TeleSaleRemuneration>().ToList();
                    }

                    TeleSaleSalaryMechanism teleSaleSalary = new TeleSaleSalaryMechanism
                    {
                        Common = common,
                        ProbationaryCondition = probationary,
                        Remunerations = remunerations,
                    };

                    //Leader telesale
                    //common info
                    TeleSaleCommon leaderCommon = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetCommonInfo_ForTeleSale",
                        new { @role = LeaderTeleRole }, commandType: CommandType.StoredProcedure))
                    {
                        leaderCommon = multipleresult.Read<TeleSaleCommon>().FirstOrDefault();
                    }

                    // kpi salary
                    Level1Condition leaderProbationary = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetProbationaryCondition_ForTeleSale",
                        new { @role = LeaderTeleRole }, commandType: CommandType.StoredProcedure))
                    {
                        leaderProbationary = multipleresult.Read<Level1Condition>().FirstOrDefault();
                    }

                    //Remuneration
                    List<TeleSaleRemuneration> leaderRemunerations = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetRemunerations_ForTeleSale",
                        new { @role = LeaderTeleRole }, commandType: CommandType.StoredProcedure))
                    {
                        leaderRemunerations = multipleresult.Read<TeleSaleRemuneration>().ToList();
                    }

                    TeleSaleSalaryMechanism leaderTeleSaleSalary = new TeleSaleSalaryMechanism
                    {
                        Common = leaderCommon,
                        ProbationaryCondition = leaderProbationary,
                        Remunerations = leaderRemunerations,
                    };

                    data.Add(new MechanismForTele
                    {
                        TeleSaleMachanism = teleSaleSalary,
                        LeaderTeleSaleMachanism = leaderTeleSaleSalary
                    });
                }

                return new DataResult<SalaryMechanismViewModel> { Result = data };
            }
            catch(Exception ex)
            {
                return new DataResult<SalaryMechanismViewModel> { Error = true };
            }
        }
        #endregion

        #region GetSetupSalaryForSale - khanhkk
        public DataResult<SalaryMechanismViewModel> GetSetupSalaryForSale()
        {
            List<SalaryMechanismViewModel> data = new List<SalaryMechanismViewModel>();
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    //TeleSale
                    // first condition
                    Level1Condition firstCondition = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetMonthsCondition_ForSale",
                        new { @role = SaleRole, @type = 0 }, commandType: CommandType.StoredProcedure))
                    {
                        firstCondition = multipleresult.Read<Level1Condition>().FirstOrDefault();
                    }
                    if (firstCondition == null) return new DataResult<SalaryMechanismViewModel>(); ;

                    // later condition
                    Level1Condition laterCondition = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetMonthsCondition_ForSale",
                        new { @role = SaleRole, @type = 1 }, commandType: CommandType.StoredProcedure))
                    {
                        laterCondition = multipleresult.Read<Level1Condition>().FirstOrDefault();
                    }

                    // first condition
                    Level1Condition leaderFirstCondition = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetMonthsCondition_ForSale",
                        new { @role = LeaderSaleRole, @type = 0 }, commandType: CommandType.StoredProcedure))
                    {
                        leaderFirstCondition = multipleresult.Read<Level1Condition>().FirstOrDefault();
                    }

                    // later condition
                    Level1Condition leaderLaterCondition = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetMonthsCondition_ForSale",
                        new { @role = LeaderSaleRole, @type = 1 }, commandType: CommandType.StoredProcedure))
                    {
                        leaderLaterCondition = multipleresult.Read<Level1Condition>().FirstOrDefault();
                    }

                    //salary
                    List<KpiSalary> firstSalary = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetMonthsSalary_ForSale",
                        new { @role = SaleRole, @type = 0 }, commandType: CommandType.StoredProcedure))
                    {
                        firstSalary = multipleresult.Read<KpiSalary>().ToList();
                    }

                    List<KpiSalary> laterSalary = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetMonthsSalary_ForSale",
                        new { @role = SaleRole, @type = 1 }, commandType: CommandType.StoredProcedure))
                    {
                        laterSalary = multipleresult.Read<KpiSalary>().ToList();
                    }

                    // leader salary
                    List<KpiSalary> leaderFirstSalary = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetMonthsSalary_ForSale",
                        new { @role = LeaderSaleRole, @type = 0 }, commandType: CommandType.StoredProcedure))
                    {
                        leaderFirstSalary = multipleresult.Read<KpiSalary>().ToList();
                    }

                    List<KpiSalary> leaderLaterSalary = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetMonthsSalary_ForSale",
                        new { @role = LeaderSaleRole, @type = 1 }, commandType: CommandType.StoredProcedure))
                    {
                        leaderLaterSalary = multipleresult.Read<KpiSalary>().ToList();
                    }

                    //remunerations
                    List<SaleRemurationLevel> saleRemurations = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetRemunerations_ForSale",
                        new { @role = SaleRole }, commandType: CommandType.StoredProcedure))
                    {
                        saleRemurations = multipleresult.Read<SaleRemurationLevel>().ToList();
                    }

                    List<SaleRemurationLevel> leaderSaleRemurations = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetRemunerations_ForSale",
                        new { @role = LeaderSaleRole }, commandType: CommandType.StoredProcedure))
                    {
                        leaderSaleRemurations = multipleresult.Read<SaleRemurationLevel>().ToList();
                    }

                    SaleSalaryMechanism saleMechanism = new SaleSalaryMechanism
                    {
                        Id = firstCondition.Id,
                        CodeKpi = firstCondition.CodeKpi,
                        FirstMonthsCondition = firstCondition,
                        FirstMonthsSalary = firstSalary,
                        LaterMonthsCondition = laterCondition,
                        LaterMonthsSalary = laterSalary,
                        Remunerations = saleRemurations,
                    };

                    SaleSalaryMechanism leaderSaleMechanism = new SaleSalaryMechanism
                    {
                        Id = leaderFirstCondition.Id,
                        CodeKpi = leaderFirstCondition.CodeKpi,
                        FirstMonthsCondition = leaderFirstCondition,
                        FirstMonthsSalary = leaderFirstSalary,
                        LaterMonthsCondition = leaderLaterCondition,
                        LaterMonthsSalary = leaderLaterSalary,
                        Remunerations = leaderSaleRemurations,
                    };

                    data.Add(new MechanismForSale
                    {
                        SaleMechanism = saleMechanism,
                        LeaderSaleMechanism = leaderSaleMechanism,
                    });
                }

                return new DataResult<SalaryMechanismViewModel> { Result = data };
            }
            catch(Exception ex)
            {
                return new DataResult<SalaryMechanismViewModel> { Error = true };
            }
        }
        #endregion

        #region GetSetupSalaryForMinister - khanhkk
        public DataResult<SalaryMechanismViewModel> GetSetupSalaryForMinister()
        {
            List<SalaryMechanismViewModel> data = new List<SalaryMechanismViewModel>();
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    //TeleSale
                    // first condition
                    Level1Condition firstCondition = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetMonthsCondition_ForSale",
                        new { @role = MinisterRole, @type = 0 }, commandType: CommandType.StoredProcedure))
                    {
                        firstCondition = multipleresult.Read<Level1Condition>().FirstOrDefault();
                    }
                    if (firstCondition == null) return new DataResult<SalaryMechanismViewModel>(); ;

                    // later condition
                    Level1Condition laterCondition = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetMonthsCondition_ForSale",
                        new { @role = MinisterRole, @type = 1 }, commandType: CommandType.StoredProcedure))
                    {
                        laterCondition = multipleresult.Read<Level1Condition>().FirstOrDefault();
                    }

                    //salary
                    List<KpiSalary> firstSalary = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetMonthsSalary_ForSale",
                        new { @role = MinisterRole, @type = 0 }, commandType: CommandType.StoredProcedure))
                    {
                        firstSalary = multipleresult.Read<KpiSalary>().ToList();
                    }

                    List<KpiSalary> laterSalary = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetMonthsSalary_ForSale",
                        new { @role = MinisterRole, @type = 1 }, commandType: CommandType.StoredProcedure))
                    {
                        laterSalary = multipleresult.Read<KpiSalary>().ToList();
                    }

                    //remunerations
                    List<SaleRemurationLevel> saleRemurations = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblLevelSalaryRevenue_GetRemunerations_ForSale",
                        new { @role = MinisterRole }, commandType: CommandType.StoredProcedure))
                    {
                        saleRemurations = multipleresult.Read<SaleRemurationLevel>().ToList();
                    }

                    SaleSalaryMechanism ministerMechanism = new SaleSalaryMechanism
                    {
                        Id = firstCondition.Id,
                        CodeKpi = firstCondition.CodeKpi,
                        FirstMonthsCondition = firstCondition,
                        FirstMonthsSalary = firstSalary,
                        LaterMonthsCondition = laterCondition,
                        LaterMonthsSalary = laterSalary,
                        Remunerations = saleRemurations,
                    };

                    data.Add(new MechanismForMinister
                    {
                        MinisterMechanism = ministerMechanism,
                    });
                }

                return new DataResult<SalaryMechanismViewModel> { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<SalaryMechanismViewModel> { Error = true };
            }
        }
        #endregion

        #region SetupSalaryElementsForTele - khanhkk
        public DataResult<SalaryMechanismViewModel> SetupSalaryElementsForTele(SalaryMechanismViewModel model)
        {
            MechanismForTele teleModel = model as MechanismForTele;

            if (teleModel != null && teleModel.TeleSaleMachanism != null && teleModel.LeaderTeleSaleMachanism != null)
            {
                //create new
                if (string.IsNullOrEmpty(teleModel.TeleSaleMachanism.Common.CodeKpi))
                {
                    try
                    {
                        using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                        {
                            db.Open();
                            using (var transaction = db.BeginTransaction())
                            {
                                // TELESALE
                                //kpi
                                if (teleModel.TeleSaleMachanism.Common != null)
                                {
                                    teleModel.TeleSaleMachanism.Common.CodeKpi = Guid.NewGuid().ToString();
                                    teleModel.TeleSaleMachanism.Common.RoleAccount = TeleRole;
                                    Kpi kpi = new Kpi
                                    {
                                        CodeKpi = teleModel.TeleSaleMachanism.Common.CodeKpi,
                                        TotalShowUp = teleModel.TeleSaleMachanism.Common.Kpi,
                                        RoleAccount = TeleRole,
                                        TypeKpi = 1,
                                        Status = true,
                                        KpiName = "Kpi for tele"
                                    };
                                    db.Execute(InsertKpiQuery, kpi, transaction);
                                }

                                if (teleModel.TeleSaleMachanism.Remunerations != null && teleModel.TeleSaleMachanism.Common != null)
                                {
                                    AddRemunerations(teleModel.TeleSaleMachanism.Remunerations, teleModel.TeleSaleMachanism, db, transaction);
                                }

                                //TELE LEADER
                                //kpi
                                if (teleModel.LeaderTeleSaleMachanism.Common != null)
                                {
                                    teleModel.LeaderTeleSaleMachanism.Common.CodeKpi = Guid.NewGuid().ToString();
                                    teleModel.LeaderTeleSaleMachanism.Common.RoleAccount = LeaderTeleRole;
                                    Kpi leaderKpi = new Kpi
                                    {
                                        CodeKpi = teleModel.LeaderTeleSaleMachanism.Common.CodeKpi,
                                        TotalShowUp = teleModel.LeaderTeleSaleMachanism.Common.Kpi,
                                        RoleAccount = LeaderTeleRole,
                                        TypeKpi = 1,
                                        Status = true,
                                        KpiName = "Kpi for leader tele"
                                    };
                                    db.Execute(InsertKpiQuery, leaderKpi, transaction);
                                }

                                if (teleModel.LeaderTeleSaleMachanism.Remunerations != null && teleModel.LeaderTeleSaleMachanism.Common != null)
                                {
                                    AddRemunerations(teleModel.LeaderTeleSaleMachanism.Remunerations, teleModel.LeaderTeleSaleMachanism, db, transaction);
                                }

                                transaction.Commit();
                            }

                        }

                        return new DataResult<SalaryMechanismViewModel> ();
                    }
                    catch(Exception ex)
                    {
                        return new DataResult<SalaryMechanismViewModel> { Error = true };
                    }
                }
                // update
                else
                {
                    try
                    {
                        using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                        {
                            db.Open();
                            using (var transaction = db.BeginTransaction())
                            {
                                // TELESALE
                                //kpi
                                if (teleModel.TeleSaleMachanism.Common != null)
                                {
                                    teleModel.TeleSaleMachanism.Common.RoleAccount = TeleRole;
                                    Kpi kpi = new Kpi
                                    {
                                        CodeKpi = teleModel.TeleSaleMachanism.Common.CodeKpi,
                                        TotalShowUp = teleModel.TeleSaleMachanism.Common.Kpi,
                                    };
                                    db.Execute(UpdateKpiQuery, kpi, transaction);
                                }

                                //delete old data
                                db.Execute(DeleteDataQuery, new { @RoleAccount = TeleRole }, transaction);

                                if (teleModel.TeleSaleMachanism.Remunerations != null && teleModel.TeleSaleMachanism.Common != null)
                                {
                                    AddRemunerations(teleModel.TeleSaleMachanism.Remunerations, teleModel.TeleSaleMachanism, db, transaction);
                                }

                                //TELE LEADER
                                //kpi
                                if (teleModel.LeaderTeleSaleMachanism.Common != null)
                                {
                                    teleModel.LeaderTeleSaleMachanism.Common.RoleAccount = LeaderTeleRole;
                                    Kpi leaderKpi = new Kpi
                                    {
                                        CodeKpi = teleModel.LeaderTeleSaleMachanism.Common.CodeKpi,
                                        TotalShowUp = teleModel.LeaderTeleSaleMachanism.Common.Kpi,
                                    };
                                    db.Execute(UpdateKpiQuery, leaderKpi, transaction);
                                }

                                // delete old data 
                                db.Execute(DeleteDataQuery, new { @RoleAccount = LeaderTeleRole }, transaction);

                                if (teleModel.LeaderTeleSaleMachanism.Remunerations != null && teleModel.LeaderTeleSaleMachanism.Common != null)
                                {
                                    AddRemunerations(teleModel.LeaderTeleSaleMachanism.Remunerations, teleModel.LeaderTeleSaleMachanism, db, transaction);
                                }

                                transaction.Commit();
                            }

                        }

                        return new DataResult<SalaryMechanismViewModel>();
                    }
                    catch (Exception ex)
                    {
                        return new DataResult<SalaryMechanismViewModel> { Error = true };
                    }
                }
            }
            return new DataResult<SalaryMechanismViewModel> { Error = true };
        }
        #endregion

        #region SetupSalaryElementsForSale - khanhkk
        public DataResult<SalaryMechanismViewModel> SetupSalaryElementsForSale(SalaryMechanismViewModel model)
        {
            MechanismForSale saleModel = model as MechanismForSale;
            if (saleModel != null && saleModel.SaleMechanism != null && saleModel.LeaderSaleMechanism != null)
            {
                //create new
                if (saleModel.SaleMechanism.Id == 0)
                {
                    try
                    {
                        using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                        {
                            db.Open();
                            using (var transaction = db.BeginTransaction())
                            {
                                //SALE
                                //kpi
                                if (saleModel.SaleMechanism.Remunerations != null && saleModel.SaleMechanism.Remunerations.Count > 0)
                                {
                                    saleModel.SaleMechanism.CodeKpi = Guid.NewGuid().ToString();
                                    Kpi kpi = new Kpi
                                    {
                                        CodeKpi = saleModel.SaleMechanism.CodeKpi,
                                        Revenue = saleModel.SaleMechanism.Remunerations.First().RevenueMax,
                                        RoleAccount = SaleRole,
                                        TypeKpi = 2,
                                        Status = true,
                                        KpiName = "Kpi for sale"
                                    };
                                    db.Execute(InsertKpiQuery, kpi, transaction);
                                }

                                //kpi condition for first months
                                if (saleModel.SaleMechanism.FirstMonthsCondition != null && saleModel.SaleMechanism.FirstMonthsCondition.KpiPercent != null && saleModel.SaleMechanism.FirstMonthsCondition.SalaryPercent != null)
                                {
                                    LevelSalaryRevenue condition = new LevelSalaryRevenue
                                    {
                                        CodeKpi = saleModel.SaleMechanism.CodeKpi,
                                        PercentKpiMin = 0,
                                        PercentKpiMax = saleModel.SaleMechanism.FirstMonthsCondition.KpiPercent,
                                        SalaryPercentLv1 = saleModel.SaleMechanism.FirstMonthsCondition.SalaryPercent,
                                        CreateDate = DateTime.Now,
                                        RoleAccount = SaleRole,
                                        Salary = saleModel.SaleMechanism.Remunerations.First().Salary,
                                        Status = true,
                                        PercentRemuneration = saleModel.SaleMechanism.Remunerations.First().PercentRemuneration,
                                        SharePercent = saleModel.SaleMechanism.Remunerations.First().CalculatingSharePercent,
                                        TimeKpi = 0,
                                        RevenueMin = 0,
                                        RevenueMax = saleModel.SaleMechanism.Remunerations.First().RevenueMin,
                                    };
                                    db.Execute(SaleInsertQuery, condition, transaction);
                                }

                                //kpi salary for first months
                                if (saleModel.SaleMechanism.FirstMonthsSalary != null)
                                {
                                    foreach (var item in saleModel.SaleMechanism.FirstMonthsSalary)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.SaleMechanism.CodeKpi,
                                            PercentKpiMin = item.MinKpiPercent,
                                            PercentKpiMax = item.MaxKpiPercent,
                                            SalaryPercentLv1 = item.SalaryPercent,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = SaleRole,
                                            Salary = saleModel.SaleMechanism.Remunerations.First().Salary,
                                            Status = true,
                                            PercentRemuneration = saleModel.SaleMechanism.Remunerations.First().PercentRemuneration,
                                            SharePercent = saleModel.SaleMechanism.Remunerations.First().CalculatingSharePercent,
                                            TimeKpi = 0,
                                            RevenueMin = 0,
                                            RevenueMax = saleModel.SaleMechanism.Remunerations.First().RevenueMin,
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                //kpi condition for later months
                                if (saleModel.SaleMechanism.LaterMonthsCondition != null && saleModel.SaleMechanism.LaterMonthsCondition.KpiPercent != null && saleModel.SaleMechanism.LaterMonthsCondition.SalaryPercent != null)
                                {
                                    LevelSalaryRevenue condition = new LevelSalaryRevenue
                                    {
                                        CodeKpi = saleModel.SaleMechanism.CodeKpi,
                                        PercentKpiMin = 0,
                                        PercentKpiMax = saleModel.SaleMechanism.LaterMonthsCondition.KpiPercent,
                                        SalaryPercentLv1 = saleModel.SaleMechanism.LaterMonthsCondition.SalaryPercent,
                                        CreateDate = DateTime.Now,
                                        RoleAccount = SaleRole,
                                        Salary = saleModel.SaleMechanism.Remunerations.First().Salary,
                                        Status = true,
                                        PercentRemuneration = saleModel.SaleMechanism.Remunerations.First().PercentRemuneration,
                                        SharePercent = saleModel.SaleMechanism.Remunerations.First().CalculatingSharePercent,
                                        TimeKpi = 1,
                                        RevenueMin = 0,
                                        RevenueMax = saleModel.SaleMechanism.Remunerations.First().RevenueMin,
                                    };
                                    db.Execute(SaleInsertQuery, condition, transaction);
                                }

                                //kpi salary for later months
                                if (saleModel.SaleMechanism.LaterMonthsSalary != null)
                                {
                                    foreach (var item in saleModel.SaleMechanism.LaterMonthsSalary)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.SaleMechanism.CodeKpi,
                                            PercentKpiMin = item.MinKpiPercent,
                                            PercentKpiMax = item.MaxKpiPercent,
                                            SalaryPercentLv1 = item.SalaryPercent,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = SaleRole,
                                            Salary = saleModel.SaleMechanism.Remunerations.First().Salary,
                                            Status = true,
                                            PercentRemuneration = saleModel.SaleMechanism.Remunerations.First().PercentRemuneration,
                                            SharePercent = saleModel.SaleMechanism.Remunerations.First().CalculatingSharePercent,
                                            TimeKpi = 1,
                                            RevenueMin = 0,
                                            RevenueMax = saleModel.SaleMechanism.Remunerations.First().RevenueMin,
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                //remuneration
                                if (saleModel.SaleMechanism.Remunerations != null && saleModel.SaleMechanism.Remunerations.Count > 0)
                                {
                                    foreach (var item in saleModel.SaleMechanism.Remunerations)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.SaleMechanism.CodeKpi,
                                            RevenueMin = item.RevenueMin,
                                            RevenueMax = item.RevenueMax,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = SaleRole,
                                            Salary = item.Salary,
                                            Status = true,
                                            PercentRemuneration = item.PercentRemuneration,
                                            SharePercent = item.CalculatingSharePercent,
                                            PercentKpiMin = 100,
                                            PercentKpiMax = 100
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                //SALE LEADER
                                //Kpi
                                if (saleModel.LeaderSaleMechanism.Remunerations != null && saleModel.LeaderSaleMechanism.Remunerations.Count > 0)
                                {
                                    saleModel.LeaderSaleMechanism.CodeKpi = Guid.NewGuid().ToString();
                                    Kpi kpi = new Kpi
                                    {
                                        CodeKpi = saleModel.LeaderSaleMechanism.CodeKpi,
                                        Revenue = saleModel.LeaderSaleMechanism.Remunerations.First().RevenueMax,
                                        RoleAccount = LeaderSaleRole,
                                        TypeKpi = 2,
                                        Status = true,
                                        KpiName = "Kpi for leader sale"
                                    };
                                    db.Execute(InsertKpiQuery, kpi, transaction);
                                }

                                //kpi condition for first months
                                if (saleModel.LeaderSaleMechanism.FirstMonthsCondition != null && saleModel.LeaderSaleMechanism.FirstMonthsCondition.KpiPercent != null && saleModel.LeaderSaleMechanism.FirstMonthsCondition.SalaryPercent != null)
                                {
                                    LevelSalaryRevenue condition = new LevelSalaryRevenue
                                    {
                                        CodeKpi = saleModel.LeaderSaleMechanism.CodeKpi,
                                        PercentKpiMin = 0,
                                        PercentKpiMax = saleModel.LeaderSaleMechanism.FirstMonthsCondition.KpiPercent,
                                        SalaryPercentLv1 = saleModel.LeaderSaleMechanism.FirstMonthsCondition.SalaryPercent,
                                        CreateDate = DateTime.Now,
                                        RoleAccount = LeaderSaleRole,
                                        Salary = saleModel.LeaderSaleMechanism.Remunerations.First().Salary,
                                        Status = true,
                                        PercentRemuneration = saleModel.LeaderSaleMechanism.Remunerations.First().PercentRemuneration,
                                        SharePercent = saleModel.LeaderSaleMechanism.Remunerations.First().CalculatingSharePercent,
                                        TimeKpi = 0,
                                        RevenueMin = 0,
                                        RevenueMax = saleModel.LeaderSaleMechanism.Remunerations.First().RevenueMin,
                                    };
                                    db.Execute(SaleInsertQuery, condition, transaction);
                                }

                                //kpi salary for first months
                                if (saleModel.LeaderSaleMechanism.FirstMonthsSalary != null)
                                {
                                    foreach (var item in saleModel.LeaderSaleMechanism.FirstMonthsSalary)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.LeaderSaleMechanism.CodeKpi,
                                            PercentKpiMin = item.MinKpiPercent,
                                            PercentKpiMax = item.MaxKpiPercent,
                                            SalaryPercentLv1 = item.SalaryPercent,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = LeaderSaleRole,
                                            Salary = saleModel.LeaderSaleMechanism.Remunerations.First().Salary,
                                            Status = true,
                                            PercentRemuneration = saleModel.LeaderSaleMechanism.Remunerations.First().PercentRemuneration,
                                            SharePercent = saleModel.LeaderSaleMechanism.Remunerations.First().CalculatingSharePercent,
                                            TimeKpi = 0,
                                            RevenueMin = 0,
                                            RevenueMax = saleModel.LeaderSaleMechanism.Remunerations.First().RevenueMin,
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                //kpi condition for later months
                                if (saleModel.LeaderSaleMechanism.LaterMonthsCondition != null && saleModel.LeaderSaleMechanism.LaterMonthsCondition.KpiPercent != null && saleModel.LeaderSaleMechanism.LaterMonthsCondition.SalaryPercent != null)
                                {
                                    LevelSalaryRevenue condition = new LevelSalaryRevenue
                                    {
                                        CodeKpi = saleModel.LeaderSaleMechanism.CodeKpi,
                                        PercentKpiMin = 0,
                                        PercentKpiMax = saleModel.LeaderSaleMechanism.LaterMonthsCondition.KpiPercent,
                                        SalaryPercentLv1 = saleModel.LeaderSaleMechanism.LaterMonthsCondition.SalaryPercent,
                                        CreateDate = DateTime.Now,
                                        RoleAccount = LeaderSaleRole,
                                        Salary = saleModel.LeaderSaleMechanism.Remunerations.First().Salary,
                                        Status = true,
                                        PercentRemuneration = saleModel.LeaderSaleMechanism.Remunerations.First().PercentRemuneration,
                                        SharePercent = saleModel.LeaderSaleMechanism.Remunerations.First().CalculatingSharePercent,
                                        TimeKpi = 1,
                                        RevenueMin = 0,
                                        RevenueMax = saleModel.LeaderSaleMechanism.Remunerations.First().RevenueMin,
                                    };
                                    db.Execute(SaleInsertQuery, condition, transaction);
                                }

                                //kpi salary for later months
                                if (saleModel.LeaderSaleMechanism.LaterMonthsSalary != null)
                                {
                                    foreach (var item in saleModel.LeaderSaleMechanism.LaterMonthsSalary)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.LeaderSaleMechanism.CodeKpi,
                                            PercentKpiMin = item.MinKpiPercent,
                                            PercentKpiMax = item.MaxKpiPercent,
                                            SalaryPercentLv1 = item.SalaryPercent,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = LeaderSaleRole,
                                            Salary = saleModel.LeaderSaleMechanism.Remunerations.First().Salary,
                                            Status = true,
                                            PercentRemuneration = saleModel.LeaderSaleMechanism.Remunerations.First().PercentRemuneration,
                                            SharePercent = saleModel.LeaderSaleMechanism.Remunerations.First().CalculatingSharePercent,
                                            TimeKpi = 1,
                                            RevenueMin = 0,
                                            RevenueMax = saleModel.LeaderSaleMechanism.Remunerations.First().RevenueMin,
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                //remuneration
                                if (saleModel.LeaderSaleMechanism.Remunerations != null && saleModel.LeaderSaleMechanism.Remunerations.Count > 0)
                                {
                                    foreach (var item in saleModel.LeaderSaleMechanism.Remunerations)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.LeaderSaleMechanism.CodeKpi,
                                            RevenueMin = item.RevenueMin,
                                            RevenueMax = item.RevenueMax,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = LeaderSaleRole,
                                            Salary = item.Salary,
                                            Status = true,
                                            PercentRemuneration = item.PercentRemuneration,
                                            SharePercent = item.CalculatingSharePercent,
                                            PercentKpiMin = 100,
                                            PercentKpiMax = 100
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                transaction.Commit();
                            }

                        }

                        return new DataResult<SalaryMechanismViewModel> ();
                    }
                    catch
                    {
                        return new DataResult<SalaryMechanismViewModel> { Error = true };
                    }
                }
                // update
                else
                {
                    try
                    {
                        using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                        {
                            db.Open();
                            using (var transaction = db.BeginTransaction())
                            {
                                //SALE
                                //kpi
                                if (saleModel.SaleMechanism.Remunerations != null && saleModel.SaleMechanism.Remunerations.Count > 0)
                                {
                                    saleModel.SaleMechanism.CodeKpi = Guid.NewGuid().ToString();
                                    Kpi kpi = new Kpi
                                    {
                                        CodeKpi = saleModel.SaleMechanism.CodeKpi,
                                        Revenue = saleModel.SaleMechanism.Remunerations.First().RevenueMax,
                                    };
                                    db.Execute(UpdateKpiQuery, kpi, transaction);
                                }

                                // delete old data
                                db.Execute(DeleteDataQuery, new { @RoleAccount = SaleRole }, transaction);

                                //kpi condition for first months
                                if (saleModel.SaleMechanism.FirstMonthsCondition != null && saleModel.SaleMechanism.FirstMonthsCondition.KpiPercent != null && saleModel.SaleMechanism.FirstMonthsCondition.SalaryPercent != null)
                                {
                                    LevelSalaryRevenue condition = new LevelSalaryRevenue
                                    {
                                        CodeKpi = saleModel.SaleMechanism.CodeKpi,
                                        PercentKpiMin = 0,
                                        PercentKpiMax = saleModel.SaleMechanism.FirstMonthsCondition.KpiPercent,
                                        SalaryPercentLv1 = saleModel.SaleMechanism.FirstMonthsCondition.SalaryPercent,
                                        CreateDate = DateTime.Now,
                                        RoleAccount = SaleRole,
                                        Salary = saleModel.SaleMechanism.Remunerations.First().Salary,
                                        Status = true,
                                        PercentRemuneration = saleModel.SaleMechanism.Remunerations.First().PercentRemuneration,
                                        SharePercent = saleModel.SaleMechanism.Remunerations.First().CalculatingSharePercent,
                                        TimeKpi = 0,
                                        RevenueMin = 0,
                                        RevenueMax = saleModel.SaleMechanism.Remunerations.First().RevenueMin,
                                    };
                                    db.Execute(SaleInsertQuery, condition, transaction);
                                }

                                //kpi salary for first months
                                if (saleModel.SaleMechanism.FirstMonthsSalary != null)
                                {
                                    foreach (var item in saleModel.SaleMechanism.FirstMonthsSalary)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.SaleMechanism.CodeKpi,
                                            PercentKpiMin = item.MinKpiPercent,
                                            PercentKpiMax = item.MaxKpiPercent,
                                            SalaryPercentLv1 = item.SalaryPercent,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = SaleRole,
                                            Salary = saleModel.SaleMechanism.Remunerations.First().Salary,
                                            Status = true,
                                            PercentRemuneration = saleModel.SaleMechanism.Remunerations.First().PercentRemuneration,
                                            SharePercent = saleModel.SaleMechanism.Remunerations.First().CalculatingSharePercent,
                                            TimeKpi = 0,
                                            RevenueMin = 0,
                                            RevenueMax = saleModel.SaleMechanism.Remunerations.First().RevenueMin,
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                //kpi condition for later months
                                if (saleModel.SaleMechanism.LaterMonthsCondition != null && saleModel.SaleMechanism.LaterMonthsCondition.KpiPercent != null && saleModel.SaleMechanism.LaterMonthsCondition.SalaryPercent != null)
                                {
                                    LevelSalaryRevenue condition = new LevelSalaryRevenue
                                    {
                                        CodeKpi = saleModel.SaleMechanism.CodeKpi,
                                        PercentKpiMin = 0,
                                        PercentKpiMax = saleModel.SaleMechanism.LaterMonthsCondition.KpiPercent,
                                        SalaryPercentLv1 = saleModel.SaleMechanism.LaterMonthsCondition.SalaryPercent,
                                        CreateDate = DateTime.Now,
                                        RoleAccount = SaleRole,
                                        Salary = saleModel.SaleMechanism.Remunerations.First().Salary,
                                        Status = true,
                                        PercentRemuneration = saleModel.SaleMechanism.Remunerations.First().PercentRemuneration,
                                        SharePercent = saleModel.SaleMechanism.Remunerations.First().CalculatingSharePercent,
                                        TimeKpi = 1,
                                        RevenueMin = 0,
                                        RevenueMax = saleModel.SaleMechanism.Remunerations.First().RevenueMin,
                                    };
                                    db.Execute(SaleInsertQuery, condition, transaction);
                                }

                                //kpi salary for later months
                                if (saleModel.SaleMechanism.LaterMonthsSalary != null)
                                {
                                    foreach (var item in saleModel.SaleMechanism.LaterMonthsSalary)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.SaleMechanism.CodeKpi,
                                            PercentKpiMin = item.MinKpiPercent,
                                            PercentKpiMax = item.MaxKpiPercent,
                                            SalaryPercentLv1 = item.SalaryPercent,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = SaleRole,
                                            Salary = saleModel.SaleMechanism.Remunerations.First().Salary,
                                            Status = true,
                                            PercentRemuneration = saleModel.SaleMechanism.Remunerations.First().PercentRemuneration,
                                            SharePercent = saleModel.SaleMechanism.Remunerations.First().CalculatingSharePercent,
                                            TimeKpi = 1,
                                            RevenueMin = 0,
                                            RevenueMax = saleModel.SaleMechanism.Remunerations.First().RevenueMin,
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                //remuneration
                                if (saleModel.SaleMechanism.Remunerations != null && saleModel.SaleMechanism.Remunerations.Count > 0)
                                {
                                    foreach (var item in saleModel.SaleMechanism.Remunerations)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.SaleMechanism.CodeKpi,
                                            RevenueMin = item.RevenueMin,
                                            RevenueMax = item.RevenueMax,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = SaleRole,
                                            Salary = item.Salary,
                                            Status = true,
                                            PercentRemuneration = item.PercentRemuneration,
                                            SharePercent = item.CalculatingSharePercent,
                                            PercentKpiMin = 100,
                                            PercentKpiMax = 100
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                //SALE LEADER
                                //Kpi
                                if (saleModel.LeaderSaleMechanism.Remunerations != null && saleModel.LeaderSaleMechanism.Remunerations.Count > 0)
                                {
                                    saleModel.LeaderSaleMechanism.CodeKpi = Guid.NewGuid().ToString();
                                    Kpi kpi = new Kpi
                                    {
                                        CodeKpi = saleModel.LeaderSaleMechanism.CodeKpi,
                                        Revenue = saleModel.LeaderSaleMechanism.Remunerations.First().RevenueMax,
                                    };
                                    db.Execute(UpdateKpiQuery, kpi, transaction);
                                }

                                // delete old data
                                db.Execute(DeleteDataQuery, new { @RoleAccount = LeaderSaleRole }, transaction);

                                //kpi condition for first months
                                if (saleModel.LeaderSaleMechanism.FirstMonthsCondition != null && saleModel.LeaderSaleMechanism.FirstMonthsCondition.KpiPercent != null && saleModel.LeaderSaleMechanism.FirstMonthsCondition.SalaryPercent != null)
                                {
                                    LevelSalaryRevenue condition = new LevelSalaryRevenue
                                    {
                                        CodeKpi = saleModel.LeaderSaleMechanism.CodeKpi,
                                        PercentKpiMin = 0,
                                        PercentKpiMax = saleModel.LeaderSaleMechanism.FirstMonthsCondition.KpiPercent,
                                        SalaryPercentLv1 = saleModel.LeaderSaleMechanism.FirstMonthsCondition.SalaryPercent,
                                        CreateDate = DateTime.Now,
                                        RoleAccount = LeaderSaleRole,
                                        Salary = saleModel.LeaderSaleMechanism.Remunerations.First().Salary,
                                        Status = true,
                                        PercentRemuneration = saleModel.LeaderSaleMechanism.Remunerations.First().PercentRemuneration,
                                        SharePercent = saleModel.LeaderSaleMechanism.Remunerations.First().CalculatingSharePercent,
                                        TimeKpi = 0,
                                        RevenueMin = 0,
                                        RevenueMax = saleModel.LeaderSaleMechanism.Remunerations.First().RevenueMin,
                                    };
                                    db.Execute(SaleInsertQuery, condition, transaction);
                                }

                                //kpi salary for first months
                                if (saleModel.LeaderSaleMechanism.FirstMonthsSalary != null)
                                {
                                    foreach (var item in saleModel.LeaderSaleMechanism.FirstMonthsSalary)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.LeaderSaleMechanism.CodeKpi,
                                            PercentKpiMin = item.MinKpiPercent,
                                            PercentKpiMax = item.MaxKpiPercent,
                                            SalaryPercentLv1 = item.SalaryPercent,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = LeaderSaleRole,
                                            Salary = saleModel.LeaderSaleMechanism.Remunerations.First().Salary,
                                            Status = true,
                                            PercentRemuneration = saleModel.LeaderSaleMechanism.Remunerations.First().PercentRemuneration,
                                            SharePercent = saleModel.LeaderSaleMechanism.Remunerations.First().CalculatingSharePercent,
                                            TimeKpi = 0,
                                            RevenueMin = 0,
                                            RevenueMax = saleModel.LeaderSaleMechanism.Remunerations.First().RevenueMin,
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                //kpi condition for later months
                                if (saleModel.LeaderSaleMechanism.LaterMonthsCondition != null && saleModel.LeaderSaleMechanism.LaterMonthsCondition.KpiPercent != null && saleModel.LeaderSaleMechanism.LaterMonthsCondition.SalaryPercent != null)
                                {
                                    LevelSalaryRevenue condition = new LevelSalaryRevenue
                                    {
                                        CodeKpi = saleModel.LeaderSaleMechanism.CodeKpi,
                                        PercentKpiMin = 0,
                                        PercentKpiMax = saleModel.LeaderSaleMechanism.LaterMonthsCondition.KpiPercent,
                                        SalaryPercentLv1 = saleModel.LeaderSaleMechanism.LaterMonthsCondition.SalaryPercent,
                                        CreateDate = DateTime.Now,
                                        RoleAccount = LeaderSaleRole,
                                        Salary = saleModel.LeaderSaleMechanism.Remunerations.First().Salary,
                                        Status = true,
                                        PercentRemuneration = saleModel.LeaderSaleMechanism.Remunerations.First().PercentRemuneration,
                                        SharePercent = saleModel.LeaderSaleMechanism.Remunerations.First().CalculatingSharePercent,
                                        TimeKpi = 1,
                                        RevenueMin = 0,
                                        RevenueMax = saleModel.LeaderSaleMechanism.Remunerations.First().RevenueMin,
                                    };
                                    db.Execute(SaleInsertQuery, condition, transaction);
                                }

                                //kpi salary for later months
                                if (saleModel.LeaderSaleMechanism.LaterMonthsSalary != null)
                                {
                                    foreach (var item in saleModel.LeaderSaleMechanism.LaterMonthsSalary)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.LeaderSaleMechanism.CodeKpi,
                                            PercentKpiMin = item.MinKpiPercent,
                                            PercentKpiMax = item.MaxKpiPercent,
                                            SalaryPercentLv1 = item.SalaryPercent,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = LeaderSaleRole,
                                            Salary = saleModel.LeaderSaleMechanism.Remunerations.First().Salary,
                                            Status = true,
                                            PercentRemuneration = saleModel.LeaderSaleMechanism.Remunerations.First().PercentRemuneration,
                                            SharePercent = saleModel.LeaderSaleMechanism.Remunerations.First().CalculatingSharePercent,
                                            TimeKpi = 1,
                                            RevenueMin = 0,
                                            RevenueMax = saleModel.LeaderSaleMechanism.Remunerations.First().RevenueMin,
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                //remuneration
                                if (saleModel.LeaderSaleMechanism.Remunerations != null && saleModel.LeaderSaleMechanism.Remunerations.Count > 0)
                                {
                                    foreach (var item in saleModel.LeaderSaleMechanism.Remunerations)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.LeaderSaleMechanism.CodeKpi,
                                            RevenueMin = item.RevenueMin,
                                            RevenueMax = item.RevenueMax,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = LeaderSaleRole,
                                            Salary = item.Salary,
                                            Status = true,
                                            PercentRemuneration = item.PercentRemuneration,
                                            SharePercent = item.CalculatingSharePercent,
                                            PercentKpiMin = 100,
                                            PercentKpiMax = 100
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                transaction.Commit();
                            }

                        }

                        return new DataResult<SalaryMechanismViewModel> ();
                    }
                    catch
                    {
                        return new DataResult<SalaryMechanismViewModel> { Error = true };
                    }
                }
            }

            return new DataResult<SalaryMechanismViewModel> { Error = true };
        }
        #endregion

        #region SetupSalaryElementsForMinister - khanhkk
        public DataResult<SalaryMechanismViewModel> SetupSalaryElementsForMinister(SalaryMechanismViewModel model)
        {
            MechanismForMinister saleModel = model as MechanismForMinister;
            if (saleModel != null && saleModel.MinisterMechanism != null)
            {
                //create new
                if (saleModel.MinisterMechanism.Id == 0)
                {
                    try
                    {
                        using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                        {
                            db.Open();
                            using (var transaction = db.BeginTransaction())
                            {
                                //SALE
                                //kpi
                                if (saleModel.MinisterMechanism.Remunerations != null && saleModel.MinisterMechanism.Remunerations.Count > 0)
                                {
                                    saleModel.MinisterMechanism.CodeKpi = Guid.NewGuid().ToString();
                                    Kpi kpi = new Kpi
                                    {
                                        CodeKpi = saleModel.MinisterMechanism.CodeKpi,
                                        Revenue = saleModel.MinisterMechanism.Remunerations.First().RevenueMax,
                                        RoleAccount = MinisterRole,
                                        TypeKpi = 2,
                                        Status = true,
                                        KpiName = "Kpi for minister"
                                    };
                                    db.Execute(InsertKpiQuery, kpi, transaction);
                                }

                                //kpi condition for first months
                                if (saleModel.MinisterMechanism.FirstMonthsCondition != null && saleModel.MinisterMechanism.FirstMonthsCondition.KpiPercent != null && saleModel.MinisterMechanism.FirstMonthsCondition.SalaryPercent != null)
                                {
                                    LevelSalaryRevenue condition = new LevelSalaryRevenue
                                    {
                                        CodeKpi = saleModel.MinisterMechanism.CodeKpi,
                                        PercentKpiMin = 0,
                                        PercentKpiMax = saleModel.MinisterMechanism.FirstMonthsCondition.KpiPercent,
                                        SalaryPercentLv1 = saleModel.MinisterMechanism.FirstMonthsCondition.SalaryPercent,
                                        CreateDate = DateTime.Now,
                                        RoleAccount = MinisterRole,
                                        Salary = saleModel.MinisterMechanism.Remunerations.First().Salary,
                                        Status = true,
                                        PercentRemuneration = saleModel.MinisterMechanism.Remunerations.First().PercentRemuneration,
                                        SharePercent = saleModel.MinisterMechanism.Remunerations.First().CalculatingSharePercent,
                                        TimeKpi = 0,
                                        RevenueMin = 0,
                                        RevenueMax = saleModel.MinisterMechanism.Remunerations.First().RevenueMin,
                                    };
                                    db.Execute(SaleInsertQuery, condition, transaction);
                                }

                                //kpi salary for first months
                                if (saleModel.MinisterMechanism.FirstMonthsSalary != null)
                                {
                                    foreach (var item in saleModel.MinisterMechanism.FirstMonthsSalary)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.MinisterMechanism.CodeKpi,
                                            PercentKpiMin = item.MinKpiPercent,
                                            PercentKpiMax = item.MaxKpiPercent,
                                            SalaryPercentLv1 = item.SalaryPercent,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = MinisterRole,
                                            Salary = saleModel.MinisterMechanism.Remunerations.First().Salary,
                                            Status = true,
                                            PercentRemuneration = saleModel.MinisterMechanism.Remunerations.First().PercentRemuneration,
                                            SharePercent = saleModel.MinisterMechanism.Remunerations.First().CalculatingSharePercent,
                                            TimeKpi = 0,
                                            RevenueMin = 0,
                                            RevenueMax = saleModel.MinisterMechanism.Remunerations.First().RevenueMin,
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                //kpi condition for later months
                                if (saleModel.MinisterMechanism.LaterMonthsCondition != null && saleModel.MinisterMechanism.LaterMonthsCondition.KpiPercent != null && saleModel.MinisterMechanism.LaterMonthsCondition.SalaryPercent != null)
                                {
                                    LevelSalaryRevenue condition = new LevelSalaryRevenue
                                    {
                                        CodeKpi = saleModel.MinisterMechanism.CodeKpi,
                                        PercentKpiMin = 0,
                                        PercentKpiMax = saleModel.MinisterMechanism.LaterMonthsCondition.KpiPercent,
                                        SalaryPercentLv1 = saleModel.MinisterMechanism.LaterMonthsCondition.SalaryPercent,
                                        CreateDate = DateTime.Now,
                                        RoleAccount = MinisterRole,
                                        Salary = saleModel.MinisterMechanism.Remunerations.First().Salary,
                                        Status = true,
                                        PercentRemuneration = saleModel.MinisterMechanism.Remunerations.First().PercentRemuneration,
                                        SharePercent = saleModel.MinisterMechanism.Remunerations.First().CalculatingSharePercent,
                                        TimeKpi = 1,
                                        RevenueMin = 0,
                                        RevenueMax = saleModel.MinisterMechanism.Remunerations.First().RevenueMin,
                                    };
                                    db.Execute(SaleInsertQuery, condition, transaction);
                                }

                                //kpi salary for later months
                                if (saleModel.MinisterMechanism.LaterMonthsSalary != null)
                                {
                                    foreach (var item in saleModel.MinisterMechanism.LaterMonthsSalary)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.MinisterMechanism.CodeKpi,
                                            PercentKpiMin = item.MinKpiPercent,
                                            PercentKpiMax = item.MaxKpiPercent,
                                            SalaryPercentLv1 = item.SalaryPercent,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = MinisterRole,
                                            Salary = saleModel.MinisterMechanism.Remunerations.First().Salary,
                                            Status = true,
                                            PercentRemuneration = saleModel.MinisterMechanism.Remunerations.First().PercentRemuneration,
                                            SharePercent = saleModel.MinisterMechanism.Remunerations.First().CalculatingSharePercent,
                                            TimeKpi = 1,
                                            RevenueMin = 0,
                                            RevenueMax = saleModel.MinisterMechanism.Remunerations.First().RevenueMin,
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                //remuneration
                                if (saleModel.MinisterMechanism.Remunerations != null && saleModel.MinisterMechanism.Remunerations.Count > 0)
                                {
                                    foreach (var item in saleModel.MinisterMechanism.Remunerations)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.MinisterMechanism.CodeKpi,
                                            RevenueMin = item.RevenueMin,
                                            RevenueMax = item.RevenueMax,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = MinisterRole,
                                            Salary = item.Salary,
                                            Status = true,
                                            PercentRemuneration = item.PercentRemuneration,
                                            SharePercent = item.CalculatingSharePercent,
                                            PercentKpiMin = 100,
                                            PercentKpiMax = 100
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                transaction.Commit();
                            }

                        }

                        return new DataResult<SalaryMechanismViewModel>();
                    }
                    catch
                    {
                        return new DataResult<SalaryMechanismViewModel> { Error = true };
                    }
                }
                // update
                else
                {
                    try
                    {
                        using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                        {
                            db.Open();
                            using (var transaction = db.BeginTransaction())
                            {
                                //SALE
                                //kpi
                                if (saleModel.MinisterMechanism.Remunerations != null && saleModel.MinisterMechanism.Remunerations.Count > 0)
                                {
                                    saleModel.MinisterMechanism.CodeKpi = Guid.NewGuid().ToString();
                                    Kpi kpi = new Kpi
                                    {
                                        CodeKpi = saleModel.MinisterMechanism.CodeKpi,
                                        Revenue = saleModel.MinisterMechanism.Remunerations.First().RevenueMax,
                                    };
                                    db.Execute(UpdateKpiQuery, kpi, transaction);
                                }

                                // delete old data
                                db.Execute(DeleteDataQuery, new { @RoleAccount = MinisterRole }, transaction);

                                //kpi condition for first months
                                if (saleModel.MinisterMechanism.FirstMonthsCondition != null && saleModel.MinisterMechanism.FirstMonthsCondition.KpiPercent != null && saleModel.MinisterMechanism.FirstMonthsCondition.SalaryPercent != null)
                                {
                                    LevelSalaryRevenue condition = new LevelSalaryRevenue
                                    {
                                        CodeKpi = saleModel.MinisterMechanism.CodeKpi,
                                        PercentKpiMin = 0,
                                        PercentKpiMax = saleModel.MinisterMechanism.FirstMonthsCondition.KpiPercent,
                                        SalaryPercentLv1 = saleModel.MinisterMechanism.FirstMonthsCondition.SalaryPercent,
                                        CreateDate = DateTime.Now,
                                        RoleAccount = MinisterRole,
                                        Salary = saleModel.MinisterMechanism.Remunerations.First().Salary,
                                        Status = true,
                                        PercentRemuneration = saleModel.MinisterMechanism.Remunerations.First().PercentRemuneration,
                                        SharePercent = saleModel.MinisterMechanism.Remunerations.First().CalculatingSharePercent,
                                        TimeKpi = 0,
                                        RevenueMin = 0,
                                        RevenueMax = saleModel.MinisterMechanism.Remunerations.First().RevenueMin,
                                    };
                                    db.Execute(SaleInsertQuery, condition, transaction);
                                }

                                //kpi salary for first months
                                if (saleModel.MinisterMechanism.FirstMonthsSalary != null)
                                {
                                    foreach (var item in saleModel.MinisterMechanism.FirstMonthsSalary)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.MinisterMechanism.CodeKpi,
                                            PercentKpiMin = item.MinKpiPercent,
                                            PercentKpiMax = item.MaxKpiPercent,
                                            SalaryPercentLv1 = item.SalaryPercent,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = MinisterRole,
                                            Salary = saleModel.MinisterMechanism.Remunerations.First().Salary,
                                            Status = true,
                                            PercentRemuneration = saleModel.MinisterMechanism.Remunerations.First().PercentRemuneration,
                                            SharePercent = saleModel.MinisterMechanism.Remunerations.First().CalculatingSharePercent,
                                            TimeKpi = 0,
                                            RevenueMin = 0,
                                            RevenueMax = saleModel.MinisterMechanism.Remunerations.First().RevenueMin,
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                //kpi condition for later months
                                if (saleModel.MinisterMechanism.LaterMonthsCondition != null && saleModel.MinisterMechanism.LaterMonthsCondition.KpiPercent != null && saleModel.MinisterMechanism.LaterMonthsCondition.SalaryPercent != null)
                                {
                                    LevelSalaryRevenue condition = new LevelSalaryRevenue
                                    {
                                        CodeKpi = saleModel.MinisterMechanism.CodeKpi,
                                        PercentKpiMin = 0,
                                        PercentKpiMax = saleModel.MinisterMechanism.LaterMonthsCondition.KpiPercent,
                                        SalaryPercentLv1 = saleModel.MinisterMechanism.LaterMonthsCondition.SalaryPercent,
                                        CreateDate = DateTime.Now,
                                        RoleAccount = MinisterRole,
                                        Salary = saleModel.MinisterMechanism.Remunerations.First().Salary,
                                        Status = true,
                                        PercentRemuneration = saleModel.MinisterMechanism.Remunerations.First().PercentRemuneration,
                                        SharePercent = saleModel.MinisterMechanism.Remunerations.First().CalculatingSharePercent,
                                        TimeKpi = 1,
                                        RevenueMin = 0,
                                        RevenueMax = saleModel.MinisterMechanism.Remunerations.First().RevenueMin,
                                    };
                                    db.Execute(SaleInsertQuery, condition, transaction);
                                }

                                //kpi salary for later months
                                if (saleModel.MinisterMechanism.LaterMonthsSalary != null)
                                {
                                    foreach (var item in saleModel.MinisterMechanism.LaterMonthsSalary)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.MinisterMechanism.CodeKpi,
                                            PercentKpiMin = item.MinKpiPercent,
                                            PercentKpiMax = item.MaxKpiPercent,
                                            SalaryPercentLv1 = item.SalaryPercent,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = MinisterRole,
                                            Salary = saleModel.MinisterMechanism.Remunerations.First().Salary,
                                            Status = true,
                                            PercentRemuneration = saleModel.MinisterMechanism.Remunerations.First().PercentRemuneration,
                                            SharePercent = saleModel.MinisterMechanism.Remunerations.First().CalculatingSharePercent,
                                            TimeKpi = 1,
                                            RevenueMin = 0,
                                            RevenueMax = saleModel.MinisterMechanism.Remunerations.First().RevenueMin,
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                //remuneration
                                if (saleModel.MinisterMechanism.Remunerations != null && saleModel.MinisterMechanism.Remunerations.Count > 0)
                                {
                                    foreach (var item in saleModel.MinisterMechanism.Remunerations)
                                    {
                                        LevelSalaryRevenue condition = new LevelSalaryRevenue
                                        {
                                            CodeKpi = saleModel.MinisterMechanism.CodeKpi,
                                            RevenueMin = item.RevenueMin,
                                            RevenueMax = item.RevenueMax,
                                            CreateDate = DateTime.Now,
                                            RoleAccount = MinisterRole,
                                            Salary = item.Salary,
                                            Status = true,
                                            PercentRemuneration = item.PercentRemuneration,
                                            SharePercent = item.CalculatingSharePercent,
                                            PercentKpiMin = 100,
                                            PercentKpiMax = 100
                                        };
                                        db.Execute(SaleInsertQuery, condition, transaction);
                                    }
                                }

                                transaction.Commit();
                            }

                        }

                        return new DataResult<SalaryMechanismViewModel>();
                    }
                    catch
                    {
                        return new DataResult<SalaryMechanismViewModel> { Error = true };
                    }
                }
            }

            return new DataResult<SalaryMechanismViewModel> { Error = true };
        }
        #endregion

        #region AddRemunerations
        private void AddRemunerations(List<TeleSaleRemuneration> remunerations, TeleSaleSalaryMechanism model, IDbConnection db, IDbTransaction transaction)
        {
            if (model.ProbationaryCondition.KpiPercent is null || model.ProbationaryCondition.KpiPercent == 0 ||
                model.ProbationaryCondition.KpiPercent == 100)
            {
                foreach (var item in remunerations)
                {
                    LevelSalaryRevenue remuneration = new LevelSalaryRevenue
                    {
                        CodeKpi = model.Common.CodeKpi,
                        PercentKpiMin = item.MinPercent,
                        PercentKpiMax = item.MaxPercent,
                        RoleAccount = TeleRole,
                        CreateDate = DateTime.Now,
                        Status = true,
                        TimeKpi = 0,
                        Salary = model.Common.Salary,
                        ProbationarySalary = model.Common.ProbationarySalary,
                        RemunerationShowUp = item.ShowRemuneration,
                        RemunerationContractTele = item.ContractRemuneration,
                    };
                    db.Execute(TeleInsertQuery, remuneration, transaction);
                }
            }
            else
            {
                foreach (var item in remunerations)
                {
                    LevelSalaryRevenue probationary = new LevelSalaryRevenue
                    {
                        CodeKpi = model.Common.CodeKpi,
                        RoleAccount = model.Common.RoleAccount,
                        CreateDate = DateTime.Now,
                        Status = true,
                        ProbationarySalary = model.Common.ProbationarySalary,
                        ProbationaryTime = model.Common.ProbationaryTime,
                        Salary = model.Common.Salary,
                        PercentKpiMin = item.MinPercent,
                        PercentKpiMax = item.MaxPercent,
                        RemunerationShowUp = item.ShowRemuneration,
                        RemunerationContractTele = item.ContractRemuneration,
                    };

                    if (item.MinPercent == model.ProbationaryCondition.KpiPercent)
                    {
                        probationary.SalaryPercentLv1 = 100;
                        db.Execute(TeleInsertQuery, probationary, transaction);
                        continue;
                    }

                    if (item.MaxPercent == model.ProbationaryCondition.KpiPercent)
                    {
                        probationary.SalaryPercentLv1 = model.ProbationaryCondition.SalaryPercent;
                        db.Execute(TeleInsertQuery, probationary, transaction);
                        continue;
                    }

                    if (item.MaxPercent > model.ProbationaryCondition.KpiPercent && item.MinPercent < model.ProbationaryCondition.KpiPercent)
                    {
                        probationary.PercentKpiMin = item.MinPercent;
                        probationary.PercentKpiMax = model.ProbationaryCondition.KpiPercent;
                        probationary.SalaryPercentLv1 = model.ProbationaryCondition.SalaryPercent;
                        db.Execute(TeleInsertQuery, probationary, transaction);

                        LevelSalaryRevenue probationary2 = new LevelSalaryRevenue
                        {
                            CodeKpi = model.Common.CodeKpi,
                            PercentKpiMin = model.ProbationaryCondition.KpiPercent,
                            PercentKpiMax = item.MaxPercent,
                            RoleAccount = model.Common.RoleAccount,
                            CreateDate = DateTime.Now,
                            TimeKpi = 2,
                            Status = true,
                            ProbationarySalary = model.Common.ProbationarySalary,
                            SalaryPercentLv1 = 100,
                            ProbationaryTime = model.Common.ProbationaryTime,
                            Salary = model.Common.Salary,
                            RemunerationShowUp = item.ShowRemuneration,
                            RemunerationContractTele = item.ContractRemuneration,
                        };
                        db.Execute(TeleInsertQuery, probationary2, transaction);
                        continue;
                    }

                    probationary.SalaryPercentLv1 = item.MinPercent > model.ProbationaryCondition.KpiPercent ? 100 : model.ProbationaryCondition.KpiPercent;
                    db.Execute(TeleInsertQuery, probationary, transaction);
                }
            }
        }
        #endregion
    }
}
