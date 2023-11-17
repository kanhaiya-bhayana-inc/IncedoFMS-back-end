namespace FMS.Services.AzueFileUploadAPI.Model.Dto
{
    public class BlobConfigDto
    {
        public string AccountName { get; set; }
        public string SourcePathPrefix { get; set; }
        public string SourcePathSufix { get; set; }

        public string ServerName { get; set; }
    }
}
