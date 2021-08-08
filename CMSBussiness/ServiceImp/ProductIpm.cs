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
using System.Threading.Tasks;

namespace CRMBussiness.ServiceImp
{
    public class ProductIpm : BaseService<Product, long>, IProduct
    {
        public bool CheckExists(string code, ref int total)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@code", code);
                param.Add("@totalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var lst = this.Procedure<Product>("sp_Products_CheckExists", param).FirstOrDefault();
                total = param.Get<int>("totalRecord");
                if(lst != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<Product> GetData(BootstrapTableParam obj, ref int total)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@txtSearch", obj.search);
                param.Add("@pageNumber", obj.pageNumber());
                param.Add("@pageSize", obj.pageSize());
                param.Add("@order", obj.order);
                param.Add("@sort", obj.sort);
                param.Add("@totalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var lst = this.Procedure<Product>("sp_Products_GetData", param).ToList();
                total = param.Get<int>("@totalRecord");
                return lst;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #region GetProducts - khanhkk
        public DataResult<SelectProductViewModel> GetProducts()
        {
            try
            {
                var lst = this.Procedure<SelectProductViewModel>("sp_tblProduct_GetProducts").ToList();
                return new DataResult<SelectProductViewModel>() { Result = lst };
            }
            catch
            {
                return new DataResult<SelectProductViewModel>() { Error = true }; ;
            }
        }
        #endregion
    }
}
