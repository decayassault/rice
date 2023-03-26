﻿namespace Data
{
    using System;
    using MarkupHandlers;
    using System.Linq;
    internal sealed class NewPrivateMessageLogic : INewPrivateMessageLogic
    {
        public readonly IStorage Storage;
        public readonly IThreadLogic ThreadLogic;
        public readonly IReplyLogic ReplyLogic;
        public readonly NewPrivateMessageMarkupHandler NewPrivateMessageMarkupHandler;
        public readonly PrivateMessageMarkupHandler PrivateMessageMarkupHandler;
        public NewPrivateMessageLogic(IStorage storage,
        IThreadLogic threadLogic,
        IReplyLogic replyLogic,
        NewPrivateMessageMarkupHandler newPrivateMessageMarkupHandler,
        PrivateMessageMarkupHandler privateMessageMarkupHandler)
        {
            Storage = storage;
            ThreadLogic = threadLogic;
            ReplyLogic = replyLogic;
            NewPrivateMessageMarkupHandler = newPrivateMessageMarkupHandler;
            PrivateMessageMarkupHandler = privateMessageMarkupHandler;
        }
        public void Start(int? id, Pair pair, string t)
        {
            if (Storage.Fast.GetPersonalMessagesToPublishCount() < Constants.MaxFirstLineLength)
            {
                MessageData personalMessageData
                      = new MessageData
                      {
                          id = id,
                          pair = pair,
                          text = t
                      };
                Storage.Fast.PersonalMessagesToPublishEnqueue(personalMessageData);
            }
        }
        public void PublishNextPrivateMessageByTimer()
        {
            if (Storage.Fast.GetPersonalMessagesToPublishCount() != Constants.Zero)
            {
                MessageData temp;
                Storage.Fast.PersonalMessagesToPublishTryDequeue(out temp);

                if (temp.id == null || temp.pair == null
                    || temp.text == null)
                { }
                else
                    CheckPersonalReplyAndPublish
                        ((int)temp.id, temp.pair.Value, temp.text);
            }
        }
        public void
            CheckPersonalReplyAndPublish(int id, Pair pair, string text)
        {
            if (Check(id, text))
                PublishPersonalReply(id, pair, text);
        }
        public void PublishPersonalReply
                (int id, Pair pair, string text)
        {
            int? accId = ReplyLogic.GetAccountId(pair);

            if (accId.HasValue)
            {
                Storage.Slow.PutPrivateMessageInBase(accId.Value, id, text);
                string ownerNick = ThreadLogic.GetNick(accId.Value);
                string companionNick = ThreadLogic.GetNick(id);
                byte order = Constants.One;
                CorrectArray(id, accId.Value, text, ownerNick, companionNick, order);

                if (id != accId.Value)
                {
                    order = 2;
                    CorrectArray(accId.Value, id, text, companionNick, ownerNick, order);
                }
            }
        }
        public void CorrectArray(int companionId,
            int ownerId, string text,
            string ownerNick, string companionNick, byte order)
        {
            //TODO неправ. Profile/x и ник x            
            int depth;
            string last = Storage.Fast.GetMessage(ownerId, companionId,
               Storage.Fast.GetPersonalPagesPageDepth(ownerId, companionId) - Constants.One); ;

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
                        NewPrivateMessageMarkupHandler.GenerateNewPrivateMessagePageIfEmpty
                        (companionId, companionNick, order, ownerId, ownerNick, text));
                string[] pages = Storage.Fast.GetMessages(ownerId, companionId);
                int i;
                int start;
                int end;
                string navigation = Constants.SE;

                for (i = Constants.Zero; i < depth; i++)
                {
                    navigation = PrivateMessageMarkupHandler.GetArrows(i, depth, companionId);
                    start = pages[i].LastIndexOf(Constants.spanIndicator);

                    if (start != -1)
                    {
                        end = pages[i].LastIndexOf(Constants.spanEnd)
                            + Constants.spanEnd.Length;
                        pages[i] = pages[i].Remove(start, end - start);
                    }
                    else
                    {
                        start = pages[i].IndexOf(Constants.answerMarker);
                    }
                    pages[i] = pages[i].Insert(start, navigation);
                    Storage.Fast.SetPersonalPagesPage(companionId, ownerId, i, pages[i]);
                }
            }
            else
            {
                int position = last.LastIndexOf(Constants.brMarker) + Constants.brMarker.Length;
                int pos = last.LastIndexOf(Constants.indic) + Constants.indic.Length;
                int start = pos;
                string countString = Constants.SE;

                while (last[pos] != '<')
                {
                    countString += last[pos];
                    pos++;
                }
                last = last.Remove(start, pos - start);
                last = last.Insert(start, (Convert.ToInt32(countString) - Constants.One).ToString());
                last = last.Insert(position,
                    NewPrivateMessageMarkupHandler.GenerateNewPrivateMessagePage(order, ownerId,
                        ownerNick, companionId, companionNick, text));
                depth = Storage.Fast.GetPersonalPagesDepth(companionId, ownerId);
                Storage.Fast.SetPersonalPagesPage
                    (companionId, ownerId, depth
                    - Constants.One, last);
            }
        }

        public bool Check(int id, string text)
        {
            int limit = Storage.Fast.GetDialogPagesLengthLocked();
            if (id > Constants.Zero
                && id <= limit)
            {
                int textLength = text.Length;

                if (textLength < Constants.One
                || textLength > Constants.MaxReplyMessageTextLength)
                    return false;

                char c;
                int rusCount = Constants.Zero;
                int othCount = Constants.One;

                for (int i = Constants.Zero; i < textLength; i++)
                {
                    c = text[i];

                    if (Constants.AlphabetRusLower.Contains(c))
                    {
                        rusCount++;
                    }
                    else if (Storage.Fast.SpecialSearch(c) || char.IsDigit(c))
                    {
                        othCount++;
                    }
                }

                if ((((double)rusCount) / ((double)(textLength + Constants.One)) < 0.5)
                    || (rusCount / othCount) < 0.8)
                    return false;
                else return true;
            }
            else return false;
        }
    }
}
