
using System;
using System.Threading.Tasks;

namespace Forum.Data.Thread
{
    public class ThreadLogic
    {
        internal static string[][] ThreadPages;
        internal static int[] ThreadPagesPageDepth;
       
        internal static int ThreadPagesLength;

        internal const string SE = "";

        internal static string GetThreadPage(int Id, int page)
        {
            if (Id > MvcApplication.Zero && Id <= ThreadPagesLength)
            {
                int index = Id - MvcApplication.One;
                if (page > MvcApplication.Zero
                        && page <= ThreadPagesPageDepth[index])
                    return ThreadPages[index][page - MvcApplication.One];
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
            int threadsCount = await ThreadData.EncountThreads();
            ThreadLogic.ThreadPages = new string[threadsCount][];
            ThreadLogic.ThreadPagesLength = threadsCount;
            ThreadLogic.ThreadPagesPageDepth = new int[ThreadLogic.ThreadPagesLength];

            for (int i = MvcApplication.Zero; i < threadsCount; i++)
            {
                await ThreadData.AddThread(i);
            }
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            sw.Stop();
            TimeSpan t = sw.Elapsed;
        }

    }
}
