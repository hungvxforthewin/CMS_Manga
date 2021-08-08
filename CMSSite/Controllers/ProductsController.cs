using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using CRMBussiness.LIB;
using CRMBussiness.ServiceImp;
using CRMSite.Common;
using static CRMSite.Common.EnumSystem;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using CRMModel.Models.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using CRMSite.Models;

namespace CRMSite.Controllers
{
    [Route("Products")]
    public class ProductsController : BaseController
    {
        private ProductIpm productIpm;
        public readonly IConfiguration configuration;
        private readonly IWebHostEnvironment _hostingEnvironment; 
        public ProductsController(IHttpContextAccessor httpContextAccessor, IConfiguration Configuration, IWebHostEnvironment hostingEnvironment, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            productIpm = new ProductIpm();
            configuration = Configuration;
            _hostingEnvironment = hostingEnvironment;
            LogModel.ItemName = "product(s)";
        }

        [Route("Index")]
        public IActionResult Index()
        {
            SetTitle("Quản lý sản phẩm");
            ViewBag.IsAdd = true;

            //write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return View();
        }

        [Route("GetData")]
        [HttpPost]
        public ActionResult GetData(BootstrapTableParam obj)
        {
            // trace log
            LogModel.Data = obj.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total = 0;
            var data = productIpm.GetData(obj, ref total);

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { success = true, datas = data, total });
        }

        [Route("Create")]
        public ActionResult Create()
        {
            SetTitle("Thêm mới sản phẩm");
            ViewBag.IsView = false;
            var entity = new Product();
            ViewBag.status = Utils.GetDesribles<ProductStatus>();

            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            LogModel.Action = ActionType.Create;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_CreateOrEdit", entity);
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public ActionResult Edit(long Id)
        {
            SetTitle("Cập nhật mức phạt");
            ViewBag.IsView = false;
            ViewBag.status = Utils.GetDesribles<ProductStatus>();
            var entity = productIpm.Raw_Get(Id);

            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = Id }).ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_CreateOrEdit", entity);
        }

        [HttpGet]
        [Route("Detail/{id}")]
        public ActionResult Detail(long Id)
        {
            SetTitle("Chi tiết mức phạt");
            ViewBag.IsView = true;
            ViewBag.status = Utils.GetDesribles<ProductStatus>();
            var entity = productIpm.Raw_Get(Id);

            //write trace log
            LogModel.Result = ActionResultValue.ViewSuccess;
            LogModel.Action = ActionType.ViewInfo;
            LogModel.Data = (new { id = Id }).ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_CreateOrEdit", entity);
        }

        [Route("Delete")]
        [HttpPost]
        public JsonResult Delete(int Id)
        {
            //trace log
            LogModel.Action = ActionType.Delete;
            LogModel.Data = (new { id = Id }).ToDataString();

            var item = productIpm.Raw_Get(Id);
            if (Id > 0)
            {
                string message = "";
                bool acction = false;
                if (productIpm.Raw_Delete(Id))
                {
                    acction = true;
                    message = "Xóa thông tin thành công";

                    //write trace log
                    LogModel.Result = ActionResultValue.DeleteSuccess;
                    LogModel.Message = "Xóa thành công!";
                    Logger.LogInformation(LogModel.ToString());
                }
                else
                {
                    acction = false;
                    message = "Xóa thông tin không thành công";

                    //write trace log
                    LogModel.Result = ActionResultValue.DeleteFailed;
                    LogModel.Message = "Xóa không thành công!";
                    Logger.LogError(LogModel.ToString());
                }
                return Json(new { success = acction, message = message });
            }
            else
            {
                //write trace log
                LogModel.Result = ActionResultValue.DeleteFailed;
                LogModel.Message = "Xóa không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { success = false, message = "Xóa không thành công" });
            }
        }

        [Route("Insert_Update")]
        public JsonResult Insert_Update(Product data)
        {
            object message = "";
            var item = productIpm.Raw_Get(data.Id);

            //trace log
            LogModel.Data = data.ToDataString();
            LogModel.Action = item == null ? ActionType.Create : ActionType.Update;

            if (string.IsNullOrEmpty(data.Name))
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Tên không được trống";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { success = false, message = "*Tên không được trống" });
            }
            if (string.IsNullOrEmpty(data.CompanyCode))
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Mã cty không được trống";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { success = false, message = "*Mã cty không được trống" });
            }
            if (item != null) //update
            {
                data.ProductCode = item.ProductCode;
                var entity = productIpm.Raw_Update(data);
                if (entity.Id > 0)
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.UpdateSuccess;
                    LogModel.Message = "Cập nhật thành công!";
                    Logger.LogInformation(LogModel.ToString());

                    return Json(new { success = true, message = "Cập nhật thành công" });
                }
                else
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.UpdateFailed;
                    LogModel.Message = "Cập nhật không thành công!";
                    Logger.LogError(LogModel.ToString());

                    return Json(new { success = false, message = "Cập nhật không thành công" });
                }
            }
            else //insert
            {
                int total = 0;
                var isExists = productIpm.CheckExists(data.ProductCode, ref total);
                if (isExists)
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Mã sản phẩm đã tồn tại";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { success = false, message = "Mã Sản phẩm đã tồn tại" });
                }
                data.ProductCode = Utils.IdentitySTT(total, "SP");
                data.Status = 1;
                var customer = productIpm.Raw_Insert(data);
                if (customer.Id <= 0)
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.CreateFailed;
                    LogModel.Message = "Thêm mới không thành công!";
                    Logger.LogError(LogModel.ToString());

                    return Json(new { success = false, message = "Thêm mới không thành công" });
                }
                else
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.CreateSuccess;
                    LogModel.Message = "Thêm mới thành công!";
                    Logger.LogInformation(LogModel.ToString());

                    return Json(new { success = true, message = "Thêm mới thành công" });
                }
            }
        }
        public static DataTable SetDataEx(string tableName, List<string> fields, List<Investor> investors)
        {
            //Creating DataTable  
            DataTable dt = new DataTable();
            List<object> fetContact = new List<object>();
            //Setiing Table Name  
            dt.TableName = tableName;
            //Add Columns  
            foreach (var item in fields)
            {
                dt.Columns.Add(item, typeof(string));
            }
            //Add Rows in DataTable  
            foreach (var item in investors)
            {
                foreach (var field in fields)
                {
                    if ("Name".ToUpper() == field.ToUpper())
                        fetContact.Add(item.Name);
                }
                dt.Rows.Add(fetContact.ToArray());
                fetContact.Clear();
            }
            dt.AcceptChanges();
            return dt;
        }
        public string SetNumberColumn(int number)
        {
            string result = string.Empty;
            switch (number)
            {
                case 1:
                    result = "A1";
                    break;
                case 2:
                    result = "B1";
                    break;
                case 3:
                    result = "C1";
                    break;
                case 4:
                    result = "D1";
                    break;
                case 5:
                    result = "E1";
                    break;
                case 6:
                    result = "F1";
                    break;
                case 7:
                    result = "G1";
                    break;
                case 8:
                    result = "H1";
                    break;
                case 9:
                    result = "I1";
                    break;
                case 10:
                    result = "J1";
                    break;
                case 11:
                    result = "K1";
                    break;
                case 12:
                    result = "L1";
                    break;
                case 13:
                    result = "M1";
                    break;
                case 14:
                    result = "N1";
                    break;
                case 15:
                    result = "O1";
                    break;
                case 16:
                    result = "P1";
                    break;
                case 17:
                    result = "Q1";
                    break;
                case 18:
                    result = "R1";
                    break;
                case 19:
                    result = "S1";
                    break;
                // case more
                default:
                    result = "T1";
                    break;
            }
            return result;
        }
        [Route("WriteDataToExcel")]
        public JsonResult WriteDataToExcel(int pageNumber, int pageSize)
        {
            //trace log 
            LogModel.Action = ActionType.Export;
            LogModel.Data = (new { PageNumber = pageNumber, PageSize = pageSize }).ToDataString();

            BootstrapTableParam obj = new BootstrapTableParam();
            obj.offset = (pageNumber - 1) * pageSize;
            obj.limit = pageSize;
            int total = 0;
            var data = productIpm.GetData(obj, ref total);
            DataTable dt = new DataTable();
            dt.TableName = "Sản phẩm";
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Tên", typeof(string));
            foreach (var item in data)
            {
                dt.Rows.Add(item.ProductCode, item.Name);
            }
            dt.AcceptChanges();
            string fileName = "San_Pham_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
            string labelCell = "A1:" + SetNumberColumn(6);
            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable in worksheet  
                var wsDetail = wb.Worksheets.Add(dt);
                wsDetail.Columns().AdjustToContents();
                wsDetail.Rows().AdjustToContents();
                wsDetail.Row(1).InsertRowsAbove(1);
                wsDetail.Cell(1, 1).Value = "Sản phẩm";
                var range = wsDetail.Range(labelCell);
                range.Merge().Style.Font.SetBold().Font.FontSize = 14;
                range.Merge().Style.Fill.BackgroundColor = XLColor.LightGreen;
                range.Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    string pathFile = configuration["PathSaveFileExport"].ToString() + fileName;
                    FileStream fileStream = new FileStream(_hostingEnvironment.WebRootPath + pathFile, FileMode.Create, FileAccess.Write);
                    stream.WriteTo(fileStream);
                    fileStream.Close();

                    //write trace log 
                    LogModel.Result = ActionResultValue.ImportSuccess;
                    LogModel.Message = "Xuất file thành công!";
                    Logger.LogInformation(LogModel.ToString());

                    return Json(new { success = true, urlFile = pathFile, fileName });
                }
            }
        }
    }
}
