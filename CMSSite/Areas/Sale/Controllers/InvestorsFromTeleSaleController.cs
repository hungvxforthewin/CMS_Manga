using CRMSite.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRMSite.Areas.Sale.Controllers
{
    [Area("Sale")]
    public class InvestorsFromTeleSaleController : BaseController
    {
        public InvestorsFromTeleSaleController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
