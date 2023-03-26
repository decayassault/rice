using Forum.Models;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Forum.Data.Forum
{
    internal sealed class ForumData
    {
        private const string SE = "";

        internal async static Task<string> LoadForumsOnMain()
        {
            string result = SE;
            using (var SqlCon = await Connection.GetConnection())
            {
                using (var cmdForums = 
                    Command.InitializeCommand(@"GetForums", SqlCon))
                {
                    using (var reader = 
                        await Reader.InitializeReader(cmdForums))
                    {
                        result = await ProcessMainPageReader(reader);
                    }
                }
            }

            return result;
        }

        private async static Task<string> 
            ProcessMainPageReader(SqlDataReader reader)
        {
            string result = SE;
            if (reader.HasRows)
                while (await reader.ReadAsync())
                {
                    var Id = Convert.ToInt32(reader[@"Id"]);
                    var Name = reader[@"Name"].ToString();
                    string elId = Id.ToString();
                    result += "<br /><span onClick='t(&quot;/endpoint/"+elId
                        +"&quot;,&quot;e"+elId
                        + "&quot;);'>" + Name + "</span><div id='e"
                        + elId + "'><br /></div>";                    
                }            

            return result;
        }

        [Obsolete]
        private static async Task<string> ProcessMainPageThreads(int Id)
        {
            string result = SE;
            using (var SqlCon = await Connection.GetConnection())
            {
                using (var cmdThreads = 
                    Command.InitializeCommandForInputEndpointId
                        (@"GetThreadsTop5", SqlCon,Id))
                {
                    using (var reader = await Reader.InitializeReader(cmdThreads))
                    {
                        result = await ProcessMainPageEachThread(reader);
                    }
                }
            }

            return result;
        }

        private async static Task<string> 
            ProcessMainPageEachThread(SqlDataReader reader)
        {
            string result = SE;
            if (reader.HasRows)
                while (await reader.ReadAsync())
                {
                    var Id = Convert.ToInt32(reader[@"Id"]);
                    var Name = reader[@"Name"].ToString();
                    result += "<p onClick='g(&quot;/thread/"
                        + Id.ToString() + "?page=1&quot;);'>" + Name + "</p><br />";                    
                }
            result += "<br /></div>";

            return result;
        }

    }
}