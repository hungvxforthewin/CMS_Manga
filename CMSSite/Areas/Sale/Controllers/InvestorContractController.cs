using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMBussiness.ViewModel;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using CRMSite.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CRMSite.Areas.Sale.Controllers
{
    [Area("Sale")]
    public class InvestorContractController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        private IContractInvester _contractInvester;
        private SendMailService _mailService;
        private IConfiguration _iconfig;
        private const string FinalStatus = "PayDone";

        public InvestorContractController(IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment env, SendMailService mailService, IConfiguration iconfig, ILogger<BaseController> logger
            ) : base(httpContextAccessor, logger)
        {
            this._env = env;
            _contractInvester = new ContractInvesterImp();
            _mailService = mailService;
            _iconfig = iconfig;
            LogModel.ItemName = "contract";
        }

        public IActionResult Index()
        {
            //write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return View();
        }

        #region GetList
        [HttpPost]
        public IActionResult GetList(SearchContractInvestorInstallmentsViewModel model)
        {
            model.Sale = tokenModel.StaffCode;
            model.Status = FinalStatus;

            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            int total;
            var data = _contractInvester.GetListByWaitPayDone(model, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;
            foreach (var item in data.Result)
            {
                item.Stock = item.InvestmentAmount / 50000;
            }
            //if (data.Error)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            //}

            //if (data.Result == null || data.Result.Count == 0)
            //{
            //    return Json(new { Result = 400, Errors = new List<string> { SiteConst.NotFoundError } });
            //}

            foreach (var item in data.Result)
            {
                if (item.IdStatusContract.Trim().ToLower() == "WaitPayDone".ToLower())
                {
                    item.NameStatus = "Chờ Duyệt";
                }
                else if (item.IdStatusContract.Trim().ToLower() == FinalStatus.ToLower())
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

        #region Sign
        [HttpGet]
        public async Task<IActionResult> Sign(int id)
        {
            //trace log
            LogModel.Action = ActionType.Sign;
            LogModel.Data = (new { id = id }).ToDataString();

            var getContractInfo = _contractInvester.GetInfoById(id);
            var handleResult = HandleGetResult(getContractInfo);
            if (handleResult != null) return handleResult;

            if (getContractInfo.Result.First().IdStatusContract != FinalStatus)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Khách hàng chỉ có thể ký hợp đồng đã hoàn thành chuyển tiền";
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Khách hàng chỉ có thể ký hợp đồng đã hoàn thành chuyển tiền" } });
            }

            //check investor email
            string emailAddress = getContractInfo.Result.First().Email;
            if (string.IsNullOrEmpty(emailAddress))
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Hợp đồng chưa có email của khách hàng! Vui lòng cập nhật email để sử dụng chức năng";
                Logger.LogWarning(LogModel.ToString());

                // expired otp
                return Json(new
                {
                    Result = 400,
                    Errors = new List<string> { "Hợp đồng chưa có email của khách hàng! Vui lòng cập nhật email để sử dụng chức năng" }
                });
            }

            // Lấy dịch vụ sendmailservice
            string otp = Utils.GenerateOTP();

            bool sendOtpResult = await SendOTP(otp, emailAddress);

            if (sendOtpResult)
            {
                int minutes = _iconfig.GetSection("MailSettings").GetValue<int>("TimeOut");
                // setup expiredtime for otp
                OTPModel otpModel = new OTPModel
                {
                    ExpiredTime = DateTime.Now.AddMinutes(minutes),
                    OTP = otp,
                };
                HttpContext.Session.SetString(SiteConst.SessionKey.OTP_VALUE, JsonConvert.SerializeObject(otpModel));
                HttpContext.Session.SetString(SiteConst.SessionKey.INVESTOR_EMAIL, emailAddress);

                //write trace log
                LogModel.Result = ActionResultValue.SentOTPSuccess;
                LogModel.ItemName = string.Empty;
                Logger.LogInformation(LogModel.ToString());

                return PartialView("_Confirm");
            }
            else
            {
                //write trace log
                LogModel.Result = ActionResultValue.SignFailed;
                LogModel.Message = "Gửi mã xác nhận tới email khách hàng không thành công. Vui lòng thử lại.";
                Logger.LogError(LogModel.ToString());

                // send mail failure
                return Json(new
                {
                    Result = 400,
                    Errors = new List<string> { "Gửi mã xác nhận tới email khách hàng không thành công. Vui lòng thử lại." }
                });
            }
        }

        #region ResendOTP
        [HttpGet]
        public async Task<IActionResult> ResendOTP()
        {
            //var originalOTP = JsonConvert.DeserializeObject<OTPModel>(HttpContext.Session.GetString(SiteConst.SessionKey.OTP_VALUE));
            //if (originalOTP != null && originalOTP.ExpiredTime > DateTime.Now)
            //{
            //    // expired otp
            //    return Json(new
            //    {
            //        Result = 400,
            //        Errors = new List<string> { "OTP có giá trị trong 3 phút. Vui lòng lấy lại OTP sau 3 phút để xác thực!" }
            //    });
            //}

            //trace log
            LogModel.Action = ActionType.SendOTP;
            LogModel.ItemName = string.Empty;

            // Lấy dịch vụ sendmailservice
            string otp = Utils.GenerateOTP();

            bool sendOtpResult = await SendOTP(otp, HttpContext.Session.GetString(SiteConst.SessionKey.INVESTOR_EMAIL));

            if (sendOtpResult)
            {
                int minutes = _iconfig.GetSection("MailSettings").GetValue<int>("TimeOut");
                // setup expiredtime for otp
                OTPModel otpModel = new OTPModel
                {
                    ExpiredTime = DateTime.Now.AddMinutes(minutes),
                    OTP = otp,
                };
                HttpContext.Session.SetString(SiteConst.SessionKey.OTP_VALUE, JsonConvert.SerializeObject(otpModel));

                //write trace log
                LogModel.Result = ActionResultValue.SentOTPSuccess;
                Logger.LogInformation(LogModel.ToString());

                //send mail successfully
                return Json(new
                {
                    Result = 200,
                    Message = "Hệ thống đã gửi mã xác nhận xác nhận thành công. Vui lòng nhập mã xác nhận để xác thực hành động",
                });
            }
            else
            {
                //write trace log
                LogModel.Result = ActionResultValue.SentOTPFailed;
                LogModel.Message = "Gửi mã xác nhận tới email khách hàng không thành công. Vui lòng thử lại.";
                Logger.LogError(LogModel.ToString());

                // send mail failure
                return Json(new
                {
                    Result = 400,
                    Errors = new List<string> { "Gửi mã xác nhận tới email khách hàng không thành công. Vui lòng thử lại." }
                });
            }
        }
        #endregion

        #region SendOTP
        private async Task<bool> SendOTP(string otp, string investorEmail = "vudinhkhanh2810@gmail.com")
        {
            // maid content is sent to an investor
            MailContent content = new MailContent
            {
                //To = "vudinhkhanh2810@gmail.com",
                To = investorEmail,
                Subject = "Gửi mã xác nhận",
                Body = @$"<p>Bạn đang có yêu cầu ký hợp đồng điện tử trên hệ thống CRM Sailfish. Vui lòng nhập mã xác nhận bên dưới để xác thực. <br /> Mã xác nhận: <strong>{otp}</strong></p>
                       <p>Đây là email tự động của hệ thống. Vui lòng không trả lời lại email này.</p>
                       <p>Cảm ơn quý khách đã sử dụng hệ thống CRM Sailfish của chúng tôi.</p>"
            };

            // result of sending mail
            bool sendOtpResult = await _mailService.SendMail(content);
            return sendOtpResult; 
        }
        #endregion

        #region ConfirmOTP
        [HttpGet]
        public IActionResult ConfirmOTP(string otp)
        {
            //trace log
            LogModel.Action = ActionType.ConfirmOTP;
            LogModel.ItemName = string.Empty;
            LogModel.Data = (new { otp = otp }).ToDataString();

            if (string.IsNullOrEmpty(otp))
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Chưa nhập mã xác nhận";
                Logger.LogWarning(LogModel.ToString());

                //invalid otp
                return Json(new
                {
                    Result = 400,
                    Errors = new List<string> { "Chưa nhập mã xác nhận!" }
                });
            }

            var originalOTP = JsonConvert.DeserializeObject<OTPModel>(HttpContext.Session.GetString(SiteConst.SessionKey.OTP_VALUE));

            if (originalOTP.ExpiredTime < DateTime.Now)
            {
                int minutes = _iconfig.GetSection("MailSettings").GetValue<int>("TimeOut");

                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = $"Mã xác nhận chỉ có giá trị trong {minutes} phút. Vui lòng lấy lại mã xác nhận để xác thực!";
                Logger.LogWarning(LogModel.ToString());

                // expired otp
                return Json(new
                {
                    Result = 400,
                    Errors = new List<string> { $"Mã xác nhận chỉ có giá trị trong {minutes} phút. Vui lòng lấy lại mã xác nhận để xác thực!" }
                });
            }

            if (otp != originalOTP.OTP)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Mã xác nhận vừa nhập không đúng";
                Logger.LogWarning(LogModel.ToString());

                //invalid otp
                return Json(new
                {
                    Result = 400,
                    Errors = new List<string> { "Mã xác nhận vừa nhập không đúng!" }
                });
            }
            HttpContext.Session.Remove(SiteConst.SessionKey.INVESTOR_EMAIL);
            HttpContext.Session.Remove(SiteConst.SessionKey.OTP_VALUE);

            //write trace log
            LogModel.Result = ActionResultValue.ConfirmOTPSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Sign");
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> Sign(string data, int id)
        {
            //trace log
            LogModel.Action = ActionType.Sign;
            LogModel.Data = (new { id = id, data = data }).ToDataString();

            if (data != null)
            {
                // memory
                string saveFolder = Path.Combine(_env.WebRootPath, "Uploads", "Signatures");
                // generate folder by current date
                DateTime now = DateTime.Now;
                string subFolder = Path.Combine(now.Year.ToString(), now.Month.ToString(), now.Day.ToString());

                string filePath = Path.Combine(saveFolder, subFolder);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                // generate file name
                string radomFileName = Path.GetRandomFileName();
                filePath = Path.Combine(filePath, radomFileName + ".png");

                try
                {
                    var bytes = Convert.FromBase64String(data.Split(',')[1]);
                    using (var imageFile = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.WriteAsync(bytes, 0, bytes.Length);
                        await imageFile.FlushAsync();
                    }
                }
                catch (Exception ex)
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.SignFailed;
                    LogModel.Message = "Lưu mẫu chữ ký không thành công!";
                    Logger.LogError(ex, LogModel.ToString());

                    return Json(new { Result = 400, ErrorMessage = new List<string> { "Lưu mẫu chữ ký không thành công!" } });
                }
                //get old signature
                var getContractInfo = _contractInvester.GetInfoById(id);
                string oldSignature = getContractInfo.Result.First().InvestorSignature;

                var updateSignatureRs = _contractInvester.UpdateSignature(id, Path.Combine(subFolder, radomFileName + ".png"));
                if (!updateSignatureRs)
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.SignFailed;
                    LogModel.Message = "Lưu mẫu chữ ký không thành công!";
                    Logger.LogError(LogModel.ToString());

                    System.IO.File.Delete(filePath);
                    return Json(new { Result = 400, ErrorMessage = new List<string> { "Lưu mẫu chữ ký không thành công!" } });
                }
                
                //check and delete old signature
                if (!string.IsNullOrEmpty(oldSignature))
                {
                    string oldSignaturePath = Path.Combine(_env.WebRootPath, "Uploads", "Signatures", oldSignature);
                    if (System.IO.File.Exists(oldSignaturePath))
                    {
                        System.IO.File.Delete(oldSignaturePath);
                    }
                }

                //write trace log 
                LogModel.Result = ActionResultValue.SignSuccess;
                LogModel.Message = "Lưu mẫu chữ ký thành công!";
                Logger.LogInformation(LogModel.ToString());

                return Json(new { Result = 200, Message = "Lưu mẫu chữ ký thành công!" });
            }

            //write trace log 
            LogModel.Result = ActionResultValue.SignFailed;
            LogModel.Message = "Hệ thống chưa nhận được mẫu chữ ký!";
            Logger.LogWarning(LogModel.ToString());

            return Json(new { Result = 400, ErrorMessage = new List<string> { "Hệ thống chưa nhận được mẫu chữ ký!" } });
        }
        #endregion

        #region Print
        [HttpPost]
        public async Task<IActionResult> Print(int id)
        {
            //trace log
            LogModel.Action = ActionType.Print;
            LogModel.Data = (new { id = id }).ToDataString();

            var model = _contractInvester.GetInfoById(id);
            foreach (var item in model.Result)
            {
                item.ListInforBill = _contractInvester.GetListBill(id).Result.ToList<InforBill>();
            }
            DateTime? nullValue = null;
            var modelPrint = new ContractInvestorPrintViewModel();
            ContractInvesterViewModel investorContractInfo = model.Result[0];
            if (!string.IsNullOrEmpty(investorContractInfo.InvestorSignature))
            {
                modelPrint.InvestorSignature = Path.Combine("https://", HttpContext.Request.Host.Value, "Uploads", "Signatures", investorContractInfo.InvestorSignature);
            }
            modelPrint.Name = model.Result[0].Name;
            modelPrint.IdStatusContract = model.Result[0].IdStatusContract;
            modelPrint.CodeContract = investorContractInfo.CodeContract;
            modelPrint.IdCard = investorContractInfo.IdCard;
            modelPrint.DateOfIssuance = investorContractInfo.DateOfIssuance;
            modelPrint.DateOfIssuancePrint = investorContractInfo.DateOfIssuance != null ? DateTime.ParseExact(investorContractInfo.DateOfIssuance, SiteConst.Format.DateFormat, null).ToString("MM/dd/yyyy") : "";
            modelPrint.AddressIssuance = investorContractInfo.AddressIssuance;
            modelPrint.AddressIssuancePrint = investorContractInfo.AddressIssuance != null ? Utils.convertToUnSign(investorContractInfo.AddressIssuance) : "";
            modelPrint.Address = investorContractInfo.Address;
            modelPrint.AddressPrint = investorContractInfo.Address != null ? Utils.convertToUnSign(investorContractInfo.Address) : "";
            modelPrint.PersonalTaxCode = investorContractInfo.PersonalTaxCode;
            modelPrint.Phone = investorContractInfo.Phone;
            modelPrint.AccountBank = investorContractInfo.AccountBank;
            modelPrint.Bank = investorContractInfo.Bank;
            modelPrint.BankPrint = investorContractInfo.Bank != null ? Utils.convertToUnSign(investorContractInfo.Bank) : "";
            modelPrint.InvestmentAmount = investorContractInfo.InvestmentAmount;
            modelPrint.InvestmentAmountPrint = investorContractInfo.InvestmentAmount != null ? Utils.MoneyToText(investorContractInfo.InvestmentAmount.ToString()) : "";
            modelPrint.InvestmentAmountPrintEN = investorContractInfo.InvestmentAmount != null ? Utils.ConvertCurrencyToText((long)investorContractInfo.InvestmentAmount) : "";
            modelPrint.CountShare = (int)(modelPrint.InvestmentAmount / (50000));
            modelPrint.CountShareVN = modelPrint.CountShare > 0 ? Utils.MoneyToText(modelPrint.CountShare.ToString()) : "";
            modelPrint.CountShareEN = modelPrint.CountShare > 0 ? Utils.ConvertCurrencyToText(modelPrint.CountShare) : "";
            //% CỌC 
            modelPrint.DepositAmount = investorContractInfo.DepositAmount;
            modelPrint.PercenDeposit = Math.Round((modelPrint.DepositAmount > 0) ? (modelPrint.DepositAmount.Value / modelPrint.InvestmentAmount.Value) : 0, 2);
            //SỐ TIỀN CÒN LẠI
            try
            {
                modelPrint.RemainAmount = (investorContractInfo.ListInforBill != null) ? modelPrint.InvestmentAmount.Value - (modelPrint.DepositAmount.Value) : modelPrint.InvestmentAmount.Value - (modelPrint.DepositAmount.Value);

            }
            catch (Exception ex)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.PrintFailed;
                LogModel.Message = "Lỗi phát sinh khi in hợp đồng!";
                Logger.LogError(ex, LogModel.ToString());

                throw;
            }
            modelPrint.RemainAmountVN = (modelPrint.RemainAmount > 0) ? Utils.MoneyToText(modelPrint.RemainAmount.ToString()) : "";
            modelPrint.RemainAmountEN = (modelPrint.RemainAmount > 0) ? Utils.ConvertCurrencyToText((long)modelPrint.RemainAmount) : "";
            //THỜI GIAN THANH TOÁN ĐỢT 2
            //if (investorContractInfo.ListInforBill != null && investorContractInfo.ListInforBill.Count == 1)
            //{
            //    modelPrint.BillTwo = (investorContractInfo.ListInforBill[0].DateBill != null) ? DateTime.ParseExact(investorContractInfo.ListInforBill[0].DateBill, SiteConst.Format.DateFormat, null) : nullValue;
            //    modelPrint.day = modelPrint.BillTwo != null ? modelPrint.BillTwo.Value.Day - DateTime.ParseExact(investorContractInfo.CreateDate, SiteConst.Format.DateFormat, null).Day : 0;
            //}
            //else
            //{
            //    modelPrint.BillTwo = nullValue;
            //    modelPrint.day = 0;
            //}
            //THỜI GIAN DUYỆT HĐ
            modelPrint.DatePaydone = investorContractInfo.DatePaydone ?? nullValue;
            string html = await this.RenderViewAsync("Print", modelPrint);
            var data = new
            {
                datahtml = html
            };

            //write trace log 
            LogModel.Result = ActionResultValue.PrintSuccess;
            LogModel.Message = "In hợp đồng thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(data);
        }
        #endregion
    }
}
