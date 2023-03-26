using MarkupHandlers;
namespace Data
{
    using System;
    internal sealed class NewPrivateDialogLogic : INewPrivateDialogLogic
    {
        public readonly IStorage Storage;
        public readonly IAccountLogic AccountLogic;
        public readonly IPrivateMessageLogic PrivateMessageLogic;
        public readonly IThreadLogic ThreadLogic;
        public readonly IReplyLogic ReplyLogic;
        public readonly IRegistrationLogic RegistrationLogic;
        public readonly INewTopicLogic NewTopicLogic;
        public readonly NewPrivateDialogMarkupHandler NewPrivateDialogMarkupHandler;
        public readonly PrivateDialogMarkupHandler PrivateDialogMarkupHandler;
        public readonly PrivateMessageMarkupHandler PrivateMessageMarkupHandler;
        public NewPrivateDialogLogic(IStorage storage,
        IAccountLogic accountLogic,
        IPrivateMessageLogic privateMessageLogic,
        IThreadLogic threadLogic,
        IReplyLogic replyLogic,
        IRegistrationLogic registrationLogic,
        INewTopicLogic newTopicLogic,
        NewPrivateDialogMarkupHandler newPrivateDialogMarkupHandler,
        PrivateDialogMarkupHandler privateDialogMarkupHandler,
        PrivateMessageMarkupHandler privateMessageMarkupHandler)
        {
            Storage = storage;
            AccountLogic = accountLogic;
            PrivateMessageLogic = privateMessageLogic;
            ThreadLogic = threadLogic;
            ReplyLogic = replyLogic;
            RegistrationLogic = registrationLogic;
            NewPrivateDialogMarkupHandler = newPrivateDialogMarkupHandler;
            PrivateDialogMarkupHandler = privateDialogMarkupHandler;
            PrivateMessageMarkupHandler = privateMessageMarkupHandler;
            NewTopicLogic = newTopicLogic;
        }
        public bool CheckNickIfExists(string nick)
        => AccountLogic.CheckNickHashIfExists(nick)
                && Storage.Slow.CheckNickInBase(nick);
        public void Start
            (string acceptorNick, Pair pair, string message)
        {
            if (Storage.Fast.GetDialogsToStartCount() < Constants.MaxFirstLineLength)
            {
                DialogData dialogData
                        = new DialogData
                        {
                            acceptorNick = acceptorNick,
                            pair = pair,
                            message = message
                        };
                Storage.Fast.DialogsToStartEnqueue(dialogData);
            }
        }
        public void StartNextDialogByTimer()
        {
            if (Storage.Fast.GetDialogsToStartCount() != Constants.Zero)
            {
                DialogData temp;
                Storage.Fast.DialogsToStartTryDequeue(out temp);

                if (temp.acceptorNick == null
                    || temp.message == null
                    || temp.pair == null)
                { }
                else
                    CheckDialogAndPublish
                         (temp.acceptorNick, temp.pair.Value,
                         temp.message);
            }
        }
        public void
            CheckDialogAndPublish(string acceptorNick,
                        Pair pair, string message)
        {
            if (CheckDialogInfo(acceptorNick, message))
                PublishDialog
                        (acceptorNick, pair, message);
        }
        public void PublishDialog
                (string acceptorNick, Pair pair, string message)
        {
            int acceptorId =
                Storage.Slow.GetIdByNick(acceptorNick);
            int? accountId =
                ReplyLogic.GetAccountId(pair);

            if (accountId.HasValue)
            {
                string nick =
                    ThreadLogic.GetNick(accountId.Value);
                Storage.Slow.PutPrivateMessageInBase
                    (accountId.Value, acceptorId, message);
                bool notEqualFlag = accountId != acceptorId;
                CorrectDialogPages(acceptorId, accountId.Value, nick,
                    acceptorNick, notEqualFlag);
                CorrectMessagesPages(accountId.Value, acceptorId, nick,
                    acceptorNick, message, notEqualFlag);
            }
        }
        public void CorrectMessagesPages(int accountId,
            int acceptorId, string accountNick, string acceptorNick,
            string message, bool notEqualFlag)
        {
            PrivateMessageLogic
                .AddNewCompanionsIfNotExists(accountId, acceptorId,
                accountNick, acceptorNick, notEqualFlag);
            CorrectMessagesArray(acceptorId, accountId,
                        accountNick, acceptorNick, message, accountNick, accountId);

            if (notEqualFlag)
                CorrectMessagesArray(accountId, acceptorId,
                    acceptorNick, accountNick, message, accountNick, accountId);
        }
        public void CorrectMessagesArray(int companionId,
            int ownerId, string ownerNick, string companionNick,
            string message, string accountNick, int accountId)
        {      //TODO   
            int depth;
            string last = Storage.Fast.GetMessage(ownerId, companionId,
               Storage.Fast.GetPersonalPagesPageDepth(ownerId, companionId) - Constants.One);

            if (last.Contains(Constants.a))
            {
                string[] temp = Storage.Fast.GetMessages(ownerId, companionId);
                Storage.Fast.AddToPersonalPagesDepth(companionId, ownerId);
                depth = Storage.Fast
                                .GetPersonalPagesDepth(companionId, ownerId);
                string[] personalPagesArray = new string[depth];
                temp.CopyTo(personalPagesArray, Constants.Zero);
                Storage.Fast.SetPersonalPagesMessagesArray
                            (companionId, ownerId, personalPagesArray);

                Storage.Fast.SetPersonalPagesPage
                        (companionId, ownerId, depth - Constants.One,
                            NewPrivateDialogMarkupHandler.GenerateNewPrivateDialogPage(companionId,
                                companionNick, ownerId, ownerNick, message));
                string[] pages = Storage.Fast.GetMessages(ownerId, companionId);
                int i;
                int start;
                int end;
                string navigation = Constants.SE;

                for (i = Constants.Zero; i < depth; i++)
                {
                    navigation = PrivateMessageMarkupHandler.GetArrows(i, depth, companionId);
                    start = pages[i].LastIndexOf(Constants.fullSpanMarker);

                    if (start != -1)
                    {
                        end = pages[i].LastIndexOf(Constants.endSpanMarker)
                                + Constants.endSpanMarker.Length;
                        pages[i] = pages[i].Remove(start, end - start);
                    }
                    else
                        start = pages[i].IndexOf(Constants.answerMarker);
                    pages[i] = pages[i].Insert(start, navigation);
                    Storage.Fast.SetPersonalPagesPage(companionId, ownerId, i, pages[i]);
                }
            }
            else
            {
                int pos = last.LastIndexOf(Constants.indic) + Constants.indic.Length;
                int start = pos;
                string countString = Constants.SE;

                while (last[pos] != '<')
                {
                    countString += last[pos];
                    pos++;
                }
                last = last.Remove(start, pos - start);
                int count = Convert.ToInt32(countString) - Constants.One;
                last = last.Insert(start, count.ToString());
                int temp = last.LastIndexOf(Constants.brMarker);
                int position;
                if (temp != -1)
                {
                    position = temp + Constants.brMarker.Length;
                }
                else
                    position = last.IndexOf(Constants.h2End) + Constants.h2End.Length;
                last = last.Insert(position, NewPrivateDialogMarkupHandler.GenerateNewPrivateDialogArticle
                    (accountId, accountNick, message));
                depth = Storage.Fast.GetPersonalPagesDepth(companionId, ownerId);
                Storage.Fast.SetPersonalPagesPage
                    (companionId, ownerId, depth
                    - Constants.One, last);
            }
        }

        public void CorrectDialogPages
            (int acceptorId, int accountId, string accountNick,
            string acceptorNick, bool notEqualFlag)
        {
            CorrectPages(accountId, accountNick,
                    acceptorId, acceptorNick);

            if (notEqualFlag)
                CorrectPages(acceptorId, acceptorNick,
                        accountId, accountNick);
        }

        public void CorrectPages
            (int firstId, string firstNick,
            int secondId, string secondNick)
        {
            int index = firstId - Constants.One;
            string[] pages = Storage.Fast
                .GetDialogPagesArrayLocked(index);
            int depth = Storage.Fast
                .GetDialogPagesPageDepthLocked(index);
            bool containsNick = false;

            for (int i = Constants.Zero; i < depth; i++)
                if (containsNick)
                    break;
                else
                    if (pages[i].Contains(">" + secondNick + "<"))
                    containsNick = true;

            if (!containsNick)
            {
                int nicksCount = NewTopicLogic
                    .CountStringOccurrences
                    (pages[depth - Constants.One], Constants.pMarker);

                if (depth > Constants.One)
                {
                    if (nicksCount == Constants.DialogsOnPage)
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
                else
                {
                    if (nicksCount == Constants.DialogsOnPage)
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
        public void AddDialogToPartial
            (string[] pages, int depth, int secondId,
            string secondNick, int firstId)
        {
            string[] pagesArray = pages;
            int index = depth - Constants.One;
            int startPos = pages[index]
                .IndexOf(Constants.fullSpanMarker);

            if (startPos == -1)
                startPos = pages[index].IndexOf(Constants.endNavMarker);
            pagesArray[index] = pagesArray[index]
                                    .Insert(startPos,
                                        NewPrivateDialogMarkupHandler.GenerateNewPrivateDialogEntry
                                            (secondId, secondNick));//doublepost ERROR
            Storage.Fast
                        .SetDialogPagesArrayLocked
                        (firstId - Constants.One, pagesArray);
        }
        public void AddDialogToFull
           (string[] pages, int depth, int firstId,
            int secondId, string secondNick)
        {
            int len = depth + Constants.One;
            string[] newPagesArray = new string[len];
            pages.CopyTo(newPagesArray, Constants.Zero);
            newPagesArray[depth] = Constants.NewDialog;
            int index = firstId - Constants.One;
            newPagesArray = InsertArrows(newPagesArray, len);
            newPagesArray = InsertDialogToFull
                (newPagesArray, depth, secondId, secondNick);
            Storage.Fast
                        .SetDialogPagesArrayLocked
                        (index, newPagesArray);
            Storage.Fast.SetDialogPagesPageDepthLocked(index, len);
        }
        public string[] InsertDialogToFull
                (string[] newPagesArray, int depth,
                int secondId, string secondNick)
        {
            int startPos = newPagesArray[depth]
                                        .IndexOf(Constants.navMarker)
                                        + Constants.navMarker.Length;
            newPagesArray[depth] = newPagesArray[depth]
                                .Insert(startPos,
                                NewPrivateDialogMarkupHandler
                                .GenerateNewPrivateDialogEntryWithEnd
                                    (secondId, secondNick));

            return newPagesArray;
        }
        public string[] InsertArrows
                (string[] newPagesArray, int len)
        {
            for (int i = Constants.Zero; i < len; i++)
            {
                string arrows =
                    PrivateDialogMarkupHandler.GetArrows(i, len);
                int tempPos = newPagesArray[i].IndexOf(Constants.fullSpanMarker);

                if (tempPos != -1)
                {
                    int startPos = tempPos;
                    int endPos = newPagesArray[i]
                                    .LastIndexOf(Constants.endSpanMarker)
                                    + Constants.endSpanMarker.Length
                                    + Constants.brMarker.Length;
                    int count = endPos - startPos;
                    newPagesArray[i] = newPagesArray[i]
                                        .Remove(startPos, count);
                    newPagesArray[i] = newPagesArray[i]
                                        .Insert(startPos, arrows);
                }
                else
                {
                    int startPos = newPagesArray[i]
                                        .IndexOf(Constants.endNavMarker);
                    newPagesArray[i] = newPagesArray[i]
                                        .Insert(startPos, arrows);
                }
            }
            return newPagesArray;
        }
        public bool CheckDialogInfo
            (string acceptorNick, string message)
        {
            if (acceptorNick != null && message != null)
            {
                if (NewTopicLogic.CheckText(message)
                    && RegistrationLogic.CheckNick(acceptorNick)
                    && CheckNickIfExists(acceptorNick))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
