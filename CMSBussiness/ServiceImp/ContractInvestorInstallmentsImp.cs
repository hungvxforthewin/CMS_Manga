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
    public class ContractInvestorInstallmentsImp : BaseService<ContractInvestorInstallments, int>, IContractInvestorInstallments
    {
        public bool DeleteByCodeDeposit(string CodeDeposit)
        {
            try
            {
                this.Raw_Query<ContractInvestorInstallments>("DELETE FROM tblContractInvestorInstallments WHERE DepositCode = @code", new Dictionary<string, object>() {
                    {"code", CodeDeposit }
                }).FirstOrDefault();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteByContract(string CodeContract)
        {
            try
            {
                this.Raw_Query<ContractInvestorInstallments>("DELETE FROM tblContractInvestorInstallments WHERE CodeContract = @CodeContract AND DepositCode IS NULL", new Dictionary<string, object>() {
                    {"CodeContract", CodeContract }
                }).FirstOrDefault();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public ContractInvestorInstallments GetLastStatus(string CodeContract)
        {
            try
            {
                ContractInvestorInstallments data = new ContractInvestorInstallments();
                data = this.Raw_Query<ContractInvestorInstallments>("SELECT * FROM tblContractInvestorInstallments WHERE CodeContract = @CodeContract ORDER BY Id DESC", new Dictionary<string, object>() {
                    {"CodeContract", CodeContract }
                }).FirstOrDefault();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool UpdateCodeContract(string CodeDeposit, string CodeContract)
        {
            try
            {
                this.Raw_Query<ContractInvesterViewModel>("UPDATE tblContractInvestorInstallments SET CodeContract = @CodeContract WHERE CodeDeposit = @CodeDeposit ", new Dictionary<string, object>() {
                    {"CodeDeposit", CodeDeposit },
                    {"CodeContract", CodeContract }
                }).SingleOrDefault();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateNewCodeContract(string CodeContract, string NewCodeContract)
        {
            try
            {
                this.Raw_Query<ContractInvesterViewModel>("UPDATE tblContractInvestorInstallments SET CodeContract = @NewCodeContract WHERE CodeContract = @CodeContract ", new Dictionary<string, object>() {
                    {"NewCodeContract", NewCodeContract },
                    {"CodeContract", CodeContract }
                }).SingleOrDefault();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool UpdatePayDone(string CodeContract)
        {
            try
            {
                this.Raw_Query<ContractInvesterViewModel>("UPDATE tblContractInvestorInstallments SET IdStatusContract = 'PayDone' WHERE CodeContract = @CodeContract ", new Dictionary<string, object>() {
                    {"CodeContract", CodeContract }
                }).SingleOrDefault();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
