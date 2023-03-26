using Logic;
namespace MarkupHandlers
{
    internal sealed class ReplyMarkupHandler
    {
        internal string GetPageWithHeader(in int id, in int sectionNum, in string threadName,
            in int accId, in string nick, in string text)
        {
            return string.Concat(Constants.indic,
                        id,
                        "</div><div class='l'><h2 onClick='n(&quot;/s/",
                        sectionNum,
                        "?p=1&quot;);'>",
                        threadName,
                        "</h2>",
                        Constants.articleStart,
                        "<span onClick='n(&quot;/k/",
                        accId,
                        "&quot;);'>",
                        nick,
                        "</span><br /><p>",
                        text,
                        Constants.pEnd,
                        Constants.articleEnd,
                        Constants.brMarker,
                        Constants.spanIndicator,
                        Constants.spanEnd,
                        "<div id='a'><a onClick='u();return false'>Ответить</a></div></div><div class='s'>4</div>");
        }

        internal string GetPage(int accId, string nick, string text)
        {
            return string.Concat(Constants.articleStart,
                        "<span onClick='n(&quot;/k/",
                        accId,
                        "&quot;);'>",
                        nick,
                        "</span><br /><p>",
                        text,
                        Constants.pEnd,
                        Constants.articleEnd,
                        Constants.brMarker);
        }
    }
}