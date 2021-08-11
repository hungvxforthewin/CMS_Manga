using CMSBussiness.ViewModel;
using CMSModel.Models.Data;
using CRMBussiness;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMSBussiness.IService
{
    public interface IBook : IBaseServices<Book, int>
    {
        DataResult<DisplayBookViewModel> GetList(SearchBookViewModel model, out int total);
        DataResult<BookViewModel> GetById(int id);
    }
}
