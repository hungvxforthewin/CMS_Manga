using CRMBussiness.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRMBussiness.IService
{
    public interface IRuleInMonth
    {
        DataResult<RuleInMonthViewModel> Setup(RuleInMonthViewModel model);

        DataResult<RuleInMonthViewModel> GetByMonth(string month);
    }
}
