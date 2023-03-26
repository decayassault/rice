using System;
using System.Timers;
namespace Data
{
    public interface INewPrivateMessageLogic
    {
        void Start(int? id, Pair pair, string t);
        void PublishNextPrivateMessageByTimer();
    }
}