using FMS.Services.ADFPipelines.Models;

namespace FMS.Services.ADFPipelines.Interfaces
{
    public interface IADFPipeineService
    {
        Task<List<PipelineDTO>> GetPipelinesDataAsync();
    }
}
