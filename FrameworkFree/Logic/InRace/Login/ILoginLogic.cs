using System.Net;
namespace Logic
{
    public interface ILoginLogic
    {
        void InitPageByTimer();
        string CheckAndAuth
        (in IPAddress ip, in string captcha, in string login, in string password);
    }
}