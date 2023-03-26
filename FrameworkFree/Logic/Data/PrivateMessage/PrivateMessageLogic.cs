using System.Collections.Generic;
using MarkupHandlers;
namespace Data
{
    internal sealed class PrivateMessageLogic : IPrivateMessageLogic
    {
        public readonly IStorage Storage;
        public readonly IThreadLogic ThreadLogic;
        public readonly IReplyLogic ReplyLogic;
        public readonly PrivateMessageMarkupHandler PrivateMessageMarkupHandler;
        public PrivateMessageLogic(IStorage storage,
        IThreadLogic threadLogic,
        IReplyLogic replyLogic,
        PrivateMessageMarkupHandler privateMessageMarkupHandler)
        {
            Storage = storage;
            ThreadLogic = threadLogic;
            ReplyLogic = replyLogic;
            PrivateMessageMarkupHandler = privateMessageMarkupHandler;
        }
        public
            PrivateMessages
                GetMessages(int companionId, int accountId)
        {
            int count = Storage.Slow.CountPrivateMessagesByIds(companionId, accountId);

            if (count == Constants.Zero)
                count++;
            int pagesCount = count / Constants.five;

            if (count - pagesCount * Constants.five > Constants.Zero)
                pagesCount++;

            return ProcessCompanionPrivateMessagesReader
                            (Storage.Slow.GetPrivateMessagesByIds(companionId, accountId),
                             companionId, accountId, pagesCount);
        }

        public PrivateMessages
                    ProcessCompanionPrivateMessagesReader
                        (IEnumerable<IdText> idTexts, int companionId,
                        int accountId, int pagesCount)
        {
            string companionNick = ThreadLogic.GetNick(companionId);
            string accountNick = ThreadLogic.GetNick(accountId);

            return PrivateMessageMarkupHandler.ProcessCompanionReaderMarkup(
                companionNick, companionId, idTexts, pagesCount, accountNick);
        }
        public string GetPersonalPage
                            (int? id, int? page, Pair pair)
        {
            if (page == null || id == null)
                return Constants.SE;
            else
            {
                int? index = ReplyLogic.GetAccountId(pair);

                if (index.HasValue)
                {
                    int Id = (int)id;
                    int limit = Storage.Fast.GetPersonalPagesPageDepth(index.Value, Id);

                    if (page > Constants.Zero
                        && page
                        <= limit)
                    {
                        return Storage.Fast.GetMessage(index.Value, Id, (int)page - Constants.One);
                    }
                    else
                        return Constants.SE;
                }
                else
                    return Constants.SE;
            }
        }
        public void LoadPersonalPages()
        {
            int ownersCount = Storage.Slow.GetAccountsCount();

            for (int i = Constants.Zero; i < ownersCount; i++)
            {
                SetMessagesDictionary(i + Constants.One);
            }
        }
        public void AddNewCompanionsIfNotExists
                    (int ownerId, int companionId, string ownerNick,
            string companionNick, bool notEqualFlag)
        {
            OwnerId ownerIdObj = new OwnerId { Id = ownerId };
            CompanionId companionIdObj = new CompanionId { Id = companionId };
            SetNewCompanionDepth(ownerIdObj, companionIdObj);
            SetNewCompanionPage(ownerIdObj, companionIdObj, companionNick);

            if (notEqualFlag)
            {
                ownerIdObj.Id = companionId;
                companionIdObj.Id = ownerId;
                SetNewCompanionDepth(ownerIdObj, companionIdObj);
                SetNewCompanionPage(ownerIdObj, companionIdObj, ownerNick);
            }
        }
        public void SetNewCompanionPage
            (OwnerId ownerId, CompanionId companionId, string companionNick)
        {
            bool flag = Storage.Fast.PersonalPagesKeysContains(ownerId);

            if (!Storage.Fast.PersonalPagesContainsKey(ownerId, companionId, flag))
                Storage.Fast.PersonalPagesAdd(ownerId, companionId, new string[Constants.One]
                {
                    PrivateMessageMarkupHandler.SetNewCompanionPageMarkup(companionId.Id,
                    companionNick)
                }, flag);
        }
        public void SetNewCompanionDepth
            (OwnerId ownerId, CompanionId companionId)
        {
            bool flag = Storage.Fast.PersonalPagesDepthsKeysContains(ownerId);

            if (!Storage.Fast.PersonalPagesDepthsContainsKey(ownerId, companionId, flag))
                Storage.Fast.PersonalPagesDepthsAdd(ownerId, companionId, flag);
        }
        public void SetMessagesDictionary(int accountId)
        {
            CompanionId[] companions
                = Storage.Slow.GetCompanionsByAccountId(accountId);

            Dictionary<CompanionId, PrivateMessages>
                    temp1 = new();
            Dictionary<CompanionId, int>
                    temp2 = new();

            for (int i = Constants.Zero; i < companions.Length; i++)
            {
                PrivateMessages pm = GetMessages
                                    (companions[i].Id, accountId);

                temp1.Add(companions[i], pm);
                temp2.Add(companions[i], pm.Messages.Length);
            }
            Storage.Fast.PersonalPagesTryAdd(new OwnerId { Id = accountId }, temp1);
            Storage.Fast.PersonalPagesDepthsTryAdd(new OwnerId { Id = accountId }, temp2);
        }
    }
}
