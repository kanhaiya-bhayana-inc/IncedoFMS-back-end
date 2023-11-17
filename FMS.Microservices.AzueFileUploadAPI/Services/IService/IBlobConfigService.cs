using FMS.Services.AzueFileUploadAPI.Model.Dto;

namespace FMS.Services.AzueFileUploadAPI.Services.IService
{
    public interface IBlobConfigService
    {
        Task<BlobConfigDto> GetBlobInfoAsync();
    }
}
