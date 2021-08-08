using CRMBussiness.IService;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using CRMModel.Models.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CRMBussiness.ServiceImp
{
    public class EventImp : BaseService<Event, long>, IEvent
    {
        private const string _FixedCode = "EV";

        #region GetBestCode
        private DataResult<EventViewModel> GetBestCode(DateTime date)
        {
            try
            {
                var lst = this.Procedure<EventViewModel>("sp_tblEvent_GetBestCode", new { @Date = date.Date }).ToList();
                return new DataResult<EventViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<EventViewModel> { Error = true };
            }
        }

        public string GetEventCode(bool isForCC, DateTime date)
        {
            string dayInMonth = date.ToString("ddMM");
            string nextCode = null;
            if (isForCC)
            {
                nextCode = dayInMonth + _FixedCode + "CC";
            }
            else
            {
                var getMaxCodeResult = GetBestCode(date);
                if (getMaxCodeResult.Error)
                {
                    return null;
                }
                else if (getMaxCodeResult.Result == null || getMaxCodeResult.Result.Count == 0)
                {
                    nextCode = dayInMonth + _FixedCode + "01";
                }
                else
                {
                    var LastEventCode = getMaxCodeResult.Result.First().CodeEvent;
                    if (LastEventCode.Contains(dayInMonth))
                    {
                        int EventNo = int.Parse(LastEventCode.Split(_FixedCode)[1]);

                        nextCode = dayInMonth + _FixedCode + (EventNo + 1 <= 9 ? "0" : string.Empty) + (EventNo + 1);
                    }
                    else
                    {
                        nextCode = dayInMonth + _FixedCode + "01";
                    }
                }
            }
            return nextCode;
        }
        #endregion

        #region Create
        public DataResult<EventViewModel> Create(EventViewModel model)
        {
            if (model.CreatedForCC)
            {
                model.Name = "Sự kiện " + model.EventTime?.ToString("dd/MM/yyyy");
            }
            model.CodeEvent = GetEventCode(model.CreatedForCC, model.EventTime.Value);
            if (string.IsNullOrEmpty(model.CodeEvent)) 
            {
                return new DataResult<EventViewModel> { Error = true };
            }
            
            Event eventModel = new Event();
            eventModel.CodeEvent = model.CodeEvent;
            eventModel.EventTime = model.EventTime;
            eventModel.EndTime = model.EndTime;
            //eventModel.Name = "SHOWUP" + model.EventTime?.ToString("dd/MM/yyyy");
            eventModel.Name = model.Name;
            eventModel.ProductCode = model.ProductCode;
            eventModel.Status = model.Status;
            eventModel.CreatedBy = model.CreatedBy;

            try
            {
                Raw_Insert(eventModel);

                return new DataResult<EventViewModel>();
            }
            catch
            {
                return new DataResult<EventViewModel> { Error = true };
            }
        }
        #endregion

        #region Delete
        public DataResult<EventViewModel> Delete(long id)
        {
            try
            {
                Raw_Delete(id.ToString());

                return new DataResult<EventViewModel>();
            }
            catch
            {
                return new DataResult<EventViewModel> { Error = true };
            }
        }
        #endregion

        #region GetList
        public DataResult<EventViewModel> GetList(SearchEventModel model, out int total)
        {
            total = 0;
            List<EventViewModel> events = new List<EventViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", model.Key ?? string.Empty);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@StartDate", model.StartDate);
            param.Add("@EndDate", model.EndDate);
            param.Add("@Status", model.Status);
            param.Add("@BranchCode", model.Branch);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            try
            {
                events = this.Procedure<EventViewModel>("sp_tblEvent_GetList", param).ToList();
                total = param.Get<int>("@Total");
            }
            catch
            {
                return new DataResult<EventViewModel> { Error = true };
            }

            return new DataResult<EventViewModel> { Result = events };
        }
        #endregion

        #region Update
        public DataResult<EventViewModel> Update(EventViewModel model)
        {
            Event eventModel = new Event();
            eventModel.Id = model.Id;
            eventModel.CodeEvent = model.CodeEvent;
            eventModel.EventTime = model.EventTime;
            eventModel.EndTime = model.EndTime;
            //eventModel.Name = "SHOWUP" + model.EventTime?.ToString("dd/MM/yyyy");
            eventModel.Name = model.Name;
            eventModel.ProductCode = model.ProductCode;
            eventModel.Status = model.Status;
            eventModel.CreatedBy = model.CreatedBy;

            try
            {
                Raw_Update(eventModel);

                return new DataResult<EventViewModel>();
            }
            catch
            {
                return new DataResult<EventViewModel> { Error = true };
            }
        }
        #endregion

        #region Get
        public DataResult<EventViewModel> Get(long id)
        {
            try
            {
                var lst = this.Procedure<EventViewModel>("sp_tblEvent_GetById", new { @Id = id }).ToList();
                return new DataResult<EventViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<EventViewModel> { Error = true };
            }
        }
        #endregion

        #region GetEventList
        public DataResult<EventViewModel> GetEventList(string branch)
        {
            try
            {
                var lst = this.Procedure<EventViewModel>("sp_tblEvent_GetShowUpList", new { @BranchCode = branch }).ToList();
                return new DataResult<EventViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<EventViewModel> { Error = true };
            }
        }
        #endregion

        #region GetByShowUpCode
        public DataResult<EventViewModel> GetByShowUpCode(string code)
        {
            try
            {
                var lst = this.Procedure<EventViewModel>("sp_tblEvent_GetByShowUpCode", new { @code = code }).ToList();
                return new DataResult<EventViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<EventViewModel> { Error = true };
            }
        }
        #endregion

        #region GetByName
        public DataResult<EventViewModel> GetByName(string name)
        {
            try
            {
                var lst = this.Procedure<EventViewModel>("sp_tblEvent_GetByName", new { @Name = name }).ToList();
                return new DataResult<EventViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<EventViewModel> { Error = true };
            }
        }
        #endregion

        #region GetNearestEvent
        public DataResult<EventViewModel> GetNearestEvent(bool? forSale, string branch)
        {
            try
            {
                var lst = this.Procedure<EventViewModel>("sp_tblEvent_GetNextEvent", new { @ForSale = forSale, @BranchCode = branch }).ToList();
                return new DataResult<EventViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<EventViewModel> { Error = true };
            }
        }

        public DataResult<EventViewModel> GetEventByProductCode(string ProductCode)
        {
            try
            {
                var data = this.Raw_Query<EventViewModel>("SELECT Id, CodeEvent, Name FROM tblEvent WHERE Status = 1").ToList();
                return new DataResult<EventViewModel> { Result = data };
            }
            catch (Exception ex)
            {

                return new DataResult<EventViewModel> { Error = true };
            }
        }
        #endregion
    }
}