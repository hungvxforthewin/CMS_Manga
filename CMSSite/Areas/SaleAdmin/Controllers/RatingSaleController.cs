using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMBussiness.ViewModel;
using CRMSite.Common;
using CRMSite.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using CRMSite.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using CRMModel.Models.Data;
using Microsoft.Extensions.Logging;

namespace CRMSite.Areas.SaleAdmin.Controllers
{
    [Area("SaleAdmin")]
    [Authorize]
    public class RatingSaleController : BaseController
    {
        private readonly IAccount _accountService;
        private readonly IRatingSale _ratingSaleService;
        public RatingSaleController(IHttpContextAccessor httpContextAccessor, IAccount account, IRatingSale ratingSale, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            LogModel.ItemName = "sale(s)";
            _accountService = account;
            _ratingSaleService = ratingSale;
        }
        public IActionResult Index()
        {
            SearchRattingViewModel model = new SearchRattingViewModel();
            model.Branch = tokenModel.BranchCode ?? "";
            return View(model);
        }
        [HttpPost]
        public IActionResult GetList(SearchRattingViewModel model)
        {
            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            model.Branch = tokenModel.BranchCode ?? "";
            var data = _ratingSaleService.GetList(model, out total);
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
            var data = _ratingSaleService.GetById(id);
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;
            return PartialView("Edit", data.DataItem);
        }
        public IActionResult IsDelete(int id)
        {
            //trace log
            LogModel.Action = ActionType.Delete;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _ratingSaleService.GetById(id);
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
                LogModel.Message = "Xóa doanh số thành công";
                Logger.LogInformation(LogModel.ToString());

                return Json(new { status = true, name = data.DataItem.Sale });
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
            var data = _ratingSaleService.Raw_Delete(id);
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;
            return Ok(new { status = true, mess = "Xóa doanh số thành công" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertOrUpdate(RatingSaleViewModel model)
        {
            if (!Utils.CheckDateTime2(model.RevenueDate))
            {
                var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "RevenueDate", ErrorMessage = "Ngày tháng không đúng định dạng dd-MM-yyyy" } };
                var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = jsonerrs;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, data = jsonerrs });
            }
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

            if (model.Id <= 0)
            {
                var data = new RatingSale()
                {
                    IsByTime = 1,
                    CreateAt = DateTime.Now,
                    CreateBy = tokenModel.StaffCode,
                    UpdateAt = dateNull,
                    UpdateBy = null,
                    RevenueSale = model.RevenueSale,
                    Sale = model.Sale,
                    RevenueDate = DateTime.ParseExact(model.RevenueDate, "dd-MM-yyyy", null),
                };
                try
                {
                    _ratingSaleService.Raw_Insert(data);
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, mess = "Lỗi hệ thống !" });
                }

            }
            else
            {
                var oldData = _ratingSaleService.Raw_Get(model.Id);
                var data = new RatingSale()
                {
                    Id = model.Id,
                    IsByTime = 1,
                    CreateAt = oldData.CreateAt,
                    CreateBy = oldData.CreateBy,
                    UpdateAt = DateTime.Now,
                    UpdateBy = tokenModel.StaffCode,
                    RevenueSale = model.RevenueSale,
                    Sale = model.Sale,
                    RevenueDate = DateTime.ParseExact(model.RevenueDate, "dd-MM-yyyy", null),
                };
                try
                {
                    _ratingSaleService.Raw_Update(data);
                }
                catch (Exception ex)
                {
                    return Json(new { status = false, mess = "Lỗi hệ thống !" });
                }
            }
            return Json(new { status = true });
        }
        #region GetAllBranches - HungVX
        [HttpGet]
        public IActionResult GetAllBranches()
        {
            var myBranch = tokenModel.BranchCode ?? "";
            // trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "branch(es)";

            IBranch iBranch = new BranchImp();
            var data = iBranch.GetAllBranches().Result.Where(x => x.BranchCode == myBranch).ToList();
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data });
        }
        #endregion

        /// <summary>
        /// GET OFFICE BY BRANCH
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
        /// 
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

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data.Result });
        }
        #endregion

        #region GetTeams - HungVX
        [HttpGet]
        public IActionResult GetTeams(string department)
        {
            // trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "team(s) in a department";
            LogModel.Data = (new { department = department }).ToDataString();

            ITeamInCompany iTeam = new TeamInCompanyImp();
            var data = iTeam.GetAllTeamsInDepartment(department);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data.Result });
        }
        #endregion

        #region GetTeams - HungVX
        /// <summary>
        /// GET ALL STAFF SALE IN TEAM
        /// </summary>
        /// <param name="teamCode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetStaffSale(string teamCode)
        {
            // trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "staff(s) in a ";
            LogModel.Data = (new { team = teamCode }).ToDataString();

            var data = _accountService.GetEmployeeInfoByTeam(teamCode).Result.ToList();
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data });
        }
        #endregion

        #region GetPositions - HungVX
        [HttpGet]
        public IActionResult GetPositions()
        {
            IPosition iTeam = new PositionImp();
            var data = iTeam.GetAllPositions();
            if (data.Error)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Lỗi hệ thống" } });
            }

            if (data.Result == null || data.Result.Count == 0)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Không tìm thấy dữ liệu" } });
            }

            return Json(new { Result = 200, Data = data.Result });
        }
        #endregion

        #region HungVX - GetRoles
        [HttpGet]
        public IActionResult GetRoles()
        {
            List<RoleViewModel> lst = new List<RoleViewModel>()
            {
                new RoleViewModel{Key=1, Value="Admin"},
                new RoleViewModel{Key=2, Value="Kế toán"},
                new RoleViewModel{Key=3, Value="HR"},
                new RoleViewModel{Key=4, Value="Sale"},
                new RoleViewModel{Key=5, Value="Leader Sale"},
                new RoleViewModel{Key=6, Value="Sale Manager"},
                new RoleViewModel{Key=7, Value="Sale Admin"},
                new RoleViewModel{Key=8, Value="TeleSale"},
                new RoleViewModel{Key=9, Value="Leader TeleSale"},
                new RoleViewModel{Key=10, Value="Trưởng khối"},
                new RoleViewModel{Key=11, Value="Cộng tác viên"}
            };

            return Json(new { Result = 200, Data = lst });
        }
        #endregion
        #region HungVX Validate
        private List<ErrorResult> validform(RatingSaleViewModel entity)
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
