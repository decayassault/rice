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
                if (Id > Constants.Zero
                    && Id <= Storage.Fast.GetThreadPagesLengthLocked())
                {
                    int index = (int)Id - Constants.One;

                    if (page > Constants.Zero
                        && page <= Storage.Fast.GetThreadPagesPageDepthLocked(index))
                        return Storage.Fast
                            .GetThreadPagesPageLocked(index,
                                (int)page - Constants.One);
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

            for (int i = Constants.Zero; i < threadsCount; i++)
            {
                AddThread(i);
            }
        }

        internal void AddThread(int Num)
        {

            int amount = Num + Constants.One;
            int number = Num;
            int count = Storage.Slow.CountMessagesByAmount(amount);

            if (count == Constants.Zero)
                count++;
            int pagesCount = count / Constants.five;

            if (count - pagesCount * Constants.five > Constants.Zero)
                pagesCount++;
            Storage.Fast.SetThreadPagesArrayLocked
                    (number, new string[pagesCount]);
            Storage.Fast.SetThreadPagesPageDepthLocked(number, pagesCount);
            ProcessThreadReader
                            (Storage.Slow.GetMessagesByAmount(amount),
                             number, pagesCount, Storage.Slow.GetSectionNumById(Num + Constants.One));
        }
        private void ProcessThreadReader
       (IEnumerable<Message> messages, int number,
           int pagesCount, int sectionNum)
        {

            int pageNumber = Constants.Zero;
            string threadName = Storage.Slow.GetThreadNameById(number + Constants.One);
            Storage.Fast.AddToThreadPagesPageLocked(number, pageNumber,
                ThreadMarkupHandler.GetSectionHeader(number, sectionNum, threadName));
            bool first = false;

            if (messages.Any())
            {
                int i = Constants.Zero;

                foreach (var message in messages)
                {
                    if (i == Constants.Zero && first)
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

                        i = Constants.Zero;
                        pageNumber++;
                    }

                    if (!first)
                        first = true;
                }
                if ((pageNumber >= Constants.Zero)
                        && (i < Constants.five) && (i > Constants.Zero))
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
            if (Id > Constants.Zero
                && Id <= Storage.Fast.GetThreadPagesLengthLocked())
            {
                int index = Id - Constants.One;

                if (page > Constants.Zero
                        && page <= Storage.Fast.GetThreadPagesPageDepthLocked(index))
                    return Storage.Fast.GetThreadPagesPageLocked
                            (index, page - Constants.One);
                else return Constants.SE;
            }
            else
                return Constants.SE;
        }
    }
}
