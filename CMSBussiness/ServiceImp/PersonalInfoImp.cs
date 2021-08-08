using CRMBussiness.IService;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CRMBussiness.ServiceImp
{
    public class PersonalInfoImp : BaseService<PersonalInfoViewModel, int>, IPersonalInfo
    {
        #region GetInfoById - khanhkk
        public DataResult<PersonalInfoViewModel> GetInfoById(long id)
        {
            List<PersonalInfoViewModel> Ac = new List<PersonalInfoViewModel>();
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("sp_tblContractStaff_GetPersonalInfo_ById",
                        new { @id = id },
                        commandType: CommandType.StoredProcedure))
                    {
                        Ac = multipleresult.Read<PersonalInfoViewModel>().ToList();
                    }
                }

                return new DataResult<PersonalInfoViewModel> { Error = false, Result = Ac };
            }
            catch(Exception ex)
            {
                return new DataResult<PersonalInfoViewModel> { Error = true };
            }
        }
        #endregion

        #region GetPersonalInformationList - hungvx
        public DataResult<DisplayPersonalTableViewModel> GetPersonalInformationList(SearchPersonalInfoModel model, out int total)
        {
            List<DisplayPersonalTableViewModel> data = new List<DisplayPersonalTableViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", model.Key ?? string.Empty);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@Position", model.Position);
            param.Add("@Team", model.Team);
            param.Add("@Department", model.Department);
            param.Add("@Status", model.Status);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    data = this.Procedure<DisplayPersonalTableViewModel>("sp_tblContractStaff_GetPersonalInfoList", param).ToList();
                    total = param.Get<int>("Total");
                }
                return new DataResult<DisplayPersonalTableViewModel> { Result = data };
            }
            catch (Exception ex)
            {
                return new DataResult<DisplayPersonalTableViewModel> { Error = true };
            }
        }
        #endregion
    }
}