namespace FMS.Services.ADFPipelines.Models
{
    public class ResponseDTO
    {
        public int StatusCode { get; set; }
        public string? IsSuccessStatusCode { get; set; }

        public string? ReasonPhrase { get; set; }
    }
}
