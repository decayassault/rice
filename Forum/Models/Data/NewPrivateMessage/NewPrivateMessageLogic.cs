using Forum.Data.PrivateDialog;
using Forum.Models;
using Forum.Data.PrivateMessage;
using Forum.Data.Thread;
namespace Forum.Data.NewPrivateMessage
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
  
    internal sealed class NewPrivateMessageLogic
    {
        const string a = "<div class='s'>0</div>";
        const string indic = "<div class='s'>";
        const string indicEnd = "</div>";
        const string spanIndicator = "<span id='arrows'>";
        const string articleStart = "<article>";
        const string articleEnd = "</article>";
        const string spanEnd = "</span>";
        const string brMarker = "<br />";
        const string answerMarker = "<div id='a'>";

        internal static ConcurrentQueue<ReplyData.MessageData> 
            PersonalMessagesToPublish;
        internal static void AllowNewPrivateMessages()
        {
            PersonalMessagesToPublish = 
                new ConcurrentQueue<ReplyData.MessageData>();
        }
        internal static void Start(int? id, string username, string t)
        {
            ReplyData.MessageData personalMessageData
                  = new ReplyData.MessageData
                  {
                      id = id,
                      username = username,
                      text = t
                  };
            PersonalMessagesToPublish.Enqueue(personalMessageData);
        }
        internal static void PublishNextPrivateMessage()
        {
            while (MvcApplication.True)
            {
                if (PersonalMessagesToPublish.Count
                    != MvcApplication.Zero)
                {
                    ReplyData.MessageData temp;
                    PersonalMessagesToPublish.TryDequeue(out temp);
                    if (temp.id == null || temp.username == null
                        || temp.text == null)
                    { }
                    else
                        Task.Run(() =>
                            CheckPersonalReplyAndPublish
                                ((int)temp.id, temp.username, temp.text));
                }
                System.Threading.Thread.Sleep(10);
            }
        }
        internal async static Task
            CheckPersonalReplyAndPublish(int id, string username, string text)
        {
            if (Check(id, text))
                await PublishPersonalReply(id, username, text);
        }
        private async static Task PublishPersonalReply
                (int id, string username, string text)
        {
            int accId = await ReplyData.GetAccountId(username);
            if (accId != id)
            {
                await NewPrivateMessageData
                    .PutPrivateMessageInBase(accId, id, text);
                string ownerNick = await ThreadData.GetNick(accId);
                string companionNick = await ThreadData.GetNick(id);
                int order = 1;
                CorrectArray(id, accId, text, ownerNick, companionNick, order);
                order = 2;
                CorrectArray(accId, id, text, companionNick, ownerNick, order);
            }
        }
        private static void CorrectArray(int companionId,
            int ownerId, string text,
            string ownerNick, string companionNick,int order)
        {
            //TODO неправ. Profile/x и ник x
            string page;
            int depth;
            string last = PrivateMessageLogic.GetLastPersonalPage(companionId, ownerId);
                
            if (last.Contains(a))
            {                
                string[] temp = PrivateMessageLogic
                                .GetPersonalPagesArray(companionId, ownerId);
                PrivateMessageLogic.AddToPersonalPagesDepth(companionId, ownerId);
                depth = PrivateMessageLogic
                                .GetPersonalPagesDepth(companionId, ownerId);
                string[] personalPagesArray = new string[depth];
                temp.CopyTo(personalPagesArray, MvcApplication.Zero);
                PrivateMessageLogic.SetPersonalPagesMessagesArray
                            (companionId, ownerId, personalPagesArray);                
                page = indic + companionId.ToString() +
                "</div><div class='l'><h2 onClick='g(&quot;/dialog/1&quot;);'>Переписка с "
                + companionNick + "</h2>";
                if(order==1)
                    page += articleStart + "<span onClick='g(&quot;/profile/" +
                        ownerId.ToString() + "&quot;);'>" + ownerNick + "</span><br />";
                else
                    page += articleStart + "<span onClick='g(&quot;/profile/" +
                        companionId.ToString() + "&quot;);'>" + companionNick + "</span><br />";
                page += "<p>" + text + "</p>" + articleEnd + "<br />" + spanIndicator + spanEnd
                    + "<div id='a'><a onClick='replyPM();return false'>Ответить "
                +companionNick+"</a></div>" + "</div><div class='s'>4</div>";
                PrivateMessageLogic.SetPersonalPagesPage
                        (companionId, ownerId, depth - MvcApplication.One, page);
                string[] pages = PrivateMessageLogic
                                .GetPersonalPagesArray(companionId, ownerId);
                int i;
                int start;
                int end;
                string navigation = string.Empty;
                for (i = MvcApplication.Zero; i < depth; i++)
                {
                    navigation = PrivateMessageData.GetArrows(i, depth, companionId);
                    start = pages[i].LastIndexOf(spanIndicator);
                    if (start != -1)
                    {
                        end = pages[i].LastIndexOf(spanEnd)
                            + spanEnd.Length;
                        pages[i] = pages[i].Remove(start, end - start);
                    }
                    else
                    {
                        start = pages[i].IndexOf(answerMarker);
                    }                    
                    pages[i] = pages[i].Insert(start, navigation);
                    PrivateMessageLogic.SetPersonalPagesPage(companionId, ownerId, i, pages[i]);
                }
            }
            else
            {
                int position = last.LastIndexOf(brMarker) + brMarker.Length;
                int pos = last.LastIndexOf(indic) + indic.Length;
                int start = pos;
                string countString = string.Empty;
                while (last[pos] != '<')
                {
                    countString += last[pos];
                    pos++;
                }
                last = last.Remove(start, pos - start);
                int count = Convert.ToInt32(countString) - MvcApplication.One;
                last = last.Insert(start, count.ToString());
                if (order==1)
                    page = articleStart + "<span onClick='g(&quot;/profile/" +
                        ownerId.ToString() + "&quot;);'>" + ownerNick + "</span><br />";
                else
                    page = articleStart + "<span onClick='g(&quot;/profile/" +
                        companionId.ToString() + "&quot;);'>" + companionNick + "</span><br />";
                page += "<p>" + text + "</p>" + articleEnd + "<br />";
                last = last.Insert(position, page);
                depth = PrivateMessageLogic.GetPersonalPagesDepth(companionId, ownerId);
                PrivateMessageLogic.SetPersonalPagesPage
                    (companionId, ownerId, depth
                    - MvcApplication.One, last);                
            }
        }
        
        private static bool Check(int id, string text)
        {
            int limit = PrivateDialogLogic.GetDialogPagesLengthLocked();
            if (id > MvcApplication.Zero
                && id <= limit)
            {
                string temp = string.Empty;
                char c;
                int rusCount = MvcApplication.Zero;
                int othCount = MvcApplication.One;
                int len = text.Length + MvcApplication.One;
                for (int i = MvcApplication.Zero; i < len - MvcApplication.One; i++)
                {
                    c = text[i];
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
            else return MvcApplication.False;                         
        }
    }
}