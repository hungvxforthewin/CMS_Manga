using CMSBussiness.IService;
using CMSBussiness.ServiceImp;
using CMSBussiness.ViewModel;
using CMSModel.Models.Data;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class BookController : BaseController
    {
        private readonly IBook _bookService;
        private readonly ICategory _categoryService;
        private readonly IBookCategory _bookCategoryService;
        public BookController(IHttpContextAccessor httpContextAccessor, IBook book, ICategory category, IBookCategory bookCategory, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            LogModel.ItemName = "book";
            _bookService = book;
            _categoryService = category;
            _bookCategoryService = bookCategory;
        }
        public IActionResult Index()
        {
            ViewBag.TokenModel = tokenModel;
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

            foreach(var item in data.Result)
            {
                item.IsApprove = item.isEnable && tokenModel.Role == 1;
            }

            return Json(new { Data = data.Result, Total = total });
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            // trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "category(es)";

            ICategory category = new CategoryImp();
            var data = category.Raw_GetAll();
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data });
        }

        [HttpGet]
        public IActionResult GetAllSexs()
        {
            // trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "category(es)";

            IBookSexInfo bookSexInfo = new BookSexInfoImp();
            var data = bookSexInfo.Raw_GetAll();
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data });
        }

        [HttpGet]
        public IActionResult GetAllAuthors()
        {
            // trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "category(es)";

            IAuthor author = new AuthorImp();
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
        public IActionResult InsertOrUpdate(BookViewModel model)
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

            if (model.BookId <= 0)
            {
                var data = new Book()
                {
                    BookUUID = Guid.NewGuid(),
                    BookName = model.BookName,
                    BookDescription = model.BookDescription,
                    adultLimit = model.adultLimit,
                    bookSexId = model.bookSexId,
                    isEnable = model.isEnable,
                    commentAllowed = model.commentAllowed,
                    authorAccountId = model.authorAccountId,
                    lastUpdateTime = DateTime.Now
                };
                try
                {
                    var dataNew = _bookService.Raw_Insert(data);
                    if (model.CategoryIds != null)
                    {
                        if (model.CategoryIds.Length > 0)
                        {
                            var bookCategories = new List<BookCategory>();
                            foreach (var item in model.CategoryIds)
                            {
                                var bookCategory = new BookCategory
                                {
                                    BookId = data.BookId,
                                    CategoryId = Int16.Parse(item),
                                };
                                if (item == model.CategoryIds.First())
                                {
                                    bookCategory.isDefaultCate = true;
                                }
                                bookCategories.Add(bookCategory);
                            }
                            _bookCategoryService.Raw_InsertAllByKeys(bookCategories);
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
                var data = _bookService.Raw_Get(model.BookId);
                if(data.BookId != 0)
                {
                    data.BookName = model.BookName;
                    data.BookDescription = model.BookDescription;
                    data.adultLimit = model.adultLimit;
                    data.bookSexId = model.bookSexId;
                    data.commentAllowed = model.commentAllowed;
                    data.authorAccountId = model.authorAccountId;
                    data.updateStatus = 1;
                    data.lastUpdateTime = DateTime.Now;
                    if(tokenModel.Role == 1)
                        data.isEnable = model.isEnable;
                    try
                    {
                        _bookService.Raw_Update(data);
                        var bookCategoriesFirst = _bookCategoryService.Raw_GetAll().Where(x => x.BookId == data.BookId);
                        if (bookCategoriesFirst.Count() > 0)
                        {
                            _bookCategoryService.Raw_Delete(string.Join(",", bookCategoriesFirst.Select(x => x.BookId)));
                        }
                        if (model.CategoryIds != null)
                        {
                            if (model.CategoryIds.Length > 0)
                            {
                                var bookCategories = new List<BookCategory>();
                                foreach (var item in model.CategoryIds)
                                {
                                    var bookCategory = new BookCategory
                                    {
                                        BookId = data.BookId,
                                        CategoryId = Int16.Parse(item),
                                    };
                                    if (item == model.CategoryIds.First())
                                    {
                                        bookCategory.isDefaultCate = true;
                                    }
                                    bookCategories.Add(bookCategory);
                                }
                                _bookCategoryService.Raw_InsertAllByKeys(bookCategories);
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

        public IActionResult Edit(int id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();
            ViewBag.TokenModel = tokenModel;
            if (id <= 0)
            {
                return BadRequest();
            }
            var data = _bookService.GetById(id);
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;
            return PartialView("Edit", data.DataItem);
        }
        public IActionResult UpdateStatus(int id)
        {

            LogModel.Data = (new { id = id }).ToDataString();
            LogModel.Action = ActionType.Update;

            if (id <= 0)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Truyện không tồn tại";
                Logger.LogWarning(LogModel.ToString());

                return Ok(new { status = false, mess = "Truyện không tồn tại" });
            }

            var data = _bookService.Raw_Get(id);
            if (data.BookId != 0)
            {
                data.isEnable = true;
                try
                {
                    _bookService.Raw_Update(data);
                    LogModel.Result = ActionResultValue.UpdateSuccess;
                    LogModel.Message = "Duyệt truyện thành công!";
                    Logger.LogInformation(LogModel.ToString());
                    return Ok(new { status = true, mess = "Duyệt truyện thành công" });
                }
                catch
                {
                    LogModel.Result = ActionResultValue.UpdateFailed;
                    LogModel.Message = "Duyệt truyện không thành công!";
                    Logger.LogInformation(LogModel.ToString());
                    return Ok(new { status = false, mess = "Duyệt truyện không thành công" });
                }
            }
            else
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Truyện không tồn tại";
                Logger.LogWarning(LogModel.ToString());

                return Ok(new { status = false, mess = "Truyện không tồn tại" });
            }
        }



        public IActionResult IsDelete(int id)
        {
            //trace log
            LogModel.Action = ActionType.Delete;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _bookService.GetById(id);
            if (data == null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.NotFoundData;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, mess = "Truyện không tồn tại", name = "" });
            }
            else
            {
                //write trace log
                LogModel.Result = ActionResultValue.DeleteSuccess;
                LogModel.Message = "Xóa truyện thành công";
                Logger.LogInformation(LogModel.ToString());

                return Json(new { status = true, name = data.DataItem.BookName });
            }
        }
        public IActionResult Delete(int id)
        {
            //trace log
            LogModel.Action = ActionType.Delete;
            LogModel.Data = (new { id = id }).ToDataString();
            if (id <= 0)
            {
                return BadRequest();
            }
            var data = _bookService.Raw_Delete(id);
            if (data)
            {
                var bookCategoriesFirst = _bookCategoryService.Raw_GetAll().Where(x => x.BookId == id);
                if(bookCategoriesFirst.Count() > 0)
                    _bookCategoryService.Raw_Delete(string.Join(",", bookCategoriesFirst.Select(x => x.BookId)));
            }
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;
            return Ok(new { status = true, mess = "Xóa truyện thành công" });
        }

        private List<ErrorResult> validform(BookViewModel entity)
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
    }

    
}
