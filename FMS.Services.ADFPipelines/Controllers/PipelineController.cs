using FMS.Services.ADFPipelines.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FMS.Services.ADFPipelines.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PipelineController : ControllerBase
    {
        private readonly IADFPipeineService _service;
        public PipelineController(IADFPipeineService aDFPipeineService)
        {
            _service = aDFPipeineService;
        }
        [HttpGet("GetPipelineData")]
        public async Task<IActionResult> GetPipelineData()
        {
            try
            {
                var response = await _service.GetPipelinesDataAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error("PipelineController-GetPipelineData exception -> {@response}", ex.Message);
                throw ex;
            }
            
        }

        [HttpPost("RerunADFPipeline")]
        public async Task<IActionResult> RerunADFPipeline(string request)
        {
            try
            {
                var response = await _service.RerunPipelineAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error("PipelineController-RerunADFPipeline exception -> {@response}", ex.Message);
                throw ex;
            }
        }
    }
}
