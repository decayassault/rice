using Models;
using System;
using MarkupHandlers;

namespace Data
{
    internal sealed class NewTopicLogic : INewTopicLogic
    {
        public readonly IStorage Storage;
        public readonly ISectionLogic SectionLogic;
        public readonly IThreadLogic ThreadLogic;
        public readonly IReplyLogic ReplyLogic;
        public readonly NewTopicMarkupHandler NewTopicMarkupHandler;
        private readonly SectionMarkupHandler SectionMarkupHandler;
        public NewTopicLogic(IStorage storage,
        ISectionLogic sectionLogic,
        IThreadLogic threadLogic,
        IReplyLogic replyLogic,
        NewTopicMarkupHandler newTopicMarkupHandler,
        SectionMarkupHandler sectionMarkupHandler)
        {
            Storage = storage;
            SectionLogic = sectionLogic;
            ThreadLogic = threadLogic;
            ReplyLogic = replyLogic;
            NewTopicMarkupHandler = newTopicMarkupHandler;
            SectionMarkupHandler = sectionMarkupHandler;
        }
        private void PublishTopic
                (string threadName, int endpointId,
                Pair pair, string message)
        {
            int? accountId = ReplyLogic.GetAccountId(pair);

            if (accountId.HasValue)
            {
                string nick = ThreadLogic.GetNick(accountId.Value);
                int threadId = Storage.Slow.PutThreadAndMessageInBase(
                    new Thread { Name = threadName, EndpointId = endpointId },
                        accountId.Value, message);
                CorrectSectionArray(endpointId, threadId, threadName);
                CorrectMessagesArray(endpointId, threadId,
                        message, accountId.Value, threadName, nick);
            }
        }
        private void CorrectMessagesArray
            (int endpointId, int threadId, string message,
                int accountId, string threadName, string nick)
        {
            string[][] threadPages = Storage.Fast.GetThreadPagesLocked();
            Storage.Fast.SetThreadPagesLengthLocked(threadId);
            int[] depthOld = Storage.Fast.GetThreadPagesPageDepthLocked();
            int[] depthNew = new int[threadId];
            depthOld.CopyTo(depthNew, Constants.Zero);
            int index = threadId - Constants.One;
            depthNew[index] = Constants.One;
            Storage.Fast.InitializeThreadPagesPageDepthLocked(depthNew);
            string[][] threadPagesNew = new string[threadId][];
            threadPages.CopyTo(threadPagesNew, Constants.Zero);
            threadPagesNew[index] = new string[] {
                NewTopicMarkupHandler.GetNewPage(threadId, endpointId, threadName,
                    accountId, nick, message) };
            Storage.Fast.InitializeThreadPagesLocked(threadPagesNew);
        }
        private void CorrectSectionArray
                                (int endpointId, int threadId, string threadName)
        {
            Storage.Fast.SetSectionPagesArray(endpointId);
            Storage.Fast.SetThreadsCount(CountStringOccurrences
                    (Storage.Fast.GetLastPage(), Constants.pMarker));

            if (Storage.Fast.GetPagesLength() > Constants.One)
            {
                if (Storage.Fast.GetThreadsCount() == Constants.threadsOnPage)
                {
                    AddThreadToFull(threadId, threadName, endpointId);
                }
                else
                {
                    AddThreadToBegin(threadId, threadName);
                }
            }
            else
            {
                if (Storage.Fast.GetThreadsCount() == Constants.threadsOnPage)
                {
                    AddThreadToSingle(threadId, threadName, endpointId);
                }
                else
                {
                    AddThread
                        (Constants.Zero, threadId, threadName);
                }
            }
            Storage.Fast.SetSectionPagesArrayLocked
                                (endpointId - Constants.One, Storage.Fast.GetPages());
        }
        private void AddThreadToSingle
            (int threadId, string threadName, int endpointId)
        {
            ThreadPreload(threadId, threadName, endpointId);
            MoveThreadAndSetArrows(threadId, endpointId);
        }
        private void ThreadPreload
            (int threadId, string threadName, int endpointId)
        {
            int len = Storage.Fast.GetPagesLength();
            string[] temp = new string[len + Constants.One];
            Storage.Fast.GetPages().CopyTo(temp, Constants.Zero);
            Storage.Fast.SetPages(temp);
            Storage.Fast.SetSectionPagesPageDepthLocked(endpointId - Constants.One, len);
            Storage.Fast.SetLastPage(NewTopicMarkupHandler.GetLastPage(endpointId));
            AddThread(Constants.Zero, threadId, threadName);
        }
        private void MoveThreadAndSetArrows
            (int threadId, int endpointId)
        {
            SetArrowsForSingle(threadId, endpointId);
            MoveThreadToNext();
        }
        private void MoveThreadToNext()
        {
            GetLastThread(Constants.Zero, true); // Проверить, нужно ли читать ответ.
            SetFirstThread(Constants.One, Storage.Fast.GetTemp());
        }
        private void SetArrowsForSingle(int threadId, int endpointId)
        {
            int index = endpointId - Constants.One;
            int pagesCount = Storage.Fast
                .GetSectionPagesPageDepthLocked(index);
            string[] arrows = new string[pagesCount];
            int pos;

            for (int i = Constants.Zero; i < pagesCount; i++)
            {
                arrows[i] = SectionMarkupHandler
                           .SetNavigation(i, pagesCount, index);
            }

            for (int i = Constants.Zero; i < arrows.Length; i++)
            {
                pos = Storage.Fast.GetPage(i).LastIndexOf(Constants.endNavMarker);
                Storage.Fast.SetPage(i, Storage.Fast.GetPage(i).Insert(pos, arrows[i]));
            }
        }
        private void AddThreadToFull
            (int threadId, string threadName, int endpointId)
        {
            ThreadPreload(threadId, threadName, endpointId);
            MoveThreadsAndSetArrows(threadId, endpointId);
        }
        private void MoveThreadsAndSetArrows
                (int threadId, int endpointId)
        {
            SetArrows(threadId, endpointId);
            MoveThreadsInFull(threadId, endpointId);
        }
        private void MoveThreadsInFull(int threadId, int endpointId)
        {
            int len = Storage.Fast.GetPagesLength();
            int i = Constants.Zero;

            while (i < len)
            {
                GetLastThread(i);
                i++;
                SetFirstThread(i, Storage.Fast.GetTemp());
            }
        }
        private void SetFirstThread(int i, string lastThread)
        {
            int pos = Storage.Fast.GetPage(i).IndexOf(Constants.navMarker) + Constants.navMarker.Length;
            Storage.Fast.SetPage(i, Storage.Fast.GetPage(i).Insert(pos, lastThread));
        }
        private void SetArrows(int threadId, int endpointId)
        {
            int index = endpointId - Constants.One;
            int pagesCount = Storage.Fast
                .GetSectionPagesPageDepthLocked(index);
            string arrows;

            for (int i = Constants.Zero; i < pagesCount; i++)
            {
                if (Storage.Fast.GetPage(i).Contains(Constants.fullSpanMarker))
                {
                    if (Storage.Fast.GetPage(i).Contains(Constants.endLinkMarker))
                    {
                        UpdateEndLink(i);
                    }
                    else
                    {
                        if (Storage.Fast.GetPage(i).Contains(Constants.nextLinkMarker))
                        {
                            AddEndLinkAndUpdateNextLink(i, pagesCount, index);
                        }
                        else
                        {
                            AddEndLinkAndNextLink(i, pagesCount, index);
                        }
                    }
                }
                else
                {
                    arrows = SectionMarkupHandler
                       .SetNavigation(i, pagesCount, index);
                    InsertArrows(arrows);
                }
            }
        }
        private void AddEndLinkAndNextLink(int i, int pagesCount,
            int number)
        {
            AddEndLinkAndUpdateNextLink(i, pagesCount, number);
        }
        private void AddEndLinkAndUpdateNextLink
            (int i, int pagesCount, int number)
        {
            string arrows = SectionMarkupHandler
                       .SetNavigation(i, pagesCount, number);
            int pos1 = Storage.Fast.GetPage(i).LastIndexOf(Constants.fullSpanMarker);
            int pos2 = Storage.Fast.GetPage(i).LastIndexOf(Constants.brMarker) + Constants.brMarker.Length;
            Storage.Fast.SetPage(i, Storage.Fast.GetPage(i).Remove(pos1, pos2 - pos1));
            Storage.Fast.SetPage(i, Storage.Fast.GetPage(i).Insert(pos1, arrows));
        }
        private void UpdateEndLink(int i)
        {
            int pos = Storage.Fast.GetPage(i).LastIndexOf(Constants.pageMarker)
                                    + Constants.pageMarker.Length;
            int newPos = pos;
            string pageNum;
            char c = Storage.Fast.GetPage(i)[newPos];
            pageNum = c.ToString();

            while (true)
            {
                newPos++;
                c = Storage.Fast.GetPage(i)[newPos];

                if (char.IsDigit(c))
                {
                    pageNum += c;
                }
                else
                    break;
            }
            int pageNumInt = Convert.ToInt32(pageNum);
            pageNumInt++;
            Storage.Fast.SetPage(i, Storage.Fast.GetPage(i).Remove(pos, pageNum.Length));
            pageNum = pageNumInt.ToString();
            Storage.Fast.SetPage(i, Storage.Fast.GetPage(i).Insert(pos, pageNum));
        }
        private void InsertArrows(string arrows)
        {
            int len = Storage.Fast.GetPagesLength() - Constants.One;
            int pos = Storage.Fast.GetPage(len).IndexOf(Constants.navMarker)
                        + Constants.navMarker.Length;
            Storage.Fast.SetPage(len, Storage.Fast.GetPage(len).Insert(pos, arrows));
        }
        private void AddThreadToBegin
            (int threadId, string threadName)
        {
            AddThread(Constants.Zero, threadId, threadName);
            MoveThreads(threadId, threadName);
        }
        private void MoveThreads(int threadId, string threadName)
        {
            int len = Storage.Fast.GetPagesLength() - Constants.One;
            int j;

            for (int i = Constants.Zero; i < len; i++)
            {
                j = i + Constants.One;
                Storage.Fast.SetPage(j,
                Storage.Fast.GetPage(i).Insert
                    (Storage.Fast.GetPage(j).IndexOf(Constants.navMarker)
                        + Constants.navMarker.Length, GetLastThread(i)));
            }
        }
        private string GetLastThread(int i, bool flag = false)
        {
            int brLen = Constants.brMarker.Length;
            string page = Storage.Fast.GetPage(i);
            int lastThreadPos = page.LastIndexOf(Constants.pMarker) - brLen;
            int endPos = flag ? page.IndexOf(Constants.endNavMarker) : page.LastIndexOf(Constants.spanMarker);
            int count = endPos - lastThreadPos;
            string result = page.Substring(lastThreadPos, count);
            Storage.Fast.SetPage(i, page
                .Remove(lastThreadPos, count));

            return result;
        }
        private void AddThread
            (int i, int threadId, string threadName)
        {
            SetPosAndTemp(i, threadId, threadName);
            Storage.Fast.SetPage(i, Storage.Fast.GetPage(i)
                .Insert(Storage.Fast.GetPos(), Storage.Fast.GetTemp()));
        }
        private void SetPosAndTemp
            (int i, int threadId, string threadName)
        {
            Storage.Fast.SetPos(Storage.Fast.GetPage(i).IndexOf(Constants.navMarker)
                + Constants.navMarker.Length);
            Storage.Fast.SetTemp(NewTopicMarkupHandler.GetTemp(threadId, threadName));
        }
        public int CountStringOccurrences
            (string text, string pattern)
        {
            int count = Constants.Zero;
            int i = Constants.Zero;

            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }

            return count;
        }

        private void
            CheckTopicAndPublish(string threadName, int endpointId,
                        Pair pair, string message)
        {
            if (CheckTopicInfo(threadName, endpointId, message))
                PublishTopic
                        (threadName, endpointId, pair, message);
        }
        public void Start
                (string threadName, int? endpointId, Pair pair, string message)
        {
            if (Storage.Fast.GetTopicsToStartCount() < Constants.MaxFirstLineLength)
            {
                TopicData topicData
                    = new TopicData
                    {
                        threadName = threadName,
                        endpointId = endpointId,
                        pair = pair,
                        message = message
                    };
                Storage.Fast.TopicsToStartEnqueue(topicData);
            }
        }
        public void StartNextTopicByTimer()
        {
            if (Storage.Fast.GetTopicsToStartCount() != Constants.Zero)
            {
                TopicData temp;
                Storage.Fast.TopicsToStartTryDequeue(out temp);
                if (temp.endpointId == null || temp.message == null
                    || temp.threadName == null || temp.pair == null)
                { }
                else
                    CheckTopicAndPublish
                     (temp.threadName, (int)temp.endpointId,
                        temp.pair.Value, temp.message);
            }
        }

        private bool CheckTopicInfo
            (string threadName, int endpointId, string message)
        {
            if (endpointId > Constants.Zero
                    && endpointId < Storage.Fast.GetSectionPagesLengthLocked())
            {
                if (CheckText(message) && CheckThreadName(threadName))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        private bool CheckThreadName(string message)
        {
            int messageLength = message.Length;

            if (messageLength < Constants.One
                || messageLength > Constants.MaxForumThreadNameTextLength)
                return false;
            char c;
            int rusCount = Constants.Zero;
            int othCount = Constants.One;
            int len = messageLength + Constants.One;

            for (int i = Constants.Zero; i < messageLength; i++)
            {
                c = message[i];

                if (Constants.AlphabetRusLower.Contains(c))
                {
                    rusCount++;
                }
                else if (Storage.Fast.SpecialSearch(c) || char.IsDigit(c))
                {
                    othCount++;
                }
            }
            if ((((double)rusCount) / ((double)len) < 0.6)
                || (rusCount / othCount) < 0.8)
                return false;
            else
                return true;
        }
        public bool CheckText(string message)
        {
            int messageLength = message.Length;

            if (messageLength < Constants.One
            || messageLength > Constants.MaxReplyMessageTextLength)
                return false;
            char c;
            int rusCount = Constants.Zero;
            int othCount = Constants.One;

            for (int i = Constants.Zero; i < messageLength; i++)
            {
                c = message[i];

                if (Constants.AlphabetRusLower.Contains(c))
                {
                    rusCount++;
                }
                else if (Storage.Fast.SpecialSearch(c) || char.IsDigit(c))
                {
                    othCount++;
                }
            }
            if ((((double)rusCount) / ((double)(messageLength + Constants.One)) < 0.5)
                || (rusCount / othCount) < 0.8)
                return false;
            else
                return true;
        }
    }
}