using CRMModel.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using Kpi = CRMModel.Models.Data.Kpi;

namespace CRMBussiness.IService
{
    public interface IKpiSaleManager : IBaseServices<Kpi, long>
    {
        List<Kpi> GetByRole(int roleAccount);
        bool DeleteAllByRole(byte role);

    }
}
