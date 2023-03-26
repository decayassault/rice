using XXHash;
using MarkupHandlers;
using System.Net;
namespace Logic
{
    internal sealed class LoginLogic : ILoginLogic
    {
        private readonly IStorage Storage;
        private readonly IRegistrationLogic RegistrationLogic;
        private readonly IAccountLogic AccountLogic;
        private readonly IAuthenticationLogic AuthenticationLogic;
        private readonly IInRace InRace;
        private readonly Captcha Captcha;
        private readonly LoginMarkupHandler LoginMarkupHandler;

        public LoginLogic(IStorage storage,
        IRegistrationLogic registrationLogic,
        IAccountLogic accountLogic,
        IAuthenticationLogic authenticationLogic,
        IInRace inRace,
        Captcha captcha,
        LoginMarkupHandler loginMarkupHandler)
        {
            Storage = storage;
            RegistrationLogic = registrationLogic;
            AccountLogic = accountLogic;
            AuthenticationLogic = authenticationLogic;
            InRace = inRace;
            Captcha = captcha;
            LoginMarkupHandler = loginMarkupHandler;
        }
        public void InitPageByTimer()
        {
            var captchaData = Captcha.GenerateCaptchaStringAndImage();
            Storage.Fast.CaptchaMessagesEnqueue(captchaData.stringHash);

            if (Storage.Fast.GetCaptchaMessagesCount() == Constants.LoginPagesCount)
                Storage.Fast.CaptchaMessagesDequeue();
            Storage.Fast.SetCaptchaPageToReturn(LoginMarkupHandler.GenerateLoginPage(captchaData.image));
        }

        public string CheckAndAuth
               (in IPAddress ip, in string captcha, in string login,
            in string password)
        {
            if (ip == null || captcha == null || login == null || password == null)
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
                            Pair? pair = InRace.Unstable.Account.CheckPair(login, password);

                            if (pair.HasValue)
                            {
                                return InRace.Unstable.Authentication.Accept(ip, pair.Value);
                            }
                        }
                    }
                }
            }
            return Constants.SE;
        }
    }
}
