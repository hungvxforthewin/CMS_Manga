using CMSBussiness.ViewModel;
using CMSModel.Models.Data;
using CRMBussiness;
using CRMBussiness.LIB;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMSBussiness.IService
{
    public interface IBookImage : IBaseServices<BookImage, int>
    {
        DataResult<DisplayBookImageViewModel> GetList(SearchBookImageViewModel model, out int total);
        DataResult<BookImageViewModel> GetDetail(int bookId, int imgId);
        bool DeleteById(int bookId, int imgId);
        //DataResult<BookImageViewModel> GetById(int id);
        bool UpdateBanner(int bookId, int imgId, string imgUrl, bool isBanner, int bookIdOld);

    }
}
