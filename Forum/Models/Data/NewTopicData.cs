using System.Threading.Tasks;
using Forum.Data.Thread;
using Forum.Models;
using Forum.Data.Account;
using Forum.Data.Section;
namespace Forum.Data
{
 
    internal sealed class NewTopicData
    {
        internal const string PageToReturn= "<p>Заголовок темы:</p>" +
                "<input type='text' tabindex='0' autofocus required maxlength='99' autocomplete='off' />" +
                "<p>Текст сообщения:</p>" +
                "<textarea id='text' tabindex='1' maxlength='1000' wrap='soft' spellcheck='true'></textarea>"+
                "<div id='msg'></div><br /><span><a id='send' onClick='startTopic();return false'>Отправить</a></span>";
        private async static Task PutTopicInBase
            (string threadName, int endpointId, int accountId, string message)
        {
            using (var SqlCon = await Connection.GetConnection())
            {
                using (var cmdStartTopic =
                    Command.InitializeCommandForStartTopic
                        (@"StartTopic", SqlCon,
                        threadName, endpointId,accountId,
                         message))
                {
                    await cmdStartTopic.ExecuteNonQueryAsync();
                }
            }
        }
        private async static Task PublishTopic
                (string threadName,int endpointId,
                    string username, string message)
        {
            int accountId = await ReplyData.GetAccountId(username);
            await PutTopicInBase
                (threadName,endpointId,accountId,message);
            CorrectSectionArray();
            //CorrectArray TODO
        }

        private static void CorrectSectionArray()
        {
            for(int i=0;i<SectionLogic.SectionPagesPageDepth.Length;i++)
            {

            }
            //TODO
        }

        internal async static Task
            CheckTopicAndPublish(string threadName,int endpointId,
                        string username, string message)
        {
            if (CheckTopicInfo(threadName,endpointId,message))
                await PublishTopic
                        (threadName,endpointId, username, message);
        }
        internal static void Start
                (string threadName, int endpointId, string username, string message)
        {
            Task.Run(() =>
                CheckTopicAndPublish
                    (threadName,endpointId,username,message));
        }
        
        private static bool CheckTopicInfo
            (string threadName,int endpointId, string message)
        {
            if (endpointId > MvcApplication.Zero 
                    && endpointId < SectionLogic.SectionPagesLength)
            {
                if (CheckText(message) && CheckThreadName(threadName))
                    return MvcApplication.True;
                else return MvcApplication.False;
            }
            else return MvcApplication.False;
        }
        private static bool CheckThreadName(string message)
        {
            string temp = string.Empty;
            char c;
            int rusCount = MvcApplication.Zero;
            int othCount = MvcApplication.One;
            int len = message.Length + MvcApplication.One;
            for (int i = MvcApplication.Zero; i < len - MvcApplication.One; i++)
            {
                c = message[i];
                if (RegistrationData.AlphabetRusLower.Contains(c))
                {
                    temp += c;
                    rusCount++;
                }
                else if (RegistrationData.Special.Contains(c) || char.IsDigit(c))
                {
                    temp += c;
                    othCount++;
                }
            }
            if ((((double)rusCount) / ((double)len) < 0.6)
                || (rusCount / othCount) < 0.8)
                return MvcApplication.False;
            else return MvcApplication.True;
        }
        private static bool CheckText(string message)
        {
            string temp = string.Empty;
            char c;
            int rusCount = MvcApplication.Zero;
            int othCount = MvcApplication.One;
            int len = message.Length + MvcApplication.One;
            for (int i = MvcApplication.Zero; i < len - MvcApplication.One; i++)
            {
                c = message[i];
                if (RegistrationData.AlphabetRusLower.Contains(c))
                {
                    temp += c;
                    rusCount++;
                }
                else if (RegistrationData.Special.Contains(c) || char.IsDigit(c))
                {
                    temp += c;
                    othCount++;
                }
            }
            if ((((double)rusCount) / ((double)len) < 0.5)
                || (rusCount / othCount) < 0.8)
                return MvcApplication.False;
            else return MvcApplication.True;
        }
    }
}