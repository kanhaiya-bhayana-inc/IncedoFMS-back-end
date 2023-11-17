namespace FMS.Services.AzueFileUploadAPI.Model.Dto
{
    public class AzureBlobResponseDto
    {
        public string? Status { get; set; }

        public bool Error { get; set; }
        public FileManagementDTO? data { get; set; }
        public IFormFile temp { get; set; }

    }
}
