using System;
using System.Timers;
namespace Data
{
    public interface INewTopicLogic
    {
        void Start(string threadName, int? endpointId, Pair pair, string message);
        void StartNextTopicByTimer();
    }
}