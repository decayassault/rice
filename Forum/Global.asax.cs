using Forum.Models;
using System.Diagnostics;
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;

namespace Forum
{
    public class MvcApplication : System.Web.HttpApplication
    {
        internal const bool False = false;
        internal const bool True = true;
        internal const byte One = 1;
        internal const byte Zero = 0;       
        
        protected void Application_Start()
        {            
            SetHighPriority();
            InitializeData();                       
            ViewEngines.Engines.Clear();            
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }

        private static void InitializeData()
        {
            Connection.cString = Connection
                //.SecureStr(@"Data Source=dahlia.arvixe.com;Initial Catalog=ForumBase;Async=True;Integrated Security = false;Persist Security Info=false;User ID=ForumAdmin;Password=Pass1Word!;Pooling=true;Connect Timeout=300;Max Pool Size=120");
                .SecureStr(@"Data Source=STATION;Initial Catalog=ForumBase;Async=True;Integrated Security = true;Persist Security Info=false;Pooling=true;Connect Timeout=300;Max Pool Size=120");
            Configuration.Initialize();
        }

        private static void SetHighPriority()
        {
            Process.GetCurrentProcess().PriorityBoostEnabled = True;
            Process.GetCurrentProcess().PriorityClass
                = ProcessPriorityClass.High;
        }

        protected void Application_Error()
        {
            /*System.Web.HttpContext.Current
                .ApplicationInstance.CompleteRequest();*/
            Response.Close();
        }

        protected void Session_End()
        {            
            Request.Cookies.Clear();
            System.GC.Collect(int.MaxValue, System.GCCollectionMode.Optimized, False);
        }
    }
}