using CRMBussiness;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMModel.Models.Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CRMBussiness.IService;
using Microsoft.Extensions.Logging;
using CRMSite.Models;
using CRMSite.Common;

namespace CRMSite.Controllers
{
    [Route("Allowance")]
    public class AllowanceController : BaseController
    {
        private AllowanceOrDeductImp allowanceImp;
        public AllowanceController(IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            allowanceImp = new AllowanceOrDeductImp();
            LogModel.ItemName = "allowance(s)";
        }
        [Route("Index")]
        public IActionResult Index()
        {
            List<AllowanceOrDeduct> lst = new List<AllowanceOrDeduct>();
            var cong_doan = allowanceImp.Raw_Get(2) ?? new AllowanceOrDeduct();
            var chuyen_can = allowanceImp.Raw_Get(1) ?? new AllowanceOrDeduct();
            lst.Add(cong_doan);
            lst.Add(chuyen_can);

            //write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return View(lst);
        }
        [HttpPost]
        [Route("Save")]
        public JsonResult Save(string lstmodel)
        {
            // trace log
            LogModel.Data = lstmodel.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            var result = JsonConvert.DeserializeObject<List<AllowanceOrDeduct>>(lstmodel);
            foreach (var item in result)
            {
                var old = allowanceImp.Raw_Get(item.Id);
                if (old != null)
                {
                    item.AllowanceCode = old.AllowanceCode;
                    item.AllowanceName = old.AllowanceName;
                    item.Id = old.Id;
                    item.Note = old.Note;
                    item.Status = old.Status;
                    //khanhkk added
                    item.Type = item.Type;
                    var entity = allowanceImp.Raw_Update(item);
                    if (entity.Id <= 0)
                    {
                        //write trace log
                        LogModel.Result = ActionResultValue.CreateFailed;
                        LogModel.Message = "Cập nhật cấu hình không thành công!";
                        Logger.LogError(LogModel.ToString());

                        return Json(new { success = false, message = "Cập nhật cấu hình không thành công" });
                    }
                    else
                    {
                    }
                }
            }

            //write trace log
            LogModel.Result = ActionResultValue.CreateSuccess;
            LogModel.Message = "Cập nhật cấu hình thành công!";
            Logger.LogInformation(LogModel.ToString());

            //update
            return Json(new { success = true, message = "Cập nhật cấu hình thành công" });
        }
    }
}
