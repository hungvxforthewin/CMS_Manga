using CRMBussiness.IService;
using CRMBussiness.ViewModel;
using CRMSite.Common;
using CRMSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CRMSite.Controllers
{
    [Authorize]
    public class PrintContractController : Controller
    {
        private const string EnDateFormat = "MM/dd/yyyy";

        public readonly IInvestor _investor;
        public readonly IDeposit _deposit;
        public readonly IStatusContractInvestors _statusContractInvestors;
        public readonly IContractInvester _contractInvester;
        public readonly IContractInvestorInstallments _contractInvestorInstallments;
        private readonly ILogger<PrintContractController> _logger;

        public PrintContractController(IHttpContextAccessor httpContextAccessor, IInvestor investor, IStatusContractInvestors statusContractInvestors, IContractInvester contractInvester, IDeposit deposit, IContractInvestorInstallments contractInvestorInstallments, ILogger<PrintContractController> logger)
        {
            _investor = investor;
            _deposit = deposit;
            _statusContractInvestors = statusContractInvestors;
            _contractInvester = contractInvester;
            _contractInvestorInstallments = contractInvestorInstallments;
            _logger = logger;
        }

        public IActionResult SelectContractTemplate()
        {
            return PartialView("_SelectContractTemplate");
        }

        #region Print - Hungvx
        [HttpPost]
        public async Task<IActionResult> Print(int id)
        {
            //trace log
            LogModel log = new LogModel
            {
                AccessTarget = HttpContext.Request.Path.Value,
                Username = HttpContext.User.Claims.First(x => x.Type == SiteConst.TokenKey.FULLNAME).Value,
                Role = Helper.GetRoleName(byte.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Role).Value)),
                ItemName = "contract",
                Action = ActionType.Print,
                Data = (new { id = id }).ToDataString(),
            };

            var getResult = GetContractInfo(id);

            string html = await this.RenderViewAsync("Print", getResult);
            var data = new
            {
                datahtml = html
            };

            //write trace log
            log.Result = ActionResultValue.PrintSuccess;
            _logger.LogInformation(log.ToString());
            return Json(data);
        }
        #endregion
        /// <summary>
        /// HĐ DANH MỤC ĐẦU TƯ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        #region PrintDMDT - Hungvx
        [HttpPost]
        public async Task<IActionResult> PrintDMDT(int id)
        {
            //trace log
            LogModel log = new LogModel
            {
                AccessTarget = HttpContext.Request.Path.Value,
                Username = HttpContext.User.Claims.First(x => x.Type == SiteConst.TokenKey.FULLNAME).Value,
                Role = Helper.GetRoleName(byte.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Role).Value)),
                ItemName = "contract_DMDT",
                Action = ActionType.Print,
                Data = (new { id = id }).ToDataString(),
            };

            var getResult = GetContractInfo(id);

            string html = await this.RenderViewAsync("PrintDMDT", getResult);
            var data = new
            {
                datahtml = html
            };

            //write trace log
            log.Result = ActionResultValue.PrintSuccess;
            _logger.LogInformation(log.ToString());
            return Json(data);
        }
        #endregion
        /// <summary>
        /// HĐ CHUYỂN NHƯỢNG
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        #region PrintTransferAgreement - Hungvx
        [HttpPost]
        public async Task<IActionResult> PrintTransferAgreement(int id)
        {
            //trace log
            LogModel log = new LogModel
            {
                AccessTarget = HttpContext.Request.Path.Value,
                Username = HttpContext.User.Claims.First(x => x.Type == SiteConst.TokenKey.FULLNAME).Value,
                Role = Helper.GetRoleName(byte.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Role).Value)),
                ItemName = "contract_TransferAgreement",
                Action = ActionType.Print,
                Data = (new { id = id }).ToDataString(),
            };

            var getResult = GetContractInfo(id);

            string html = await this.RenderViewAsync("TransferAgreementPrint", getResult);
            var data = new
            {
                datahtml = html
            };

            //write trace log
            log.Result = ActionResultValue.PrintSuccess;
            _logger.LogInformation(log.ToString());
            return Json(data);
        }
        #endregion

        /// <summary>
        /// HĐ THỎA THUẬN BỔ SUNG
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        #region PrintSupplementAgreement - Hungvx
        [HttpPost]
        public async Task<IActionResult> PrintSupplementAgreement(int id)
        {
            //trace log
            LogModel log = new LogModel
            {
                AccessTarget = HttpContext.Request.Path.Value,
                Username = HttpContext.User.Claims.First(x => x.Type == SiteConst.TokenKey.FULLNAME).Value,
                Role = Helper.GetRoleName(byte.Parse(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Role).Value)),
                ItemName = "contract_SupplementAgreement",
                Action = ActionType.Print,
                Data = (new { id = id }).ToDataString(),
            };

            var getResult = GetContractInfo(id);

            string html = await this.RenderViewAsync("SupplementAgreementPrint", getResult);
            var data = new
            {
                datahtml = html
            };

            //write trace log
            log.Result = ActionResultValue.PrintSuccess;
            _logger.LogInformation(log.ToString());
            return Json(data);
        }
        #endregion

        #region GetContractInfo
        private ContractInvestorPrintViewModel GetContractInfo(int id)
        {
            var model = _contractInvester.GetInfoById(id);
            ContractInvesterViewModel investorContract = model.Result[0];
            foreach (var item in model.Result)
            {
                item.ListInforBill = _contractInvester.GetListBill(id).Result.ToList<InforBill>();
            }
            DateTime? nullValue = null;
            var modelPrint = new ContractInvestorPrintViewModel();
            //khanhkk added
            if (!string.IsNullOrEmpty(investorContract.InvestorSignature))
            {
                modelPrint.InvestorSignature = Path.Combine("https://", HttpContext.Request.Host.Value, "Uploads", "Signatures", investorContract.InvestorSignature);
            }
            modelPrint.Name = investorContract.Name;
            modelPrint.PrintName = Utils.convertToUnSign(investorContract.Name);
            modelPrint.IdStatusContract = investorContract.IdStatusContract;
            //khanhkk added
            modelPrint.CodeContract = investorContract.CodeContract;
            modelPrint.IdCard = investorContract.IdCard;
            modelPrint.DateOfIssuance = investorContract.DateOfIssuance;
            modelPrint.DateOfIssuancePrint = investorContract.DateOfIssuance != null ? DateTime.ParseExact(investorContract.DateOfIssuance, SiteConst.Format.DateFormat, null).ToString(EnDateFormat) : string.Empty;
            //modelPrint.AddressIssuance = investorContract.AddressIssuance;
            if (investorContract.IsCMT == "True")
            {
                modelPrint.AddressIssuance = investorContract.AddressIssuance;
                modelPrint.AddressIssuancePrint = investorContract.AddressIssuance != null ? Utils.convertToUnSign(investorContract.AddressIssuance) : string.Empty;
            }
            else if (investorContract.IsCMT == "False" && investorContract.AddressIssuance == "0")
            {
                modelPrint.AddressIssuance = SiteConst.CCCD.CSDK;
                modelPrint.AddressIssuancePrint = SiteConst.CCCD.CSDK_EN;
            }
            else if (investorContract.IsCMT == "False" && investorContract.AddressIssuance == "1")
            {
                modelPrint.AddressIssuance = SiteConst.CCCD.CSHC;
                modelPrint.AddressIssuancePrint = SiteConst.CCCD.CSHC_EN;
            }
            modelPrint.Address = investorContract.Address;
            modelPrint.AddressPrint = investorContract.Address != null ? Utils.convertToUnSign(investorContract.Address) : string.Empty;
            modelPrint.PersonalTaxCode = investorContract.PersonalTaxCode;
            modelPrint.Phone = investorContract.Phone;
            modelPrint.AccountBank = investorContract.AccountBank;
            modelPrint.AccountBank2 = investorContract.AccountBank2;
            modelPrint.Bank = investorContract.Bank;
            modelPrint.BankPrint = investorContract.Bank != null ? Utils.convertToUnSign(investorContract.Bank) : string.Empty;
            modelPrint.InvestmentAmount = investorContract.InvestmentAmount;
            modelPrint.InvestmentAmountPrint = investorContract.InvestmentAmount != null ? Utils.MoneyToText(investorContract.InvestmentAmount.ToString()) : string.Empty;
            modelPrint.InvestmentAmountPrintEN = investorContract.InvestmentAmount != null ? Utils.ConvertCurrencyToText((long)investorContract.InvestmentAmount) : string.Empty;
            modelPrint.CountShare = (int)(modelPrint.InvestmentAmount / (50000));
            modelPrint.CountShareVN = modelPrint.CountShare > 0 ? Utils.MoneyToText(modelPrint.CountShare.ToString()) : string.Empty;
            modelPrint.CountShareEN = modelPrint.CountShare > 0 ? Utils.ConvertCurrencyToText(modelPrint.CountShare) : string.Empty;
            //% CỌC 
            var result = _investor.CheckSumAmountDepositWithContract(investorContract.Phone, investorContract.CodeContract) ?? new InvestorViewModel();
            modelPrint.DepositAmount = result != null ? decimal.Parse(result.SumPaymentAmount ?? "0") : 0;
            modelPrint.PercenDeposit = Math.Round((modelPrint.DepositAmount > 0) ? (modelPrint.DepositAmount.Value / modelPrint.InvestmentAmount.Value) : 0, 2);
            //SỐ TIỀN CÒN LẠI
            try
            {
                modelPrint.RemainAmount = (investorContract.ListInforBill != null) ? modelPrint.InvestmentAmount.Value - (modelPrint.DepositAmount.Value) : modelPrint.InvestmentAmount.Value - (modelPrint.DepositAmount.Value);

            }
            catch (Exception ex)
            {

                throw;
            }
            modelPrint.RemainAmountVN = (modelPrint.RemainAmount > 0) ? Utils.MoneyToText(modelPrint.RemainAmount.ToString()) : string.Empty;
            modelPrint.RemainAmountEN = (modelPrint.RemainAmount > 0) ? Utils.ConvertCurrencyToText((long)modelPrint.RemainAmount) : string.Empty;
            modelPrint.CreatedDateValue = DateTime.ParseExact(investorContract.CreateDate, SiteConst.Format.DateFormat, null);
            //CÁC ĐỢT THANH TOÁN
            foreach (var item in investorContract.ListInforBill)
            {
                DateTime? billTow = (item.DateBill != null) ? DateTime.ParseExact(item.DateBill, SiteConst.Format.DateFormat, null) : nullValue;
                var dayR = billTow != null && investorContract.CreateDate != null ?
                    modelPrint.CreatedDateValue.Subtract(billTow.Value).Days : 0;
                var percenntR = Math.Round((item.BillMoney > 0) ? (item.BillMoney / modelPrint.InvestmentAmount.Value * 100) : 0, 2);
                modelPrint.LstPayment.Add(new ListPayment()
                {
                    BillTwo = billTow,
                    day = -dayR,
                    PriceBill = item.BillMoney,
                    PriceBillVN = (item.BillMoney > 0) ? Utils.MoneyToText(item.BillMoney.ToString()) : string.Empty,
                    PriceBillEN = (item.BillMoney > 0) ? Utils.ConvertCurrencyToText((long)item.BillMoney) : string.Empty,
                    PricePercent = percenntR
                });
            }
            //if (investorContract.ListInforBill != null && investorContract.ListInforBill.Count == 1)
            //{
            //    modelPrint.BillTwo = (investorContract.ListInforBill[0].DateBill != null) ? DateTime.ParseExact(investorContract.ListInforBill[0].DateBill, SiteConst.Format.DateFormat, null) : nullValue;
            //    modelPrint.day = modelPrint.BillTwo != null && investorContract.CreateDate != null ?
            //        modelPrint.CreatedDateValue.Subtract(modelPrint.BillTwo.Value).Days : 0;
            //}
            //else
            //{
            //    modelPrint.BillTwo = nullValue;
            //    modelPrint.day = 0;
            //}
            //THỜI GIAN DUYỆT HĐ
            modelPrint.DatePaydone = investorContract.DatePaydone ?? nullValue;

            modelPrint.CodeIntermediaries = investorContract.CodeIntermediaries;
            modelPrint.CodeManagementCatalog = investorContract.CodeManagementCatalog;
            modelPrint.CodeSupplementAgreement = investorContract.CodeSupplementAgreement;
            modelPrint.CodeTransferAgreement = investorContract.CodeTransferAgreement;
            return modelPrint;
        }
        #endregion
    }
}
