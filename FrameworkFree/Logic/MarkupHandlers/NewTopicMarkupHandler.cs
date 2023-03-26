using Data;
namespace MarkupHandlers
{
    internal sealed class NewTopicMarkupHandler
    {
        internal string GetTemp(int threadId, string threadName)
        {
            return string.Concat("<br /><p onclick='n(&quot;/f/",
                     threadId,
                     "?p=1&quot;);'>",
                     threadName,
                     "</p><br /><br />");
        }
        internal string GetLastPage(int endpointId)
        {
            return string.Concat("<div id='t'><span><a onclick='q();return false;'>Новая тема</a>",
                                        "</span></div><nav class='n'></nav><div class='s'>",
                                        endpointId,
                                        "</div></div>");
        }
        internal string GetNewPage
            (int threadId, int endpointId, string threadName,
             int accountId, string nick, string message)
        {
            return string.Concat("<div class='s'>",
                    threadId - Constants.One,
                    "</div><div class='l'><h2 onclick='n(&quot;/s/",
                    endpointId,
                    "?p=1&quot;);'>",
                    threadName,
                    "</h2><article><span onclick='n(&quot;/Profile/",
                    accountId,
                    "&quot;);'>",
                    nick,
                    "</span><br /><p>",
                    message,
                    "</p></article><br /><div id='a'><a onclick='u();return false'>Ответить</a>",
                    "</div></div><div class='s'>4</div>");
        }
    }
}