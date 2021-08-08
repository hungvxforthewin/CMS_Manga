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

namespace CRMBussiness
{
    public class AllowanceOrDeductImp : BaseService<AllowanceOrDeduct, long>, IAllowanceOrDeduct
    {
        public List<AllowanceOrDeduct> GetDatas(BootstrapTableParam obj, ref int totalRow)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@txtSearch", obj.search);
                param.Add("@pageNumber", obj.pageNumber());
                param.Add("@pageSize", obj.pageSize());
                param.Add("@order", obj.order);
                param.Add("@sort", obj.sort);
                param.Add("@totalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var lst = this.Procedure<AllowanceOrDeduct>("sp_AllowanceOrDeducts_GetData", param).ToList();
                totalRow = param.Get<int>("@totalRecord");
                return lst;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public AllowanceOrDeduct GetLastRow()
        {
            try
            {
                return this.Raw_Query<AllowanceOrDeduct>("SELECT TOP 1 * FROM tblAllowanceOrDeduct WHERE UpOrDown=0 ORDER BY Id DESC").FirstOrDefault();
            }
            catch (Exception ex)
            {
                return new AllowanceOrDeduct();
                throw;
            }
        }

        #region GetAllowanceOrDeductByType - khanhkk
        public DataResult<AllowanceOrDeductViewModel> GetAllowanceOrDeductByType(byte type)
        {
            List<AllowanceOrDeductViewModel> data = new List<AllowanceOrDeductViewModel>();
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("sp_tblAllowanceOrDeduct_GetByType",
                        new { @type = type }, commandType: CommandType.StoredProcedure))
                    {
                        data = multipleresult.Read<AllowanceOrDeductViewModel>().ToList();
                    }
                }

                return new DataResult<AllowanceOrDeductViewModel> { Result = data };
            }
            catch
            {
                return new DataResult<AllowanceOrDeductViewModel> { Error = true };
            }
        }
        #endregion

        #region GetAllowanceOrDeductByTypeHavingPagination - khanhkk
        public DataResult<AllowanceOrDeductViewModel> GetAllowanceOrDeductByTypeHavingPagination(SearchAllowanceInfoModel model, out int total)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@type", model.Type);
            param.Add("@Key", model.Key);
            param.Add("@Start", model.Start);
            param.Add("@Size", model.Size);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            List<AllowanceOrDeductViewModel> data = new List<AllowanceOrDeductViewModel>();
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("sp_tblAllowanceOrDeduct_GetList_ByType",
                        param, commandType: CommandType.StoredProcedure))
                    {
                        data = multipleresult.Read<AllowanceOrDeductViewModel>().ToList();
                    }
                }
                total = param.Get<int>("Total");
                return new DataResult<AllowanceOrDeductViewModel> { Result = data };
            }
            catch
            {
                return new DataResult<AllowanceOrDeductViewModel> { Error = true };
            }
        }
        #endregion

        #region GetMaximumAllowanceCodeByType
        private DataResult<AllowanceOrDeductViewModel> GetMaximumAllowanceCodeByType(byte type)
        {
            try
            {
                //DynamicParameters param = new DynamicParameters();
                //param.Add("@type", type);
                var lst = this.Procedure<AllowanceOrDeductViewModel>("sp_tblAllowanceOrDeduct_GetMaximumCodeByType", new { @type = type }).ToList();
                return new DataResult<AllowanceOrDeductViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<AllowanceOrDeductViewModel> { Error = true };
            }
        }

        public DataResult<AllowanceOrDeductViewModel> CreateSenoirityBonus(AllowanceOrDeductViewModel model)
        {
            model.Type = 1;
            var getMaxCodeResult = GetMaximumAllowanceCodeByType(model.Type.Value);
            if (getMaxCodeResult.Error)
            {
                return new DataResult<AllowanceOrDeductViewModel> { Error = true };
            }
            else if (getMaxCodeResult.Result == null || getMaxCodeResult.Result.Count == 0)
            {
                model.AllowanceCode = "THAMNIEN0001";
            }
            else
            {
                model.AllowanceCode = Helper.GenerateNextCode(getMaxCodeResult.Result.First().AllowanceCode);
            }
            AllowanceOrDeduct all = new AllowanceOrDeduct();
            all.UpOrDown = true;
            all.Calculation = true;
            all.Status = true;
            all.Note = model.Note;
            all.Type = model.Type;
            all.AllowanceName = model.AllowanceName;
            all.AllowanceCode = model.AllowanceCode;
            all.AllowanceAmount = model.AllowanceAmount.Value;
            try
            {
                Raw_Insert(all);

                return new DataResult<AllowanceOrDeductViewModel>();
            }
            catch (Exception ex)
            {
                return new DataResult<AllowanceOrDeductViewModel> { Error = true };
            }
        }
        #endregion

        #region UpdateSenoirityBonus
        public DataResult<AllowanceOrDeductViewModel> UpdateSenoirityBonus(AllowanceOrDeductViewModel model)
        {
            AllowanceOrDeduct all = new AllowanceOrDeduct();
            all.UpOrDown = true;
            all.Calculation = true;
            all.Status = true;
            all.Note = model.Note;
            all.Type = 1;
            all.AllowanceName = model.AllowanceName;
            all.AllowanceCode = model.AllowanceCode;
            all.AllowanceAmount = model.AllowanceAmount.Value;
            all.AllwancePercent = model.AllwancePercent;
            all.Id = model.Id;
            try
            {
                Raw_Update(all);

                return new DataResult<AllowanceOrDeductViewModel>();
            }
            catch
            {
                return new DataResult<AllowanceOrDeductViewModel> { Error = true };
            }
        }
        #endregion

        #region GetById
        public DataResult<AllowanceOrDeductViewModel> GetAllowanceOrDeductById(long id)
        {
            try
            {
                var lst = this.Procedure<AllowanceOrDeductViewModel>("sp_tblAllowanceOrDeduct_GetById", new { @id = id }).ToList();
                return new DataResult<AllowanceOrDeductViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<AllowanceOrDeductViewModel> { Error = true };
            }
        }
        #endregion

        #region DeleteAllowanceOrDeduct
        public DataResult<AllowanceOrDeductViewModel> DeleteAllowanceOrDeduct(long id)
        {
            try
            {
                Raw_Delete(id.ToString());
                return new DataResult<AllowanceOrDeductViewModel>();
            }
            catch
            {
                return new DataResult<AllowanceOrDeductViewModel> { Error = true };
            }
        }
        #endregion
    }
}
