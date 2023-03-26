using System;
using System.Threading.Tasks;
using Forum.Data.Account;
namespace Forum.Data.PrivateDialog
{
    internal sealed class PrivateDialogLogic
    {
        private static string[][] DialogPages;
        private static object DialogPagesLock = new object();
        private static int DialogPagesLength;
        private static object DialogPagesLengthLock = new object();
        private static int[] DialogPagesPageDepth;
        private static object DialogPagesPageDepthLock = new object();
        private const string SE = "";    
        
        internal async static Task<string> GetDialogPage
                            (int? page, string username)
        {
            if (page == null) 
                return SE;
            else
            {
                int index = await GetAccountId(username) - MvcApplication.One;
                if (page > MvcApplication.Zero
                        && page
                        <= GetDialogPagesPageDepthLocked(index))
                {
                    return PrivateDialogLogic
                        .GetDialogPagesPageLocked(index
                        , (int)page - MvcApplication.One);
                }
                else
                    return SE;
            }
        }
        private async static Task<int> GetAccountId(string username)
        {
            return
                await ReplyData.GetAccountId(username);
        }
        internal async static Task LoadDialogPages() 
        {
            SetDialogPagesLengthLocked(await AccountData.GetAccountsCount());
            InitializeDialogPagesPageDepthLocked(
                new int[GetDialogPagesLengthLocked()]);
            InitializeDialogPagesLocked
                (new string[GetDialogPagesLengthLocked()][]);
            int len=GetDialogPagesLengthLocked();
            for (int i = MvcApplication.Zero;
                i < len; i++)
            {
               await PrivateDialogData.AddDialog(i);
            }           
        }
        internal static string[] GetDialogPagesArrayLocked(int index)
        {
            lock (DialogPagesLock)
                return DialogPages[index];
        }
        internal static void SetDialogPagesArrayLocked(int index, string[] value)
        {
            lock (DialogPagesLock)
                DialogPages[index] = value;
        }
        internal static string GetDialogPagesPageLocked
                        (int arrayIndex, int pageIndex)
        {
            lock (DialogPagesLock)
                return DialogPages[arrayIndex][pageIndex];
        }
        internal static void SetDialogPagesPageLocked
            (int arrayIndex, int pageIndex, string value)
        {
            lock (DialogPagesLock)
                DialogPages[arrayIndex][pageIndex] = value;
        }
        internal static void AddToDialogPagesPageLocked
            (int arrayIndex, int pageIndex, string value)
        {
            lock (DialogPagesLock)
                DialogPages[arrayIndex][pageIndex] += value;
        }
        internal static void InitializeDialogPagesLocked(string[][] value)
        {
            lock (DialogPagesLock)
                DialogPages = value;
        }
        internal static int GetDialogPagesLengthLocked()
        {
            lock (DialogPagesLengthLock)
                return DialogPagesLength;
        }
        internal static void SetDialogPagesLengthLocked(int value)
        {
            lock (DialogPagesLengthLock)
                DialogPagesLength = value;
        }
        internal static void SetDialogPagesPageDepthLocked(int index, int value)
        {
            lock (DialogPagesPageDepthLock)
                DialogPagesPageDepth[index] = value;
        }
        internal static int GetDialogPagesPageDepthLocked(int index)
        {
            lock (DialogPagesPageDepthLock)
                return DialogPagesPageDepth[index];
        }
        internal static void
            InitializeDialogPagesPageDepthLocked(int[] value)
        {
            lock (DialogPagesPageDepthLock)
                DialogPagesPageDepth = value;
        }
    }
}