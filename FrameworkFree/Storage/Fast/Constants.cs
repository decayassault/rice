using System.Collections.Generic;
namespace Data
{
    public static class Constants
    {
        public const string LoginRequirement =
              "<span id='n'><a onClick='j();return false'>Войдите</a>&nbsp;или&nbsp;" +
              "<a onClick='h();return false'>Зарегистрируйтесь</a></span>";
        public const string SE = "";
        public const byte EndPointPagesCount = 5;
        public const string PageToReturn = "<p>Получатель сообщения:</p>" +
                 "<input type='text' tabindex='0' autofocus required maxlength='25' autocomplete='off' />" +
                 "<p>Текст сообщения:</p>" +
                 "<textarea id='x' tabindex='1' maxlength='1000' wrap='soft' spellcheck='true'></textarea>" +
                 "<div id='c'></div><br /><span><a id='s' onClick='startDialog();return false'>Отправить</a></span>";
        public const string pMarker = "<p";
        public const string fullSpanMarker = "<span id='w'>";
        public const string endSpanMarker = "</span>";
        public const string brMarker = "<br />";
        public const string endNavMarker = "</nav>";
        public const string h2End = "</h2>";
        public const string pStart = "<p onclick='n(&quot;/p/";
        public const string pMiddle = "?p=1&quot;);'>";
        public const string pEnd = "</p>";
        public const string a = "<div class='s'>0</div>";
        public const string indic = "<div class='s'>";
        public const string articleStart = "<article>";
        public const string articleEnd = "</article>";
        public const string answerMarker = "<div id='a'>";
        public const string PrivateReplyPage = "<p>Ваш ответ:</p>" +
                "<textarea id='x' autofocus maxlength='1000' wrap='soft' spellcheck='true'></textarea>" +
                "<div id='c'></div><br /><span><a id='s' onClick='sendPrivateReply();return false'>Отправить</a></span>";

        public const string indicEnd = "</div>";
        public const string spanIndicator = "<span id='w'>";
        public const string spanEnd = "</span>";
        public const int DialogsOnPage = 9;
        public const int five = 5;
        public const int threadsOnPage = 9;
        public const int len = 25;
        public const int LoginPagesCount = 3000;
        public const uint TestRegistrationEnterCaptchaHash = 315702632; //"3ЁЗЙ"
        public const uint TestAuthenticationEnterCaptchaHash = 1916808717; //"ЫЪЬЩ"
        public const uint TestsInterfaceAccessKeyHash = 331014098; //"900d9537-af23-477c-91fe-dcae78edfd5d"
        public static List<char> AlphabetRusLower =
                             new List<char> { 'а', 'б', 'в', 'г', 'д', 'е', 'ё',
                                'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п',
                                'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ',
                                'ъ', 'ы', 'ь', 'э', 'ю', 'я' };

        public static List<char> Special = new List<char>{
            '.', ',', '-', ' ', '!', '?', ';', ':', '"', '(', ')'
        };

        public const string PageToReturnTopic = "<p>Заголовок темы:</p>" +
                "<input type='text' tabindex='0' autofocus required maxlength='99' autocomplete='off' />" +
                "<p>Текст сообщения:</p>" +
                "<textarea id='x' tabindex='1' maxlength='1000' wrap='soft' spellcheck='true'></textarea>" +
                "<div id='c'></div><br /><span><a id='s' onClick='startTopic();return false'>Отправить</a></span>";
        public const int RegistrationPagesCount = 120;
        public const string MainPage = "<!DOCTYPE html><html lang='ru' dir='ltr' spellcheck='false'><head><meta content='text/html;charset=utf-8' http-equiv='Content-Type'><meta charset='utf-8' http-equiv='encoding'>" +
                "<meta name='description' content='Первый мобильный форум бесплатных онлайн-знакомств на русском языке с экономией трафика поможет вам построить хорошие отношения с партнерами для любых целей посредством добрых бесед; форум любви - это место, где встречаются люди.' />" +
                "<meta name='keywords' content='знакомства онлайн, хочу познакомиться, бесплатные знакомства, знакомства для серьезных отношений, поиск друзей, девушка знакомство, форум знакомств, форум знакомств с иностранцами, форум любви, сайт знакомств' />" +
                "<link rel='stylesheet' type='text/css' href='c.css'>" +
                "<meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no' />" +
                "<base target='_blank' />" +
                "<title>Форум любви</title></head><body><header><h1 onClick='p();' title='На главную'>Форум любви</h1>"
                //+ "<div id='back' title='Назад' onClick='back();'>&#8617;</div>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" 
                + "<div id='p' title='Личные сообщения' onClick='openDialogsList();'>&#9993;</div>"
                //+"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                //+"<div>&#9786;</div>&#9829;" 
                //+"<div id='forward' title='Вперёд' onClick='forward();'>&#8618;</div>"
                + "</header><div id='o'>";
        public const string MainPageEnd = "</div><small><a id='m' href='mailto:'>Обратная связь</a></small>" +
                "<script src='j.js' async></script></body></html>";
        public const string NewDialog = "<div id='d'><span><a onclick=" +
                "'newDialog();return false;'>Новый диалог</a>" +
                        "</span></div><nav class='n'></nav></div>";
        public const string navMarker = "<nav class='n'>";
        public const string spanMarker = "<span";
        public const string endLinkMarker = "»";
        public const string pageMarker = "?p=";
        public const string nextLinkMarker = "►";
        public const string ReplyPage = "<p>Ваш ответ:</p>" +
            "<textarea id='x' autofocus maxlength='1000' wrap='soft' spellcheck='true'></textarea>" +
            "<div id='c'></div><br /><span><a id='s' onClick='s();return false'>Отправить</a></span>";
        public const string buttonTxt
                = "<div id='t'><span><a onClick='newTopic();return false;'>Новая тема</a></span></div>";
        public const char TagStartSymbol = '<';
    }
}