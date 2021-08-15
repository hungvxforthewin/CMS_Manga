using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMBussiness.ViewModel;
using CRMModel.Models.Data;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EncodeByMd5;
using CMSBussiness.IService;
using CMSBussiness.ViewModel;
using CMSModel.Models.Data;

namespace CMSSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : BaseController
    {
        private readonly IAccount _account;
        private readonly ICategory _category;
        private EncodeImp _eCode;
        public CategoriesController(IConfiguration configuration, IAccount account, ICategory category, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
           : base(httpContextAccessor, logger)
        {
            _account = account;
            _category = category;
            _eCode = new EncodeImp();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetList(SearchCategoryViewModel model)
        {
            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            var data = _category.GetList(model, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;
            
            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Data = data.Result, Total = total });
        }
        public IActionResult Edit(int id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();
            if (id <= 0)
            {
                return BadRequest();
            }
            var data = _category.Raw_Get(id);
            var result = new CategoryViewModel()
            {
                CategoryId = data.CategoryId,
                CategoryName = data.CategoryName,
                CategoryDescription = data.CategoryDescription,
                isActive = data.isActive,
                OrderNo = data.OrderNo,
                ParentCategoryId = data.ParentCategoryId
            };
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;
            return PartialView("Edit", result);
        }
        public IActionResult View(int id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();
            if (id <= 0)
            {
                return BadRequest();
            }
            var data = _category.Raw_Get(id);
            var result = new CategoryViewModel()
            {
               CategoryId = data.CategoryId,
               CategoryName = data.CategoryName,
               CategoryDescription = data.CategoryDescription,
               isActive = data.isActive,
               OrderNo = data.OrderNo,
               ParentCategoryId = data.ParentCategoryId
            };
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;
            return PartialView("View", result);
        }
        public IActionResult IsDelete(int id)
        {
            //trace log
            LogModel.Action = ActionType.Delete;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _category.Raw_Get(id);
            if (data == null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.NotFoundData;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, mess = "Nhân sự không tồn tại", name = "" });
            }
            else
            {
                //write trace log
                LogModel.Result = ActionResultValue.DeleteSuccess;
                LogModel.Message = "Xóa category thành công";
                Logger.LogInformation(LogModel.ToString());

                return Json(new { status = true, name = data.CategoryName });
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
            var data = _category.Raw_Delete(id);
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;
            return Ok(new { status = true, mess = "Xóa category thành công" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertOrUpdate(CategoryViewModel model)
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
            DateTime? dateNull = null;

            if (model.CategoryId <= 0)
            {
                var status = 0;
                try
                {
                    var data = new Category()
                    {
                        CategoryName = model.CategoryName,
                        CategoryDescription = model.CategoryDescription,
                        isActive = true,
                        OrderNo = model.OrderNo,
                        ParentCategoryId = model.ParentCategoryId
                    };
                    _category.Raw_Insert(data);
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, mess = "Lỗi hệ thống !" });
                }

            }
            else
            {
                var oldData = _account.Raw_Get(model.CategoryId);
                var data = new Category()
                {
                    CategoryName = model.CategoryName,
                    CategoryDescription = model.CategoryDescription,
                    isActive = model.isActive,
                    OrderNo = model.OrderNo,
                    ParentCategoryId = model.ParentCategoryId,
                    CategoryId = (short)model.CategoryId
                };
                try
                {
                    _account.Raw_Update(data);
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, mess = "Lỗi hệ thống !" });
                }
            }
            return Json(new { status = true });
        }
        public IActionResult GetAllCategories()
        {
            List<CategoryViewModel> data = new List<CategoryViewModel>();
            data = _category.GetAll().Result.ToList();
            return Json(new { status = true, data });
        }
        #region HungVX Validate
        private List<ErrorResult> validform(CategoryViewModel entity)
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
        #endregion
    }
}
