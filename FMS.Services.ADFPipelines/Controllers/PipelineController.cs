using FMS.Services.ADFPipelines.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet]
        public async Task<IActionResult> GetPipelineData()
        {
            var response = await _service.GetPipelinesDataAsync();
            return Ok(response);
        }
    }
}
