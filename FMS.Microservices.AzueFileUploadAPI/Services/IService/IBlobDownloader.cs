using Microsoft.AspNetCore.Mvc;

namespace FMS.Services.AzueFileUploadAPI.Services.IService
{
    public interface IBlobDownloader
    {
        Task<IFormFile> DownloadTemplateAsync(bool fixedLengthCheck);
    }
}
