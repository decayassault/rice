using MarkupHandlers;
using System;
using Logic;
namespace Logic
{
    internal sealed class NewPrivateDialogLogic : INewPrivateDialogLogic
    {
        private readonly IStorage Storage;
        private readonly IAccountLogic AccountLogic;
        private readonly IPrivateMessageLogic PrivateMessageLogic;
        private readonly IThreadLogic ThreadLogic;
        private readonly IReplyLogic ReplyLogic;
        private readonly IRegistrationLogic RegistrationLogic;
        private readonly INewTopicLogic NewTopicLogic;
        private readonly ISequential Sequential;
        private readonly NewPrivateDialogMarkupHandler NewPrivateDialogMarkupHandler;
        private readonly PrivateDialogMarkupHandler PrivateDialogMarkupHandler;
        private readonly PrivateMessageMarkupHandler PrivateMessageMarkupHandler;
        public NewPrivateDialogLogic(IStorage storage,
        IAccountLogic accountLogic,
        IPrivateMessageLogic privateMessageLogic,
        IThreadLogic threadLogic,
        IReplyLogic replyLogic,
        IRegistrationLogic registrationLogic,
        INewTopicLogic newTopicLogic,
        ISequential sequential,
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
            Sequential = sequential;
            NewPrivateDialogMarkupHandler = newPrivateDialogMarkupHandler;
            PrivateDialogMarkupHandler = privateDialogMarkupHandler;
            PrivateMessageMarkupHandler = privateMessageMarkupHandler;
            NewTopicLogic = newTopicLogic;
        }
        public bool CheckNickIfExists(in string nick)
        => Sequential.Unstable.Account.CheckNickHashIfExists(nick)
                && Storage.Slow.CheckNickInBase(nick);
        public void Start
            (in string acceptorNick, in Pair pair, in string message)
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
                DialogData temp = Storage.Fast.DialogsToStartDequeue();

                if (temp.acceptorNick == null
                    || temp.message == null
                    || temp.pair == null)
                { }
                else
                    CheckDialogAndPublish
                         (temp.pair.Value, temp.acceptorNick, temp.message);
            }
        }
        public void
            CheckDialogAndPublish(in Pair pair, in string acceptorNick, in string message)
        {
            if (CheckDialogInfo(acceptorNick, message))
                PublishDialog
                        (acceptorNick, pair, message);
        }
        public void PublishDialog
                (in string acceptorNick, in Pair pair, in string message)
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
        public void CorrectMessagesPages(in int accountId,
            in int acceptorId, in string accountNick, in string acceptorNick,
            in string message, in bool notEqualFlag)
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
        public void CorrectMessagesArray(in int companionId,
            in int ownerId, in string ownerNick, in string companionNick,
            in string message, in string accountNick, in int accountId)
        {
            int depth;
            string last = Storage.Fast.GetMessage(ownerId, companionId,
               Storage.Fast.GetPersonalPagesPageDepth(ownerId, companionId) - Constants.One);

            if (last.Contains(Constants.a))
            {
                Storage.Fast.AddToPersonalPagesDepth(companionId, ownerId);
                depth = Storage.Fast
                                .GetPersonalPagesDepth(companionId, ownerId);
                string[] personalPagesArray = new string[depth];
                Storage.Fast.GetMessages(ownerId, companionId).CopyTo(personalPagesArray, Constants.Zero);
                Storage.Fast.SetPersonalPagesMessagesArray
                            (companionId, ownerId, personalPagesArray);

                Storage.Fast.SetPersonalPagesPage
                        (companionId, ownerId, depth - Constants.One,
                            NewPrivateDialogMarkupHandler.GenerateNewPrivateDialogPage(companionId,
                                companionNick, ownerId, ownerNick, message));
                string[] pages = Storage.Fast.GetMessages(ownerId, companionId);
                int start;

                for (int i = Constants.Zero; i < depth; i++)
                {
                    start = pages[i].LastIndexOf(Constants.fullSpanMarker);

                    if (start != -1)
                        pages[i] = pages[i].Remove(start, pages[i].LastIndexOf(Constants.endSpanMarker)
                                + Constants.endSpanMarker.Length - start);
                    else
                        start = pages[i].IndexOf(Constants.answerMarker);
                    pages[i] = pages[i].Insert(start, PrivateMessageMarkupHandler.GetArrows(i, depth, companionId));
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
                last = last.Remove(start, pos - start).Insert(start, (Convert.ToInt32(countString) - Constants.One).ToString());
                int temp = last.LastIndexOf(Constants.brMarker);
                Storage.Fast.SetPersonalPagesPage
                    (companionId, ownerId, Storage.Fast.GetPersonalPagesDepth(companionId, ownerId) - Constants.One,
                    last.Insert(temp != -1
                        ? temp + Constants.brMarker.Length
                        : last.IndexOf(Constants.h2End) + Constants.h2End.Length,
                         NewPrivateDialogMarkupHandler.GenerateNewPrivateDialogArticle(accountId, accountNick, message)));
            }
        }

        public void CorrectDialogPages
            (in int acceptorId, in int accountId, in string accountNick,
            in string acceptorNick, in bool notEqualFlag)
        {
            CorrectPages(accountId, accountNick,
                    acceptorId, acceptorNick);

            if (notEqualFlag)
                CorrectPages(acceptorId, acceptorNick,
                        accountId, accountNick);
        }

        public void CorrectPages
            (in int firstId, in string firstNick,
            in int secondId, in string secondNick)
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
                    if (pages[i].Contains(string.Concat(">", secondNick, "<")))
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
            (in string[] pages, in int depth, in int secondId,
            in string secondNick, in int firstId)
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
           (in string[] pages, in int depth, in int firstId,
            in int secondId, in string secondNick)
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
                (in string[] newPagesArray, in int depth,
                in int secondId, in string secondNick)
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
                (in string[] newPagesArray, in int len)
        {
            for (int i = Constants.Zero; i < len; i++)
            {
                string arrows =
                    PrivateDialogMarkupHandler.GetArrows(i, len);
                int tempPos = newPagesArray[i].IndexOf(Constants.fullSpanMarker);

                if (tempPos != -1)
                    newPagesArray[i] = newPagesArray[i].Remove(tempPos,
                                                newPagesArray[i].LastIndexOf(Constants.endSpanMarker)
                                                + Constants.endSpanMarker.Length
                                                + Constants.brMarker.Length - tempPos)
                                           .Insert(tempPos, arrows);
                else
                    newPagesArray[i] = newPagesArray[i]
                                        .Insert(newPagesArray[i]
                                        .IndexOf(Constants.endNavMarker), arrows);

            }

            return newPagesArray;
        }

        public bool CheckDialogInfo
            (in string acceptorNick, in string message)
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
