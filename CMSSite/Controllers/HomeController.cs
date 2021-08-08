using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace CRMSite.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
            : base(httpContextAccessor, logger)
        {
        }

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();

            //write trace log
            LogModel.Result = Models.ActionResultValue.Logout;
            LogModel.Action = Models.ActionType.Logout;
            LogModel.ItemName = "account";
            Logger.LogInformation(LogModel.ToString());

            return RedirectToAction("Index","Login");
        }
        #endregion

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
