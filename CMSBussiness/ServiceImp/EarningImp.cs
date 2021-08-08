using CRMBussiness.IService;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using CRMModel.Models.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CRMBussiness.ServiceImp
{
    public class EarningImp : BaseService<Earning, long>, IEarning
    {
        #region GetSalaries
        public DataResult<EarningViewModel> GetSalaries(string month)
        {
            if (string.IsNullOrEmpty(month))
            {
                month = TimeKeepingImp._currentMonth;
            }

            List<EarningViewModel> data = new List<EarningViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@month", month);
            try
            {
                data = this.Procedure<EarningViewModel>("St_Calculate_SalaryAndRemuneration_In_Month_WithShowUpRevenue", param).ToList();
            }
            catch (Exception ex)
            {
                return new DataResult<EarningViewModel> { Error = true };
            }

            return new DataResult<EarningViewModel> { Result = data };
        }
        #endregion

        #region GetSalaries2
        public DataResult<EarningViewModel> GetSalaries(SearchSalaryModel model, out int total)
        {
            List<EarningViewModel> data = new List<EarningViewModel>();
            total = 0;
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", model.Key ?? string.Empty);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@Branch", model.Branch);
            param.Add("@Department", model.Department);
            param.Add("@Team", model.Team);
            param.Add("@Status", model.Status);
            param.Add("@Month", model.Month);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            try
            {
                data = this.Procedure<EarningViewModel>("St_Calculate_SalaryAndRemuneration_In_Month_WithShowUpRevenue", param).ToList();
                total = param.Get<int>("Total");
            }
            catch (Exception ex)
            {
                return new DataResult<EarningViewModel> { Error = true };
            }

            return new DataResult<EarningViewModel> { Result = data };
        }
        #endregion

        #region GetTemporarySalary - khanhkk
        public DataResult<EarningViewModel> GetTemporarySalary(string month, string codeStaff)
        {
            if (string.IsNullOrEmpty(month))
            {
                month = TimeKeepingImp._currentMonth;
            }

            List<EarningViewModel> data = new List<EarningViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@month", month);
            param.Add("@codeStaff", codeStaff);
            try
            {
                data = this.Procedure<EarningViewModel>("St_Calculate_TempSalary_In_Month", param).ToList();
            }
            catch (Exception ex)
            {
                return new DataResult<EarningViewModel> { Error = true };
            }

            return new DataResult<EarningViewModel> { Result = data };
        }
        #endregion
    }
}
