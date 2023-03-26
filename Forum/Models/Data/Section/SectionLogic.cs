
using System;
using System.Threading.Tasks;

namespace Forum.Data.Section
{
    public class SectionLogic
    {
        private static string[][] SectionPages;
        private static object SectionPagesLock = new object();
        private static int SectionPagesLength;
        private static object SectionPagesLengthLock = new object();
        private static int[] SectionPagesPageDepth;
        private static object SectionPagesPageDepthLock = new object();
        private const string SE = "";    
        private const int len = 25;

        internal static string GetSectionPage(int Id, int page)
        {
            if (Id > MvcApplication.Zero && Id <= GetSectionPagesLengthLocked())
            {
                int index = Id - MvcApplication.One;
                if (page > MvcApplication.Zero 
                    && page <= GetSectionPagesPageDepthLocked(index))
                    return GetSectionPagesPageLocked
                        (index,page - MvcApplication.One);
                else return SE;
           }
            else
                return SE;
        }

        internal async static Task LoadSectionPages() //3 sec
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            SectionLogic.SetSectionPagesLengthLocked(len);
            SectionLogic.SectionPagesPageDepth = 
                new int[SectionLogic.GetSectionPagesLengthLocked()];
            SectionLogic.InitializeSectionPagesLocked
                (new string[SectionLogic.GetSectionPagesLengthLocked()][]);

            for (int i = MvcApplication.Zero; 
                i < SectionLogic.GetSectionPagesLengthLocked(); i++)
            {
                await SectionData.AddSection(i);                
            }

            sw.Stop();
            TimeSpan t = sw.Elapsed;
        }
        internal static string[] GetSectionPagesArrayLocked(int index)
        {
            lock (SectionPagesLock)
                return SectionPages[index];
        }
        internal static void SetSectionPagesArrayLocked(int index,string[] value)
        {
            lock (SectionPagesLock)
                SectionPages[index] = value;
        }
        internal static string GetSectionPagesPageLocked
                        (int arrayIndex,int pageIndex)
        {
            lock (SectionPagesLock)
                return SectionPages[arrayIndex][pageIndex];
        }
        internal static void SetSectionPagesPageLocked
            (int arrayIndex,int pageIndex,string value)
        {
            lock (SectionPagesLock)
                SectionPages[arrayIndex][pageIndex] = value;
        }
        internal static void AddToSectionPagesPageLocked
            (int arrayIndex, int pageIndex,string value)
        {
            lock (SectionPagesLock)
                SectionPages[arrayIndex][pageIndex] += value;
        }
        internal static void InitializeSectionPagesLocked(string[][] value)
        {
            lock (SectionPagesLock)
                SectionPages = value;
        }
        internal static int GetSectionPagesLengthLocked()
        {
            lock (SectionPagesLengthLock)
                return SectionPagesLength;
        }
        internal static void SetSectionPagesLengthLocked(int value)
        {
            lock(SectionPagesLengthLock)
                SectionPagesLength = value;
        }
        internal static void SetSectionPagesPageDepthLocked(int index, int value)
        {
            lock (SectionPagesPageDepthLock)
                SectionPagesPageDepth[index] = value;
        }
        internal static int GetSectionPagesPageDepthLocked(int index)
        {
            lock (SectionPagesPageDepthLock)
                return SectionPagesPageDepth[index];
        }
        internal static void 
            InitializeSectionPagesPageDepthLocked(int[] value)
        {
            lock (SectionPagesPageDepthLock)
                SectionPagesPageDepth = value;
        }
    }
}
