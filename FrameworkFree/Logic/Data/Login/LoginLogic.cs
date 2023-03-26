﻿using XXHash;
using MarkupHandlers;
namespace Data
{
    internal sealed class LoginLogic : ILoginLogic
    {
        public readonly IStorage Storage;
        public readonly IRegistrationLogic RegistrationLogic;
        public readonly IAccountLogic AccountLogic;
        public readonly IAuthenticationLogic AuthenticationLogic;
        public readonly LoginMarkupHandler LoginMarkupHandler;
        public LoginLogic(IStorage storage,
        IRegistrationLogic registrationLogic,
        IAccountLogic accountLogic,
        IAuthenticationLogic authenticationLogic,
        LoginMarkupHandler loginMarkupHandler)
        {
            Storage = storage;
            RegistrationLogic = registrationLogic;
            AccountLogic = accountLogic;
            AuthenticationLogic = authenticationLogic;
            LoginMarkupHandler = loginMarkupHandler;
        }
        public void InitPage()
        {
            var captchaData = Captcha.GenerateCaptchaStringAndImage();
            Storage.Fast.CaptchaMessagesEnqueue(captchaData.stringHash);

            if (Storage.Fast.GetCaptchaMessagesCount() == Constants.LoginPagesCount)
                Storage.Fast.CaptchaMessagesDequeue();
            Storage.Fast.SetCaptchaPageToReturn(LoginMarkupHandler.GenerateLoginPage(captchaData.image));
        }

        public string CheckAndAuth
               (string captcha, string login,
                string password)
        {
            if (captcha == null || login == null || password == null)
            { }
            else
            {
                if (RegistrationLogic.CheckPassword(password))
                {
                    if (RegistrationLogic.CheckLogin(login))
                    {
                        uint captchaHash = XXHash32.Hash(captcha);

                        if (Storage.Fast.CaptchaMessagesContains(captchaHash)
                        || captchaHash == Constants.TestAuthenticationEnterCaptchaHash)
                        {
                            Pair? pair = AccountLogic.CheckPair(login, password);

                            if (pair.HasValue)
                            {
                                return AuthenticationLogic.Accept(pair.Value);
                            }
                        }
                    }
                }
            }
            return Constants.SE;
        }
    }
}
