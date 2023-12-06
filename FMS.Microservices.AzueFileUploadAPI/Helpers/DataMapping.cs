using FMS.Services.AzueFileUploadAPI.Model;
using FMS.Services.AzueFileUploadAPI.Model.Dto;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace FMS.Services.AzueFileUploadAPI.Helpers
{
    public class DataMapping
    {
        private readonly string _containerName;

        public DataMapping(IConfiguration configuration)
        {
            _containerName = configuration.GetValue<string>("BlobConfiguration:BlobContainerNameTempFile");
        }
        public FileManagement MapData(FileManagementDTO request)
        {
            FileManagement response = new FileManagement();
            if (request != null)
            {
                response.FileMasterId = request.FileMasterId;
                response.FileDate = request.FileDate;
                response.FileName = request.FileName;
                response.SourcePath = $"{_containerName}{request.SourcePath}";
                response.DestinationPath = $"00raw{request.DestinationPath}";
                response.EmailID = request.EmailID;
                response.Delimiter = request.Delimiter;
                response.IsActive = request.IsActive == "true" ? "Y" : "N";
                response.ClientID = request.VendorName;
                response.FileTypeID = request.FileTypeID;
                response.InsertionMode = request.InsertionMode;
                response.TemplateName = request.FileMasterId+"_"+request.FileName;
                response.FixedLength = request.FixedLength == "true" ? "Y" : "N";
                response.DbNotebook = request.DbNotebook;
            }

            return response;
        }

        public static FileManagementDTO MapReturnData(SqlDataReader request)
        {
            FileManagementDTO response = new FileManagementDTO();
            if (request != null)
            {
                response.FileMasterId = request.GetGuid(0).ToString();
                response.FileName = request.GetString(1);
                response.SourcePath = request.GetString(2);
                response.DestinationPath = request.GetString(3);
                response.FileTypeID = request.GetInt32(4).ToString();
                response.Delimiter = request.GetString(5);
                response.FixedLength = request.GetString(6) == "Y" ? "true" : "false";
                response.EmailID = request.GetString(8);
                response.VendorName = request.GetInt64(9).ToString();
                response.FileDate = request.GetString(10);
                response.InsertionMode = request.GetString(11);
                response.IsActive = request.GetString(12) == "Y" ? "true" : "false";
                response.DbNotebook = request.GetString(13);
            }

            return response;
        }
    }
}


