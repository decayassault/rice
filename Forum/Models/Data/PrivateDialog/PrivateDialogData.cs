using System;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Forum.Models;
namespace Forum.Data.PrivateDialog
{
    internal sealed class PrivateDialogData
    {
        internal const int DialogsOnPage = 9;
        private const string SE = "";
        internal async static Task AddDialog(int Num)
        {
            using (var SqlCon = await Connection.GetConnection())
            {
                int count;
                int number = Num;
                using (var cmdThreadsCount =
                    Command.InitializeCommandForInputAccountId
                        (@"GetPrivateMessagesCount", SqlCon, Num + MvcApplication.One))
                {
                    object o = await cmdThreadsCount.ExecuteScalarAsync();
                    if (o == DBNull.Value || o == null)
                        count = MvcApplication.One;
                    else
                        count = Convert.ToInt32(o);
                }
                if (count == MvcApplication.Zero)
                    count++;
                int pagesCount = count / DialogsOnPage;
                if (count - pagesCount * DialogsOnPage > MvcApplication.Zero)
                    pagesCount++;
                PrivateDialogLogic.SetDialogPagesArrayLocked
                                (number, new string[pagesCount]);
                PrivateDialogLogic.SetDialogPagesPageDepthLocked(number, pagesCount);
                string buttonTxt
                    = "<div id='topic'><span><a onClick='newDialog();return false;'>Новый диалог</a></span></div>";

                for (int i = MvcApplication.Zero; i < pagesCount; i++)
                    PrivateDialogLogic
                        .SetDialogPagesPageLocked(number, i, buttonTxt);

               using (var cmdPrivateMessages =
                    Command
                    .InitializeCommandForInputAccountId(@"GetPrivateMessagesAuthors",
                        SqlCon, Num + MvcApplication.One))
                {
                    using (var reader 
                        = await Reader.InitializeReader(cmdPrivateMessages))
                    {
                       await ProcessDialogReader(reader, number, pagesCount);
                    }
                }
            }
        }
        private static string GetArrows
            (int pageNumber, int pagesCount)
        {
            string result = SE;
            
            const string a = "<span id='arrows'><a onClick='g(&quot;/dialog/";
            const string b = "&quot;);return false' title='Первая страница'>«</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/dialog/";

            if ((pageNumber - MvcApplication.One >= MvcApplication.One)
                    && (pageNumber + 3 <= pagesCount))
            {
                result = "<span id='arrows'><a onClick='n(&quot;/dialog/1"
                    + "&quot;);return false' title='Первая страница'>«</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/dialog/"
                    + pageNumber.ToString()
                    + "&quot;);return false' title='Предыдущая страница'>◄</a>"
                    + "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/dialog/" +
              (pageNumber + 2).ToString()
              + "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/dialog/"
              + pagesCount.ToString() + "&quot;);return false' title='Последняя страница'>»</a></span>";
           
            }
            else if ((pageNumber - MvcApplication.One >= MvcApplication.One)
                    && (pageNumber + 2 == pagesCount))
            {
                result = "<span id='arrows'><a onClick='n(&quot;/dialog/1"+
                   "&quot;);return false' title='Первая страница'>«</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/dialog/" +
                   pageNumber.ToString() + "&quot;);return false' title='Предыдущая страница'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/dialog/" +
                (pageNumber + 2).ToString() +
               "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }
            else if ((pageNumber - MvcApplication.One >= MvcApplication.One)
                    && (pageNumber + MvcApplication.One == pagesCount))
            {
                result = "<span id='arrows'><a onClick='n(&quot;/dialog/1" +
                    "&quot;);return false' title='Первая страница'>«</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/dialog/" 
                    + pageNumber.ToString() + "&quot;);return false' title='Предыдущая страница'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }
            else if ((pageNumber == MvcApplication.One)
                    && (pageNumber + 3 <= pagesCount))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<a onClick='g(&quot;/dialog/" +
                   pageNumber.ToString() + "&quot;);return false' title='Предыдущая страница'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/dialog/" +
                (pageNumber + 2).ToString() +
               "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/dialog/"
               + pagesCount.ToString() + "&quot;);return false' title='Последняя страница'>»</a></span>";
            }
            else if ((pageNumber == MvcApplication.Zero)
                    && (pageNumber + 3 <= pagesCount))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/dialog/" +
                (pageNumber + 2).ToString() +
               "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/dialog/"
               +  pagesCount.ToString() + "&quot;);return false' title='Последняя страница'>»</a></span>";
            }
            else if ((pageNumber == MvcApplication.One) && (pagesCount == 3))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<a onClick='n(&quot;/dialog/" +
                   pageNumber.ToString() + "&quot;);return false' title='Предыдущая страница'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/dialog/" +
               (pageNumber + 2).ToString() +
               "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }
            else if ((pageNumber == MvcApplication.One) && (pagesCount == 2))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<a onClick='n(&quot;/dialog/" +
                   pageNumber.ToString() + "&quot;);return false' title='Предыдущая страница'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }
            else if ((pageNumber == MvcApplication.Zero) && (pagesCount == 2))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/dialog/" +
                (pageNumber + 2).ToString() +
               "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }

            return result;
        }
        internal static Task<string> SetNavigation
           (int pageNumber, int pagesCount)
        {
            string result = GetArrows(pageNumber, pagesCount);

            result += "<br />";

            return Task.FromResult(result);
        }
        private async static Task ProcessDialogReader
            (SqlDataReader reader, int number, int pagesCount)
        {
            if (reader.HasRows)
            {
                /*string endpointHidden = string.Empty;
                */
                int pageNumber = MvcApplication.Zero;
                int i = MvcApplication.Zero;
                int AccountId;
                string Nick;
                string text;
                object o = null;
                while
                    (await reader.ReadAsync())
                {
                    if (i == MvcApplication.Zero)
                        PrivateDialogLogic.AddToDialogPagesPageLocked
                            (number, pageNumber, "<nav class='n'><br />");

                    o = reader["Id"];
                    if (o == DBNull.Value || o == null)
                        AccountId = 1;
                    else
                        AccountId = Convert.ToInt32(o);
                    o = reader["Nick"];
                    if (o == DBNull.Value || o == null)
                        Nick = "Пользователь";
                    else
                        Nick = o.ToString();
                    text = "<p onClick='g(&quot;/personal/" 
                        + AccountId.ToString() + "?page=1&quot;);'>"
                        + Nick + "</p><br /><br />";
                    PrivateDialogLogic.AddToDialogPagesPageLocked
                        (number, pageNumber, text);
                    i++;
                                 
                    if (i == DialogsOnPage)
                    {
                        string temp= await SetNavigation
                                (pageNumber, pagesCount);
                        PrivateDialogLogic.AddToDialogPagesPageLocked
                            (number, pageNumber, temp + "</nav>");
                            //+ endpointHidden);

                        i = MvcApplication.Zero;
                        pageNumber++;
                    }
                    else
                        PrivateDialogLogic.AddToDialogPagesPageLocked
                            (number, pageNumber, "<br />");
                }
                
                RemoveBrOfIncompletePages(number);

                if ((i < DialogsOnPage) && (i > MvcApplication.Zero))
                {
                    if (pageNumber > MvcApplication.Zero)
                    {
                        PrivateDialogLogic.AddToDialogPagesPageLocked
                            (number, pageNumber,
                            await SetNavigation(pageNumber, pagesCount));
                    }
                    PrivateDialogLogic.AddToDialogPagesPageLocked
                        (number, pageNumber, "</nav>");
                                //+ endpointHidden);
                }
            }
        }
        private static void RemoveBrOfIncompletePages(int number)
        {
            string temp = PrivateDialogLogic.GetDialogPagesPageLocked(number,
                   PrivateDialogLogic
                    .GetDialogPagesArrayLocked(number).Length - 1);            
            int pos = temp.LastIndexOf("<br />");
            temp = temp.Remove(pos, "<br />".Length);
            PrivateDialogLogic.SetDialogPagesPageLocked(number,
                PrivateDialogLogic
                    .GetDialogPagesArrayLocked(number).Length - 1, temp);
        }
    }
}