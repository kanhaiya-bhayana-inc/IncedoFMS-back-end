using FMS.Services.AzueFileUploadAPI.Model;
using FMS.Services.AzueFileUploadAPI.Model.DropdownOptions;
using FMS.Services.AzueFileUploadAPI.Model.Dto;

namespace FMS.Services.AzueFileUploadAPI.Services.IService
{
    public interface IFilesDataService
    {
        Task<IEnumerable<FileManagement>> GetAllFilesAsync();
        //Task<IEnumerable<FileManagementDTO>> UpdateFileAsync();
    }
}
