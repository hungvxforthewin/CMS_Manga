using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMSite.Common;
using CRMSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CRMSite.Controllers
{
    public class SelectionDataController : BaseController
    {
        public SelectionDataController(IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        #region GetProducts
        public IActionResult GetProducts()
        {
            // trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "product(s)";

            IProduct iPro = new ProductIpm();
            var data = iPro.GetProducts();
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data.Result });
        }
        #endregion

        #region GetShowUpList
        public IActionResult GetShowUpList()
        {
            //trace log
            LogModel.ItemName = "event(s)";
            LogModel.Action = ActionType.GetInfo;

            IEvent iEvent = new EventImp();
            var getResult = iEvent.GetEventList(tokenModel.BranchCode);
            var handleResult = HandleGetResult(getResult);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = getResult.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            var data = getResult.Result;
            List<byte> saleRoles = new List<byte> { 4, 5, 6, 10, 11 };
            if (saleRoles.Contains(tokenModel.Role))
            {
                data = data.Where(x => x.CodeEvent.Contains("CC")).ToList();
            }
            else if (tokenModel.Role == 7) {}
            else
            {
                data = data.Where(x => !x.CodeEvent.Contains("CC")).ToList();
            }    

            return Json(new { Result = 200, Data = data });
        }
        #endregion

        #region GetActiveShowUpList
        public IActionResult GetActiveShowUpList(string currentShowUpCode)
        {
            //trace log
            LogModel.ItemName = "active event(s)";
            LogModel.Action = ActionType.GetInfo;

            IEvent iEvent = new EventImp();
            var getResult = iEvent.GetEventList(tokenModel.BranchCode);
            var handleResult = HandleGetResult(getResult);
            if (handleResult != null) return handleResult;

            var shows = getResult.Result.Where(x => x.EventTime.Value.Date >= DateTime.Now.Date).ToList();
            if (!string.IsNullOrEmpty(currentShowUpCode) && shows.Where(x => x.CodeEvent == currentShowUpCode).Count() == 0)
            {
                var getAResult = iEvent.GetByShowUpCode(currentShowUpCode);
                var handleAResult = HandleGetResult(getAResult);
                if (handleAResult != null) return handleAResult;

                var currentShowUp = getAResult.Result.First();
                currentShowUp.EventTimeString = currentShowUp.EventTime?.ToString(SiteConst.Format.FullDateTimeFormat);
                shows.Add(getAResult.Result.First());
            }

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = getResult.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            List<byte> saleRoles = new List<byte> { 4, 5, 6, 10, 11 };
            if (saleRoles.Contains(tokenModel.Role))
            {
                shows = shows.Where(x => x.CodeEvent.Contains("CC")).ToList();
            }
            else if (tokenModel.Role == 7) {}
            else
            {
                shows = shows.Where(x => !x.CodeEvent.Contains("CC")).ToList();
            }

            return Json(new { Result = 200, Data = shows });
        }
        #endregion

        #region GetInvestorResource
        public IActionResult GetInvestorResource()
        {
            //trace log
            LogModel.ItemName = "investor resource(s)";
            LogModel.Action = ActionType.GetInfo;

            IWhereToFindInvestor iResource = new WhereToFindInvestorImp();
            var getResult = iResource.GetInvestResourceList();
            var handleResult = HandleGetResult(getResult);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = getResult.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = getResult.Result });
        }
        #endregion

        #region GetSale
        public IActionResult GetSale()
        {
            //trace log
            LogModel.ItemName = "sale(s)";
            LogModel.Action = ActionType.GetInfo;

            IAccount iAcc = new AccountImp();
            var getResult = iAcc.GetEmployeeListByType(false);
            var handleResult = HandleGetResult(getResult);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = getResult.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = getResult.Result.Where(x => x.BranchCode == tokenModel.BranchCode).ToList() });
        }
        #endregion

        #region GetTeleSale
        public IActionResult GetTeleSale()
        {
            //trace log
            LogModel.ItemName = "telesale(s)";
            LogModel.Action = ActionType.GetInfo;

            IAccount iAcc = new AccountImp();
            var getResult = iAcc.GetEmployeeListByType(true);

            var handleResult = HandleGetResult(getResult);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = getResult.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = getResult.Result.Where(x => x.BranchCode == tokenModel.BranchCode).ToList() });
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

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data.Result });
        }
        #endregion

        #region GetDepartments - khanhkk
        [HttpGet]
        public IActionResult GetDepartments(string office)
        {
            // trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "department(s) in an office";
            LogModel.Data = (new { Office = office }).ToDataString();

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

        #region GetDepartmentsInBranch - khanhkk
        [HttpGet]
        public IActionResult GetDepartmentsInBranch(string branch)
        {
            // trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "department(s) in a branch";
            LogModel.Data = (new { Branch = branch }).ToDataString();

            IDepartment iDept = new DepartmentImp();
            var data = iDept.GetDepartmentListInBranch(branch);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

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

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data.Result });
        }
        #endregion

        #region GetOfficeByBranch
        /// <summary>
        /// GET OFFICE BY BRANCH
        /// </summary>
        /// <param name="branch">branch code</param>
        /// <returns></returns>
        /// 
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

        #region GetStaffSale
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

            IAccount _accountService = new AccountImp();
            var data = _accountService.GetEmployeeInfoByTeam(teamCode).Result
                        .Where(x => x.Role == 4 || x.Role == 5 || x.Role == 11).ToList();
            //var handleResult = HandleGetResult(data);
            //if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data });
        }
        #endregion
    }
}
