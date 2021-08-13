using CMSBussiness.IService;
using CMSBussiness.ViewModel;
using CMSModel.Models.Data;
using CRMBussiness;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CMSBussiness.ServiceImp
{
    public class BookImp : BaseService<Book, int>, IBook
    {
        public DataResult<BookViewModel> GetById(int id)
        {
            try
            {
                BookViewModel data = new BookViewModel();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", id);
                data = this.Procedure<BookViewModel>("SP_Book_GetById", param).SingleOrDefault();
                return new DataResult<BookViewModel> { DataItem = data ?? new BookViewModel() };
            }
            catch (Exception ex)
            {
                return new DataResult<BookViewModel> { Error = true };
            }
        }

        public DataResult<DisplayBookViewModel> GetList(SearchBookViewModel model, out int total)
        {
            List<DisplayBookViewModel> data = new List<DisplayBookViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", model.Key);
            param.Add("@CategoryId", model.CategoryId);
            param.Add("@SexId", model.SexId);
            param.Add("@DateStart", model.DateStart);
            param.Add("@DateEnd", model.DateEnd);
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
                    data = this.Procedure<DisplayBookViewModel>("SP_Book_GetList", param).ToList();
                    total = param.Get<int>("Total");
                }
                return new DataResult<DisplayBookViewModel> { Result = data ?? new List<DisplayBookViewModel>() };
            }
            catch (Exception ex)
            {
                return new DataResult<DisplayBookViewModel> { Error = true };
            }
        }
    }
}
