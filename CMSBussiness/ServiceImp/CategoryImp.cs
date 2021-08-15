using CMSBussiness.IService;
using CMSBussiness.ViewModel;
using CMSModel.Models.Data;
using CRMBussiness;
using CRMBussiness.LIB;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CMSBussiness.ServiceImp
{
    public class CategoryImp : BaseService<Category, int>, ICategory
    {
        public DataResult<CategoryViewModel> GetAll()
        {
            try
            {
                List<CategoryViewModel> data = new List<CategoryViewModel>();
                data = this.Raw_Query<CategoryViewModel>("SELECT * FROM Category WHERE isActive = 1").ToList();
                return new DataResult<CategoryViewModel>() { Result = data };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataResult<CategoryViewModel> GetList(SearchCategoryViewModel model, out int total)
        {
            List<CategoryViewModel> data = new List<CategoryViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", model.Key);
            param.Add("@Status", model.Status);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    data = this.Procedure<CategoryViewModel>("SP_CMS_Category_GetList_Paging", param).ToList();
                    total = param.Get<int>("Total");
                }
                return new DataResult<CategoryViewModel> { Result = data ?? new List<CategoryViewModel>() };
            }
            catch (Exception ex)
            {
                return new DataResult<CategoryViewModel> { Error = true };
            }
        }
    }
}
