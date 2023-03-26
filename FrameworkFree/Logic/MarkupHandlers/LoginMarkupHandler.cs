namespace MarkupHandlers
{
    internal sealed class LoginMarkupHandler
    {
        internal string GenerateLoginPage(string captchaImageCode)
        {
            return string.Concat("<div id='g'>",
                captchaImageCode,
                "<br /><a>Текст на картинке</a><br><input id='y' type='text'><br />",
                "<a>Логин</a><br /><input id='l' type='text'><br />",
                "<a>Пароль</a><br /><input id='q'",
                " type='password'><br /><div id='c'></div><br />",
                "<a onClick='d();return false'>Войти</a></div>");
        }
    }
}