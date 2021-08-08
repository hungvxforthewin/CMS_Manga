using CRMBussiness.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMBussiness.IService
{
    public interface IPosition
    {
        DataResult<PositionViewModel> GetAllPositions();

        DataResult<BranchViewModel> GetPositionList(string key, int start = 1, int size = 10, int pages = 5);
    }
}
