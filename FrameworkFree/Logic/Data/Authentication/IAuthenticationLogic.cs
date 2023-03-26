namespace Data
{
    public interface IAuthenticationLogic
    {
        bool AccessGranted(string token);
        Pair GetPair(string token);
        string Accept(Pair pair);
        void Logout(string token);
    }
}