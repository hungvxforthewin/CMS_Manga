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
    [Route("Investors")]
    public class InvestorsController : BaseController
    {
        private readonly InvestorImp _investerImp;
        public readonly IConfiguration configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public InvestorsController(IHttpContextAccessor httpContextAccessor, IConfiguration Configuration, IWebHostEnvironment hostingEnvironment, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            _investerImp = new InvestorImp();
            configuration = Configuration;
            _hostingEnvironment = hostingEnvironment;
            LogModel.ItemName = "investor(s)";
        }

        [Route("Index")]
        public IActionResult Index()
        {
            SetTitle("Quản lý khách hàng");
            ViewBag.IsAdd = true;
            ViewBag.InvestorStatus = Utils.GetDesribles<InvestorStatus>();

            //write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return View();
        }

        [Route("GetData")]
        [HttpPost]
        public ActionResult GetData(BootstrapTableParam obj, int Status)
        {
            // trace log
            LogModel.Data = (new { obj = obj, status = Status }).ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total = 0;
            var data = _investerImp.GetDatas(obj, ref total, Status);
            foreach (var item in data)
            {
                item.IsDelete = true;
                item.IsEdit = true;
            }

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { success = true, datas = data, total });
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetTitle("Thêm mới Khách hàng");
            ViewBag.IsView = false;
            //var entity = new ContactEntity();

            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            LogModel.Action = ActionType.Create;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_CreateOrEdit");
        }
        [HttpGet]
        public ActionResult Edit(long Id)
        {
            SetTitle("Cập nhật Khách hàng");
            ViewBag.IsView = false;
            //var entity = _contactServices.Raw_Get(Id);

            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = Id }).ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_CreateOrEdit");
        }
        [HttpGet]
        [Route("Detail/{id}")]
        public ActionResult Detail(long Id)
        {
            SetTitle("Chi tiết Khách hàng");
            ViewBag.IsView = true;
            var entity = _investerImp.Raw_Get(Id);

            //write trace log
            LogModel.Result = ActionResultValue.ViewSuccess;
            LogModel.Action = ActionType.ViewInfo;
            LogModel.Data = (new { id = Id }).ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_CreateOrEdit", entity);
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
            obj.offset = (pageNumber-1) * pageSize;
            obj.limit = pageSize;
            int total = 0;
            var data = _investerImp.GetDatas(obj, ref total, 0);
            DataTable dt = new DataTable();
            dt.TableName = "Khách hàng";
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Họ và tên khách hàng", typeof(string));
            dt.Columns.Add("Số điện thoại", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Số hợp đồng đã ký", typeof(string));
            dt.Columns.Add("Trạng thái", typeof(string));
            foreach (var item in data)
            {
                if(item.Status == 0)
                {
                    dt.Rows.Add(item.IdCard, item.Name, item.PhoneNumber, item.Email, item.AccountBank, "Khách chưa đầu tư");
                }
                else if(item.Status == 1)
                {
                    dt.Rows.Add(item.IdCard, item.Name, item.PhoneNumber, item.Email, item.AccountBank, "Khách hàng đã và đang đầu tư");
                }
                else if(item.Status == 2)
                {
                    dt.Rows.Add(item.IdCard, item.Name, item.PhoneNumber, item.Email, item.AccountBank, "Khách hàng đã đầu tư và hiện tại đã rút hết tiền");
                }
            }
            dt.AcceptChanges();
            string fileName = "KhachHang_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
            string labelCell = "A1:" + SetNumberColumn(6);
            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable in worksheet  
                var wsDetail = wb.Worksheets.Add(dt);
                wsDetail.Columns().AdjustToContents();
                wsDetail.Rows().AdjustToContents();
                wsDetail.Row(1).InsertRowsAbove(1);
                wsDetail.Cell(1, 1).Value = "Report Contact";
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

