using Forum.Models;
namespace Forum.Data.NewPrivateMessage
{
    using System.Threading.Tasks;
    internal sealed class NewPrivateMessageData
    {
        internal const string PrivateReplyPage = "<p>Ваш ответ:</p>" +
            "<textarea id='text' autofocus maxlength='1000' wrap='soft' spellcheck='true'></textarea>" +
            "<div id='msg'></div><br /><span><a id='send' onClick='sendPrivateReply();return false'>Отправить</a></span>";
        internal async static Task PutPrivateMessageInBase
            (int senderAccId, int acceptorAccId, string privateText)
        {
            using (var SqlCon = await Connection.GetConnection())
            {
                using (var cmdAddPrivateMessage =
                    Command.InitializeCommandForPutPrivateMessage
                        (@"AddPrivateMessage", SqlCon, senderAccId,
                        acceptorAccId
                        , privateText))
                {
                    await cmdAddPrivateMessage.ExecuteNonQueryAsync();
                }
            }
        }
    }
}