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
    public class TeamsController : BaseController
    {
        //private IPersonalInfo _iPersonalInfo;
        //private readonly IContractStaff _contractStaff;
        //private readonly IBranch _branchService;
        //private readonly IOffice _officeService;
        //private readonly IDepartment _departService;
        private readonly IAccount _accountService;
        private readonly ITeamInCompany _teamService;

        public TeamsController(IConfiguration configuration, IAccount accountService, IContractStaff contractStaff, IBranch branch, IOffice office, IDepartment department, ITeamInCompany team, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
            : base(httpContextAccessor, logger)
        {
            //_iPersonalInfo = new PersonalInfoImp();
            //_contractStaff = contractStaff;
            //_branchService = branch;
            //_officeService = office;
            //_departService = department;
            _accountService = accountService;
            _teamService = team;
            LogModel.ItemName = "team(s)";
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
        public IActionResult GetList(SearchTeamViewModel model)
        {
            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            var data = _teamService.GetList(model, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;
            foreach (var item in data.Result)
            {
                if (string.IsNullOrEmpty(item.NameStaffLeader))
                {
                    item.NameStaffLeader = "Chưa chọn Team Leader";
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

        #region GetDepartments - HungVX
        [HttpGet]
        public IActionResult GetDepartments(string office)
        {
            // trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "department(s) in an office";
            LogModel.Data = (new { office = office }).ToDataString();

            IDepartment iDept = new DepartmentImp();
            var data = iDept.GetDepartmentsInOffice(office);
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

        #region HUNGVX-SAVE
        public IActionResult InsertOrUpdate(TeamInCompanyViewModel model)
        {
            //trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = model.Id == 0 ? ActionType.Create : ActionType.Update;

            if (string.IsNullOrEmpty(model.Name))
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Tên team được trống";
                Logger.LogWarning(LogModel.ToString());

                return Ok(new { status = false, mess = "Tên team được trống" });
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
            if (string.IsNullOrEmpty(model.DepartmentCode))
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Chọn phòng ban";
                Logger.LogWarning(LogModel.ToString());

                return Ok(new { status = false, mess = "Chọn phòng ban" });
            }
            string newCode = Guid.NewGuid().ToString();
            if (model.Id <= 0)
            {
                var objectData = new TeamInCompany()
                {
                    Name = model.Name,
                    TeamCode = newCode,
                    DepartmentCode = model.DepartmentCode,
                    Status = model.StatusTeam == "1" ? true : false,
                    CodeStaffLeader = model.CodeStaffLeader
                };
                _teamService.Raw_Insert(objectData);

                //write trace log 
                LogModel.Result = ActionResultValue.CreateSuccess;
                LogModel.Message = "Thêm mới team thành công!";
                Logger.LogInformation(LogModel.ToString());
            }
            else
            {
                var objectData = new TeamInCompany()
                {
                    Id = model.Id,
                    Name = model.Name,
                    TeamCode = model.TeamCode,
                    DepartmentCode = model.DepartmentCode,
                    Status = model.StatusTeam == "1" ? true : false,
                    CodeStaffLeader = model.CodeStaffLeader
                };
                _teamService.Raw_Update(objectData);

                //write trace log 
                LogModel.Result = ActionResultValue.UpdateSuccess;
                LogModel.Message = "Cập nhật team thành công!";
                Logger.LogInformation(LogModel.ToString());
            }
            return Ok(new { status = true, mess = "Thêm mới team thành công" });
        }
        #endregion

        #region Edit - Hungvx
        public IActionResult Edit(int id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _teamService.GetById(id);
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

        #region HungVX-Get Staff In Team
        [HttpGet]
        public IActionResult GetStaffOfTeam(string codeTeam)
        {
            // trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "staff(s) in a team";
            LogModel.Data = (new { CodeTeam = codeTeam }).ToDataString();

            var data = _accountService.GetEmployeeInfoByTeam(codeTeam);
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
