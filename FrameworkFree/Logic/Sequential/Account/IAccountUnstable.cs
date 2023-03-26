using Logic;
namespace Logic
{
    public interface IAccountUnstable
    {
        public void LoadAccounts();
        public void LoadNicks();
        public void CheckAccountIdByTimer();
        public int? GetAccIdAndStore(Pair pair);
        public bool CheckNickHashIfExists(in string nick);
    }
}