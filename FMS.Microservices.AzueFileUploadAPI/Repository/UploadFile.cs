using FMS.Services.AzueFileUploadAPI.Model.Dto;
using FMS.Services.AzueFileUploadAPI.Services;
using Microsoft.AspNetCore.Mvc;
using FMS.Services.AzueFileUploadAPI.DBContext;
using System.Data.SqlClient;
using FMS.Services.AzueFileUploadAPI.Helpers;
using FMS.Services.AzueFileUploadAPI.Utility;

namespace FMS.Services.AzueFileUploadAPI.Repository
{
    public class UploadFile : IUploadFile
    {
        private readonly IConfiguration _configuration;
        //private DBConnect db;

        private readonly FileUpload _uploadFile;
        private readonly string _tempFileContainerName;
        private readonly string _sampFileContainerName;
        private readonly DataMapping _dataMapping;
        private readonly string _connectionString;


        public UploadFile(IConfiguration configuration, FileUpload uploadFile)
        {
            _configuration = configuration;
            //db = new DBConnect(_configuration);
            _uploadFile = uploadFile;
            _tempFileContainerName = configuration.GetValue<string>("BlobContainerNameTempFile");
            _sampFileContainerName = configuration.GetValue<string>("BlobContainerNameSampFile");
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            _dataMapping = new DataMapping(configuration);
        }
        public AzureBlobResponseDto UploadFileAsync([FromForm] FileManagementDTO fileManagementDTO)
        {
            var requestData = _dataMapping.MapData(fileManagementDTO);
            var t = new FileManagementDTO();
            AzureBlobResponseDto response = new();
            var fileNameObj = new NewFileNameDto();
            string fileNameNew = "";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UpsertIntoFileDetailsMaster8", connection))
                {
                    Guid id = new Guid();
                    string tempName = requestData.FileMasterId;
                    if (tempName == null)
                    {
                        tempName = id.ToString();
                    }
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FileMasterId", (requestData.FileMasterId)==null?id:requestData.FileMasterId);
                    command.Parameters.AddWithValue("@FileName", requestData.FileName);
                    command.Parameters.AddWithValue("@SourcePath", requestData.SourcePath);
                    command.Parameters.AddWithValue("@DestinationPath", requestData.DestinationPath);
                    command.Parameters.AddWithValue("@FileTypeID", requestData.FileTypeID);
                    command.Parameters.AddWithValue("@Delimiter", requestData.Delimiter);
                    command.Parameters.AddWithValue("@FixedLength", requestData.FixedLength);
                    command.Parameters.AddWithValue("@TemplateName", requestData.TemplateName);
                    command.Parameters.AddWithValue("@EmailID", requestData.EmailID);
                    command.Parameters.AddWithValue("@ClientID", requestData.ClientID);
                    command.Parameters.AddWithValue("@FileDate", requestData.FileDate);
                    command.Parameters.AddWithValue("@StartPosition", requestData.StartPosition);
                    command.Parameters.AddWithValue("@EndPosition", requestData.EndPosition);
                    command.Parameters.AddWithValue("@InsertionMode", requestData.InsertionMode);
                    command.Parameters.AddWithValue("@IsActive", requestData.IsActive);
                    command.Parameters.AddWithValue("@DbNotebook", requestData.DbNotebook);
                    command.Parameters.AddWithValue("@Stage", requestData.Stage);
                    command.Parameters.AddWithValue("@Curated", requestData.Curated);
                    command.Parameters.AddWithValue("@Header", requestData.Header);

                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        command.Transaction = transaction;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                fileNameObj.GUID = reader.GetGuid(0);
                                fileNameObj.FileName = reader.GetString(1);
                                t = DataMapping.MapReturnData(reader);
                            }

                            if (fileNameObj.GUID != null && fileNameObj.FileName != null)
                            {
                                fileNameNew = $"{fileNameObj.GUID}_{fileNameObj.FileName}";

                            }

                            if (fileNameObj.GUID == new Guid())
                            {
                                fileNameNew = $"{requestData.FileMasterId}_{requestData.FileName}";
                            }
                        }

                        // create new file name

                        IFormFile uploadFile = null;
                        AzureBlobResponseDto response1 = new();
                        AzureBlobResponseDto response2 = new();
                        string newFileName = RenameFile.CreateNewFileName(fileManagementDTO.TemplateFile.FileName, fileNameNew);
                        if (newFileName != null)
                        {
                            uploadFile = new RenameFile(fileManagementDTO.TemplateFile, newFileName);
                            t.TemplateFile = uploadFile;
                            t.SampleFile = fileManagementDTO.SampleFile;
                            response1 = _uploadFile.FileUploadAsync(uploadFile,StaticDetails.fileTypeTemplate,requestData.SourcePath).Result;
                        }

                        if (!response1.Error)
                        {
                            if (fileManagementDTO.SampleFile != null)
                            {
                                response2 = _uploadFile.FileUploadAsync(fileManagementDTO.SampleFile,StaticDetails.fileTypeSample,requestData.SourcePath).Result;
                                if (!response2.Error)
                                {
                                    transaction.Commit();
                                    response.Error = false;
                                    response.Status = response2.Status;
                                    response.data = t;
                                    Console.WriteLine("Transaction Committed");
                                }
                                else
                                {
                                    response.Error = true;
                                    response.Status = response2.Status;
                                    throw new Exception(response.Status);
                                }
                            }
                            else
                            {
                                transaction.Commit();
                                response.Error = false;
                                response.Status = response1.Status;
                                response.data = t;
                                Console.WriteLine("Transaction Committed");
                            }
                        }
                        else
                        {
                            response.Error = true;
                            response.Status = response1.Status;
                            throw new Exception(response.Status);
                        }

                    }
                    catch (Exception EX)
                    {
                        transaction.Rollback();
                        response.Error = true;
                        response.Status = EX.Message;
                    }
                    finally
                    {
                        connection.Close();
                    }

                }
            }

            return response;
        }
    }
}
