using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using Microsoft.Extensions.Logging;

namespace CRMSite.Providers
{
    public interface IRecurringJobs
    {
        void UpdateShare();
    }

    public class RecurringJobs : IRecurringJobs
    {
        ILogger<RecurringJobs> _logger;
        public RecurringJobs(ILogger<RecurringJobs> logger)
        {
            _logger = logger;
        }

        public void UpdateShare()
        {
            //IAccount iAcc = new AccountImp();

            //bool result = iAcc.UpdateShareForSale();
            //if (result)
            //{
            //    _logger.LogInformation("Update share for sale successfully!");
            //}
            //else
            //{
            //    _logger.LogError("Update share for sale failed!");
            //}
        }
    }
}
