using System.Data.SqlClient;

namespace FMS.Services.AzueFileUploadAPI.DBContext
{
    public class DBConnect
    {
        private readonly string connectionString;

        public DBConnect(IConfiguration configuration)
        {
            connectionString = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
        }

        public SqlConnection EstablishConnection()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return connection;
            }

        }
    }
}
