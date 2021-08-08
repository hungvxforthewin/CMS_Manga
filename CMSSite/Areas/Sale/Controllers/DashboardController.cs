using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMBussiness.ViewModel;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using CRMSite.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CRMSite.Areas.Sale.Controllers
{
    [Area("Sale")]
    public class DashboardController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        private IContractInvester _contractInvester;
        private SendMailService _mailService;
        private IConfiguration _iconfig;
        private const string FinalStatus = "PayDone";
        private readonly IRatingSale _ratingSaleService;


        public DashboardController(IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment env, SendMailService mailService, IRatingSale ratingSale, IConfiguration iconfig, ILogger<BaseController> logger
            ) : base(httpContextAccessor, logger)
        {
            this._env = env;
            _contractInvester = new ContractInvesterImp();
            _mailService = mailService;
            _iconfig = iconfig;
            LogModel.ItemName = "contract";
            _ratingSaleService = ratingSale;
        }
        public IActionResult Index()
        {
            ViewBag.FullName = tokenModel.FullName;
            var data = _ratingSaleService.GetTop(DateTime.Now.ToString("dd-MM-yyyy"));
            return View(data.Result);
        }
    }
}
