using Azure;
using FMS.Services.AzueFileUploadAPI.Model.DropdownOptions;
using FMS.Services.AzueFileUploadAPI.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FMS.Services.AzueFileUploadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDrowpdownOptionsService _dropdownOptions;
        private readonly IFilesDataService _filesData;
        private readonly IBlobConfigService _blobConfigService;

        public DataController(IDrowpdownOptionsService drowpdownOptionsService,IFilesDataService filesData, IBlobConfigService blobConfigService)
        {
            _dropdownOptions = drowpdownOptionsService;
            _filesData = filesData;
            _blobConfigService = blobConfigService;

        }

        [HttpGet("GetDelimiters")]
        public async Task<IActionResult> GetDelimiters()
        {
            try
            {
                var dropdownOptions = await _dropdownOptions.GetDelimiterOptionsAsync();
                return new JsonResult(dropdownOptions);
            }
            catch (Exception ex)
            {
                Log.Error("DataController-GetDelimiters exception -> {@response}", ex.Message);
                throw ex;
            }
        }

        [HttpGet("GetVendorDetails")]
        public async Task<IActionResult> GetVendorDetails()
        {
            try
            {
                var dropdownOptions = await _dropdownOptions.GetVendorOptionsAsync();
                return new JsonResult(dropdownOptions);
            }
            catch (Exception ex)
            {
                Log.Error("DataController-GetVendorDetails exception -> {@response}", ex.Message);
                throw ex;
            }
        }

        [HttpGet("GetFiledateDetails")]
        public async Task<IActionResult> GetFiledateDetails()
        {
            try
            {
                var dropdownOptions = await _dropdownOptions.GetFiledateOptionsAsync();
                return new JsonResult(dropdownOptions);
            }
            catch (Exception ex)
            {
                Log.Error("DataController-GetFiledateDetails exception -> {@response}", ex.Message);
                throw ex;
            }
        }

        [HttpGet("GetFiletypeDetails")]
        public async Task<IActionResult> GetFiletypeDetails()
        {
            try
            {
                var dropdownOptions = await _dropdownOptions.GetFiledtypeOptionsAsync();
                return new JsonResult(dropdownOptions);
            }
            catch (Exception ex)
            {
                Log.Error("DataController-GetFiletypeDetails exception -> {@response}", ex.Message);
                throw ex;
            }
        }

        [HttpGet("GetAllFilesDetails")]
        public async Task<IActionResult> GetAllFilesDetails()
        {
            try
            {
                var response = await _filesData.GetAllFilesAsync();
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Log.Error("DataController-GetAllFilesDetails exception -> {@response}", ex.Message);
                throw ex;
            }
        }


        [HttpGet("GetBlobConfigInfo")]
        public async Task<IActionResult> GetBlobConfigInfo()
        {
            try
            {
                var response = await _blobConfigService.GetBlobInfoAsync();
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                Log.Error("DataController-GetBlobConfigInfo exception -> {@response}", ex.Message);
                throw ex;
            }
        }

    }
}
