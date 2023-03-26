namespace Logic
{
    public interface INewPrivateDialogLogic
    {
        void StartNextDialogByTimer();
        void Start(in string acceptorNick, in Pair pair, in string message);
    }
}