using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface ILevelSalaryRevenueSaleAdmin : IBaseServices<LevelSalaryRevenue, short>
    {
        bool DeleteAllByRole(byte role);
        List<LevelSalaryRevenueSaleAdmin> LevelSalaryRevenueSaleAdminWithRole(byte roleAccount);
        short InsertWithSaleAdmin(byte RoleAccount, decimal Salary, byte ProbationaryTime, decimal ProbationarySalary, float PercentRemuneration, decimal RevenueMin, decimal RevenueMax, ref short id);
    }
    public class LevelSalaryRevenueSaleAdmin
    {
        public short Id { get; set; }
        public short RoleAccount { get; set; }
        public decimal Salary { get; set; }
        public byte ProbationaryTime { get; set; }
        public decimal ProbationarySalary { get; set; }
        public float PercentRemuneration { get; set; }
        //khanhkk added
        public float SharePercent { get; set; }
        //khanhkk added
        public string RevenueMin { get; set; }
        public string RevenueMax { get; set; }
        public string CodeKpi { get; set; }
    }
}
