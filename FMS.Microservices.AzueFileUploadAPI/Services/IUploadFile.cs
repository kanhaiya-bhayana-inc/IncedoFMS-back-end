using FMS.Services.AzueFileUploadAPI.Model.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FMS.Services.AzueFileUploadAPI.Services
{
    public interface IUploadFile
    {
        AzureBlobResponseDto UploadFileAsync([FromForm] FileManagementDTO fileManagementDTO);
    }
}
