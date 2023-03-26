namespace Logic
{
    public interface IStorage // все lock расположить внутри
    {
        IMemory Fast { get; }
        IDatabase Slow { get; }
    }
}