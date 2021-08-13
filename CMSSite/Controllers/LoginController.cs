using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMSite.Common;
using CRMSite.Models;
using CRMSite.ViewModels;
using EncodeByMd5;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CRMSite.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private LogModel _logModel = null;
        //NamNP encode pass
        private EncodeImp _eCode;


        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
            _eCode = new EncodeImp();
        }

        public IActionResult Index()
        {
            //check user logined in an other tab 
            var claims = HttpContext.User.Claims;
            var roleClaim = claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault();
            if (roleClaim != null)
            {
                byte role = byte.Parse(roleClaim.Value);
                string nextView = "/";
                switch (role)
                {
                    // administrator
                    case 1:
                        nextView = "/Admin/Mechanism/Index";
                        break;

                    // accountant
                    case 2:
                        nextView = "/Accountant/ConfirmPayment/Index";
                        break;

                    // HR
                    case 3:
                        nextView = "/HR/PersonalInfo/Index";
                        break;

                    // sale
                    case 4:
                    case 5:
                    case 6:
                    case 10:
                    case 11:
                        nextView = "/Sale/Dashboard/Index";
                        break;

                    //sale admin
                    case 7:
                        nextView = "/SaleAdmin/PersonalInfo/Index";
                        break;

                    //telesale
                    //case 8:
                    case 9:
                        nextView = "/Tele/ExpectedInvestor/Index";
                        break;

                    default:
                        return Json(new { Status = 400, Errors = new List<string> { "Bạn chưa có quyền đăng nhập phần mềm!" } });

                }
                return Redirect(nextView);
            }

            //write trace log
            _logModel = new LogModel();
            _logger.LogInformation(_logModel.ToString("Anyone has been accessed the login system page"));

            return View();
        }

        public IActionResult Login()
        {
            //HttpContext.Session.Remove(SystemConstants.SessionKey.PreviousScreen);
            return RedirectToAction("Index");
        }

        #region LoginAccount - khanhkk
        [HttpPost]
        public async Task<IActionResult> LoginAccount(LoginAccount model)
        {
            //setup trace log
            _logModel = new LogModel
            {
                AccessTarget = HttpContext?.Request.Path.Value,
                Username = "Anyone",
                Role = "Undefined",
                Data = model.ToDataString(),
                Action = ActionType.Login,
                ItemName = "account",
            };

            //check input
            if (!ModelState.IsValid)
            {
                //write trace log
                _logModel.Message = Helper.GetErrors(ModelState).ToMessageString();
                _logModel.Result = ActionResultValue.LoginFailed;
                _logger.LogWarning(_logModel.ToString());

                return Json(new { Status = 400, Errors = Helper.GetErrors(ModelState) });
            }

            IAccount iAcc = new AccountImp();
            var res = iAcc.Login(model.UserName);
            if (res.Error && res.Result == null)
            {
                //write trace log
                _logger.LogError(_logModel.ToString());

                return Json(new { Status = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            //if()
            //{
            //    return Json(new { Status = 400, Errors = new List<string> { "Tài khoản chưa tồn tại trên hệ thống!" } });
            //}

            //NamNP encode pass
            string passwEncode = _eCode.EncodeMD5(model.Pass);

            if (res.Result.Count == 0 || res.Result.First().AccountPassword != passwEncode)
            {
                //write trace log
                _logModel.Message = "Invalid username or password";
                _logModel.Result = ActionResultValue.LoginFailed;
                _logger.LogError(_logModel.ToString());

                return Json(new { Status = 400, Errors = new List<string> { "Tài khoản hoặc mật khẩu không đúng!" } });
            }

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.Now.AddDays(7),
                IsPersistent = false
            };

            var user = res.Result[0];
            user.Role = 1;
            var claims = new List<Claim>
            {
                new Claim(SiteConst.TokenKey.FULLNAME, user.AccountFullName ?? string.Empty),
                new Claim(SiteConst.TokenKey.USERNAME, user.AccountName),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            try
            {
                await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity), authProperties);
            }
            catch (Exception ex)
            {
                //write trace log
                _logModel.Message = $"Throw an except [{ex.Message}] when an user login this account";
                _logModel.Result = ActionResultValue.LoginFailed;
                _logger.LogError(_logModel.ToString());

                return Json(new { Status = 400, Errors = new List<string> { "Không đăng nhập thành công!" } });
            }

            string nextView = "/";
            switch (user.Role)
            {
                // administrator
                case 1:
                    nextView = "/Admin/Accounts/Index";
                    break;

                // accountant
                case 2:
                    nextView = "/Accountant/ConfirmPayment/Index";
                    break;

                // HR
                case 3:
                    nextView = "/HR/PersonalInfo/Index";
                    break;

                // sale
                case 4:
                case 5:
                case 6:
                case 10:
                case 11:
                    nextView = "/Sale/Dashboard/Index";
                    break;

                //sale admin
                case 7:
                    nextView = "/SaleAdmin/PersonalInfo/Index";
                    break;

                //telesale
                //case 8:
                case 9:
                    nextView = "/Tele/ExpectedInvestor/Index";
                    break;

                default:
                    await HttpContext.SignOutAsync();
                    return Json(new { Status = 400, Errors = new List<string> { "Bạn chưa có quyền đăng nhập phần mềm!" } });

            }

            HttpContext.Session.Remove(SystemConstants.SessionKey.PreviousScreen);
            var previousView = SessionExtensions.GetString(HttpContext.Session, SystemConstants.SessionKey.PreviousScreen);
            if (!string.IsNullOrEmpty(previousView))
            {
                nextView = previousView;
            }

            //write trace log
            _logModel.Data = string.Empty;
            _logModel.Username = user.AccountName;
            _logModel.Role = Helper.GetRoleName((byte)user.Role);
            _logModel.Result = ActionResultValue.LoginSuccess;
            _logger.LogInformation(_logModel.ToString());

            return Json(new { Status = 200, Url = nextView });
        }
        #endregion

        //#region Logout
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync();
        //    HttpContext.Session.Clear();
        //    return RedirectToAction("Index");
        //}
        //#endregion
    }
}
