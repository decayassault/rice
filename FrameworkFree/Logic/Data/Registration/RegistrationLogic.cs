using System;
using System.Text.RegularExpressions;
using System.Globalization;
using MarkupHandlers;
using Models;
using XXHash;

namespace Data
{
    internal sealed class RegistrationLogic : IRegistrationLogic
    {
        private readonly IStorage Storage;
        private readonly RegistrationMarkupHandler RegistrationMarkupHandler;
        public RegistrationLogic(IStorage storage,
            RegistrationMarkupHandler registrationMarkupHandler)
        {
            Storage = storage;
            RegistrationMarkupHandler = registrationMarkupHandler;
        }
        public void PreRegistration
            (string captcha, string login, string password, string email, string nick)
        {
            var bag = new PreRegBag()
            {
                captcha = captcha,
                login = login,
                password = password,
                email = email,
                nick = nick
            };
            Storage.Fast.PreRegistrationLineTryAdd(Storage.Fast.PreRegistrationLineCount, bag);
        }

        private static readonly object locker = new object();

        public void InitPage()
        {
            var captchaData = Captcha.GenerateCaptchaStringAndImage();
            Storage.Fast.CaptchaMessagesRegistrationDataEnqueue(captchaData.stringHash);

            if (Storage.Fast.CaptchaMessagesRegistrationDataCount
                == Constants.RegistrationPagesCount)
                Storage.Fast.CaptchaMessagesRegistrationDataDequeue();
            Storage.Fast.PageToReturnRegistrationData = RegistrationMarkupHandler
                .GetPageToReturnRegistrationData(captchaData.image);
        }
        public void RegisterInBaseByTimer()
        {
            if (Storage.Fast.RegistrationLineCount != 0)
            {
                for (int i = 0;
                        i < Storage.Fast.RegistrationLineCount; i++)
                {
                    RegBag regBag = new RegBag();
                    Storage.Fast.RegistrationLineTryRemove(i, out regBag);
                    SendToBase(regBag);
                }
            }
        }

        private void SendToBase(RegBag account)
        {
            var dbAccount = new Account
            {
                Nick = account.nick,
                Identifier = (int)account.loginHash,
                Passphrase = (int)account.passwordHash
            };//email

            Storage.Slow.AddAccount(dbAccount);
        }

        public void PutRegInfoByTimer()
        {
            PreRegBag temp = new PreRegBag();
            if (Storage.Fast.PreRegistrationLineCount > 0)
            {
                Storage.Fast.PreRegistrationLineTryRemove(0, out temp);
                if (temp.captcha == null || temp.login == null
            || temp.password == null || temp.email == null || temp.nick == null)
                { }
                else
                {
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

        private bool Register
                (uint loginHash, uint passwordHash, uint nickHash)
        {
            bool result = false;
            var pair = new Pair
            { LoginHash = loginHash, PasswordHash = passwordHash };
            lock (locker)
                if (!Storage.Fast.LoginPasswordHashesContainsKey(pair))
                    if (!Storage.Fast.NicksHashesKeysContains(nickHash))
                        if (!Storage.Fast.LoginPasswordHashesDeltaContainsKey(pair))
                        {
                            Storage.Fast.LoginPasswordHashesTryAdd(pair, null);
                            Storage.Fast.NicksHashesTryAdd(nickHash, 0);
                            Storage.Fast.LoginPasswordHashesDeltaTryAdd(pair, 0);
                            Storage.Fast.CopyDialogPagesArraysToIncreasedSizeArraysAndFillGapsLocked();
                            result = true;
                        }

            return result;
        }

        private void CheckInputAndRegister
            (string captcha, string login, string password,
            string email, string nick)
        {
            if (CheckNick(nick))
            {
                if (CheckEmail(email))
                {
                    if (CheckPassword(password))
                    {
                        if (CheckLogin(login))
                        {
                            uint captchaHash = XXHash32.Hash(captcha);

                            if (Storage.Fast.CaptchaMessagesRegistrationDataContains(captchaHash)
                                || captchaHash == Constants.TestRegistrationEnterCaptchaHash)
                            {
                                uint loginHash = XXHash32.Hash(login);
                                uint passwordHash = XXHash32.Hash(password);
                                uint nickHash = XXHash32.Hash(nick);

                                if (Register(loginHash, passwordHash, nickHash))
                                {
                                    Storage.Fast.RegistrationLineTryAdd(Storage.Fast.RegistrationLineCount,
                                        new RegBag
                                        {
                                            loginHash = loginHash,
                                            passwordHash = passwordHash,
                                            email = email,
                                            nick = nick
                                        }
                                        );
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool CheckLogin(string login)
        {
            bool result = false;
            int len = login.Length;

            if ((len >= 6) && (len <= 25))
            {
                bool flag = false;

                foreach (char x in login)
                {
                    if ((!Char.IsDigit(x))
                        && (Char.IsUpper(x) ?
                                 !Constants.AlphabetRusLower.Contains(Char.ToLower(x)) :
                                    !Constants.AlphabetRusLower.Contains(x)))
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                    result = true;
            }

            return result;
        }

        public bool CheckPassword(string password)
        {
            bool result = false;
            int len = password.Length;

            if ((len >= 8) && (len <= 50))
            {
                bool flag = false;
                foreach (char x in password)
                {
                    if (
                            (Char.IsUpper(x) ?
                                 !Constants.AlphabetRusLower.Contains(Char.ToLower(x)) :
                                    !Constants.AlphabetRusLower.Contains(x))
                            && (!Char.IsDigit(x)
                        )
                        &&
                            (Char.IsUpper(x) ?
                                 !Constants.AlphabetRusLower.Contains(Char.ToLower(x)) :
                                    !Constants.AlphabetRusLower.Contains(x)) &&
                        (!Constants.Special.Contains(x)))
                    {
                        flag = true;

                        break;
                    }
                }
                if (!flag)
                    result = true;
            }

            return result;
        }
        private static bool CheckEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckNick(string nick)
        {
            bool result = false;
            int len = nick.Length;
            if ((len >= 4) && (len <= 25))
            {
                if (new Regex(nick).Matches(" ").Count <= 3)
                {
                    if ((nick[0] != ' ')
                          && (nick[len - 1] != ' '))
                    {
                        string nik = String.Join(Constants.SE, nick.Split(' '));
                        len = nik.Length;

                        if ((len >= 1) && (len <= 22))
                        {
                            bool flag = false;

                            foreach (char x in nik)
                            {
                                if ((Char.IsUpper(x) ?
                                        !Constants.AlphabetRusLower.Contains(Char.ToLower(x)) :
                                        !Constants.AlphabetRusLower.Contains(x))
                                    && !Char.IsDigit(x))
                                {
                                    flag = true;

                                    break;
                                }
                            }

                            if (!flag)
                                result = true;
                        }
                    }
                }
            }

            return result;
        }
    }
}
