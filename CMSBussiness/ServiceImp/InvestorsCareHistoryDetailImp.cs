using System;
using System.Linq;
using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using CRMBussiness.IService;
using CRMBussiness.LIB;
using CRMModel.Models.Data;
using CRMBussiness.ViewModel;

namespace CRMBussiness.ServiceImp
{
    public class InvestorsCareHistoryDetailImp : BaseService<InvestorsCareHistoryDetail, int>, IInvestorsCareHistoryDetail
    {
        public bool DeleteByHistoryCode(string historyCode)
        {
            try
            {
                this.Raw_Query<DetailCallCareHistory>("DELETE tblInvestorsCareHistoryDetail WHERE HistoryCode = @HistoryCode", new Dictionary<string, object>() {
                    {"HistoryCode", historyCode }
                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataResult<DetailCallCareHistory> GetByHistoryCode(string code)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@HistoryCode", code);
                List<DetailCallCareHistory> data = new List<DetailCallCareHistory>();
                data = this.Procedure<DetailCallCareHistory>("tblInvestorsCareHistoryDetail_GetByCode", p).ToList();
                return new DataResult<DetailCallCareHistory>() { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<DetailCallCareHistory>() { Error = true };
            }
        }
    }
}
