using System.Linq;
using System.Collections.Generic;
using MarkupHandlers;
namespace Data
{
    internal sealed class EndPointLogic : IEndPointLogic
    {
        public readonly IStorage Storage;
        public readonly EndPointMarkupHandler EndPointMarkupHandler;
        public EndPointLogic(IStorage storage, EndPointMarkupHandler endPointMarkupHandler)
        {
            Storage = storage;
            EndPointMarkupHandler = endPointMarkupHandler;
        }
        private static readonly object locker = new object();

        public string GetEndPointPage(int? Id)
        {
            if (Id == null)
                return Constants.SE;
            else
            {
                if (Id > 0 && Id <= Constants.EndPointPagesCount)
                    return Storage.Fast.GetEndPointPageLocked((int)Id - 1);
                else
                    return Constants.SE;
            }
        }

        public void LoadEndPointPages()
        {
            Storage.Fast.InitializeEndPointPagesLocked(Constants.EndPointPagesCount);

            for (int i = 0;
                    i < Constants.EndPointPagesCount; i++)
            {
                ProcessEndPointReader(Storage.Slow.GetEndpointIdNames(i + 1), i);
            }
        }

        public void ProcessEndPointReader
            (IEnumerable<IdName> idNames, int number)
        {
            if (idNames.Any())
                Storage.Fast.SetEndPointPageLocked(number,
                    EndPointMarkupHandler.GenerateEndPointLinks(idNames));
        }

        public string GetEndPointPageLocked(int index)
        {
            lock (locker)
                return Storage.Fast.GetEndPointPageLocked(index);
        }
    }
}
