using Data;
namespace MarkupHandlers
{
    internal sealed class ThreadMarkupHandler
    {
        internal string GetThreadDiv(int i)
        {
            return string.Concat("</div><div class='s'>",
                        Constants.five - i,
                        Constants.indicEnd);
        }
        internal string GetIndicAndA()
        {
            return string.Concat(Constants.indicEnd, Constants.a);
        }
        internal string GetArticle(int accountId, string nick, string msgText)
        {
            return string.Concat("<article><span onClick='n(&quot;/Profile/",
                        accountId,
                        "&quot;);'>",
                        nick,
                        "</span><br /><p>",
                        msgText,
                        "</p></article><br />"); //br2
        }
        internal string GetSectionHeader(int number, int sectionNum, string threadName)
        {
            return string.Concat(Constants.indic,
                        number,
                        "</div><div class='l'><h2 onClick='n(&quot;/s/",
                        sectionNum,
                        "?p=1&quot;);'>",
                        threadName,
                        "</h2>");
        }
        internal string SetNavigation
                    (int pageNumber, int pagesCount, int number)
        {
            return string.Concat(GetArrows(pageNumber, pagesCount, number),
                        "<div id='a'><a onClick='u();return false'>Ответить</a></div>"); //br1
        }
        internal string GetArrows
                  (int pageNumber, int pagesCount, int number)
        {
            string result = Constants.SE;
            string thread = (number + 1).ToString();
            const string a = "<span id='w'><a onClick='n(&quot;/f/";
            const string b = "?p=1&quot;);return false' title='Первая страница'>«</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/";

            if ((pageNumber - 1 >= 1)
                    && (pageNumber + 3 <= pagesCount))
            {
                result = string.Concat(a,
                            thread,
                            b,
                            thread,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pagesCount,
                            "&quot;);return false' title='Последняя страница'>»</a></span>");
            }
            else if ((pageNumber - 1 >= 1)
                    && (pageNumber + 2 == pagesCount))
            {
                result = string.Concat(a,
                            thread,
                            b,
                            thread,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber - 1 >= 1)
                && (pageNumber + 1 == pagesCount))
            {
                result = string.Concat(a,
                            thread,
                            b,
                            thread,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber == 1) && (pageNumber + 3 <= pagesCount))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pagesCount,
                            "&quot;);return false' title='Последняя страница'>»</a></span>");
            }
            else if ((pageNumber == 0) && (pageNumber + 3 <= pagesCount))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pagesCount,
                            "&quot;);return false' title='Последняя страница'>»</a></span>");
            }
            else if ((pageNumber == 1) && (pagesCount == 3))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber == 1) && (pagesCount == 2))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber,
                            "&quot;);return false' title='Предыдущая страница'>◄</a>",
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }
            else if ((pageNumber == 0) && (pagesCount == 2))
            {
                result = string.Concat("<span id='w'>&nbsp;&nbsp;&nbsp;&nbsp;",
                            "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='n(&quot;/f/",
                            thread,
                            "?p=",
                            pageNumber + 2,
                            "&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;</span>");
            }

            return result;
        }
    }
}