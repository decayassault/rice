using System.Threading.Tasks;
using System.Collections.Concurrent;
using System;
using Forum.Data.Thread;
using Forum.Models;
using Forum.Data.Account;
namespace Forum.Data
{
    internal sealed class ReplyData
    {
        internal const string ReplyPage = "<p>Ваш ответ:</p>" +
            "<textarea id='text' autofocus maxlength='1000' wrap='soft' spellcheck='true'></textarea>" +
            "<div id='msg'></div><br /><span><a id='send' onClick='s();return false'>Отправить</a></span>";
            const string a="<div class='s'>0</div>";
            const string indic="<div class='s'>";
            const string indicEnd = "</div>";
            const string spanIndicator = "<span id='arrows'>";
            const string articleStart = "<article>";
            const string articleEnd = "</article>";
            const string spanEnd = "</span>";
            const string brMarker = "<br />";
        internal struct MessageData
        {
            internal int id;
            internal string username;
            internal string text;
        }
            internal static ConcurrentQueue<MessageData> MessagesToPublish;
        internal async static Task 
            CheckReplyAndPublish(int id,string username,string text)
        {
            if (Check(id, text))
                await PublishReply(id, username, text);
        }

        internal static void Start(int id,string username,string t)
        {
            MessageData messageData
                  = new MessageData
                  {
                      id = id,
                      username = username,
                      text = t
                  };
            MessagesToPublish.Enqueue(messageData);
        }
        internal static void PublishNextMessage()
        {
            while (MvcApplication.True)
            {
                if (MessagesToPublish.Count != MvcApplication.Zero)
                {
                    MessageData temp;
                    MessagesToPublish.TryDequeue(out temp);
                    Task.Run(() =>
                        CheckReplyAndPublish
                            (temp.id, temp.username, temp.text));
                }
                System.Threading.Thread.Sleep(10);
            }
        }

        internal static Task<int> GetAccountId(string username)
        {
            string[] temp = username.Split('_');
            var pair = new AccountData.Pair
            {
                LoginHash = Convert.ToInt32(temp[MvcApplication.Zero]),
                PasswordHash = Convert.ToInt32(temp[MvcApplication.One])
            };
            int accId = AccountData.LoginPasswordAccIdHashes[pair];

            return Task.FromResult(accId);
        }

        private async static Task PublishReply
                (int id,string username,string text)
        {
            int accId = await GetAccountId(username);
            await PutInBase(id,accId, text);
            CorrectArray(id,accId, text);         
        }

        private async static void CorrectArray(int id,int accId, string text)
        {
            string nick = await ThreadData.GetNick(accId);            
            string page;         
            string last =
                ThreadLogic.GetThreadPagesPageLocked
                    (id,ThreadLogic.GetThreadPagesPageDepthLocked(id)
                    - MvcApplication.One);
            if(last.Contains(a))
            {
                string threadName = await ThreadData.GetThreadName(id);
                int sectionNum = await ThreadData.GetSectionNum(id);
                string[] temp = ThreadLogic.GetThreadPagesArrayLocked(id);
                ThreadLogic.AddToThreadPagesPageDepthLocked(id,1);
                ThreadLogic.SetThreadPagesArrayLocked
                    (id, new string[ThreadLogic
                            .GetThreadPagesPageDepthLocked(id)]);
                temp.CopyTo(ThreadLogic.GetThreadPagesArrayLocked(id)
                            ,MvcApplication.Zero);
                page =  indic+ id.ToString() +
                "</div><div class='l'><h2 onClick='g(&quot;/section/" +
                    sectionNum.ToString() + "?page=1&quot;);'>" + threadName + "</h2>";
                page += articleStart+"<span onClick='g(&quot;/Profile/" +
                        accId.ToString() + "&quot;);'>" + nick + "</span><br />";
                page += "<p>" + text + "</p>" + articleEnd + "<br />" + spanIndicator + spanEnd 
                    + "<div id='a'><a onClick='u();return false'>Ответить</a></div>" + "</div><div class='s'>4</div>";
                int len = ThreadLogic.GetThreadPagesPageDepthLocked(id);
                ThreadLogic.SetThreadPagesPageLocked(id,len - 1,page);
                string[] thread = ThreadLogic.GetThreadPagesArrayLocked(id);
                int i;
                int start;
                int end;
                string navigation=string.Empty;
                for (i = MvcApplication.Zero; i < len; i++)
                {
                    navigation = ThreadData.GetArrows(i,len,id);
                    start = thread[i].LastIndexOf(spanIndicator);
                    end = thread[i].LastIndexOf(spanEnd)
                            + spanEnd.Length;
                    thread[i] = thread[i].Remove(start, end - start);
                    thread[i] = thread[i].Insert(start, navigation);
                    ThreadLogic.SetThreadPagesPageLocked(id,i,thread[i]);
                }                
            }
            else
            {                
                int position = last.LastIndexOf(brMarker)+brMarker.Length;                
                int pos = last.LastIndexOf(indic)+indic.Length;
                int start=pos;
                string countString = string.Empty;
                while(last[pos]!='<')
                {
                    countString += last[pos];
                    pos++;
                }
                last = last.Remove(start, pos - start);
                int count = Convert.ToInt32(countString)-MvcApplication.One;
                last = last.Insert(start, count.ToString());
                page = articleStart+"<span onClick='g(&quot;/Profile/" +
                        accId.ToString() + "&quot;);'>" + nick + "</span><br />";
                page += "<p>" + text + "</p>" + articleEnd+"<br />";                
                last = last.Insert(position, page);
                ThreadLogic.SetThreadPagesPageLocked
                    (id,ThreadLogic.GetThreadPagesPageDepthLocked(id)
                    - MvcApplication.One,last);               
            }
        }

        private async static Task PutInBase(int id,int accId, string text)
        {
            using (var SqlCon = await Connection.GetConnection())
            {
                using (var cmdPutMessage = 
                    Command.InitializeCommandForPutMessage
                        (@"PutMessage", SqlCon, id + MvcApplication.One,
                        accId
                        ,text))
                {
                    await cmdPutMessage.ExecuteNonQueryAsync();                    
                }                
            }     
        }

        private static bool Check(int id, string text)
        {
            int test = ThreadLogic.GetThreadPagesLengthLocked();
            if (id >= MvcApplication.Zero 
                && id < ThreadLogic.GetThreadPagesLengthLocked())
            {
                string temp=string.Empty;
                char c;
                int rusCount = MvcApplication.Zero;
                int othCount = MvcApplication.One;
                int len = text.Length + MvcApplication.One;
                for (int i = MvcApplication.Zero; i < len - MvcApplication.One; i++)
                {
                    c=text[i];
                    if (RegistrationData.AlphabetRusLower.Contains(c))
                    {
                        temp += c;
                        rusCount++;
                    }
                    else if(RegistrationData.Special.Contains(c)||char.IsDigit(c))
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
            else return MvcApplication.False;
        }

        internal static void AllowNewMessages()
        {
            MessagesToPublish = new ConcurrentQueue<MessageData>();
        }
    }
}