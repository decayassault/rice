using System.Collections.Generic;
namespace Own.Permanent
{
    internal static class Constants
    {
        public const string GooglePasswordCharactersString = " ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz!@\"#$;%^:&?*()-_=+[{]}|\\/'`~,.<>?+";
        public const byte GooglePasswordMaxLength = 100;
        public const string LoginRequirement =
              "<span id='n'><a onClick='j();return false'>Войти</a>&nbsp;или&nbsp;" +
              "<a onClick='h();return false'>Зарегистрироваться</a></span>";
        public const string SE = "";
        public const byte EndPointPagesCount = 5;
        public const byte Zero = 0;
        public const byte One = 1;
        public const string PageToReturn = "<p>Получатель сообщения:</p>" +
                 "<input type='text' tabindex='0' autofocus required maxlength='25' autocomplete='off' />" +
                 "<p>Текст сообщения:</p>" +
                 "<textarea id='x' tabindex='1' maxlength='1000' wrap='soft' spellcheck='true'></textarea>" +
                 "<div id='c'></div><br /><span><a id='s' onClick='i();return false'>Отправить</a></span>";
        public const string pMarker = "<p";
        public const string fullSpanMarker = "<span id='w'>";
        public const string endSpanMarker = "</span>";
        public const string brMarker = "<br />";
        public const string endNavMarker = "</nav>";
        public const byte Fifty = 50;
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
                "<div id='c'></div><br /><span><a id='s' onClick='e();return false'>Отправить</a></span>";
        public const string indicEnd = "</div>";
        public const string spanEnd = "</span>";
        public const byte DialogsOnPage = 9;
        public const byte five = 5;
        public const byte threadsOnPage = 9;
        public const byte len = 25;
        public const short LoginPagesCount = 3000;
#if DEBUG
        public const uint TestRegistrationEnterCaptchaHash = 315702632; //"3ЁЗЙ"
        public const uint TestAuthenticationEnterCaptchaHash = 1916808717; //"ЫЪЬЩ" 
#endif
        internal static List<char> AlphabetRusLower =
                             new List<char> { 'а', 'б', 'в', 'г', 'д', 'е', 'ё',
                                'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п',
                                'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ',
                                'ъ', 'ы', 'ь', 'э', 'ю', 'я' };

        internal static char[] Special = new char[12]{
            '.', ',', '-', ' ', '!', '?', ';', ':', '"', '(', ')', '\0'
        };

        public const string PageToReturnTopic = "<p>Заголовок темы:</p>" +
                "<input type='text' tabindex='0' autofocus required maxlength='99' autocomplete='off' />" +
                "<p>Текст сообщения:</p>" +
                "<textarea id='x' tabindex='1' maxlength='1000' wrap='soft' spellcheck='true'></textarea>" +
                "<div id='c'></div><br /><span><a id='s' onClick='l();return false'>Отправить</a></span>";
        public const int RegistrationPagesCount = 120;
        public const string MainPage = "<!DOCTYPE html><html lang='ru' dir='ltr' spellcheck='false'><head><script type='text/javascript' > (function(m,e,t,r,i,k,a){m[i]=m[i]||function(){(m[i].a=m[i].a||[]).push(arguments)}; m[i].l=1*new Date();k=e.createElement(t),a=e.getElementsByTagName(t)[0],k.async=1,k.src=r,a.parentNode.insertBefore(k,a)}) (window, document, 'script', 'https://mc.yandex.ru/metrika/tag.js', 'ym'); ym(88054100, 'init',{clickmap:true, trackLinks:true, accurateTrackBounce:true, webvisor:true});</script><noscript><div><img src='https://mc.yandex.ru/watch/88054100' style='position:absolute; left:-9999px;' alt=''/></div></noscript><meta content='text/html;charset=utf-8' http-equiv='Content-Type'><meta charset='utf-8' http-equiv='encoding'>" +
                "<meta name='description' content='Первый мобильный форум бесплатных онлайн-знакомств на русском языке с экономией трафика поможет Вам построить хорошие отношения с партнерами для любых целей посредством добрых бесед; форум любви - это место, где встречаются люди.' />" +
                "<meta name='keywords' content='честные знакомства, порядочные знакомства, бесплатные знакомства, брачные знакомства, поиск друзей, форум любви, форум знакомств, дружеское общение, как познакомиться' />" +
                "<link rel='stylesheet' type='text/css' href='m.css'><link rel='icon' href='favicon.png' type='image/png'><link rel='stylesheet' type='text/css' href='a.css'><link rel='stylesheet' type='text/css' href='b.css' disabled='true'>" +
                "<meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no' />" +
                "<base target='_blank' />" +
                "<title>Форум любви</title></head><body><header><h1 onClick='z();' title='На главную'>Форум любви</h1>" +
                "<div class='p s' id='ba' title='Назад' onClick='B();'>&lArr;</div>"
                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + "<div class='p' title='Личные сообщения' onClick='b();'>&#128140;</div>"
                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + "<div class='p' title='Анкета' onClick='p();'>&#128521;</div>"
                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                + "<div class='p' title='Масштаб' onClick='y();'>&#128269;</div>"
                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                /*+ "<div class='p' title='' onClick='L();'>&#9829;</div>"
                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"*/
                + "<div class='p s' id='fo' title='Вперёд' onClick='F();'>&rArr;</div>"
                + "</header><div id='o'>";
        public const string MainPageEnd = "</div><small><a id='m' href='mailto:mail@алексейлот.рф'>Обратная связь</a></small>" +
                "<script src='m.js' async></script></body></html>";
        public const string NewDialog = "<div id='d'><span><a onclick=" +
                "'k();return false;'>Новый диалог</a>" +
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
                = "<div id='t'><span><a onClick='q();return false;'>Новая тема</a></span></div>";
        public const char TagStartSymbol = '<';
        public const short MaxReplyMessageTextLength = 1000;
        public const byte MaxForumThreadNameTextLength = 99;
        public const byte MaxNickTextLength = 25;
        public const short MaxFirstLineLength = 10000;
        public const int MaxProfileImageSizeBytes = 500 * 1024;
        public const byte MaxAttemptsCountPerIp = byte.MaxValue;
        public const byte ProfileQuestionsCount = 25;
        public const byte CaptchaLength = 4;
        public const short MinProfileImageSizeBytes = 1024;
        public const short ProfileImageHeightPixels = 809;
        public const short ProfileImageWidthPixels = 500;
        public const string Checked = " checked";
        public const string Yes = " Да.";
        public const string No = " Нет.";
        public const string CaptchaLetters = "0123456789"; //"12456789АБВГДЕЖИКЛМНПРСТУФХЦЧШЭЮЯ";
        public const ushort TimerPeriodMilliseconds = 1000;
        public const byte CaptchaJsonQueueLength = byte.MaxValue;
        public const string About = @"Форум любви - бесплатный простой и быстрый форум для эксплуатации с регулируемой нагрузкой и низкой себестоимостью.

Версия 1.5.0.0 для asp.net core mvc.

Код последней версии опубликован на дешёвом российском shared-хостинге.

Определение номера версии:
Номер состоит из четырёх неотрицательных целых чисел, разделённых точкой, и соответствует шаблону A.B.C.D, где A обозначает номер улучшения или исправления безопасности, B - номер улучшения или исправления функционала, С - номер улучшения или исправления производительности, D - номер прочих улучшений и исправлений(D = 0 - релиз, иначе - beta-версия), причём увеличение одного из чисел старших разрядов номера версии обнуляет все младшие разряды.К примеру, если обновляется функционал и добавляются прочие исправления, но не изменяется производительность, то инкрементируется номер обновления функционала, как более важный параметр.


https://форумлюбви.рф/u - капча (captcha) с кодом от 1000 до 9999 в виде plain text json с картинкой в виде разметки HTML.
https://форумлюбви.рф/v - генератор максимально безопасного пароля для аккаунта Google (Gmail и т.п.).
https://форумлюбви.рф/w - эта страница с информацией о продукте.

Функциональность:
аутентификация по капче, логину и паролю,
переходы по истории просмотра назад и вперёд,
изменение масштаба элементов страницы,
раскрытие списка разделов,
создание новой темы,
создание нового сообщения темы,
создание нового личного сообщения,
просмотр списка тем,
листание списка тем в конец,
листание списка тем в начало,
листание списка тем к следующей порции,
листание списка тем к предыдущей порции,
просмотр списка сообщений темы,
листание списка сообщений темы в конец,
листание списка сообщений темы в начало,
листание списка сообщений темы к следующей порции,
листание списка сообщений темы к предыдущей порции,
возврат на главную страницу,
отправка обратной связи,
просмотр списка личных сообщений,
листание списка личных сообщений в конец,
листание списка личных сообщений в начало,
листание списка личных сообщений к следующей порции,
листание списка личных сообщений к предыдущей порции,
регистрация по капче, логину, паролю, повтору пароля, секретному слову, повтору секретного слова, имени на форуме,
занесение данных в анкету профиля,
редактирование анкеты профиля,
масштабирование интерфейса,
безлимитное API капчи,
генератор пароля для аккаунта Google.

Движок форума на данный момент находится в тестировании.";
    }
}