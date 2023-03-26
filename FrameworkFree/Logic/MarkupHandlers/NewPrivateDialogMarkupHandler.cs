using Data;
namespace MarkupHandlers
{
    internal sealed class NewPrivateDialogMarkupHandler
    {
        internal string GenerateNewPrivateDialogEntry
                        (int secondId, string secondNick)
        {
            return string.Concat(Constants.brMarker,
                                Constants.pStart,
                                secondId,
                                Constants.pMiddle,
                                secondNick,
                                Constants.brMarker,
                                Constants.brMarker);
        }
        internal string GenerateNewPrivateDialogEntryWithEnd
            (int secondId, string secondNick)
        {
            return string.Concat(Constants.brMarker,
                                Constants.pStart,
                                secondId,
                                Constants.pMiddle,
                                secondNick,
                                Constants.pEnd,
                                Constants.brMarker,
                                Constants.brMarker);
        }
        internal string GenerateNewPrivateDialogPage(int companionId,
            string companionNick, int ownerId, string ownerNick, string message)
        {
            return string.Concat(Constants.indic,
                companionId,
                "</div><div class='l'><h2 onClick='n(&quot;/d/1&quot;);'>Переписка с ",
                companionNick,
                "</h2>",
                Constants.articleStart,
                "<span onClick='n(&quot;/Profile/",
                ownerId,
                "&quot;);'>",
                ownerNick,
                "</span><br /><p>",
                message,
                "</p>",
                Constants.articleEnd,
                "<br />",
                Constants.fullSpanMarker,
                Constants.endSpanMarker,
                "<div id='a'><a onClick='replyPM();return false'>Ответить ",
                companionNick,
                "</a></div></div><div class='s'>4</div>");
        }

        internal string GenerateNewPrivateDialogArticle(int accountId, string accountNick, string message)
        {
            return string.Concat(Constants.articleStart,
                    "<span onClick='n(&quot;/Profile/",
                    accountId,
                    "&quot;);'>",
                    accountNick,
                    "</span><br /><p>",
                    message,
                    "</p>",
                    Constants.articleEnd,
                    "<br />");
        }
    }
}