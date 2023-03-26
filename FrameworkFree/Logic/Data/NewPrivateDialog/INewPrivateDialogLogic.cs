using System;
using System.Timers;
namespace Data
{
    public interface INewPrivateDialogLogic
    {
        void StartNextDialogByTimer();
        void Start(string acceptorNick, Pair pair, string message);
    }
}