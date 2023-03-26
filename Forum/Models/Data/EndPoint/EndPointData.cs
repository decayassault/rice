using System;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Forum.Models;

namespace Forum.Data.EndPoint
{
    internal sealed class EndPointData
    {
        private const string SE = "";
        private const string 
            GetEndpointsTop5Procedure = @"GetEndPointsTop5";     

        internal async static Task AddEndPoint(int Num)
        {
            using (var SqlCon = await Connection.GetConnection())
            {                
                int number = Num;              

                using (var cmdEndPoints = 
                    Command.InitializeCommandForInputForumId
                        (GetEndpointsTop5Procedure, SqlCon,
                            Num + MvcApplication.One))
                {
                    using (var reader = await Reader.InitializeReader(cmdEndPoints))
                    {
                        await ProcessEndPointReader(reader, number);
                    }
                }
            }
        }

        private async static Task ProcessEndPointReader
            (SqlDataReader reader, int number)
        {
            string text=SE;
            if (reader.HasRows)
            {                                
                int id;
                string name;

                while
                    (await reader.ReadAsync())
                {
                    object o = reader["Id"];
                    if (o == DBNull.Value)
                        id = MvcApplication.Zero;
                    else id = Convert.ToInt32(o);

                    o = reader["Name"];
                    if(o==DBNull.Value)
                        name="endpoint";
                    else name=o.ToString();

                    text += "<p onClick='g(&quot;/section/" + 
                        id.ToString() + "?page=1&quot;);'>" + name + "</p><br />";                    
                }
                   
                }     
            EndPointLogic.EndPointPages[number]=text;
            }
        }
    }
