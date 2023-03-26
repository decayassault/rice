namespace Logic
{
    public interface INewPrivateMessageLogic
    {
        void Start(in int? id, in Pair pair, in string t);
        void PublishNextPrivateMessageByTimer();
    }
}