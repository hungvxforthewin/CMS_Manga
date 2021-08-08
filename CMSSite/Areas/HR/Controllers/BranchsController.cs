using CRMBussiness.IService;
using CRMBussiness.ViewModel;
using CRMModel.Models.Data;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CRMSite.Areas.HR.Controllers
{
    [Area("HR")]
    [Authorize]
    public class BranchsController : BaseController
    {
        //private IPersonalInfo _iPersonalInfo;
        ///private readonly IContractStaff _contractStaff;
        private readonly IAccount _accountService;
        private readonly IBranch _branchService;

        public BranchsController(IConfiguration configuration, IAccount accountService, IContractStaff contractStaff, IBranch branch, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
            : base(httpContextAccessor, logger)
        {
            //_iPersonalInfo = new PersonalInfoImp();
            //_contractStaff = contractStaff;
            _accountService = accountService;
            _branchService = branch;
            LogModel.ItemName = "branch(es)";
        }
        public IActionResult Index()
        {
            //write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return View();
        }
        #region GetList - HUNGVX
        [HttpPost]
        public IActionResult GetList(SearchBranchViewModel model)
        {
            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            var data = _branchService.GetList(model, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;
            foreach (var item in data.Result)
            {
                if (string.IsNullOrEmpty(item.NameStaffAdminSale))
                {
                    item.NameStaffAdminSale = "Chưa chọn Admin";
                }
            }
            //if (data.Error)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { "Lỗi hệ thống" } });
            //}

            //if (data.Result == null || data.Result.Count == 0)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { "Không tìm thấy dữ liệu" } });
            //}

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Data = data.Result, Total = total });
        }
        #endregion
        #region Edit - HUNGVX
        public IActionResult Edit(int id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _branchService.GetById(id);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            //if (data.Error)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { "Lỗi hệ thống" } });
            //}

            //if (data.Result == null || data.Result.Count == 0)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { "Không tìm thấy dữ liệu" } });
            //}

            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("Edit", data.Result.First());
        }
        #endregion
        #region GetContractStaffStatus - HUNGVX
        public IActionResult GetContractStaffStatus()
        {
            var data = new List<ContractStaffStatus>()
            {
                new ContractStaffStatus(){Key = 1, Value = "Đang hoạt động"},
                new ContractStaffStatus(){Key = 0, Value = "Không hoạt động"},
            };
            return Json(new { status = true, data });
        }
        #endregion
        #region HUNGVX-SAVE
        public IActionResult InsertOrUpdate(BranchViewModel model)
        {
            //trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = model.Id == 0 ? ActionType.Create : ActionType.Update;

            if (string.IsNullOrEmpty(model.BranchName))
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Tên chi nhánh không được trống";
                Logger.LogWarning(LogModel.ToString());

                return Ok(new { status = false, mess = "Tên chi nhánh không được trống" });
            }
            if (string.IsNullOrEmpty(model.Address))
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Địa chỉ chi nhánh không được trống";
                Logger.LogWarning(LogModel.ToString());

                return Ok(new { status = false, mess = "Địa chỉ chi nhánh không được trống" });
            }
           
            //string codeOld = _branchService.GetLastBranch().DataItem.BranchCode ?? "branch1";
            //string newCode = string.Concat("branch", int.Parse(codeOld.Split('h')[1]) + 1);
            string newCode = Guid.NewGuid().ToString();
            if(model.Id <= 0)
            {
                //HUNGVX-CHECK EXISTS NAME BRANCH
                string oldName = _branchService.GetByName(model.BranchName).DataItem.BranchName;
                if (!string.IsNullOrEmpty(oldName))
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Tên chi nhánh đã tồn tại trong hệ thống";
                    Logger.LogWarning(LogModel.ToString());

                    return Ok(new { status = false, mess = "Tên chi nhánh đã tồn tại trong hệ thống" });
                }
                var objectData = new Branch()
                {
                    Address = model.Address,
                    BranchName = model.BranchName.ToUpper(),
                    BranchCode = newCode,
                    Status = model.Status == "1" ? true : false,
                    CodeStaffAdminSale = model.CodeStaffAdminSale
                };
                _branchService.Raw_Insert(objectData);

                //write trace log 
                LogModel.Result = ActionResultValue.CreateSuccess;
                LogModel.Message = "Thêm mới chi nhánh thành công!";
                Logger.LogInformation(LogModel.ToString());
            }
            else
            {
                var branchOld = _branchService.GetById((int)model.Id).Result[0];
                if(branchOld.BranchName.ToUpper() != model.BranchName.ToUpper())
                {
                    string oldName = _branchService.GetByName(model.BranchName).DataItem.BranchName;
                    if (!string.IsNullOrEmpty(oldName))
                    {
                        //write trace log 
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = "Tên chi nhánh đã tồn tại trong hệ thống";
                        Logger.LogWarning(LogModel.ToString());

                        return Ok(new { status = false, mess = "Tên chi nhánh đã tồn tại trong hệ thống" });
                    }
                }
                var objectData = new Branch()
                {
                    Id = model.Id,
                    Address = model.Address,
                    BranchName = model.BranchName.ToUpper(),
                    BranchCode = model.BranchCode,
                    Status = model.Status == "1" ? true : false,
                    CodeStaffAdminSale = model.CodeStaffAdminSale
                };
                _branchService.Raw_Update(objectData);

                //write trace log 
                LogModel.Result = ActionResultValue.UpdateSuccess;
                LogModel.Message = "Cập nhật chi nhánh thành công!";
                Logger.LogInformation(LogModel.ToString());
            }
            return Ok(new { status = true, mess = "Thêm mới chi nhánh thành công"});
        }
        #endregion
        #region HUNGVX-Get Staff In Branch
        [HttpGet]
        public IActionResult GetStaffOfBranch(string codeBranch)
        {
            // trace log
            LogModel.Data = (new { CodeBranch = codeBranch }).ToDataString();
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "staff(s) in a branch";

            var data = _accountService.GetEmployeeInfoByBranch(codeBranch);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            //if (data.Error)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { "Lỗi hệ thống" } });
            //}

            //if (data.Result == null || data.Result.Count == 0)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { "Không tìm thấy dữ liệu" } });
            //}

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data.Result });
        }
        #endregion
        #region
        public IActionResult Delete(int id)
        {
            var data = _branchService.GetById(id);
            if (data.Result == null)
            {
                return NotFound();
            }
            return PartialView("Delete", data.Result.First());
        }
        #endregion
        #region HUNGVX-DELETE
        [HttpPost]
        public IActionResult DeleteConfirm(int id)
        {
            if(id <= 0)
            {
                return BadRequest("Request was incorrect");
            }
            var data = _branchService.GetById(id);
            if (data.Result == null)
            {
                return NotFound();
            }
            var result = _branchService.DeleteById(id);
            return Ok();
        }
        #endregion
    }
}
