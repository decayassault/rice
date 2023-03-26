namespace Data
{
    public interface IPrivateDialogLogic
    {
        void LoadDialogPages();
        string GetDialogPage(int? page, Pair pair);
    }
}