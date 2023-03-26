using Forum.Attributes;
using Forum.Data;
using Forum.Data.Account;
using Forum.Data.EndPoint;
using Forum.Data.Forum;
using Forum.Data.Section;
using Forum.Data.PrivateDialog;
using Forum.Data.PrivateMessage;
using Forum.Data.NewPrivateMessage;
using Forum.Data.NewPrivateDialog;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading.Tasks;

namespace Forum.Controllers
{
    [NoCache]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public sealed class forumController : AsyncController
    {
        [HttpGet]
        public string threads()
        {            
            return ForumLogic.GetMainPageLocked();
        }
        
        [HttpGet]
        public string thread(int? Id,int? page)
        {           
            return ThreadData.GetThreadPage(Id, page);            
        }             

        [HttpGet]
        public string maincontent()
        {
            return ForumLogic.GetMainContentLocked();
        }
        
        [HttpGet]
        public string section(int? Id, int? page)
        {           
            return SectionLogic.GetSectionPage(Id, page);            
        }
        
        [HttpGet]
        public string endpoint(int? Id)
        {            
            return EndPointLogic.GetEndPointPage(Id);
        }
      
        [HttpPost]
        public void authenticate
            (string captcha,string login,string password)
        {            
            bool flag=LoginData.CheckAndAuth
                (captcha, login, password);
            if (flag)       
                FormsAuthentication.SetAuthCookie
                        (login.GetHashCode().ToString() +
                        '_' + password.GetHashCode().ToString(), MvcApplication.True);     
        }
        
        [HttpGet]
        public string registrationpage()
        {
            return RegistrationData.PageToReturn;
        }
       
        [Authorize]
        [HttpGet]
        public string login()
        {                       
            return ReplyData.ReplyPage;            
        }
        
        [Authorize]
        [HttpGet]
        public void reply(int? id, string t)
        {
            ReplyData.Start(id,User.Identity.Name,t); 
        }

        [HttpGet]
        public string unknown()
        {
            return AccountLogic.LoginRequirement;
        }
        
        [HttpGet]
        public string loginpage()
        {            
            return LoginData.PageToReturn;
        }
        
        [HttpPost]
        public void register(string captcha, string login, string password,
            string email, string nick)
        {
            RegistrationData.PreRegistration
                (captcha, login, password, email, nick);
        }
        [Authorize]
        [HttpGet]
        public string newtopic()
        {
            return NewTopicData.PageToReturn;
        }

        [Authorize]
        [HttpGet]
        public void starttopic(int? id, string t, string m)
        {            
            NewTopicData.Start(t,id,User.Identity.Name,m);
        }
        
        [Authorize]
        [HttpGet]
        public Task<string> dialog(int? id)
        {           
                return PrivateDialogLogic
                    .GetDialogPage(id, User.Identity.Name);           
        }
        [Authorize]
        [HttpGet]
        public Task<string> personal(int? id, int? page)
        {
            return PrivateMessageLogic.GetPersonalPage
                    (id,page,User.Identity.Name);
        }
        [Authorize]
        [HttpGet]
        public string replypersonal()
        {
            return NewPrivateMessageData.PrivateReplyPage;
        }
        [Authorize]
        [HttpGet]
        public void sendpersonal(int? id, string t)
        {
            NewPrivateMessageLogic.Start(id, User.Identity.Name, t); 
        }
        [Authorize]
        [HttpGet]
        public string newdialog()
        {
            return NewPrivateDialogData.PageToReturn;
        }
        [Authorize]
        [HttpGet]
        public void startdialog(string nick,string msg)
        {
            NewPrivateDialogLogic.Start(nick, User.Identity.Name, msg);
        }
        [Authorize]
        [HttpGet]
        public void exit()
        {
            FormsAuthentication.SignOut();
        }
    }
}
