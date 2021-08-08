using CRMBussiness.IService;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using CRMModel.Models.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CRMBussiness.ServiceImp
{
    public class TypeCallImp : BaseService<TypeCall, int>, ITypeCall
    {
        #region GetList
        public DataResult<TypeCallViewModel> GetList(string key, int page, int size, out int total)
        {
            List<TypeCallViewModel> data = new List<TypeCallViewModel>();
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
                    using (var multipleresult = db.QueryMultiple("sp_tblTypeCall_GetList",
                        param, commandType: CommandType.StoredProcedure))
                    {
                        data = multipleresult.Read<TypeCallViewModel>().ToList();
                    }
                    total = param.Get<int>("Total");
                }

                return new DataResult<TypeCallViewModel> { Result = data };
            }
            catch(Exception ex)
            {
                return new DataResult<TypeCallViewModel> { Error = true };
            }
        }
        #endregion

        #region GetMaximumCode
        private DataResult<TypeCallViewModel> GetMaximumCode()
        {
            try
            {
                var lst = this.Procedure<TypeCallViewModel>("sp_tblTypeCall_GetBestCode",
                    new { }).ToList();
                return new DataResult<TypeCallViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<TypeCallViewModel> { Error = true };
            }
        }
        #endregion

        #region CreateNewCallType
        public DataResult<TypeCallViewModel> CreateNewCallType(TypeCallViewModel model)
        {
            var getMaxCodeResult = GetMaximumCode();
            if (getMaxCodeResult.Error)
            {
                return new DataResult<TypeCallViewModel> { Error = true };
            }
            else if (getMaxCodeResult.Result == null || getMaxCodeResult.Result.Count == 0)
            {
                model.TypeCallCode = "CUOCGOI0001";
            }
            else
            {
                model.TypeCallCode = Helper.GenerateNextCode(getMaxCodeResult.Result.First().TypeCallCode);
            }

            TypeCall level = new TypeCall();
            level.TypeCallCode = model.TypeCallCode;
            level.NameTypeCall = model.NameTypeCall;
            level.Status = true;

            try
            {
                Raw_Insert(level);

                return new DataResult<TypeCallViewModel> ();
            }
            catch
            {
                return new DataResult<TypeCallViewModel> { Error = true };
            }
        }
        #endregion

        #region GetById
        public DataResult<TypeCallViewModel> GetById(int id)
        {
            try
            {
                var lst = this.Procedure<TypeCallViewModel>("sp_tblTypeCall_GetById", new { @id = id }).ToList();
                return new DataResult<TypeCallViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<TypeCallViewModel> { Error = true };
            }
        }
        #endregion

        #region Update
        public DataResult<TypeCallViewModel> Update(TypeCallViewModel model)
        {
            TypeCall data = new TypeCall();
            data.Id = model.Id;
            data.TypeCallCode = model.TypeCallCode;
            data.NameTypeCall = model.NameTypeCall;
            data.Status = true;
            try
            {
                Raw_Update(data);

                return new DataResult<TypeCallViewModel>();
            }
            catch
            {
                return new DataResult<TypeCallViewModel> { Error = true };
            }
        }
        #endregion

        #region Delete
        public DataResult<TypeCallViewModel> Delete(int id)
        {
            try
            {
                Raw_Delete(id.ToString());
                return new DataResult<TypeCallViewModel>();
            }
            catch
            {
                return new DataResult<TypeCallViewModel> { Error = true };
            }
        }
        #endregion
    }
}
