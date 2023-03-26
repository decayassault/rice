using Forum.Controllers;
using Forum.Data.Thread;
using Forum.Data.Section;
using Forum.Data.EndPoint;
using System;
namespace Forum.Models.Tests
{
    internal sealed class ControllerTest
    {
        internal static void ThreadTest()
        {            
                        
            bool result = (ThreadData.GetThreadPage(0, 0)=="")?true:false;
            result = (ThreadData.GetThreadPage(null,null) == "") ? true : false;
            result = (ThreadData.GetThreadPage(99999, 999999999) == "") ? true : false;
            result = (ThreadData.GetThreadPage(1, 1) == "<div class='s'>0</div><div class='l'><h2 onClick='g(&quot;/section/2?page=1&quot;);'>Познакомлюсь с красоткой в Кемерово</h2><article><span onClick='g(&quot;/Profile/1&quot;);'>Хороший</span><br /><p>test 1</p></article><br /><article><span onClick='g(&quot;/Profile/1&quot;);'>Хороший</span><br /><p>test 1</p></article><br /><article><span onClick='g(&quot;/Profile/1&quot;);'>Хороший</span><br /><p>test 1</p></article><br /><div id='a'><a onClick='u();return false'>Ответить</a></div></div><div class='s'>2</div>") ? true : false;
        }
        internal static void SectionTest()
        {
            bool result = (SectionLogic.GetSectionPage(0, 0)=="")?true:false;
            result = (SectionLogic.GetSectionPage(null, null) == "") ? true : false;
            result = (SectionLogic.GetSectionPage(99999, 999999999) == "") ? true : false;
            string temp = SectionLogic.GetSectionPage(1, 1);
            result = (SectionLogic.GetSectionPage(1, 1) == "<div id='topic'><span><a onClick='newTopic();return false;'>Новая тема</a></span></div><nav class='n'><br /><p onClick='g(&quot;/thread/77?page=1&quot;);'>Ищем автомеханика, Таня и все</p><br /><br /><br /><p onClick='g(&quot;/thread/76?page=1&quot;);'>Весь в ожидании любви, Игнат 54 года</p><br /><br /><br /><p onClick='g(&quot;/thread/72?page=1&quot;);'>Нужен строитель дач, Рима 56 лет</p><br /><br /><br /><p onClick='g(&quot;/thread/71?page=1&quot;);'>Ищу шахматистку</p><br /><br /><br /><p onClick='g(&quot;/thread/68?page=1&quot;);'>Ищу партнера по рукопашке, Тагил</p><br /><br /><br /><p onClick='g(&quot;/thread/67?page=1&quot;);'>Ищу фанатов Спартака, Клава Псков 34 года</p><br /><br /><br /><p onClick='g(&quot;/thread/66?page=1&quot;);'>Познакомлюсь со спортсменкой</p><br /><br /><br /><p onClick='g(&quot;/thread/62?page=1&quot;);'>Познакомлюсь с рыболовом, Юля Новгород</p><br /><br /><br /><p onClick='g(&quot;/thread/61?page=1&quot;);'>Хозяйственный парень ищет девушку, Владивосток 31 год</p><br /><br /><span id='arrows'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/section/1?page=2&quot;);return false' title='Следующая страница'>►</a>&nbsp;&nbsp;&nbsp;&nbsp;<a onClick='g(&quot;/section/1?page=4&quot;);return false' title='Последняя страница'>»</a></span><br /></nav><div class='s'>1</div>") ? true : false;
        }
        internal static void EndPointTest()
        {
            bool result =(EndPointLogic.GetEndPointPage(0)=="")?true:false;
            result = (EndPointLogic.GetEndPointPage(null) == "") ? true : false;
            result = (EndPointLogic.GetEndPointPage(99999) == "") ? true : false;
            string temp = EndPointLogic.GetEndPointPage(1);
            result = (EndPointLogic.GetEndPointPage(1) == "<p onClick='g(&quot;/section/1?page=1&quot;);'>Знакомства в Москве</p><br /><p onClick='g(&quot;/section/2?page=1&quot;);'>Религиозные знакомства</p><br /><p onClick='g(&quot;/section/3?page=1&quot;);'>Знакомства для отношений</p><br /><p onClick='g(&quot;/section/4?page=1&quot;);'>Знакомства в Европе</p><br /><p onClick='g(&quot;/section/5?page=1&quot;);'>Прочие знакомства</p><br />") ? true : false;
        }
    }
}