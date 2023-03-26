using Forum.Data.Account;
using Forum.Models;
namespace Forum.Data.NewPrivateDialog
{
    using System;
    using System.Threading.Tasks;
    internal sealed class NewPrivateDialogData
    {
        internal const string PageToReturn = "<p>Получатель сообщения:</p>" +
                "<input type='text' tabindex='0' autofocus required maxlength='25' autocomplete='off' />" +
                "<p>Текст сообщения:</p>" +
                "<textarea id='text' tabindex='1' maxlength='1000' wrap='soft' spellcheck='true'></textarea>" +
                "<div id='msg'></div><br /><span><a id='send' onClick='startDialog();return false'>Отправить</a></span>";
        internal static bool CheckNickIfExists(string nick)
        {
            bool result = false;
            if (AccountData.CheckNickHashIfExists(nick)
                && CheckNickInBase(nick))
                result = true;
            return result;
        }
        private static bool CheckNickInBase(string nick)
        {
            bool result=false;
             using(var SqlCon = Connection.GetConnectionNoAsync())
            {
                using (var cmdNick =
                        Command.InitializeCommandForInputNick
                            (@"CheckNickIfExists", SqlCon,nick))
                {
                    object o;
                    o = cmdNick.ExecuteScalar();
                    if (o == DBNull.Value || o == null)
                        result = false;
                    else result = true;
                }
            }

            return result;
        }
        internal async static Task<int> GetIdByNick(string nick)
        {
            int result = 1;
            using (var SqlCon = await Connection.GetConnection())
            {
                using (var cmdNick =
                        Command.InitializeCommandForInputNick
                            (@"CheckNickIfExists", SqlCon, nick))
                {
                    object o;
                    o = await cmdNick.ExecuteScalarAsync();
                    if (o == DBNull.Value || o == null)
                        result = MvcApplication.One;
                    else result = Convert.ToInt32(o);
                }
            }

            return result;
        }
    }
}