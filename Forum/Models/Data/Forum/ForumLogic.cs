using System;
using System.Threading.Tasks;

namespace Forum.Data.Forum
{
    internal sealed class ForumLogic
    {
        internal static string MainPage;
        internal static string MainContent;

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
            MainPage = "<!DOCTYPE html><html lang='ru' dir='ltr' spellcheck='false'><head><meta charset='utf-8'>" +
                "<meta name='description' content='Форум знакомств Upsense.ru' />" +
                "<meta name='keywords' content='форум знакомств, знакомства онлайн, хочу познакомиться, русскоязычный сайт знакомств, бесплатные знакомства, upsense' />" +
                "<link rel='stylesheet' type='text/css' href='c.css'>" +
                "<meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no' />" +
                "<base target='_blank' />" +
                "<title>Форум знакомств Upsense.ru</title></head><body><header><h1 onClick='p();'>Форум знакомств</h1></header><div id='content'>";

            MainContent = "<nav class='o'>" 
                + await ForumData.LoadForumsOnMain() + "</nav>";
            MainPage += MainContent;

            MainPage += "</div><small><a id='m' href='mailto:support@upsense.ru'>Обратная связь</a></small>" +
                "<script src='j.js' async></script></body></html>";

            
        }
    }
}
