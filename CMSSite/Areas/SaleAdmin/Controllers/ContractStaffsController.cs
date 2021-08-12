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
    public class ContractStaffsController : BaseController
    {
        public readonly IInvestor _investor;
        public readonly IDeposit _deposit;
        public readonly IStatusContractInvestors _statusContractInvestors;
        public readonly IContractInvester _contractInvester;
        public readonly IContractInvestorInstallments _contractInvestorInstallments;
        public readonly IIntermediaries _intermediaries;
        public ContractStaffsController(IHttpContextAccessor httpContextAccessor, IInvestor investor, IStatusContractInvestors statusContractInvestors, IContractInvester contractInvester, IDeposit deposit, IContractInvestorInstallments contractInvestorInstallments, IIntermediaries intermediaries, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            _investor = investor;
            _deposit = deposit;
            _statusContractInvestors = statusContractInvestors;
            _contractInvester = contractInvester;
            _contractInvestorInstallments = contractInvestorInstallments;
            _intermediaries = intermediaries;
            LogModel.ItemName = "contract(s)";
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
        public IActionResult GetList(SearchContractInvesterViewModel model)
        {
            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            model.BranchCode = tokenModel.BranchCode;
            int total;
            var data = _contractInvester.GetList(model, out total);
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

            if (data.Result != null)
                foreach (var item in data.Result)
                {
                    if (item.IdStatusContract.Trim().ToLower() == "WaitPayDone".ToLower())
                    {
                        item.NameStatus = "Chờ Duyệt";
                    }
                    else if (item.IdStatusContract.Trim().ToLower() == "PayDone".ToLower())
                    {
                        item.NameStatus = "Đã Duyệt";
                    }
                    else
                    {
                        item.NameStatus = "Đang thanh toán";
                    }
                }           
            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Data = data.Result, Total = total });
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

            //if (getResult.Error)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            //}

            //if (getResult.Result == null || getResult.Result.Count == 0)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { SiteConst.NotFoundError } });
            //}

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = getResult.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = getResult.Result });
        }
        #endregion

        #region GetSale
        //public IActionResult GetSale()
        //{
        //    //trace log
        //    LogModel.ItemName = "sale(s)";
        //    LogModel.Action = ActionType.GetInfo;

        //    IAccount iAcc = new AccountImp();
        //    //var getResult = iAcc.GetEmployeeListByType(false);
        //    var handleResult = HandleGetResult(getResult);
        //    if (handleResult != null) return handleResult;

        //    //if (getResult.Error)
        //    //{
        //    //    return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
        //    //}

        //    //if (getResult.Result == null || getResult.Result.Count == 0)
        //    //{
        //    //    return Json(new { Result = 400, Errors = new List<string> { SiteConst.NotFoundError } });
        //    //}

        //    //write trace log
        //    LogModel.Result = ActionResultValue.GetInfoSuccess;
        //    LogModel.Data = getResult.Result.ToDataString();
        //    Logger.LogInformation(LogModel.ToString());

        //    return Json(new { Result = 200, Data = getResult.Result });
        //}
        #endregion

        #region GetSaleAndCTV
        //public IActionResult GetSaleAndCTV()
        //{
        //    IAccount iAcc = new AccountImp();
        //    var getResult = iAcc.GetEmployeeListByType(false);
        //    if (getResult.Error)
        //    {
        //        return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
        //    }

        //    if (getResult.Result == null || getResult.Result.Count == 0)
        //    {
        //        return Json(new { Result = 400, Errors = new List<string> { SiteConst.NotFoundError } });
        //    }

        //    return Json(new { Result = 200, Data = getResult.Result });
        //}
        #endregion

        #region GetTeleSale
        //public IActionResult GetTeleSale()
        //{
        //    //trace log
        //    LogModel.ItemName = "telesale(s)";
        //    LogModel.Action = ActionType.GetInfo;

        //    IAccount iAcc = new AccountImp();
        //    var getResult = iAcc.GetEmployeeListByType(true);
        //    var handleResult = HandleGetResult(getResult);
        //    if (handleResult != null) return handleResult;

        //    //if (getResult.Error)
        //    //{
        //    //    return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
        //    //}

        //    //if (getResult.Result == null || getResult.Result.Count == 0)
        //    //{
        //    //    return Json(new { Result = 400, Errors = new List<string> { SiteConst.NotFoundError } });
        //    //}

        //    //write trace log
        //    LogModel.Result = ActionResultValue.GetInfoSuccess;
        //    LogModel.Data = getResult.Result.ToDataString();
        //    Logger.LogInformation(LogModel.ToString());

        //    return Json(new { Result = 200, Data = getResult.Result });
        //}
        #endregion

        #region GetContractStaffStatus
        public IActionResult GetContractStaffStatus()
        {
            var data = new List<ContractStaffStatus>()
            {
                new ContractStaffStatus(){Key = 1, Value = "Đã duyệt"},
                new ContractStaffStatus(){Key = 0, Value = "Chưa duyệt"},
            };
            return Json(new { status = true, data });
        }
        #endregion

        [HttpPost]
        [AllowAnonymous]
        public JsonResult getmv()
        {
            return Json(new
            {
                Status = 200,
                ModelView = new ContractInvesterViewModel()
            });
        }

        #region View - Hungvx
        public IActionResult View(int id)
        {
            //trace log
            LogModel.Action = ActionType.ViewInfo;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _contractInvester.GetInfoById(id);
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

            foreach (var item in data.Result)
            {
                item.ListInforBill = _contractInvester.GetListBill(id).Result.ToList<InforBill>();
                var result = _investor.CheckSumAmountDepositWithContract(item.Phone, item.CodeContract);
                item.DepositAmount = result != null ? decimal.Parse(result.SumPaymentAmount) : 0;
            }

            //write trace log
            LogModel.Result = ActionResultValue.ViewSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return PartialView("View", data.Result.First());
        }
        #endregion

        #region View - Hungvx
        public IActionResult Preview(int id)
        {
            //trace log
            LogModel.Action = ActionType.ViewInfo;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _contractInvester.GetInfoById(id);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            foreach (var item in data.Result)
            {
                item.ListInforBill = _contractInvester.GetListBill(id).Result.ToList<InforBill>();
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
            LogModel.Result = ActionResultValue.ViewSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return PartialView("View", data.Result.First());
        }
        #endregion

        #region Edit - Hungvx
        public IActionResult Edit(int id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _contractInvester.GetInfoById(id);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            foreach (var item in data.Result)
            {
                item.ListInforBill = _contractInvester.GetListBill(id).Result.ToList<InforBill>();
                var result = _investor.CheckSumAmountDepositWithContract(item.Phone, item.CodeContract);
                item.DepositAmount = result != null ? decimal.Parse(result.SumPaymentAmount) : 0;
            }
            //if (data.Error)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { "Lỗi hệ thống" } });
            //}

            //if (data.Result == null || data.Result.Count == 0)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { "Không tìm thấy dữ liệu" } });
            //}

            var contractInvestorOld = _contractInvester.GetInfoById(data.Result[0].Id);
            if (contractInvestorOld.Result[0].IdStatusContract == "PayDone")
            {
                var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "DepositDate", ErrorMessage = "Hợp đồng đã được duyệt thì không được chỉnh sửa" } };
                var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = jsonerrs;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, data = jsonerrs });
            }


            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return PartialView("Edit", data.Result.First());
        }
        #endregion

        #region Hungvx InsertOrUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertOrUpdate(ContractInvesterViewModel model)
        {
            //trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = model.Id == 0 ? ActionType.Create : ActionType.Update;

            DateTime? nullValue = null;
            //GIƠI HẠN THANH TOÁN
            //if (model.ListInforBill == null || model.ListInforBill.Count != 1)
            //{
            //    var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "BillMoney", ErrorMessage = "Chỉ được thanh toán 1 lần, cọc 1 lần" } };
            //    var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);
            //    return Json(new { status = false, data = jsonerrs });
            //}
            //CHECK DATETIME
            if (model.Birthday != null && !Utils.CheckDateTime(model.Birthday))
            {
                var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "Birthday", ErrorMessage = "Ngày sinh không đúng định dạng dd/MM/yyyy" } };
                var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = jsonerrs;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, data = jsonerrs });
            }
            if (model.DateOfIssuance != null && !Utils.CheckDateTime(model.DateOfIssuance))
            {
                var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "DateOfIssuance", ErrorMessage = "Ngày cấp không đúng định dạng dd/MM/yyyy" } };
                var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = jsonerrs;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, data = jsonerrs });
            }
            if (model.DepositDate != null && !Utils.CheckDateTime(model.DepositDate))
            {
                var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "DepositDate", ErrorMessage = "Ngày đặt cọc không đúng định dạng dd/MM/yyyy" } };
                var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = jsonerrs;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, data = jsonerrs });
            }
            if (!Utils.CheckDateTime(model.CreateDate))
            {
                var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "CreateDate", ErrorMessage = "Ngày vào HĐ không đúng định dạng dd/MM/yyyy" } };
                var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = jsonerrs;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, data = jsonerrs });
            }
            if (model.ListInforBill != null)
                foreach (var item in model.ListInforBill)
                {
                    if (!Utils.CheckDateTime(item.DateBill))
                    {
                        var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "DateBill", ErrorMessage = "Ngày thanh toán HĐ không đúng định dạng dd/MM/yyyy" } };
                        var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                        //write trace log 
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = jsonerrs;
                        Logger.LogWarning(LogModel.ToString());

                        return Json(new { status = false, data = jsonerrs });
                    }
                }
            DateTime? lastBillDate = null;
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
            //if (model.Id <= 0 && model.CodeDeposit != null)
            //{
            //    var checkDeposit = _deposit.GetByCode(model.CodeDeposit);
            //    if (checkDeposit != null)
            //    {
            //        var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "CodeDeposit", ErrorMessage = "Mã đặt cọc Đã tồn tại" } };
            //        var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);
            //        return Json(new { status = false, data = jsonerrs });
            //    }
            //}
            if (model.Id <= 0)
            {
                var checkContract = _contractInvester.GetByCode(model.CodeContract);
                if (checkContract != null)
                {
                    var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "CodeContract", ErrorMessage = "Mã HĐ Đã tồn tại" } };
                    var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                    //write trace log 
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = jsonerrs;
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, data = jsonerrs });
                }
            }

            //khanhkk added
            //check min investment amount
            if (model.InvestmentAmount is null || (model.InvestmentAmount / 50000) < 1000)
            {
                var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "InvestmentAmount", ErrorMessage = "Số tiền tối thiểu là 50,000,000 VND" } };
                var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = jsonerrs;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, data = jsonerrs });
            }

            if (model.InvestmentAmount % (50000 * 500) != 0)
            {
                var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "InvestmentAmount", ErrorMessage = "Mức tiền tăng tối thiểu là 25,000,000 VND/1 lần" } };
                var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = jsonerrs;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, data = jsonerrs });
            }
            //khanhkk added
            decimal countBill = 0M;
            if (model.ListInforBill != null)
            {
                countBill = model.DepositAmount != null ? model.DepositAmount.Value : 0M;
                if (model.ListInforBill != null)
                    foreach (var item in model.ListInforBill)
                    {
                        countBill += item.BillMoney;
                    }
            }
            // CHECK MONEY SỐ TIỀN THANH TOÁN
            //if (model.ListInforBill != null)
            //{
            //    countBill = model.DepositAmount != null ? model.DepositAmount.Value : 0M;
            //    if (model.ListInforBill != null)
            //        foreach (var item in model.ListInforBill)
            //        {
            //            countBill += item.BillMoney;
            //        }
            //    if (countBill != model.InvestmentAmount)
            //    {
            //        var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "InvestmentAmount", ErrorMessage = "Thanh toán vượt quá hoặc không đủ giá trị HĐ" } };
            //        var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);
            //        return Json(new { status = false, data = jsonerrs });
            //    }
            //}
            // Check DateTime Deposit, DateTime 
            if (model.ListInforBill != null)
            {
                int sumBill = model.ListInforBill.Count;
                DateTime firstTime = DateTime.ParseExact(model.ListInforBill.First().DateBill, SiteConst.Format.DateFormat, null);
                DateTime? valiDate = null;
                if (model.DepositDate != null)
                {
                    DateTime depositTime = DateTime.ParseExact(model.DepositDate, SiteConst.Format.DateFormat, null);
                    if (depositTime.Date > firstTime.Date)
                    {
                        var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "DepositDate", ErrorMessage = "Ngày thanh toán phải lớn hơn hoặc bằng ngày cọc" } };
                        var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                        //write trace log 
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = jsonerrs;
                        Logger.LogWarning(LogModel.ToString());

                        return Json(new { status = false, data = jsonerrs });
                    }
                }
                foreach (var item in model.ListInforBill)
                {
                    if (valiDate != null)
                    {
                        if (DateTime.ParseExact(item.DateBill, SiteConst.Format.DateFormat, null).Date > valiDate.Value.Date)
                        {
                            var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "DateBill", ErrorMessage = "Ngày thanh toán sau phải hơn hoặc bằng lần trước" } };
                            var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                            //write trace log 
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = jsonerrs;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new { status = false, data = jsonerrs });
                        }
                        valiDate = DateTime.ParseExact(item.DateBill, SiteConst.Format.DateFormat, null);
                    }
                }
                lastBillDate = DateTime.ParseExact(model.ListInforBill[sumBill - 1].DateBill, SiteConst.Format.DateFormat, null);
            }
            //HUNGVX-INVESTOR
            var investor_result = new Investor();
            if (model.IdInvestor <= 0)
            {
                //HUNGVX-INSERT NEW INVESTOR
                var investor = new Investor()
                {
                    Name = model.Name,
                    IdCard = model.IdCard,
                    DateOfIssuance = model.DateOfIssuance != null ? DateTime.ParseExact(model.DateOfIssuance, SiteConst.Format.DateFormat, null) : nullValue,
                    CodeInvestor = Guid.NewGuid().ToString(),
                    AddressIssuance = model.IsCMT == "0" ? model.AddressIssuance : model.AddressIssuanceCC,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    Birthday = model.Birthday != null ? DateTime.ParseExact(model.Birthday, SiteConst.Format.DateFormat, null) : nullValue,
                    Status = 1,
                    Stock = (long)model.InvestmentAmount / (50000),
                    AccountBank = model.AccountBank,
                    Bank = model.Bank,
                    PersonalTaxCode = model.PersonalTaxCode,
                    Address = model.Address,
                    IsCMT = model.IsCMT == "0" ? true : false
                };
                try
                {
                    investor_result = _investor.Raw_Insert(investor);

                }
                catch (Exception ex)
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.CreateFailed;
                    LogModel.Message = "Tạo thông tin khách hàng không thành công";
                    Logger.LogError(ex, LogModel.ToString());

                    throw ex;
                }
            }
            else
            {
                var investor_old = _investor.Raw_Get(model.IdInvestor);
                var investor = new Investor()
                {
                    Id = model.IdInvestor,
                    Name = model.Name,
                    IdCard = model.IdCard,
                    CodeInvestor = investor_old.CodeInvestor,
                    DateOfIssuance = model.DateOfIssuance != null ? DateTime.ParseExact(model.DateOfIssuance, SiteConst.Format.DateFormat, null) : nullValue,
                    AddressIssuance = model.IsCMT == "0" ? model.AddressIssuance : model.AddressIssuanceCC,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    Birthday = model.Birthday != null ? DateTime.ParseExact(model.Birthday, SiteConst.Format.DateFormat, null) : nullValue,
                    Status = 1,
                    Stock = (long)model.InvestmentAmount / (50000),
                    AccountBank = model.AccountBank,
                    Bank = model.Bank,
                    PersonalTaxCode = model.PersonalTaxCode,
                    Address = model.Address,
                    IsCMT = model.IsCMT == "0" ? true : false
                };

                try
                {
                    investor_result = _investor.Raw_Update(investor);
                }
                catch (Exception ex)
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.UpdateFailed;
                    LogModel.Message = "Cập nhật thông tin khách hàng không thành công";
                    Logger.LogError(ex, LogModel.ToString());

                    throw ex;
                }
            }
            //HUNGVX-EMBER
            var intermediaries_result = new Intermediaries();
            if (model.IdIntermediaries == "0" && !string.IsNullOrEmpty(model.CodeIntermediaries))
            {
                //HUNGVX-INSERT NEW INTERMEDIARIES
                var intermediaries = new Intermediaries()
                {
                    Name = model.NameIntermediaries,
                    CodeIntermediaries = model.CodeIntermediaries,
                    Address = model.AddressIntermediaries,
                    Phone = model.PhoneIntermediaries,
                    TaxCode = model.TaxCodeIntermediaries,
                    CreateDate = DateTime.Now,
                    Status = true,
                    UserCreate = tokenModel.StaffCode,
                    UpdateDate = nullValue
                };
                try
                {
                    intermediaries_result = _intermediaries.Raw_Insert(intermediaries);

                }
                catch (Exception ex)
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.CreateFailed;
                    LogModel.Message = "Tạo thông tin khách hàng trung gian không thành công";
                    Logger.LogError(ex, LogModel.ToString());

                    throw ex;
                }
            }
            else if (model.IdIntermediaries != "0" && !string.IsNullOrEmpty(model.CodeIntermediaries))
            {
                var intermediar_old = _intermediaries.Raw_Get(model.IdIntermediaries);
                var intermediaries = new Intermediaries()
                {
                    Id = int.Parse(model.IdIntermediaries),
                    Name = model.NameIntermediaries,
                    CodeIntermediaries = model.CodeIntermediaries,
                    Address = model.AddressIntermediaries,
                    Phone = model.PhoneIntermediaries,
                    TaxCode = model.TaxCodeIntermediaries,
                    Status = true,
                    UserUpdate = tokenModel.StaffCode,
                    UpdateDate = DateTime.Now,
                    CreateDate = intermediar_old.CreateDate,
                    UserCreate = intermediar_old.UserCreate
                };

                try
                {
                    intermediaries_result = _intermediaries.Raw_Update(intermediaries);
                }
                catch (Exception ex)
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.UpdateFailed;
                    LogModel.Message = "Cập nhật thông tin khách hàng trung gian không thành công";
                    Logger.LogError(ex, LogModel.ToString());

                    throw ex;
                }
            }
            //HUNGVX-COUNT EMBER
            int countIntermediaries = 0;
            if (!string.IsNullOrEmpty(model.CodeIntermediaries))
            {
                countIntermediaries = _contractInvester.CountInntermediariesContract(model.CodeIntermediaries);
            }
            //_contractInvester.CountInntermediariesContract(model.CodeIntermediaries);
            //HUNGVX-CONTRACT INVESTOR
            if (model.Id <= 0)
            {
                //GEN CODECONTRACT(AMBER OR NOT)
                string checkCode = string.Concat(DateTime.Now.ToString("yyyy"), "/SFG/SFI");
                int countContractByToday = _contractInvester.CountDateContract(checkCode);
                int countContractByTodayForAmber = 0;
                if (!string.IsNullOrEmpty(model.CodeIntermediaries))
                    countContractByTodayForAmber = _contractInvester.CountDateContractForAmber(checkCode, model.CodeIntermediaries);
                //HUNGVX-DOWN COUNT CONTRACT
                string newCodeContract = string.Empty;
                if (!string.IsNullOrEmpty(model.CodeIntermediaries))
                {
                    newCodeContract = Utils.GenCodeContractForAmber(countContractByTodayForAmber, model.CodeIntermediaries);
                }
                else
                {
                    newCodeContract = Utils.GenCodeContract(countContractByToday - 2);
                }
                string firstNumber = newCodeContract.Split('/')[0];
                string CodeTransferAgreement = string.Concat(firstNumber, "/", model.CodeIntermediaries, "/", countIntermediaries + 1, "/", DateTime.Now.ToString("ddMM/yyyy"), "/FSFG/SFI");
                string CodeManagementCatalog = string.Concat(firstNumber, "/", model.CodeIntermediaries, "/", countIntermediaries + 1, "/", DateTime.Now.ToString("ddMM/yyyy"), "/HĐQLDM/AFM/C3");
                string CodeSupplementAgreement = string.Concat(firstNumber, "/", model.CodeIntermediaries, "/", countIntermediaries + 1, "/", DateTime.Now.ToString("ddMM/yyyy"), "/FSFG/SFI");
                //HUNGVX-DEPOSIT
                var deposit = new Deposit()
                {
                    CodeDeposit = model.CodeDeposit,
                    NameDeposit = "Deposit Money",
                    CodeInvestor = investor_result.CodeInvestor,
                    DepositAmount = model.DepositAmount != null ? model.DepositAmount.Value : 0M,
                    TeleSale = model.TeleSale,
                    SaleRep = model.SaleRep,
                    Status = 1,
                    DepositForm = model.DepositForm,
                    DepositDate = model.DepositDate != null ? DateTime.ParseExact(model.DepositDate, SiteConst.Format.DateFormat, null) : nullValue
                };
                var deposit_result = new Deposit();
                try
                {
                    if (model.CodeDeposit != null)
                        deposit_result = _deposit.Raw_Update(deposit);
                }
                catch (Exception ex)
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.UpdateFailed;
                    LogModel.Message = "Cập nhật đặt cọc không thành công";
                    Logger.LogError(ex, LogModel.ToString());

                    throw ex;
                }
                int stt = 1;
                string isStatusContract = string.Empty;


                //HUNGVX-CONTRACTINVESTOR
                var contractInvestor = new ContractInvestor();
                if (model.DepositAmount == model.InvestmentAmount)
                {
                    contractInvestor = new ContractInvestor()
                    {
                        CodeContract = newCodeContract,
                        NameContract = "Contract Investor",
                        CodeInvestor = investor_result.CodeInvestor,
                        CodeDeposit = deposit_result.CodeDeposit,
                        InvestmentAmount = model.InvestmentAmount,
                        Sale = model.Sale,
                        TeleSale = model.TeleSale != null ? model.TeleSale : "",
                        SaleRepresent = model.SaleRep,
                        CreateDate = DateTime.Now,
                        IdStatusContract = (countBill >= model.InvestmentAmount.Value) ? "WaitPayDone" : "",
                        PayOffDate = DateTime.ParseExact(model.DateBill, SiteConst.Format.DateFormat, null),
                        CodeIntermediaries = model.CodeIntermediaries != null ? model.CodeIntermediaries : null,
                        CodeManagementCatalog = model.CodeIntermediaries != null ? CodeManagementCatalog : null,
                        CodeSupplementAgreement = model.CodeIntermediaries != null ? CodeSupplementAgreement : null,
                        CodeTransferAgreement = model.CodeIntermediaries != null ? CodeTransferAgreement : null
                    };
                }
                else
                {
                    contractInvestor = new ContractInvestor()
                    {
                        CodeContract = newCodeContract,
                        NameContract = "Contract Investor",
                        CodeInvestor = investor_result.CodeInvestor,
                        CodeDeposit = deposit_result.CodeDeposit,
                        InvestmentAmount = model.InvestmentAmount,
                        Sale = model.Sale,
                        TeleSale = model.TeleSale != null ? model.TeleSale : "",
                        SaleRepresent = model.SaleRep,
                        CreateDate = DateTime.Now,
                        IdStatusContract = (countBill >= model.InvestmentAmount.Value) ? "WaitPayDone" : "",
                        PayOffDate = (countBill >= model.InvestmentAmount.Value) ? lastBillDate : nullValue,
                        CodeIntermediaries = model.CodeIntermediaries != null ? model.CodeIntermediaries : null,
                        CodeManagementCatalog = model.CodeIntermediaries != null ? CodeManagementCatalog : null,
                        CodeSupplementAgreement = model.CodeIntermediaries != null ? CodeSupplementAgreement : null,
                        CodeTransferAgreement = model.CodeIntermediaries != null ? CodeTransferAgreement : null
                    };
                }

                var contractInvestor_result = new ContractInvestor();
                try
                {
                    contractInvestor_result = _contractInvester.Raw_Insert(contractInvestor);
                }
                catch (Exception ex)
                {
                    var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "CodeContract", ErrorMessage = "Lỗi hệ thống !" } };
                    var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);
                    //write trace log 
                    LogModel.Result = ActionResultValue.CreateFailed;
                    LogModel.Message = "Thêm hợp đồng không thành công";
                    Logger.LogError(ex, LogModel.ToString());

                    throw;
                }
                ContractInvestorInstallments lastStatus = new ContractInvestorInstallments();
                //HUNGVX-STATUS --> CONTRACTINVESTORINSTALLMENTS
                if (model.ListInforBill != null)
                    foreach (var item in model.ListInforBill)
                    {
                        stt = stt + 1;
                        var statusInvestor = new ContractInvestorInstallments()
                        {
                            IdStatusContract = (countBill >= model.InvestmentAmount.Value) ? "WaitPayDone" : "tt" + stt,
                            CreateDate = DateTime.ParseExact(item.DateBill, SiteConst.Format.DateFormat, null),
                            PaymentFormat = (byte)item.FormBill,
                            CodeContract = newCodeContract,
                            PaymentAmount = item.BillMoney,
                            DepositCode = null
                        };
                        var statusInvestor_result = new ContractInvestorInstallments();
                        try
                        {
                            statusInvestor_result = _contractInvestorInstallments.Raw_Insert(statusInvestor);
                        }
                        catch (Exception ex)
                        {
                            var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "CodeContract", ErrorMessage = "Lỗi hệ thống !" } };
                            var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);
                            //write trace log 
                            LogModel.Result = ActionResultValue.CreateFailed;
                            LogModel.Message = "Thêm phiếu thanh toán không thành công";
                            Logger.LogError(ex, LogModel.ToString());

                            throw;
                        }

                    }
                lastStatus = _contractInvestorInstallments.GetLastStatus(model.CodeContract);
                if (lastStatus == null)
                {
                    lastStatus = new ContractInvestorInstallments();
                    lastStatus.IdStatusContract = "";
                }
                //HUNGVX-UPDATE CODECONTRACT
                _contractInvestorInstallments.UpdateCodeContract(deposit_result.CodeDeposit, contractInvestor_result.CodeContract);

                //write trace log 
                LogModel.Result = ActionResultValue.CreateSuccess;
                LogModel.Message = "Thêm hợp đồng thành công";
                Logger.LogInformation(LogModel.ToString());

            }
            else
            {
                var contractInvestorOld = _contractInvester.GetInfoById(model.Id);
                var codeContractOld = contractInvestorOld.Result[0].CodeContract;
                //HUNGVX-IF CHANGE TO CONTRACT FOR AMBER
                if (!string.IsNullOrEmpty(model.CodeIntermediaries))
                {
                    //GEN CODECONTRACT(AMBER OR NOT)
                    string checkCode = string.Concat(DateTime.Now.ToString("yyyy"), "/SFG/SFI");
                    int countContractByToday = _contractInvester.CountDateContract(checkCode);
                    int countContractByTodayForAmber = 0;
                    if (!string.IsNullOrEmpty(model.CodeIntermediaries))
                        countContractByTodayForAmber = _contractInvester.CountDateContractForAmber(checkCode, model.CodeIntermediaries);
                    //HUNGVX-DOWN COUNT CONTRACT
                    string newCodeContract = string.Empty;
                    if (!string.IsNullOrEmpty(model.CodeIntermediaries))
                    {
                        newCodeContract = Utils.GenCodeContractForAmber(countContractByTodayForAmber, model.CodeIntermediaries);
                        //if(model.Id > 0) EDIT NO COUNT CONTRACT FOR AMBER
                        //    contractInvestorOld.Result[0].CodeContract = newCodeContract;
                    }
                    else
                    {
                        newCodeContract = Utils.GenCodeContract(countContractByToday - 2);
                    }
                }
                string firstNumber = contractInvestorOld.Result[0].CodeContract.Split('/')[0];
                string CodeTransferAgreement = string.Concat(firstNumber, "/", model.CodeIntermediaries, "/", countIntermediaries + 1, "/", DateTime.Now.ToString("ddMM/yyyy"), "/FSFG/SFI");
                string CodeManagementCatalog = string.Concat(firstNumber, "/", model.CodeIntermediaries, "/", countIntermediaries + 1, "/", DateTime.Now.ToString("ddMM/yyyy"), "/HĐQLDM/AFM/C3");
                string CodeSupplementAgreement = string.Concat(firstNumber, "/", model.CodeIntermediaries, "/", countIntermediaries + 1, "/", DateTime.Now.ToString("ddMM/yyyy"), "/FSFG/SFI");
                if (model.CodeIntermediaries != contractInvestorOld.Result[0].CodeIntermediaries)
                {
                    contractInvestorOld.Result[0].CodeTransferAgreement = CodeTransferAgreement;
                    contractInvestorOld.Result[0].CodeManagementCatalog = CodeManagementCatalog;
                    contractInvestorOld.Result[0].CodeSupplementAgreement = CodeSupplementAgreement;
                }
                if (contractInvestorOld.Result[0].IdStatusContract == "PayDone")
                {
                    var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "DepositDate", ErrorMessage = "Hợp đồng đã được duyệt thì không được chỉnh sửa" } };
                    var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                    //write trace log 
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = jsonerrs;
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, data = jsonerrs });
                }
                //HUNGVX-UPDATE INVESTOR
                if (model.IdInvestor > 0)
                {
                    var investor_old = _investor.Raw_Get(model.IdInvestor);
                    var investor = new Investor()
                    {
                        Id = model.IdInvestor,
                        Name = model.Name,
                        IdCard = model.IdCard,
                        CodeInvestor = investor_old.CodeInvestor,
                        DateOfIssuance = model.DateOfIssuance != null ? DateTime.ParseExact(model.DateOfIssuance, SiteConst.Format.DateFormat, null) : nullValue,
                        AddressIssuance = model.IsCMT == "0" ? model.AddressIssuance : model.AddressIssuanceCC,
                        Email = model.Email,
                        PhoneNumber = model.Phone,
                        Birthday = model.Birthday != null ? DateTime.ParseExact(model.Birthday, SiteConst.Format.DateFormat, null) : nullValue,
                        Status = 1,
                        Stock = (long)model.InvestmentAmount / (50000),
                        AccountBank = model.AccountBank,
                        Bank = model.Bank,
                        PersonalTaxCode = model.PersonalTaxCode,
                        Address = model.Address,
                        IsCMT = model.IsCMT == "0" ? true : false
                    };

                    try
                    {
                        investor_result = _investor.Raw_Update(investor);
                    }
                    catch (Exception ex)
                    {
                        //write trace log 
                        LogModel.Result = ActionResultValue.UpdateFailed;
                        LogModel.Message = "Cập nhật thông tin khách hàng không thành công";
                        Logger.LogError(ex, LogModel.ToString());

                        throw ex;
                    }
                }
                //HUNGVX-UPDATE DEPOSIT
                var deposit_result = new Deposit();
                if (model.DepositId > 0)
                {
                    var deposit_old = _deposit.Raw_Get(model.DepositId);
                    var deposit = new Deposit()
                    {
                        Id = deposit_old.Id,
                        CodeDeposit = model.CodeDeposit,
                        NameDeposit = "Deposit Money",
                        CodeInvestor = investor_result.CodeInvestor,
                        DepositAmount = model.DepositAmount != null ? model.DepositAmount.Value : 0M,
                        TeleSale = model.TeleSale,
                        SaleRep = model.SaleRep,
                        Status = 1,
                        DepositForm = model.DepositForm,
                        DepositDate = model.DepositDate != null ? DateTime.ParseExact(model.DepositDate, SiteConst.Format.DateFormat, null) : nullValue
                    };
                    try
                    {
                        if (model.DepositId > 0)
                            deposit_result = _deposit.Raw_Update(deposit);
                    }
                    catch (Exception ex)
                    {
                        //write trace log 
                        LogModel.Result = ActionResultValue.UpdateFailed;
                        LogModel.Message = "Cập nhật đặt cọc không thành công";
                        Logger.LogError(ex, LogModel.ToString());

                        throw ex;
                    }
                }
                else
                {
                    //HUNGVX-DEPOSIT
                    var deposit = new Deposit()
                    {
                        CodeDeposit = model.CodeDeposit,
                        NameDeposit = "Deposit Money",
                        CodeInvestor = investor_result.CodeInvestor,
                        DepositAmount = model.DepositAmount != null ? model.DepositAmount.Value : 0M,
                        TeleSale = model.TeleSale,
                        SaleRep = model.SaleRep,
                        Status = 1,
                        DepositForm = model.DepositForm,
                        DepositDate = model.DepositDate != null ? DateTime.ParseExact(model.DepositDate, SiteConst.Format.DateFormat, null) : nullValue
                    };
                    try
                    {
                        if (model.CodeDeposit != null) { }
                        //deposit_result = _deposit.Raw_Insert(deposit);
                    }
                    catch (Exception ex)
                    {
                        //write trace log 
                        LogModel.Result = ActionResultValue.UpdateFailed;
                        LogModel.Message = "Thêm đặt cọc không thành công";
                        Logger.LogError(ex, LogModel.ToString());

                        throw ex;
                    }
                }
                int stt = 1;
                string isStatusContract = string.Empty;

                
                //HUNGVX-CONTRACTINVESTOR
                var contractInvestor = new ContractInvestor();
                if (model.DepositAmount == model.InvestmentAmount)
                {
                    contractInvestor = new ContractInvestor()
                    {
                        Id = model.Id,
                        CodeContract = contractInvestorOld.Result[0].CodeContract,
                        NameContract = "Contract Investor",
                        CodeInvestor = investor_result.CodeInvestor,
                        CodeDeposit = deposit_result != null ? deposit_result.CodeDeposit : contractInvestorOld.Result[0].CodeDeposit,
                        InvestmentAmount = model.InvestmentAmount,
                        Sale = model.Sale,
                        TeleSale = model.TeleSale != null ? model.TeleSale : "",
                        SaleRepresent = model.SaleRep,
                        CreateDate = !string.IsNullOrEmpty(contractInvestorOld.Result[0].CreateDate) ? DateTime.ParseExact(contractInvestorOld.Result[0].CreateDate, SiteConst.Format.DateFormat, null) : DateTime.Now,
                        IdStatusContract = (countBill >= model.InvestmentAmount.Value) ? "WaitPayDone" : "",
                        PayOffDate = DateTime.ParseExact(model.DateBill, SiteConst.Format.DateFormat, null),
                        CodeIntermediaries = model.CodeIntermediaries != null ? model.CodeIntermediaries : null,
                        CodeManagementCatalog = model.CodeIntermediaries != null ? contractInvestorOld.Result[0].CodeManagementCatalog : null,
                        CodeSupplementAgreement = model.CodeIntermediaries != null ? contractInvestorOld.Result[0].CodeSupplementAgreement : null,
                        CodeTransferAgreement = model.CodeIntermediaries != null ? contractInvestorOld.Result[0].CodeTransferAgreement : null
                    };
                }
                else
                {
                    contractInvestor = new ContractInvestor()
                    {
                        Id = model.Id,
                        CodeContract = contractInvestorOld.Result[0].CodeContract,
                        NameContract = "Contract Investor",
                        CodeInvestor = investor_result.CodeInvestor,
                        CodeDeposit = deposit_result != null ? deposit_result.CodeDeposit : contractInvestorOld.Result[0].CodeDeposit,
                        InvestmentAmount = model.InvestmentAmount,
                        Sale = model.Sale,
                        TeleSale = model.TeleSale != null ? model.TeleSale : "",
                        SaleRepresent = model.SaleRep,
                        CreateDate = !string.IsNullOrEmpty(contractInvestorOld.Result[0].CreateDate) ? DateTime.ParseExact(contractInvestorOld.Result[0].CreateDate, SiteConst.Format.DateFormat, null) : DateTime.Now,
                        IdStatusContract = (countBill >= model.InvestmentAmount.Value) ? "WaitPayDone" : "", //lastStatus.IdStatusContract
                        PayOffDate = (countBill >= model.InvestmentAmount.Value) ? lastBillDate : nullValue,
                        CodeIntermediaries = model.CodeIntermediaries != null ? model.CodeIntermediaries : null,
                        CodeManagementCatalog = model.CodeIntermediaries != null ? contractInvestorOld.Result[0].CodeManagementCatalog : null,
                        CodeSupplementAgreement = model.CodeIntermediaries != null ? contractInvestorOld.Result[0].CodeSupplementAgreement : null,
                        CodeTransferAgreement = model.CodeIntermediaries != null ? contractInvestorOld.Result[0].CodeTransferAgreement : null

                    };
                }

                var contractInvestor_result = new ContractInvestor();
                try
                {
                    contractInvestor_result = _contractInvester.Raw_Update(contractInvestor);
                }
                catch (Exception ex)
                {
                    var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "CodeContract", ErrorMessage = "Lỗi hệ thống !" } };
                    var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);
                    //write trace log 
                    LogModel.Result = ActionResultValue.UpdateFailed;
                    LogModel.Message = "Cập nhật hợp đồng không thành công";
                    Logger.LogError(ex, LogModel.ToString());

                    throw;
                }
                ContractInvestorInstallments lastStatus = new ContractInvestorInstallments();
                //HUNGVX-STATUS --> CONTRACTINVESTORINSTALLMENTS

                var isDel = _contractInvestorInstallments.DeleteByContract(codeContractOld);
                if (model.ListInforBill != null)
                    foreach (var item in model.ListInforBill)
                    {
                        stt = stt + 1;
                        var statusInvestor = new ContractInvestorInstallments()
                        {
                            Id = item.IdBill,
                            IdStatusContract = (countBill >= model.InvestmentAmount.Value) ? "WaitPayDone" : "tt" + stt,
                            CreateDate = DateTime.ParseExact(item.DateBill, SiteConst.Format.DateFormat, null),
                            PaymentFormat = (byte)item.FormBill,
                            CodeContract = contractInvestorOld.Result[0].CodeContract,
                            PaymentAmount = item.BillMoney,
                            DepositCode = null
                        };
                        var statusInvestor_result = new ContractInvestorInstallments();
                        try
                        {
                            if (statusInvestor.Id > 0)
                                statusInvestor_result = _contractInvestorInstallments.Raw_Update(statusInvestor);
                            else
                                statusInvestor_result = _contractInvestorInstallments.Raw_Insert(statusInvestor);
                        }
                        catch (Exception ex)
                        {
                            var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "CodeContract", ErrorMessage = "Lỗi hệ thống !" } };
                            var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);
                            //write trace log 
                            LogModel.Result = ActionResultValue.UpdateFailed;
                            LogModel.Message = "Cập nhật đặt cọc không thành công";
                            Logger.LogError(ex, LogModel.ToString());

                            throw;
                        }

                    }
                lastStatus = _contractInvestorInstallments.GetLastStatus(model.CodeContract);
                if (lastStatus == null)
                {
                    lastStatus = new ContractInvestorInstallments();
                    lastStatus.IdStatusContract = "";
                }
                //HUNGVX-UPDATE CODECONTRACT
                _contractInvestorInstallments.UpdateCodeContract(deposit_result.CodeDeposit, contractInvestor_result.CodeContract);

                //write trace log 
                LogModel.Result = ActionResultValue.UpdateSuccess;
                LogModel.Message = "Cập nhật hợp đồng thành công";
                Logger.LogInformation(LogModel.ToString());
            }
            return Json(new { status = true });
        }
        #endregion

        #region HungVX Validate
        private List<ErrorResult> validform(ContractInvesterViewModel entity)
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

            //HUNGVX-NGÀY CẤP KHÔNG LỚN HƠN NGÀY HIỆN TẠI
            if (!string.IsNullOrEmpty(entity.Birthday))
                if (DateTime.ParseExact(entity.Birthday, SiteConst.Format.DateFormat, null).Date >= DateTime.Now.Date) dictErrors["birthday"] = new ErrorResult() { ErrorMessage = "Ngày sinh nhật không lớn hơn ngày hiện tại.", Field = "birthday" };
            //HUNGVX-EMAIL KHÔNG ĐƯỢC TRỐNG
            //if (string.IsNullOrWhiteSpace(entity.Email)) dictErrors["Email"] = new ErrorResult() { ErrorMessage = "Email không để trống.", Field = "Email" };
            //else if (!IsValidEmail(entity.Email)) dictErrors["Email"] = new ErrorResult() { ErrorMessage = "Email không hợp lệ !", Field = "Email" };

            //thẩm định trùng email và trùng account, trùng cmnd, trùng password
            //if (string.IsNullOrWhiteSpace(entity.AccountName)) dictErrors["AccountName"] = new ErrorResult() { ErrorMessage = "Tên tài khoản không để trống.", Field = "AccountName" };
            //else if (entity.AccountName.Length < 6) dictErrors["AccountName"] = new ErrorResult() { ErrorMessage = "Tên Tài khoản đăng ký phải từ 6 kí tự trở lên.", Field = "AccountName" };
            //string regX = "^[a-zA-Z0-9]+$";
            //if ( !Regex.IsMatch( entity.AccountName, regX))
            //{
            //    dictErrors["AccountName"] = new ErrorResult() { ErrorMessage = "Tên Tài khoản không đúng định dạng!", Field = "AccountName" };
            //}

            //thẩm định số điện thoại Việt Nam
            var cellnumber = entity.Phone ?? "";
            if (cellnumber.Length == 10)
            {
                Regex regex = new Regex(@"(03|05|07|09|08)+([0-9]{8})\b");
                Match match = regex.Match(cellnumber);
                if (!match.Success)
                {
                    dictErrors["phone"] = new ErrorResult() { ErrorMessage = "Số điện thoại không hợp lệ.", Field = "phone" };
                }
            }
            else dictErrors["phone"] = new ErrorResult() { ErrorMessage = "Số điện thoại không hợp lệ.", Field = "phone" };

            return dictErrors.Values.GroupBy(x => x.ErrorMessage).Select(y => y.First()).ToList();
        }
        #endregion

        #region HungVX Check Exists Email
        private bool IsValidEmail(string emailaddress)
        {
            //UrlHelper uHelp = new UrlHelper(this.ControllerContext.RequestContext);
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        #endregion

        #region HungVX Check Phone Investor
        [HttpPost]
        public IActionResult CheckPhoneInvestor(string phone)
        {
            //trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Data = (new { phone = phone }).ToDataString();

            if (string.IsNullOrEmpty(phone))
            {
                //write trace log
                LogModel.Result = ActionResultValue.NotFoundData;
                LogModel.Message = "Số điện thoại không được trống";
                Logger.LogInformation(LogModel.ToString());

                return Ok(new { status = false, mess = "Số điện thoại không được trống" });
            }
            else
            {
                var data = _investor.GetByPhone(phone.Trim()) ?? new InvestorViewModel();
                data.BirthdayString = data.Birthday != null ? data.Birthday.Value.ToString(SiteConst.Format.DateFormat) : "";
                data.DateOfIssuanceString = data.DateOfIssuance != null ? data.DateOfIssuance.Value.ToString(SiteConst.Format.DateFormat) : "";
                var resultAmountDeposit = _investor.CheckSumAmountDeposit(phone) ?? new InvestorViewModel();
                data.SumPaymentAmount = resultAmountDeposit.SumPaymentAmount;
                data.CodeDeposit = resultAmountDeposit.CodeDeposit;

                //write trace log
                LogModel.Result = ActionResultValue.GetInfoSuccess;
                LogModel.Data = data.ToDataString();
                Logger.LogInformation(LogModel.ToString());

                return Ok(new { status = true, data });
            }
        }
        #endregion
        #region HungVX Check Phone Ember
        [HttpPost]
        public IActionResult CheckPhoneEmber(string phone)
        {
            //trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Data = (new { phone = phone }).ToDataString();

            if (string.IsNullOrEmpty(phone))
            {
                //write trace log
                LogModel.Result = ActionResultValue.NotFoundData;
                LogModel.Message = "Số điện thoại không được trống";
                Logger.LogInformation(LogModel.ToString());

                return Ok(new { status = false, mess = "Số điện thoại không được trống" });
            }
            else
            {
                var data = _intermediaries.CheckByPhone(phone.Trim()).DataItem ?? new IntermediariesViewModel();

                //write trace log
                LogModel.Result = ActionResultValue.GetInfoSuccess;
                LogModel.Data = data.ToDataString();
                Logger.LogInformation(LogModel.ToString());

                return Ok(new { status = true, data });
            }
        }
        #endregion
        #region HungVX Check Phone Ember
        [HttpPost]
        public IActionResult CheckTaxCodeEmber(string taxCode)
        {
            //trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Data = (new { taxCode = taxCode }).ToDataString();

            if (string.IsNullOrEmpty(taxCode))
            {
                //write trace log
                LogModel.Result = ActionResultValue.NotFoundData;
                LogModel.Message = "Mã số thuế không được trống";
                Logger.LogInformation(LogModel.ToString());

                return Ok(new { status = false, mess = "Mã số thuế không được trống" });
            }
            else
            {
                var data = _intermediaries.CheckByTaxCode(taxCode.Trim()).DataItem ?? new IntermediariesViewModel();

                //write trace log
                LogModel.Result = ActionResultValue.GetInfoSuccess;
                LogModel.Data = data.ToDataString();
                Logger.LogInformation(LogModel.ToString());

                return Ok(new { status = true, data });
            }
        }
        #endregion
        //#region Print - Hungvx
        //[HttpPost]
        //public IActionResult Print(int id)
        //{
        //    return RedirectToRoute("PrintContract/Print", id);
        //var model = _contractInvester.GetInfoById(id);
        //foreach (var item in model.Result)
        //{
        //    item.ListInforBill = _contractInvester.GetListBill(id).Result.ToList<InforBill>();
        //}
        //DateTime? nullValue = null;
        //var modelPrint = new ContractInvestorPrintViewModel();
        ////khanhkk added
        //if (!string.IsNullOrEmpty(model.Result[0].InvestorSignature))
        //{
        //    modelPrint.InvestorSignature = Path.Combine("https://", HttpContext.Request.Host.Value, "Uploads", "Signatures", model.Result[0].InvestorSignature);
        //}
        //modelPrint.Name = model.Result[0].Name;
        //modelPrint.IdStatusContract = model.Result[0].IdStatusContract;
        ////khanhkk added
        //modelPrint.CodeContract = model.Result[0].CodeContract;
        //modelPrint.IdCard = model.Result[0].IdCard;
        //modelPrint.DateOfIssuance = model.Result[0].DateOfIssuance;
        //modelPrint.DateOfIssuancePrint = model.Result[0].DateOfIssuance != null ? DateTime.ParseExact(model.Result[0].DateOfIssuance, "dd/MM/yyyy", null).ToString("MM/dd/yyyy") : "";
        //modelPrint.AddressIssuance = model.Result[0].AddressIssuance;
        //modelPrint.AddressIssuancePrint = model.Result[0].AddressIssuance != null ? Utils.convertToUnSign(model.Result[0].AddressIssuance) : "";
        //modelPrint.Address = model.Result[0].Address;
        //modelPrint.AddressPrint = model.Result[0].Address != null ? Utils.convertToUnSign(model.Result[0].Address) : "";
        //modelPrint.PersonalTaxCode = model.Result[0].PersonalTaxCode;
        //modelPrint.Phone = model.Result[0].Phone;
        //modelPrint.AccountBank = model.Result[0].AccountBank;
        //modelPrint.Bank = model.Result[0].Bank;
        //modelPrint.BankPrint = model.Result[0].Bank != null ? Utils.convertToUnSign(model.Result[0].Bank) : "";
        //modelPrint.InvestmentAmount = model.Result[0].InvestmentAmount;
        //modelPrint.InvestmentAmountPrint = model.Result[0].InvestmentAmount != null ? Utils.MoneyToText(model.Result[0].InvestmentAmount.ToString()) : "";
        //modelPrint.InvestmentAmountPrintEN = model.Result[0].InvestmentAmount != null ? Utils.ConvertCurrencyToText((long)model.Result[0].InvestmentAmount) : "";
        //modelPrint.CountShare = (int)(modelPrint.InvestmentAmount / (50000));
        //modelPrint.CountShareVN = modelPrint.CountShare > 0 ? Utils.MoneyToText(modelPrint.CountShare.ToString()) : "";
        //modelPrint.CountShareEN = modelPrint.CountShare > 0 ? Utils.ConvertCurrencyToText(modelPrint.CountShare) : "";
        ////% CỌC 
        //modelPrint.DepositAmount = model.Result[0].DepositAmount;
        //modelPrint.PercenDeposit = Math.Round((modelPrint.DepositAmount > 0) ? (modelPrint.DepositAmount.Value / modelPrint.InvestmentAmount.Value) : 0, 2);
        ////SỐ TIỀN CÒN LẠI
        //try
        //{
        //    modelPrint.RemainAmount = (model.Result[0].ListInforBill != null) ? modelPrint.InvestmentAmount.Value - (modelPrint.DepositAmount.Value) : modelPrint.InvestmentAmount.Value - (modelPrint.DepositAmount.Value);

        //}
        //catch (Exception ex)
        //{

        //    throw;
        //}
        //modelPrint.RemainAmountVN = (modelPrint.RemainAmount > 0) ? Utils.MoneyToText(modelPrint.RemainAmount.ToString()) : "";
        //modelPrint.RemainAmountEN = (modelPrint.RemainAmount > 0) ? Utils.ConvertCurrencyToText((long)modelPrint.RemainAmount) : "";
        //modelPrint.CreatedDateValue = DateTime.ParseExact(model.Result[0].CreateDate, "dd/MM/yyyy", null);
        ////THỜI GIAN THANH TOÁN ĐỢT 2
        //if (model.Result[0].ListInforBill != null && model.Result[0].ListInforBill.Count == 1)
        //{
        //    modelPrint.BillTwo = (model.Result[0].ListInforBill[0].DateBill != null) ? DateTime.ParseExact(model.Result[0].ListInforBill[0].DateBill, "dd/MM/yyyy", null) : nullValue;
        //    modelPrint.day = modelPrint.BillTwo != null && model.Result[0].CreateDate != null ? 
        //        modelPrint.CreatedDateValue.Subtract(modelPrint.BillTwo.Value).Days : 0;
        //}
        //else
        //{
        //    modelPrint.BillTwo = nullValue;
        //    modelPrint.day = 0;
        //}
        ////THỜI GIAN DUYỆT HĐ
        //modelPrint.DatePaydone = model.Result[0].DatePaydone ?? nullValue;
        //string html = await this.RenderViewAsync("Print", modelPrint);
        //var data = new
        //{
        //    datahtml = html
        //};
        //return Json(data);
        //}
        //#endregion

        #region HungVX - Check AmountDeposit
        public IActionResult CheckAmountDeposit(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return Ok(new { status = false, data = 0 });
            }

            return Ok();
        }
        #endregion
    }
}
