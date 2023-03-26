namespace Forum.Models
{
    using System.Threading.Tasks;
    using System.Data;
    using System.Data.SqlClient;
    internal sealed class Command
    {
        internal const string LinkMiddle = @"'>";
        internal const string Id = @"Id";

        private const string ForumIdParameter = @"@ForumId";
        private const string ThreadIdParameter = @"@ThreadId";
        private const string AccountIdParameter = @"@AccountId";
        private const string LoginHashParameter = @"@LoginHash";
        private const string PasswordHashParameter = @"@PasswordHash";
        private const string MessageParameter = @"@Message";
        private const string EmailParameter = @"@Email";
        private const string NickParameter = @"@Nick";
        private const string EndpointIdParameter = @"@EndpointId";
        private const string ThreadNameParameter = @"@ThreadName";
        private const int CmdTimeout = 300;
        internal static SqlCommand InitializeCommand
            (string Text, SqlConnection SqlCon)
        {
            SqlCommand cmd = new SqlCommand(Text, SqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = CmdTimeout;
            cmd.Prepare();

            return cmd;
        }

        internal static SqlCommand InitializeCommandForInputForumId
            (string Text, SqlConnection SqlCon, int ForumId)
        {
            SqlCommand cmd = new SqlCommand(Text, SqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter par = cmd.Parameters.Add(ForumIdParameter, SqlDbType.Int);
            par.Value = ForumId;
            cmd.CommandTimeout = CmdTimeout;
            cmd.Prepare();

            return cmd;
        }

        internal static SqlCommand InitializeCommandForPutMessage
            (string Text, SqlConnection SqlCon, 
                int ThreadId, int AccountId, string Message)
        {
            SqlCommand cmd = new SqlCommand(Text, SqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter par = cmd.Parameters.Add(ThreadIdParameter, SqlDbType.Int);
            par.Value = ThreadId;
            SqlParameter par1 = cmd.Parameters.Add(AccountIdParameter, SqlDbType.Int);
            par1.Value = AccountId;
            SqlParameter par2 = cmd.Parameters.Add(MessageParameter, SqlDbType.NVarChar);
            par2.Value = Message;
            cmd.CommandTimeout = CmdTimeout;
            cmd.Prepare();

            return cmd;
        }
        internal static SqlCommand InitializeCommandForStartTopic
            (string Text, SqlConnection SqlCon,
                string threadName, int endpointId,int accountId,
                         string message)
        {
            SqlCommand cmd = new SqlCommand(Text, SqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter par = cmd.Parameters.Add(ThreadNameParameter, SqlDbType.NVarChar);
            par.Value = threadName;
            SqlParameter par1 = cmd.Parameters.Add(EndpointIdParameter, SqlDbType.Int);
            par1.Value = endpointId;
            SqlParameter par2 = cmd.Parameters.Add(AccountIdParameter, SqlDbType.Int);
            par2.Value = accountId;
            SqlParameter par3 = cmd.Parameters.Add(MessageParameter, SqlDbType.NVarChar);
            par3.Value = message;
            cmd.CommandTimeout = CmdTimeout;
            cmd.Prepare();

            return cmd;
        }

        internal static SqlCommand InitializeCommandForInputEndpointId
            (string Text, SqlConnection SqlCon, int EndpointId)
        {
            SqlCommand cmd = new SqlCommand(Text, SqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter par = cmd.Parameters.Add(EndpointIdParameter, SqlDbType.Int);
            par.Value = EndpointId;
            cmd.CommandTimeout = CmdTimeout;
            cmd.Prepare();

            return cmd;
        }

        internal static SqlCommand InitializeCommandForAuthentication(string Text,
            SqlConnection SqlCon, int loginHash, int passwordHash)
        {
            SqlCommand cmd = new SqlCommand(Text, SqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter par = cmd.Parameters.Add(LoginHashParameter, SqlDbType.Int);
            par.Value = loginHash;
            SqlParameter par1 = cmd.Parameters.Add(PasswordHashParameter, SqlDbType.Int);
            par1.Value = passwordHash;
            cmd.UpdatedRowSource = UpdateRowSource.OutputParameters;
            cmd.CommandTimeout = CmdTimeout;
            cmd.Prepare();

            return cmd;
        }

        internal static SqlCommand InitializeCommandForInputThreadId(string Text,
                SqlConnection SqlCon, int ThreadId)
        {

            SqlCommand cmd = new SqlCommand(Text, SqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter par = cmd.Parameters.Add(ThreadIdParameter, SqlDbType.Int);
            par.Value = ThreadId;
            cmd.CommandTimeout = CmdTimeout;
            cmd.Prepare();

            return cmd;
        }

        internal static SqlCommand InitializeCommandForInputRegister(string Text,
            SqlConnection SqlCon, int loginHash, int passwordHash, string email, string nick)
        {
            SqlCommand cmd = new SqlCommand(Text, SqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter par = cmd.Parameters.Add(LoginHashParameter, SqlDbType.Int);
            par.Value = loginHash;
            SqlParameter par1 = cmd.Parameters.Add(PasswordHashParameter, SqlDbType.Int);
            par1.Value = passwordHash;
            SqlParameter par2 = cmd.Parameters.Add(EmailParameter, SqlDbType.NVarChar);
            par2.Value = email;
            SqlParameter par3 = cmd.Parameters.Add(NickParameter, SqlDbType.NVarChar);
            par3.Value = nick;
            cmd.CommandTimeout = CmdTimeout;
            cmd.Prepare();

            return cmd;
        }


        internal static SqlCommand InitializeCommandForInputAccountId
            (string Function, SqlConnection SqlCon, int AccountId)
        {
            SqlCommand cmd = new SqlCommand(Function, SqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter par = cmd.Parameters.Add(AccountIdParameter, SqlDbType.Int);
            par.Value = AccountId;
            cmd.CommandTimeout = CmdTimeout;
            cmd.Prepare();

            return cmd;
        }
    }
}