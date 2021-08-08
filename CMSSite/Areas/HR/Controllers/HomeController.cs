using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using CRMSite.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace CRMSite.Areas.HR.Controllers
{
    [Area("HR")]
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IConfiguration _configuration;
        private string _apiUrl;
        public HomeController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _configuration = configuration;
            _apiUrl = configuration["ApiUrl"].ToString();
        }

        #region Index - khanhkk
        [HttpGet]
        public IActionResult Index()
        {
            if (tokenModel == null)
            {
                return Redirect("/Login/LoginAccount");
            }
            return View();
        }
        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
