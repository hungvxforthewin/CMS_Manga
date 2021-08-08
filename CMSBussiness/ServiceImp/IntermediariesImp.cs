using CRMBussiness.IService;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using CRMModel.Models.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CRMBussiness.ServiceImp
{
    public class IntermediariesImp : BaseService<Intermediaries, long>, IIntermediaries
    {
        public DataResult<IntermediariesViewModel> CheckByPhone(string phone)
        {
            try
            {
                var data = this.Raw_Query<IntermediariesViewModel>("SELECT * FROM tblIntermediaries WHERE Phone = @Phone", new Dictionary<string, object>() {
                    {"Phone", phone }
                }).SingleOrDefault();
                return new DataResult<IntermediariesViewModel>() { Error = false, DataItem = data };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataResult<IntermediariesViewModel> CheckByTaxCode(string taxCode)
        {
            try
            {
                var data = this.Raw_Query<IntermediariesViewModel>("SELECT * FROM tblIntermediaries WHERE TaxCode = @TaxCode", new Dictionary<string, object>() {
                    {"TaxCode", taxCode }
                }).SingleOrDefault();
                return new DataResult<IntermediariesViewModel>() { Error = false, DataItem = data };
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
