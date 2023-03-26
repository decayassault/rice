using Data;
using System.Collections.Generic;
using System.Linq;
namespace MarkupHandlers
{
    internal sealed class PrivateMessageMarkupHandler
    {
        internal string SetNewCompanionPageMarkup(int id, string companionNick)
        {
            return string.Concat("<div class='s'>",
                    id,
                    "</div><div class='l'><h2 onclick='n(&quot;/d/1&quot;);'>Переписка с ",
                    companionNick,
                    "</h2><div id='a'><a onclick='replyPM();return false'>Ответить ",
                    companionNick,
                    "</a></div></div><div class='s'>5</div>");
        }
        internal string GetArrows
            (int pageNumber, int pagesCount, int companionId)
        {
            string result = Constants.SE;
            string personal = companionId.ToString();
            const string a = "<span id='w'><a onClick='n(&quot;/p/";
            const string b = "?p=1&quot;);return false' title='Первая страница'>«</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/p/";

            if ((pageNumber - 1 >= 1)
                    && (pageNumber + 3 <= pagesCount))
            {
                result = string.Concat(a,
                            personal,
                            b,
                            personal,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/p/",
                            personal,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/p/",
                            personal,
                            "?p=",
                            pagesCount,
                            "&quot;);return false' title='Последняя страница'>»</a></span>");
            }
            else if ((pageNumber - 1 >= 1)
                    && (pageNumber + 2 == pagesCount))
            {
                result = string.Concat(a,
                            personal,
                            b,
                            personal,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/p/",
                            personal,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber - 1 >= 1)
                && (pageNumber + 1 == pagesCount))
            {
                result = string.Concat(a,
                            personal,
                            b,
                            personal,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber == 1) && (pageNumber + 3 <= pagesCount))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "<a onClick='n(&quot;/p/",
                            personal,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/p/",
                            personal,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/p/",
                            personal,
                            "?p=",
                            pagesCount,
                            "&quot;);return false' title='Последняя страница'>»</a></span>");
            }
            else if ((pageNumber == 0) && (pageNumber + 3 <= pagesCount))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/p/",
                            personal,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/p/",
                            personal,
                            "?p=",
                            pagesCount,
                            "&quot;);return false' title='Последняя страница'>»</a></span>");
            }
            else if ((pageNumber == 1) && (pagesCount == 3))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "<a onClick='n(&quot;/p/",
                            personal,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/p/",
                            personal,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber == 1) && (pagesCount == 2))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "<a onClick='n(&quot;/p/",
                            personal,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber == 0) && (pagesCount == 2))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/p/",
                            personal,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }

            return result;
        }
        internal string SetNavigation
            (int pageNumber, int pagesCount, int authorId, string companionNick)
        {
            return string.Concat(GetArrows(pageNumber, pagesCount, authorId),
                    "<div id='a'><a onClick='replyPM();return false'>Ответить ",
                    companionNick,
                    "</a></div>");
        }
        internal PrivateMessages ProcessCompanionReaderMarkup(string companionNick, int companionId,
        IEnumerable<IdText> idTexts, int pagesCount, string accountNick)
        {
            int pageNumber = 0;
            var result = new PrivateMessages
            { Messages = new string[pagesCount] };

            string dialogName = string.Concat("Переписка с ", companionNick);

            result.Messages[pageNumber] = string.Concat("<div class='s'>",
                                            companionId,
                                            "</div><div class='l'><h2 onClick='n(&quot;/d/1&quot;);'>",
                                            dialogName,
                                            "</h2>");

            bool first = false;

            if (idTexts.Count() > 0)
            {
                int authorId;
                int i = 0;
                string privateText;

                foreach (var idText in idTexts)
                {
                    if (i == 0 && first)
                    {
                        result.Messages[pageNumber] += "<div class='s'>"
                            + companionId.ToString() +
                         "</div><div class='l'><h2 onClick='n(&quot;/d/1" +
                         "&quot;);'>" + dialogName + "</h2>";
                    }

                    authorId = idText.SenderAccountId;
                    privateText = idText.PrivateText;

                    string nick;
                    if (authorId == companionId)
                        nick = companionNick;
                    else
                        nick = accountNick;
                    result.Messages[pageNumber] = string.Concat(result.Messages[pageNumber],
                                                    "<article><span onClick='n(&quot;/Profile/",
                                                    authorId,
                                                    "&quot;);'>",
                                                    nick,
                                                    "</span><br /><p>",
                                                    privateText,
                                                    "</p></article><br />");

                    i++;

                    if (i == Constants.five)
                    {
                        result.Messages[pageNumber] = string.Concat(result.Messages[pageNumber],
                                                        SetNavigation
                                (pageNumber, pagesCount, companionId, companionNick));
                        if (first)
                            result.Messages[pageNumber] = string.Concat(result.Messages[pageNumber],
                                                            "</div><div class='s'>0</div>");

                        i = 0;
                        pageNumber++;
                    }

                    if (!first)
                        first = true;
                }
                if ((pageNumber >= 0)
                        && (i < Constants.five) && (i > 0))
                {
                    result.Messages[pageNumber] +=
                                SetNavigation
                                (pageNumber, pagesCount, companionId, companionNick);
                    if (first)
                        result.Messages[pageNumber] = string.Concat(result.Messages[pageNumber],
                                                         "</div><div class='s'>",
                                                         Constants.five - i,
                                                         Constants.indicEnd);
                }
            }
            if (!first)
                result.Messages[pageNumber] = string.Concat(result.Messages[pageNumber],
                                                Constants.indicEnd);

            return result;
        }
    }
}