using System.Timers;
using System;
namespace Data
{
    public interface IAccountLogic
    {
        void CheckAccountIdByTimer();
        void LoadAccounts();
        void LoadNicks();
        bool CheckNickHashIfExists(string nick);
        int? GetAccIdAndStore(Pair pair);
        Pair? CheckPair(string login, string password);
    }
}