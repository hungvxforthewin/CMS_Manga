using CRMBussiness.IService;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CRMBussiness.ServiceImp
{
    public class SetupSalaryElementsImp :  ISetupSalaryElements
    {
        #region GetSetupSalaryForTele - khanhkk
        public DataResult<SalaryElementViewModel> GetSetupSalaryForTele()
        {
            List<SalaryElementViewModel> data = new List<SalaryElementViewModel>();
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    //TeleSale
                    //common info
                    CommonSetupSalary common = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblSalaryWithRoleStaff_GetByRole",
                        new { @role = 8 }, commandType: CommandType.StoredProcedure))
                    {
                        common = multipleresult.Read<CommonSetupSalary>().FirstOrDefault();
                    }

                    //kpi
                    SetupKPI kpi = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblKpi_GetByRole",
                        new { @role = 8 }, commandType: CommandType.StoredProcedure))
                    {
                        kpi = multipleresult.Read<SetupKPI>().FirstOrDefault();
                    }

                    // kpi salary
                    KPISalary kPISalary = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblSalaryRealWithRuleKpi_GetByRole",
                        new { @role = 8 }, commandType: CommandType.StoredProcedure))
                    {
                        kPISalary = multipleresult.Read<KPISalary>().FirstOrDefault();
                    }

                    //Remuneration
                    List<Remuneration> remunerations = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblRemuneration_GetByRole_ForTeleSale",
                        new { @role = 8 }, commandType: CommandType.StoredProcedure))
                    {
                        remunerations = multipleresult.Read<Remuneration>().ToList();
                    }

                    TeleSaleSalary teleSaleSalary = new TeleSaleSalary
                    {
                        Id = common.Id,
                        RoleAccount = 8,
                        Salary = common.Salary,
                        TimeProbationary = common.TimeProbationary,
                        ProbationarySalary = common.ProbationarySalary,
                        Probationary = kPISalary,
                        KPI = kpi,
                        KPIRemunerations = remunerations
                    };

                    //Leader telesale
                    //common info
                    CommonSetupSalary commonLeader = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblSalaryWithRoleStaff_GetByRole",
                        new { @role = 9 }, commandType: CommandType.StoredProcedure))
                    {
                        commonLeader = multipleresult.Read<CommonSetupSalary>().FirstOrDefault();
                    }

                    //kpi
                    SetupKPI kpiLeader = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblKpi_GetByRole",
                        new { @role = 9 }, commandType: CommandType.StoredProcedure))
                    {
                        kpiLeader = multipleresult.Read<SetupKPI>().FirstOrDefault();
                    }

                    // kpi salary
                    KPISalary kPISalaryLeader = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblSalaryRealWithRuleKpi_GetByRole",
                        new { @role = 9 }, commandType: CommandType.StoredProcedure))
                    {
                        kPISalaryLeader = multipleresult.Read<KPISalary>().FirstOrDefault();
                    }

                    //Remuneration
                    List<Remuneration> remunerationsLeader = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblRemuneration_GetByRole_ForTeleSale",
                        new { @role = 9 }, commandType: CommandType.StoredProcedure))
                    {
                        remunerationsLeader = multipleresult.Read<Remuneration>().ToList();
                    }

                    TeleSaleSalary teleSaleSalaryLeader = new TeleSaleSalary
                    {
                        Id = commonLeader.Id,
                        RoleAccount = 9,
                        Salary = commonLeader.Salary,
                        TimeProbationary = commonLeader.TimeProbationary,
                        ProbationarySalary = commonLeader.ProbationarySalary,
                        Probationary = kPISalaryLeader,
                        KPI = kpiLeader,
                        KPIRemunerations = remunerationsLeader
                    };
                    data.Add(new TeleSalaryElementViewModel 
                    { 
                        SetupTeleSaleSalary = teleSaleSalary, 
                        SetupTeleSaleLeaderSalary = teleSaleSalaryLeader 
                    });
                }

                return new DataResult<SalaryElementViewModel> { Error = false, Result = data };
            }
            catch
            {
                return new DataResult<SalaryElementViewModel> { Error = true };
            }
        }
        #endregion

        #region GetSetupSalaryForTele - khanhkk
        public DataResult<SalaryElementViewModel> GetSetupSalaryForSale()
        {
            List<SalaryElementViewModel> data = new List<SalaryElementViewModel>();
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    //Sale
                    //common info
                    CommonSetupSalary common = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblSalaryWithRoleStaff_GetByRole",
                        new { @role = 4 }, commandType: CommandType.StoredProcedure))
                    {
                        common = multipleresult.Read<CommonSetupSalary>().FirstOrDefault();
                    }

                    //kpi
                    List<SetupKPI> kpis = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblKpi_GetByRole",
                        new { @role = 4 }, commandType: CommandType.StoredProcedure))
                    {
                        kpis = multipleresult.Read<SetupKPI>().ToList();
                    }

                    // kpi salary
                    List<KPISalary> kPISalary = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblSalaryRealWithRuleKpi_GetByRole",
                        new { @role = 4 }, commandType: CommandType.StoredProcedure))
                    {
                        kPISalary = multipleresult.Read<KPISalary>().ToList();
                    }

                    //Remuneration
                    List<Remuneration> remunerations = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblRemuneration_GetByRole_ForSale",
                        new { }, commandType: CommandType.StoredProcedure))
                    {
                        remunerations = multipleresult.Read<Remuneration>().ToList();
                    }

                    SaleSalary saleSalary = new SaleSalary
                    {
                        Id = common.Id,
                        RoleAccount = 4,
                        Salary = common.Salary,
                        TimeProbationary = common.TimeProbationary,
                        ProbationarySalary = common.ProbationarySalary,
                        SetupKpiSalary = kPISalary,
                        KPIs = kpis,
                        KPIRemunerations = remunerations
                    };

                    //Leader sale
                    //common info
                    CommonSetupSalary commonLeader = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblSalaryWithRoleStaff_GetByRole",
                        new { @role = 5 }, commandType: CommandType.StoredProcedure))
                    {
                        commonLeader = multipleresult.Read<CommonSetupSalary>().FirstOrDefault();
                    }

                    //kpi
                    List<SetupKPI> kpiLeader = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblKpi_GetByRole",
                        new { @role = 5 }, commandType: CommandType.StoredProcedure))
                    {
                        kpiLeader = multipleresult.Read<SetupKPI>().ToList();
                    }

                    // kpi salary
                    List<KPISalary> kPISalaryLeader = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblSalaryRealWithRuleKpi_GetByRole",
                        new { @role = 5 }, commandType: CommandType.StoredProcedure))
                    {
                        kPISalaryLeader = multipleresult.Read<KPISalary>().ToList();
                    }

                    //Remuneration
                    List<Remuneration> remunerationsLeader = null;
                    using (var multipleresult = db.QueryMultiple("sp_tblRemuneration_GetByRole_ForLeaderSale",
                        new { }, commandType: CommandType.StoredProcedure))
                    {
                        remunerationsLeader = multipleresult.Read<Remuneration>().ToList();
                    }

                    SaleSalary saleSalaryLeader = new SaleSalary
                    {
                        Id = commonLeader.Id,
                        RoleAccount = 5,
                        Salary = commonLeader.Salary,
                        TimeProbationary = commonLeader.TimeProbationary,
                        ProbationarySalary = commonLeader.ProbationarySalary,
                        SetupKpiSalary = kPISalaryLeader,
                        KPIs = kpiLeader,
                        KPIRemunerations = remunerationsLeader,
                        TeamSize = commonLeader.TeamSize,
                        SalaryMin = commonLeader.SalaryMin
                    };
                    data.Add(new SaleSalaryElementViewModel
                    {
                        SetupSaleSalary = saleSalary,
                        SetupSaleLeaderSalary = saleSalaryLeader
                    });
                }

                return new DataResult<SalaryElementViewModel> { Error = false, Result = data };
            }
            catch
            {
                return new DataResult<SalaryElementViewModel> { Error = true };
            }
        }
        #endregion

        #region SetupSalaryElementsForTele - khanhkk
        public DataResult<SalaryElementViewModel> SetupSalaryElementsForTele(SalaryElementViewModel model)
        {
            TeleSalaryElementViewModel teleModel = model as TeleSalaryElementViewModel;

            if (teleModel != null && teleModel.SetupTeleSaleSalary != null && teleModel.SetupTeleSaleLeaderSalary != null)
            {
                //create new
                if (teleModel.SetupTeleSaleSalary.Id == 0)
                {
                    try
                    {
                        using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                        {
                            db.Open();
                            using (var transaction = db.BeginTransaction())
                            {
                                //TElE
                                //SalaryWithRoleStaff
                                if (teleModel.SetupTeleSaleSalary != null)
                                {
                                    string query = "INSERT INTO tblSalaryWithRoleStaff(RoleAccount, Salary, TimeProbationary, ProbationarySalary, Status) VALUES (8, @Salary, @TimeProbationary, @ProbationarySalary, 1)";
                                    db.Execute(query, teleModel.SetupTeleSaleSalary, transaction);
                                }

                                //kpi
                                if (teleModel.SetupTeleSaleSalary.KPI != null)
                                {
                                    teleModel.SetupTeleSaleSalary.KPI.CodeKpi = Guid.NewGuid().ToString();
                                    teleModel.SetupTeleSaleSalary.KPI.KpiName = "KPI for tele";
                                    teleModel.SetupTeleSaleSalary.KPI.TypeKpi = 1;
                                    string query = "INSERT INTO tblKpi(CodeKpi, KpiName, RoleAccount, TypeKpi, TotalShowUp, TypeContract, Status) VALUES (@CodeKpi, @KpiName, 8, @TypeKpi, @TotalShowUp, 0, 1)";
                                    db.Execute(query, teleModel.SetupTeleSaleSalary.KPI, transaction);
                                }

                                //kpi salary
                                if (teleModel.SetupTeleSaleSalary.Probationary != null)
                                {
                                    string query = "INSERT INTO tblSalaryRealWithRuleKpi(RoleAccount, RatioKpiMin, RatioKpiMax, PercentSalary, StatusProbationary, Status) VALUES (8, @RatioKpiMin, @RatioKpiMax, @PercentSalary, 1, 1)";
                                    db.Execute(query, teleModel.SetupTeleSaleSalary.Probationary, transaction);
                                }

                                //remuneration
                                if (teleModel.SetupTeleSaleSalary.KPIRemunerations != null && teleModel.SetupTeleSaleSalary.KPIRemunerations.Count > 0)
                                {
                                    foreach (var item in teleModel.SetupTeleSaleSalary.KPIRemunerations)
                                    {
                                        item.CodeRemuneration = Guid.NewGuid().ToString();
                                        item.CodeKpi = teleModel.SetupTeleSaleSalary.KPI.CodeKpi;

                                        string query = "INSERT INTO tblRemuneration(CodeRemuneration, CodeKpi, PercentKpiMin, PercentKpiMax, RoleAccount, AmountContractTele, AmountShowupTele) VALUES (@CodeRemuneration, @CodeKpi, @PercentKpiMin, @PercentKpiMax, 8, @AmountContractTele, @AmountShowupTele)";
                                        db.Execute(query, item, transaction);
                                    }
                                }

                                //TELE LEADER
                                //SalaryWithRoleStaff
                                if (teleModel.SetupTeleSaleLeaderSalary != null)
                                {
                                    string query = "INSERT INTO tblSalaryWithRoleStaff(RoleAccount, Salary, TimeProbationary, ProbationarySalary, Status) VALUES (9, @Salary, @TimeProbationary, @ProbationarySalary, 1)";
                                    db.Execute(query, teleModel.SetupTeleSaleLeaderSalary, transaction);
                                }

                                //kpi
                                if (teleModel.SetupTeleSaleLeaderSalary.KPI != null)
                                {
                                    teleModel.SetupTeleSaleLeaderSalary.KPI.CodeKpi = Guid.NewGuid().ToString();
                                    teleModel.SetupTeleSaleLeaderSalary.KPI.KpiName = "KPI for tele leader";
                                    teleModel.SetupTeleSaleLeaderSalary.KPI.TypeKpi = 1;
                                    string query = "INSERT INTO tblKpi(CodeKpi, KpiName, RoleAccount, TypeKpi, TotalShowUp, TypeContract, Status) VALUES (@CodeKpi, @KpiName, 9, @TypeKpi, @TotalShowUp, 0, 1)";
                                    db.Execute(query, teleModel.SetupTeleSaleLeaderSalary.KPI, transaction);
                                }

                                //kpi salary
                                if (teleModel.SetupTeleSaleLeaderSalary.Probationary != null)
                                {
                                    string query = "INSERT INTO tblSalaryRealWithRuleKpi(RoleAccount, RatioKpiMin, RatioKpiMax, PercentSalary, StatusProbationary, Status) VALUES (9, @RatioKpiMin, @RatioKpiMax, @PercentSalary, 1, 1)";
                                    db.Execute(query, teleModel.SetupTeleSaleLeaderSalary.Probationary, transaction);
                                }

                                //remuneration
                                if (teleModel.SetupTeleSaleLeaderSalary.KPIRemunerations != null && teleModel.SetupTeleSaleLeaderSalary.KPIRemunerations.Count > 0)
                                {
                                    foreach (var item in teleModel.SetupTeleSaleLeaderSalary.KPIRemunerations)
                                    {
                                        item.CodeRemuneration = Guid.NewGuid().ToString();
                                        item.CodeKpi = teleModel.SetupTeleSaleLeaderSalary.KPI.CodeKpi;

                                        string query = "INSERT INTO tblRemuneration(CodeRemuneration, CodeKpi, PercentKpiMin, PercentKpiMax, RoleAccount, AmountContractTele, AmountShowupTele) VALUES (@CodeRemuneration, @CodeKpi, @PercentKpiMin, @PercentKpiMax, 9, @AmountContractTele, @AmountShowupTele)";
                                        db.Execute(query, item, transaction);
                                    }
                                }

                                transaction.Commit();
                            }

                        }

                        return new DataResult<SalaryElementViewModel> { Error = false };
                    }
                    catch
                    {
                        return new DataResult<SalaryElementViewModel> { Error = true };
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
                                //TElE
                                //SalaryWithRoleStaff
                                if (teleModel.SetupTeleSaleSalary != null)
                                {
                                    string query = "UPDATE tblSalaryWithRoleStaff SET Salary = @Salary, TimeProbationary = @TimeProbationary, ProbationarySalary = @ProbationarySalary Where Id = @Id";
                                    db.Execute(query, teleModel.SetupTeleSaleSalary, transaction);
                                }

                                //kpi
                                if (teleModel.SetupTeleSaleSalary.KPI != null)
                                {
                                    //teleModel.SetupTeleSaleSalary.KPI.CodeKpi = Guid.NewGuid().ToString();
                                    //teleModel.SetupTeleSaleSalary.KPI.KpiName = "KPI for tele";
                                    //teleModel.SetupTeleSaleSalary.KPI.TypeKpi = 1;
                                    string query = "UPDATE tblKpi SET TotalShowUp = @TotalShowUp WHERE Id = @Id";
                                    db.Execute(query, teleModel.SetupTeleSaleSalary.KPI, transaction);
                                }

                                //kpi salary
                                if (teleModel.SetupTeleSaleSalary.Probationary != null)
                                {
                                    string query = "UPDATE tblSalaryRealWithRuleKpi SET RatioKpiMin = @RatioKpiMin, RatioKpiMax = @RatioKpiMax, PercentSalary = @PercentSalary WHERE Id = @Id";
                                    db.Execute(query, teleModel.SetupTeleSaleSalary.Probationary, transaction);
                                }

                                //remuneration
                                if (teleModel.SetupTeleSaleSalary.KPIRemunerations != null && teleModel.SetupTeleSaleSalary.KPIRemunerations.Count > 0)
                                {
                                    foreach (var item in teleModel.SetupTeleSaleSalary.KPIRemunerations)
                                    {
                                        string query = "DELETE FROM tblRemuneration WHERE Id = @Id";
                                        db.Execute(query, item, transaction);
                                    }

                                    foreach (var item in teleModel.SetupTeleSaleSalary.KPIRemunerations)
                                    {
                                        item.CodeRemuneration = Guid.NewGuid().ToString();
                                        item.CodeKpi = teleModel.SetupTeleSaleSalary.KPI.CodeKpi;

                                        string query = "INSERT INTO tblRemuneration(CodeRemuneration, CodeKpi, PercentKpiMin, PercentKpiMax, RoleAccount, AmountContractTele, AmountShowupTele) VALUES (@CodeRemuneration, @CodeKpi, @PercentKpiMin, @PercentKpiMax, 8, @AmountContractTele, @AmountShowupTele)";
                                        db.Execute(query, item, transaction);
                                    }
                                }

                                //TELE LEADER
                                //SalaryWithRoleStaff
                                if (teleModel.SetupTeleSaleLeaderSalary != null)
                                {
                                    string query = "UPDATE tblSalaryWithRoleStaff SET Salary = @Salary, TimeProbationary = @TimeProbationary, ProbationarySalary = @ProbationarySalary Where Id = @Id";
                                    db.Execute(query, teleModel.SetupTeleSaleLeaderSalary, transaction);
                                }

                                //kpi
                                if (teleModel.SetupTeleSaleLeaderSalary.KPI != null)
                                {
                                    //teleModel.SetupTeleSaleLeaderSalary.KPI.CodeKpi = Guid.NewGuid().ToString();
                                    //teleModel.SetupTeleSaleLeaderSalary.KPI.KpiName = "KPI for tele leader";
                                    //teleModel.SetupTeleSaleLeaderSalary.KPI.TypeKpi = 1;
                                    string query = "UPDATE tblKpi SET TotalShowUp = @TotalShowUp WHERE Id = @Id";
                                    db.Execute(query, teleModel.SetupTeleSaleLeaderSalary.KPI, transaction);
                                }

                                //kpi salary
                                if (teleModel.SetupTeleSaleLeaderSalary.Probationary != null)
                                {
                                    string query = "UPDATE tblSalaryRealWithRuleKpi SET RatioKpiMin = @RatioKpiMin, RatioKpiMax = @RatioKpiMax, PercentSalary = @PercentSalary WHERE Id = @Id";
                                    db.Execute(query, teleModel.SetupTeleSaleLeaderSalary.Probationary, transaction);
                                }

                                //remuneration
                                if (teleModel.SetupTeleSaleLeaderSalary.KPIRemunerations != null && teleModel.SetupTeleSaleLeaderSalary.KPIRemunerations.Count > 0)
                                {
                                    foreach (var item in teleModel.SetupTeleSaleLeaderSalary.KPIRemunerations)
                                    {
                                        string query = "DELETE FROM tblRemuneration WHERE Id = @Id";
                                        db.Execute(query, item, transaction);
                                    }

                                    foreach (var item in teleModel.SetupTeleSaleLeaderSalary.KPIRemunerations)
                                    {
                                        item.CodeRemuneration = Guid.NewGuid().ToString();
                                        item.CodeKpi = teleModel.SetupTeleSaleLeaderSalary.KPI.CodeKpi;

                                        string query = "INSERT INTO tblRemuneration(CodeRemuneration, CodeKpi, PercentKpiMin, PercentKpiMax, RoleAccount, AmountContractTele, AmountShowupTele) VALUES (@CodeRemuneration, @CodeKpi, @PercentKpiMin, @PercentKpiMax, 9, @AmountContractTele, @AmountShowupTele)";
                                        db.Execute(query, item, transaction);
                                    }
                                }
                                transaction.Commit();
                            }
                        }

                        return new DataResult<SalaryElementViewModel> { Error = false };
                    }
                    catch
                    {
                        return new DataResult<SalaryElementViewModel> { Error = true };
                    }
                }
            }
            return new DataResult<SalaryElementViewModel> { Error = true };
        }
        #endregion

        #region SetupSalaryElementsForSale - khanhkk
        public DataResult<SalaryElementViewModel> SetupSalaryElementsForSale(SalaryElementViewModel model)
        {
            SaleSalaryElementViewModel saleModel = model as SaleSalaryElementViewModel;

            if (saleModel != null && saleModel.SetupSaleSalary != null && saleModel.SetupSaleLeaderSalary != null)
            {
                //create new
                if (saleModel.SetupSaleSalary.Id == 0)
                {
                    try
                    {
                        using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                        {
                            db.Open();
                            using (var transaction = db.BeginTransaction())
                            {
                                //SALE
                                //SalaryWithRoleStaff
                                if (saleModel.SetupSaleSalary != null)
                                {
                                    string query = "INSERT INTO tblSalaryWithRoleStaff(RoleAccount, Salary, TimeProbationary, ProbationarySalary, Status) VALUES (4, @Salary, @TimeProbationary, @ProbationarySalary, 1)";
                                    db.Execute(query, saleModel.SetupSaleSalary, transaction);
                                }

                                //kpi
                                if (saleModel.SetupSaleSalary.KPIs != null && saleModel.SetupSaleSalary.KPIs.Count > 0)
                                {
                                    foreach (var item in saleModel.SetupSaleSalary.KPIs)
                                    {
                                        item.CodeKpi = Guid.NewGuid().ToString();
                                        item.KpiName = "KPI for sale";
                                        item.TypeKpi = 2;
                                        string query = "INSERT INTO tblKpi(CodeKpi, KpiName, RoleAccount, TypeKpi, Revenue, TypeContract, Status) VALUES (@CodeKpi, @KpiName, 4, @TypeKpi, @Revenue, @TypeContract, 1)";
                                        db.Execute(query, item, transaction);
                                    }
                                }

                                //kpi salary
                                if (saleModel.SetupSaleSalary.SetupKpiSalary != null && saleModel.SetupSaleSalary.SetupKpiSalary.Count > 0)
                                {
                                    foreach (var item in saleModel.SetupSaleSalary.SetupKpiSalary)
                                    {
                                        string query = "INSERT INTO tblSalaryRealWithRuleKpi(RoleAccount, RatioKpiMin, RatioKpiMax, PercentSalary, StatusProbationary, Status) VALUES (4, @RatioKpiMin, @RatioKpiMax, @PercentSalary, @StatusProbationary, 1)";
                                        db.Execute(query, item, transaction);
                                    }
                                }

                                //revenue salary
                                if (saleModel.SetupSaleSalary.RevenueSalary != null && saleModel.SetupSaleSalary.RevenueSalary.Count > 0)
                                {
                                    foreach (var item in saleModel.SetupSaleSalary.RevenueSalary)
                                    {
                                        string query = "INSERT INTO tblSalaryRealWithRuleKpi(RoleAccount, RevenuMin, RevenuMax, SalaryReal, StatusProbationary, Status) VALUES (4, @RevenuMin, @RevenuMax, @SalaryReal, 0, 1)";
                                        db.Execute(query, item, transaction);
                                    }
                                }

                                //remuneration
                                if (saleModel.SetupSaleSalary.KPIRemunerations != null && saleModel.SetupSaleSalary.KPIRemunerations.Count > 0)
                                {
                                    foreach (var item in saleModel.SetupSaleSalary.KPIRemunerations)
                                    {
                                        item.CodeRemuneration = Guid.NewGuid().ToString();
                                        item.CodeKpi = saleModel.SetupSaleSalary.KPIs.Where(x => x.TypeContract == 0).First().CodeKpi;

                                        string query = "INSERT INTO tblRemuneration(CodeRemuneration, CodeKpi, AmountMinInMonth, AmountMaxInMonth, RoleAccount, [Percent]) VALUES (@CodeRemuneration, @CodeKpi, @AmountMinInMonth, @AmountMaxInMonth, 4, @Percent)";
                                        db.Execute(query, item, transaction);
                                    }
                                }

                                //SALE LEADER
                                //SalaryWithRoleStaff
                                if (saleModel.SetupSaleLeaderSalary != null)
                                {
                                    string query = "INSERT INTO tblSalaryWithRoleStaff(RoleAccount, Salary, TimeProbationary, ProbationarySalary, SalaryMin, TeamSize, Status) VALUES (5, @Salary, @TimeProbationary, @ProbationarySalary, @SalaryMin, @TeamSize, 1)";
                                    db.Execute(query, saleModel.SetupSaleLeaderSalary, transaction);
                                }

                                //kpi
                                if (saleModel.SetupSaleLeaderSalary.KPIs != null && saleModel.SetupSaleLeaderSalary.KPIs.Count > 0)
                                {
                                    foreach (var item in saleModel.SetupSaleLeaderSalary.KPIs)
                                    {
                                        item.CodeKpi = Guid.NewGuid().ToString();
                                        item.KpiName = "KPI for sale leader";
                                        item.TypeKpi = 2;
                                        string query = "INSERT INTO tblKpi(CodeKpi, KpiName, RoleAccount, TypeKpi, Revenue, TypeContract, Status) VALUES (@CodeKpi, @KpiName, 5, @TypeKpi, @Revenue, @TypeContract, 1)";
                                        db.Execute(query, item, transaction);
                                    }
                                }

                                //kpi salary
                                if (saleModel.SetupSaleLeaderSalary.SetupKpiSalary != null && saleModel.SetupSaleLeaderSalary.SetupKpiSalary.Count > 0)
                                {
                                    foreach (var item in saleModel.SetupSaleLeaderSalary.SetupKpiSalary)
                                    {
                                        string query = "INSERT INTO tblSalaryRealWithRuleKpi(RoleAccount, RatioKpiMin, RatioKpiMax, PercentSalary, StatusProbationary, Status) VALUES (5, @RatioKpiMin, @RatioKpiMax, @PercentSalary, @StatusProbationary, 1)";
                                        db.Execute(query, item, transaction);
                                    }
                                }

                                ////revenue salary
                                //if (saleModel.SetupSaleLeaderSalary.RevenueSalary != null && saleModel.SetupSaleLeaderSalary.RevenueSalary.Count > 0)
                                //{
                                //    foreach (var item in saleModel.SetupSaleLeaderSalary.SetupKpiSalary)
                                //    {
                                //        string query = "INSERT INTO tblSalaryRealWithRuleKpi(RoleAccount, RevenuMin, RevenuMax, SalaryReal, StatusProbationary, Status) VALUES (5, @RevenuMin, @RevenuMax, @SalaryReal, false, 1)";
                                //        db.Execute(query, item, transaction);
                                //    }
                                //}

                                //remuneration
                                if (saleModel.SetupSaleLeaderSalary.KPIRemunerations != null && saleModel.SetupSaleLeaderSalary.KPIRemunerations.Count > 0)
                                {
                                    //saleModel.SetupSaleLeaderSalary.KPIRemunerations = 
                                    //    saleModel.SetupSaleLeaderSalary.KPIRemunerations.OrderBy(x => x.AmountMinInMonth).ToList();
                                    //for(int i = saleModel.SetupSaleLeaderSalary.KPIRemunerations.Count - 2; i >= 0; i++)
                                    //{
                                    //    saleModel.SetupSaleLeaderSalary.KPIRemunerations[i].AmountMaxInMonth = saleModel.SetupSaleLeaderSalary.KPIRemunerations[i + 1].AmountMinInMonth;
                                    //}

                                    foreach (var item in saleModel.SetupSaleLeaderSalary.KPIRemunerations)
                                    {
                                        item.CodeRemuneration = Guid.NewGuid().ToString();
                                        item.CodeKpi = saleModel.SetupSaleLeaderSalary.KPIs.Where(x => x.TypeContract == 0).First().CodeKpi;
                                        string query = "INSERT INTO tblRemuneration(CodeRemuneration, CodeKpi, AmountMinInMonth, AmountMaxInMonth, RoleAccount, [Percent], MinRevenueTeam, MaxRevenueTeam) VALUES (@CodeRemuneration, @CodeKpi, @AmountMinInMonth, NULL, 5, @Percent, @MinRevenueTeam, @MaxRevenueTeam)";
                                        db.Execute(query, item, transaction);
                                    }
                                }

                                transaction.Commit();
                            }

                        }

                        return new DataResult<SalaryElementViewModel> { Error = false };
                    }
                    catch
                    {
                        return new DataResult<SalaryElementViewModel> { Error = true };
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
                                //SalaryWithRoleStaff
                                if (saleModel.SetupSaleSalary != null)
                                {
                                    string query = "UPDATE tblSalaryWithRoleStaff SET Salary = @Salary, TimeProbationary = @TimeProbationary, ProbationarySalary = @ProbationarySalary WHERE Id = @Id";
                                    db.Execute(query, saleModel.SetupSaleSalary, transaction);
                                }

                                //kpi
                                if (saleModel.SetupSaleSalary.KPIs != null && saleModel.SetupSaleSalary.KPIs.Count > 0)
                                {
                                    foreach (var item in saleModel.SetupSaleSalary.KPIs)
                                    {
                                        if (item.Id != 0)
                                        {
                                            if (item.Revenue != 0)
                                            {
                                                string query = "UPDATE tblKpi SET Revenue = @Revenue WHERE Id = @Id";
                                                db.Execute(query, item, transaction);
                                            }
                                            else 
                                            {
                                                string query = "DELETE FROM tblKpi WHERE Id = @Id";
                                                db.Execute(query, item, transaction);
                                            }
                                        }
                                        else 
                                        {
                                            item.CodeKpi = Guid.NewGuid().ToString();
                                            item.KpiName = "KPI for sale";
                                            item.TypeKpi = 2;
                                            string query = "INSERT INTO tblKpi(CodeKpi, KpiName, RoleAccount, TypeKpi, Revenue, TypeContract, Status) VALUES        (@CodeKpi, @KpiName, 4, @TypeKpi, @Revenue, @TypeContract, 1)";
                                            db.Execute(query, item, transaction);
                                        }
                                    }
                                }

                                //kpi salary
                                if (saleModel.SetupSaleSalary.SetupKpiSalary != null && saleModel.SetupSaleSalary.SetupKpiSalary.Count > 0)
                                {
                                    foreach (var item in saleModel.SetupSaleSalary.SetupKpiSalary)
                                    {
                                        string query = "DELETE FROM tblSalaryRealWithRuleKpi WHERE Id = @Id";
                                        db.Execute(query, item, transaction);
                                    }

                                    foreach (var item in saleModel.SetupSaleSalary.SetupKpiSalary)
                                    {
                                        string query = "INSERT INTO tblSalaryRealWithRuleKpi(RoleAccount, RatioKpiMin, RatioKpiMax, PercentSalary, StatusProbationary, Status) VALUES (4, @RatioKpiMin, @RatioKpiMax, @PercentSalary, @StatusProbationary, 1)";
                                        db.Execute(query, item, transaction);
                                    }
                                }

                                //revenue salary
                                if (saleModel.SetupSaleSalary.RevenueSalary != null && saleModel.SetupSaleSalary.RevenueSalary.Count > 0)
                                {
                                    foreach (var item in saleModel.SetupSaleSalary.RevenueSalary)
                                    {
                                        string query = "DELETE FROM tblSalaryRealWithRuleKpi WHERE Id = @Id";
                                        db.Execute(query, item, transaction);
                                    }

                                    foreach (var item in saleModel.SetupSaleSalary.RevenueSalary)
                                    {
                                        string query = "INSERT INTO tblSalaryRealWithRuleKpi(RoleAccount, RevenuMin, RevenuMax, SalaryReal, StatusProbationary, Status) VALUES (4, @RevenuMin, @RevenuMax, @SalaryReal, 0, 1)";
                                        db.Execute(query, item, transaction);
                                    }
                                }

                                //remuneration
                                if (saleModel.SetupSaleSalary.KPIRemunerations != null && saleModel.SetupSaleSalary.KPIRemunerations.Count > 0)
                                {
                                    foreach (var item in saleModel.SetupSaleSalary.KPIRemunerations)
                                    {
                                        string query = "DELETE FROM tblRemuneration WHERE Id = @Id";
                                        db.Execute(query, item, transaction);
                                    }

                                    foreach (var item in saleModel.SetupSaleSalary.KPIRemunerations)
                                    {
                                        item.CodeRemuneration = Guid.NewGuid().ToString();
                                        item.CodeKpi = saleModel.SetupSaleSalary.KPIs.Where(x => x.TypeContract == 0).First().CodeKpi;

                                        string query = "INSERT INTO tblRemuneration(CodeRemuneration, CodeKpi, AmountMinInMonth, AmountMaxInMonth, RoleAccount, [Percent]) VALUES (@CodeRemuneration, @CodeKpi, @AmountMinInMonth, @AmountMaxInMonth, 4, @Percent)";
                                        db.Execute(query, item, transaction);
                                    }
                                }

                                //SALE LEADER
                                //SalaryWithRoleStaff
                                if (saleModel.SetupSaleLeaderSalary != null)
                                {
                                    string query = "UPDATE tblSalaryWithRoleStaff SET Salary = @Salary, TimeProbationary = @TimeProbationary, ProbationarySalary = @ProbationarySalary, TeamSize = @TeamSize, SalaryMin = @SalaryMin WHERE Id = @Id";
                                    db.Execute(query, saleModel.SetupSaleLeaderSalary, transaction);
                                }

                                //kpi
                                if (saleModel.SetupSaleLeaderSalary.KPIs != null && saleModel.SetupSaleLeaderSalary.KPIs.Count > 0)
                                {
                                    foreach (var item in saleModel.SetupSaleLeaderSalary.KPIs)
                                    {
                                        if (item.Id != 0)
                                        {
                                            if (item.Revenue != 0)
                                            {
                                                string query = "UPDATE tblKpi SET Revenue = @Revenue WHERE Id = @Id";
                                                db.Execute(query, item, transaction);
                                            }
                                            else
                                            {
                                                string query = "DELETE FROM tblKpi WHERE Id = @Id";
                                                db.Execute(query, item, transaction);
                                            }
                                        }
                                        else
                                        {
                                            item.CodeKpi = Guid.NewGuid().ToString();
                                            item.KpiName = "KPI for sale leader";
                                            item.TypeKpi = 2;
                                            string query = "INSERT INTO tblKpi(CodeKpi, KpiName, RoleAccount, TypeKpi, Revenue, TypeContract, Status) VALUES        (@CodeKpi, @KpiName, 5, @TypeKpi, @Revenue, @TypeContract, 1)";
                                            db.Execute(query, item, transaction);
                                        }
                                    }
                                }

                                //kpi salary
                                if (saleModel.SetupSaleLeaderSalary.SetupKpiSalary != null && saleModel.SetupSaleLeaderSalary.SetupKpiSalary.Count > 0)
                                {
                                    foreach (var item in saleModel.SetupSaleLeaderSalary.SetupKpiSalary)
                                    {
                                        string query = "DELETE FROM tblSalaryRealWithRuleKpi WHERE Id = @Id";
                                        db.Execute(query, item, transaction);
                                    }

                                    foreach (var item in saleModel.SetupSaleLeaderSalary.SetupKpiSalary)
                                    {
                                        string query = "INSERT INTO tblSalaryRealWithRuleKpi(RoleAccount, RatioKpiMin, RatioKpiMax, PercentSalary, StatusProbationary, Status) VALUES (5, @RatioKpiMin, @RatioKpiMax, @PercentSalary, @StatusProbationary, 1)";
                                        db.Execute(query, item, transaction);
                                    }
                                }

                                //remuneration
                                if (saleModel.SetupSaleLeaderSalary.KPIRemunerations != null && saleModel.SetupSaleLeaderSalary.KPIRemunerations.Count > 0)
                                {
                                    foreach (var item in saleModel.SetupSaleLeaderSalary.KPIRemunerations)
                                    {
                                        string query = "DELETE FROM tblRemuneration WHERE Id = @Id";
                                        db.Execute(query, item, transaction);
                                    }

                                    foreach (var item in saleModel.SetupSaleLeaderSalary.KPIRemunerations)
                                    {
                                        item.CodeRemuneration = Guid.NewGuid().ToString();
                                        item.CodeKpi = saleModel.SetupSaleLeaderSalary.KPIs.Where(x => x.TypeContract == 0).First().CodeKpi;
                                        string query = "INSERT INTO tblRemuneration(CodeRemuneration, CodeKpi, AmountMinInMonth, AmountMaxInMonth, RoleAccount, [Percent], MinRevenueTeam, MaxRevenueTeam) VALUES (@CodeRemuneration, @CodeKpi, @AmountMinInMonth, NULL, 5, @Percent, @MinRevenueTeam, @MaxRevenueTeam)";
                                        db.Execute(query, item, transaction);
                                    }
                                }
                                transaction.Commit();
                            }

                        }

                        return new DataResult<SalaryElementViewModel> { Error = false };
                    }
                    catch
                    {
                        return new DataResult<SalaryElementViewModel> { Error = true };
                    }
                }
            }
            return new DataResult<SalaryElementViewModel> { Error = true };
        }
        #endregion

        public DataResult<SalaryElementViewModel> UpdateSetupSalaryElements(SalaryElementViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
