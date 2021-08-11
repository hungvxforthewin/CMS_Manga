using CMSBussiness.IService;
using CMSBussiness.ViewModel;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BookController : BaseController
    {
        private readonly IBook _bookService;
        public BookController(IHttpContextAccessor httpContextAccessor, IBook book, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            LogModel.ItemName = "book";
            _bookService = book;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetList(SearchBookViewModel model)
        {
            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            var data = _bookService.GetList(model, out total);
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
