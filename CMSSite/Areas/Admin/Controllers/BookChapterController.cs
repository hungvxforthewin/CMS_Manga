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
    public class BookChapterController : BaseController
    {
        private readonly IBook _bookService;
        private readonly IBookChapter _bookChapterService;
        private readonly IBookChapterDetail _bookChapterDetailService;
        public BookChapterController(IHttpContextAccessor httpContextAccessor, IBook book, IBookChapter bookChapter, IBookChapterDetail bookChapterDetail, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            LogModel.ItemName = "chapter";
            _bookService = book;
            _bookChapterService = bookChapter;
            _bookChapterDetailService = bookChapterDetail;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetList(SearchBookChapterViewModel model)
        {
            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            var data = _bookChapterService.GetList(model, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Data = data.Result, Total = total });
        }

        public IActionResult IsDelete(Guid id)
        {
            //trace log
            LogModel.Action = ActionType.Delete;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _bookChapterService.Raw_Get(id.ToString());
            if (data == null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.NotFoundData;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, mess = "Chapter không tồn tại", name = "" });
            }
            else
            {
                //write trace log
                LogModel.Result = ActionResultValue.DeleteSuccess;
                LogModel.Message = "Xóa Chapter thành công";
                Logger.LogInformation(LogModel.ToString());

                return Json(new { status = true, name = data.ChapterName });
            }
        }
        public IActionResult Delete(Guid id)
        {
            //trace log
            LogModel.Action = ActionType.Delete;
            LogModel.Data = (new { id = id }).ToDataString();
            if (id == null)
            {
                return BadRequest();
            }
            var data = _bookChapterService.Raw_Delete($"'{id}'");
            if (data)
            {
                var bookChapterDetails = _bookChapterDetailService.Raw_GetAll().Where(x => x.ChapterId == id);
                if (bookChapterDetails.Count() > 0)
                    _bookChapterDetailService.Raw_Delete(string.Join(",", bookChapterDetails.Select(x => x.ID)));
            }
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;
            return Ok(new { status = true, mess = "Xóa Chapter thành công" });
        }
    }
}
