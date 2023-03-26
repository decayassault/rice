namespace Forum.Models.Threads
{
    using System.Threading;
    using Forum.Data;
    internal sealed class Messenger
    {
        private static Thread MessagePublisher;
        private static object MessagePublisherLock = new object();
        internal static void Start()
        {
            InitializeThread();
            SetPriority();
            StartThread();
        }
        private static void InitializeThread()
        {
            lock (MessagePublisherLock)
                MessagePublisher = new Thread(ReplyData.PublishNextMessage);
        }
        private static void SetPriority()
        {
            lock (MessagePublisherLock)
                MessagePublisher.Priority = ThreadPriority.Lowest;
        }
        private static void StartThread()
        {
            lock (MessagePublisherLock)
                MessagePublisher.Start();
        }
    }
}