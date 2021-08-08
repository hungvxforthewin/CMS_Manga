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
    public class InvestorsCareHistoryImp : BaseService<InvestorsCareHistory, int>, IInvestorsCareHistory
    {
        public DataResult<InvestorsCareHistoryViewModel> GetById(int id)
        {
            try
            {
                var data = new InvestorsCareHistoryViewModel();
                DynamicParameters p = new DynamicParameters();
                p.Add("@Id", id);
                data = this.Procedure<InvestorsCareHistoryViewModel>("tblInvestorsCareHistory_GetById", p).FirstOrDefault();
                return new DataResult<InvestorsCareHistoryViewModel>() { DataItem = data};
            }
            catch (Exception ex)
            {

                return new DataResult<InvestorsCareHistoryViewModel> { Error = true };
            }
        }

        public DataResult<DisplayInvestorsCareHistoryViewModel> GetList(SearchInvestorsCareHistoryViewModel model, out int total)
        {
            List<DisplayInvestorsCareHistoryViewModel> data = new List<DisplayInvestorsCareHistoryViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@DateFrom", model.DateFrom);        
            param.Add("@DateTo", model.DateTo);        
            param.Add("@ProductCode", model.ProductCode);
            param.Add("@EventCode", model.EventCode);
            param.Add("@LevelconcernCode", model.LevelconcernCode);
            param.Add("@StatusCode", model.StatusCode);
            param.Add("@Key", model.Key);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    data = this.Procedure<DisplayInvestorsCareHistoryViewModel>("sp_tblInvestorCareHistory_GetList", param).ToList();
                    total = param.Get<int>("Total");
                }
                return new DataResult<DisplayInvestorsCareHistoryViewModel> { Result = data ?? new List<DisplayInvestorsCareHistoryViewModel>() };
            }
            catch (Exception ex)
            {
                return new DataResult<DisplayInvestorsCareHistoryViewModel> { Error = true };
            }
        }
    }
}
