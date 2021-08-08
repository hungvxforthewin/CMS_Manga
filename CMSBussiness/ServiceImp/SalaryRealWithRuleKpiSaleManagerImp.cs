using CRMBussiness.IService;
using CRMBussiness.LIB;
using CRMModel.Models.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CRMBussiness.ViewModel;
using System.Data.SqlClient;
using SalaryRealWithRuleKpi = CRMModel.Models.Data.SalaryRealWithRuleKpi;

namespace CRMBussiness.ServiceImp
{
    public class SalaryRealWithRuleKpiSaleManagerImp : BaseService<SalaryRealWithRuleKpi, long>, ISalaryRealWithRuleKpiSaleManager
    {

    }
}
