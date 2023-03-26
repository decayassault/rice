using System.Threading.Tasks;
using Forum.Models;
namespace Forum.Data.PrivateMessage
{   
    internal sealed class PrivateMessageData
    {
        private async static Task PutPrivateMessageInBase
            (int senderAccId,int acceptorAccId, string privateText)
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