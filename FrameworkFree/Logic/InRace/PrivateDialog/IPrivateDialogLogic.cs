namespace Logic
{
    public interface IPrivateDialogLogic
    {
        void LoadDialogPages();
        string GetDialogPage(in int? page, in Pair pair);
    }
}