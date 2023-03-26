using System;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Forum.Models;

namespace Forum.Data.Section
{
    internal sealed class SectionData
    {
        private const string SE = "";

        internal const int threadsOnPage = 9;

        internal async static Task AddSection(int Num)
        {
            using (var SqlCon = await Connection.GetConnection())
            {
                int count;
                int number=Num;
                using (var cmdThreadsCount = 
                    Command.InitializeCommandForInputEndpointId
                        (@"GetThreadsCount", SqlCon, Num + MvcApplication.One))
                {
                    object o = await cmdThreadsCount.ExecuteScalarAsync();
                    if (o == DBNull.Value||o==null)
                        count = MvcApplication.One;
                    else
                        count = Convert.ToInt32(o);                    
                }
                if (count == MvcApplication.Zero)
                    count++;
                int pagesCount = count / threadsOnPage;
                if (count - pagesCount * threadsOnPage > MvcApplication.Zero)
                    pagesCount++;
                SectionLogic.SetSectionPagesArrayLocked
                                (number,new string[pagesCount]);
                SectionLogic.
                    SetSectionPagesPageDepthLocked(number,pagesCount);
                string buttonTxt 
                    = "<div id='topic'><span><a onClick='newTopic();return false;'>Новая тема</a></span></div>";
                
                    for (int i = MvcApplication.Zero; i < pagesCount; i++)
                        SectionLogic.SetSectionPagesPageLocked(number,i,buttonTxt);                
               
                using (var cmdThreads = 
                    Command.InitializeCommandForInputEndpointId(@"GetThreadsAll",
                        SqlCon,Num+MvcApplication.One))
                {
                    using (var reader = await Reader.InitializeReader(cmdThreads))
                    {
                        await ProcessSectionReader(reader, number,pagesCount);
                    }
                }
            }            
        }
       

        private static string GetArrows
            (int pageNumber, int pagesCount, int number)
        {
            string result = SE;
            string section = (number + MvcApplication.One).ToString();
            const string a = "<span id='arrows'><a onClick='g(&quot;/section/";
            const string b = "?page=1&quot;);return false' title='Первая страница'>«</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/section/";

            if ((pageNumber - MvcApplication.One >= MvcApplication.One)
                    && (pageNumber + 3 <= pagesCount))
            {
                result = a + section +
                    b +
                  section + "?page=" + pageNumber.ToString() + "&quot;);return false' title='Предыдущая страница'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/section/" +
               section + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/section/"
               + section +
                    "?page=" + pagesCount.ToString() + "&quot;);return false' title='Последняя страница'>»</a></span>";
            }
            else if ((pageNumber - MvcApplication.One >= MvcApplication.One)
                    && (pageNumber + 2 == pagesCount))
            {
                result = a + section +
                    b +
                  section + "?page=" + pageNumber.ToString() + "&quot;);return false' title='Предыдущая страница'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/section/" +
               section + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }
            else if ((pageNumber - MvcApplication.One >= MvcApplication.One)
                    && (pageNumber + MvcApplication.One == pagesCount))
            {
                result = a + section +
                    b +
                  section + "?page=" + pageNumber.ToString() + "&quot;);return false' title='Предыдущая страница'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }
            else if ((pageNumber == MvcApplication.One) 
                    && (pageNumber + 3 <= pagesCount))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<a onClick='g(&quot;/section/" +
                  section + "?page=" + pageNumber.ToString() + "&quot;);return false' title='Предыдущая страница'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/section/" +
               section + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/section/"
               + section +
                    "?page=" + pagesCount.ToString() + "&quot;);return false' title='Последняя страница'>»</a></span>";
            }
            else if ((pageNumber == MvcApplication.Zero) 
                    && (pageNumber + 3 <= pagesCount))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/section/" +
               section + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/section/"
               + section +
                    "?page=" + pagesCount.ToString() + "&quot;);return false' title='Последняя страница'>»</a></span>";
            }
            else if ((pageNumber == MvcApplication.One) && (pagesCount == 3))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<a onClick='g(&quot;/section/" +
                  section + "?page=" + pageNumber.ToString() + "&quot;);return false' title='Предыдущая страница'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/section/" +
               section + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }
            else if ((pageNumber == MvcApplication.One) && (pagesCount == 2))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<a onClick='g(&quot;/section/" +
                  section + "?page=" + pageNumber.ToString() + "&quot;);return false' title='Предыдущая страница'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }
            else if ((pageNumber == MvcApplication.Zero) && (pagesCount == 2))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/section/" +
               section + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }

            return result;
        }

        internal static Task<string> SetNavigation
            (int pageNumber, int pagesCount, int number)
        {
            string result = GetArrows(pageNumber,pagesCount,number);
            
            result+="<br />";

            return Task.FromResult(result);
        }
        private static void RemoveBrOfIncompletePages(int number)
        {
            string temp = SectionLogic.GetSectionPagesPageLocked(number,
                   SectionLogic.GetSectionPagesArrayLocked(number).Length - 1);
            int pos = temp.LastIndexOf("<br />");
            temp = temp.Remove(pos, "<br />".Length);
            SectionLogic.SetSectionPagesPageLocked(number,
                SectionLogic.GetSectionPagesArrayLocked(number).Length - 1,temp);
        }
        
        private async static Task ProcessSectionReader
            (SqlDataReader reader, int number,int pagesCount)
        {
            if (reader.HasRows)
            {
                string endpointHidden = string.Empty;
                string text;
                int pageNumber = MvcApplication.Zero;
                int i = MvcApplication.Zero;

                while
                    (await reader.ReadAsync())
                {                    
                    if (i == MvcApplication.Zero)
                        SectionLogic.AddToSectionPagesPageLocked
                            (number,pageNumber,"<nav class='n'><br />");
                    object Id = reader["Id"];
                    int id_;
                    if (Id == DBNull.Value||Id==null)
                        id_ = MvcApplication.One;
                    else
                        id_ = Convert.ToInt32(Id);
                    endpointHidden = "<div class='s'>"
                        + (await ThreadData.GetSectionNum(id_-1)).ToString()+"</div>";
                    var Name = reader["Name"];
                    text = "<p onClick='g(&quot;/thread/" + Id.ToString() + "?page=1&quot;);'>" + Name + "</p><br /><br />";
                    SectionLogic.AddToSectionPagesPageLocked
                        (number,pageNumber,text);
                    i++;

                    if (i == threadsOnPage)
                    {
                        string test = await SetNavigation
                                (pageNumber, pagesCount, number);
                        SectionLogic.AddToSectionPagesPageLocked
                            (number,pageNumber,test+ "</nav>"
                            + endpointHidden);

                        i = MvcApplication.Zero;
                        pageNumber++;
                    }
                    else
                        SectionLogic.AddToSectionPagesPageLocked
                            (number,pageNumber, "<br />");                                         
                }

                RemoveBrOfIncompletePages(number);

                if ((i < threadsOnPage) && (i > MvcApplication.Zero))
                {
                    if (pageNumber > MvcApplication.Zero)
                    {
                        SectionLogic.AddToSectionPagesPageLocked
                            (number,pageNumber,
                            await SetNavigation(pageNumber, pagesCount, number));                        
                    }
                    SectionLogic.AddToSectionPagesPageLocked
                        (number,pageNumber,"</nav>"
                                + endpointHidden);                    
                }
            }                     
        }

    }
}