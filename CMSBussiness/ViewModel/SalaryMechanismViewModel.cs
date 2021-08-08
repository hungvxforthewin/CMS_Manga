using System.Collections.Generic;

namespace CRMBussiness.ViewModel
{
    public class SalaryMechanismViewModel
    {

    }

    public class MechanismForSale : SalaryMechanismViewModel
    {
        public SaleSalaryMechanism SaleMechanism { get; set; }

        public SaleSalaryMechanism LeaderSaleMechanism { get; set; }
    }

    public class MechanismForMinister : SalaryMechanismViewModel
    {
        public SaleSalaryMechanism MinisterMechanism { get; set; }
    }

    public class SaleSalaryMechanism
    {
        public virtual long Id { get; set; }

        public virtual string CodeKpi { get; set; }

        public List<SaleRemurationLevel> Remunerations { get; set; }

        public Level1Condition FirstMonthsCondition { get; set; }

        public List<KpiSalary> FirstMonthsSalary { get; set; }

        public Level1Condition LaterMonthsCondition { get; set; }

        public List<KpiSalary> LaterMonthsSalary { get; set; }
    }

    public class MechanismForTele : SalaryMechanismViewModel
    {
        public TeleSaleSalaryMechanism TeleSaleMachanism { get; set; }

        public TeleSaleSalaryMechanism LeaderTeleSaleMachanism { get; set; }
    }

    public class TeleSaleSalaryMechanism
    {
        public TeleSaleCommon Common { get; set; }

        public Level1Condition ProbationaryCondition { get; set; }

        public List<TeleSaleRemuneration> Remunerations { get; set; }
    }

    public class SaleRemurationLevel
    {
        public float PercentRemuneration { get; set; }

        public float CalculatingSharePercent { get; set; }

        public decimal Salary { get; set; }

        public string RevenueRange { get; set; }

        public decimal? RevenueMin { get; set; }

        public decimal? RevenueMax { get; set; }
    }

    public class Level1Condition
    {
        public virtual long Id { get; set; }

        public virtual string CodeKpi { get; set; }

        public float? KpiPercent { get; set; }

        public float? SalaryPercent { get; set; }
    }

    public class KpiSalary
    {
        public float MinKpiPercent { get; set; }

        public float MaxKpiPercent { get; set; }

        public float SalaryPercent { get; set; }
    }

    public class TeleSaleCommon
    {
        public int Id { get; set; }

        public string CodeKpi { get; set; }

        public byte RoleAccount { get; set; }

        public decimal Salary { get; set; }

        public float Kpi { get; set; }

        public byte? ProbationaryTime { get; set; }

        public decimal? ProbationarySalary { get; set; }
    }

    public class TeleSaleRemuneration
    {
        public virtual string PercentRange { get; set; }

        public virtual float? MinPercent { get; set; }

        public virtual float? MaxPercent { get; set; }

        public virtual decimal ShowRemuneration { get; set; }

        public virtual decimal ContractRemuneration { get; set; }
    }
}
