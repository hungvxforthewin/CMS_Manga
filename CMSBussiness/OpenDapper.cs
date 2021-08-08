using System.Data;
using System.Data.SqlClient;

namespace CRMBussiness
{
    public class OpenDapper
    {
        public static string connectionStr = "";
        public static IDbConnection db = new SqlConnection(connectionStr);
        public static void ConnectStr(string connectionString)
        {
            connectionStr = connectionString;
        }
    }
}
