namespace Data
{
    using System.IO;
    using System;
    internal sealed class Logging
    {
        private static object locker = new object();
        internal static void LogEvent(string definition)
        {
            lock(locker)
          File.AppendAllLines("log_webtest.txt",new []{DateTime.Now+definition});
        }
    }
}
