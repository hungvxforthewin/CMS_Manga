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
    public class InvestorImp : BaseService<Investor, long>, IInvestor
    {
        public InvestorViewModel CheckSumAmountDeposit(string phone)
        {
            try
            {
                InvestorViewModel data = new InvestorViewModel();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@phone", phone);
                data = this.Procedure<InvestorViewModel>("sp_tblDeposit_SumAmountDeposit", parameters).SingleOrDefault();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public InvestorViewModel CheckSumAmountDepositWithContract(string phone, string codeContract)
        {
            try
            {
                try
                {
                    InvestorViewModel data = new InvestorViewModel();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@phone", phone);
                    parameters.Add("@codeContract", codeContract);
                    data = this.Procedure<InvestorViewModel>("sp_tblDeposit_SumAmountDepositWithContract", parameters).SingleOrDefault();
                    return data;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public InvestorViewModel GetByPhone(string phone)
        {
            try
            {
                return this.Raw_Query<InvestorViewModel>("SELECT * FROM tblInvestors WHERE PhoneNumber = @Phone", new Dictionary<string, object>() {
                    {"Phone", phone }
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //throw ex;
                return new InvestorViewModel();
            }
        }

        public List<Investor> GetDatas(BootstrapTableParam obj, ref int totalRow, int status)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@txtSearch", obj.search);
                param.Add("@status", status);
                param.Add("@pageNumber", obj.pageNumber());
                param.Add("@pageSize", obj.pageSize());
                param.Add("@order", obj.order);
                param.Add("@sort", obj.sort);
                param.Add("@totalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var lst = this.Procedure<Investor>("sp_Investor_GetData", param).ToList();
                totalRow = param.Get<int>("@totalRecord");
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region GetGoWithPersons
        public DataResult<GoWithInvestorModel> GetGoWithPersons(string checkinCode)
        {
            try
            {
                var lst = this.Procedure<GoWithInvestorModel>("sp_tblInvestor_GetGoWithByCheckInCode", new { @code = checkinCode }).ToList();
                return new DataResult<GoWithInvestorModel> { Result = lst };
            }
            catch (Exception ex)
            {
                return new DataResult<GoWithInvestorModel>{ Error = true };
            }
        }

        public bool UpdateAccountBank2(string bank, string investorCode)
        {
            try
            {
                 this.Raw_Query<InvestorViewModel>("UPDATE tblInvestors SET AccountBank2 = @Bank WHERE CodeInvestor = @CodeInvestor", new Dictionary<string, object>() {
                    {"Bank", bank },
                    {"CodeInvestor", investorCode }
                }).FirstOrDefault();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateByPhone(string phone, string name)
        {
            try
            {
                this.Raw_Query<InvestorViewModel>("UPDATE tblInvestors SET Name = @Name WHERE PhoneNumber = @Phone", new Dictionary<string, object>() {
                    {"Phone", phone },
                    {"Name", name }
                }).FirstOrDefault();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
