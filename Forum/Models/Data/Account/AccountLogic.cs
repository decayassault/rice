using System.Diagnostics;
using System.Threading.Tasks;

namespace Forum.Data.Account
{
    internal sealed class AccountLogic
    {
        internal const string LoginRequirement =
             "<span><a onClick='j();return false'>Войдите</a>&nbsp;или&nbsp;" +
             "<a onClick='h();return false'>Зарегистрируйтесь</a></span>";

        internal async static Task LoadAccounts() //5 sec
        {
            var sw = new Stopwatch();
            sw.Start();

            await AccountData.LoadAccounts();           
            await AccountData.LoadNicks();

            sw.Stop();
            System.TimeSpan temp = sw.Elapsed;
        }
    }
}
