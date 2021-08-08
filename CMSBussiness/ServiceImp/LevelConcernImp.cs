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
    public class LevelConcernImp : BaseService<LevelConcern, int>, ILevelConcern
    {
        #region GetList
        public DataResult<LevelConcernViewModel> GetList(string key, int page, int size, out int total)
        {
            List<LevelConcernViewModel> data = new List<LevelConcernViewModel>();
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
                    data = this.Procedure<LevelConcernViewModel>("sp_tblLevelConcern_GetList", param).ToList();
                    total = param.Get<int>("Total");
                }

                return new DataResult<LevelConcernViewModel> { Result = data };
            }
            catch(Exception ex)
            {
                return new DataResult<LevelConcernViewModel> { Error = true };
            }
        }
        #endregion

        #region GetMaximumCode
        private DataResult<LevelConcernViewModel> GetMaximumCode()
        {
            try
            {
                var lst = this.Procedure<LevelConcernViewModel>("sp_tblLevelConcern_GetBestCode", 
                    new {}).ToList();
                return new DataResult<LevelConcernViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<LevelConcernViewModel> { Error = true };
            }
        }
        #endregion

        #region CreateNewLevel
        public DataResult<LevelConcernViewModel> CreateNewLevel(LevelConcernViewModel model)
        {
            var getMaxCodeResult = GetMaximumCode();
            if (getMaxCodeResult.Error)
            {
                return new DataResult<LevelConcernViewModel> { Error = true };
            }
            else if (getMaxCodeResult.Result == null || getMaxCodeResult.Result.Count == 0)
            {
                model.LevelConcernCode = "QTAM0001";
            }
            else
            {
                model.LevelConcernCode = Helper.GenerateNextCode(getMaxCodeResult.Result.First().LevelConcernCode);
            }

            LevelConcern level = new LevelConcern();
            level.LevelConcernCode = model.LevelConcernCode;
            level.NameConcern = model.NameConcern;
            level.CreateDate = DateTime.Now;
            level.Status = true;
            
            try
            {
                Raw_Insert(level);

                return new DataResult<LevelConcernViewModel>();
            }
            catch
            {
                return new DataResult<LevelConcernViewModel> { Error = true };
            }
        }

        public DataResult<LevelConcernViewModel> GetById(int id)
        {
            try
            {
                var lst = this.Procedure<LevelConcernViewModel>("sp_tblLevelConcern_GetById", new { @id = id }).ToList();
                return new DataResult<LevelConcernViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<LevelConcernViewModel> { Error = true };
            }
        }

        public DataResult<LevelConcernViewModel> Update(LevelConcernViewModel model)
        {
            LevelConcern all = new LevelConcern();
            all.Id = model.Id;
            all.LevelConcernCode = model.LevelConcernCode;
            all.NameConcern = model.NameConcern;
            all.Status = true;
            try
            {
                Raw_Update(all);

                return new DataResult<LevelConcernViewModel>();
            }
            catch
            {
                return new DataResult<LevelConcernViewModel> { Error = true };
            }
        }

        public DataResult<LevelConcernViewModel> Delete(int id)
        {
            try
            {
                Raw_Delete(id.ToString());
                return new DataResult<LevelConcernViewModel>();
            }
            catch
            {
                return new DataResult<LevelConcernViewModel> { Error = true };
            }
        }

        public DataResult<LevelConcernViewModel> GetAll()
        {
            try
            {
                var data = this.Raw_Query<LevelConcernViewModel>("SELECT Id, NameConcern, LevelConcernCode FROM tblLevelConcern WHERE Status = 1").ToList();
                return new DataResult<LevelConcernViewModel> { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<LevelConcernViewModel> { Error = true };
            }
        }
        #endregion
    }
}
