using FMS.Services.AzueFileUploadAPI.Model.Dto;
using FMS.Services.AzueFileUploadAPI.Services;
using FMS.Services.AzueFileUploadAPI.Services.IService;
using FMS.Services.AzueFileUploadAPI.Services.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FMS.Services.AzueFileUploadAPI.Controllers
{
    [Route("api/file")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IUploadFile _uploadFile;
        private readonly IBlobDownloader _blobDownloader;
        

        public FileController(IUploadFile uploadFile,IBlobDownloader blobDownloader)
        {
            _uploadFile = uploadFile;
            _blobDownloader = blobDownloader;
        }
        [HttpPost("UploadFile")]
        public async Task<AzureBlobResponseDto> UploadFile([FromForm] FileManagementDTO fileManagementDTO)
        {
            var response = _uploadFile.UploadFileAsync(fileManagementDTO);
            return response;
        }
        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadBlob([FromQuery] bool check)
        {
            var res = new AzureBlobResponseDto();
            //string cn = "DefaultEndpointsProtocol=https;AccountName=fmstest;AccountKey=ZCsPRTzeel8dk6HHgdmKuKihcMAK3cc59d3t1EDyBbAUxTQ290ijWPFfjmzQcxE91WMFHScfs0GV+AStbHiXLQ==;EndpointSuffix=core.windows.net";
            try
            {
                IFormFile response = await _blobDownloader.DownloadTemplateAsync(check);

                if (response != null)
                {
                    // Return the IFormFile as a FileResult
                    return File(response.OpenReadStream(), "text/csv", response.FileName);
                }
                else
                {
                    return NotFound(); // Handle not found case
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500); // Handle internal server error
            }
        }
    }
}
