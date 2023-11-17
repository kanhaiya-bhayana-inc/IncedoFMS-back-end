using FMS.Services.AzueFileUploadAPI.Model.Dto;

namespace FMS.Services.AzueFileUploadAPI.Services
{
    public interface IAzureUploadFileService
    {
        Task<AzureBlobResponseDto> UploadAsync(IFormFile file, string containerName, string path);


    }
}
