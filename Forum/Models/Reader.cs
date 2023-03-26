namespace Forum.Models
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    public class Reader
    {
        internal async static
            Task<SqlDataReader> InitializeReader(SqlCommand cmd)
        {
            while (cmd.Connection.State != ConnectionState.Open) { }
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            
            return reader;
        }
    }
}