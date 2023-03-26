using System;
using System.Timers;
namespace Data
{
    public interface INewTopicLogic
    {
        bool CheckText(string message);
        int CountStringOccurrences
            (string text, string pattern);
        void Start(string threadName, int? endpointId, Pair pair, string message);
        void StartNextTopicByTimer();
    }
}