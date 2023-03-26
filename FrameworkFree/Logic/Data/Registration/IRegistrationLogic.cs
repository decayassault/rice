using System;
using System.Timers;
namespace Data
{
    public interface IRegistrationLogic
    {
        bool CheckNick(string nick);
        void RegisterInBaseByTimer();
        void PutRegInfoByTimer();
        void PreRegistration
            (string captcha, string login, string password, string email, string nick);
        void InitPage();
        bool CheckPassword(string password);
        bool CheckLogin(string login);
    }
}