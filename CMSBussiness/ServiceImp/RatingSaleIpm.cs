using System;
using System.Linq;
using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Data.SqlClient;
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

namespace CRMBussiness.ServiceImp
{
    public class RatingSaleIpm : BaseService<RatingSale, int>, IRatingSale
    {
        public DataResult<RatingSaleViewModel> GetById(int id)
        {
            try
            {
                RatingSaleViewModel data = new RatingSaleViewModel();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", id);
                data = this.Procedure<RatingSaleViewModel>("sp_tbLRatingSale_GetById", param).SingleOrDefault();
                return new DataResult<RatingSaleViewModel> { DataItem = data ?? new RatingSaleViewModel() };
            }
            catch (Exception ex)
            {
                return new DataResult<RatingSaleViewModel> { Error = true };
            }
        }

        public DataResult<DisplayRatingSaleTableViewModel> GetList(SearchRattingViewModel model, out int total)
        {
            List<DisplayRatingSaleTableViewModel> data = new List<DisplayRatingSaleTableViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@DateRevenue", model.DateRevenue);
            param.Add("@Key", model.Key);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@Sale", model.Sale);
            param.Add("@BranchCode", model.Branch);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    data = this.Procedure<DisplayRatingSaleTableViewModel>("sp_tbLRatingSale_GetList", param).ToList();
                    total = param.Get<int>("Total");
                }
                return new DataResult<DisplayRatingSaleTableViewModel> { Result = data ?? new List<DisplayRatingSaleTableViewModel>() };
            }
            catch (Exception ex)
            {
                return new DataResult<DisplayRatingSaleTableViewModel> { Error = true };
            }
        }

        public DataResult<SaleChart> GetTop(string DateSale)
        {
            try
            {
                List<SaleChart> data = new List<SaleChart>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@DateSale", DateSale);
                data = this.Procedure<SaleChart>("sp_tbLRatingSale_Top10", param).ToList();
                return new DataResult<SaleChart> { Result = data ?? new List<SaleChart>() };
            }
            catch (Exception ex)
            {
                return new DataResult<SaleChart> { Error = true };
            }
        }

        public DataResult<SaleTop10> GetTop10Day(DateTime today)
        {
            try
            {
                List<SaleTop10> data = new List<SaleTop10>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Today", today);
                data = this.Procedure<SaleTop10>("sp_tbLRatingSale_Top10_WithDay", param).ToList();
                return new DataResult<SaleTop10> { Result = data ?? new List<SaleTop10>() };
            }
            catch (Exception ex)
            {

                return new DataResult<SaleTop10> { Error = true }; ;
            }
        }

        public DataResult<SaleTop10> GetTop10Month(DateTime today, out int month)
        {
            try
            {
                List<SaleTop10> data = new List<SaleTop10>();
                DynamicParameters param = new DynamicParameters();
                month = 0;
                param.Add("@Today", today);
                param.Add("@Month", dbType: DbType.Int32, direction: ParameterDirection.Output);
                data = this.Procedure<SaleTop10>("sp_tbLRatingSale_Top10_WithMonth", param).ToList();
                month = param.Get<int>("Month");
                return new DataResult<SaleTop10> { Result = data ?? new List<SaleTop10>() };
            }
            catch (Exception ex)
            {
                month = 0;
                return new DataResult<SaleTop10> { Error = true }; ;
            }
        }

        public DataResult<SaleTop10> GetTop10Week(DateTime today, out int week)
        {
            try
            {
                List<SaleTop10> data = new List<SaleTop10>();
                DynamicParameters param = new DynamicParameters();
                week = 0;
                param.Add("@Today", today);
                param.Add("@NumberWeek", dbType: DbType.Int32, direction: ParameterDirection.Output);
                data = this.Procedure<SaleTop10>("sp_tbLRatingSale_Top10_WithWeek", param).ToList();
                week = param.Get<int>("NumberWeek");
                return new DataResult<SaleTop10> { Result = data ?? new List<SaleTop10>() };
            }
            catch (Exception ex)
            {
                week = 0;
                return new DataResult<SaleTop10> { Error = true }; ;
            }
        }
    }
}
