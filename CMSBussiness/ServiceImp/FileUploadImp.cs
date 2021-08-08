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
    public class FileUploadImp : BaseService<UploadFileToView, long>, IFileUpload
    {
        #region Create
        public DataResult<FileUploadViewModel> Create(FileUploadViewModel model)
        {
            UploadFileToView view = new UploadFileToView
            {
                LinksUpload = model.LinksUpload,
                Reason = model.Reason,
                CreateDate = DateTime.Now,
                UserUpload = model.UserUpload,
                IdContractInvestor = model.IdContractInvestor
            };

            try
            {
                var data = Raw_Insert(view);

                return new DataResult<FileUploadViewModel>();
            }
            catch
            {
                return new DataResult<FileUploadViewModel> { Error = true };
            }
        }
        #endregion

        #region Delete
        public DataResult<FileUploadViewModel> Delete(long id)
        {
            try
            {
                Raw_Delete(id.ToString());

                return new DataResult<FileUploadViewModel>();
            }
            catch
            {
                return new DataResult<FileUploadViewModel> { Error = true };
            }
        }
        #endregion

        #region GetList
        public DataResult<FileUploadViewModel> GetList(string key, out int total, int page = 1, int size = 10)
        {
            total = 0;

            DynamicParameters param = new DynamicParameters();
            param.Add("@Key", key ?? string.Empty);
            param.Add("@Page", page);
            param.Add("@Size", size);
            param.Add("@Total", dbType: DbType.Int32, direction: ParameterDirection.Output);

            try
            {
                var lst = this.Procedure<FileUploadViewModel>("sp_tblUploadFileToView_GetList", param).ToList();
                total = param.Get<int>("@Total");
                return new DataResult<FileUploadViewModel> { Result = lst };
            }
            catch(Exception ex)
            {
                return new DataResult<FileUploadViewModel> { Error = true };
            }
        }
        #endregion

        #region Update
        public DataResult<FileUploadViewModel> Update(FileUploadViewModel model)
        {
            UploadFileToView view = new UploadFileToView
            {
                Id = model.Id,
                LinksUpload = model.LinksUpload,
                Reason = model.Reason,
                CreateDate = model.CreateDate,
                UserUpload = model.UserUpload,
                UpdateDate = DateTime.Now,
                UserUpdate = model.UserUpdate
            };

            try
            {
                Raw_Update(view);

                return new DataResult<FileUploadViewModel>();
            }
            catch
            {
                return new DataResult<FileUploadViewModel> { Error = true };
            }
        }
        #endregion

        #region Get
        public DataResult<FileUploadViewModel> Get(long id)
        {
            try
            {
                var file = Raw_Get(id);
                FileUploadViewModel model = new FileUploadViewModel
                {
                    Id = file.Id,
                    LinksUpload = file.LinksUpload,
                    Reason = file.Reason,
                    CreateDate = file.CreateDate,
                    UserUpload = file.UserUpload,
                    UserUpdate = file.UserUpdate,
                    UpdateDate = file.UpdateDate
                };
                return new DataResult<FileUploadViewModel> { Result = new List<FileUploadViewModel> { model } };
            }
            catch
            {
                return new DataResult<FileUploadViewModel> { Error = true };
            }
        }

        public DataResult<UploadFileToView> Insert(FileUploadViewModel model)
        {
            UploadFileToView view = new UploadFileToView
            {
                LinksUpload = model.LinksUpload,
                Reason = model.Reason,
                CreateDate = DateTime.Now,
                UserUpload = model.UserUpload,
                IdContractInvestor = model.IdContractInvestor
            };

            try
            {
                var data = Raw_Insert(view);

                return new DataResult<UploadFileToView>() { DataItem = data };
            }
            catch
            {
                return new DataResult<UploadFileToView> { Error = true };
            }
        }
        #endregion
    }
}
