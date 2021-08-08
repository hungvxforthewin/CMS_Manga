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
using CRMModel.Models.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using CRMBussiness;
using Microsoft.Extensions.Logging;
using CRMSite.Models;

namespace CRMSite.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    [Authorize]
    public class ConfirmPaymentController : BaseController
    {
        private readonly IContractInvestorInstallments _contractInvestorInstallments;
        private readonly IContractInvester _contractInvester;
        private IFileUpload _iFile;
        private IWebHostEnvironment _env;
        private string _saveFileFolder;
        public readonly IInvestor _investor;

        public ConfirmPaymentController(IHttpContextAccessor httpContextAccessor, IContractInvestorInstallments contractInvestorInstallments, IContractInvester contractInvester, IInvestor investor, IWebHostEnvironment env, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            _contractInvestorInstallments = contractInvestorInstallments;
            _contractInvester = contractInvester;
            _env = env;
            _iFile = new FileUploadImp();
            _saveFileFolder = _env.WebRootPath + "\\Uploads\\Files";
            _investor = investor;
            LogModel.ItemName = "investor contract(s)";
        }

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
        public IActionResult GetSale()
        {
            //trace log
            LogModel.ItemName = "sale(s)";
            LogModel.Action = ActionType.GetInfo;

            IAccount iAcc = new AccountImp();
            var getResult = iAcc.GetEmployeeListByType(false);
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

        public IActionResult Index()
        {
            //write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return View();
        }

        #region ViewTotal - Hungvx
        public IActionResult View(int id)
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
            Logger.LogInformation(LogModel.ToString());

            return PartialView("View", data.Result.First());
        }
        #endregion

        #region GetList - hungvx
        [HttpPost]
        public IActionResult GetList(SearchContractInvestorInstallmentsViewModel model)
        {
            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            var data = _contractInvester.GetListByWaitPayDone(model, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;
            
            //if (data.Error)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { "Lỗi hệ thống" } });
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
                    if (item.TeleSaleName == "")
                    {
                        item.TeleSaleName = "Không xác định";
                    }
                }
            DataResult<DisplayContractInvesterTableViewModel> newData = new DataResult<DisplayContractInvesterTableViewModel>();
            newData = data;
            //CODECONTRACT TRANSFER UPDATED
            //foreach (var item in data.Result.ToList())
            //{
            //    if(item.CodeIntermediaries != null)
            //    {
            //        var data1 = new DisplayContractInvesterTableViewModel()
            //        {
            //            Id = item.Id,
            //            ContractCode = item.CodeManagementCatalog,
            //            CodeIntermediaries = item.CodeIntermediaries,
            //            CMT = item.CMT,
            //            IdStatusContract = item.IdStatusContract,
            //            InvestmentAmount = item.InvestmentAmount,
            //            NameStatus = item.NameStatus,
            //            CodeTransferAgreement = item.CodeTransferAgreement,
            //            CodeManagementCatalog = item.CodeManagementCatalog,
            //            CodeSupplementAgreement = item.CodeSupplementAgreement,
            //            NameInvestor = item.NameInvestor,
            //            Phone = item.Phone,
            //            SaleName = item.SaleName,
            //            Status = item.Status,
            //            Stock = item.Stock,
            //            TeleSaleName = item.TeleSaleName
            //        };
            //        var data2 = new DisplayContractInvesterTableViewModel()
            //        {
            //            Id = item.Id,
            //            ContractCode = item.CodeSupplementAgreement,
            //            CodeIntermediaries = item.CodeIntermediaries,
            //            CMT = item.CMT,
            //            IdStatusContract = item.IdStatusContract,
            //            InvestmentAmount = item.InvestmentAmount,
            //            NameStatus = item.NameStatus,
            //            CodeTransferAgreement = item.CodeTransferAgreement,
            //            CodeManagementCatalog = item.CodeManagementCatalog,
            //            CodeSupplementAgreement = item.CodeSupplementAgreement,
            //            NameInvestor = item.NameInvestor,
            //            Phone = item.Phone,
            //            SaleName = item.SaleName,
            //            Status = item.Status,
            //            Stock = item.Stock,
            //            TeleSaleName = item.TeleSaleName
            //        };
            //        newData.Result.Add(data1);
            //        newData.Result.Add(data2);
            //    }
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

        #region Payment info - Hungvx
        public IActionResult GetInfoPay(int id)
        {
            //trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.ItemName = "contract payment(s)";
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

            var resultData = new PaymentViewModel();
            resultData.Name = data.Result[0].Name;
            resultData.Phone = data.Result[0].Phone;
            resultData.CMT = data.Result[0].IdCard;
            resultData.DatePaydone = data.Result[0].DatePaydone;
            resultData.DatePaydoneString = resultData.DatePaydone != null ? resultData.DatePaydone?.ToString("dd-MM-yyyy") : "";
            resultData.AmountContract = data.Result[0].InvestmentAmount.Value;
            resultData.BillDeposit = data.Result[0].DepositAmount != null ? data.Result[0].DepositAmount.Value : 0M;
            resultData.CodeTransferAgreement =  !string.IsNullOrEmpty(data.Result[0].CodeTransferAgreement) ?  string.Concat(data.Result[0].CodeTransferAgreement.Split('/')[0], "/", data.Result[0].CodeTransferAgreement.Split('/')[1], "/", data.Result[0].CodeTransferAgreement.Split('/')[2]) : "";
            resultData.CodeManagementCatalog =  !string.IsNullOrEmpty(data.Result[0].CodeManagementCatalog) ? string.Concat(data.Result[0].CodeManagementCatalog.Split('/')[0], "/", data.Result[0].CodeManagementCatalog.Split('/')[1]) : "";
            resultData.CodeSupplementAgreement =  !string.IsNullOrEmpty(data.Result[0].CodeSupplementAgreement) ? string.Concat(data.Result[0].CodeSupplementAgreement.Split('/')[0], "/", data.Result[0].CodeSupplementAgreement.Split('/')[1], "/", data.Result[0].CodeSupplementAgreement.Split('/')[2]) : "";
            foreach (var item in data.Result[0].ListInforBill)
            {
                resultData.ListBills.Add(new ListBill() { BillMoney = item.BillMoney });
            }
            resultData.RemainAmount = resultData.AmountContract - (resultData.BillDeposit + (resultData.ListBills.Sum(x => x.BillMoney)));

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = resultData.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = resultData });
        }
        #endregion

        #region Update PayDone - Hungvx
        public IActionResult UpdatePayDone(int id, string payDoneDate, string createDateContract, string codeManagementCatalog, string codeSupplementAgreement, string codeTransferAgreement, string AccountBank2)
        {
            // trace log
            LogModel.Data = (new { id = id, PayDoneDate = payDoneDate }).ToDataString();
            LogModel.Action = ActionType.Update;

            if (id <= 0)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "HĐ không tồn tại";
                Logger.LogWarning(LogModel.ToString());

                return Ok(new { status = false, mess = "HĐ không tồn tại" });
            }

            var data = _contractInvester.GetInfoById(id);
            //HUNGVX-CONTRACT AMBER ALLOW UPDATE
            if (data.Result[0].IdStatusContract == "PayDone" && data.Result[0].CodeIntermediaries == null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "HĐ đã được duyệt";
                Logger.LogWarning(LogModel.ToString());

                return Ok(new { status = false, mess = "HĐ đã được duyệt" });
            }

            #region Khanh added
            DateTime? approvedDate = null;
            if (!string.IsNullOrEmpty(payDoneDate))
            {
                payDoneDate = payDoneDate.Replace(SiteConst.SubstractChar, SiteConst.SlashChar);
                DateTime approvedDateValue;
                bool isValidDate = DateTime.TryParseExact(payDoneDate, SiteConst.Format.DateFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out approvedDateValue);
                if (!isValidDate)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Ngày duyệt hơp đồng phải có định dạng ngày tháng năm";
                    Logger.LogWarning(LogModel.ToString());

                    return Ok(new { status = false, mess = "Ngày duyệt hơp đồng phải có định dạng ngày tháng năm" });
                }
                approvedDate = approvedDateValue;
            }
            #endregion
            //HUNGVX-CHECK EXISTS CONTRACT
            //if (!string.IsNullOrEmpty(codeTransferAgreement) && data.Result[0].CodeIntermediaries != null)
            //{
            //    var result = _contractInvester.GetInfoByCodeContract(string.Concat(codeTransferAgreement.Trim(), "/FSFG/SFI"));
            //    if(result != null)
            //    {
            //        return Ok(new { status = false, mess = "HĐ đã tồn tại" });
            //    }
            //}
            //else if(string.IsNullOrEmpty(codeTransferAgreement) && data.Result[0].CodeIntermediaries != null)
            //{
            //    return Ok(new { status = false, mess = "HĐ chuyển nhượng không được trống" });
            //}
            //HUNGVX-UPDATE CODECONTRACT
            var contractInvestorOld = _contractInvester.Raw_Get(id);
            var codeContractOld = contractInvestorOld.CodeContract;
            codeManagementCatalog = !string.IsNullOrEmpty(codeManagementCatalog) ? string.Concat(codeManagementCatalog.Trim(), "/HÐQLDM-AFM") : "";
            codeTransferAgreement = !string.IsNullOrEmpty(codeTransferAgreement) ? string.Concat(codeTransferAgreement.Trim(), "/FSFG/SFI") : "";
            codeSupplementAgreement = !string.IsNullOrEmpty(codeSupplementAgreement) ? string.Concat(codeSupplementAgreement.Trim(), "/FSFG/SFI") : "";

            contractInvestorOld.CodeManagementCatalog = codeManagementCatalog;
            contractInvestorOld.CodeTransferAgreement = codeTransferAgreement;
            contractInvestorOld.CodeSupplementAgreement = codeSupplementAgreement;
            if (codeContractOld == codeTransferAgreement)
            {
                try
                {
                    _contractInvester.Raw_Update(contractInvestorOld);
                }
                catch (Exception ex)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.UpdateFailed;
                    LogModel.Message = "Duyệt HĐ không thành công!";
                    Logger.LogError(LogModel.ToString());

                    return Ok(new { status = false, mess = "Duyệt HĐ không thành công" });
                }
            }
            else
            {
                var result = _contractInvester.GetInfoByCodeContract(codeTransferAgreement);
                if (result != null)
                {
                    return Ok(new { status = false, mess = "HĐ đã tồn tại" });
                }
                else
                {
                    try
                    {
                        _contractInvester.Raw_Update(contractInvestorOld);
                    }
                    catch (Exception ex)
                    {
                        //write trace log
                        LogModel.Result = ActionResultValue.UpdateFailed;
                        LogModel.Message = "Duyệt HĐ không thành công!";
                        Logger.LogError(LogModel.ToString());

                        return Ok(new { status = false, mess = "Duyệt HĐ không thành công" });
                    }
                    if (!string.IsNullOrEmpty(codeTransferAgreement) && data.Result[0].CodeIntermediaries != null)
                    {
                        try
                        {
                            _contractInvester.UpdateNewCodeContract(id, codeTransferAgreement);

                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                        try
                        {
                            _contractInvestorInstallments.UpdateNewCodeContract(codeContractOld, codeTransferAgreement);

                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                        data.Result[0].CodeContract = codeTransferAgreement;
                    }
                }
            }
            //HUNGVX-NOT AMBER, EDIT CODECONTRACT
            if (!string.IsNullOrEmpty(createDateContract) && data.Result[0].CodeIntermediaries == null)
            {
                createDateContract = createDateContract.Replace(SiteConst.SubstractChar, SiteConst.SlashChar);
                DateTime changeDateValue;
                bool isValidDate = DateTime.TryParseExact(createDateContract, SiteConst.Format.DateFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out changeDateValue);
                if (!isValidDate)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Ngày vào hợp đồng phải có định dạng ngày tháng năm";
                    Logger.LogWarning(LogModel.ToString());

                    return Ok(new { status = false, mess = "Ngày vào hợp đồng phải có định dạng ngày tháng năm" });
                }
                string stt = contractInvestorOld.CodeContract.Split('/')[0];
                string sttYear = contractInvestorOld.CodeContract.Split('/')[2];
                string sttSFG = contractInvestorOld.CodeContract.Split('/')[3];
                string sttSFI = contractInvestorOld.CodeContract.Split('/')[4];
                string newContract = string.Concat(stt, "/", changeDateValue.ToString("ddMM"), "/", sttYear, "/", sttSFG, "/", sttSFI);
                //HUNGVX-UPDATE NEW CONTRACT FOR ACCOUNTAIN
                try
                {
                    _contractInvester.UpdateNewCodeContract(id, newContract);

                }
                catch (Exception ex)
                {

                    throw;
                }
                try
                {
                    _contractInvestorInstallments.UpdateNewCodeContract(data.Result[0].CodeContract, newContract);

                }
                catch (Exception ex)
                {

                    throw;
                }
                data.Result[0].CodeContract = newContract;
            }
            var update_contract = _contractInvester.UpdatePayDone(id, approvedDate);
            var update_status = _contractInvestorInstallments.UpdatePayDone(data.Result[0].CodeContract);
            //HUNGVX-UPDATE CODECONTRACT FOR AMBER
            if (!string.IsNullOrEmpty(codeTransferAgreement) && data.Result[0].CodeIntermediaries != null)
            {
                try
                {
                    _contractInvester.UpdateNewCodeContract(id, codeTransferAgreement);

                }
                catch (Exception ex)
                {

                    throw;
                }
                try
                {
                    _contractInvestorInstallments.UpdateNewCodeContract(data.Result[0].CodeContract, codeTransferAgreement);

                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            if (!update_contract && !update_status)
            {
                //write trace log
                LogModel.Result = ActionResultValue.UpdateFailed;
                LogModel.Message = "Duyệt HĐ không thành công!";
                Logger.LogError(LogModel.ToString());

                return Ok(new { status = false, mess = "Duyệt HĐ không thành công" });
            }

            //HUNGVX-UPDATE ACCOUNTBANK LƯU KÝ
            if (!string.IsNullOrEmpty(AccountBank2))
            {
                _investor.UpdateAccountBank2(AccountBank2, contractInvestorOld.CodeInvestor);
            }
            //write trace log
            LogModel.Result = ActionResultValue.UpdateSuccess;
            LogModel.Message = "Duyệt HĐ thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Ok(new { status = true, mess = "Duyệt HĐ thành công" });
        }
        #endregion

        #region View - Hungvx
        public IActionResult PreviewContract(int id)
        {
            var model = _contractInvester.GetInfoById(id);
            foreach (var item in model.Result)
            {
                item.ListInforBill = _contractInvester.GetListBill(id).Result.ToList<InforBill>();
            }
            DateTime? nullValue = null;
            var modelPrint = new ContractInvestorPrintViewModel();
            //khanhkk added
            if (!string.IsNullOrEmpty(model.Result[0].InvestorSignature))
            {
                modelPrint.InvestorSignature = Path.Combine("https://", HttpContext.Request.Host.Value, "Uploads", "Signatures", model.Result[0].InvestorSignature);
            }
            modelPrint.Name = model.Result[0].Name;
            modelPrint.IdStatusContract = model.Result[0].IdStatusContract;
            //khanhkk added
            modelPrint.CodeContract = model.Result[0].CodeContract;
            modelPrint.IdCard = model.Result[0].IdCard;
            modelPrint.DateOfIssuance = model.Result[0].DateOfIssuance;
            modelPrint.DateOfIssuancePrint = model.Result[0].DateOfIssuance != null ? DateTime.ParseExact(model.Result[0].DateOfIssuance, "dd/MM/yyyy", null).ToString("MM/dd/yyyy") : "";
            modelPrint.AddressIssuance = model.Result[0].AddressIssuance;
            modelPrint.AddressIssuancePrint = model.Result[0].AddressIssuance != null ? Utils.convertToUnSign(model.Result[0].AddressIssuance) : "";
            modelPrint.Address = model.Result[0].Address;
            modelPrint.AddressPrint = model.Result[0].Address != null ? Utils.convertToUnSign(model.Result[0].Address) : "";
            modelPrint.PersonalTaxCode = model.Result[0].PersonalTaxCode;
            modelPrint.Phone = model.Result[0].Phone;
            modelPrint.AccountBank = model.Result[0].AccountBank;
            modelPrint.Bank = model.Result[0].Bank;
            modelPrint.BankPrint = model.Result[0].Bank != null ? Utils.convertToUnSign(model.Result[0].Bank) : "";
            modelPrint.InvestmentAmount = model.Result[0].InvestmentAmount;
            modelPrint.InvestmentAmountPrint = model.Result[0].InvestmentAmount != null ? Utils.MoneyToText(model.Result[0].InvestmentAmount.ToString()) : "";
            modelPrint.InvestmentAmountPrintEN = model.Result[0].InvestmentAmount != null ? Utils.ConvertCurrencyToText((long)model.Result[0].InvestmentAmount) : "";
            modelPrint.CountShare = (int)(modelPrint.InvestmentAmount / (50000));
            modelPrint.CountShareVN = modelPrint.CountShare > 0 ? Utils.MoneyToText(modelPrint.CountShare.ToString()) : "";
            modelPrint.CountShareEN = modelPrint.CountShare > 0 ? Utils.ConvertCurrencyToText(modelPrint.CountShare) : "";
            //% CỌC 
            var result = _investor.CheckSumAmountDepositWithContract(model.Result[0].Phone, model.Result[0].CodeContract) ?? new InvestorViewModel();
            modelPrint.DepositAmount = result != null ? decimal.Parse(result.SumPaymentAmount) : 0; 
            modelPrint.PercenDeposit = Math.Round((modelPrint.DepositAmount > 0) ? (modelPrint.DepositAmount.Value / modelPrint.InvestmentAmount.Value) : 0, 2);
            //SỐ TIỀN CÒN LẠI
            try
            {
                modelPrint.RemainAmount = (model.Result[0].ListInforBill != null) ? modelPrint.InvestmentAmount.Value - (modelPrint.DepositAmount.Value) : modelPrint.InvestmentAmount.Value - (modelPrint.DepositAmount.Value);

            }
            catch (Exception ex)
            {

                throw;
            }
            modelPrint.RemainAmountVN = (modelPrint.RemainAmount > 0) ? Utils.MoneyToText(modelPrint.RemainAmount.ToString()) : "";
            modelPrint.RemainAmountEN = (modelPrint.RemainAmount > 0) ? Utils.ConvertCurrencyToText((long)modelPrint.RemainAmount) : "";
            //THỜI GIAN THANH TOÁN ĐỢT 2
            //if (model.Result[0].ListInforBill != null && model.Result[0].ListInforBill.Count == 1)
            //{
            //    modelPrint.BillTwo = (model.Result[0].ListInforBill[0].DateBill != null) ? DateTime.ParseExact(model.Result[0].ListInforBill[0].DateBill, "dd/MM/yyyy", null) : nullValue;
            //    modelPrint.day = modelPrint.BillTwo != null ? modelPrint.BillTwo.Value.Day - DateTime.ParseExact(model.Result[0].CreateDate, "dd/MM/yyyy", null).Day : 0;
            //}
            //else
            //{
            //    modelPrint.BillTwo = nullValue;
            //    modelPrint.day = 0;
            //}
            //THỜI GIAN DUYỆT HĐ
            modelPrint.DatePaydone = model.Result[0].DatePaydone ?? nullValue;

            return PartialView("Preview", modelPrint);
        }
        #endregion

        #region UploadFile
        [HttpPost]
        public IActionResult UploadFile(IFormFile Files, string IdContractInvestor)
        {
            //trace log
            LogModel.ItemName = "contract file(s)";
            LogModel.Action = ActionType.Upload;
            LogModel.Data = (new { Files = Files, IdContractInvestor = IdContractInvestor }).ToDataString();

            string lstFileName = string.Empty;
            var createResult = new DataResult<UploadFileToView>();
            // attach user
            string fileName;
            SiteConst.UploadStatus result = Helper.UploadFile(Files, _saveFileFolder, out fileName);
            var fileModel = new FileUploadViewModel()
            {
                CreateDate = DateTime.Now,
                Reason = "",
                UserUpload = tokenModel.StaffCode,
                LinksUpload = fileName,
                IdContractInvestor = int.Parse(IdContractInvestor)
            };
            createResult = _iFile.Insert(fileModel);
            lstFileName = fileName;

            //write trace log
            LogModel.Result = ActionResultValue.UploadSuccess;
            LogModel.Message = "Upload file thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Ok(new { data = lstFileName, idImage = createResult.DataItem.Id});
        }
        #endregion

        #region Del File
        public IActionResult DeleteImage(string filePath, int id)
        {
            //trace log
            LogModel.Action = ActionType.Delete;
            LogModel.ItemName = "contract file(s)";
            LogModel.Data = (new { id = id, FilePath = filePath }).ToDataString();

            if (string.IsNullOrEmpty(filePath))
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Đường dẫn file không được để trống";
                Logger.LogWarning(LogModel.ToString());

                return Ok(new { status = false });
            }
            else
            {
                var result = Helper.DeleteImage(_saveFileFolder + "\\" + filePath);
                try
                {
                    _iFile.Delete(id);
                }
                catch (Exception ex)
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.DeleteFailed;
                    LogModel.Message = "Xóa file thất bại";
                    Logger.LogError(ex, LogModel.ToString());

                    return Ok(new { status = false });
                }

                //write trace log 
                LogModel.Result = ActionResultValue.DeleteSuccess;
                LogModel.Message = "Xóa file thành công";
                Logger.LogInformation(LogModel.ToString());

                return Ok(new { status = result });
            }
        }
        #endregion
        #region HUNGVX-UPDATE CODECONTRACT AMBER
        public IActionResult UpdateCodeContractAmber(int id, string payDoneDate, string createDateContract, string codeManagementCatalog, string codeSupplementAgreement, string codeTransferAgreement)
        {
            var contractInvestorOld = _contractInvester.Raw_Get(id);
            var codeContractOld = contractInvestorOld.CodeContract;
            codeManagementCatalog = string.Concat(codeManagementCatalog.Trim(), "/HÐQLDM-AFM");
            codeTransferAgreement = string.Concat(codeTransferAgreement.Trim(), "/FSFG/SFI");
            codeSupplementAgreement = string.Concat(codeSupplementAgreement.Trim(), "/FSFG/SFI");

            contractInvestorOld.CodeManagementCatalog = codeManagementCatalog;
            contractInvestorOld.CodeTransferAgreement = codeTransferAgreement;
            contractInvestorOld.CodeSupplementAgreement = codeSupplementAgreement;
            if (codeContractOld == string.Concat(codeTransferAgreement.Trim(), "/FSFG/SFI"))
            {
                try
                {
                    _contractInvester.Raw_Update(contractInvestorOld);
                }
                catch (Exception ex)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.UpdateFailed;
                    LogModel.Message = "Duyệt HĐ không thành công!";
                    Logger.LogError(LogModel.ToString());

                    return Ok(new { status = false, mess = "Duyệt HĐ không thành công" });
                }
            }
            else
            {
                var result = _contractInvester.GetInfoByCodeContract(string.Concat(codeTransferAgreement.Trim(), "/FSFG/SFI"));
                if (result != null)
                {
                    return Ok(new { status = false, mess = "HĐ đã tồn tại" });
                }
                else
                {
                    try
                    {
                        _contractInvester.Raw_Update(contractInvestorOld);
                    }
                    catch (Exception ex)
                    {
                        //write trace log
                        LogModel.Result = ActionResultValue.UpdateFailed;
                        LogModel.Message = "Duyệt HĐ không thành công!";
                        Logger.LogError(LogModel.ToString());

                        return Ok(new { status = false, mess = "Duyệt HĐ không thành công" });
                    }
                    if (!string.IsNullOrEmpty(codeTransferAgreement))
                    {
                        try
                        {
                            _contractInvester.UpdateNewCodeContract(id, codeTransferAgreement);

                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                        try
                        {
                            _contractInvestorInstallments.UpdateNewCodeContract(codeContractOld, codeTransferAgreement);

                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                }
            }
           
            return Ok(new { status = true, mess = "Cập nhật thành công" });
        }
        #endregion
    }
}
