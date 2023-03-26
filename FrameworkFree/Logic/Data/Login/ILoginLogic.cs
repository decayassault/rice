namespace Data
{
    public interface ILoginLogic
    {
       void InitPage();
       string CheckAndAuth(string captcha, string login, string password);
    }
}