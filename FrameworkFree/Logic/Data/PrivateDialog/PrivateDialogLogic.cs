﻿using System.Collections.Generic;
using MarkupHandlers;
using System.Linq;

namespace Data
{
    internal sealed class PrivateDialogLogic : IPrivateDialogLogic
    {
        public readonly IStorage Storage;
        public readonly IReplyLogic ReplyLogic;
        public readonly PrivateDialogMarkupHandler PrivateDialogMarkupHandler;
        private static readonly object locker = new object();
        public PrivateDialogLogic(IStorage storage,
        IReplyLogic replyLogic,
        PrivateDialogMarkupHandler privateDialogMarkupHandler)
        {
            Storage = storage;
            ReplyLogic = replyLogic;
            PrivateDialogMarkupHandler = privateDialogMarkupHandler;
        }
        public void AddDialog(int Num)
        {
            int count;
            int number = Num;
            int id = number + 1;

            count = Storage.Slow.CountPrivateMessages(id);

            if (count == 0)
                count++;
            int pagesCount = count / Constants.DialogsOnPage;

            if (count - pagesCount * Constants.DialogsOnPage > 0)
                pagesCount++;
            Storage.Fast.SetDialogPagesArrayLocked
                            (number, new string[pagesCount]);
            Storage.Fast.SetDialogPagesPageDepthLocked(number, pagesCount);

            for (int i = 0; i < pagesCount; i++)
                Storage.Fast
                    .SetDialogPagesPageLocked(number, i, Constants.NewDialog);

            ProcessDialogReader(Storage.Slow.GetIdNicksByAccountId(id),
                number, pagesCount);
        }
        public void ProcessDialogReader
            (IEnumerable<IdNick> idNicks, int number, int pagesCount)
        {
            if (idNicks.Any())
            {
                /*string endpointHidden = Constants.SE;
                */
                int pageNumber = 0;
                var i = 0;

                foreach (var idNick in idNicks)
                {
                    if (i == 0)
                        Storage.Fast.AddToDialogPagesPageLocked
                            (number, pageNumber, string.Concat(Constants.navMarker,
                                                Constants.brMarker));
                    Storage.Fast.AddToDialogPagesPageLocked
                        (number, pageNumber,
                            PrivateDialogMarkupHandler.GetPersonalLinkText(idNick.Id, idNick.Nick));
                    i++;

                    if (i == Constants.DialogsOnPage)
                    {
                        Storage.Fast.AddToDialogPagesPageLocked
                            (number, pageNumber,
                            string.Concat(PrivateDialogMarkupHandler.GetArrows(pageNumber, pagesCount),
                            Constants.endNavMarker));
                        //+ endpointHidden);

                        i = 0;
                        pageNumber++;
                    }
                    else
                        Storage.Fast.AddToDialogPagesPageLocked
                            (number, pageNumber, Constants.brMarker);
                }

                RemoveBrOfIncompletePages(number);

                if ((i < Constants.DialogsOnPage) && (i > 0))
                {
                    if (pageNumber > 0)
                    {
                        Storage.Fast.AddToDialogPagesPageLocked
                            (number, pageNumber,
                            PrivateDialogMarkupHandler.GetArrows(pageNumber, pagesCount));
                    }
                    Storage.Fast.AddToDialogPagesPageLocked
                        (number, pageNumber, Constants.endNavMarker);
                    //+ endpointHidden);
                }
            }
        }
        public void RemoveBrOfIncompletePages(int number)
        {
            string temp = Storage.Fast.GetDialogPagesPageLocked(number,
                   Storage.Fast
                    .GetDialogPagesArrayLocked(number).Length - 1);
            int pos = temp.LastIndexOf(Constants.brMarker);
            temp = temp.Remove(pos, Constants.brMarker.Length);
            Storage.Fast.SetDialogPagesPageLocked(number,
                Storage.Fast
                    .GetDialogPagesArrayLocked(number).Length - 1, temp);
        }

        public string GetDialogPage
                            (int? page, Pair pair)
        {
            if (page == null)
                return Constants.SE;
            else
            {
                int? accountId = ReplyLogic.GetAccountId(pair);

                if (accountId.HasValue)
                {
                    int index = accountId.Value - 1;

                    if (page > 0
                        && page
                        <= Storage.Fast.GetDialogPagesPageDepthLocked(index))
                    {
                        return Storage.Fast
                          .GetDialogPagesPageLocked(index
                             , (int)page - 1);
                    }
                    else
                        return Constants.SE;
                }
                else
                    return Constants.SE;
            }
        }

        public void LoadDialogPages()
        {
            Storage.Fast.SetDialogPagesLengthLocked(Storage.Slow.GetAccountsCount());
            Storage.Fast.InitializeDialogPagesPageDepthLocked(
                new int[Storage.Fast.GetDialogPagesLengthLocked()]);
            Storage.Fast.InitializeDialogPagesLocked
                (new string[Storage.Fast.GetDialogPagesLengthLocked()][]);
            int len = Storage.Fast.GetDialogPagesLengthLocked();

            for (int i = 0;
                i < len; i++)
                AddDialog(i);
        }
    }
}
