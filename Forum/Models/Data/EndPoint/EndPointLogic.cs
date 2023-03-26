using System.Threading.Tasks;

namespace Forum.Data.EndPoint
{
    public class EndPointLogic
    {
        internal static string[] EndPointPages;

        private const byte EndPointPagesCount = 5;
        private const string SE = "";

        internal static string GetEndPointPage(int Id)
        {
            if (Id > MvcApplication.Zero && Id <= EndPointPagesCount)
                return EndPointPages[Id - MvcApplication.One];
            else
                return SE;
        }

        internal async static Task LoadEndPointPages()
        {
            EndPointPages = new string[EndPointPagesCount];
            for (int i = MvcApplication.Zero; 
                    i < EndPointPagesCount; i++)
            {
                await EndPointData.AddEndPoint(i);
            }
        }

    }
}
