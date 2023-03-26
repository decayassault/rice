using Data;
namespace MarkupHandlers
{
    internal sealed class NewPrivateMessageMarkupHandler
    {
        internal string GenerateNewPrivateMessagePageIfEmpty(int companionId,
            string companionNick, byte order, int ownerId, string ownerNick, string text)
        {
            var result = string.Concat(Constants.indic,
                            companionId,
                            "</div><div class='l'><h2 onClick='n(&quot;/d/1&quot;);'>Переписка с ",
                            companionNick,
                            "</h2>");

            if (order == Constants.One)
                result = string.Concat(result,
                            Constants.articleStart,
                            "<span onClick='n(&quot;/Profile/",
                            ownerId,
                            "&quot;);'>",
                            ownerNick,
                            "</span><br />");
            else
                result = string.Concat(result,
                            Constants.articleStart,
                            "<span onClick='n(&quot;/Profile/",
                            companionId,
                            "&quot;);'>",
                            companionNick,
                            "</span><br />");

            return string.Concat(result,
                        "<p>",
                        text,
                        "</p>",
                        Constants.articleEnd,
                        Constants.brMarker,
                        Constants.spanIndicator,
                        Constants.spanEnd,
                        "<div id='a'><a onClick='f();return false'>Ответить ",
                        companionNick,
                        "</a></div></div><div class='s'>4</div>");
        }
        internal string GenerateNewPrivateMessagePage(byte order, int ownerId,
            string ownerNick, int companionId, string companionNick, string text)
        {
            var result = Constants.SE;

            if (order == Constants.One)
                result = string.Concat(Constants.articleStart,
                            "<span onClick='n(&quot;/Profile/",
                            ownerId,
                            "&quot;);'>",
                            ownerNick,
                            "</span><br />");
            else
                result = string.Concat(Constants.articleStart,
                            "<span onClick='n(&quot;/Profile/",
                            companionId,
                            "&quot;);'>",
                            companionNick,
                            "</span><br />");

            return string.Concat(result,
                    "<p>",
                    text,
                    "</p>",
                    Constants.articleEnd,
                    "<br />");
        }
    }
}