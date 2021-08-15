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
    public class BookImageImp : BaseService<BookImage, int>, IBookImage
    {
        //public DataResult<BookImageViewModel> GetById(int id)
        //{
        //    try
        //    {
        //        BookImageViewModel data = new BookImageViewModel();
        //        DynamicParameters param = new DynamicParameters();
        //        param.Add("@Id", id);
        //        data = this.Procedure<BookImageViewModel>("SP_BookImage_GetById", param).SingleOrDefault();
        //        return new DataResult<BookImageViewModel> { DataItem = data ?? new BookImageViewModel() };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new DataResult<BookImageViewModel> { Error = true };
        //    }
        //}

        public DataResult<DisplayBookImageViewModel> GetList(SearchBookImageViewModel model, out int total)
        {
            List<DisplayBookImageViewModel> data = new List<DisplayBookImageViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", model.Key);
            param.Add("@IsBanner", model.IsBanner);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    data = this.Procedure<DisplayBookImageViewModel>("SP_BookImage_GetList", param).ToList();
                    total = param.Get<int>("Total");
                }
                return new DataResult<DisplayBookImageViewModel> { Result = data ?? new List<DisplayBookImageViewModel>() };
            }
            catch (Exception ex)
            {
                return new DataResult<DisplayBookImageViewModel> { Error = true };
            }
        }
    }
}
