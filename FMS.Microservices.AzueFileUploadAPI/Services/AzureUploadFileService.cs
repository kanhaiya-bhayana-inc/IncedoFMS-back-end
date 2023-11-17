using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;
using FMS.Services.AzueFileUploadAPI.Model.Dto;
using FMS.Services.AzueFileUploadAPI.Helpers;

namespace FMS.Services.AzueFileUploadAPI.Services
{
    public class AzureUploadFileService : IAzureUploadFileService
    {
        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;
        private FixPathMapping fpm;

        public AzureUploadFileService(IConfiguration configuration)
        {
            _storageConnectionString = configuration.GetValue<string>("BlobConfiguration:ConnectionString");
            fpm = new FixPathMapping(configuration);
            _storageContainerName = configuration.GetValue<string>("BlobConfiguration:BlobContainerNameTempFile");
        }
        public async Task<AzureBlobResponseDto> UploadAsync(IFormFile blob, string filetype, string path)
        {
            AzureBlobResponseDto response = new();
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
           
            
            string location = fpm.FixPathMapper(path, filetype);
            try
            {
                BlobClient client = container.GetBlobClient($"{location}/{blob.FileName}");

                await using (Stream? data = blob.OpenReadStream())
                {
                    await client.UploadAsync(data, overwrite: true);
                }

                response.Status = $"File Uploaded Successfully";
                response.Error = false;
            }
            catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                response.Status = $"Failed to upload, File with name {blob.FileName} already exists. Please use another name to store your file.";
                response.Error = true;

                return response;
            }
            catch (RequestFailedException ex)
            {
                response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                response.Error = true;
                return response;
            }

            return response;
        }
    }
}


