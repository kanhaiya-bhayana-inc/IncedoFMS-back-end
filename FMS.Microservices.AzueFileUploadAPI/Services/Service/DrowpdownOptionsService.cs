using FMS.Services.AzueFileUploadAPI.Model.DropdownOptions;
using FMS.Services.AzueFileUploadAPI.Services.IService;
using System.Data;
using System.Data.SqlClient;

namespace FMS.Services.AzueFileUploadAPI.Services.Service
{
    public class DrowpdownOptionsService : IDrowpdownOptionsService
    {
        private readonly string _connectionString;
        public DrowpdownOptionsService(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
        }
        public async Task<IEnumerable<DelimiterOptions>> GetDelimiterOptionsAsync()
        {
            List<DelimiterOptions> data = new List<DelimiterOptions>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM DelimiterDetailsMaster";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "DelimiterDetailsMaster");

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            int id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString());
                            string name = ds.Tables[0].Rows[i]["Name"].ToString();
                            string description = ds.Tables[0].Rows[i]["Symbol"].ToString();

                            DelimiterOptions delimiterOptions = new DelimiterOptions
                            {
                                Id = id,
                                Name = name,
                                Description = description
                            };

                            data.Add(delimiterOptions);
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

        public async Task<IEnumerable<FiledateOptions>> GetFiledateOptionsAsync()
        {
            List<FiledateOptions> data = new List<FiledateOptions>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM FiledateDetailsMaster";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "FiledateDetailsMaster");

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            int id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString());
                            string name = ds.Tables[0].Rows[i]["Name"].ToString();

                            FiledateOptions filedataOptions = new FiledateOptions
                            {
                                Id = id,
                                Name = name,
                            };

                            data.Add(filedataOptions);
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

        public async Task<IEnumerable<FiletypeOptions>> GetFiledtypeOptionsAsync()
        {
            List<FiletypeOptions> data = new List<FiletypeOptions>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM FiletypeDetailsMaster";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "FiletypeDetailsMaster");

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            int id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString());
                            string name = ds.Tables[0].Rows[i]["Name"].ToString();

                            FiletypeOptions filedataOptions = new FiletypeOptions
                            {
                                Id = id,
                                Name = name,
                            };

                            data.Add(filedataOptions);
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

        public async Task<IEnumerable<VendorOptions>> GetVendorOptionsAsync()
        {
            List<VendorOptions> data = new List<VendorOptions>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM VendorDetailsMaster";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "DelimiterDetailsMaster");

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            int id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString());
                            string name = ds.Tables[0].Rows[i]["Name"].ToString();

                            VendorOptions vendorOptions = new VendorOptions
                            {
                                Id = id,
                                Name = name,
                            };

                            data.Add(vendorOptions);
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
