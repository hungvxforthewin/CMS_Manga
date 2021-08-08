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
    public class TypeContractImp : BaseService<TypeContract, int>, ITypeContract
    {
        public DataResult<TypeContractViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        #region GetList
        public DataResult<TypeContractViewModel> GetList(SearchTypeContractModel model, out int total)
        {
            List<TypeContractViewModel> data = new List<TypeContractViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", model.Key ?? string.Empty);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("sp_tblTypeContract_GetList",
                        param, commandType: CommandType.StoredProcedure))
                    {
                        data = multipleresult.Read<TypeContractViewModel>().ToList();
                    }
                    total = param.Get<int>("Total");
                }

                return new DataResult<TypeContractViewModel> { Result = data };
            }
            catch
            {
                return new DataResult<TypeContractViewModel> { Error = true };
            }
        }
        #endregion

        #region GetMaximumCode
        private DataResult<TypeContractViewModel> GetMaximumCode()
        {
            try
            {
                var lst = this.Procedure<TypeContractViewModel>("sp_tblTypeContract_GetBestCode",
                    new { }).ToList();
                return new DataResult<TypeContractViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<TypeContractViewModel> { Error = true };
            }
        }
        #endregion

        #region CreateNewContractType
        public DataResult<TypeContractViewModel> CreateNewContractType(TypeContractViewModel model)
        {
            var getMaxCodeResult = GetMaximumCode();
            if (getMaxCodeResult.Error)
            {
                return new DataResult<TypeContractViewModel> { Error = true };
            }
            else if (getMaxCodeResult.Result == null || getMaxCodeResult.Result.Count == 0)
            {
                model.TypeContractCode = "LHD0001";
            }
            else
            {
                model.TypeContractCode = Helper.GenerateNextCode(getMaxCodeResult.Result.First().TypeContractCode);
            }

            TypeContract level = new TypeContract();
            level.TypeContractCode = model.TypeContractCode;
            level.NameType = model.NameType;
            level.Content = model.Content;
            level.Status = true;
            level.CreateDate = DateTime.Now;

            try
            {
                Raw_Insert(level);

                return new DataResult<TypeContractViewModel>();
            }
            catch(Exception ex)
            {
                return new DataResult<TypeContractViewModel> { Error = true };
            }
        }

        public DataResult<TypeContractViewModel> GetById(int id)
        {
            try
            {
                var lst = this.Procedure<TypeContractViewModel>("sp_tblTypeContract_GetById", new { @id = id }).ToList();
                return new DataResult<TypeContractViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<TypeContractViewModel> { Error = true };
            }
        }

        public DataResult<TypeContractViewModel> Update(TypeContractViewModel model)
        {
            TypeContract all = new TypeContract();
            all.Id = model.Id;
            all.TypeContractCode = model.TypeContractCode;
            all.NameType = model.NameType;
            all.Status = true;
            all.CreateDate = DateTime.Now;
            all.Content = model.Content;
            try
            {
                Raw_Update(all);

                return new DataResult<TypeContractViewModel>();
            }
            catch
            {
                return new DataResult<TypeContractViewModel> { Error = true };
            }
        }

        public DataResult<TypeContractViewModel> Delete(int id)
        {
            try
            {
                Raw_Delete(id.ToString());
                return new DataResult<TypeContractViewModel>();
            }
            catch
            {
                return new DataResult<TypeContractViewModel> { Error = true };
            }
        }
        #endregion
    }
}
