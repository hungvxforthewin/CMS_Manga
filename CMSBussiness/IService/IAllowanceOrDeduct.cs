using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
﻿using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface IAllowanceOrDeduct : IBaseServices<AllowanceOrDeduct, long>
    {
        List<AllowanceOrDeduct> GetDatas(BootstrapTableParam obj, ref int totalRow);

        AllowanceOrDeduct GetLastRow();

        //DataResult<AllowanceOrDeductViewModel> GetSocialInsurance();

        //DataResult<AllowanceOrDeductViewModel> GetSeniorityBonus();

        DataResult<AllowanceOrDeductViewModel> GetAllowanceOrDeductByType(byte type);

        DataResult<AllowanceOrDeductViewModel> GetAllowanceOrDeductById(long id);

        DataResult<AllowanceOrDeductViewModel> GetAllowanceOrDeductByTypeHavingPagination(SearchAllowanceInfoModel model, out int total);

        DataResult<AllowanceOrDeductViewModel> CreateSenoirityBonus(AllowanceOrDeductViewModel model);

        DataResult<AllowanceOrDeductViewModel> UpdateSenoirityBonus(AllowanceOrDeductViewModel model);

        DataResult<AllowanceOrDeductViewModel> DeleteAllowanceOrDeduct(long id);
    }
}
