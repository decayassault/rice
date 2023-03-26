﻿using Forum.Data;
using Forum.Data.Account;
using Forum.Data.EndPoint;
using Forum.Data.Forum;
using Forum.Data.Section;
using Forum.Data.PrivateDialog;
using Forum.Data.PrivateMessage;
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
                PrivateMessageLogic.LoadPersonalPagesNoAsyncTest();
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
                Task.WaitAll(new Task[] { a, b, c, d, e,f});
            /*}
            catch { Initialize(); }//проверить на дедлоки*/
        }

        
               
    }
}