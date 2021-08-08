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
    public class DepartmentsController : BaseController
    {
        //private IPersonalInfo _iPersonalInfo;
        //private readonly IContractStaff _contractStaff;
        private readonly IBranch _branchService;
        private readonly IOffice _officeService;
        private readonly IAccount _accountService;
        private readonly IDepartment _departService;

        public DepartmentsController(IConfiguration configuration, IAccount accountService, IContractStaff contractStaff, IBranch branch, IOffice office, IDepartment department, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
            : base(httpContextAccessor,logger)
        {
            //_iPersonalInfo = new PersonalInfoImp();
            //_contractStaff = contractStaff;
            _branchService = branch;
            _officeService = office;
            _accountService = accountService;
            _departService = department;
            LogModel.ItemName = "department(s)";
        }
        public IActionResult Index()
        {
            //write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return View();
        }
        #region GetList - hungvx
        [HttpPost]
        public IActionResult GetList(SearchDepartmentViewModel model)
        {
            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            var data = _departService.GetList(model, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;
            foreach (var item in data.Result)
            {
                if (string.IsNullOrEmpty(item.NameStaffSaleManage))
                {
                    item.NameStaffSaleManage = "Chưa chọn Capital Manager";
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
        #region GetAllBranches - HungVX
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
        /// <summary>
        /// GET OFFICE BY BRANCH
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
        #region GetOfficeByBranh - HungVX
        [HttpGet]
        public IActionResult GetAllOffice(string branch)
        {
            // trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "office(s) in a branch";
            LogModel.Data = (new { branch = branch }).ToDataString();

            IOffice iOffice = new OfficeImp();
            var data = iOffice.GetOfficesInBranch(branch);
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
        /// <summary>
        /// GET DEPARTMENT BY OFFICE
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
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
        public IActionResult InsertOrUpdate(DepartmentViewModel model)
        {
            //trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = model.Id == 0 ? ActionType.Create : ActionType.Update;

            if (string.IsNullOrEmpty(model.DepartmentName))
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Tên phòng ban không được trống";
                Logger.LogWarning(LogModel.ToString());

                return Ok(new { status = false, mess = "Tên phòng ban không được trống" });
            }
            if (string.IsNullOrEmpty(model.BranchCode))
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Chọn chi nhánh";
                Logger.LogWarning(LogModel.ToString());

                return Ok(new { status = false, mess = "Chọn chi nhánh" });
            }
            if (string.IsNullOrEmpty(model.OfficeCode))
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Chọn khối";
                Logger.LogWarning(LogModel.ToString());

                return Ok(new { status = false, mess = "Chọn khối" });
            }
            //HUNGVX-GET BANCH NAME, OFFICE NAME
            string branchName = _branchService.GetByCode(model.BranchCode).DataItem.BranchName ?? "";
            string officeName = _officeService.GetByCode(model.OfficeCode).DataItem.OfficeName ?? "";
            if (!string.IsNullOrEmpty(model.DepartmentName))
            {
                model.DepartmentName = string.Concat(officeName, model.DepartmentName);
            }
            string newCode = Guid.NewGuid().ToString();
            if (model.Id <= 0)
            {
                //HUNGVX-CHECK DEPARTMENT NAME EXISTS
                string oldNameDepart = _departService.CheckByOfficeAndBranch(model.DepartmentName, model.OfficeCode, model.BranchCode).DataItem.DepartmentName;
                if (!string.IsNullOrEmpty(oldNameDepart))
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Tên phòng ban đã tồn tại trong hệ thống";
                    Logger.LogWarning(LogModel.ToString());

                    return Ok(new { status = false, mess = "Tên phòng ban đã tồn tại trong hệ thống" });
                }
                var objectData = new Department()
                {
                    BranchCode = model.BranchCode,
                    OfficeCode = model.OfficeCode,
                    DepartmentCode = newCode,
                    DepartmentName = model.DepartmentName.ToUpper(),
                    Status = model.StatusDepartment == "1" ? true : false,
                    CodeStaffSaleManage = model.CodeStaffSaleManage
                };
                _departService.Raw_Insert(objectData);

                //write trace log 
                LogModel.Result = ActionResultValue.CreateSuccess;
                LogModel.Message = "Thêm mới phòng ban thành công!";
                Logger.LogInformation(LogModel.ToString());
            }
            else
            {
                var departOld = _departService.GetById((int)model.Id).Result[0];
                if (departOld.DepartmentName.ToUpper() != model.DepartmentName.ToUpper())
                {
                    string oldNameDepart = _departService.CheckByOfficeAndBranch(model.DepartmentName, model.OfficeCode, model.BranchCode).DataItem.DepartmentName;
                    if (!string.IsNullOrEmpty(oldNameDepart))
                    {
                        //write trace log 
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = "Tên phòng ban đã tồn tại trong hệ thống";
                        Logger.LogWarning(LogModel.ToString());

                        return Ok(new { status = false, mess = "Tên phòng ban đã tồn tại trong hệ thống" });
                    }
                }
                var objectData = new Department()
                {
                    Id = model.Id,
                    BranchCode = model.BranchCode,
                    OfficeCode = model.OfficeCode,
                    DepartmentCode = model.DepartmentCode,
                    DepartmentName = model.DepartmentName.ToUpper(),
                    Status = model.StatusDepartment == "1" ? true : false,
                    CodeStaffSaleManage = model.CodeStaffSaleManage
                };
                _departService.Raw_Update(objectData);

                //write trace log 
                LogModel.Result = ActionResultValue.UpdateSuccess;
                LogModel.Message = "Cập nhật phòng ban thành công!";
                Logger.LogInformation(LogModel.ToString());
            }
            return Ok(new { status = true, mess = "Thêm mới phòng ban thành công" });
        }
        #endregion
        #region Edit - Hungvx
        public IActionResult Edit(int id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _departService.GetById(id);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;
            //CHECK CONTAINS
            if (data.Result[0].DepartmentName.Contains('P'))
            {
                data.Result[0].DepartmentName = string.Concat('P', data.Result[0].DepartmentName.Split('P')[1]);
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
        #region HungVX-Get Staff In Depart
        [HttpGet]
        public IActionResult GetStaffOfDepart(string codeDepart)
        {
            // trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "staff(s) in a department";
            LogModel.Data = (new { CodeDepart = codeDepart }).ToDataString();

            var data = _accountService.GetEmployeeInfoByDepart(codeDepart);
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
