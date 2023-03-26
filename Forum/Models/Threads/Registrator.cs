using Forum.Data;
using System.Threading;
using System.Collections.Concurrent;

namespace Forum.Models.Threads
{
    internal sealed class Registrator
    {
        private static Thread DbRegistration;
        private static object DbRegistrationLock = new object();
        private static Thread RamRegistration;
        private static object RamRegistrationLock = new object();

        internal static void Start()
        {
            InitializeThreads();
            SetPriority();
            StartThreads();
        }        

        private static void SetPriority()
        {
            lock(RamRegistrationLock)
                RamRegistration.Priority = ThreadPriority.Lowest;
            lock(DbRegistrationLock)
                DbRegistration.Priority = ThreadPriority.Lowest;             
        }

        private static void StartThreads()
        {
            lock(RamRegistrationLock)
                RamRegistration.Start();
            lock(DbRegistrationLock)
                DbRegistration.Start();            
        }

        private static void InitializeThreads()
        {
            lock(DbRegistrationLock)
                DbRegistration = new Thread(RegistrationData.RegisterInBase);
            lock(RamRegistrationLock)
                RamRegistration = new Thread(RegistrationData.PutRegInfo);            
        }
        
    }
}