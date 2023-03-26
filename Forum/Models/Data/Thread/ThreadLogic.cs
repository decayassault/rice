
using System;
using System.Threading.Tasks;

namespace Forum.Data.Thread
{
    public class ThreadLogic
    {
        private static string[][] ThreadPages;
        private static object ThreadPagesLock = new object();
        private static int[] ThreadPagesPageDepth;
        private static object ThreadPagesPageDepthLock = new object();       
        private static int ThreadPagesLength;
        private static object ThreadPagesLengthLock = new object();

        internal const string SE = "";

        internal static string GetThreadPage(int Id, int page)
        {
            if (Id > MvcApplication.Zero 
                && Id <= GetThreadPagesLengthLocked())
            {
                int index = Id - MvcApplication.One;
                if (page > MvcApplication.Zero
                        && page <= GetThreadPagesPageDepthLocked(index))
                    return GetThreadPagesPageLocked
                            (index,page - MvcApplication.One);
                else return SE;
            }
            else
                return SE;
        }

        internal async static Task LoadThreadPages() //1 min 37 sec
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

           await LoadEachThreadPage();

            sw.Stop();
            TimeSpan t = sw.Elapsed;
        }
        internal async static Task LoadEachThreadPage()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            int threadsCount = await ThreadData.EncountThreads();
            sw.Stop();
            TimeSpan t = sw.Elapsed;
            ThreadLogic.InitializeThreadPagesLocked
                (new string[threadsCount][]);
            ThreadLogic.SetThreadPagesLengthLocked(threadsCount);
            ThreadLogic.InitializeThreadPagesPageDepthLocked
                    (new int[ThreadLogic.GetThreadPagesLengthLocked()]);

            for (int i = MvcApplication.Zero; i < threadsCount; i++)
            {
                await ThreadData.AddThread(i);
            }     
        }
        internal static void SetThreadPagesPageLocked
            (int arrayIndex,int pageIndex,string value)
        {
            lock (ThreadPagesLock)
                ThreadPages[arrayIndex][pageIndex] = value;
        }
        internal static string GetThreadPagesPageLocked
            (int arrayIndex,int pageIndex)
        {
            lock (ThreadPagesLock)
                return ThreadPages[arrayIndex][pageIndex];
        }
        internal static string[] GetThreadPagesArrayLocked(int arrayIndex)
        {
            lock (ThreadPagesLock)
                return ThreadPages[arrayIndex];
        }
        internal static void SetThreadPagesArrayLocked
                (int arrayIndex,string[] value)
        {
            lock (ThreadPagesLock)
                ThreadPages[arrayIndex] = value;
        }

        internal static void AddToThreadPagesPageLocked
                (int arrayIndex,int pageIndex,string value)
        {
            lock (ThreadPagesLock)
                ThreadPages[arrayIndex][pageIndex] += value;
        }        
        internal static void InitializeThreadPagesLocked(string[][] value)
        {
            lock (ThreadPagesLock)
                ThreadPages = value;
        }
        internal static string[][] GetThreadPagesLocked()
        {
            lock (ThreadPagesLock)
                return ThreadPages;
        }        
        internal static int GetThreadPagesPageDepthLocked(int index)
        {
            lock (ThreadPagesPageDepthLock)
                return ThreadPagesPageDepth[index];
        }
        internal static void AddToThreadPagesPageDepthLocked(int index, int value)
        {
            lock (ThreadPagesPageDepthLock)
                ThreadPagesPageDepth[index] += value;
        }
        internal static void SetThreadPagesPageDepthLocked(int index,int value)
        {
            lock (ThreadPagesPageDepthLock)
                ThreadPagesPageDepth[index] = value;
        }
        internal static void InitializeThreadPagesPageDepthLocked(int[] value)
        {
            lock (ThreadPagesPageDepthLock)
                ThreadPagesPageDepth = value;
        }
        internal static int[] GetThreadPagesPageDepthLocked()
        {
            lock (ThreadPagesPageDepthLock)
                return ThreadPagesPageDepth;
        }
        internal static int GetThreadPagesLengthLocked()
        {
            lock (ThreadPagesLengthLock)
                return ThreadPagesLength;
        }
        internal static void SetThreadPagesLengthLocked(int value)
        {
            lock (ThreadPagesLengthLock)
                ThreadPagesLength = value;
        }
    }
}
