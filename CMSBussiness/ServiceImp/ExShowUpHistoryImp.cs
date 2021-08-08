using CRMBussiness.IService;
using CRMBussiness.LIB;
using CRMBussiness.ViewModel;
using CRMModel.Models.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CRMBussiness.ServiceImp
{
    public class ExShowUpHistoryImp : BaseService<ShowUpHistory, long>, IShowUpHistory
    {
        #region Const
        private const string InsertGoWithInvestorQuery = @"INSERT INTO tblInvestors(CodeInvestor, Name, PhoneNumber, CodeShowUpWithGroup, Status) VALUES(@CodeInvestor, @Name, @PhoneNumber, @CodeShowUpWithGroup, 0)";
        private const string InsertInvestorQuery = @"INSERT INTO tblInvestors(CodeInvestor, Name, PhoneNumber, IdCard, Birthday, Status) VALUES(@CodeInvestor, @Name, @PhoneNumber, @IdCard, @Birthday, @Status)";
        private const string InsertCheckInQuery = @"INSERT INTO tblShowUpHistory(CodeShow, CodeEvent, CodeInvestor, CodeStaff, InvestorResourceCode, [Table], Note, Sale, SaleTO, ContractValue, Deposit, JoinedObject, [Status], Gift, TimeIn, TimeOut, IsDirectAddition, CreatedBy) VALUES(@CodeShow, @CodeEvent, @CodeInvestor, @CodeStaff, @InvestorResourceCode, @Table, @Note, @Sale, @SaleTO, @ContractValue, @Deposit, @JoinedObject, @Status, @Gift, @TimeIn, @TimeOut, @IsDirectAddition, @CreatedBy)";
        private const string UpdateCheckInQuery = @"UPDATE tblShowUpHistory SET CodeEvent = @CodeEvent, CodeStaff = @CodeStaff, InvestorResourceCode = @InvestorResourceCode, [Table] = @Table, Note = @Note, Sale = @Sale, SaleTO = @SaleTO, ContractValue = @ContractValue, Deposit = @Deposit, JoinedObject = @JoinedObject, Gift = @Gift WHERE Id = @Id";
        private const string UpdateInvestorQuery = @"UPDATE tblInvestors SET Name = @Name, PhoneNumber = @PhoneNumber, IdCard = @IdCard, Birthday = @Birthday WHERE CodeInvestor = @CodeInvestor";
        //private const string DeleteGoWithInvestorQuery = @"DELETE FROM tblInvestors WHERE CodeShowUpWithGroup = @CodeShowUpWithGroup";
        private const string CheckInQuery = @"UPDATE tblShowUpHistory SET TimeIn = @TimeIn WHERE Id = @Id";
        private const string CheckOutQuery = @"UPDATE tblShowUpHistory SET TimeOut = @TimeOut WHERE Id = @Id";
        private const string RemoveCheckinQuery = @"UPDATE tblShowUpHistory SET TimeOut = NULL, TimeIn = NULL WHERE Id = @Id";
        private const string UpdateInvestorGoWithQuery = @"UPDATE tblInvestors SET Name = @Name, CodeShowUpWithGroup = @CodeShowUpWithGroup WHERE CodeInvestor = @CodeInvestor";
        private const string DeleteShowUpInfo = "DELETE FROM tblShowUpHistory WHERE Id = @id";
        private const string DeleteInvestor = "DELETE FROM tblInvestors WHERE CodeInvestor = @CodeInvestor";
        #endregion

        #region CheckIn
        public DataResult<ShowUpHistoryTableViewModel> CheckIn(long id)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Execute(CheckInQuery, new { @TimeIn = DateTime.Now, @Id = id });
                }

                return new DataResult<ShowUpHistoryTableViewModel>();
            }
            catch
            {
                return new DataResult<ShowUpHistoryTableViewModel> { Error = true };
            }
        }
        #endregion

        #region CheckOut
        public DataResult<ShowUpHistoryTableViewModel> CheckOut(long id)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Execute(CheckOutQuery, new { @TimeOut = DateTime.Now, @Id = id });
                }

                return new DataResult<ShowUpHistoryTableViewModel>();
            }
            catch
            {
                return new DataResult<ShowUpHistoryTableViewModel> { Error = true };
            }
        }
        #endregion

        #region GetById
        public DataResult<ShowUpHistoryCreateViewModel> GetById(long id)
        {
            try
            {
                var lst = this.Procedure<ShowUpHistoryCreateViewModel>("sp_tblShowUpHistory_GetById", new { @id = id }).ToList();
                return new DataResult<ShowUpHistoryCreateViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<ShowUpHistoryCreateViewModel> { Error = true };
            }
        }
        #endregion

        #region GetList
        public DataResult<ShowUpHistoryTableViewModel> GetList(SearchShowUpHistoryModel model, out int total)
        {
            List<ShowUpHistoryTableViewModel> data = new List<ShowUpHistoryTableViewModel>();
            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", model.Key ?? string.Empty);
            param.Add("@Page", model.Page);
            param.Add("@Size", model.Size);
            param.Add("@ShowUp", model.ShowUp);
            param.Add("@TeleSale", model.TeleSale);
            param.Add("@CC", model.Sale);
            param.Add("@Branch", model.Branch);
            param.Add("@CreatedBy", model.CreatedBy);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
            total = 0;
            try
            {
                var lst = this.Procedure<ShowUpHistoryTableViewModel>("sp_tblShowUpHistory_GetExpectedtList", param).ToList();
                total = param.Get<int>("Total");
                return new DataResult<ShowUpHistoryTableViewModel> { Result = lst };
            }
            catch (Exception ex)
            {
                return new DataResult<ShowUpHistoryTableViewModel> { Error = true };
            }
        }
        #endregion

        #region CreateCheckIn
        public DataResult<ShowUpHistoryTableViewModel> CreateCheckIn(ShowUpHistoryCreateViewModel model)
        {
            Investor investor = new Investor()
            {
                Name = model.InvestorName,
                //CodeInvestor = model.CodeEvent + "P",
                Birthday = model.Birthday,
                PhoneNumber = model.PhoneNumber,
                IdCard = model.IdCard,
                Status = 0,
            };

            ShowUpHistory history = new ShowUpHistory
            {
                CodeShow = Guid.NewGuid().ToString(),
                CodeEvent = model.CodeEvent,
                //CodeInvestor = investor.CodeInvestor,
                InvestorResourceCode = model.InvestorResourceCode,
                Note = model.Note,
                Table = model.Table,
                CodeStaff = model.TeleSale,
                Status = true,
                Sale = model.Sale,
                SaleTO = model.SaleTO,
                ContractValue = model.ContractValue,
                Deposit = model.Deposit,
                JoinedObject = model.JoinedObject,
                Gift = model.Gift,
                IsDirectAddition = false,
                CreatedBy = model.CreatedBy,
            };

            List<GoWithInvestorModel> personList = new List<GoWithInvestorModel>();
            if (model.Group != null)
            {
                int stt = 0;
                foreach (var item in model.Group)
                {
                    stt++;
                    GoWithInvestorModel investorModel = new GoWithInvestorModel
                    {
                        //CodeInvestor = model.CodeEvent + "S" + stt,
                        Name = item.Name,
                        PhoneNumber = item.PhoneNumber,
                        CodeShowUpWithGroup = history.CodeShow,
                    };
                    personList.Add(investorModel);
                }
            }

            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {

                        // add investor
                        InvestorViewModel existedInvestor = IsExistedInvestorInfo(investor.PhoneNumber, investor.Id);
                        if (existedInvestor == null)
                        {
                            using (var getResult = db.QueryMultiple("sp_tblInvestors_GetLastInfo", new { @EventCode = model.CodeEvent }, transaction, commandType: CommandType.StoredProcedure))
                            {
                                var lastInvestor = getResult.Read<Investor>().FirstOrDefault();
                                if (lastInvestor == null || !lastInvestor.CodeInvestor.Contains(model.CodeEvent))
                                {
                                    investor.CodeInvestor = model.CodeEvent + "P001";
                                }
                                else
                                {
                                    int stt = int.Parse(lastInvestor.CodeInvestor.Substring(lastInvestor.CodeInvestor.Length-3));
                                    string investorNo = null;
                                    if (stt + 1 < 10)
                                    {
                                        investorNo += "00";
                                    }
                                    else if (stt + 1 < 100)
                                    {
                                        investorNo += "0";
                                    }
                                    investorNo += (stt + 1);
                                    investor.CodeInvestor = model.CodeEvent + "P" + investorNo;
                                }
                            }

                            db.Execute(InsertInvestorQuery, investor, transaction);
                        }
                        else
                        {
                            investor.CodeInvestor = existedInvestor.CodeInvestor;
                            db.Execute(UpdateInvestorQuery, investor, transaction);
                        }

                        // add checkin
                        history.CodeInvestor = investor.CodeInvestor;
                        db.Execute(InsertCheckInQuery, history, transaction);

                        // add go with person
                        if (personList.Count > 0)
                        {
                            int stt = 0;
                            foreach (var person in personList)
                            {
                                stt++;
                                using (var multipleresult = db.QueryMultiple("sp_tblInvestors_GetInfo_ByPhone",
                                        new { @phone = person.PhoneNumber, @id = person.Id }, transaction,
                                        commandType: CommandType.StoredProcedure))
                                {
                                    var existedGoWithInvestor = multipleresult.Read<InvestorViewModel>().FirstOrDefault();
                                    if (existedGoWithInvestor is null)
                                    {
                                        person.CodeInvestor =
                                            investor.CodeInvestor + "S" + (stt < 10 ? "0" : string.Empty) + stt;
                                        db.Execute(InsertGoWithInvestorQuery, person, transaction);
                                    }
                                    else
                                    {
                                        existedGoWithInvestor.Name = person.Name;
                                        if (string.IsNullOrEmpty(existedGoWithInvestor.CodeShowUpWithGroup))
                                        {
                                            existedGoWithInvestor.CodeShowUpWithGroup = history.CodeShow;
                                        }
                                        else if (!existedGoWithInvestor.CodeShowUpWithGroup.Contains(history.CodeShow))
                                        {
                                            existedGoWithInvestor.CodeShowUpWithGroup =
                                                existedGoWithInvestor.CodeShowUpWithGroup + "," + history.CodeShow;
                                        }
                                        db.Execute(UpdateInvestorGoWithQuery, existedGoWithInvestor, transaction);
                                    }
                                }
                            }
                        }

                        transaction.Commit();
                    }
                }

                return new DataResult<ShowUpHistoryTableViewModel>();
            }
            catch (Exception ex)
            {
                return new DataResult<ShowUpHistoryTableViewModel> { Error = true };
            }
        }
        #endregion

        #region UpdateCheckIn
        public DataResult<ShowUpHistoryTableViewModel> UpdateCheckIn(ShowUpHistoryCreateViewModel model)
        {
            Investor investor = new Investor()
            {
                //CodeInvestor = model.CodeEvent + "P",
                CodeInvestor = model.CodeInvestor,
                Name = model.InvestorName,
                Birthday = model.Birthday,
                PhoneNumber = model.PhoneNumber,
                IdCard = model.IdCard,
                Status = 0,
            };

            ShowUpHistory history = new ShowUpHistory
            {
                Id = model.Id,
                CodeShow = model.CodeShow,
                CodeEvent = model.CodeEvent,
                CodeInvestor = investor.CodeInvestor,
                InvestorResourceCode = model.InvestorResourceCode,
                Note = model.Note,
                Table = model.Table,
                CodeStaff = model.TeleSale,
                Sale = model.Sale,
                SaleTO = model.SaleTO,
                ContractValue = model.ContractValue,
                Deposit = model.Deposit,
                JoinedObject = model.JoinedObject,
                Status = true,
                UserUpdate = model.UserUpdate,
                Gift = model.Gift,
            };

            List<GoWithInvestorModel> personList = new List<GoWithInvestorModel>();
            if (model.Group != null)
            {
                int stt = 0;
                foreach (var item in model.Group)
                {
                     stt++;
                    GoWithInvestorModel investorModel = new GoWithInvestorModel
                    {
                        //CodeInvestor = model.CodeEvent + "S" + stt,
                        Name = item.Name,
                        PhoneNumber = item.PhoneNumber,
                        CodeShowUpWithGroup = history.CodeShow,
                    };
                    personList.Add(investorModel);
                }
            }

            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        //update investor
                        InvestorViewModel existedInvestor = IsExistedInvestorInfo(investor.PhoneNumber, investor.Id);
                        if (existedInvestor != null)
                        {
                            history.CodeInvestor = existedInvestor.CodeInvestor;
                            investor.CodeInvestor = existedInvestor.CodeInvestor;
                        }
                        db.Execute(UpdateInvestorQuery, investor, transaction);

                        // update checkin
                        db.Execute(UpdateCheckInQuery, history, transaction);

                        // update go with person
                        if (personList.Count > 0)
                        {
                            //delete some go with primary investor
                            //get showup info by id
                            using (var getResult = db.QueryMultiple("sp_tblShowUpHistory_GetById",
                                new { @id = model.Id }, transaction, commandType: CommandType.StoredProcedure))
                            {
                                var showUp = getResult.Read<ShowUpHistoryCreateViewModel>().FirstOrDefault();

                                // get investor info who go with primary investor
                                using (var getGoWithInvestorResult = db.QueryMultiple("sp_tblInvestor_GetGoWithByCheckInCode",
                                new
                                {
                                    @code = showUp.CodeShow
                                },
                                transaction,
                                commandType: CommandType.StoredProcedure))
                                {
                                    var listGoWiths = getGoWithInvestorResult.Read<GoWithInvestorModel>().ToList();
                                    foreach (var item in listGoWiths)
                                    {
                                        //skip person in above list
                                        if (personList.Select(x => x.PhoneNumber).Contains(item.PhoneNumber)) continue;

                                        if (item.CodeShowUpWithGroup == showUp.CodeShow)
                                        {
                                            //check số lần tham dự show up của khách đi cùng
                                            using (var getPrimaryInvestorResult = db.QueryMultiple("sp_tblShowUpHistory_Get_By_CodeInvestor",
                                                new
                                                {
                                                    @CodeInvestor = showUp.CodeInvestor
                                                },
                                                transaction,
                                                commandType: CommandType.StoredProcedure))
                                            {
                                                var listShows = getPrimaryInvestorResult.Read<ShowUpHistoryCreateViewModel>().ToList();
                                                // this is first showup
                                                if (listShows.Count == 1)
                                                {
                                                    using (var getContractResult = db.QueryMultiple("sp_tblContractInvestor_GeInfo_By_CodeInvestor",
                                                            new
                                                            {
                                                                @CodeInvestor = item.CodeInvestor
                                                            },
                                                            transaction,
                                                            commandType: CommandType.StoredProcedure))
                                                    {
                                                        var contractList = getContractResult.Read<ContractInvesterViewModel>().ToList();
                                                        // this investor didn't have any contracts
                                                        if (contractList.Count == 0)
                                                        {
                                                            db.Execute(DeleteInvestor,
                                                            new { @CodeInvestor = item.CodeInvestor },
                                                            transaction);
                                                        }
                                                    }
                                                }
                                            }
                                            continue;
                                        }

                                        if (item.CodeShowUpWithGroup.StartsWith(showUp.CodeShow))
                                        {
                                            db.Execute(UpdateInvestorGoWithQuery,
                                                new
                                                {
                                                    @Name = item.Name,
                                                    @CodeShowUpWithGroup = item.CodeShowUpWithGroup.Replace(showUp.CodeShow +
                                                    ",", string.Empty)
                                                },
                                                transaction);
                                            continue;
                                        }

                                        db.Execute(UpdateInvestorGoWithQuery,
                                            new
                                            {
                                                @Name = item.Name,
                                                @CodeShowUpWithGroup = item.CodeShowUpWithGroup.Replace("," +
                                                showUp.CodeShow, string.Empty)
                                            },
                                            transaction);
                                    }
                                }
                            }

                            int stt = 0;
                            foreach (var person in personList)
                            {
                                stt++;
                                using (var multipleresult = db.QueryMultiple("sp_tblInvestors_GetInfo_ByPhone",
                                        new { @phone = person.PhoneNumber, @id = person.Id }, transaction,
                                        commandType: CommandType.StoredProcedure))
                                {
                                    var existedGoWithInvestor = multipleresult.Read<InvestorViewModel>().FirstOrDefault();
                                    if (existedGoWithInvestor is null)
                                    {
                                        person.CodeInvestor =
                                            investor.CodeInvestor + "S" + (stt < 10 ? "0" : string.Empty) + stt;
                                        db.Execute(InsertGoWithInvestorQuery, person, transaction);
                                    }
                                    else
                                    {
                                        existedGoWithInvestor.Name = person.Name;
                                        if (string.IsNullOrEmpty(existedGoWithInvestor.CodeShowUpWithGroup))
                                        {
                                            existedGoWithInvestor.CodeShowUpWithGroup = history.CodeShow;
                                        }
                                        else if (!existedGoWithInvestor.CodeShowUpWithGroup.Contains(history.CodeShow))
                                        {
                                            existedGoWithInvestor.CodeShowUpWithGroup =
                                                existedGoWithInvestor.CodeShowUpWithGroup + "," + history.CodeShow;
                                        }
                                        db.Execute(UpdateInvestorGoWithQuery, existedGoWithInvestor, transaction);
                                    }
                                }
                            }
                        }
                        transaction.Commit();
                    }
                }

                return new DataResult<ShowUpHistoryTableViewModel>();
            }
            catch
            {
                return new DataResult<ShowUpHistoryTableViewModel> { Error = true };
            }
        }
        #endregion

        #region IsExistedInvestorInfo
        private InvestorViewModel IsExistedInvestorInfo(string phoneNumber, long id)
        {
            var lst = this.Procedure<InvestorViewModel>("sp_tblInvestors_GetInfo_ByPhone",
                new { @phone = phoneNumber, @id = id }).FirstOrDefault();
            return lst;
        }
        #endregion

        #region GetCheckinInfo
        public DataResult<CheckinInfoModel> GetCheckinInfo(long id)
        {
            try
            {
                var lst = this.Procedure<CheckinInfoModel>("sp_tblShowUpHistory_GetCheckinInfo", new { @id = id }).ToList();
                return new DataResult<CheckinInfoModel> { Result = lst };
            }
            catch
            {
                return new DataResult<CheckinInfoModel> { Error = true };
            }
        }
        #endregion

        #region GetCheckinInByShowUpCode
        public DataResult<ShowUpHistoryCreateViewModel> GetCheckinInByShowUpCode(string showUpCode)
        {
            try
            {
                var lst = this.Procedure<ShowUpHistoryCreateViewModel>("sp_tblShowUpHistory_GetByShowUpCode", new { @eventCode = showUpCode }).ToList();
                return new DataResult<ShowUpHistoryCreateViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<ShowUpHistoryCreateViewModel> { Error = true };
            }
        }
        #endregion

        #region GetStaticalInfo
        public DataResult<StatisticShowUpViewModel> GetStaticalInfo(string branch)
        {
            try
            {
                var lst = this.Procedure<StatisticShowUpViewModel>("sp_tblShowUpHistory_StatisticShowUp", new { @BranchCode = branch }).ToList();
                return new DataResult<StatisticShowUpViewModel> { Result = lst };
            }
            catch
            {
                return new DataResult<StatisticShowUpViewModel> { Error = true };
            }
        }
        #endregion

        #region GetInfoByPhoneNumber
        public DataResult<CheckinInfoModel> GetInfoByPhoneNumber(string phoneNumber)
        {
            try
            {
                var lst = this.Procedure<CheckinInfoModel>("sp_tblShowUpHistory_GetByPhoneNumber", new { phone = phoneNumber }).ToList();
                return new DataResult<CheckinInfoModel> { Result = lst };
            }
            catch
            {
                return new DataResult<CheckinInfoModel> { Error = true };
            }
        }
        #endregion

        #region ImportCheckin - khanhkk
        /// <summary>
        /// Import Checkin data
        /// </summary>
        /// <param name="model">list of checkin info</param>
        /// <returns></returns>
        public DataResult<ShowUpHistoryCreateViewModel> ImportCheckin(List<ShowUpHistoryCreateViewModel> model)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        foreach (var item in model)
                        {
                            Investor investor = new Investor()
                            {
                                Name = item.InvestorName,
                                //CodeInvestor = Guid.NewGuid().ToString(),
                                Birthday = item.Birthday,
                                PhoneNumber = item.PhoneNumber,
                                IdCard = item.IdCard,
                                Status = 0,
                            };

                            ShowUpHistory history = new ShowUpHistory
                            {
                                CodeShow = Guid.NewGuid().ToString(),
                                CodeEvent = item.CodeEvent,
                                //CodeInvestor = investor.CodeInvestor,
                                InvestorResourceCode = item.InvestorResourceCode,
                                Note = item.Note,
                                Table = item.Table,
                                CodeStaff = item.TeleSale,
                                Status = true,
                                Sale = item.Sale,
                                SaleTO = item.SaleTO,
                                ContractValue = item.ContractValue,
                                Deposit = item.Deposit,
                                JoinedObject = item.JoinedObject,
                                Gift = item.Gift,
                                //temp
                                TimeIn = item.TimeIn,
                                TimeOut = item.TimeOut,
                                IsDirectAddition = false,
                                CreatedBy = item.CreatedBy,
                            };

                            List<GoWithInvestorModel> personList = new List<GoWithInvestorModel>();
                            if (item.Group != null)
                            {
                                foreach (var pr in item.Group)
                                {
                                    GoWithInvestorModel investoritem = new GoWithInvestorModel
                                    {
                                        //CodeInvestor = Guid.NewGuid().ToString(),
                                        Name = pr.Name,
                                        PhoneNumber = pr.PhoneNumber,
                                        CodeShowUpWithGroup = history.CodeShow,
                                    };
                                    personList.Add(investoritem);
                                }
                            }

                            // add investor
                            using (var multipleresult = db.QueryMultiple("sp_tblInvestors_GetInfo_ByPhone", 
                                new { @phone = investor.PhoneNumber, @id = investor.Id }, transaction, 
                                commandType: CommandType.StoredProcedure))
                            {

                                InvestorViewModel existedInvestor = multipleresult.Read<InvestorViewModel>().FirstOrDefault();
                                if (existedInvestor == null)
                                {
                                    using (var getResult = db.QueryMultiple("sp_tblInvestors_GetLastInfo", 
                                        new { @EventCode = item.CodeEvent }, transaction, 
                                        commandType: CommandType.StoredProcedure))
                                    {
                                        var lastInvestor = getResult.Read<Investor>().FirstOrDefault();
                                        if (lastInvestor == null || !lastInvestor.CodeInvestor.Contains(item.CodeEvent))
                                        {
                                            investor.CodeInvestor = item.CodeEvent + "P001";
                                        }
                                        else
                                        {
                                            int stt = int.Parse(
                                                lastInvestor.CodeInvestor.Substring(lastInvestor.CodeInvestor.Length - 3));
                                            string investorNo = null;
                                            if (stt + 1 < 10)
                                            {
                                                investorNo += "00";
                                            }
                                            else if (stt + 1 < 100)
                                            {
                                                investorNo += "0";
                                            }
                                            investorNo += (stt + 1);
                                            investor.CodeInvestor = item.CodeEvent + "P" + investorNo;
                                        }
                                    }

                                    db.Execute(InsertInvestorQuery, investor, transaction);
                                }
                                else
                                {
                                    history.CodeInvestor = existedInvestor.CodeInvestor;
                                    investor.CodeInvestor = existedInvestor.CodeInvestor;
                                    db.Execute(UpdateInvestorQuery, investor, transaction);
                                }
                            }

                            //check existed showup
                            using (var multipleresult = db.QueryMultiple("sp_tblShowUpHistory_GetInfo_By_PhoneNumber_Event", 
                                new { 
                                    @phone = investor.PhoneNumber, 
                                    @Event = item.CodeEvent 
                                }, transaction, commandType: CommandType.StoredProcedure))
                            {
                                CheckinInfoModel existedCheckin = multipleresult.Read<CheckinInfoModel>().FirstOrDefault();
                                if (existedCheckin is null)
                                {
                                    // add checkin
                                    history.CodeInvestor = investor.CodeInvestor;
                                    db.Execute(InsertCheckInQuery, history, transaction);
                                }
                                else
                                {
                                    // update checkin
                                    history.Id = existedCheckin.Id;
                                    history.CodeShow = existedCheckin.CodeShow;
                                    db.Execute(UpdateCheckInQuery, history, transaction);

                                    //delete some go with primary investor
                                    // get investor info who go with primary investor
                                    using (var getGoWithInvestorResult = db.QueryMultiple("sp_tblInvestor_GetGoWithByCheckInCode",
                                    new
                                    {
                                        @code = existedCheckin.CodeShow
                                    },
                                    transaction,
                                    commandType: CommandType.StoredProcedure))
                                    {
                                        var listGoWiths = getGoWithInvestorResult.Read<GoWithInvestorModel>().ToList();
                                        foreach (var goWith in listGoWiths)
                                        {
                                            //skip person in above list
                                            if (personList.Select(x => x.PhoneNumber).Contains(goWith.PhoneNumber)) continue;

                                            if (goWith.CodeShowUpWithGroup == existedCheckin.CodeShow)
                                            {
                                                //check số lần tham dự show up của khách đi cùng
                                                using (var getPrimaryInvestorResult = db.QueryMultiple("sp_tblShowUpHistory_Get_By_CodeInvestor",
                                                    new
                                                    {
                                                        @CodeInvestor = existedCheckin.CodeInvestor
                                                    },
                                                    transaction,
                                                    commandType: CommandType.StoredProcedure))
                                                {
                                                    var listShows = getPrimaryInvestorResult.Read<ShowUpHistoryCreateViewModel>().ToList();
                                                    // this is first showup
                                                    if (listShows.Count == 1)
                                                    {
                                                        using (var getContractResult = db.QueryMultiple("sp_tblContractInvestor_GeInfo_By_CodeInvestor",
                                                                new
                                                                {
                                                                    @CodeInvestor = goWith.CodeInvestor
                                                                },
                                                                transaction,
                                                                commandType: CommandType.StoredProcedure))
                                                        {
                                                            var contractList = getContractResult.Read<ContractInvesterViewModel>().ToList();
                                                            // this investor didn't have any contracts
                                                            if (contractList.Count == 0)
                                                            {
                                                                db.Execute(DeleteInvestor,
                                                                new { @CodeInvestor = goWith.CodeInvestor },
                                                                transaction);
                                                            }
                                                        }
                                                    }
                                                }
                                                continue;
                                            }

                                            if (goWith.CodeShowUpWithGroup.StartsWith(existedCheckin.CodeShow))
                                            {
                                                db.Execute(UpdateInvestorGoWithQuery,
                                                    new
                                                    {
                                                        @Name = goWith.Name,
                                                        @CodeShowUpWithGroup = goWith.CodeShowUpWithGroup.Replace(existedCheckin.CodeShow +
                                                        ",", string.Empty)
                                                    },
                                                    transaction);
                                                continue;
                                            }

                                            db.Execute(UpdateInvestorGoWithQuery,
                                                new
                                                {
                                                    @Name = goWith.Name,
                                                    @CodeShowUpWithGroup = goWith.CodeShowUpWithGroup.Replace("," +
                                                    existedCheckin.CodeShow, string.Empty)
                                                },
                                                transaction);
                                        }
                                    }
                                }
                            }

                            // add go with person
                            if (personList.Count > 0)
                            {
                                int stt = 0;
                                foreach (var person in personList)
                                {
                                    stt++;
                                    using (var multipleresult = db.QueryMultiple("sp_tblInvestors_GetInfo_ByPhone", 
                                        new { @phone = person.PhoneNumber, @id = person.Id }, transaction, 
                                        commandType: CommandType.StoredProcedure))
                                    {
                                        var existedInvestor = multipleresult.Read<InvestorViewModel>().FirstOrDefault();
                                        if (existedInvestor is null)
                                        {
                                            person.CodeInvestor = 
                                                investor.CodeInvestor + "S" + (stt < 10 ? "0" : string.Empty) + stt;
                                            db.Execute(InsertGoWithInvestorQuery, person, transaction);
                                        }
                                        else
                                        {
                                            existedInvestor.Name = person.Name;
                                            if (string.IsNullOrEmpty(existedInvestor.CodeShowUpWithGroup))
                                            {
                                                existedInvestor.CodeShowUpWithGroup = history.CodeShow;
                                            }
                                            else if (!existedInvestor.CodeShowUpWithGroup.Contains(history.CodeShow))
                                            {
                                                existedInvestor.CodeShowUpWithGroup =
                                                    existedInvestor.CodeShowUpWithGroup + "," + history.CodeShow;
                                            }
                                            db.Execute(UpdateInvestorGoWithQuery, existedInvestor, transaction);
                                        }
                                    }
                                }
                            }
                        }
                        transaction.Commit();
                    }

                }

                return new DataResult<ShowUpHistoryCreateViewModel>();
            }
            catch(Exception ex)
            {
                return new DataResult<ShowUpHistoryCreateViewModel> { Error = true };
            }
        }
        #endregion

        #region RemoveCheckin
        public DataResult<ShowUpHistoryTableViewModel> RemoveCheckin(long id)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Execute(RemoveCheckinQuery, new { @Id = id });
                }

                return new DataResult<ShowUpHistoryTableViewModel>();
            }
            catch
            {
                return new DataResult<ShowUpHistoryTableViewModel> { Error = true };
            }
        }
        #endregion

        #region Delete
        public DataResult<ShowUpHistoryTableViewModel> Delete(long id)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(OpenDapper.connectionStr))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        //get showup info by id
                        using (var getResult = db.QueryMultiple("sp_tblShowUpHistory_GetById", 
                            new { @id = id }, transaction, commandType: CommandType.StoredProcedure))
                        {
                            var showUp = getResult.Read<ShowUpHistoryCreateViewModel>().FirstOrDefault();

                            if (showUp != null)
                            {
                                //check số lần tham dự show up
                                using (var getPrimaryInvestorResult = db.QueryMultiple("sp_tblShowUpHistory_Get_By_CodeInvestor", 
                                    new { 
                                        @CodeInvestor = showUp.CodeInvestor 
                                    }, transaction, commandType: CommandType.StoredProcedure))
                                {
                                    var listShows = getPrimaryInvestorResult.Read<ShowUpHistoryCreateViewModel>().ToList();
                                    // this is first showup
                                    if (listShows.Count == 1)
                                    {
                                        using (var getContractResult = db.QueryMultiple("sp_tblContractInvestor_GeInfo_By_CodeInvestor",
                                                new
                                                {
                                                    @CodeInvestor = showUp.CodeInvestor
                                                },
                                                transaction,
                                                commandType: CommandType.StoredProcedure))
                                        {
                                            var contractList = getContractResult.Read<ContractInvesterViewModel>().ToList();
                                            // this investor didn't have any contracts
                                            if (contractList.Count == 0)
                                            {
                                                db.Execute(DeleteInvestor,
                                                new { @CodeInvestor = showUp.CodeInvestor },
                                                transaction);
                                            }
                                        }
                                    }
                                }

                                // get investor info who go with primary investor
                                using (var getGoWithInvestorResult = db.QueryMultiple("sp_tblInvestor_GetGoWithByCheckInCode", 
                                    new { 
                                        @code = showUp.CodeShow 
                                    }, 
                                    transaction, 
                                    commandType: CommandType.StoredProcedure))
                                {
                                    var listGoWiths = getGoWithInvestorResult.Read<GoWithInvestorModel>().ToList();
                                    foreach (var item in listGoWiths)
                                    {
                                        if (item.CodeShowUpWithGroup == showUp.CodeShow)
                                        {
                                            //check số lần tham dự show up của khách đi cùng
                                            using (var getPrimaryInvestorResult = db.QueryMultiple("sp_tblShowUpHistory_Get_By_CodeInvestor", 
                                                new { 
                                                    @CodeInvestor = showUp.CodeInvestor 
                                                }, 
                                                transaction, 
                                                commandType: CommandType.StoredProcedure))
                                            {
                                                var listShows = getPrimaryInvestorResult.Read<ShowUpHistoryCreateViewModel>().ToList();
                                                // this is first showup
                                                if (listShows.Count == 1)
                                                {
                                                    using (var getContractResult = db.QueryMultiple("sp_tblContractInvestor_GeInfo_By_CodeInvestor",
                                                            new
                                                            {
                                                                @CodeInvestor = item.CodeInvestor
                                                            },
                                                            transaction,
                                                            commandType: CommandType.StoredProcedure))
                                                    {
                                                        var contractList = getContractResult.Read<ContractInvesterViewModel>().ToList();
                                                        // this investor didn't have any contracts
                                                        if (contractList.Count == 0)
                                                        {
                                                            db.Execute(DeleteInvestor,
                                                            new { @CodeInvestor = item.CodeInvestor },
                                                            transaction);
                                                        }
                                                    }
                                                }
                                            }
                                            //using (var getContractResult = db.QueryMultiple("sp_tblContractInvestor_GeInfo_By_CodeInvestor",
                                            //    new {
                                            //        @CodeInvestor = item.CodeInvestor
                                            //    },
                                            //    transaction,
                                            //    commandType: CommandType.StoredProcedure))
                                            //{
                                            //    var contractList = getContractResult.Read<ContractInvesterViewModel>().ToList();
                                            //    if (contractList.Count == 0)
                                            //    {
                                            //        db.Execute(DeleteInvestor, 
                                            //        new { @CodeInvestor = item.CodeInvestor }, 
                                            //        transaction);
                                            //    }    
                                            //}
                                            continue;
                                        }

                                        if (item.CodeShowUpWithGroup.StartsWith(showUp.CodeShow))
                                        {
                                            db.Execute(UpdateInvestorGoWithQuery, 
                                                new { 
                                                    @Name = item.Name, 
                                                    @CodeShowUpWithGroup = item.CodeShowUpWithGroup.Replace(showUp.CodeShow + 
                                                    ",", string.Empty) }, 
                                                transaction);
                                            continue;
                                        }

                                        db.Execute(UpdateInvestorGoWithQuery, 
                                            new { 
                                                @Name = item.Name, 
                                                @CodeShowUpWithGroup = item.CodeShowUpWithGroup.Replace("," + 
                                                showUp.CodeShow, string.Empty) }, 
                                            transaction);
                                    } 
                                   
                                }

                                db.Execute(DeleteShowUpInfo, new { @Id = id }, transaction);
                            }
                        }

                        transaction.Commit();
                    }
                }

                return new DataResult<ShowUpHistoryTableViewModel>();
            }
            catch (Exception ex)
            {
                return new DataResult<ShowUpHistoryTableViewModel> { Error = true };
            }
        }
        #endregion
    }
}
