namespace FMS.Services.ADFPipelines.Models
{
    public class PipelineDTO
    {
        public string? PipelineName { get; set; }
        public string? RunID { get; set; }
        public string? Status { get; set; }
        public string? ErrorMessage { get; set; }

        public bool RetryAction { get; set; }

        public int DurationInMS { get; set; }
        public DateTime RunStart { get; set; }
        public DateTime RunEnd { get; set; }

        public Dictionary<string,string>? Parameters { get; set; }
    }
}
