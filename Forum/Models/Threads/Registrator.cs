using Forum.Data;
using System.Threading;
using System.Collections.Concurrent;

namespace Forum.Models.Threads
{
    internal sealed class Registrator
    {
        private static Thread DbRegistration;
        private static Thread RamRegistration;

        internal static void Start()
        {
            InitializeThreads();
            SetPriority();
            StartThreads();
        }        

        private static void SetPriority()
        {
            RamRegistration.Priority = ThreadPriority.Lowest;
            DbRegistration.Priority = ThreadPriority.Lowest;             
        }

        private static void StartThreads()
        {
            RamRegistration.Start();
            DbRegistration.Start();            
        }

        private static void InitializeThreads()
        {
            DbRegistration = new Thread(RegistrationData.RegisterInBase);
            RamRegistration = new Thread(RegistrationData.PutRegInfo);            
        }
    }
}