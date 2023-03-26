namespace Forum.Models
{
    using System;
    using System.Collections.Concurrent;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Threading.Tasks;
    internal sealed class Connection
    {
        internal static
            ConcurrentQueue<SqlConnection> ConnectionsCache;
        internal static SecureString cString;
        private const int ConnectionsCacheSize = 100;        

        internal async static Task InitializeConnectionsCache()
        {
            ConnectionsCache =
                new ConcurrentQueue<SqlConnection>();

            for (int i = MvcApplication.Zero; i < ConnectionsCacheSize; i++)
            {
                await AddConnectionToCache();
            }
        }

        internal async static
            Task<SqlConnection> GetConnection()
        {
            await AddConnectionToCache();
            SqlConnection result;
            ConnectionsCache.TryDequeue(out result);

            return result;
        }

        internal async static Task<SqlConnection> InitializeConnection()
        {
            SqlConnection sc;
            sc = new SqlConnection();
            sc.ConnectionString = await DecryptString(cString);
            sc.StatisticsEnabled = MvcApplication.False;
            await sc.OpenAsync();

            return sc;
        }
        internal static Task<string> DecryptString(SecureString s)
        {

            IntPtr stringPointer = Marshal.SecureStringToBSTR(s);
            string result = null;
            try
            {
                result = Marshal.PtrToStringBSTR(stringPointer);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(stringPointer);
            }

            return Task.FromResult(result);
        }

        internal static SecureString SecureStr(string s)
        {
            var result = new SecureString();
            for (int i = MvcApplication.Zero; i < s.Length; i++)
                result.AppendChar(s[i]);
            result.MakeReadOnly();
            return result;
        }

        private async static Task AddConnectionToCache()
        {
            SqlConnection sc;
            sc = await InitializeConnection();
            ConnectionsCache.Enqueue(sc);
        }

        
    }
}