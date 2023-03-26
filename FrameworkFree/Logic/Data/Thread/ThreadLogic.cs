using System.Collections.Generic;
using MarkupHandlers;
using System.Linq;
namespace Data
{
    internal sealed class ThreadLogic : IThreadLogic
    {
        private readonly IStorage Storage;
        private readonly ThreadMarkupHandler ThreadMarkupHandler;
        public ThreadLogic(IStorage storage,
        ThreadMarkupHandler threadMarkupHandler)
        {
            Storage = storage;
            ThreadMarkupHandler = threadMarkupHandler;
        }

        public string GetThreadPage(int? Id, int? page)
        {
            if (Id == null || page == null)
                return Constants.SE;
            else
            {
                if (Id > 0
                    && Id <= Storage.Fast.GetThreadPagesLengthLocked())
                {
                    int index = (int)Id - 1;

                    if (page > 0
                        && page <= Storage.Fast.GetThreadPagesPageDepthLocked(index))
                        return Storage.Fast
                            .GetThreadPagesPageLocked(index,
                                (int)page - 1);
                    else
                        return Constants.SE;
                }
                else
                    return Constants.SE;
            }
        }

        public void LoadThreadPages()
        {
            int threadsCount = Storage.Slow.GetThreadsCount();
            Storage.Fast.InitializeThreadPagesLocked
                (new string[threadsCount][]);
            Storage.Fast.SetThreadPagesLengthLocked(threadsCount);
            Storage.Fast.InitializeThreadPagesPageDepthLocked
                    (new int[Storage.Fast.GetThreadPagesLengthLocked()]);

            for (int i = 0; i < threadsCount; i++)
            {
                AddThread(i);
            }
        }

        internal void AddThread(int Num)
        {

            int amount = Num + 1;
            int number = Num;
            int count = Storage.Slow.CountMessagesByAmount(amount);

            if (count == 0)
                count++;
            int pagesCount = count / Constants.five;

            if (count - pagesCount * Constants.five > 0)
                pagesCount++;
            Storage.Fast.SetThreadPagesArrayLocked
                    (number, new string[pagesCount]);
            Storage.Fast.SetThreadPagesPageDepthLocked(number, pagesCount);
            ProcessThreadReader
                            (Storage.Slow.GetMessagesByAmount(amount),
                             number, pagesCount, Storage.Slow.GetSectionNumById(Num + 1));
        }
        private void ProcessThreadReader
       (IEnumerable<Message> messages, int number,
           int pagesCount, int sectionNum)
        {

            int pageNumber = 0;
            string threadName = Storage.Slow.GetThreadNameById(number + 1);
            Storage.Fast.AddToThreadPagesPageLocked(number, pageNumber,
                ThreadMarkupHandler.GetSectionHeader(number, sectionNum, threadName));
            bool first = false;

            if (messages.Any())
            {
                int i = 0;

                foreach (var message in messages)
                {
                    if (i == 0 && first)
                    {
                        Storage.Fast.AddToThreadPagesPageLocked(number, pageNumber,
                            ThreadMarkupHandler.GetSectionHeader(number, sectionNum, threadName));
                    }

                    int accountId = message.AccountId;

                    Storage.Fast.AddToThreadPagesPageLocked(number, pageNumber,
                        ThreadMarkupHandler.GetArticle(accountId, GetNick(accountId), message.MsgText));
                    i++;

                    if (i == Constants.five)
                    {
                        string test = ThreadMarkupHandler.SetNavigation(pageNumber, pagesCount, number);
                        Storage.Fast.AddToThreadPagesPageLocked(number, pageNumber, test);

                        if (first)
                            Storage.Fast.AddToThreadPagesPageLocked
                                (number, pageNumber, ThreadMarkupHandler.GetIndicAndA());

                        i = 0;
                        pageNumber++;
                    }

                    if (!first)
                        first = true;
                }
                if ((pageNumber >= 0)
                        && (i < Constants.five) && (i > 0))
                {
                    Storage.Fast.AddToThreadPagesPageLocked
                        (number, pageNumber,
                        ThreadMarkupHandler.SetNavigation(pageNumber, pagesCount, number));

                    if (first)
                        Storage.Fast.AddToThreadPagesPageLocked
                            (number, pageNumber, ThreadMarkupHandler.GetThreadDiv(i));
                }

            }
            if (!first)
                Storage.Fast.AddToThreadPagesPageLocked
                    (number, pageNumber, Constants.indicEnd);
        }
        public string GetNick(int accountId)
        => Storage.Slow.GetNickByAccountId(accountId);
        internal string GetThreadPage(int Id, int page)
        {
            if (Id > 0
                && Id <= Storage.Fast.GetThreadPagesLengthLocked())
            {
                int index = Id - 1;

                if (page > 0
                        && page <= Storage.Fast.GetThreadPagesPageDepthLocked(index))
                    return Storage.Fast.GetThreadPagesPageLocked
                            (index, page - 1);
                else return Constants.SE;
            }
            else
                return Constants.SE;
        }
    }
}
