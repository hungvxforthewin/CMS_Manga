using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using CRMSite.Controllers;

namespace CRMSite.Areas.Admin.Controllers
{
    [Area("Admin")]
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
    }
}
