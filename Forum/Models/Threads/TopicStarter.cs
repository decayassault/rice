namespace Forum.Models.Threads
{
    using Forum.Data;
    using System.Threading;
    internal sealed class TopicStarter
    {
        private static Thread NewTopicStarter;
        private static object NewTopicStarterLock = new object();
        internal static void Start()
        {
            InitializeThread();
            SetPriority();
            StartThread();
        }  
        private static void InitializeThread()
        {
            lock (NewTopicStarterLock)
                NewTopicStarter = new Thread(NewTopicData.StartNextTopic);
        }
        private static void SetPriority()
        {
            lock (NewTopicStarterLock)
                NewTopicStarter.Priority = ThreadPriority.Lowest;
        }
        private static void StartThread()
        {
            lock (NewTopicStarterLock)
                NewTopicStarter.Start();
        }
    }
}