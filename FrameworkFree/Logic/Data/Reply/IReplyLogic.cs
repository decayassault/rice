using System;
using System.Timers;
namespace Data
{
    public interface IReplyLogic
    {
        int? GetAccountId(Pair pair);
        void Start(int? id, Pair pair, string t);
        void PublishNextMessageByTimer();
    }
}