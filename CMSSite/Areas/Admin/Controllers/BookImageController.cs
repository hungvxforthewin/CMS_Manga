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
    public class BookImageController : BaseController
    {
        private readonly IBookImage _bookImageService;
        private IWebHostEnvironment _env;
        private string _saveFileFolder;
        public BookImageController(IHttpContextAccessor httpContextAccessor, IBookImage bookImage, IWebHostEnvironment env, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            LogModel.ItemName = "bookImage";
            _bookImageService = bookImage;

            _env = env;
            _saveFileFolder = _env.WebRootPath + "\\media";
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Edit(int bookId, int imgId)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = bookId }).ToDataString();
            if (bookId <= 0)
            {
                return BadRequest();
            }
            var data = _bookImageService.GetDetail(bookId, imgId);
            //var result = new CategoryViewModel()
            //{
            //    CategoryId = data.CategoryId,
            //    CategoryName = data.CategoryName,
            //    CategoryDescription = data.CategoryDescription,
            //    isActive = data.isActive,
            //    OrderNo = data.OrderNo,
            //    ParentCategoryId = data.ParentCategoryId
            //};
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;
            return PartialView("Edit", data.DataItem);
        }
        [HttpPost]
        public IActionResult GetList(SearchBookImageViewModel model)
        {
            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            var data = _bookImageService.GetList(model, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Data = data.Result, Total = total });
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            // trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "book(es)";

            IBook author = new BookImp();
            var data = author.Raw_GetAll();
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertOrUpdate(BookImageViewModel model)
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

            if (model.imgId <= 0)
            {
                var data = new BookImage()
                {
                    BookId = model.BookId,
                    imgUrl = model.imgUrl,
                    IsBanner = model.IsBanner
                };
                try
                {
                    var dataNew = _bookImageService.Raw_Insert(data);
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, mess = "Lỗi hệ thống !" });
                }

            }
            else
            {
                //var data = _bookService.Raw_Get(model.BookId);
                //if (data.BookId != 0)
                //{
                //    data.BookName = model.BookName;
                //    data.BookDescription = model.BookDescription;
                //    data.adultLimit = model.adultLimit;
                //    data.bookSexId = model.bookSexId;
                //    data.isEnable = model.isEnable;
                //    data.commentAllowed = model.commentAllowed;
                //    data.authorAccountId = model.authorAccountId;
                //    data.updateStatus = 1;
                //    data.lastUpdateTime = DateTime.Now;
                //    try
                //    {
                //        _bookService.Raw_Update(data);
                //        var bookCategoriesFirst = _bookCategoryService.Raw_GetAll().Where(x => x.BookId == data.BookId);
                //        if (bookCategoriesFirst.Count() > 0)
                //        {
                //            _bookCategoryService.Raw_Delete(string.Join(",", bookCategoriesFirst.Select(x => x.BookId)));
                //        }
                //        if (model.CategoryIds != null)
                //        {
                //            if (model.CategoryIds.Length > 0)
                //            {
                //                var bookCategories = new List<BookCategory>();
                //                foreach (var item in model.CategoryIds)
                //                {
                //                    var bookCategory = new BookCategory
                //                    {
                //                        BookId = data.BookId,
                //                        CategoryId = Int16.Parse(item),
                //                    };
                //                    if (item == model.CategoryIds.First())
                //                    {
                //                        bookCategory.isDefaultCate = true;
                //                    }
                //                    bookCategories.Add(bookCategory);
                //                }
                //                _bookCategoryService.Raw_InsertAllByKeys(bookCategories);
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        return Json(new { status = false, mess = "Lỗi hệ thống !" });
                //    }
                //}
                //else
                //{
                //    return Json(new { status = false, mess = "Không tìm thấy dữ liệu !" });
                //}

            }
            return Json(new { status = true });
        }

        public IActionResult IsDelete(int bookId, int imgId)
        {
            //trace log
            LogModel.Action = ActionType.Delete;
            LogModel.Data = (new { id = bookId }).ToDataString();

            var data = _bookImageService.GetDetail(bookId, imgId).DataItem;
            if (data == null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.NotFoundData;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, mess = "Banner không tồn tại", name = "" });
            }
            else
            {
                //write trace log
                LogModel.Result = ActionResultValue.DeleteSuccess;
                LogModel.Message = "Xóa banner thành công";
                Logger.LogInformation(LogModel.ToString());

                return Json(new { status = true, name = data.imgUrl });
            }
        }
        public IActionResult Delete(int bookId, int imgId)
        {
            //trace log
            LogModel.Action = ActionType.Delete;
            LogModel.Data = (new { id = bookId }).ToDataString();
            if (bookId <= 0)
            {
                return BadRequest();
            }
            var data = _bookImageService.DeleteById(bookId, imgId);
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;
            return Ok(new { status = true, mess = "Xóa Banner thành công" });
        }

        private List<ErrorResult> validform(BookImageViewModel entity)
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

        [HttpPost]
        public IActionResult upFile(IFormFile file)
        {
            var files = HttpContext.Request.Form.Files[0];
            string fileName = string.Empty;
            if (files != null)
            {
                SiteConst.UploadStatus result = Helper.UploadFile(files, _saveFileFolder, out fileName);
            }
            return Ok(new { status = true, data = fileName });
        }
    }
}
