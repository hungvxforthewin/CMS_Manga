using CMSModel.Models.Data;
using CRMBussiness.LIB;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.ViewModel;
using CRMBussiness;
using CMSBussiness.ViewModel;

namespace CMSBussiness.IService
{
    public interface ICategory : IBaseServices<Category, int>
    {
        DataResult<CategoryViewModel> GetList(SearchCategoryViewModel model, out int total);
        DataResult<CategoryViewModel> GetAll();

    }
}
