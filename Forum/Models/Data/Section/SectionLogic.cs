
using System;
using System.Threading.Tasks;

namespace Forum.Data.Section
{
    public class SectionLogic
    {
        internal static string[][] SectionPages;

        internal static int SectionPagesLength;
        internal static int[] SectionPagesPageDepth;

        private const string SE = "";        

        private const int len = 25;

        internal static string GetSectionPage(int Id, int page)
        {
            if (Id > MvcApplication.Zero && Id <= SectionPagesLength)
            {
                int index = Id - MvcApplication.One;
                if (page > MvcApplication.Zero && page <= SectionPagesPageDepth[index])
                    return SectionPages[index][page - MvcApplication.One];
                else return SE;
           }
            else
                return SE;
        }

        internal async static Task LoadSectionPages() //3 sec
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            SectionLogic.SectionPagesLength = len;
            SectionLogic.SectionPagesPageDepth = 
                new int[SectionLogic.SectionPagesLength];
            SectionLogic.SectionPages = 
                new string[SectionLogic.SectionPagesLength][];

            for (int i = MvcApplication.Zero; 
                i < SectionLogic.SectionPagesLength; i++)
            {
                await SectionData.AddSection(i);                
            }

            sw.Stop();
            TimeSpan t = sw.Elapsed;
        }
        
    }
}
