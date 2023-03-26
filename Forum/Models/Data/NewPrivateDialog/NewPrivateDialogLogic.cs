using Forum.Models;
using Forum.Data.NewPrivateMessage;
using Forum.Data.PrivateDialog;
using Forum.Data.Thread;
using Forum.Data.PrivateMessage;
namespace Forum.Data.NewPrivateDialog
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using System;
    internal sealed class NewPrivateDialogLogic
    {
        internal struct DialogData
        {
            internal string acceptorNick;
            internal string senderUsername;
            internal string message;
        }
        internal static ConcurrentQueue<DialogData> DialogsToStart;
        private const string pMarker = "<p";
        private const string fullSpanMarker = "<span id='arrows'>";
        private const string endSpanMarker="</span>";
        private const string brMarker="<br />";
        private const string endNavMarker="</nav>";
        private const string h2End = "</h2>";
        private const string startNavMarker="<nav class='n'>";
        private const string pStart="<p onclick='n(&quot;/personal/";
        private const string pMiddle="?page=1&quot;);'>";
        private const string pEnd = "</p>";
        private const string a = "<div class='s'>0</div>";
        private const string indic = "<div class='s'>";
        const string articleStart = "<article>";
        const string articleEnd = "</article>";
        const string answerMarker = "<div id='a'>";
        internal static void Start
            (string acceptorNick,string senderUsername,string message)
        {
            DialogData dialogData
                    = new DialogData
                    {                        
                        acceptorNick = acceptorNick,
                        senderUsername = senderUsername,
                        message = message
                    };
            DialogsToStart.Enqueue(dialogData);
        }
        internal static void AllowNewDialogs()
        {
            DialogsToStart = new ConcurrentQueue<DialogData>();
        }
        internal static void StartNextDialog()
        {
            while (MvcApplication.True)
            {
                if (DialogsToStart.Count != MvcApplication.Zero)
                {
                    DialogData temp;
                    DialogsToStart.TryDequeue(out temp);
                    if (temp.acceptorNick == null 
                        || temp.message == null
                        || temp.senderUsername == null)
                    { }
                    else
                        Task.Run(() =>
                            CheckDialogAndPublish
                             (temp.acceptorNick,temp.senderUsername,
                             temp.message));
                }
                System.Threading.Thread.Sleep(10);
            }
        }
        internal async static Task
            CheckDialogAndPublish(string acceptorNick, 
                        string senderUsername, string message)
        {
            if (CheckDialogInfo(acceptorNick, senderUsername, message))                
                await PublishDialog
                        (acceptorNick, senderUsername, message);
        }
        private async static Task PublishDialog
                (string acceptorNick,string senderUsername,string message)
        {
            int acceptorId =
                await NewPrivateDialogData.GetIdByNick(acceptorNick);
            int accountId =
                await ReplyData.GetAccountId(senderUsername);
            if(accountId!=acceptorId)
            {
                string nick =
                    await ThreadData.GetNick(accountId);
                await NewPrivateMessageData.PutPrivateMessageInBase
                    (accountId, acceptorId, message);
                CorrectDialogPages(acceptorId, accountId, nick,
                    acceptorNick);
                CorrectMessagesPages(accountId, acceptorId, nick,
                    acceptorNick, message);
            }
        }
        private static void CorrectMessagesPages(int accountId,
            int acceptorId,string accountNick,string acceptorNick,string message)
        {
            PrivateMessageLogic
                .AddNewCompanionsIfNotExists(accountId, acceptorId,
                accountNick, acceptorNick);            
            CorrectMessagesArray(acceptorId, accountId,
                        accountNick, acceptorNick, message,accountNick, accountId);
            CorrectMessagesArray(accountId, acceptorId,
                acceptorNick, accountNick, message,accountNick,accountId);
        }
        private static void CorrectMessagesArray(int companionId,
            int ownerId,string ownerNick,string companionNick,
            string message,string accountNick, int accountId)
        {      //TODO   
            string page;
            int depth;
            string last = PrivateMessageLogic
                .GetLastPersonalPage(companionId, ownerId);           

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
                page += articleStart + "<span onClick='g(&quot;/profile/" +
                        ownerId.ToString() + "&quot;);'>" + ownerNick + "</span><br />";
                page += "<p>" + message + "</p>" + articleEnd + "<br />" + fullSpanMarker
                    + endSpanMarker
                    + "<div id='a'><a onClick='replyPM();return false'>Ответить "
                + companionNick + "</a></div>" + "</div><div class='s'>4</div>";
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
                    start = pages[i].LastIndexOf(fullSpanMarker);
                    if (start != -1)
                    {
                        end = pages[i].LastIndexOf(endSpanMarker)
                                + endSpanMarker.Length;
                        pages[i] = pages[i].Remove(start, end - start);
                    }
                    else
                        start = pages[i].IndexOf(answerMarker);
                    pages[i] = pages[i].Insert(start, navigation);
                    PrivateMessageLogic.SetPersonalPagesPage(companionId, ownerId, i, pages[i]);
                }
            }
            else
            {                
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
                page = articleStart + "<span onClick='g(&quot;/profile/" +
                        accountId.ToString() + "&quot;);'>" + accountNick + "</span><br />";
                page += "<p>" + message + "</p>" + articleEnd + "<br />";
                int temp = last.LastIndexOf(brMarker);
                int position;
                if (temp != -1)
                {
                    position = temp + brMarker.Length;
                }
                else
                    position = last.IndexOf(h2End)+h2End.Length;
                last = last.Insert(position, page);
                depth = PrivateMessageLogic.GetPersonalPagesDepth(companionId, ownerId);                
                PrivateMessageLogic.SetPersonalPagesPage
                    (companionId, ownerId, depth
                    - MvcApplication.One, last);
            }
        }

        private static void CorrectDialogPages
            (int acceptorId, int accountId, string accountNick,
            string acceptorNick)
        {
            CorrectPages(accountId, accountNick,
                    acceptorId, acceptorNick);
            CorrectPages(acceptorId, acceptorNick,
                    accountId, accountNick);
        }
        
        private static void CorrectPages
            (int firstId,string firstNick,
            int secondId, string secondNick)
        {
            int index=firstId-MvcApplication.One;
            string[] pages = PrivateDialogLogic
                .GetDialogPagesArrayLocked(index);
            int depth = PrivateDialogLogic
                .GetDialogPagesPageDepthLocked(index);
            bool containsNick = false;
            for (int i = 0; i < depth; i++)
                if (containsNick)
                    break;
                else
                    if (pages[i].Contains(">" + secondNick + "<"))
                        containsNick = true;
            if(!containsNick)
            {
                int nicksCount = NewTopicData
                    .CountStringOccurrences
                    (pages[depth-MvcApplication.One],pMarker);
                if(depth>1)
                {
                    if(nicksCount==PrivateDialogData.DialogsOnPage)
                    {
                        AddDialogToFull
                            (pages, depth, firstId,
                            secondId,secondNick);
                    }
                    else
                    {
                        AddDialogToPartial
                            (pages,depth,secondId,
                            secondNick,firstId);
                    }
                }
                else
                {
                    if(nicksCount==PrivateDialogData.DialogsOnPage)
                    {
                        AddDialogToFull
                            (pages, depth, firstId,
                            secondId, secondNick);
                    }
                    else
                    {
                        AddDialogToPartial
                           (pages, depth, secondId,
                           secondNick, firstId);
                    }
                }         
            }            
        }
        private static void AddDialogToPartial
            (string[] pages,int depth, int secondId,
            string secondNick,int firstId)
        {
            string[] pagesArray=pages;
            int index=depth - MvcApplication.One;
            int startPos = pages[index]
                .IndexOf(fullSpanMarker);
            if (startPos == -1)
                startPos = pages[index].IndexOf(endNavMarker);
            string dialog = brMarker + pStart + secondId.ToString()
                + pMiddle + secondNick + brMarker + brMarker;
            pagesArray[index] = pagesArray[index]
                                    .Insert(startPos, dialog);//doublepost ERROR
            PrivateDialogLogic
                        .SetDialogPagesArrayLocked
                        (firstId-MvcApplication.One, pagesArray);
        }
        private async static void AddDialogToFull
           (string[] pages,int depth, int firstId,
            int secondId, string secondNick)
        {
            int len=depth + MvcApplication.One;
            string[] newPagesArray = new string[len];
            pages.CopyTo(newPagesArray, MvcApplication.Zero);
            newPagesArray[depth] =
                "<div id='dialog'><span><a onclick="+
                "'newDialog();return false;'>Новый диалог</a>" +
                        "</span></div><nav class='n'></nav></div>";
            int index=firstId - MvcApplication.One;
            newPagesArray=await InsertArrows(newPagesArray,len);
            newPagesArray = InsertDialogToFull
                (newPagesArray,depth,secondId,secondNick);
            PrivateDialogLogic
                        .SetDialogPagesArrayLocked
                        (index, newPagesArray);
            PrivateDialogLogic.SetDialogPagesPageDepthLocked(index, len);
        }
        private static string[] InsertDialogToFull
                (string[] newPagesArray,int depth,
                int secondId,string secondNick)
        {
            int startPos = newPagesArray[depth]
                                        .IndexOf(startNavMarker)
                                        +startNavMarker.Length;
            string dialog=brMarker+pStart+secondId.ToString()
                +pMiddle+secondNick+pEnd
                +brMarker+brMarker;
            newPagesArray[depth] = newPagesArray[depth]
                                .Insert(startPos, dialog);

            return newPagesArray;
        }
        private async static Task<string[]> InsertArrows
                (string[] newPagesArray,int len)
        {
            for (int i = 0; i < len; i++)
            {
                string arrows =
                   await PrivateDialogData.SetNavigation(i, len);
                int tempPos = newPagesArray[i].IndexOf(fullSpanMarker);
                if (tempPos != -1)
                {
                    int startPos = tempPos;
                    int endPos = newPagesArray[i]
                                    .LastIndexOf(endSpanMarker)
                                    + endSpanMarker.Length
                                    + brMarker.Length;
                    int count = endPos - startPos;
                    newPagesArray[i] = newPagesArray[i]
                                        .Remove(startPos, count);
                    newPagesArray[i] = newPagesArray[i]
                                        .Insert(startPos, arrows);
                }
                else
                {
                    int startPos = newPagesArray[i]
                                        .IndexOf(endNavMarker);
                    newPagesArray[i] = newPagesArray[i]
                                        .Insert(startPos, arrows);
                }
            }
            return newPagesArray;
        }
        private static bool CheckDialogInfo
            (string acceptorNick, string senderUsername, string message)
        {
            if (acceptorNick!=null&&senderUsername!=null&&message!=null)                    
            {
                if (NewTopicData.CheckText(message)
                    && RegistrationData.CheckNick(acceptorNick)
                    &&NewPrivateDialogData.CheckNickIfExists(acceptorNick))
                    return MvcApplication.True;
                else return MvcApplication.False;
            }
            else return MvcApplication.False;
        }
    }
}