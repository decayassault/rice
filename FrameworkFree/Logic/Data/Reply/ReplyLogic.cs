using System;
using Models;
using MarkupHandlers;
namespace Data
{
    internal sealed class ReplyLogic : IReplyLogic
    {
        private static readonly object locker = new object();
        private readonly IStorage Storage;
        private readonly IAccountLogic AccountLogic;
        private readonly IThreadLogic ThreadLogic;
        private readonly ReplyMarkupHandler ReplyMarkupHandler;
        private readonly ThreadMarkupHandler ThreadMarkupHandler;
        public ReplyLogic(IStorage storage,
        IAccountLogic accountLogic,
        IThreadLogic threadLogic,
        ReplyMarkupHandler replyMarkupHandler,
        ThreadMarkupHandler threadMarkupHandler)
        {
            Storage = storage;
            AccountLogic = accountLogic;
            ThreadLogic = threadLogic;
            ReplyMarkupHandler = replyMarkupHandler;
            ThreadMarkupHandler = threadMarkupHandler;
        }
        internal void
            CheckReplyAndPublish(int id, Pair pair, string text)
        {
            if (Check(id, text))
                PublishReply(id, pair, text);
        }

        public void Start(int? id, Pair pair, string t)
        {
            MessageData messageData
                  = new MessageData
                  {
                      id = id,
                      pair = pair,
                      text = t
                  };
            Storage.Fast.MessagesToPublishEnqueue(messageData);
        }
        public void PublishNextMessageByTimer()
        {
            if (Storage.Fast.MessagesToPublishCount != 0)
            {
                MessageData temp;
                Storage.Fast.MessagesToPublishTryDequeue(out temp);

                if (temp.id == null || temp.pair == null
                    || temp.text == null)
                { }
                else
                    CheckReplyAndPublish
                        ((int)temp.id, temp.pair.Value, temp.text);
            }
        }

        public int? GetAccountId(Pair pair)
        {
            int? accId = null;

            try
            {
                accId = Storage.Fast.GetLoginPasswordAccIdHashes(pair);
            }
            catch
            {
                lock (locker)
                    accId = AccountLogic.GetAccIdAndStore(pair);
            }

            return accId;
        }

        private void PublishReply
                (int id, Pair pair, string text)
        {
            int? accId = GetAccountId(pair);

            if (accId.HasValue)
            {
                Storage.Slow.PutMessageInBase(new Msg
                { ThreadId = id + 1, AccountId = accId.Value, MsgText = text });
                CorrectArray(id, accId.Value, text);
            }
        }

        private void CorrectArray(int id, int accId, string text)
        {
            string nick = ThreadLogic.GetNick(accId);
            string page;
            string last =
                Storage.Fast.GetThreadPagesPageLocked
                    (id, Storage.Fast.GetThreadPagesPageDepthLocked(id)
                    - 1);

            if (last.Contains(Constants.a))
            {
                string threadName = Storage.Slow.GetThreadNameById(id + 1);
                int sectionNum = Storage.Slow.GetSectionNumById(id + 1);
                string[] temp = Storage.Fast.GetThreadPagesArrayLocked(id);
                Storage.Fast.AddToThreadPagesPageDepthLocked(id, 1);
                Storage.Fast.SetThreadPagesArrayLocked
                    (id, new string[Storage.Fast.GetThreadPagesPageDepthLocked(id)]);
                temp.CopyTo(Storage.Fast.GetThreadPagesArrayLocked(id)
                            , 0);
                page = ReplyMarkupHandler.GetPageWithHeader(id, sectionNum, threadName,
                    accId, nick, text);
                int len = Storage.Fast.GetThreadPagesPageDepthLocked(id);
                Storage.Fast.SetThreadPagesPageLocked(id, len - 1, page);
                string[] thread = Storage.Fast.GetThreadPagesArrayLocked(id);
                int i;
                int start;
                int end;
                string navigation = Constants.SE;

                for (i = 0; i < len; i++)
                {
                    navigation = ThreadMarkupHandler.GetArrows(i, len, id);
                    start = thread[i].LastIndexOf(Constants.spanIndicator);
                    end = thread[i].LastIndexOf(Constants.spanEnd)
                            + Constants.spanEnd.Length;
                    thread[i] = thread[i].Remove(start, end - start);
                    thread[i] = thread[i].Insert(start, navigation);
                    Storage.Fast.SetThreadPagesPageLocked(id, i, thread[i]);
                }
            }
            else
            {
                int position = last.LastIndexOf(Constants.brMarker) + Constants.brMarker.Length;
                int pos = last.LastIndexOf(Constants.indic) + Constants.indic.Length;
                int start = pos;
                string countString = Constants.SE;

                while (last[pos] != Constants.TagStartSymbol)
                {
                    countString += last[pos];
                    pos++;
                }
                last = last.Remove(start, pos - start);
                last = last.Insert(start, (Convert.ToInt32(countString) - 1).ToString());
                page = ReplyMarkupHandler.GetPage(accId, nick, text);
                last = last.Insert(position, page);
                Storage.Fast.SetThreadPagesPageLocked
                    (id, Storage.Fast.GetThreadPagesPageDepthLocked(id)
                    - 1, last);
            }
        }
        private bool Check(int id, string text)
        {
            if (id >= 0
                && id < Storage.Fast.GetThreadPagesLengthLocked())
            {
                string temp = Constants.SE;
                char c;
                int rusCount = 0;
                int othCount = 1;
                int len = text.Length + 1;

                for (int i = 0; i < len - 1; i++)
                {
                    c = text[i];

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
            else
                return false;
        }
    }
}
