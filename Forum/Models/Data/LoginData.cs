using System.Collections.Generic;
using Forum.Data.Account;
using Forum.Models;
using System.Threading.Tasks;

namespace Forum.Data
{
    internal sealed class LoginData
    {
        internal const int LoginPagesCount = 30;

        private volatile static Queue<string> CaptchaMessages;

        internal volatile static string PageToReturn;

        internal async static void InitPage()
        {
            var res = await Captcha.GetImageCode();
            string toQueue = res.message;
            CaptchaMessages.Enqueue(toQueue);
            if (CaptchaMessages.Count == LoginPagesCount)
                CaptchaMessages.Dequeue();
            PageToReturn = "<div id='logon'>" + res.image +  
                "<br /><a>Число на картинке</a><br><input id='captcha' type='text'><br />" +
                 "<a>Логин</a><br /><input id='login' type='password'><br />" +
                 "<a>Пароль</a><br /><input id='password'" +
                 " type='password'><br /><div id='msg'></div><br />" +
                 "<a onClick='d();return false'>Войти</a></div>";
        }        

        internal static bool CheckAndAuth
               (string captcha, string login,
                string password)
        {
            
                if (RegistrationData.CheckPassword(password))
                {
                    if (RegistrationData.CheckLogin(login))
                    {
                        if (CheckCaptcha(captcha))
                        {
                            if(AccountData.CheckPair(login,password))
                            {                                
                                return MvcApplication.True;                               
                            }                            
                        }                        
                }                
            }
                return MvcApplication.False;
        }
              

        private static bool CheckCaptcha(string captcha)
        {
            bool result = MvcApplication.False;
            if (CaptchaMessages.Contains(captcha))
                result = MvcApplication.True;

            return result;
        }

        internal static void LoadLoginPages()
        {
            CaptchaMessages = new Queue<string>(LoginPagesCount);
        }

    }
}