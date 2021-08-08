using ClosedXML.Excel;
using CRMBussiness;
using CRMBussiness.IService;
using CRMBussiness.ServiceImp;
using CRMBussiness.ViewModel;
using CRMSite.Common;
using CRMSite.Controllers;
using CRMSite.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CRMSite.Areas.Sale.Controllers
{
    [Area("Sale")]
    public class ExpectedInvestorController : BaseController
    {
        private IShowUpHistory _iHis;
        private IWebHostEnvironment _env;
        private Dictionary<string, short?> JoinedObjectList = new Dictionary<string, short?>
        {
            { "SINGLE NAM" , 0 },
            { "SINGLE NỮ" , 1 },
            { "COUPLE" , 2 },
            { "GROUP" , 3 },
        };

        public ExpectedInvestorController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env, ILogger<BaseController> logger) : base(httpContextAccessor, logger)
        {
            _iHis = new ExShowUpHistoryImp();
            _env = env;
            LogModel.ItemName = "expected investor(s)";
        }

        #region Index
        public IActionResult Index()
        {
            IEvent iEvent = new EventImp();
            var getNearEvent = iEvent.GetNearestEvent(true, tokenModel.BranchCode);
            if (getNearEvent.Error)
            {
                //write trace log
                LogModel.Action = ActionType.GetInfo;
                LogModel.ItemName = "get nearest event";
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());
            }

            if (getNearEvent.Result != null && getNearEvent.Result.Count > 0)
            {
                ViewBag.NearEvent = getNearEvent.Result.First().CodeEvent;
            }

            //write trace log
            LogModel.Action = ActionType.GetInfo;
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return View();
        }
        #endregion

        #region Create
        [HttpPost]
        public IActionResult Create(ShowUpHistoryCreateViewModel model)
        {
            model.TeleSale = null;
            //trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.Create;

            if (!ModelState.IsValid)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = Helper.GetErrors(ModelState).ToMessageString();
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = Helper.GetErrors(ModelState) });
            }

            var checkInputResult = CheckInput(model);
            if (checkInputResult != null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = checkInputResult.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return checkInputResult;
            }

            model.CreatedBy = tokenModel.StaffCode;
            model.Sale = model.Sale??tokenModel.StaffCode;
            var createResult = _iHis.CreateCheckIn(model);
            if (createResult.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.CreateFailed;
                LogModel.Message = "Thêm mới thông tin khách hàng dự kiến không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Thêm mới thông tin khách hàng dự kiến không thành công" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.CreateSuccess;
            LogModel.Message = "Thêm mới thông tin khách hàng dự kiến thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Thêm mới thông tin khách hàng dự kiến thành công" });
        }
        #endregion

        #region Update
        public IActionResult Update(long id)
        {
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = (new { id = id }).ToDataString();

            var getResult = _iHis.GetById(id);
            var handleResult = HandleGetResult(getResult);
            if (handleResult != null) return handleResult;

            ShowUpHistoryCreateViewModel model = getResult.Result.First();

            if (model.TimeIn != null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.UpdateFailed;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Khách hàng đã check in. Liên hệ tới admin để cập thông tin khách hàng" } });
            }

            IInvestor iInvestor = new InvestorImp();
            var getGoWithPersonList = iInvestor.GetGoWithPersons(model.CodeShow);
            if (getGoWithPersonList.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }
            model.Group = getGoWithPersonList.Result.ToList();
            HttpContext.Session.SetString(SiteConst.SessionKey.EVENT, model.CodeEvent);

            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Update", model);
        }

        [HttpPost]
        public IActionResult Update(ShowUpHistoryCreateViewModel model)
        {
            model.TeleSale = null;
            //trace log
            LogModel.Action = ActionType.Update;
            LogModel.Data = model.ToDataString();

            if (!ModelState.IsValid)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = Helper.GetErrors(ModelState).ToMessageString();
                Logger.LogWarning(LogModel.ToString());

                return Json(new { Result = 400, Errors = Helper.GetErrors(ModelState) });
            }

            var checkInputResult = CheckInput(model);
            if (checkInputResult != null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = checkInputResult.ToDataString();
                Logger.LogWarning(LogModel.ToString());

                return checkInputResult;
            }

            model.UserUpdate = tokenModel.StaffCode;
            var updateResult = _iHis.UpdateCheckIn(model);
            if (updateResult.Error)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.UpdateFailed;
                LogModel.Message = "Cập nhật thông tin khách hàng dự kiến không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Cập nhật thông tin khách hàng dự kiến không thành công" } });
            }

            //write trace log 
            LogModel.Result = ActionResultValue.UpdateSuccess;
            LogModel.Message = "Cập nhật thông tin khách hàng dự kiến thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Cập nhật thông tin khách hàng dự kiến thành công" });
        }
        #endregion

        #region CheckInput
        private IActionResult CheckInput(ShowUpHistoryCreateViewModel model)
        {
            if (!string.IsNullOrEmpty(model.BirthdayString))
            {
                DateTime dob;
                bool validDOBDate = DateTime.TryParseExact(model.BirthdayString, SiteConst.Format.DateFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dob);
                if (!validDOBDate)
                {
                    return Json(new { Result = 400, Errors = new List<string> { "Ngày sinh không đúng định dạng ngày/tháng/năm" } });
                }
                model.Birthday = dob;
            }

            //check phone number participate in event
            List<string> phones = new List<string>();
            if (model.Group != null)
            {
                var goWithPhones = model.Group.Select(x => x.PhoneNumber).ToList();
                phones.AddRange(goWithPhones);
            }
            var checkSampePhoneNumber = phones.GroupBy(x => x).ToList();
            if (checkSampePhoneNumber.Count != phones.Count)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Số điện thoại không được trùng nhau" } });
            }

            //check event info
            IEvent iEvent = new EventImp();
            var eventInfo = iEvent.GetByShowUpCode(model.CodeEvent);
            if (eventInfo.Error || eventInfo.Result == null)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Không tìm thấy thông tin sự kiện" } });
            }

            if (eventInfo.Result.First().EndTime <= DateTime.Now)
            {
                return Json(new { Result = 400, Errors = new List<string> { "Show up đã kết thúc. Vui lòng chọn sự kiện khác" } });
            }

            var getCheckInfoResult = _iHis.GetInfoByPhoneNumber(model.PhoneNumber);
            if (!getCheckInfoResult.Error && getCheckInfoResult.Result != null)
            {
                var checkin = getCheckInfoResult.Result
                    .Where(x => x.ShowStartTime.Date == eventInfo.Result.First().EventTime.Value.Date && x.Id != model.Id).FirstOrDefault();
                if (checkin != null)
                {
                    return Json(new
                    {
                        Result = 400,
                        Errors = new List<string> { "Số điện thoại đã tồn tại trong danh sách khách hàng dự kiến tham gia show up trong ngày tổ chức sự kiện" }
                    });
                }
            }

            return null;
        }
        #endregion

        #region View
        public IActionResult View(long id)
        {
            //trace log
            LogModel.Data = (new { id = id }).ToDataString();
            LogModel.Action = ActionType.ViewInfo;

            var getResult = _iHis.GetById(id);
            var handleResult = HandleGetResult(getResult);
            if (handleResult != null) return handleResult;

            ShowUpHistoryCreateViewModel model = getResult.Result.First();

            IInvestor iInvestor = new InvestorImp();
            var getGoWithPersonList = iInvestor.GetGoWithPersons(model.CodeShow);
            if (getGoWithPersonList.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.ThrowException;
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { SiteConst.SystemError } });
            }
            model.Group = getGoWithPersonList.Result.ToList();

            //write trace log
            LogModel.Result = ActionResultValue.ViewSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_View", model);
        }
        #endregion

        #region GetList
        [HttpPost]
        public IActionResult GetList(SearchShowUpHistoryModel model)
        {
            model.CreatedBy = tokenModel.StaffCode;
            model.Branch = tokenModel.BranchCode;

            // trace log
            LogModel.Data = model.ToDataString();
            LogModel.Action = ActionType.GetInfo;

            //validate checkin date
            if (!string.IsNullOrEmpty(model.CheckIn))
            {
                DateTime checkinDate;
                bool validCheckinDate = DateTime.TryParseExact(model.CheckIn, SiteConst.Format.DateFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out checkinDate);
                if (!validCheckinDate)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Ngày checkin không đúng định dạng ngày/tháng/năm";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { Result = 400, Errors = new List<string> { "Ngày checkin không đúng định dạng ngày/tháng/năm" } });
                }
                model.CheckInDate = checkinDate;
            }

            // validate checkout date
            if (!string.IsNullOrEmpty(model.CheckOut))
            {
                DateTime checkoutDate;
                bool validCheckoutDate = DateTime.TryParseExact(model.CheckOut, SiteConst.Format.DateFormat, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out checkoutDate);
                if (!validCheckoutDate)
                {
                    //write trace log
                    LogModel.Result = ActionResultValue.InvalidInput;
                    LogModel.Message = "Ngày checkin không đúng định dạng ngày/tháng/năm";
                    Logger.LogWarning(LogModel.ToString());

                    return Json(new { Result = 400, Errors = new List<string> { "Ngày checkout không đúng định dạng ngày/tháng/năm" } });
                }
                model.CheckOutDate = checkoutDate;
            }

            int total;
            var data = _iHis.GetList(model, out total);
            var handleResult = HandleGetResult(data);
            if (handleResult != null) return handleResult;

            //write trace log
            LogModel.Result = ActionResultValue.GetInfoSuccess;
            LogModel.Data = data.Result.ToDataString();
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Data = data.Result, Total = total });
        }
        #endregion

        #region Import
        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file, string EventCode)
        {
            //trace log
            LogModel.Action = ActionType.Import;
            LogModel.Data = (new { file = file }).ToDataString();

            //valid model data
            if (string.IsNullOrEmpty(EventCode))
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Chưa chọn sự kiện!";
                Logger.LogWarning(LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Chưa chọn sự kiện!" }
                });
            }

            if (file == null)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Chưa chọn file chứa danh sách thông tin khách hàng dự kiến!";
                Logger.LogWarning(LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Chưa chọn file chứa danh sách thông tin khách hàng dự kiến!" }
                });
            }

            //check file type
            if (Path.GetExtension(file.FileName).ToLower() != ".xlsx" && Path.GetExtension(file.FileName).ToLower() != ".xls")
            {
                //write trace log 
                LogModel.Result = ActionResultValue.InvalidInput;
                LogModel.Message = "Chức năng nhập dữ liệu chỉ nhận file excel!";
                Logger.LogWarning(LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Chức năng nhập dữ liệu chỉ nhận file excel!" }
                });
            }

            var filePath = Path.Combine(_env.WebRootPath, "Content",
            Path.GetRandomFileName() + Path.GetExtension(file.FileName));

            int stt = 0;
            int cellNo;
            try
            {
                //save temp file
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }

                List<ShowUpHistoryCreateViewModel> listData = new List<ShowUpHistoryCreateViewModel>();
                IEvent iEvent = new EventImp();
                IAccount iAcc = new AccountImp();
                IWhereToFindInvestor iWhere = new WhereToFindInvestorImp();

                //import data timekeeping
                using (var excelWorkbook = new XLWorkbook(filePath))
                {
                    #region Show up
                    DataResult<EventViewModel> getEventInfo = iEvent.GetByShowUpCode(EventCode);
                    if (getEventInfo.Error || getEventInfo.Result == null || getEventInfo.Result.Count == 0)
                    {
                        //write trace log 
                        LogModel.Result = ActionResultValue.InvalidInput;
                        LogModel.Message = "Không tìm thấy thông tin sự kiện";
                        Logger.LogWarning(LogModel.ToString());

                        return Json(new
                        {
                            Result = 400,
                            Errors = new List<string> { "Không tìm thấy thông tin sự kiện" }
                        });
                    }
                    #endregion

                    #region Checkin date
                    //var dateRow = excelWorkbook.Worksheet(1).RowsUsed().ElementAt(2);
                    //string checkinDateValue = dateRow.Cell(3).Value.ToString();
                    //if (string.IsNullOrEmpty(checkinDateValue))
                    //{
                    //    //write trace log 
                    //    LogModel.Result = ActionResultValue.InvalidInput;
                    //    LogModel.Message = "Ngày checkin không được để trống";
                    //    Logger.LogWarning(LogModel.ToString());

                    //    return Json(new
                    //    {
                    //        Result = 400,
                    //        Errors = new List<string> { "Ngày checkin không được để trống" }
                    //    });
                    //}
                    //DateTime checkinDate;
                    //bool isValidCheckinDate = DateTime.TryParseExact(checkinDateValue.Trim(), SiteConst.Format.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out checkinDate);
                    //if (!isValidCheckinDate)
                    //{
                    //    //write trace log 
                    //    LogModel.Result = ActionResultValue.InvalidInput;
                    //    LogModel.Message = "Ngày checkin phải có dạng ngày tháng năm";
                    //    Logger.LogWarning(LogModel.ToString());

                    //    return Json(new
                    //    {
                    //        Result = 400,
                    //        Errors = new List<string> { "Ngày checkin phải có dạng ngày tháng năm" }
                    //    });
                    //}
                    #endregion

                    var nonEmptyDataRows = excelWorkbook.Worksheet(1).RowsUsed().Skip(4);
                    foreach (var dataRow in nonEmptyDataRows)
                    {
                        stt++;
                        cellNo = 2;
                        //skip telesale info
                        cellNo++;

                        #region Investor resource
                        string investorResource = dataRow.Cell(cellNo).Value.ToString();
                        if (string.IsNullOrEmpty(investorResource))
                        {
                            //write trace log 
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Tên nguồn không được để trống ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Tên nguồn không được để trống ở dòng " + stt }
                            });
                        }
                        var getResourceInfo = iWhere.GetByName(investorResource.Trim());
                        if (getResourceInfo.Error || getResourceInfo.Result == null || getResourceInfo.Result.Count == 0)
                        {
                            //write trace log 
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Không tìm thấy thông tin nguồn khách hàng ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Không tìm thấy thông tin nguồn khách hàng ở dòng " + stt }
                            });
                        }
                        if (getResourceInfo.Result.Count > 1)
                        {
                            //write trace log 
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Có nhiều hơn 1 nguồn khách hàng trùng tên ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Có nhiều hơn 1 nguồn khách hàng trùng tên ở dòng " + stt + ". Vui lòng liên hệ tới admin để được hỗ trợ." }
                            });
                        }
                        #endregion

                        cellNo++;

                        string investorName = dataRow.Cell(cellNo).Value.ToString();
                        if (string.IsNullOrEmpty(investorName))
                        {
                            //write trace log 
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Họ tên khách hàng không được để trống ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Họ tên khách hàng không được để trống ở dòng " + stt }
                            });
                        }

                        cellNo++;

                        string goWithInvestorName = dataRow.Cell(cellNo).Value.ToString();

                        cellNo++;

                        string investorPhone = dataRow.Cell(cellNo).Value.ToString();
                        if (string.IsNullOrEmpty(investorPhone))
                        {
                            //write trace log 
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Số điện thoại khách hàng không được để trống ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Số điện thoại khách hàng không được để trống ở dòng " + stt }
                            });
                        }
                        var getCheckInfoResult = _iHis.GetInfoByPhoneNumber(investorPhone);
                        if (!getCheckInfoResult.Error && getCheckInfoResult.Result != null)
                        {
                            var checkinInfo = getCheckInfoResult.Result
                                .Where(x => x.ShowStartTime.Date == getEventInfo.Result.First().EventTime.Value.Date
                                    && x.CodeEvent != getEventInfo.Result.First().CodeEvent).FirstOrDefault();
                            if (checkinInfo != null)
                            {
                                return Json(new
                                {
                                    Result = 400,
                                    Errors = new List<string> { "Khách hàng đã được ghi nhận tham gia show up trong ngày tổ chức sự kiện" }
                                });
                            }
                        }

                        cellNo++;

                        #region Go with the investor
                        string goWithInvestorPhone = dataRow.Cell(cellNo).Value.ToString();
                        List<GoWithInvestorModel> goWithList = null;
                        if (!string.IsNullOrEmpty(goWithInvestorName) && !string.IsNullOrEmpty(goWithInvestorPhone))
                        {
                            List<string> goWithNames = goWithInvestorName.Trim().Split(SiteConst.CommaChar).ToList();
                            List<string> goWithPhones = goWithInvestorPhone.Trim().Split(SiteConst.CommaChar).ToList();
                            if (goWithNames.Count != goWithPhones.Count)
                            {
                                //write trace log 
                                LogModel.Result = ActionResultValue.InvalidInput;
                                LogModel.Message = "Nhập đầy đủ họ tên và số điện thoại của khách hàng đi cùng không được để trống ở dòng " + stt;
                                Logger.LogWarning(LogModel.ToString());

                                return Json(new
                                {
                                    Result = 400,
                                    Errors = new List<string> { "Nhập đầy đủ họ tên và số điện thoại của khách hàng đi cùng không được để trống ở dòng " + stt }
                                });
                            }
                            else
                            {
                                goWithList = new List<GoWithInvestorModel>();
                                for (int i = 0; i < goWithNames.Count; i++)
                                {
                                    goWithList.Add(new GoWithInvestorModel
                                    {
                                        Name = goWithNames[i],
                                        PhoneNumber = goWithPhones[i],
                                    });
                                }
                            }
                        }
                        #endregion

                        cellNo++;

                        string idCard = dataRow.Cell(cellNo).Value.ToString();

                        cellNo++;

                        string investorDOB = dataRow.Cell(cellNo).Value.ToString();
                        if (string.IsNullOrEmpty(investorDOB))
                        {
                            //write trace log 
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Ngày sinh khách hàng không được để trống ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Ngày sinh khách hàng không được để trống ở dòng " + stt }
                            });
                        }
                        DateTime dob;
                        bool isValidDob = DateTime.TryParseExact(investorDOB.Trim(), SiteConst.Format.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dob);
                        if (!isValidDob)
                        {
                            //write trace log 
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Ngày sinh khách hàng phải có dạng ngày tháng năm ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Ngày sinh khách hàng phải có dạng ngày tháng năm ở dòng " + stt }
                            });
                        }

                        cellNo++;

                        string joinedObject = dataRow.Cell(cellNo).Value.ToString();
                        //if (string.IsNullOrEmpty(joinedObject))
                        //{
                        //    return Json(new
                        //    {
                        //        Result = 400,
                        //        Errors = new List<string> { "Tình trạng tham dự không được để trống ở dòng " + stt }
                        //    });
                        //}

                        cellNo++;

                        string table = dataRow.Cell(cellNo).Value.ToString();

                        cellNo++;

                        string checkin = dataRow.Cell(cellNo).RichText.Text;
                        //DateTime? checkinTime = null;
                        if (!string.IsNullOrEmpty(checkin))
                        {
                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Không được nhập thời gian checkin ở dòng " + stt }
                            });
                        }

                        cellNo++;

                        string checkout = dataRow.Cell(cellNo).RichText.Text;
                        //DateTime? checkoutTime = null;
                        if (!string.IsNullOrEmpty(checkout))
                        {
                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Không được nhập thời gian checkout ở dòng " + stt }
                            });
                        }

                        // ingore sale manager
                        cellNo++;
                        cellNo++;

                        #region Sale
                        string sale = dataRow.Cell(cellNo).Value.ToString();
                        if (!string.IsNullOrEmpty(sale))
                        {
                            sale = sale.Trim();
                            if (sale.Contains("_"))
                            {
                                //get sale info by staff code
                                var staffCode = sale.Split("_")[1];
                                var getStaffInfo = iAcc.GetEmployeeInfoByStaffCode(staffCode);
                                if (getStaffInfo.Error || getStaffInfo.Result == null || getStaffInfo.Result.Count == 0)
                                {
                                    //write trace log 
                                    LogModel.Result = ActionResultValue.InvalidInput;
                                    LogModel.Message = "Không tìm thấy thông tin nhân viên capital consultant ở dòng " + stt;
                                    Logger.LogWarning(LogModel.ToString());

                                    return Json(new
                                    {
                                        Result = 400,
                                        Errors = new List<string> { "Không tìm thấy thông tin nhân viên capital consultant ở dòng " + stt }
                                    });
                                }

                                //check same fullname
                                if (getStaffInfo.Result.First().FullName.ToLower() != sale.Split("_")[0].ToLower())
                                {
                                    //write trace log 
                                    LogModel.Result = ActionResultValue.InvalidInput;
                                    LogModel.Message = "Họ tên capital consultant không trùng khớp với nhân viên có mã đã nhập ở dòng " + stt;
                                    Logger.LogWarning(LogModel.ToString());

                                    return Json(new
                                    {
                                        Result = 400,
                                        Errors = new List<string> { "Họ tên capital consultant không trùng khớp với nhân viên có mã đã nhập ở dòng " + stt }
                                    });
                                }

                                //check same branch
                                if (getStaffInfo.Result.First().BranchCode != tokenModel.BranchCode)
                                {
                                    //write trace log 
                                    LogModel.Result = ActionResultValue.InvalidInput;
                                    LogModel.Message = "Nhân viên capital consultant không thuộc chi nhánh này ở dòng " + stt;
                                    Logger.LogWarning(LogModel.ToString());

                                    return Json(new
                                    {
                                        Result = 400,
                                        Errors = new List<string> { "Nhân viên capital consultant không thuộc chi nhánh này ở dòng " + stt }
                                    });
                                }

                                sale = getStaffInfo.Result.First().CodeStaff;
                            }
                            else
                            {
                                //get sale info by name
                                var getStaffInfo = iAcc.GetEmployeeListByTypeAndName(false, sale);
                                if (getStaffInfo.Error || getStaffInfo.Result == null || getStaffInfo.Result.Count == 0)
                                {
                                    //write trace log 
                                    LogModel.Result = ActionResultValue.InvalidInput;
                                    LogModel.Message = "Không tìm thấy thông tin nhân viên capital consultant ở dòng " + stt;
                                    Logger.LogWarning(LogModel.ToString());

                                    return Json(new
                                    {
                                        Result = 400,
                                        Errors = new List<string> { "Không tìm thấy thông tin nhân viên capital consultant ở dòng " + stt }
                                    });
                                }

                                // check number of found result
                                if (getStaffInfo.Result.Count > 1)
                                {
                                    //write trace log 
                                    LogModel.Result = ActionResultValue.InvalidInput;
                                    LogModel.Message = "Có nhiều hơn 1 capital consultant trùng tên ở dòng " + stt + ". Thêm mã nhân viên sau họ tên phân cách bằng dấu \"_\" để hệ thống tiếp nhận đúng thông tin";
                                    Logger.LogWarning(LogModel.ToString());

                                    return Json(new
                                    {
                                        Result = 400,
                                        Errors = new List<string> { "Có nhiều hơn 1 capital consultant trùng tên ở dòng " + stt + ". Thêm mã nhân viên sau họ tên phân cách bằng dấu \"_\" để hệ thống tiếp nhận đúng thông tin" }
                                    });
                                }

                                //check same info
                                if (getStaffInfo.Result.First().BranchCode != tokenModel.BranchCode)
                                {
                                    //write trace log 
                                    LogModel.Result = ActionResultValue.InvalidInput;
                                    LogModel.Message = "Nhân viên capital consultant không thuộc chi nhánh này ở dòng " + stt;
                                    Logger.LogWarning(LogModel.ToString());

                                    return Json(new
                                    {
                                        Result = 400,
                                        Errors = new List<string> { "Nhân viên capital consultant không thuộc chi nhánh này ở dòng " + stt }
                                    });
                                }

                                sale = getStaffInfo.Result.First().CodeStaff;
                            }
                        }
                        else
                        {
                            //write trace log 
                            LogModel.Result = ActionResultValue.InvalidInput;
                            LogModel.Message = "Chưa chọn capital consultant liên hệ khách ở dòng " + stt;
                            Logger.LogWarning(LogModel.ToString());

                            return Json(new
                            {
                                Result = 400,
                                Errors = new List<string> { "Chưa chọn capital consultant liên hệ khách ở dòng " + stt }
                            });
                        }
                        #endregion

                        cellNo++;

                        #region SaleTo
                        string saleTO = dataRow.Cell(cellNo).Value.ToString();
                        if (!string.IsNullOrEmpty(saleTO))
                        {
                            saleTO = saleTO.Trim();
                            if (saleTO.Contains("_"))
                            {
                                //get saleTO info by staff code
                                var staffCode = saleTO.Split("_")[1];
                                var getStaffInfo = iAcc.GetEmployeeInfoByStaffCode(staffCode);
                                if (getStaffInfo.Error || getStaffInfo.Result == null || getStaffInfo.Result.Count == 0)
                                {
                                    //write trace log 
                                    LogModel.Result = ActionResultValue.InvalidInput;
                                    LogModel.Message = "Không tìm thấy thông tin nhân viên capital leader chốt hộ ở dòng " + stt;
                                    Logger.LogWarning(LogModel.ToString());

                                    return Json(new
                                    {
                                        Result = 400,
                                        Errors = new List<string> { "Không tìm thấy thông tin nhân viên capital leader chốt hộ ở dòng " + stt }
                                    });
                                }

                                //check same fullname
                                if (getStaffInfo.Result.First().FullName.ToLower() != saleTO.Split("_")[0].ToLower())
                                {
                                    //write trace log 
                                    LogModel.Result = ActionResultValue.InvalidInput;
                                    LogModel.Message = "Họ tên capital leader không trùng khớp với nhân viên có mã đã nhập ở dòng " + stt;
                                    Logger.LogWarning(LogModel.ToString());

                                    return Json(new
                                    {
                                        Result = 400,
                                        Errors = new List<string> { "Họ tên capital leader không trùng khớp với nhân viên có mã đã nhập ở dòng " + stt }
                                    });
                                }

                                //check same branch
                                if (getStaffInfo.Result.First().BranchCode != tokenModel.BranchCode)
                                {
                                    //write trace log 
                                    LogModel.Result = ActionResultValue.InvalidInput;
                                    LogModel.Message = "Nhân viên capital leader chốt hộ không thuộc chi nhánh này ở dòng " + stt;
                                    Logger.LogWarning(LogModel.ToString());

                                    return Json(new
                                    {
                                        Result = 400,
                                        Errors = new List<string> { "Nhân viên capital leader chốt hộ không thuộc chi nhánh này ở dòng " + stt }
                                    });
                                }

                                saleTO = getStaffInfo.Result.First().CodeStaff;
                            }
                            else
                            {
                                //get sale TO info by name
                                var getStaffInfo = iAcc.GetEmployeeListByTypeAndName(false, saleTO);
                                if (getStaffInfo.Error || getStaffInfo.Result == null || getStaffInfo.Result.Count == 0)
                                {
                                    //write trace log 
                                    LogModel.Result = ActionResultValue.InvalidInput;
                                    LogModel.Message = "Không tìm thấy thông tin nhân viên capital leader chốt hộ ở dòng " + stt;
                                    Logger.LogWarning(LogModel.ToString());

                                    return Json(new
                                    {
                                        Result = 400,
                                        Errors = new List<string> { "Không tìm thấy thông tin nhân viên capital leader chốt hộ ở dòng " + stt }
                                    });
                                }

                                //check number of found result
                                if (getStaffInfo.Result.Count > 1)
                                {
                                    //write trace log 
                                    LogModel.Result = ActionResultValue.InvalidInput;
                                    LogModel.Message = "Có nhiều hơn 1 capital leader chốt hộ trùng tên ở dòng " + stt + ". Thêm mã nhân viên sau họ tên phân cách bằng dấu \"_\" để hệ thống tiếp nhận đúng thông tin";
                                    Logger.LogWarning(LogModel.ToString());

                                    return Json(new
                                    {
                                        Result = 400,
                                        Errors = new List<string> { "Có nhiều hơn 1 capital leader chốt hộ trùng tên ở dòng " + stt + ". Thêm mã nhân viên sau họ tên phân cách bằng dấu \"_\" để hệ thống tiếp nhận đúng thông tin" }
                                    });
                                }

                                //check same branch
                                if (getStaffInfo.Result.First().BranchCode != tokenModel.BranchCode)
                                {
                                    //write trace log 
                                    LogModel.Result = ActionResultValue.InvalidInput;
                                    LogModel.Message = "Nhân viên capital leader chốt hộ không thuộc chi nhánh này ở dòng " + stt;
                                    Logger.LogWarning(LogModel.ToString());

                                    return Json(new
                                    {
                                        Result = 400,
                                        Errors = new List<string> { "Nhân viên capital leader chốt hộ không thuộc chi nhánh này ở dòng " + stt }
                                    });
                                }

                                saleTO = getStaffInfo.Result.First().CodeStaff;
                            }
                        }
                        #endregion

                        cellNo++;

                        string gift = dataRow.Cell(cellNo).Value.ToString();

                        cellNo++;

                        #region Contract
                        string contractValue = dataRow.Cell(cellNo).Value.ToString();
                        decimal? value = null;
                        if (!string.IsNullOrEmpty(contractValue))
                        {
                            decimal money;
                            bool isValidContractValue = decimal.TryParse(contractValue.Trim(), out money);
                            if (!isValidContractValue)
                            {
                                //write trace log 
                                LogModel.Result = ActionResultValue.InvalidInput;
                                LogModel.Message = "Giá trị hợp đồng không đúng định dạng số ở dòng " + stt;
                                Logger.LogWarning(LogModel.ToString());

                                return Json(new
                                {
                                    Result = 400,
                                    Errors = new List<string> { "Giá trị hợp đồng không đúng định dạng số ở dòng " + stt }
                                });
                            }
                            value = money;
                        }

                        cellNo++;

                        string depositValue = dataRow.Cell(cellNo).Value.ToString();
                        decimal? deposit = null;
                        if (!string.IsNullOrEmpty(depositValue))
                        {
                            decimal money;
                            bool isValidContractValue = decimal.TryParse(depositValue.Trim(), out money);
                            if (!isValidContractValue)
                            {
                                //write trace log 
                                LogModel.Result = ActionResultValue.InvalidInput;
                                LogModel.Message = "Đặt cọc hợp đồng không đúng định dạng số ở dòng " + stt;
                                Logger.LogWarning(LogModel.ToString());

                                return Json(new
                                {
                                    Result = 400,
                                    Errors = new List<string> { "Đặt cọc hợp đồng không đúng định dạng số ở dòng " + stt }
                                });
                            }
                            deposit = money;
                        }
                        #endregion

                        cellNo++;

                        string note = dataRow.Cell(cellNo).Value.ToString();

                        ShowUpHistoryCreateViewModel model = new ShowUpHistoryCreateViewModel
                        {
                            InvestorName = investorName,
                            IdCard = idCard,
                            InvestorResourceCode = getResourceInfo.Result.First().InvestorResourceCode,
                            Birthday = dob,
                            CodeEvent = getEventInfo.Result.First().CodeEvent,
                            ContractValue = value,
                            Deposit = deposit,
                            Gift = gift,
                            JoinedObject = JoinedObjectList.GetValueOrDefault(joinedObject.ToUpper()),
                            PhoneNumber = investorPhone,
                            Sale = sale,
                            SaleTO = saleTO,
                            //TeleSale = teleSale,
                            Table = table,
                            Note = note.Trim(),
                            Group = goWithList,
                            //TimeIn = checkinTime,
                            //TimeOut = checkoutTime,
                            CreatedBy = tokenModel.StaffCode,
                        };
                        listData.Add(model);
                    }
                }

                //import data
                var resultUpload = _iHis.ImportCheckin(listData);
                if (resultUpload.Error)
                {
                    //write trace log 
                    LogModel.Result = ActionResultValue.ImportFailed;
                    LogModel.Message = "Nhập thông tin khách hàng dự kiến không thành công!";
                    Logger.LogError(LogModel.ToString());

                    return Json(new
                    {
                        Result = 400,
                        Errors =
                        new List<string> { "Nhập thông tin khách hàng dự kiến không thành công!" }
                    });
                }

            }
            catch (Exception ex)
            {
                //write trace log 
                LogModel.Result = ActionResultValue.ImportFailed;
                LogModel.Message = "Nhập thông tin khách hàng dự kiến không thành công!";
                Logger.LogError(ex, LogModel.ToString());

                return Json(new
                {
                    Result = 400,
                    Errors =
                    new List<string> { "Nhập thông tin khách hàng dự kiến không thành công!" }
                });
            }
            finally
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            //write trace log 
            LogModel.Result = ActionResultValue.ImportSuccess;
            LogModel.Message = "Nhập thông tin khách hàng dự kiến thành công!";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Nhập thông tin khách hàng dự kiến thành công!" });
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(long id)
        {
            //trace log
            LogModel.Data = (new { id = id }).ToDataString();
            LogModel.Action = ActionType.Delete;

            // ViewBag.Id = id;
            var showup = _iHis.GetById(id);
            var handleResult = HandleGetResult(showup);
            if (handleResult != null) return handleResult;

            if (showup.Result.First().TimeIn != null)
            {
                //write trace log
                LogModel.Result = ActionResultValue.DeleteFailed;
                LogModel.Message = "Khách hàng đã checkin nên không thể xóa!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Khách hàng đã checkin nên không thể xóa!" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.AccessSuccess;
            Logger.LogInformation(LogModel.ToString());

            return PartialView("_Delete", showup.Result.First());
        }

        [HttpGet]
        public IActionResult ConfirmDelete(long id)
        {
            //trace log
            LogModel.Data = (new { id = id }).ToDataString();
            LogModel.Action = ActionType.Delete;

            var showup = _iHis.GetById(id);
            var handleResult = HandleGetResult(showup);
            if (handleResult != null) return handleResult;

            var deleteResult = _iHis.Delete(id);
            if (deleteResult.Error)
            {
                //write trace log
                LogModel.Result = ActionResultValue.DeleteFailed;
                LogModel.Message = "Xóa thông tin khách hàng dự kiến không thành công!";
                Logger.LogError(LogModel.ToString());

                return Json(new { Result = 400, Errors = new List<string> { "Xóa thông tin khách hàng dự kiến không thành công!" } });
            }

            //write trace log
            LogModel.Result = ActionResultValue.DeleteSuccess;
            LogModel.Message = "Xóa thông tin khách hàng dự kiến thành công";
            Logger.LogInformation(LogModel.ToString());

            return Json(new { Result = 200, Message = "Xóa thông tin khách hàng dự kiến thành công" });
        }
        #endregion
    }
}
