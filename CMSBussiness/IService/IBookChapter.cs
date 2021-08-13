using CMSBussiness.ViewModel;
using CMSModel.Models.Data;
using CRMBussiness;
using CRMBussiness.LIB;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMSBussiness.IService
{
    public interface IBookChapter : IBaseServices<BookChapter, int>
    {
        DataResult<DisplayBookChapterViewModel> GetList(SearchBookChapterViewModel model, out int total);
        DataResult<BookChapterViewModel> GetById(int id);
    }
}
