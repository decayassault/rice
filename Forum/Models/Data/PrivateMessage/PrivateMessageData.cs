using System.Threading.Tasks;
using Forum.Models;
using Forum.Data.Thread;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;
namespace Forum.Data.PrivateMessage
{
    internal sealed class PrivateMessageData
    {
        private const int five = 5;
        private async static Task PutPrivateMessageInBase
            (int senderAccId, int acceptorAccId, string privateText)
        {
            using (var SqlCon = await Connection.GetConnection())
            {
                using (var cmdAddPrivateMessage =
                    Command.InitializeCommandForPutPrivateMessage
                        (@"AddPrivateMessage", SqlCon, senderAccId,
                        acceptorAccId
                        , privateText))
                {
                    await cmdAddPrivateMessage.ExecuteNonQueryAsync();
                }
            }
        }
        internal async static
            Task<PrivateMessageLogic.CompanionId[]> GetCompanions(int accountId)
        {
            PrivateMessageLogic.CompanionId[] result = null;

            using (var SqlCon = await Connection.GetConnection())
            {
                using (var cmdMessages =
                    Command.InitializeCommandForInputAccountId
                        (@"GetPrivateMessagesCompanions", SqlCon, accountId))
                {
                    using (var reader = await Reader.InitializeReader(cmdMessages))
                    {
                        result=await ProcessPrivateMessagesReader
                            (reader);
                    }
                }
                
            }
            return result;
        }
        internal async static
            Task<PrivateMessageLogic.PrivateMessages> 
                GetMessages(int companionId, int accountId)
        {   PrivateMessageLogic.PrivateMessages result;

            using (var SqlCon = await Connection.GetConnection())
            {
                int count;                
                object o = null;

                using (var cmdIdsCount =
                    Command.InitializeCommandForInputIds
                        (@"GetPrivateMessagesAuthorsCount",
                            SqlCon, companionId,accountId))
                {
                    o = await cmdIdsCount.ExecuteScalarAsync();
                }

                if (o == DBNull.Value || o == null)
                    count = MvcApplication.One;
                else
                    count = Convert.ToInt32(o);
                if (count == MvcApplication.Zero)
                    count++;
                int pagesCount = count / five;
                if (count - pagesCount * five > MvcApplication.Zero)
                    pagesCount++;
                

                using (var cmdMessages =
                    Command.InitializeCommandForInputIds
                        (@"GetPrivateMessagesTexts", SqlCon, companionId, accountId))
                {
                    using (var reader = await Reader.InitializeReader(cmdMessages))
                    {
                        result = await ProcessCompanionPrivateMessagesReader
                            (reader,companionId,accountId, pagesCount);
                    }
                }

            }
            return result;
        }
        private async static Task<PrivateMessageLogic.CompanionId[]>
                    ProcessPrivateMessagesReader
                        (SqlDataReader reader)
        {
            List<PrivateMessageLogic.CompanionId> result =
                new List<PrivateMessageLogic.CompanionId>();
            if (reader.HasRows)
            {
                int AccountId;
                object o = null;
                while
                    (await reader.ReadAsync())
                {
                    o = reader["Id"];
                    if (o == DBNull.Value || o == null)
                        AccountId = 1;
                    else
                        AccountId = Convert.ToInt32(o);
                    PrivateMessageLogic.CompanionId temp =
                        new PrivateMessageLogic.CompanionId { Id = AccountId };
                    if (!result.Contains(temp))
                        result.Add(temp);
                }                
            }
            return result.ToArray();
        }
        internal static string GetArrows
            (int pageNumber, int pagesCount, int companionId)
        {
            string result = ThreadLogic.SE;
            string personal = companionId.ToString();
            const string a = "<span id='arrows'><a onClick='g(&quot;/personal/";
            const string b = "?page=1&quot;);return false' title='Первая страница'>«</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/personal/";

            if ((pageNumber - MvcApplication.One >= MvcApplication.One)
                    && (pageNumber + 3 <= pagesCount))
            {
                result = a + personal +
                    b +
                  personal + "?page=" + pageNumber.ToString() + "&quot;);return false' title='Предыдущая страница'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/personal/" +
               personal + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/personal/"
               + personal +
                    "?page=" + pagesCount.ToString() + "&quot;);return false' title='Последняя страница'>»</a></span>";
            }
            else if ((pageNumber - MvcApplication.One >= MvcApplication.One)
                    && (pageNumber + 2 == pagesCount))
            {
                result = a + personal +
                    b +
                  personal + "?page=" + pageNumber.ToString() + "&quot;);return false' title='Предыдущая страница'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/personal/" +
               personal + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }
            else if ((pageNumber - MvcApplication.One >= MvcApplication.One)
                && (pageNumber + MvcApplication.One == pagesCount))
            {
                result = a + personal +
                    b +
                  personal + "?page=" + pageNumber.ToString() + "&quot;);return false' title='Предыдущая страница'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }
            else if ((pageNumber == MvcApplication.One) && (pageNumber + 3 <= pagesCount))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<a onClick='g(&quot;/personal/" +
                  personal + "?page=" + pageNumber.ToString() + "&quot;);return false' title='Предыдущая страница'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/personal/" +
               personal + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/personal/"
               + personal +
                    "?page=" + pagesCount.ToString() + "&quot;);return false' title='Последняя страница'>»</a></span>";
            }
            else if ((pageNumber == MvcApplication.Zero) && (pageNumber + 3 <= pagesCount))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/personal/" +
               personal + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/personal/"
               + personal +
                    "?page=" + pagesCount.ToString() + "&quot;);return false' title='Последняя страница'>»</a></span>";
            }
            else if ((pageNumber == MvcApplication.One) && (pagesCount == 3))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<a onClick='g(&quot;/personal/" +
                  personal + "?page=" + pageNumber.ToString() + "&quot;);return false' title='Предыдущая страница'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/personal/" +
               personal + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }
            else if ((pageNumber == MvcApplication.One) && (pagesCount == 2))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<a onClick='g(&quot;/personal/" +
                  personal + "?page=" + pageNumber.ToString() + "&quot;);return false' title='Предыдущая страница'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }
            else if ((pageNumber == MvcApplication.Zero) && (pagesCount == 2))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/personal/" +
               personal + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }

            return result;
        }

        private static Task<string> SetNavigation
            (int pageNumber, int pagesCount, int authorId, string companionNick)
        {
            string result = GetArrows(pageNumber, pagesCount, authorId);

            result += "<div id='a'><a onClick='replyPM();return false'>Ответить "
                +companionNick+"</a></div>";

            return Task.FromResult(result);
        }


        private async static Task<PrivateMessageLogic.PrivateMessages>
                    ProcessCompanionPrivateMessagesReader
                        (SqlDataReader reader, int companionId,
                        int accountId, int pagesCount)
        {
            var result = new PrivateMessageLogic.PrivateMessages 
                { Messages = new string[pagesCount] };
            
            int pageNumber = MvcApplication.Zero;
            string companionNick = await ThreadData.GetNick(companionId);
            string accountNick = await ThreadData.GetNick(accountId);
            string dialogName = "Переписка с " + companionNick;
            
            result.Messages[pageNumber]="<div class='s'>" + companionId.ToString() +
                    "</div><div class='l'><h2 onClick='g(&quot;/dialog/1&quot;);'>"
                    + dialogName + "</h2>";           
            
            bool first = MvcApplication.False;
           
            if (reader.HasRows)
            {
                int authorId;
                string text;
                int i = MvcApplication.Zero;
                string privateText;                
                object o = null;
                while
                    (await reader.ReadAsync())
                {
                    if (i == MvcApplication.Zero && first)
                    {                        
                        result.Messages[pageNumber]+="<div class='s'>" 
                            + companionId.ToString() +
                         "</div><div class='l'><h2 onClick='g(&quot;/dialog/1" +
                         "&quot;);'>" + dialogName + "</h2>";
                    }

                    o = reader["SenderAccountId"];
                    if (o == DBNull.Value || o == null)
                        authorId = 1;
                    else 
                        authorId = Convert.ToInt32(o);

                    o = reader["PrivateText"];
                    if (o == DBNull.Value || o == null)
                        privateText = ThreadLogic.SE;
                    else
                        privateText = o.ToString();

                    string nick;
                    if (authorId == companionId)
                        nick = companionNick;
                    else
                        nick = accountNick;

                    text = "<article><span onClick='g(&quot;/Profile/" +
                        authorId.ToString() + "&quot;);'>" + nick + "</span><br />";
                    text += "<p>" + privateText + "</p></article><br />";
                    result.Messages[pageNumber]+=text;

                    i++;

                    if (i == five)
                    {
                        string test = 
                            await SetNavigation
                                (pageNumber, pagesCount, companionId,companionNick);
                        result.Messages[pageNumber]+= test;
                        if (first)
                            result.Messages[pageNumber]
                                += "</div><div class='s'>0</div>";

                        i = MvcApplication.Zero;
                        pageNumber++;
                    }

                    if (!first)
                        first = MvcApplication.True;

                    
                }
                if ((pageNumber >= MvcApplication.Zero)
                        && (i < five) && (i > MvcApplication.Zero))
                {
                    result.Messages[pageNumber] +=
                                await SetNavigation
                                (pageNumber, pagesCount, companionId, companionNick);
                    if (first)
                        result.Messages[pageNumber] += "</div><div class='s'>" +
                            (5 - i).ToString() + "</div>";
                }
                
            }
            if (!first)
                result.Messages[pageNumber]+= "</div>";

            return result;
        }
    }
}