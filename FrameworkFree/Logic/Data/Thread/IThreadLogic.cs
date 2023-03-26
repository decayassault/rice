namespace Data
{
    public interface IThreadLogic
    {
        string GetNick(int AccountId);
        void LoadThreadPages();
        string GetThreadPage(int? Id, int? page);
    }
}