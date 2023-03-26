using System;
using System.Threading.Tasks;

namespace Forum.Data.Forum
{
    internal sealed class ForumLogic
    {
        private static string MainPage;
        private static string MainContent;
        private static object MainContentLock = new object();
        private static object MainPageLock = new object();

        internal async static Task LoadMainPage()//1 sec
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            
            await LoadMainPageCode();

            sw.Stop();
            TimeSpan t = sw.Elapsed;
        }


        private async static Task LoadMainPageCode()
        {
            SetMainPageLocked("<!DOCTYPE html><html lang='ru' dir='ltr' spellcheck='false'><head><meta charset='utf-8'>" +
                "<meta name='description' content='Форум знакомств Upsense.ru' />" +
                "<meta name='keywords' content='форум знакомств, знакомства онлайн, хочу познакомиться, русскоязычный сайт знакомств, бесплатные знакомства, upsense' />" +
                "<link rel='stylesheet' type='text/css' href='c.css'>" +
                "<meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no' />" +
                "<base target='_blank' />" +
                "<title>Форум знакомств Upsense.ru</title></head><body><header><h1 onClick='p();'>Форум знакомств</h1></header><div id='content'>");

            SetMainContentLocked("<nav class='o'>" 
                + await ForumData.LoadForumsOnMain() + "</nav>");
            AddToMainPageLocked(GetMainContentLocked());

            AddToMainPageLocked("</div><small><a id='m' href='mailto:support@upsense.ru'>Обратная связь</a></small>" +
                "<script src='j.js' async></script></body></html>");            
        }
        internal static string GetMainContentLocked()
        {
            lock (MainContentLock)
                return MainContent;
        }
        internal static void SetMainContentLocked(string value)
        {
            lock (MainContentLock)
                MainContent = value;
        }
        internal static string GetMainPageLocked()
        {
            lock (MainPageLock)
                return MainPage;
        }
        internal static void SetMainPageLocked(string value)
        {
            lock (MainPageLock)
                MainPage = value;
        }
        internal static void AddToMainPageLocked(string value)
        {
            lock (MainPageLock)
                MainPage += value;
        }
    }
}
