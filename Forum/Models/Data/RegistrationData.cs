using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using System.Collections.Concurrent;
using System.Threading;
using Forum.Models;
using Forum.Data.Account;

namespace Forum.Data
{
    internal sealed class RegistrationData
    {
        internal const int RegistrationPagesCount = 120;

        private const string RegisterParam=@"Register";

        private volatile static Queue<string> CaptchaMessages;

        internal static List<char> AlphabetRusLower =
                            new List<char> { 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 
                                'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 
                                'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 
                                'ъ', 'ы', 'ь', 'э', 'ю', 'я' };

        internal static List<char> Special = new List<char>{
            '.', ',', '-', ' ', '!', '?', ';', ':', '"'
        };

        internal volatile static string PageToReturn;

        internal static ConcurrentDictionary<int, RegBag> RegistrationLine;
        internal static ConcurrentDictionary<int,PreRegBag> PreRegistrationLine;

        internal static void PreRegistration
            (string captcha, string login, string password, string email, string nick)
        {
            var bag = new PreRegBag()
            {
                captcha=captcha,
                login = login,
                password = password,
                email=email,
                nick=nick
            };
           bool flag= PreRegistrationLine.TryAdd(PreRegistrationLine.Count, bag);
        }

        internal struct PreRegBag
        {
            public string captcha;
            public string login;
            public string password;
            public string email;
            public string nick;
        }

        internal struct RegBag
        {
            public int loginHash;
            public int passwordHash;
            public string email;
            public string nick;           
        }  

        private static bool InvalidEmail=false;        

        internal static void AllowRegistration()
        {
            PreRegistrationLine =
                    new ConcurrentDictionary<int, PreRegBag>();
        }

        internal static void AllowRegisterInBase()
        {
            RegistrationData.RegistrationLine =
                new ConcurrentDictionary<int, RegistrationData.RegBag>();
        }

        internal async static void InitPage()
        {
            var res = await Captcha.GetImageCode();            
            string toQueue=res.message;
            CaptchaMessages.Enqueue(toQueue);
            if(CaptchaMessages.Count==RegistrationPagesCount)
                CaptchaMessages.Dequeue();
            PageToReturn="<div id='registration'>" + res.image +
                "<br /><a>Число на картинке</a><br /><input id='captcha' type='text'><br />" +
             "<a>Логин</a><br /><input id='login' type='password'><br />" +
             "<a>Пароль</a><br /><input id='password' type='password'><br />" +
            "<a>Повторите пароль</a><br /><input id='pwdconfirm' type='password'><br />" +
            "<a>Электронная почта</a><br /><input id='email' type='password'><br />" +
            "<a>Повторите почту</a><br /><input id='emailconfirm' type='password'><br />"
            + "<a>Имя на форуме</a><br /><input id='nick' type='text'><br /><div>Обратите" +
            " внимание: Ваши персональные данные не требуются для пользования форумом." +
            "</div><div id='msg'></div><br />" +
            "<a onClick='c();return false'>Продолжить</a></div>";            
        }    

        internal static void LoadRegistrationPages()
        {
            CaptchaMessages = new Queue<string>(RegistrationPagesCount);
        }

        private static bool CheckCaptcha(string captcha)
        {
            bool result = MvcApplication.False;
            if (CaptchaMessages.Contains(captcha))
                result = MvcApplication.True;

            return result;
        }        

        internal async static void RegisterInBase()
        {       
            while(MvcApplication.True)
            {
                if (RegistrationLine.Count != MvcApplication.Zero)
                {
                    for (int i = MvcApplication.Zero; 
                            i < RegistrationLine.Count; i++)
                    {
                        RegBag regBag=new RegBag();
                        RegistrationLine.TryRemove(i,out regBag);
                        SendToBase(regBag);
                    }
                }
                System.Threading.Thread.Sleep(10);
            }
        }

        private async static void SendToBase(RegBag account)
        {
            using (var SqlCon = await Connection.GetConnection())
            {
                using (var cmdReg =
                        Command.InitializeCommandForInputRegister
                          (RegisterParam, 
                          SqlCon,
                          account.loginHash, 
                          account.passwordHash,
                          account.email, 
                          account.nick))
                {
                    await cmdReg.ExecuteNonQueryAsync();
                }
            }
        }

        internal static void PutRegInfo()
        {
            PreRegBag temp=new PreRegBag();
            while (MvcApplication.True)
            {
                if (PreRegistrationLine.Count > MvcApplication.Zero)
                {
                    PreRegistrationLine.TryRemove(MvcApplication.Zero, out temp);
                    CheckInputAndRegister(
                        temp.captcha,
                        temp.login,
                        temp.password,
                        temp.email,
                        temp.nick
                        );
                }
            }
        }

        private static bool Register
                (int loginHash,int passwordHash,int nickHash)
        {
            bool result=MvcApplication.False;
            var pair = new AccountData.Pair 
                { LoginHash = loginHash, PasswordHash = passwordHash };
            if (!AccountData.LoginPasswordHashes.ContainsKey(pair))
                if (!AccountData.NicksHashes.ContainsKey(nickHash))
                {
                    AccountData.LoginPasswordHashes.TryAdd(pair, MvcApplication.Zero);
                    AccountData.NicksHashes.TryAdd(nickHash, MvcApplication.Zero);
                    result = MvcApplication.True;
                }

            return result;
        }

        private static void CheckInputAndRegister
            (string captcha, string login, string password,
            string email, string nick)
        {
            if (CheckNick(nick))
            {
                if (CheckEmail(email))
                {
                    if (CheckPassword(password))
                    {
                        if(CheckLogin(login))
                        {
                            if(CheckCaptcha(captcha))
                            {
                                int loginHash = login.GetHashCode();
                                int passwordHash = password.GetHashCode();                                
                                int nickHash = nick.GetHashCode();
                                if (Register(loginHash, passwordHash, nickHash))
                                {
                                    RegistrationLine.TryAdd(RegistrationLine.Count,
                                        new RegBag{
                                         loginHash=loginHash,
                                         passwordHash=passwordHash,
                                         email=email,
                                         nick=nick
                                        }                                        
                                        );                                    
                                }                                
                            }                           
                        }                       
                    }                   
                }                
            }            
        }

        internal static bool CheckLogin(string login)
        {
            bool result=MvcApplication.False;
            int len = login.Length;
            if((len>=6)&&(len<=25))
            {
                bool flag=MvcApplication.False;
                foreach(char x in login)
                {
                    if ((!Char.IsDigit(x))
                        &&(Char.IsUpper(x) ?
                                 !AlphabetRusLower.Contains(Char.ToLower(x)) :
                                    !AlphabetRusLower.Contains(x)))
                    {
                        flag=MvcApplication.True;
                        break;
                    }
                }
                if(!flag)
                    result=MvcApplication.True;
            }

            return result;
        }

        internal static bool CheckPassword(string password)
        {
            bool result = MvcApplication.False;
            int len = password.Length;
            if((len>=8)&&(len<=50))
            {
                bool flag=MvcApplication.False;
                foreach(char x in password)
                {
                    if ((Char.IsUpper(x) ?
                                 !AlphabetRusLower.Contains(Char.ToLower(x)) :
                                    !AlphabetRusLower.Contains(x)) && (!Char.IsDigit(x))
                        &&(Char.IsUpper(x) ?
                                 !AlphabetRusLower.Contains(Char.ToLower(x)) :
                                    !AlphabetRusLower.Contains(x)) &&
                        (!Special.Contains(x)))
                    {
                        flag = MvcApplication.True;
                        break;
                    }
                }
                if(!flag)
                    result=MvcApplication.True;
            }

            return result;
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
               InvalidEmail = MvcApplication.True;
            }
            return match.Groups[MvcApplication.One].Value + domainName;
        }

        private static bool CheckEmail(string email)
        {
            InvalidEmail = MvcApplication.False;
            string strIn = email;
            if (String.IsNullOrEmpty(strIn))
                return MvcApplication.False;

            // Use IdnMapping class to convert Unicode domain names. 
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper,
                                RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return MvcApplication.False;
            }

            if (InvalidEmail)
                return MvcApplication.False;

            // Return true if strIn is in valid e-mail format. 
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,24}))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return MvcApplication.False;
            }
        }

        private static bool CheckNick(string nick)
        {
            bool result = MvcApplication.False;
            int len = nick.Length;
            if((len>=4)&&(len<=25))
            {
              if(new Regex(nick).Matches(" ").Count<=3)
              {
                  if ((nick[MvcApplication.Zero] != ' ')
                        && (nick[len - 1] != ' '))
                {
                    string nik = String.Join(String.Empty, nick.Split(' '));
                    len = nik.Length;
                    if ((len >= MvcApplication.One) && (len <= 22))
                    {
                        bool flag = MvcApplication.False;
                        foreach (char x in nik)
                            {
                             if ((Char.IsUpper(x) ? 
                                 !AlphabetRusLower.Contains(Char.ToLower(x)) :
                                    !AlphabetRusLower.Contains(x))&&(!Char.IsDigit(x)))
                                    {
                                        flag = MvcApplication.True;
                                        break;
                                    }
                            }
                        if (!flag)
                            result = MvcApplication.True;
                    }
                }
              }
            }

            return result;
        }
    }
}