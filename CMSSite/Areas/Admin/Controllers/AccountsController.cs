using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMBussiness.ViewModel;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CMSSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : BaseController
    {
        private readonly IAccount _account;
        public AccountsController(IConfiguration configuration, IAccount account, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
           : base(httpContextAccessor, logger)
        {
            _account = account;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetList(SearchAccountViewModel model)
        {
            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            var data = _account.GetList(model, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Data = data.Result, Total = total });
        }
    }
}
