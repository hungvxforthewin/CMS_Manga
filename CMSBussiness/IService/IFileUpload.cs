using CRMBussiness.ViewModel;
using CRMModel.Models.Data;

namespace CRMBussiness.IService
{
    public interface IFileUpload
    {
        DataResult<FileUploadViewModel> GetList(string key, out int total, int page = 1, int size = 10);

        DataResult<FileUploadViewModel> Create(FileUploadViewModel model);
        DataResult<UploadFileToView> Insert(FileUploadViewModel model);

        DataResult<FileUploadViewModel> Update(FileUploadViewModel model);

        DataResult<FileUploadViewModel> Delete(long id);

        DataResult<FileUploadViewModel> Get(long id);
    }
}
