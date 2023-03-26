namespace Data
{
    public interface ISectionLogic
    {
        string GetSectionPage(int? Id, int? page);
        void LoadSectionPages();
    }
}