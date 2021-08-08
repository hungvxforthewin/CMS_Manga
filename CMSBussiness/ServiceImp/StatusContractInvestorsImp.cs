using System;
using System.Linq;
using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using CRMBussiness.IService;
using CRMBussiness.LIB;
using CRMModel.Models.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CRMBussiness.ViewModel;
using System.Data.SqlClient;

namespace CRMBussiness.ServiceImp
{
    public class StatusContractInvestorsImp : BaseService<StatusContractInvestor, string>, IStatusContractInvestors
    {
        public StatusContractInvestor InsertData(StatusContractInvestor model, ref string statusContractId)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@IdStatusContract", model.IdStatusContract);
                param.Add("@NameStatus", model.NameStatus);
                param.Add("@CreateDate", model.CreateDate);
                param.Add("@Description", model.Description);
                param.Add("@Status", model.Status);
                //param.Add("@PaymentFormat", model.PaymentFormat);
                param.Add("@idStatus", dbType: DbType.String, direction: ParameterDirection.Output,size: 50);
                var lst = this.Procedure<StatusContractInvestor>("sp_tblStatusContractInvestors_Insert", param).ToList();
                statusContractId = param.Get<string>("@idStatus");
                model.IdStatusContract = statusContractId;
                return model;
            }
            catch (Exception ex)
            {
                return new StatusContractInvestor();
            }
        }
    }
}
