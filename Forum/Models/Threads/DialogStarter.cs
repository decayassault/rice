using Forum.Data.NewPrivateDialog;
namespace Forum.Models.Threads
{    
    using System.Threading;
    internal sealed class DialogStarter
    {
        private static Thread NewDialogStarter;
        private static object NewDialogStarterLock = new object();
        internal static void Start()
        {
            InitializeThread();
            SetPriority();
            StartThread();
        }  
        private static void InitializeThread()
        {
            lock (NewDialogStarterLock)
                NewDialogStarter = 
                    new Thread(NewPrivateDialogLogic.StartNextDialog);
        }
        private static void SetPriority()
        {
            lock (NewDialogStarterLock)
                NewDialogStarter.Priority = ThreadPriority.Lowest;
        }
        private static void StartThread()
        {
            lock (NewDialogStarterLock)
                NewDialogStarter.Start();
        }
    }    
}