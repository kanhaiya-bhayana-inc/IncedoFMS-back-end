using FMS.Services.AzueFileUploadAPI.Model;
using FMS.Services.AzueFileUploadAPI.Model.DropdownOptions;
using FMS.Services.AzueFileUploadAPI.Services.IService;
using System.Data.SqlClient;
using System.Data;

namespace FMS.Services.AzueFileUploadAPI.Services.Service
{
    public class FilesDataService : IFilesDataService
    {
        private readonly string _connectionString;
        public FilesDataService(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
        }

        public async Task<IEnumerable<FileManagement>> GetAllFilesAsync()
        {
            List<FileManagement> data = new List<FileManagement>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM FileDetailsMaster";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "FileDetailsMaster");

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string id = ds.Tables[0].Rows[i]["filemasterid"].ToString();
                            string name = ds.Tables[0].Rows[i]["filename"].ToString();
                            string sourcepath = ds.Tables[0].Rows[i]["sourcepath"].ToString();
                            string destinationpath = ds.Tables[0].Rows[i]["destinationpath"].ToString();
                            string filetypeid = ds.Tables[0].Rows[i]["filetypeid"].ToString();
                            string delimiter = ds.Tables[0].Rows[i]["delimiter"].ToString();
                            string fixedlength = ds.Tables[0].Rows[i]["fixedlength"].ToString();
                            string templatename = ds.Tables[0].Rows[i]["templatename"].ToString();
                            string emailid = ds.Tables[0].Rows[i]["emailid"].ToString();
                            string clientid = ds.Tables[0].Rows[i]["clientid"].ToString();
                            string filedate = ds.Tables[0].Rows[i]["filedate"].ToString();
                            string insertionmode = ds.Tables[0].Rows[i]["insertionmode"].ToString();
                            string isactive = ds.Tables[0].Rows[i]["isactive"].ToString();
                            string dbnotebook = ds.Tables[0].Rows[i]["dbnotebook"].ToString();

                            FileManagement fileData = new FileManagement
                            {
                                FileMasterId = id,
                                FileName = name,
                                SourcePath = sourcepath,
                                DestinationPath = destinationpath,
                                FileTypeID = filetypeid,
                                Delimiter = delimiter,
                                FixedLength = (fixedlength == "Y") ? "true" : "false",
                                TemplateName = templatename,
                                EmailID = emailid,
                                ClientID = clientid,
                                FileDate = filedate.Trim(),
                                InsertionMode = insertionmode.Trim(),
                                IsActive = (isactive == "Y") ? "true" : "false",
                                DbNotebook = dbnotebook
                            };

                            data.Add(fileData);
                        }
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
