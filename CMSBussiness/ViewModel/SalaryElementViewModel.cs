using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRMBussiness.ViewModel
{
    public class SalaryElementViewModel
    {

    }

    [Serializable]
    public class TeleSalaryElementViewModel : SalaryElementViewModel
    {
        public TeleSaleSalary SetupTeleSaleSalary { get; set; }

        public TeleSaleSalary SetupTeleSaleLeaderSalary { get; set; }
    }

    [Serializable]
    public class SaleSalaryElementViewModel : SalaryElementViewModel
    {
        public SaleSalary SetupSaleSalary { get; set; }

        public SaleSalary SetupSaleLeaderSalary { get; set; }
    }

    public class TeleSaleSalary : CommonSetupSalary
    {
        public SetupKPI KPI { get; set; }

        public KPISalary Probationary { get; set; }

        public IList<Remuneration> KPIRemunerations { get; set; }
    }

    public class SaleSalary : CommonSetupSalary
    {
        public IList<SetupKPI> KPIs { get; set; }

        public IList<KPISalary> SetupKpiSalary { get; set; }

        public IList<RevenueSalary> RevenueSalary { get; set; }

        public IList<Remuneration> KPIRemunerations { get; set; }
    }

    public class CommonSetupSalary
    {
        public virtual int Id { get; set; }

        public virtual byte RoleAccount { get; set; }

        public virtual decimal Salary { get; set; }

        public virtual byte TimeProbationary { get; set; }

        public virtual decimal ProbationarySalary { get; set; }

        public virtual byte TeamSize { get; set; }

        public virtual decimal SalaryMin { get; set; }
    }

    public class KPISalary
    {
        public virtual short Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập khoảng % KPI doanh số đạt được để tính lương cứng")]
        public virtual string KPIRange { get; set; }

        public virtual byte RatioKpiMin { get; set; }

        public virtual byte? RatioKpiMax { get; set; }

        public virtual byte? PercentSalary { get; set; }

        public virtual bool StatusProbationary { get; set; }
    }

    public class RevenueSalary
    {
        public virtual short Id { get; set; }

        public virtual decimal RevenuMin { get; set; }

        public virtual decimal? RevenuMax { get; set; }

        public virtual decimal SalaryReal { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập khoảng doanh số đạt được để tính hoa hồng")]
        public virtual string AmountRange { get; set; }
    }

    public class SetupKPI
    {
        public virtual long Id { get; set; }

        [StringLength(50)]
        public virtual string CodeKpi { get; set; }

        [StringLength(100)]
        public virtual string KpiName { get; set; }

        public virtual byte TypeKpi { get; set; }

        public virtual decimal? Revenue { get; set; }

        public virtual short TotalShowUp { get; set; }

        public virtual byte TypeContract { get; set; }
    }

    public class Remuneration
    {
        public virtual byte Id { get; set; }

        public virtual short RuleKpiId { get; set; }

        [StringLength(50)]
        public virtual string CodeRemuneration { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập khoảng % KPI để tính hoa hồng")]
        public virtual string PercentKpiRange { get; set; }

        [StringLength(50)]
        public virtual string CodeKpi { get; set; }

        public virtual byte PercentKpiMin { get; set; }

        public virtual byte? PercentKpiMax { get; set; }

        public virtual byte RoleAccount { get; set; }

        public virtual byte Percent { get; set; }

        public virtual decimal AmountContractTele { get; set; }

        public virtual decimal AmountShowupTele { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập khoảng doanh số đạt được để tính hoa hồng")]
        public virtual string AmountRange { get; set; }

        public virtual decimal AmountMinInMonth { get; set; }

        public virtual decimal? AmountMaxInMonth { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Chưa nhập khoảng doanh số nhóm đạt được để tính hoa hồng")]
        public virtual string AmountTeamRange { get; set; }

        public virtual decimal MinRevenueTeam { get; set; }

        public virtual decimal? MaxRevenueTeam { get; set; }

        public virtual decimal SalaryReal { get; set; }
    }
}
