using System.Threading.Tasks;
using Forum.Data.Thread;
using Forum.Models;
using Forum.Data.Account;
using Forum.Data.Section;
using System.Collections.Concurrent;
using System;
namespace Forum.Data
{
 
    internal sealed class NewTopicData
    {        
        internal const string PageToReturn= "<p>Заголовок темы:</p>" +
                "<input type='text' tabindex='0' autofocus required maxlength='99' autocomplete='off' />" +
                "<p>Текст сообщения:</p>" +
                "<textarea id='text' tabindex='1' maxlength='1000' wrap='soft' spellcheck='true'></textarea>"+
                "<div id='msg'></div><br /><span><a id='send' onClick='startTopic();return false'>Отправить</a></span>";
        internal struct TopicData
        {
            internal string threadName;
            internal int? endpointId;
            internal string username;
            internal string message;
        }
        internal static ConcurrentQueue<TopicData> TopicsToStart;
        private static string[] pages;
        private static int threadsCount;
        private static int pos;
        private static string temp;
        private const string pMarker = "<p";
        private const string navMarker="<nav class='n'>";
        private const string spanMarker = "<span";
        private const string fullSpanMarker = "<span id='arrows'>";
        private const string endLinkMarker = "»";
        private const string pageMarker = "?page=";
        private const string nextLinkMarker = "►";
        private const string brMarker = "<br />";
        private const string endNavMarker = "</nav>";
        internal static void AllowNewTopics()
        {
            TopicsToStart = new ConcurrentQueue<TopicData>();
        }
        private async static Task<int> PutTopicInBase
            (string threadName, int endpointId,
                int accountId, string message)
        {
            using (var SqlCon = await Connection.GetConnection())
            {
                using (var cmdStartTopic =
                    Command.InitializeCommandForStartTopic
                        (@"StartTopic", SqlCon,
                        threadName, endpointId,accountId,
                         message))
                {
                    await cmdStartTopic.ExecuteNonQueryAsync();
                    return (int)(cmdStartTopic
                        .Parameters[Command.ThreadIdParameter].Value);
                }
            }
        }
        private async static Task PublishTopic
                (string threadName,int endpointId,
                    string username, string message)
        {
            int accountId = await ReplyData.GetAccountId(username);
            string nick = await ThreadData.GetNick(accountId); 
            int threadId = await PutTopicInBase
                (threadName,endpointId,accountId,message);
            CorrectSectionArray(endpointId, threadId,threadName);            
            CorrectMessagesArray(endpointId, threadId,
                        message, accountId,threadName,nick);
        }
        private static void CorrectMessagesArray
            (int endpointId,int threadId,string message,
                int accountId,string threadName,string nick)
        {
            string[][] threadPages = ThreadLogic.GetThreadPagesLocked();
            string newPage = "<div class='s'>" + (threadId-1).ToString()
                + "</div><div class='l'>" +
                "<h2 onclick='g(&quot;/section/" + endpointId.ToString()
                + "?page=1&quot;);'>" + threadName + "</h2><article>"
                + "<span onclick='g(&quot;/profile/" + accountId.ToString()
                + "&quot;);'>"+nick+"</span><br /><p>" + message + "</p></article><br />"
                + "<div id='a'><a onclick='u();return false'>Ответить</a>" +
                "</div></div><div class='s'>4</div>";
            ThreadLogic.SetThreadPagesLengthLocked(threadId);
            int[] depthOld = ThreadLogic.GetThreadPagesPageDepthLocked();
            int[] depthNew=new int[threadId];
            depthOld.CopyTo(depthNew, MvcApplication.Zero);
            depthNew[threadId - MvcApplication.One] = MvcApplication.One;
            ThreadLogic.InitializeThreadPagesPageDepthLocked(depthNew);
            string[][] threadPagesNew = new string[threadId][];
            threadPages.CopyTo(threadPagesNew, MvcApplication.Zero);
            threadPagesNew[threadId - 1] = new string[] { newPage };
            ThreadLogic.InitializeThreadPagesLocked(threadPagesNew);
        }
        private static void CorrectSectionArray
                                (int endpointId,int threadId,string threadName)
        {
            pages = SectionLogic.GetSectionPagesArrayLocked(endpointId-1);
            threadsCount = CountStringOccurrences
                    (pages[pages.Length-MvcApplication.One], pMarker);
            if(pages.Length>1)
            {
                if(threadsCount==SectionData.threadsOnPage)
                {
                    AddThreadToFull(threadId, threadName,endpointId);
                }
                else
                {
                    AddThreadToBegin(threadId,threadName);
                }
            }
            else
            {                
                if(threadsCount==SectionData.threadsOnPage)
                {
                    AddThreadToSingle(threadId,threadName,endpointId);
                }
                else
                {
                    AddThread
                        (MvcApplication.Zero,threadId,threadName);
                }
            }
            SectionLogic.SetSectionPagesArrayLocked
                                (endpointId - 1,pages);
            
        }
        private static void AddThreadToSingle
            (int threadId,string threadName,int endpointId)
        {
            ThreadPreload(threadId, threadName, endpointId);
            MoveThreadAndSetArrows(threadId, endpointId);
        }
        private static void ThreadPreload
            (int threadId,string threadName,int endpointId)
        {
            string[] temp = new string[pages.Length + MvcApplication.One];
            pages.CopyTo(temp, MvcApplication.Zero);
            pages = temp;
            int len = pages.Length;
            SectionLogic.SetSectionPagesPageDepthLocked(endpointId - 1,len);
            pages[len - 1] = "<div id='topic'><span><a onclick='newTopic();return false;'>Новая тема</a>" +
                        "</span></div><nav class='n'></nav><div class='s'>"
                        + endpointId.ToString() + "</div></div>";
            AddThread(MvcApplication.Zero, threadId, threadName);            
        }
        private static void MoveThreadAndSetArrows
            (int threadId,int endpointId)
        {
            SetArrowsForSingle(threadId, endpointId);
            MoveThreadToNext();
        }
        private static void MoveThreadToNext()
        {
            GetLastThread(MvcApplication.Zero);
            SetFirstThread(MvcApplication.One, temp);
        }
        private async static void SetArrowsForSingle(int threadId,int endpointId)
        {
            int pagesCount = SectionLogic
                .GetSectionPagesPageDepthLocked(endpointId - MvcApplication.One);
            string[] arrows=new string[pagesCount];
            int pos;
            for (int i = MvcApplication.Zero; i < pagesCount; i++)
            {
                arrows[i] = await SectionData
                           .SetNavigation(i, pagesCount, endpointId - 1);
            }
            for(int i=0;i<arrows.Length;i++)
            {
                pos = pages[i].LastIndexOf(endNavMarker);
                pages[i] = pages[i].Insert(pos, arrows[i]);
            }            
        }
        private static void AddThreadToFull
            (int threadId,string threadName,int endpointId)
        {
            ThreadPreload(threadId, threadName, endpointId);
            MoveThreadsAndSetArrows(threadId,endpointId);            
        }
        private static void MoveThreadsAndSetArrows
                (int threadId,int endpointId)
        {
            SetArrows(threadId,endpointId);
            MoveThreadsInFull(threadId,endpointId);            
        }
        private static void MoveThreadsInFull(int threadId,int endpointId)
        {
            int len=pages.Length;
            int i=0;
            string lastThread;
            while(i<len)
            {
                GetLastThread(i);
                lastThread=temp;
                i++;
                SetFirstThread(i,lastThread);
            }            
        }
        private static void SetFirstThread(int i, string lastThread)
        {
            int pos = pages[i].IndexOf(navMarker)+navMarker.Length;
            pages[i] = pages[i].Insert(pos, lastThread);
        }
        private async static void SetArrows(int threadId,int endpointId)
        {
            int pagesCount = SectionLogic
                .GetSectionPagesPageDepthLocked(endpointId - MvcApplication.One);
            string arrows;
            for(int i=MvcApplication.Zero;i<pagesCount;i++)
            {
                if (pages[i].Contains(fullSpanMarker))
                {
                   if(pages[i].Contains(endLinkMarker))
                   {
                       UpdateEndLink(i);
                   }
                   else
                   {
                       if(pages[i].Contains(nextLinkMarker))
                       {
                           AddEndLinkAndUpdateNextLink(i,pagesCount,endpointId-1);
                       }
                       else
                       {
                           AddEndLinkAndNextLink(i, pagesCount, endpointId - 1);                           
                       }                       
                   }                    
                }
                else
                {
                    arrows = await SectionData
                       .SetNavigation(i, pagesCount, endpointId-1);
                    InsertArrows(arrows);                    
                }
            }
        }
        private static void AddEndLinkAndNextLink(int i, int pagesCount,
            int number)
        {
            AddEndLinkAndUpdateNextLink(i, pagesCount, number);
        }
        private async static void AddEndLinkAndUpdateNextLink
            (int i,int pagesCount, int number)
        {
            string arrows = await SectionData
                       .SetNavigation(i, pagesCount, number);
            int pos1 = pages[i].LastIndexOf(fullSpanMarker);
            int pos2 = pages[i].LastIndexOf(brMarker)+brMarker.Length;
            pages[i] = pages[i].Remove(pos1, pos2 - pos1);
            pages[i] = pages[i].Insert(pos1, arrows);            
        }
        private static void UpdateEndLink(int i)
        {
            int pos = pages[i].LastIndexOf(pageMarker)
                                    + pageMarker.Length;
            int newPos = pos;
            string pageNum;
            char c = pages[i][newPos];
            pageNum = c.ToString();
            while (true)
            {
                newPos++;
                c = pages[i][newPos];
                if (char.IsDigit(c))
                {                    
                    pageNum += c;
                }
                else break;
            }
            int pageNumInt = Convert.ToInt32(pageNum);
            pageNumInt++;
            pages[i] = pages[i].Remove(pos, pageNum.Length);
            pageNum = pageNumInt.ToString();
            pages[i] = pages[i].Insert(pos, pageNum);
        }
        private static void InsertArrows(string arrows)
        {
            int len=pages.Length - 1;
            int pos = pages[len].IndexOf(navMarker)
                        + navMarker.Length;
            pages[len] = pages[len].Insert(pos, arrows);
        }
        private static void AddThreadToBegin
            (int threadId, string threadName)
        {
            AddThread(MvcApplication.Zero,threadId, threadName);
            MoveThreads(threadId, threadName);
        }
        private static void MoveThreads(int threadId,string threadName)
        {            
            int len=pages.Length-1;
            string data;
            for (int i = 0; i < len;i++)
            {
                data = GetLastThread(i);                
                AddNextThread(i+1, threadId, threadName, data);
            }
        }
      
        private static string GetLastThread(int i)
        {
            int brLen = brMarker.Length;
            int lastThreadPos = pages[i].LastIndexOf(pMarker) - brLen;
            int lastSpanPos = pages[i].LastIndexOf(spanMarker);
            int count = lastSpanPos - lastThreadPos;
            string result = pages[i].Substring(lastThreadPos, count);
            pages[i] = pages[i]
                .Remove(lastThreadPos, count);
            return result;
        }
        private static void AddThread
            (int i,int threadId,string threadName)
        {
            SetPosAndTemp(i,threadId, threadName);
            pages[i]=pages[i]
                .Insert(pos, temp);
        }
        private static void AddNextThread
            (int i,int threadId,string threadName,string data)
        {
            int position=pages[i].IndexOf(navMarker)
                + navMarker.Length;
            pages[i]=pages[i]
                .Insert(position, data);
        }
        private static void SetPosAndTemp
            (int i, int threadId, string threadName)
        {
            pos = pages[i].IndexOf(navMarker)
                + navMarker.Length;
            temp = "<br /><p onclick='g(&quot;/thread/"
                + threadId.ToString() + "?page=1&quot;);'>" + threadName +
                "</p><br /><br />";
        }

        internal static int CountStringOccurrences
            (string text, string pattern)
        {            
            int count = 0;
            int i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }

        internal async static Task
            CheckTopicAndPublish(string threadName,int endpointId,
                        string username, string message)
        {
            if (CheckTopicInfo(threadName,endpointId,message))
                await PublishTopic
                        (threadName,endpointId, username, message);
        }
        internal static void Start
                (string threadName, int? endpointId, string username, string message)
        {            
                TopicData topicData
                    = new TopicData
                    {
                        threadName = threadName,
                        endpointId = endpointId,
                        username = username,
                        message = message
                    };
                TopicsToStart.Enqueue(topicData);
        }
        internal static void StartNextTopic()
        {
            while (MvcApplication.True)
            {
                if (TopicsToStart.Count != MvcApplication.Zero)
                {
                    TopicData temp;
                    TopicsToStart.TryDequeue(out temp);
                    if(temp.endpointId==null||temp.message==null
                        ||temp.threadName==null||temp.username==null)
                    { }
                    else
                        Task.Run(() =>
                            CheckTopicAndPublish
                             (temp.threadName, (int)temp.endpointId, 
                                temp.username, temp.message));
                }
                System.Threading.Thread.Sleep(10);
            }
        }
        
        private static bool CheckTopicInfo
            (string threadName,int endpointId, string message)
        {
            if (endpointId > MvcApplication.Zero 
                    && endpointId < SectionLogic
                                        .GetSectionPagesLengthLocked())
            {
                if (CheckText(message) && CheckThreadName(threadName))
                    return MvcApplication.True;
                else return MvcApplication.False;
            }
            else return MvcApplication.False;
        }
        private static bool CheckThreadName(string message)
        {
            string temp = string.Empty;
            char c;
            int rusCount = MvcApplication.Zero;
            int othCount = MvcApplication.One;
            int len = message.Length + MvcApplication.One;
            for (int i = MvcApplication.Zero; i < len - MvcApplication.One; i++)
            {
                c = message[i];
                if (RegistrationData.AlphabetRusLower.Contains(c))
                {
                    temp += c;
                    rusCount++;
                }
                else if (RegistrationData.Special.Contains(c) || char.IsDigit(c))
                {
                    temp += c;
                    othCount++;
                }
            }
            if ((((double)rusCount) / ((double)len) < 0.6)
                || (rusCount / othCount) < 0.8)
                return MvcApplication.False;
            else return MvcApplication.True;
        }
        internal static bool CheckText(string message)
        {
            string temp = string.Empty;
            char c;
            int rusCount = MvcApplication.Zero;
            int othCount = MvcApplication.One;
            int len = message.Length + MvcApplication.One;
            for (int i = MvcApplication.Zero; i < len - MvcApplication.One; i++)
            {
                c = message[i];
                if (RegistrationData.AlphabetRusLower.Contains(c))
                {
                    temp += c;
                    rusCount++;
                }
                else if (RegistrationData.Special.Contains(c) || char.IsDigit(c))
                {
                    temp += c;
                    othCount++;
                }
            }
            if ((((double)rusCount) / ((double)len) < 0.5)
                || (rusCount / othCount) < 0.8)
                return MvcApplication.False;
            else return MvcApplication.True;
        }
    }
}