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
    public class BookChapterImp : BaseService<BookChapter, int>, IBookChapter
    {
        public DataResult<BookChapterViewModel> GetById(Guid id)
        {
            try
            {
                BookChapterViewModel data = new BookChapterViewModel();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", id);
                data = this.Procedure<BookChapterViewModel>("SP_BookChapter_GetById", param).SingleOrDefault();
                return new DataResult<BookChapterViewModel> { DataItem = data ?? new BookChapterViewModel() };
            }
            catch (Exception ex)
            {
                return new DataResult<BookChapterViewModel> { Error = true };
            }
        }

        public DataResult<DisplayBookChapterViewModel> GetList(SearchBookChapterViewModel model, out int total)
        {
            List<DisplayBookChapterViewModel> data = new List<DisplayBookChapterViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", model.Key);
            param.Add("@BookName", model.BookName);
            param.Add("@ChapterStatus", model.ChapterStatus);
            param.Add("@DateStart", model.DateStart);
            param.Add("@DateEnd", model.DateEnd);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    data = this.Procedure<DisplayBookChapterViewModel>("SP_BookChapter_GetList", param).ToList();
                    total = param.Get<int>("Total");
                }
                return new DataResult<DisplayBookChapterViewModel> { Result = data ?? new List<DisplayBookChapterViewModel>() };
            }
            catch (Exception ex)
            {
                return new DataResult<DisplayBookChapterViewModel> { Error = true };
            }
        }
    }
}
