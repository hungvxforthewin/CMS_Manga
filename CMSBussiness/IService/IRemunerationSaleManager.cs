using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using Remuneration = CRMModel.Models.Data.Remuneration;


namespace CRMBussiness.IService
{
    public interface IRemunerationSaleManager : IBaseServices<Remuneration, byte>
    {
        List<Remuneration> GetByRole(int roleAccount);
    }
}
