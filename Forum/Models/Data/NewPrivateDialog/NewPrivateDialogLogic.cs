using Forum.Models;
using Forum.Data.NewPrivateMessage;
using Forum.Data.PrivateDialog;
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
        private const string startNavMarker="<nav class='n'>";
        private const string pStart="<p onclick='n(&quot;/personal/";
        private const string pMiddle="?page=1&quot;);'>";
        private const string pEnd = "</p>";
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
            string nick = 
                await ThreadData.GetNick(accountId);
            await NewPrivateMessageData.PutPrivateMessageInBase
                (accountId, acceptorId, message);
            CorrectDialogPages(acceptorId,accountId,nick,
                acceptorNick);
            /*CorrectMessagesArray(endpointId, threadId,
                        message, accountId, threadName, nick);*/
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
                    if (pages[i].Contains(">" + firstNick + "<"))
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
                                    .Insert(startPos, dialog);
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