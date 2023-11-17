namespace FMS.Services.AzueFileUploadAPI.Model.Dto
{
    public class CustomerBankDetails
    {
        public string AccountNumber { get; set; }
        public string CustomerName { get; set; }
        public int Balance { get; set; }

        public IFormFile File { get; set; }
        public IFormFile File2 { get; set; }
        public string? Status { get; set; }

        public bool? Error { get; set; }
    }
}
