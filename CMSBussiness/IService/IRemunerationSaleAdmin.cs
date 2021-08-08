using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using Remuneration = CRMModel.Models.Data.Remuneration;

namespace CRMBussiness.IService
{
    //HungVX Sale Admin
    public interface IRemunerationSaleAdmin : IBaseServices<Remuneration, byte>
    {
        bool DeleteAllByRole(byte role);
    }
}
