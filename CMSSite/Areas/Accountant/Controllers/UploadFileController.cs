using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMBussiness.ViewModel;
using CRMSite.Common;
using CRMSite.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
namespace CRMSite.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    [Authorize]
    public class UploadFileController : BaseController
    {
        private IFileUpload _iFile;
        private IWebHostEnvironment _env;
        private string _saveFileFolder;

        public UploadFileController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env) : base(httpContextAccessor)
        {
            _env = env;
            _iFile = new FileUploadImp();
            _saveFileFolder = _env.WebRootPath + "\\Uploads\\Files";
        }

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region GetList
        [HttpGet]
        public IActionResult GetList(string key, int page, int size)
        {
            int totalItems;
            var getResult = _iFile.GetList(key, out totalItems, page, size);
            if (getResult.Error)
            {
                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            if (getResult.Result == null || getResult.Result.Count == 0)
            {
                return Json(new { Result = 400, Errors = new List<string> { SiteConst.NotFoundError } });
            }

            return Json(new { Result = 200, Data = getResult.Result, Total = totalItems });
        }
        #endregion

        #region Create
        [HttpPost]
        public IActionResult Create(FileUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Result = 400, Errors = Helper.GetErrors(ModelState) });
            }

            var checkInputResult = CheckInput(model);
            if (checkInputResult != null) return checkInputResult;

            // attach user
            model.UserUpload = tokenModel.StaffCode;

            var createResult = _iFile.Create(model);

            if (createResult.Error)
            {
                // delete new file
                string url = System.IO.Path.Combine(_saveFileFolder, model.LinksUpload);
                if (System.IO.File.Exists(url))
                {
                    System.IO.File.Delete(url);
                }

                return Json(new { Result = 400, Errors = new List<string> { "Tải file lên hệ thống không thành công!" } });
            }
            else
            {
                return Json(new { Result = 200, Message = "Tải file lên hệ thống thành công!" });
            }
        }
        #endregion

        #region Update
        [HttpGet]
        public IActionResult Update(long id)
        {
            var getResult = _iFile.Get(id);

            if (getResult.Error)
            {
                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            if (getResult.Result == null || getResult.Result.Count == 0)
            {
                return Json(new { Result = 400, Errors = new List<string> { SiteConst.NotFoundError } });
            }
            HttpContext.Session.SetString(SiteConst.SessionKey.FILE_NAME, getResult.Result.First().LinksUpload);
            return PartialView("_Update", getResult.Result.First());
        }

        [HttpPost]
        public IActionResult Update(FileUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { Result = 400, Errors = Helper.GetErrors(ModelState) });
            }

            if (model.File != null)
            {
                var checkInputResult = CheckInput(model);
                if (checkInputResult != null) return checkInputResult;
            }

            // attach updated user
            model.UserUpdate = tokenModel.StaffCode;

            var createResult = _iFile.Update(model);

            if (createResult.Error)
            {
                // delete new  file
                string url = System.IO.Path.Combine(_saveFileFolder, model.LinksUpload);
                if (System.IO.File.Exists(url))
                {
                    System.IO.File.Delete(url);
                }

                return Json(new { Result = 400, Errors = new List<string> { "Tải file lên hệ thống không thành công!" } });
            }
            else
            {
                // delete old file
                if (model.File != null)
                {
                    string oldFileName = HttpContext.Session.GetString(SiteConst.SessionKey.FILE_NAME);
                    string url = System.IO.Path.Combine(_saveFileFolder, oldFileName);
                    if (System.IO.File.Exists(url))
                    {
                        System.IO.File.Delete(url);
                    }
                }

                return Json(new { Result = 200, Message = "Tải file lên hệ thống thành công!" });
            }
        }
        #endregion

        #region Delete
        public IActionResult Delete(long id)
        {
            var getResult = _iFile.Get(id);
            if (getResult.Error)
            {
                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            if (getResult.Result == null || getResult.Result.Count == 0)
            {
                return Json(new { Result = 400, Errors = new List<string> { SiteConst.NotFoundError } });
            }

            HttpContext.Session.SetString(SiteConst.SessionKey.FILE_NAME, getResult.Result.First().LinksUpload);
            return PartialView("_Delete", getResult.Result.First());
        }

        [HttpGet]
        public IActionResult ConfirmDelete(long id)
        {
            var createResult = _iFile.Delete(id);

            if (createResult.Error)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Xóa file hệ thống không thành công!" } });
            }
            else
            {
                // delete file
                string oldFileName = HttpContext.Session.GetString(SiteConst.SessionKey.FILE_NAME);
                string url = System.IO.Path.Combine(_saveFileFolder, oldFileName);
                if (System.IO.File.Exists(url))
                {
                    System.IO.File.Delete(url);
                }

                return Json(new { Result = 200, Message = "Xóa file hệ thống thành công!" });
            }
        }
        #endregion

        #region CheckInput
        private IActionResult CheckInput(FileUploadViewModel model)
        {
            // add new file
            string fileName;
            SiteConst.UploadStatus result = Helper.UploadFile(model.File, _saveFileFolder, out fileName);
            switch (result)
            {
                case SiteConst.UploadStatus.NOT_SELECT_FILE:
                    return Json(new { Result = 400, Errors = new List<string> { "Chưa chọn file!" } });

                case SiteConst.UploadStatus.NO_CONTENT:
                    return Json(new { Result = 400, Errors = new List<string> { "File không chứa dữ liệu!" } });

                case SiteConst.UploadStatus.INVALID_FORMAT:
                    return Json(new { Result = 400, Errors = new List<string> { "File không không đúng format hình ảnh!" } });

                case SiteConst.UploadStatus.FAILURE:
                    return Json(new { Result = 400, Errors = new List<string> { "Tải file lên hệ thông thất bại!" } });

                case SiteConst.UploadStatus.TOO_BIG:
                    return Json(new { Result = 400, Errors = new List<string> { "File phải có dung lượng nhỏ hơn 2 MB!" } });
            }
            model.LinksUpload = fileName;

            return null;
        }
        #endregion

        #region View
        public IActionResult View(long id)
        {
            var getResult = _iFile.Get(id);
            if (getResult.Error)
            {
                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }

            if (getResult.Result == null || getResult.Result.Count == 0)
            {
                return Json(new { Result = 400, Errors = new List<string> { SiteConst.NotFoundError } });
            }

            ViewBag.Url = System.IO.Path.DirectorySeparatorChar + System.IO.Path.Combine("Uploads", "Files", getResult.Result.First().LinksUpload);
            return PartialView("_View");
        }
        #endregion
    }
}
