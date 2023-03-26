using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
using Forum.Models;

namespace Forum.Data.Account
{
    internal sealed class AccountData
    {
        internal struct Pair
        {
            public int LoginHash;
            public int PasswordHash;
        }
        internal static 
            ConcurrentDictionary<Pair,int> LoginPasswordAccIdHashes;
        internal static 
            ConcurrentDictionary<int, byte> NicksHashes;
        internal static 
            ConcurrentDictionary<Pair, byte> LoginPasswordHashes;
        

        private const string GetAccountsParam = @"GetAccounts";

        private const string GetNicksParam = @"GetNicks";

        private const string GetAccountIdParam = @"GetAccountId";

        private const string IdentifierField = @"Identifier";

        private const string NickField = @"Nick";

        private const string PassphraseField = @"Passphrase";


        internal async static Task<int> GetAccountsCount()
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            int result = MvcApplication.One;
            using (var SqlCon = await Connection.GetConnection())
            {
                using (var cmdSection =
                        Command.InitializeCommandForGetAccountsCount
                            (@"GetAccountsCount", SqlCon))
                {
                    object oo;
                    oo = await cmdSection.ExecuteScalarAsync();
                    if (oo == DBNull.Value || oo == null)
                        result = MvcApplication.One;
                    else result = Convert.ToInt32(oo);
                }
            }
            sw.Stop();
            TimeSpan t = sw.Elapsed;
            return result;
        }

        internal async static Task LoadAccounts()
        {            
            using (var SqlCon = await Connection.GetConnection())
            {
                using (var cmdAccounts = 
                    Command.InitializeCommand(GetAccountsParam, SqlCon))
                {
                    using (var reader = await Reader.InitializeReader(cmdAccounts))
                    {
                        await ProcessAccountsReader(reader);
                    }
                }
            }
                       
        }

        internal async static Task LoadNicks()
        {
            using (var SqlCon = await Connection.GetConnection())
            {
                using (var cmdNicks = 
                    Command.InitializeCommand(GetNicksParam, SqlCon))
                {
                    using (var reader = await Reader.InitializeReader(cmdNicks))
                    {
                        await ProcessNicksReader(reader);
                    }
                }
            }
           
        }

        

              

        internal async static void CheckAccountId()
        {
            while(MvcApplication.True)
            {
                /*foreach(Pair pair in LoginPasswordHashes.Keys)
                {
                    if(!LoginPasswordAccIdHashes.ContainsKey(pair))
                    {
                        int accountId =
                            await GetAccountIdFromBase
                                (pair.LoginHash, pair.PasswordHash);
                        LoginPasswordAccIdHashes.TryAdd(pair, accountId);
                    }
                }*/
               

                System.Threading.Thread.Sleep(500);
            }
        }       

        private async static Task<int> GetAccountIdFromBase
                (int loginHash,int passwordHash)
        {
            int result;
            using (var SqlCon = await Connection.GetConnection())
            {
                using(var cmd =Command.InitializeCommandForAuthentication
                    (GetAccountIdParam,SqlCon,loginHash,passwordHash))
                    {
                        object o = await cmd.ExecuteScalarAsync();
                        if (o == DBNull.Value)
                            result = MvcApplication.One;
                        else result = (int)o;
                    }
            }

            return result;
        }

       

        private async static Task ProcessAccountsReader(SqlDataReader reader)
        {
            LoginPasswordAccIdHashes = 
                new ConcurrentDictionary<Pair,int>();
            LoginPasswordHashes = 
                new ConcurrentDictionary<Pair, byte>();
            if (reader.HasRows)
            {
                object o;                
                Pair pair = new Pair();

                while (await reader.ReadAsync())
                {
                    o = reader[IdentifierField];
                    if (o == DBNull.Value)
                        pair.LoginHash = MvcApplication.Zero;
                    else pair.LoginHash= Convert.ToInt32(o);

                    o = reader[PassphraseField];
                    if (o == DBNull.Value)
                        pair.PasswordHash = MvcApplication.Zero;
                    else pair.PasswordHash = Convert.ToInt32(o);

                    if (!LoginPasswordAccIdHashes.ContainsKey(pair))
                    {
                        int accountId = await GetAccountIdFromBase
                                (pair.LoginHash, pair.PasswordHash);
                        LoginPasswordAccIdHashes.TryAdd(pair, accountId);
                    }
                    if (!LoginPasswordHashes.ContainsKey(pair))
                    {
                        LoginPasswordHashes.TryAdd(pair, MvcApplication.Zero);
                    }
                }
            }
        }

        internal static bool CheckPair(string login, string password)
        {
            int loginHash = login.GetHashCode();
            int passwordHash = password.GetHashCode();
            var pair = new Pair 
                {LoginHash=loginHash,PasswordHash=passwordHash };
            if (LoginPasswordHashes.ContainsKey(pair))
                return MvcApplication.True;
            else return MvcApplication.False;
        }

        private async static Task ProcessNicksReader(SqlDataReader reader)
        {
            NicksHashes = new ConcurrentDictionary<int, byte>();
            if (reader.HasRows)
            {
                object o;
                int nickHash;

                while (await reader.ReadAsync())
                {
                    o = reader[NickField];
                    if (o == DBNull.Value)
                        nickHash = MvcApplication.Zero;
                    else nickHash = o.ToString().GetHashCode();

                    NicksHashes.TryAdd(nickHash, MvcApplication.Zero);
                }
            }
        }
    }
}