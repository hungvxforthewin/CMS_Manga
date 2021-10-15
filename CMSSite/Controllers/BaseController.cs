using CRMBussiness;
using CRMSite.Common;
using CRMSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CRMSite.Controllers
{
    [Authorize]
    //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class BaseController : Controller
    {
        protected TokenModel tokenModel = null;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _title;
        private List<string> _NoInAreaController = new List<string>
        {
            "/Allowance/",
            "/AllowanceOrDeducts/",
            "/Employees/",
            "/Investors/",
            "/Products/",
        };
        public LogModel LogModel = null;
        public ILogger<BaseController> Logger;

        public BaseController(IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger = null)
        {
            _httpContextAccessor = httpContextAccessor;
            Logger = logger;

            if (_httpContextAccessor.HttpContext.User.Claims.Any())
            {
                tokenModel = new TokenModel();
                var claims = _httpContextAccessor.HttpContext.User.Claims;
                if (claims != null)
                {
                    if(claims.First(x => x.Type == SiteConst.TokenKey.USERNAME).Value.Equals("admin")) 
                    {
                        tokenModel.Role = 1;
                    }
                    else
                    {
                        tokenModel.Role = 2;
                    }
                    tokenModel.FullName = claims.First(x => x.Type == SiteConst.TokenKey.FULLNAME).Value;
                    tokenModel.Username = claims.First(x => x.Type == SiteConst.TokenKey.USERNAME).Value;
                }
            }
            LogModel = new LogModel
            {
                AccessTarget = httpContextAccessor.HttpContext.Request.Path.Value,
                Username = tokenModel.Username,
                Role = Helper.GetRoleName(tokenModel.Role),
            };
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (tokenModel.Role != 0)
            {
                string currentPath = filterContext.HttpContext.Request.Path.Value;
                bool _isAllow = true;
                if (!currentPath.Contains("/Home/Logout") && !currentPath.Contains("/SelectionData/"))
                {
                    switch (tokenModel.Role)
                    {
                        case 1:
                            if (!currentPath.Contains("/Admin/") && _NoInAreaController.Where(x => currentPath.Contains(x)).FirstOrDefault() == null)
                            {
                                _isAllow = false;
                            }
                            break;

                        case 2:
                            if (!currentPath.Contains("/Admin/") && _NoInAreaController.Where(x => currentPath.Contains(x)).FirstOrDefault() == null)
                            {
                                _isAllow = false;
                            }
                            break;

                        case 3:
                            if (!currentPath.Contains("/Accountant/"))
                            {
                                _isAllow = false;
                            }
                            break;


                        case 4:
                            if (!currentPath.Contains("/HR/"))
                            {
                                _isAllow = false;
                            }
                            break;

                        case 6:
                            if (!currentPath.Contains("/Sale/"))
                            {
                                _isAllow = false;
                            }
                            break;

                        case 7:
                            if (!currentPath.Contains("/SaleAdmin/"))
                            {
                                _isAllow = false;
                            }
                            break;

                        //case 8:
                        case 9:
                            if (!currentPath.Contains("/Tele/"))
                            {
                                _isAllow = false;
                            }
                            break;

                    }
                }
                if (!_isAllow)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.NotAllowAccess;
                    Logger.LogWarning(LogModel.ToString());

                    filterContext.Result = new RedirectResult("/Notice/ForbiddenAccess");
                }
            }
        }

        protected void SetTitle(string title)
        {
            _title = title;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!Equals(_title, null))
                ViewBag.Title = _title;
            base.OnActionExecuted(filterContext);
        }

        protected IActionResult HandleGetResult(dynamic result)
        {
            if (result.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            if (result.Result == null || result.Result.Count == 0)
            {
                //write trace log
                LogModel.Result = ActionResultValue.NotFoundData;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.NotFoundError } });
            }

            return null;
        }    
    }

    #region TokenModel
    public class TokenModel
    {
        public byte Role { get; set; }

        public string Id { get; set; }

        public string FullName { get; set; }

        public string BranchCode { get; set; }

        public string OfficeCode { get; set; }

        public string DepartmentCode { get; set; }

        public string TeamCode { get; set; }

        public string StaffCode { get; set; }

        public string Username { get; set; }
    }
    #endregion
}