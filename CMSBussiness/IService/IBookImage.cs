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
        //DataResult<BookImageViewModel> GetById(int id);
    }
}
