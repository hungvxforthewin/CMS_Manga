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
    public class DepositImp : BaseService<Deposit, long>, IDeposit
    {
        public Deposit GetByCode(string codeDeposit)
        {
            try
            {
                return this.Raw_Query<Deposit>("SELECT Id FROM tblDeposit WHERE CodeDeposit = @codeDeposit", new Dictionary<string, object>() {
                    {"codeDeposit", codeDeposit }
                }).SingleOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataResult<DepositAgreementViewModel> GetInfoById(int id)
        {
            try
            {
                List<DepositAgreementViewModel> Ac = new List<DepositAgreementViewModel>();
                try
                {
                    using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                    {
                        using (var multipleresult = db.QueryMultiple("sp_tblDeposit_GeInfo_ById",
                            new { @id = id },
                            commandType: CommandType.StoredProcedure))
                        {
                            Ac = multipleresult.Read<DepositAgreementViewModel>().ToList();
                        }
                    }

                    return new DataResult<DepositAgreementViewModel> { Error = false, Result = Ac };
                }
                catch (Exception ex)
                {
                    return new DataResult<DepositAgreementViewModel> { Error = true };
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataResult<DepositAgreementTableViewModel> GetList(SearchDepositAAgreementViewModel model, out int total)
        {
            List<DepositAgreementTableViewModel> data = new List<DepositAgreementTableViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", model.Key ?? string.Empty);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@DateFrom", model.DateFrom);
            param.Add("@DateTo", model.DateTo);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    data = this.Procedure<DepositAgreementTableViewModel>("sp_tblDeposit_GetList", param).ToList();
                    total = param.Get<int>("Total");
                }
                return new DataResult<DepositAgreementTableViewModel> { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<DepositAgreementTableViewModel> { Error = true };
            }
        }

        public DataResult<InforDepositBill> GetListBill(int id)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
                var data = this.Procedure<InforDepositBill>("sp_tblDeposit_GetBill", parameters).ToList();
                return new DataResult<InforDepositBill>() { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<InforDepositBill>();
            }
        }
    }
}
