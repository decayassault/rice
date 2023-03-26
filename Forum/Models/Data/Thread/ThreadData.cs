using System;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Forum.Data.Thread;
using Forum.Models;

namespace Forum.Data
{
    internal sealed class ThreadData
    {
        private const int five = 5;  

        internal static string GetThreadPage(int Id,int page)
        {
            if (Id > MvcApplication.Zero && Id <= ThreadLogic.ThreadPagesLength)
            {
                int index = Id - MvcApplication.One;
                if (page > MvcApplication.Zero
                    && page <= ThreadLogic.ThreadPagesPageDepth[index])
                    return ThreadLogic.ThreadPages[index][page - MvcApplication.One];
                else return ThreadLogic.SE;
            }
            else
                return ThreadLogic.SE;
        }

        internal async static Task LoadThreadPages() //1 min 37 sec
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            
                await ThreadLogic.LoadEachThreadPage();

            sw.Stop();
            TimeSpan t = sw.Elapsed;
        }
        
        internal async static Task AddThread(int Num)//4 sec
        {

            using (var SqlCon = await Connection.GetConnection())
            {
                int count;
                int number = Num;
                object o = null;
                
                using (var cmdThreadsCount =
                    Command.InitializeCommandForInputThreadId
                        (@"GetMessagesCount", SqlCon, Num + MvcApplication.One))
                {
                    o = await cmdThreadsCount.ExecuteScalarAsync();                    
                }
                
                if (o == DBNull.Value)
                    count = MvcApplication.One;
                else
                    count = Convert.ToInt32(o);
                if (count == MvcApplication.Zero)
                    count++;
                int pagesCount = count / five;
                if (count - pagesCount * five > MvcApplication.Zero)
                    pagesCount++;
                ThreadLogic.ThreadPages[number] = new string[pagesCount];
                ThreadLogic.ThreadPagesPageDepth[number] = pagesCount;

                int sectionNum = await GetSectionNum(Num);
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                using (var cmdThreads =
                    Command.InitializeCommandForInputThreadId
                        (@"GetMessages", SqlCon, Num + MvcApplication.One))
                {
                    using (var reader = await Reader.InitializeReader(cmdThreads))
                    {
                        await ProcessThreadReader
                            (reader, number,pagesCount,sectionNum);
                    }
                }
                sw.Stop();
                TimeSpan t = sw.Elapsed;
            }
            
        }

        internal async static Task<int> GetSectionNum(int num)//2 sec
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            int result = MvcApplication.One;
            using (var SqlCon = await Connection.GetConnection())
            {
            using (var cmdSection = 
                    Command.InitializeCommandForInputThreadId
                        (@"GetThreadSection", SqlCon, num + MvcApplication.One))
                {
                    object oo;
                      oo= await cmdSection.ExecuteScalarAsync();
                      if (oo == DBNull.Value)
                          result = MvcApplication.One;
                      else result = Convert.ToInt32(oo);
                }
            }
            sw.Stop();
            TimeSpan t = sw.Elapsed;
            return result;
        }

        internal static string GetArrows
            (int pageNumber,int pagesCount,int number)
        {
            string result = ThreadLogic.SE;
            string thread = (number + MvcApplication.One).ToString();
            const string a = "<span id='arrows'><a onClick='g(&quot;/thread/";
            const string b = "?page=1&quot;);return false'>«</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/thread/";

            if ((pageNumber - MvcApplication.One >= MvcApplication.One)
                    && (pageNumber + 3 <= pagesCount))
            {
                result = a + thread +
                    b +
                  thread + "?page=" + pageNumber.ToString() + "&quot;);return false'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/thread/" +
               thread + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/thread/"
               + thread +
                    "?page=" + pagesCount.ToString() + "&quot;);return false'>»</a></span>";
            }
            else if ((pageNumber - MvcApplication.One >= MvcApplication.One)
                    && (pageNumber + 2 == pagesCount))
            {
                result = a + thread +
                    b +
                  thread + "?page=" + pageNumber.ToString() + "&quot;);return false'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/thread/" +
               thread + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }
            else if ((pageNumber - MvcApplication.One >= MvcApplication.One)
                && (pageNumber + MvcApplication.One == pagesCount))
            {
                result = a + thread +
                    b +
                  thread + "?page=" + pageNumber.ToString() + "&quot;);return false'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }
            else if ((pageNumber == MvcApplication.One) && (pageNumber + 3 <= pagesCount))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<a onClick='g(&quot;/thread/" +
                  thread + "?page=" + pageNumber.ToString() + "&quot;);return false'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/thread/" +
               thread + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/thread/"
               + thread +
                    "?page=" + pagesCount.ToString() + "&quot;);return false'>»</a></span>";
            }
            else if ((pageNumber == MvcApplication.Zero) && (pageNumber + 3 <= pagesCount))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/thread/" +
               thread + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/thread/"
               + thread +
                    "?page=" + pagesCount.ToString() + "&quot;);return false'>»</a></span>";
            }
            else if ((pageNumber == MvcApplication.One) && (pagesCount == 3))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<a onClick='g(&quot;/thread/" +
                  thread + "?page=" + pageNumber.ToString() + "&quot;);return false'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/thread/" +
               thread + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }
            else if ((pageNumber == MvcApplication.One) && (pagesCount == 2))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<a onClick='g(&quot;/thread/" +
                  thread + "?page=" + pageNumber.ToString() + "&quot;);return false'>◄</a>";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }
            else if ((pageNumber == MvcApplication.Zero) && (pagesCount == 2))
            {
                result = "<span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;";
                result += "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/thread/" +
               thread + "?page=" + (pageNumber + 2).ToString() +
               "&quot;);return false'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>";
            }

            return result;
        }

        private static Task<string> SetNavigation
            (int pageNumber,int pagesCount, int number)
        {
            string result = GetArrows(pageNumber,pagesCount,number);

            result += "<div id='a'><a onClick='u();return false'>Ответить</a></div>";//br1

            return Task.FromResult(result);
        }

        internal async static Task<string> GetThreadName(int number)
        {
            string result = ThreadLogic.SE;

            using (var SqlCon = await Connection.GetConnection())
            {                               
                object o = null;
                using (var cmdThreadName = 
                    Command.InitializeCommandForInputThreadId
                        (@"GetThreadName", SqlCon, number + MvcApplication.One))
                {
                    o = await cmdThreadName.ExecuteScalarAsync();
                }
                if (o == DBNull.Value)
                    result = "undefined";
                else
                    result = o.ToString();
            }

            return result;
        }

        private async static Task ProcessThreadReader
            (SqlDataReader reader, int number,int pagesCount, int sectionNum)
        {

            int pageNumber = MvcApplication.Zero;  
            string threadName = await GetThreadName(number);
            ThreadLogic.ThreadPages[number][pageNumber] += "<div class='s'>" + number.ToString() +
            "</div><div class='l'><h2 onClick='g(&quot;/section/" +
                sectionNum.ToString() + "?page=1&quot;);'>" + threadName + "</h2>";
            bool first = MvcApplication.False;
            
            if (reader.HasRows)
            {
                string text;

                int i = MvcApplication.Zero;
                
                while
                    (await reader.ReadAsync())
                {
                    if (i == MvcApplication.Zero && first)
                    {
                        ThreadLogic.ThreadPages[number][pageNumber] += "<div class='s'>" + number.ToString() +
                         "</div><div class='l'><h2 onClick='g(&quot;/section/" +
                         sectionNum.ToString() + "?page=1&quot;);'>" + threadName + "</h2>";
                    }
                    var MsgText = reader["MsgText"];
                    int AccountId=(int)reader["AccountId"];
                    var Nick = await GetNick(AccountId);
                    text = "<article><span onClick='g(&quot;/Profile/"+
                        AccountId.ToString()+"&quot;);'>"+Nick+"</span><br />";
                    text += "<p>" + MsgText + "</p></article><br />";//br2
                    ThreadLogic.ThreadPages[number][pageNumber] += text;
                    i++;
                    
                    if (i == five)
                    {
                        
                        string test=await SetNavigation(pageNumber,pagesCount,number);
                        ThreadLogic.ThreadPages[number][pageNumber] += test;
                        if (first)
                            ThreadLogic.ThreadPages[number][pageNumber] += "</div><div class='s'>0</div>";

                        i = MvcApplication.Zero;
                        pageNumber++;                        
                    }               
                    
                    if(!first)
                        first = MvcApplication.True;
                }
                if ((pageNumber >= MvcApplication.Zero) 
                        && (i < five) && (i > MvcApplication.Zero))
                {
                    ThreadLogic.ThreadPages[number][pageNumber] +=
                            await SetNavigation(pageNumber, pagesCount, number);
                    if (first)
                        ThreadLogic.ThreadPages[number][pageNumber] += "</div><div class='s'>" +
                            (5-i).ToString()+"</div>";
                }
                
            }
            if(!first)
                ThreadLogic.ThreadPages[number][pageNumber] += "</div>";
        }

        internal async static Task<string> GetNick(int AccountId)
        {
            string result = ThreadLogic.SE;
            object o=null;
            using (var SqlCon = await Connection.GetConnection())
            {
              using (var cmdThreads =
                    Command.InitializeCommandForInputAccountId(@"GetNick", SqlCon,AccountId))
                {
                    o = await cmdThreads.ExecuteScalarAsync();
                }
            }
        
            if (o == DBNull.Value)
                result = "undefined";
            else result = o.ToString();

            return result;
        }
        
        internal async static Task<int> EncountThreads()
        {
            int result;
            object o=null;
            using (var SqlCon = await Connection.GetConnection())
            {
                using (var cmdThreads = 
                    Command.InitializeCommand(@"GetAllThreadsCount", SqlCon))
                {
                    o = await cmdThreads.ExecuteScalarAsync();
                }
            }
            if (o == DBNull.Value)
                result = MvcApplication.Zero;
            else result = Convert.ToInt32(o);

            return result;
        }
    }
}