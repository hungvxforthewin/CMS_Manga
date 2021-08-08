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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CRMSite.Areas.HR.Controllers
{
    [Area("HR")]
    [Authorize]
    public class OfficesController : BaseController
    {
        //private IPersonalInfo _iPersonalInfo;
        //private readonly IContractStaff _contractStaff;
        private readonly IBranch _branchService;
        private readonly IAccount _accountService;
        private readonly IOffice _officeService;

        public OfficesController(IConfiguration configuration, IAccount accountService, IContractStaff contractStaff, IBranch branch, IOffice office, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
            : base(httpContextAccessor, logger)
        {
            //_iPersonalInfo = new PersonalInfoImp();
            //_contractStaff = contractStaff;
            _branchService = branch;
            _accountService = accountService;
            _officeService = office;
            LogModel.ItemName = "office(s)";
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
        public IActionResult GetList(SerachOfficeViewModel model)
        {
            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            var data = _officeService.GetList(model, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;
            foreach (var item in data.Result)
            {
                if (string.IsNullOrEmpty(item.NameStaffOffice))
                {
                    item.NameStaffOffice = "Chưa chọn Capital Director";
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

        #region GetAllBranches - HUNGVX
        [HttpGet]
        public IActionResult GetAllBranches()
        {
            // trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "branch(es)";

            IBranch iBranch = new BranchImp();
            var data = iBranch.GetAllBranches();
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

        #region GetContractStaffStatus
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
        public IActionResult InsertOrUpdate(OfficeViewModel model)
        {
            //trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = model.Id == 0 ? ActionType.Create : ActionType.Update;

            if (string.IsNullOrEmpty(model.OfficeName))
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Tên khối không được trống";
                Logger.LogWarning(LogModel.ToString());

                return Ok(new { status = false, mess = "Tên khối không được trống" });
            }
            if (string.IsNullOrEmpty(model.BranchCode))
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Chọn chi nhánh";
                Logger.LogWarning(LogModel.ToString());

                return Ok(new { status = false, mess = "Chọn chi nhánh" });
            }
            string newCode = Guid.NewGuid().ToString();
            //HUNGVX-UPDATE NAME OFFICE
            string nameBranch = _branchService.GetByCode(model.BranchCode).DataItem.BranchName;
            if (!string.IsNullOrEmpty(nameBranch))
                model.OfficeName = string.Concat(nameBranch, model.OfficeName);
            if (model.Id <= 0)
            {
                //HUNGVX-CHECK OFFICE NAME EXISTS
                string oldNameOffice = _officeService.CheckOffficeInBranch(model.BranchCode, model.OfficeName).DataItem.OfficeName;
                if (!string.IsNullOrEmpty(oldNameOffice))
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Tên khối đã tồn tại trong hệ thống";
                    Logger.LogWarning(LogModel.ToString());

                    return Ok(new { status = false, mess = "Tên khối đã tồn tại trong hệ thống" });
                }
                var objectData = new Office()
                {
                    BranchCode = model.BranchCode,
                    OfficeName = model.OfficeName.ToUpper(),
                    OfficeCode = newCode,
                    CodeStaffOffice = model.CodeStaffOffice,
                    Status = model.StatusOfiice == "1" ? true : false
                };
                _officeService.Raw_Insert(objectData);

                //write trace log 
                LogModel.Result = ActionResultValue.CreateSuccess;
                LogModel.Message = "Thêm mới khối thành công!";
                Logger.LogInformation(LogModel.ToString());
            }
            else
            {
                var officeOld = _officeService.GetById((int)model.Id).Result[0];
                var branchOld = _branchService.GetByCode(model.BranchCode).DataItem;
                if (officeOld.OfficeName.ToUpper() != model.OfficeName.ToUpper())
                {
                    string oldNameOffice = _officeService.CheckOffficeInBranch(model.BranchCode, model.OfficeName).DataItem.OfficeName;
                    if (!string.IsNullOrEmpty(oldNameOffice))
                    {
                        //write trace log 
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = "Tên khối đã tồn tại trong hệ thống";
                        Logger.LogWarning(LogModel.ToString());

                        return Ok(new { status = false, mess = "Tên khối đã tồn tại trong hệ thống" });
                    }
                }
                var objectData = new Office()
                {
                    Id = model.Id,
                    BranchCode = model.BranchCode,
                    OfficeName = model.OfficeName.ToUpper(),
                    OfficeCode = model.OfficeCode,
                    CodeStaffOffice = model.CodeStaffOffice,
                    Status = model.StatusOfiice == "1" ? true : false
                };
                _officeService.Raw_Update(objectData);

                //write trace log 
                LogModel.Result = ActionResultValue.UpdateSuccess;
                LogModel.Message = "Cập nhật khối thành công!";
                Logger.LogInformation(LogModel.ToString());
            }
            return Ok(new { status = true, mess = "Thêm mới khối thành công" });
        }
        #endregion

        #region Edit - HUNGVX
        public IActionResult Edit(int id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _officeService.GetById(id);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;
            //CHECK CONTAINS
            if (data.Result[0].OfficeName.Contains('K'))
            {
                data.Result[0].OfficeName = string.Concat('K', data.Result[0].OfficeName.Split('K')[1]);
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
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("Edit", data.Result.First());
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
    }
}
