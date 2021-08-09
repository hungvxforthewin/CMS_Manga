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
        public AccountsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
           : base(httpContextAccessor, logger)
        {
            
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
