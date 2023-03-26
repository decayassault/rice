using System.Threading.Tasks;

namespace Forum.Data.EndPoint
{
    public class EndPointLogic
    {
        private static string[] EndPointPages;        
        private const byte EndPointPagesCount = 5;
        private const string SE = "";
        private static object locker = new object();

        internal static string GetEndPointPage(int? Id)
        {
            if (Id == null)
                return SE;
            else
            {
                if (Id > MvcApplication.Zero && Id <= EndPointPagesCount)
                    return GetEndPointPageLocked((int)Id - MvcApplication.One);
                else
                    return SE;
            }
        }

        internal async static Task LoadEndPointPages()
        {
           EndPointLogic
               .InitializeEndPointPagesLocked(EndPointPagesCount);
            
            for (int i = MvcApplication.Zero; 
                    i < EndPointPagesCount; i++)
            {
                await EndPointData.AddEndPoint(i);
            }
        }
        internal static string GetEndPointPageLocked(int index)
        {
            lock(locker)            
                return EndPointPages[index];            
        }
        internal static void SetEndPointPageLocked
                (int index,string value)
        {
            lock (locker)
                EndPointPages[index] = value;
        }
        internal static void InitializeEndPointPagesLocked(int size)
        {
            lock (locker)
                EndPointPages = new string[size];
        }
    }
}
