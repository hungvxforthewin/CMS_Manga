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
    public class ContractInvesterImp : BaseService<ContractInvestor, int>, IContractInvester
    {
        public int CountDateContract(string CodeContract)
        {
            try
            {
                var count = this.Raw_Query<ContractInvestor>("SELECT * FROM tblContractInvestors WHERE CodeContract LIKE @CodeContract AND CodeIntermediaries IS NULL ", new Dictionary<string, object>() {
                    {"CodeContract", string.Concat( "%", CodeContract)}
                }).ToList();
                return count.Count;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int CountDateContractForAmber(string CodeContract, string Code = "Amber")
        {
            try
            {
                var count = this.Raw_Query<ContractInvestor>("SELECT * FROM tblContractInvestors WHERE CodeIntermediaries = @Code ", new Dictionary<string, object>() {
                    {"CodeContract", string.Concat( "%", CodeContract)},
                    {"Code", Code }
                }).ToList();
                return count.Count;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int CountInntermediariesContract(string CodeIntermediaries)
        {
            try
            {
                var count = this.Raw_Query<ContractInvestor>("SELECT * FROM tblContractInvestors WHERE CodeIntermediaries = @CodeIntermediaries ", new Dictionary<string, object>() {
                    {"CodeIntermediaries", CodeIntermediaries}
                }).ToList();
                return count.Count;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public ContractInvestor GetByCode(string CodeContract)
        {
            try
            {
                return this.Raw_Query<ContractInvestor>("SELECT Id FROM tblContractInvestors WHERE CodeContract = @codeContract", new Dictionary<string, object>() {
                    {"codeContract", CodeContract }
                }).SingleOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ContractInvesterViewModel GetInfoByCodeContract(string code)
        {
            try
            {
                ContractInvesterViewModel data = new ContractInvesterViewModel();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CodeContract", code);
                data = this.Raw_Query<ContractInvesterViewModel>("SELECT Id, InvestmentAmount FROM tblContractInvestors WHERE CodeContract = @code", new Dictionary<string, object>() {
                    {"code", code }
                }).FirstOrDefault();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataResult<ContractInvesterViewModel> GetInfoById(int id)
        {
            try
            {
                List<ContractInvesterViewModel> Ac = new List<ContractInvesterViewModel>();
                try
                {
                    using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                    {
                        using (var multipleresult = db.QueryMultiple("sp_tblContractInvestor_GeInfo_ById",
                            new { @id = id },
                            commandType: CommandType.StoredProcedure))
                        {
                            Ac = multipleresult.Read<ContractInvesterViewModel>().ToList();
                        }
                    }

                    return new DataResult<ContractInvesterViewModel> { Error = false, Result = Ac };
                }
                catch (Exception ex)
                {
                    return new DataResult<ContractInvesterViewModel> { Error = true };
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DataResult<DisplayContractInvesterTableViewModel> GetList(SearchContractInvesterViewModel model, out int total)
        {
            List<DisplayContractInvesterTableViewModel> data = new List<DisplayContractInvesterTableViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", model.Key ?? string.Empty);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@Sale", model.Sale);
            param.Add("@TeleSale", model.TeleSale);
            param.Add("@Resource", model.Resource);
            param.Add("@Status", model.Status);
            param.Add("@BranchCode", model.BranchCode);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    data = this.Procedure<DisplayContractInvesterTableViewModel>("sp_tblContractInvester_GetList", param).ToList();
                    total = param.Get<int>("Total");
                }
                return new DataResult<DisplayContractInvesterTableViewModel> { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<DisplayContractInvesterTableViewModel> { Error = true };
            }
        }

        public DataResult<InforBill> GetListBill(int id)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
                var data = this.Procedure<InforBill>("sp_tblContractInvestor_GetBill", parameters).ToList();
                return new DataResult<InforBill>() { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<InforBill>();
            }
        }

        public DataResult<DisplayContractInvesterTableViewModel> GetListByWaitPayDone(SearchContractInvestorInstallmentsViewModel model, out int total)
        {
            List<DisplayContractInvesterTableViewModel> data = new List<DisplayContractInvesterTableViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", model.Key ?? string.Empty);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@Sale", model.Sale);
            param.Add("@TeleSale", model.TeleSale);
            param.Add("@Status", model.Status);
            param.Add("@Amber", model.Amber);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    data = this.Procedure<DisplayContractInvesterTableViewModel>("sp_tblContractInvester_GetList_ByAwaitPayDone", param).ToList();
                    total = param.Get<int>("Total");
                }
                return new DataResult<DisplayContractInvesterTableViewModel> { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<DisplayContractInvesterTableViewModel> { Error = true };
            }
        }

        public bool UpdateDepositCode(int id, string depositCode)
        {
            try
            {
                this.Raw_Query<ContractInvesterViewModel>("UPDATE tblContractInvestors SET CodeDeposit = @code WHERE Id = @Id", new Dictionary<string, object>() {
                    {"Id", id },
                    {"code", depositCode}
                }).SingleOrDefault();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool UpdateNewCodeContract(int id, string newCodeContract)
        {
            try
            {
                this.Raw_Query<ContractInvesterViewModel>("UPDATE tblContractInvestors SET CodeContract = @NewCodeContract WHERE Id = @Id", new Dictionary<string, object>() {
                    {"NewCodeContract", newCodeContract },
                    {"Id", id }
                });
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool UpdatePayDone(int id, DateTime? approvedDate)
        {
            try
            {
                this.Raw_Query<ContractInvesterViewModel>("UPDATE tblContractInvestors SET IdStatusContract = 'PayDone', DatePaydone = @DateNow WHERE Id = @Id", new Dictionary<string, object>() {
                    { "Id", id },
                    { "DateNow", approvedDate??DateTime.Now }
                }).SingleOrDefault();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region UpdateSignature - khanhkk
        public bool UpdateSignature(int id, string signaturePath)
        {
            try
            {
                this.Raw_Query<ContractInvesterViewModel>("UPDATE tblContractInvestors SET InvestorSignature = @signature WHERE Id = @Id", new Dictionary<string, object>() {
                    {"Id", id },
                    {"signature", signaturePath}
                });
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
    public class CountContractViewModel
    {
        public int CountContract { get; set; }
    }
}
