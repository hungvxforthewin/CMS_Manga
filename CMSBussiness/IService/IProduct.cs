using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;

namespace CRMBussiness.IService
{
    public interface IProduct : IBaseServices<Product, long>
    {
        List<Product> GetData(BootstrapTableParam obj, ref int total);
        bool CheckExists(string code, ref int total);

        DataResult<SelectProductViewModel> GetProducts();
    }
}
