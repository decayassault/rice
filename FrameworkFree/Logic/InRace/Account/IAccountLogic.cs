namespace Logic
{
    public interface IAccountLogic
    {
        Pair? CheckPair(in string login, in string password);
    }
}