using Data;
namespace MarkupHandlers
{
    internal sealed class PrivateDialogMarkupHandler
    {
        internal string GetArrows(int pageNumber, int pagesCount)
        {
            var result = Constants.SE;

            if ((pageNumber - 1 >= 1)
                    && (pageNumber + 3 <= pagesCount))
            {
                result = string.Concat("<span id='w'><a onClick='n(&quot;/d/1",
                    "&quot;);return false' title='Первая страница'>«</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/d/",
                    pageNumber,
                    "&quot;);return false' title='Предыдущая страница'>◄</a>",
                    "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/d/",
              (pageNumber + 2).ToString(),
              "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/d/",
              pagesCount,
              "&quot;);return false' title='Последняя страница'>»</a></span>");

            }
            else if ((pageNumber - 1 >= 1)
                    && (pageNumber + 2 == pagesCount))
            {
                result = string.Concat("<span id='w'><a onClick='n(&quot;/d/1",
                   "&quot;);return false' title='Первая страница'>«</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/d/",
                   pageNumber,
                   "&quot;);return false' title='Предыдущая страница'>◄</a>",
                   "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/d/",
                   pageNumber + 2,
                   "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber - 1 >= 1)
                    && (pageNumber + 1 == pagesCount))
            {
                result = string.Concat("<span id='w'><a onClick='n(&quot;/d/1",
                    "&quot;);return false' title='Первая страница'>«</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/d/",
                    pageNumber,
                    "&quot;);return false' title='Предыдущая страница'>◄</a>",
                    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber == 1)
                    && (pageNumber + 3 <= pagesCount))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                    "<a onClick='n(&quot;/d/",
                   pageNumber,
                   "&quot;);return false' title='Предыдущая страница'>◄</a>",
                   "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/d/",
                   pageNumber + 2,
                   "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/d/",
                   pagesCount,
                   "&quot;);return false' title='Последняя страница'>»</a></span>");
            }
            else if ((pageNumber == 0)
                    && (pageNumber + 3 <= pagesCount))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                        "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/d/",
                        pageNumber + 2,
                        "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/d/",
                        pagesCount,
                        "&quot;);return false' title='Последняя страница'>»</a></span>");
            }
            else if ((pageNumber == 1) && (pagesCount == 3))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                    "<a onClick='n(&quot;/d/",
                   pageNumber,
                   "&quot;);return false' title='Предыдущая страница'>◄</a>",
                   "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/d/",
                   pageNumber + 2,
                   "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber == 1) && (pagesCount == 2))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                    "<a onClick='n(&quot;/d/",
                   pageNumber,
                   "&quot;);return false' title='Предыдущая страница'>◄</a>",
                   "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber == 0) && (pagesCount == 2))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                    "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/d/",
                    pageNumber + 2,
                    "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }

            return string.Concat(result,
                        "<br />");
        }

        internal string GetPersonalLinkText(int accountId, string nick)
        {
            return string.Concat("<p onClick='n(&quot;/p/",
                        accountId,
                        "?p=1&quot;);'>",
                        nick,
                        "</p><br /><br />");
        }
    }
}