using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FMS.Services.AzueFileUploadAPI.Services.IService;

namespace FMS.Services.AzueFileUploadAPI.Services.Service
{
    public class BlobDownloader : IBlobDownloader
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly string _templateName;
        private readonly string _fixedLengthTemplateName;

        public BlobDownloader(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("BlobConfiguration:ConnectionString");
            _containerName = configuration.GetValue<string>("BlobConfiguration:BlobContainerNameFxTempFile");
            _templateName = configuration.GetValue<string>("BlobConfiguration:TemplateFileName");
            _fixedLengthTemplateName = configuration.GetValue<string>("BlobConfiguration:FixedLengthTemplateFileName");

            _blobServiceClient = new BlobServiceClient(_connectionString);
        }

        public async Task<IFormFile> DownloadBlobAndConvertToFormFileAsync(string containerName, string blobName)
        {
            IFormFile formFile1 = null;
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            if (await blobClient.ExistsAsync())
            {
                BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();

                MemoryStream memoryStream = new MemoryStream();
                await blobDownloadInfo.Content.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                IFormFile formFile = new FormFile(memoryStream, 0, memoryStream.Length, blobName, blobName);

                return formFile;
            }

            return formFile1; // Blob not found
        }

        public async Task<IFormFile> DownloadTemplateAsync(bool fixedLengthCheck)
        {
            try
            {
                string blobName = "";
                if (fixedLengthCheck == true)
                {
                    blobName = _fixedLengthTemplateName;
                }
                else
                {
                    blobName = _templateName;
                }
                IFormFile formFile1 = null;
                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                if (await blobClient.ExistsAsync())
                {
                    BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();

                    MemoryStream memoryStream = new MemoryStream();
                    await blobDownloadInfo.Content.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    IFormFile formFile = new FormFile(memoryStream, 0, memoryStream.Length, blobName, blobName);

                    return formFile;
                }

                return formFile1; // Blob not found
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}

