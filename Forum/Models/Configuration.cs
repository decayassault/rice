using Forum.Data;
using Forum.Data.Account;
using Forum.Data.EndPoint;
using Forum.Data.Forum;
using Forum.Data.Section;
using Forum.Data.Thread;
using Forum.Data.PrivateDialog;
using Forum.Data.PrivateMessage;
using Forum.Data.NewPrivateMessage;
using Forum.Data.NewPrivateDialog;
using Forum.Models.Threads;
using System.Threading;
using System.Threading.Tasks;

namespace Forum.Models
{
    internal sealed class Configuration
    {   

        internal static void Initialize()
        {
            /*try
            {*/
            
            RegistrationData.AllowRegistration();
            NewTopicData.AllowNewTopics();
            ReplyData.AllowNewMessages();
            PrivateMessageLogic.AllowPrivateMessages();
            NewPrivateMessageLogic.AllowNewPrivateMessages();
            NewPrivateDialogLogic.AllowNewDialogs();
                Task ConstructConnectionsCache = Task.Run(
                    () => Connection.InitializeConnectionsCache());
                Task.WaitAll(ConstructConnectionsCache);

                Task a = Task.Run(() => AccountLogic.LoadAccounts());
                Task b = Task.Run(() => ForumLogic.LoadMainPage());
                Task c = Task.Run(() => SectionLogic.LoadSectionPages());
                Task d = Task.Run(() => ThreadData.LoadThreadPages());
                Task e = Task.Run(() => EndPointLogic.LoadEndPointPages());
                Task f = Task.Run(() => PrivateDialogLogic.LoadDialogPages());
                //Task g = Task.Run(() => PrivateMessageLogic.LoadPersonalPages());
                PrivateMessageLogic.LoadPersonalPagesNoAsync();
                RegistrationData.LoadRegistrationPages();
                LoginData.LoadLoginPages();  
                
                Thread refreshLogReg = new Thread(Captcha.RefreshLogRegPages);
                refreshLogReg.Priority = ThreadPriority.Lowest;                            
                refreshLogReg.Start();

                Thread copy = new Thread(AccountData.CheckAccountId);
                copy.Priority = ThreadPriority.Lowest;
                copy.Start();
                RegistrationData.AllowRegisterInBase();
                Registrator.Start();
                TopicStarter.Start();
                Messenger.Start();
                PersonalMessenger.Start();
                DialogStarter.Start();
                Task.WaitAll(new Task[] { a, b, c, d, e,f});
            /*}
            catch { Initialize(); }//проверить на дедлоки*/
        }

        
               
    }
}