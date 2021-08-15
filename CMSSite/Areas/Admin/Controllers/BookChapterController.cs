using CMSBussiness.IService;
using CMSBussiness.ServiceImp;
using CMSBussiness.ViewModel;
using CMSModel.Models.Data;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        private IWebHostEnvironment _env;
        private string _saveFileFolder;
        public BookChapterController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env, IBook book, IBookChapter bookChapter, IBookChapterDetail bookChapterDetail, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            LogModel.ItemName = "chapter";
            _bookService = book;
            _bookChapterService = bookChapter;
            _bookChapterDetailService = bookChapterDetail;
            _env = env;
            _saveFileFolder = _env.WebRootPath + "\\media\\chapter";
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
        [HttpPost]
        public IActionResult upFile(IFormFile file)
        {
            var files = HttpContext.Request.Form.Files[0];
            string fileName = string.Empty;
            if (files != null)
            {
                SiteConst.UploadStatus result = Helper.UploadFile(files, _saveFileFolder, out fileName);
                if(result != SiteConst.UploadStatus.SUCCESS)
                    return Ok(new { status = false, data = fileName });
            }
            return Ok(new { status = true, data = fileName });
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertOrUpdate(BookChapterViewModel model)
        {
            var errs = validform(model);
            if (errs.Count > 0)
            {
                //var jsonSerialiser = new JavaScriptSerializer();
                var jsonerrs = JsonConvert.SerializeObject(errs, Formatting.Indented);

                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = jsonerrs;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, data = jsonerrs });
            }

            if (model.ChapterId == Guid.Empty)
            {
                var data = new BookChapter()
                {
                    BookId = model.BookId,
                    ChapterId  = Guid.NewGuid(),
                    ChapterName = model.ChapterName,
                    adultLimit = model.adultLimit,
                    ChapterStatus = model.ChapterStatus,
                    PublishDate = DateTime.ParseExact(model.PublishDate, "dd-MM-yyyy", null),
                    isPremium = model.isPremium,
                    lastUpdateTime = DateTime.Now
                };
                try
                {
                    var dataNew = _bookChapterService.Raw_Insert(data);
                    if (model.imgUrls != null)
                    {
                        var imgUrls = model.imgUrls.Split(';').ToList();
                        if (imgUrls.Count > 0)
                        {
                            var bookChapterDetails = new List<BookChapterDetail>();
                            foreach (var item in imgUrls)
                            {
                                var bookChapterDetail = new BookChapterDetail
                                {
                                    ChapterId = dataNew.ChapterId,
                                    Page = imgUrls.IndexOf(item) + 1,
                                    PageImgUrl = item
                                };
                                _bookChapterDetailService.Raw_Insert(bookChapterDetail);
                                //bookChapterDetails.Add(bookChapterDetail);
                            }
                            //_bookChapterDetailService.Raw_InsertAll(bookChapterDetails);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, mess = "Lỗi hệ thống !" });
                }

            }
            else
            {
                var data = _bookChapterService.Raw_Get(model.ChapterId.ToString());
                if (data.ChapterId != Guid.Empty)
                {
                    data.BookId = model.BookId;
                    data.ChapterName = model.ChapterName;
                    data.adultLimit = model.adultLimit;
                    data.ChapterStatus = model.ChapterStatus;
                    data.PublishDate = DateTime.ParseExact(model.PublishDate, "dd-MM-yyyy", null);
                    data.isPremium = model.isPremium;
                    data.lastUpdateTime = DateTime.Now;
                    try
                    {
                        _bookChapterService.Raw_Update(data);
                        var bookChapterDetails = _bookChapterDetailService.Raw_GetAll().Where(x => x.ChapterId == data.ChapterId);
                        if (bookChapterDetails.Count() > 0)
                        {
                            _bookChapterDetailService.Raw_Delete(string.Join(",", bookChapterDetails.Select(x => x.ID)));
                        }
                        if (model.imgUrlsEdit != null)
                        {
                            var imgUrls = model.imgUrlsEdit.Split(';').ToList();
                            if (imgUrls.Count > 0)
                            {
                                foreach (var item in imgUrls)
                                {
                                    var bookChapterDetail = new BookChapterDetail
                                    {
                                        ChapterId = data.ChapterId,
                                        Page = imgUrls.IndexOf(item) + 1,
                                        PageImgUrl = item
                                    };
                                    _bookChapterDetailService.Raw_Insert(bookChapterDetail);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { status = false, mess = "Lỗi hệ thống !" });
                    }
                }
                else
                {
                    return Json(new { status = false, mess = "Không tìm thấy dữ liệu !" });
                }

            }
            return Json(new { status = true });
        }

        public IActionResult Edit(Guid id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            var data = _bookChapterService.GetById(id);
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;
            return PartialView("Edit", data.DataItem);
        }

        private List<ErrorResult> validform(BookChapterViewModel entity)
        {

            Dictionary<string, ErrorResult> dictErrors = new Dictionary<string, ErrorResult>();
            //  List<ErrorResult> Errors = new List<ErrorResult>();
            if (!TryValidateModel(entity))
            {
                //Error while validating user info. Please review the errors below!";
                foreach (KeyValuePair<string, ModelStateEntry> modelStateDD in ViewData.ModelState)
                {
                    string key = modelStateDD.Key;
                    ModelStateEntry modelState = modelStateDD.Value;

                    foreach (ModelError error in modelState.Errors)
                    {
                        ErrorResult er = new ErrorResult();
                        er.ErrorMessage = error.ErrorMessage;
                        if (key.IndexOf('.') > -1) key = key.Split('.')[1]; //key sẽ có dạng model.property nếu để dạng này sẽ bị ,lỗi phân tích phía client
                        er.Field = key;
                        dictErrors[key] = er;
                    }
                }
            }

            return dictErrors.Values.GroupBy(x => x.ErrorMessage).Select(y => y.First()).ToList();
        }


        [HttpGet]
        public IActionResult GetAllBooks()
        {
            // trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "books(es)";

            IBook book = new BookImp();
            var data = book.Raw_GetAll();
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data });
        }
    }
}
