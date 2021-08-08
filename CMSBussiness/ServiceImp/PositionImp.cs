using CRMBussiness.IService;
using CRMBussiness.ViewModel;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CRMBussiness.ServiceImp
{
    public class PositionImp : IPosition
    {
        public DataResult<PositionViewModel> GetAllPositions()
        {
            List<PositionViewModel> data = new List<PositionViewModel>();
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    using (var multipleresult = db.QueryMultiple("sp_tblPosition_GetAll",
                        new { }, commandType: CommandType.StoredProcedure))
                    {
                        data = multipleresult.Read<PositionViewModel>().ToList();
                    }
                }

                return new DataResult<PositionViewModel> { Error = false, Result = data };
            }
            catch
            {
                return new DataResult<PositionViewModel> { Error = true };
            }
        }

        public DataResult<BranchViewModel> GetPositionList(string key, int start = 1, int size = 10, int pages = 5)
        {
            throw new NotImplementedException();
        }
    }
}
