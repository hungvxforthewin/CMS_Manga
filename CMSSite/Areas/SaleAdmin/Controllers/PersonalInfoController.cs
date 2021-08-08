using CRMBussiness;
using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMBussiness.ViewModel;
using CRMModel.Models.Data;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using EncodeByMd5;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CRMSite.Areas.SaleAdmin.Controllers
{
    [Area("SaleAdmin")]
    [Authorize]
    public class PersonalInfoController : BaseController
    {
        private IPersonalInfo _iPersonalInfo;
        public readonly IAccount _accountService;
        public readonly IContractStaff _contractStaff;
        private IWebHostEnvironment _env;
        private string _saveFileFolder;
        //NamNP encode pass
        private EncodeImp _eCode;

        public PersonalInfoController(IConfiguration configuration, IAccount accountService, IContractStaff contractStaff, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IWebHostEnvironment env)
            : base(httpContextAccessor, logger)
        {
            _iPersonalInfo = new PersonalInfoImp();
            _accountService = accountService;
            _contractStaff = contractStaff;
            _eCode = new EncodeImp();

            LogModel.ItemName = "staff(s)";
            _env = env;
            _saveFileFolder = _env.WebRootPath + "\\Uploads\\Files";
        }

        #region Index - khanhkk
        [HttpGet]
        public IActionResult Index()
        {
            //write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return View();
        }
        #endregion

        #region GetList - hungvx
        [HttpPost]
        public IActionResult GetList(SearchPersonalInfoModel model)
        {
            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            var data = _iPersonalInfo.GetPersonalInformationList(model, out total);
            foreach (var item in data.Result)
            {
                if (item.Role == 4)
                {
                    item.PositionName = "Sale";
                }
                else if (item.Role == 5)
                {
                    item.PositionName = "Leader Sele";
                }
                else if (item.Role == 6)
                {
                    item.PositionName = "Sale Manager";
                }
                else if (item.Role == 7)
                {
                    item.PositionName = "Sale Admin";
                }
                else if (item.Role == 8)
                {
                    item.PositionName = "TeleSale";
                }
                else if (item.Role == 9)
                {
                    item.PositionName = "Leader Tele";
                }
                else
                {
                    item.PositionName = "Không xác định";
                }
            }

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

            return Json(new { Data = data.Result, Total = total });
        }
        #endregion

        #region GetById - khanhkk
        [HttpGet]
        public IActionResult GetById(long id)
        {
            //trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _iPersonalInfo.GetInfoById(id);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            //if (data.Error)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { "Lỗi hệ thống" } });
            //}

            //if (data.Result == null || data.Result.Count == 0)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { "Không tìm thấy dữ liệu chấm công" } });
            //}

            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data.Result });
        }
        #endregion 

        #region View - khanhkk
        public IActionResult View(long id)
        {
            //trace log
            LogModel.Action = ActionType.ViewInfo;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _iPersonalInfo.GetInfoById(id);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            //if (data.Error)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { "Lỗi hệ thống" } });
            //}

            //if (data.Result == null || data.Result.Count == 0)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { "Không tìm thấy dữ liệu chấm công" } });
            //}

            //write trace log
            LogModel.Result = ActionResultValue.ViewSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return PartialView("View", data.Result.First());
        }
        #endregion

        #region GetAllBranches - khanhkk
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
        #region GetDepartments - khanhkk
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

        #region GetTeams - khanhkk
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

        #region GetPositions - khanhkk
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
                new RoleViewModel{Key=11, Value="Cộng tác viên"},
            };

            return Json(new { Result = 200, Data = lst });
        }
        #endregion

        #region GetAllowanceOrDeductByType - khanhkk
        [HttpGet]
        public IActionResult GetAllowanceOrDeductByType(byte type)
        {
            // trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "allowance(s)";
            LogModel.Data = (new { type = type }).ToDataString();

            IAllowanceOrDeduct iTeam = new AllowanceOrDeductImp();
            var data = iTeam.GetAllowanceOrDeductByType(type);
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

        #region Import Excel
        public class ErrorResult
        {
            public string Field { get; set; }
            public string ErrorMessage { get; set; }
        }
        [HttpPost]
        public IActionResult upFile(IFormFile file)
        {
            var files = HttpContext.Request.Form.Files[0];
            string fileName = string.Empty;
            if (files != null)
            {
                SiteConst.UploadStatus result = Helper.UploadFile(files, _saveFileFolder, out fileName);
            }
            return Ok(new { status = true, data = fileName });
        }
        [HttpPost]
        public IActionResult InsertOrUpdate(PersonViewModel model)
        {
           
            //trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = model.Id == 0 ? ActionType.Create : ActionType.Update;

            List<ErrorResult> Errors = new List<ErrorResult>();
            ModelState.Remove("model.Id");
            ModelState.Remove("model.Gender");
            ModelState.Remove("model.StartDateOffical");
            ModelState.Remove("model.Pass");
            ModelState.Remove("model.Email");
            ModelState.Remove("model.Phone");
            ModelState.Remove("model.IdCard");
            ModelState.Remove("model.FullName");
            ModelState.Remove("model.Religion");
            ModelState.Remove("model.TeamCode");
            ModelState.Remove("model.UserName");
            ModelState.Remove("model.BirthPlace");
            ModelState.Remove("model.BranchCode");
            ModelState.Remove("model.OfficeCode");
            ModelState.Remove("model.Nationality");
            ModelState.Remove("model.PositionCode");
            ModelState.Remove("model.CurrentAddress");
            ModelState.Remove("model.DepartmentCode");
            ModelState.Remove("model.IdCardIssuedPlace");
            if (!TryValidateModel(model))
            {
                foreach (var modelStateDD in ModelState)
                {
                    string key = modelStateDD.Key;
                    ModelStateEntry modelState = modelStateDD.Value;
                    foreach (ModelError error in modelState.Errors)
                    {
                        ErrorResult er = new ErrorResult();
                        er.ErrorMessage = error.ErrorMessage;// KeyTranslator(key) +  " Không hợp lệ";
                        er.Field = key;
                        Errors.Add(er);
                    }
                }
            }
            Errors.GroupBy(x => x.ErrorMessage).Select(y => y.First()).ToList();
            if (model.Birthday != null && !Utils.CheckDateTime(model.Birthday))
            {
                Errors.Add(new ErrorResult() { Field = "Ex", ErrorMessage = "Ngày sinh sai định dạng dd/MM/yyyy" });

                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = Errors.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return Json(new { success = false, lst = Errors });
            }
            if (model.IdCardIssuedDate != null && !Utils.CheckDateTime(model.IdCardIssuedDate))
            {
                Errors.Add(new ErrorResult() { Field = "Ex", ErrorMessage = "Ngày cấp sai định dạng dd/MM/yyyy" });

                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = Errors.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return Json(new { success = false, lst = Errors });
            }
            if (model.StartDateOfProbation != null && !Utils.CheckDateTime(model.StartDateOfProbation))
            {
                Errors.Add(new ErrorResult() { Field = "Ex", ErrorMessage = "Ngày thử việc sai định dạng dd/MM/yyyy" });

                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = Errors.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return Json(new { success = false, lst = Errors });
            }
            if (model.EndDate != null && !Utils.CheckDateTime(model.EndDate))
            {
                Errors.Add(new ErrorResult() { Field = "Ex", ErrorMessage = "Ngày thôi việc sai định dạng dd/MM/yyyy" });

                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = Errors.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return Json(new { success = false, lst = Errors });
            }
            if (model.StartDateOffical != null && !Utils.CheckDateTime(model.StartDateOffical))
            {
                Errors.Add(new ErrorResult() { Field = "Ex", ErrorMessage = "Ngày nhận việc sai định dạng dd/MM/yyyy" });

                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = Errors.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return Json(new { success = false, lst = Errors });
            }
            if (Errors.Count > 0)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = Errors.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return Json(new { success = false, lst = Errors });
            }
            else
            {
                if (model.Id <= 0)
                {
                    DateTime? valueDate = null;
                    string codeStaff = string.Empty;
                  
                    var account = new Account()
                    {
                        AccountName = model.UserName,
                        AccountPassword = model.Pass != null ? _eCode.EncodeMD5(model.Pass) : _eCode.EncodeMD5("abc123"),
                        AccountFullName = model.FullName,
                        isEnable = true,
                        CreateDate = DateTime.Now,
                    };
                    var account_result = new Account();
                    try
                    {
                        account_result = _accountService.Raw_Insert(account);
                    }
                    catch (Exception ex)
                    {
                        //write trace log 
                        LogModel.Result = ActionResultValue.CreateFailed;
                        LogModel.Message = "Thêm tài khoản nhân viên không thành công";
                        Logger.LogError(ex, LogModel.ToString());

                        Errors.Add(new ErrorResult() { Field = "Ex", ErrorMessage = "Tài khoản đã được sử dụng" });
                        return Json(new { success = false, lst = Errors });
                    }
                    

                    //write trace log 
                    LogModel.Result = ActionResultValue.CreateSuccess;
                    LogModel.Message = "Thêm nhân viên thành công";
                    Logger.LogInformation(LogModel.ToString());
                }
                else
                {
                    var accountOld = _accountService.Raw_Get(model.Id);
                    //UPDATE ACCOUNT
                    var account = new Account()
                    {
                        AccountID = model.Id,
                        AccountName = model.UserName,
                        AccountPassword = model.Pass != null ? _eCode.EncodeMD5(model.Pass) : _eCode.EncodeMD5("abc123"),
                        AccountFullName = model.FullName,
                    };
                    var account_result = new Account();
                    try
                    {
                        account_result = _accountService.Raw_Update(account);
                    }
                    catch (Exception ex)
                    {
                        //write trace log 
                        LogModel.Result = ActionResultValue.UpdateFailed;
                        LogModel.Message = "Cập nhật tài khoản nhân viên không thành công";
                        Logger.LogError(ex, LogModel.ToString());

                        Errors.Add(new ErrorResult() { Field = "Ex", ErrorMessage = "Có lỗi xảy ra !" });
                        return Json(new { success = false, lst = Errors });
                    }
                    DateTime? valueDate = null;
                    var contractStaffOld = _contractStaff.Raw_Get(model.IdConstracStaff);
                    //UPDATE CONTRACT STAFF
                   

                    //write trace log 
                    LogModel.Result = ActionResultValue.UpdateSuccess;
                    LogModel.Message = "Cập nhật nhân viên thành công";
                    Logger.LogInformation(LogModel.ToString());
                }
                return Json(new { success = true, lst = Errors });
            }
        }
        #endregion

        #region HungVX-Delete
        public IActionResult Delete(int id)
        {
            //trace log
            LogModel.Action = ActionType.Delete;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _accountService.Raw_Get(id);
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
                LogModel.Result = ActionResultValue.AccessSuccess;
                LogModel.Message = "Xóa nhân viên thành công";
                Logger.LogInformation(LogModel.ToString());

                return Json(new { status = true, name = data.AccountFullName });
            }
        }

        //public IActionResult ConfirmDelete(int id)
        //{
        //    //trace log
        //    LogModel.Action = ActionType.Delete;
        //    LogModel.Data = (new { id = id }).ToDataString();

        //    var data = _accountService.Raw_Get(id);
        //    if (data != null)
        //    {
        //        var createResult = _contractStaff.DeleteAnEmployee(data.CodeStaff);
        //        if (!createResult.Error)
        //        {
        //            //write trace log
        //            LogModel.Result = ActionResultValue.DeleteSuccess;
        //            LogModel.Message = "Xóa nhân sự thành công";
        //            Logger.LogInformation(LogModel.ToString());

        //            return Json(new { status = true, Message = "Xóa nhân sự thành công!" });
        //        }
        //        else
        //        {
        //            //write trace log
        //            LogModel.Result = ActionResultValue.DeleteFailed;
        //            LogModel.Message = "Xóa nhân sự không thành công";
        //            Logger.LogError(LogModel.ToString());

        //            return Json(new { status = false, Message = "Xóa nhân sự không thành công!" });
        //        }
        //    }
        //    //write trace log
        //    LogModel.Result = ActionResultValue.NotFoundData;
        //    LogModel.Message = "Không tìm thấy thông tin nhân viên trên hệ thống";
        //    Logger.LogWarning(LogModel.ToString());

        //    return Json(new { status = false, Message = "Không tìm thấy thông tin nhân viên trên hệ thống!" });
        //}
        #endregion
    }
}
