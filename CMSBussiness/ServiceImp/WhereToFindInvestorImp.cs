using CRMBussiness.IService;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using CRMModel.Models.Data;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CRMBussiness.ServiceImp
{
    public class WhereToFindInvestorImp : BaseService<WhereToFindInvestor, int>, IWhereToFindInvestor
    {
        #region GetList
        public DataResult<WhereToFindInvestorViewModel> GetList(string key, int page, int size, out int total)
        {
            List<WhereToFindInvestorViewModel> data = new List<WhereToFindInvestorViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", key ?? string.Empty);
            param.Add("@Page", page);
            param.Add("@Size", size);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (var multipleresult = db.QueryMultiple("sp_tblWhereToFindInvestor_GetList",
                        param, commandType: CommandType.StoredProcedure))
                    {
                        data = multipleresult.Read<WhereToFindInvestorViewModel>().ToList();
                    }
                    total = param.Get<int>("Total");
                }

                return new DataResult<WhereToFindInvestorViewModel> { Result = data };
            }
            catch
            {
                return new DataResult<WhereToFindInvestorViewModel> { Error = true };
            }
        }
        #endregion

        #region GetMaximumCode
        private DataResult<WhereToFindInvestorViewModel> GetMaximumCode()
        {
            try
            {
                var lst = this.Procedure<WhereToFindInvestorViewModel>("sp_tblWhereToFindInvestor_GetBestCode",
                    new { }).ToList();
                return new DataResult<WhereToFindInvestorViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<WhereToFindInvestorViewModel> { Error = true };
            }
        }
        #endregion

        #region CreateNewLevel
        public DataResult<WhereToFindInvestorViewModel> CreateNewResource(WhereToFindInvestorViewModel model)
        {
            var getMaxCodeResult = GetMaximumCode();
            if (getMaxCodeResult.Error)
            {
                return new DataResult<WhereToFindInvestorViewModel> { Error = true };
            }
            else if (getMaxCodeResult.Result == null || getMaxCodeResult.Result.Count == 0)
            {
                model.InvestorResourceCode = "NGUON0001";
            }
            else
            {
                model.InvestorResourceCode = Helper.GenerateNextCode(getMaxCodeResult.Result.First().InvestorResourceCode);
            }

            WhereToFindInvestor level = new WhereToFindInvestor();
            level.InvestorResourceCode = model.InvestorResourceCode;
            level.AddressFind = model.AddressFind;
            level.Bytele = false;

            try
            {
                Raw_Insert(level);

                return new DataResult<WhereToFindInvestorViewModel>();
            }
            catch
            {
                return new DataResult<WhereToFindInvestorViewModel> { Error = true };
            }
        }

        public DataResult<WhereToFindInvestorViewModel> GetById(int id)
        {
            try
            {
                var lst = this.Procedure<WhereToFindInvestorViewModel>("sp_tblWhereToFindInvestor_GetById", new { @id = id }).ToList();
                return new DataResult<WhereToFindInvestorViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<WhereToFindInvestorViewModel> { Error = true };
            }
        }

        public DataResult<WhereToFindInvestorViewModel> Update(WhereToFindInvestorViewModel model)
        {
            WhereToFindInvestor data = new WhereToFindInvestor();
            data.Id = model.Id;
            data.InvestorResourceCode = model.InvestorResourceCode;
            data.AddressFind = model.AddressFind;
            data.Bytele = false;
            try
            {
                Raw_Update(data);

                return new DataResult<WhereToFindInvestorViewModel>();
            }
            catch
            {
                return new DataResult<WhereToFindInvestorViewModel> { Error = true };
            }
        }

        public DataResult<WhereToFindInvestorViewModel> Delete(int id)
        {
            try
            {
                Raw_Delete(id.ToString());
                return new DataResult<WhereToFindInvestorViewModel>();
            }
            catch
            {
                return new DataResult<WhereToFindInvestorViewModel> { Error = true };
            }
        }
        #endregion

        #region GetInvestResourceList
        public DataResult<WhereToFindInvestorViewModel> GetInvestResourceList()
        {
            try
            {
                var lst = this.Procedure<WhereToFindInvestorViewModel>("sp_tblWhereToFindInvestor_GetInvestorResourceList",
                    new { }).ToList();
                return new DataResult<WhereToFindInvestorViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<WhereToFindInvestorViewModel> { Error = true };
            }
        }
        #endregion

        #region GetByName
        public DataResult<WhereToFindInvestorViewModel> GetByName(string name)
        {
            try
            {
                var lst = this.Procedure<WhereToFindInvestorViewModel>("sp_tblWhereToFindInvestor_GetByName", new { @name = name }).ToList();
                return new DataResult<WhereToFindInvestorViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<WhereToFindInvestorViewModel> { Error = true };
            }
        }
        #endregion
    }
}