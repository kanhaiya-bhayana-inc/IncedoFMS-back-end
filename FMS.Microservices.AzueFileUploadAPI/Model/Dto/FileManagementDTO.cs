namespace FMS.Services.AzueFileUploadAPI.Model.Dto
{
    public class FileManagementDTO
    {   
        public string? FileMasterId { get; set; } = null;
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public string? FileName { get; set; }
        public string VendorName { get; set; }
        public string? FixedLength { get; set; }
        public string? IsActive { get; set; }
        public string? EmailID { get; set; }
        public string? FileTypeID { get; set; }
        public string? StartPosition { get; set; }
        public string? EndPosition { get; set; }
        public string? InsertionMode { get; set; }

        public string? DbNotebook { get; set; }
        public string? Delimiter { get; set; }
        public string? FileDate { get; set; }

        public IFormFile TemplateFile { get; set; }
        public IFormFile? SampleFile { get; set; }
        public string Stage { get; set; }
        public string Curated { get; set; }
        public string Header { get; set; }




    }
}
