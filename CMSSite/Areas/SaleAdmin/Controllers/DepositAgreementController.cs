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
using System.Globalization;
using System.Linq;
using CRMSite.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using CRMModel.Models.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using CRMBussiness;
using Microsoft.Extensions.Logging;

namespace CRMSite.Areas.SaleAdmin.Controllers
{
    [Area("SaleAdmin")]
    [Authorize]
    public class DepositAgreementController : BaseController
    {
        public readonly IInvestor _investor;
        public readonly IDeposit _deposit;
        public readonly IStatusContractInvestors _statusContractInvestors;
        public readonly IContractInvester _contractInvester;
        public readonly IContractInvestorInstallments _contractInvestorInstallments;
        public DepositAgreementController(IHttpContextAccessor httpContextAccessor, IInvestor investor, IStatusContractInvestors statusContractInvestors, IContractInvester contractInvester, IDeposit deposit, IContractInvestorInstallments contractInvestorInstallments, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            _investor = investor;
            _deposit = deposit;
            _statusContractInvestors = statusContractInvestors;
            _contractInvester = contractInvester;
            _contractInvestorInstallments = contractInvestorInstallments;
            LogModel.ItemName = "deposit(s)";
        }
        public IActionResult Index()
        {
            //write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult getmv()
        {
            return Json(new
            {
                Status = 200,
                ModelView = new DepositAgreementViewModel()
            });
        }
        #region GetList - hungvx
        [HttpPost]
        public IActionResult GetList(SearchDepositAAgreementViewModel model)
        {
            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            var data = _deposit.GetList(model, out total);
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

        #region Edit - Hungvx
        public IActionResult Edit(int id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();

            var data = _deposit.GetInfoById(id);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            foreach (var item in data.Result)
            {
                item.ListInforDepositBill = _deposit.GetListBill(id).Result.ToList<InforDepositBill>();
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
            LogModel.Data = data.Result.First().ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return PartialView("Edit", data.Result.First());
        }
        #endregion

        #region HungVX Check Email Investor
        [HttpPost]
        public IActionResult CheckPhoneInvestor(string phone)
        {
            //trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Data = (new { phone = phone }).ToDataString();

            if (string.IsNullOrEmpty(phone))
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Số điện thoại không được trống";
                Logger.LogWarning(LogModel.ToString());

                return Ok(new { status = false, mess = "Số điện thoại không được trống" });
            }
            else
            {
                var data = _investor.GetByPhone(phone.Trim()) ?? new InvestorViewModel();
                data.BirthdayString = data.Birthday != null ? data.Birthday.Value.ToString("dd/MM/yyyy") : "";
                data.DateOfIssuanceString = data.DateOfIssuance != null ? data.DateOfIssuance.Value.ToString("dd/MM/yyyy") : "";

                //write trace log 
                LogModel.Result = ActionResultValue.GetInfoSuccess;
                LogModel.Data = data.ToDataString();
                Logger.LogInformation(LogModel.ToString());

                return Ok(new { status = true, data });
            }
        }
        #endregion

        #region HungVX Check Email Investor
        [HttpPost]
        public IActionResult CheckContract(string CodeContract)
        {
            //trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Data = (new { CodeContract = CodeContract }).ToDataString();

            if (string.IsNullOrEmpty(CodeContract))
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Mã HĐ không được trống";
                Logger.LogWarning(LogModel.ToString());

                return Ok(new { status = false, mess = "Mã HĐ không được trống" });
            }
            else
            {
                var data = _contractInvester.GetInfoByCodeContract(CodeContract.Trim()) ?? new ContractInvesterViewModel();

                //write trace log 
                LogModel.Result = ActionResultValue.GetInfoSuccess;
                LogModel.Data = data.ToDataString();
                Logger.LogInformation(LogModel.ToString());

                return Ok(new { status = true, data });
            }
        }
        #endregion

        #region Hungvx InsertOrUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertOrUpdate(DepositAgreementViewModel model)
        {
            //trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = model.Id == 0 ? ActionType.Create : ActionType.Update;

            DateTime? nullValue = null;
            //GIƠI HẠN ĐẶT CỌC
            if (model.ListInforDepositBill == null)
            {
                var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "BillMoney", ErrorMessage = "Chỉ được thanh toán 1 lần, cọc 1 lần" } };
                var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = jsonerrs;
                Logger.LogWarning(LogModel.ToString());

                return Json(new { status = false, data = jsonerrs });
            }
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
            if (model.ListInforDepositBill != null)
                foreach (var item in model.ListInforDepositBill)
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
            //if (model.Id <= 0)
            //{
            //    var checkContract = _contractInvester.GetByCode(model.CodeContract);
            //    if (checkContract != null)
            //    {
            //        var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "CodeContract", ErrorMessage = "Mã HĐ Đã tồn tại" } };
            //        var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);
            //        return Json(new { status = false, data = jsonerrs });
            //    }
            //}
            // Check Money
            if (!string.IsNullOrEmpty(model.ContractCode))
            {
                decimal invesmentAmount = _contractInvester.GetInfoByCodeContract(model.ContractCode).InvestmentAmount.Value;
                decimal countBill = 0M;
                if (model.ListInforDepositBill != null)
                    foreach (var item in model.ListInforDepositBill)
                    {
                        countBill += item.BillMoney;
                    }
                if (countBill > invesmentAmount)
                {
                    var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "InvestmentAmount", ErrorMessage = "Đặt cọc vượt quá GT HĐ" } };
                    var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                    //write trace log 
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = jsonerrs;
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { status = false, data = jsonerrs });
                }
            }
            // Check DateTime Deposit, DateTime 
            if (model.ListInforDepositBill != null)
            {
                int sumBill = model.ListInforDepositBill.Count;
                DateTime firstTime = DateTime.ParseExact(model.ListInforDepositBill.First().DateBill, "dd/MM/yyyy", null);
                DateTime? valiDate = null;
                foreach (var item in model.ListInforDepositBill)
                {
                    if (valiDate != null)
                    {
                        if (DateTime.ParseExact(item.DateBill, "dd/MM/yyyy", null).Date > valiDate.Value.Date)
                        {
                            var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "DateBill", ErrorMessage = "Ngày thanh toán sau phải hơn hoặc bằng lần trước" } };
                            var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                            //write trace log 
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = jsonerrs;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new { status = false, data = jsonerrs });
                        }
                        valiDate = DateTime.ParseExact(item.DateBill, "dd/MM/yyyy", null);
                    }
                }
                lastBillDate = DateTime.ParseExact(model.ListInforDepositBill[sumBill - 1].DateBill, "dd/MM/yyyy", null);
            }
            //Investor
            var investor_result = new Investor();
            if (model.IdInvestor <= 0)
            {
                //Insert new Investor
                var investor = new Investor()
                {
                    Name = model.Name,
                    IdCard = model.IdCard,
                    DateOfIssuance = model.DateOfIssuance != null ? DateTime.ParseExact(model.DateOfIssuance, "dd/MM/yyyy", null) : nullValue,
                    CodeInvestor = Guid.NewGuid().ToString(),
                    AddressIssuance = model.AddressIssuance,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    Birthday = model.Birthday != null ? DateTime.ParseExact(model.Birthday, "dd/MM/yyyy", null) : nullValue,
                    Status = 1,
                    AccountBank = model.AccountBank,
                    Bank = model.Bank,
                    Address = model.Address
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
                    DateOfIssuance = model.DateOfIssuance != null ? DateTime.ParseExact(model.DateOfIssuance, "dd/MM/yyyy", null) : nullValue,
                    AddressIssuance = model.AddressIssuance,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    Birthday = model.Birthday != null ? DateTime.ParseExact(model.Birthday, "dd/MM/yyyy", null) : nullValue,
                    Status = 1,
                    Stock = investor_old.Stock,
                    AccountBank = model.AccountBank,
                    Bank = model.Bank,
                    PersonalTaxCode = investor_old.PersonalTaxCode,
                    Address = model.Address
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
            //Deposit
            if (model.Id <= 0)
            {
                //GEN CodeDeposit
                string genCodeDeposit = string.Concat("DP/", DateTime.Now.ToString("ddMM/yyyy-hh/mm/ss"));
                //Deposit
                var deposit = new Deposit()
                {
                    CodeDeposit = genCodeDeposit,
                    NameDeposit = "Deposit Money",
                    CodeInvestor = investor_result.CodeInvestor,
                    DepositAmount = 0M,
                    Status = 1,
                    DepositDate = nullValue,
                    Description = model.Description
                };
                var deposit_result = new Deposit();
                try
                {
                    deposit_result = _deposit.Raw_Insert(deposit);
                }
                catch (Exception ex)
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.CreateFailed;
                    LogModel.Message = "Cập nhật thông tin đặt cọc không thành công";
                    Logger.LogError(ex, LogModel.ToString());

                    throw ex;
                }
                //Update ContractInvestor
                if (!string.IsNullOrEmpty(model.ContractCode))
                {
                    var checkContractInvestor = _contractInvester.GetInfoByCodeContract(model.ContractCode);
                    if (checkContractInvestor == null)
                    {
                        var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "ContratCode", ErrorMessage = "HĐ không tồn tại" } };
                        var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                        //write trace log 
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = jsonerrs;
                        Logger.LogWarning(LogModel.ToString());

                        return Json(new { status = false, data = jsonerrs });
                    }
                    _contractInvester.UpdateDepositCode(checkContractInvestor.Id, depositCode: deposit_result.CodeDeposit);
                }
                int stt = 1;
                string isStatusContract = string.Empty;

                ContractInvestorInstallments lastStatus = new ContractInvestorInstallments();
                //Status --> ContractInvestorInstallments
                if (model.ListInforDepositBill != null)
                    foreach (var item in model.ListInforDepositBill)
                    {
                        stt = stt + 1;
                        var statusInvestor = new ContractInvestorInstallments()
                        {
                            IdStatusContract = "",
                            CreateDate = DateTime.ParseExact(item.DateBill, "dd/MM/yyyy", null),
                            CodeContract = model.ContractCode != null ? model.ContractCode : null,
                            PaymentAmount = item.BillMoney,
                            DepositCode = deposit_result.CodeDeposit,
                            Description = item.DescriptionBill
                        };
                        var statusInvestor_result = new ContractInvestorInstallments();
                        try
                        {
                            statusInvestor_result = _contractInvestorInstallments.Raw_Insert(statusInvestor);
                        }
                        catch (Exception ex)
                        {
                            //write trace log 
                            LogModel.Result = ActionResultValue.CreateFailed;
                            LogModel.Message = "Tạo mới thông tin đặt cọc không thành công";
                            Logger.LogError(ex, LogModel.ToString());

                            throw;
                        }

                    }

                //write trace log 
                LogModel.Result = ActionResultValue.CreateSuccess;
                LogModel.Message = "Tạo mới thông tin đặt cọc thành công";
                Logger.LogInformation(LogModel.ToString());

            }
            else
            {
                //Update Investor
                if (model.IdInvestor > 0)
                {
                    
                }
                //Update Deposit
                var deposit_result = new Deposit();

                var deposit_old = _deposit.Raw_Get(model.Id);
                var deposit = new Deposit()
                {
                    Id = deposit_old.Id,
                    CodeDeposit = deposit_old.CodeDeposit,
                    NameDeposit = "Deposit Money",
                    CodeInvestor = investor_result.CodeInvestor,
                    DepositAmount = deposit_old.DepositAmount,
                    Status = 1,
                    DepositDate = nullValue,
                    Description = model.Description
                };
                try
                {
                    if (model.Id > 0)
                        deposit_result = _deposit.Raw_Update(deposit);
                }
                catch (Exception ex)
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.UpdateFailed;
                    LogModel.Message = "Cập nhật thông tin đặt cọc không thành công";
                    Logger.LogError(ex, LogModel.ToString());

                    throw ex;
                }
                //Update ContractInvestor
                if (!string.IsNullOrEmpty(model.ContractCode))
                {
                    var checkContractInvestor = _contractInvester.GetInfoByCodeContract(model.ContractCode);
                    if (checkContractInvestor == null)
                    {
                        var lstErr = new List<ErrorResult>() { new ErrorResult() { Field = "ContratCode", ErrorMessage = "HĐ không tồn tại" } };
                        var jsonerrs = JsonConvert.SerializeObject(lstErr, Formatting.Indented);

                        //write trace log 
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = jsonerrs;
                        Logger.LogWarning(LogModel.ToString());

                        return Json(new { status = false, data = jsonerrs });
                    }
                    _contractInvester.UpdateDepositCode(checkContractInvestor.Id, depositCode: deposit_result.CodeDeposit);
                }
                int stt = 1;
                string isStatusContract = string.Empty;

                ContractInvestorInstallments lastStatus = new ContractInvestorInstallments();
                //Status --> ContractInvestorInstallments
                var isDel = _contractInvestorInstallments.DeleteByCodeDeposit(deposit_result.CodeDeposit);
                if (model.ListInforDepositBill != null)
                    foreach (var item in model.ListInforDepositBill)
                    {
                        stt = stt + 1;
                        var statusInvestor = new ContractInvestorInstallments()
                        {
                            Id = item.IdBill,
                            IdStatusContract = "",
                            CreateDate = DateTime.ParseExact(item.DateBill, "dd/MM/yyyy", null),
                            CodeContract = model.ContractCode != null ? model.ContractCode : null,
                            DepositCode = deposit_result.CodeDeposit,
                            PaymentAmount = item.BillMoney,
                            Description = item.DescriptionBill
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
                            //write trace log 
                            LogModel.Result = ActionResultValue.UpdateFailed;
                            LogModel.Message = "Cập nhật thông tin đặt cọc không thành công";
                            Logger.LogError(ex, LogModel.ToString());

                            throw;
                        }

                    }

                //write trace log 
                LogModel.Result = ActionResultValue.UpdateSuccess;
                LogModel.Message = "Cập nhật thông tin đặt cọc không thành công";
                Logger.LogInformation(LogModel.ToString());
            }
            return Json(new { status = true });
        }
        #endregion

        #region HungVX Validate
        private List<ErrorResult> validform(DepositAgreementViewModel entity)
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

            //NGÀY CẤP KHÔNG LỚN HƠN NGÀY HIỆN TẠI
            if (!string.IsNullOrEmpty(entity.Birthday))
                if (DateTime.ParseExact(entity.Birthday, "dd/MM/yyyy", null).Date >= DateTime.Now.Date) dictErrors["birthday"] = new ErrorResult() { ErrorMessage = "Ngày sinh nhật không lớn hơn ngày hiện tại.", Field = "birthday" };
            //EMAIL KHÔNG ĐƯỢC TRỐNG
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
    }
}
