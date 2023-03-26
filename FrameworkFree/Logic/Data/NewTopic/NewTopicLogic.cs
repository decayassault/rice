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
            depthOld.CopyTo(depthNew, 0);
            depthNew[threadId - 1] = 1;
            Storage.Fast.InitializeThreadPagesPageDepthLocked(depthNew);
            string[][] threadPagesNew = new string[threadId][];
            threadPages.CopyTo(threadPagesNew, 0);
            threadPagesNew[threadId - 1] = new string[] {
                NewTopicMarkupHandler.GetNewPage(threadId, endpointId, threadName,
                    accountId, nick, message) };
            Storage.Fast.InitializeThreadPagesLocked(threadPagesNew);
        }
        private void CorrectSectionArray
                                (int endpointId, int threadId, string threadName)
        {
            Storage.Fast.SetSectionPagesArray(endpointId);
            Storage.Fast.SetThreadsCount(CountStringOccurrences
                    (Storage.Fast.LastPage, Constants.pMarker));

            if (Storage.Fast.PagesLength > 1)
            {
                if (Storage.Fast.ThreadsCount == Constants.threadsOnPage)
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
                if (Storage.Fast.ThreadsCount == Constants.threadsOnPage)
                {
                    AddThreadToSingle(threadId, threadName, endpointId);
                }
                else
                {
                    AddThread
                        (0, threadId, threadName);
                }
            }
            Storage.Fast.SetSectionPagesArrayLocked
                                (endpointId - 1, Storage.Fast.Pages);
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
            int len = Storage.Fast.PagesLength;
            string[] temp = new string[len + 1];
            Storage.Fast.Pages.CopyTo(temp, 0);
            Storage.Fast.SetPages(temp);
            Storage.Fast.SetSectionPagesPageDepthLocked(endpointId - 1, len);
            Storage.Fast.SetLastPage(NewTopicMarkupHandler.GetLastPage(endpointId));
            AddThread(0, threadId, threadName);
        }
        private void MoveThreadAndSetArrows
            (int threadId, int endpointId)
        {
            SetArrowsForSingle(threadId, endpointId);
            MoveThreadToNext();
        }
        private void MoveThreadToNext()
        {
            GetLastThread(0); // Проверить, нужно ли читать ответ.
            SetFirstThread(1, Storage.Fast.Temp);
        }
        private void SetArrowsForSingle(int threadId, int endpointId)
        {
            int pagesCount = Storage.Fast
                .GetSectionPagesPageDepthLocked(endpointId - 1);
            string[] arrows = new string[pagesCount];
            int pos;

            for (int i = 0; i < pagesCount; i++)
            {
                arrows[i] = SectionMarkupHandler
                           .SetNavigation(i, pagesCount, endpointId - 1);
            }

            for (int i = 0; i < arrows.Length; i++)
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
            int len = Storage.Fast.PagesLength;
            int i = 0;

            while (i < len)
            {
                GetLastThread(i);
                i++;
                SetFirstThread(i, Storage.Fast.Temp);
            }
        }
        private void SetFirstThread(int i, string lastThread)
        {
            int pos = Storage.Fast.GetPage(i).IndexOf(Constants.navMarker) + Constants.navMarker.Length;
            Storage.Fast.SetPage(i, Storage.Fast.GetPage(i).Insert(pos, lastThread));
        }
        private void SetArrows(int threadId, int endpointId)
        {
            int pagesCount = Storage.Fast
                .GetSectionPagesPageDepthLocked(endpointId - 1);
            string arrows;

            for (int i = 0; i < pagesCount; i++)
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
                            AddEndLinkAndUpdateNextLink(i, pagesCount, endpointId - 1);
                        }
                        else
                        {
                            AddEndLinkAndNextLink(i, pagesCount, endpointId - 1);
                        }
                    }
                }
                else
                {
                    arrows = SectionMarkupHandler
                       .SetNavigation(i, pagesCount, endpointId - 1);
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
            int len = Storage.Fast.PagesLength - 1;
            int pos = Storage.Fast.GetPage(len).IndexOf(Constants.navMarker)
                        + Constants.navMarker.Length;
            Storage.Fast.SetPage(len, Storage.Fast.GetPage(len).Insert(pos, arrows));
        }
        private void AddThreadToBegin
            (int threadId, string threadName)
        {
            AddThread(0, threadId, threadName);
            MoveThreads(threadId, threadName);
        }
        private void MoveThreads(int threadId, string threadName)
        {
            int len = Storage.Fast.PagesLength - 1;
            int j;

            for (int i = 0; i < len; i++)
            {
                j = i + 1;
                Storage.Fast.SetPage(j, Storage.Fast.GetPage(i)
                    .Insert(Storage.Fast.GetPage(j).IndexOf(Constants.navMarker)
                    + Constants.navMarker.Length, GetLastThread(i)));
            }
        }
        private string GetLastThread(int i)
        {
            int brLen = Constants.brMarker.Length;
            int lastThreadPos = Storage.Fast.GetPage(i).LastIndexOf(Constants.pMarker) - brLen;
            int lastSpanPos = Storage.Fast.GetPage(i).LastIndexOf(Constants.spanMarker);
            int count = lastSpanPos - lastThreadPos;
            string result = Storage.Fast.GetPage(i).Substring(lastThreadPos, count);
            Storage.Fast.SetPage(i, Storage.Fast.GetPage(i)
                .Remove(lastThreadPos, count));

            return result;
        }
        private void AddThread
            (int i, int threadId, string threadName)
        {
            SetPosAndTemp(i, threadId, threadName);
            Storage.Fast.SetPage(i, Storage.Fast.GetPage(i)
                .Insert(Storage.Fast.Pos, Storage.Fast.Temp));
        }
        private void SetPosAndTemp
            (int i, int threadId, string threadName)
        {
            Storage.Fast.Pos = Storage.Fast.GetPage(i).IndexOf(Constants.navMarker)
                + Constants.navMarker.Length;
            Storage.Fast.Temp = NewTopicMarkupHandler.GetTemp(threadId, threadName);
        }
        internal static int CountStringOccurrences
            (string text, string pattern)
        {
            int count = 0;
            int i = 0;

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
        public void StartNextTopicByTimer()
        {
            if (Storage.Fast.TopicsToStartCount != 0)
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
            if (endpointId > 0
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
        private static bool CheckThreadName(string message)
        {
            string temp = Constants.SE;
            char c;
            int rusCount = 0;
            int othCount = 1;
            int len = message.Length + 1;

            for (int i = 0; i < len - 1; i++)
            {
                c = message[i];

                if (Constants.AlphabetRusLower.Contains(c))
                {
                    temp += c;
                    rusCount++;
                }
                else if (Constants.Special.Contains(c) || char.IsDigit(c))
                {
                    temp += c;
                    othCount++;
                }
            }
            if ((((double)rusCount) / ((double)len) < 0.6)
                || (rusCount / othCount) < 0.8)
                return false;
            else
                return true;
        }
        internal static bool CheckText(string message)
        {
            string temp = Constants.SE;
            char c;
            int rusCount = 0;
            int othCount = 1;
            int len = message.Length + 1;

            for (int i = 0; i < len - 1; i++)
            {
                c = message[i];

                if (Constants.AlphabetRusLower.Contains(c))
                {
                    temp += c;
                    rusCount++;
                }
                else if (Constants.Special.Contains(c) || char.IsDigit(c))
                {
                    temp += c;
                    othCount++;
                }
            }
            if ((((double)rusCount) / ((double)len) < 0.5)
                || (rusCount / othCount) < 0.8)
                return false;
            else
                return true;
        }
    }
}
