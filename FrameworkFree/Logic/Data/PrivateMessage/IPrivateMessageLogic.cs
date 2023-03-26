namespace Data
{
    public interface IPrivateMessageLogic
    {
        void LoadPersonalPages();
        string GetPersonalPage(int? id, int? page, Pair pair);
        void AddNewCompanionsIfNotExists
            (int ownerId, int companionId, string ownerNick, string companionNick);
    }
}