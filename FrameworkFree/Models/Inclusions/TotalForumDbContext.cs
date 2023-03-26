using Microsoft.EntityFrameworkCore;
using Models;
namespace Data
{
    public partial class TotalForumDbContext : DbContext
    {
        public TotalForumDbContext()
        {
        }

        public TotalForumDbContext(DbContextOptions<TotalForumDbContext> options)
            : base(options)
        {
        }


        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Endpoint> Endpoint { get; set; }
        public virtual DbSet<Forum> Forum { get; set; }
        public virtual DbSet<Msg> Msg { get; set; }
        public virtual DbSet<PrivateMessage> PrivateMessage { get; set; }
        public virtual DbSet<Thread> Thread { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost; Database=TotalForum;User ID=forumadminuser; Password=Dctv1ghbdtn");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(e => e.Nick)
                    .HasDatabaseName("GetNicks");

                entity.HasIndex(e => new { e.Id, e.Nick })
                    .HasDatabaseName("GetNick");

                entity.HasIndex(e => new { e.Identifier, e.Passphrase })
                    .HasDatabaseName("UQ__Account__04545158F4AED675")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.Identifier, e.Passphrase })
                    .HasDatabaseName("GetAccounts");

                entity.HasIndex(e => new { e.Nick, e.Identifier, e.Passphrase })
                    .HasDatabaseName("Register");
            });

            modelBuilder.Entity<Endpoint>(entity =>
            {
                entity.HasIndex(e => new { e.Id, e.Name, e.ForumId })
                    .HasDatabaseName("GetEndpointsTop5");

                entity.Property(e => e.Name).HasDefaultValueSql("(N'КонечнаяТочка')");
            });

            modelBuilder.Entity<Forum>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasDatabaseName("UQ__Forum__737584F6DBD66E38")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.Name })
                    .HasDatabaseName("GetForums");

                entity.Property(e => e.Name).HasDefaultValueSql("(N'Форум')");
            });

            modelBuilder.Entity<Msg>(entity =>
            {
                entity.HasIndex(e => new { e.Id, e.ThreadId })
                    .HasDatabaseName("GetMessagesCount");

                entity.HasIndex(e => new { e.ThreadId, e.AccountId })
                    .HasDatabaseName("PutMessage");

                entity.HasIndex(e => new { e.Id, e.ThreadId, e.AccountId })
                    .HasDatabaseName("GetMessages");

                entity.Property(e => e.AccountId).HasDefaultValueSql("((1))");

                entity.Property(e => e.MsgText).HasDefaultValueSql("(N'Сообщение')");

                entity.Property(e => e.ThreadId).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<PrivateMessage>(entity =>
            {
                entity.HasIndex(e => new { e.SenderAccountId, e.AcceptorAccountId })
                    .HasDatabaseName("AddPrivateMessage");

                entity.HasIndex(e => new { e.Id, e.SenderAccountId, e.AcceptorAccountId })
                    .HasDatabaseName("GetPrivateMessagesCount");

                entity.Property(e => e.AcceptorAccountId).HasDefaultValueSql("((1))");

                entity.Property(e => e.PrivateText).HasDefaultValueSql("(N'Сообщение')");

                entity.Property(e => e.SenderAccountId).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Thread>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasDatabaseName("GetAllThreadsCount");

                entity.HasIndex(e => e.Name)
                    .HasDatabaseName("UQ__Thread__737584F626CEEBC2")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.EndpointId })
                    .HasDatabaseName("GetThreadsCount");

                entity.HasIndex(e => new { e.Id, e.Name })
                    .HasDatabaseName("GetThreadName");

                entity.HasIndex(e => new { e.Name, e.EndpointId })
                    .HasDatabaseName("StartTopic");

                entity.HasIndex(e => new { e.Id, e.Name, e.EndpointId })
                    .HasDatabaseName("GetThreads");

                entity.Property(e => e.EndpointId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasDefaultValueSql("(N'Поток')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
