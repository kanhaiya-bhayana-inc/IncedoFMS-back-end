using FMS.Services.AzueFileUploadAPI.Utility;
using Microsoft.AspNetCore.Components.Forms;

namespace FMS.Services.AzueFileUploadAPI.Helpers
{
    public class FixPathMapping
    {
        private readonly string _templateFileLocation;
        public FixPathMapping(IConfiguration configuration)
        {
            _templateFileLocation = configuration.GetValue<string>("BlobConfiguration:TemplateFileLocation");
        }
        public string FixPathMapper(string path, string fileType)
        {
            string[] pathComponents = path.Split('/');
            string returnPath = "";
            if (fileType == StaticDetails.fileTypeTemplate)
            {
                if (pathComponents.Length >= 3)
                {
                    returnPath = _templateFileLocation;
                }
            }
            else if(fileType == StaticDetails.fileTypeSample)
            {
                if (pathComponents.Length >= 2)
                {
                    returnPath = string.Join("/", pathComponents, 1, 4);
                }
            }
            

            return returnPath;
        }
    }
}
