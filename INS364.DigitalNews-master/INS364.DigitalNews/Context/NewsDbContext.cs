using INS364.DigitalNews.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace INS364.DigitalNews.Context
{
    public partial class NewsDbContext : DbContext
    {
        private IConfiguration _configuration { get; }

        public NewsDbContext(DbContextOptions<NewsDbContext> options,
            IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<CommentModel> Comments { get; set; }
        public virtual DbSet<NewsContentModel> News { get; set; }
        public virtual DbSet<NewsFileModel> NewsFiles { get; set; }
        public virtual DbSet<NewsImpactModel> NewsImpacts { get; set; }
        public virtual DbSet<NewsTagModel> NewsTags { get; set; }
        public virtual DbSet<UserModel> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = _configuration.GetConnectionString("DigitalNewsConneciton");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CommentModel>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("PK__tblComme__C3B4DFCA78E6847C");

                entity.ToTable("tblComments");

                entity.HasIndex(e => e.CommentUserId, "IX_tblComments_CommentUserId");

                entity.Property(e => e.CommentBody)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.CommentPublishDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(CONVERT([date],getdate()))");

                entity.HasOne(d => d.CommentUser)
                    .WithMany(p => p.TblComments)
                    .HasForeignKey(d => d.CommentUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblCommen__Comme__6B24EA82");

                entity.HasOne(d => d.News)
                    .WithMany(p => p.TblComments)
                    .HasForeignKey(d => d.NewsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblCommen__NewsI__4BAC3F29");
            });

            modelBuilder.Entity<NewsContentModel>(entity =>
            {
                entity.HasKey(e => e.NewsId)
                    .HasName("PK__tblNews__954EBDF399BAB2F9");

                entity.ToTable("tblNews");

                entity.HasIndex(e => e.NewsAuthorId, "IX_tblNews_NewsAuthorId");

                entity.HasIndex(e => e.NewsFileId, "IX_tblNews_NewsFileId");

                entity.HasIndex(e => e.NewsImpactId, "IX_tblNews_NewsImpactId");

                entity.HasIndex(e => e.NewsTagId, "IX_tblNews_NewsTagId");

                entity.Property(e => e.NewsContent)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.NewsDesc)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.NewsPublishDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(CONVERT([date],getdate()))");

                entity.Property(e => e.NewsTitle)
                    .IsRequired()
                    .HasMaxLength(96);

                entity.HasOne(d => d.NewsAuthor)
                    .WithMany(p => p.TblNews)
                    .HasForeignKey(d => d.NewsAuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblNews__NewsAut__47DBAE45");

                entity.HasOne(d => d.NewsFile)
                    .WithMany(p => p.TblNews)
                    .HasForeignKey(d => d.NewsFileId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__tblNews__NewsFil__6E01572D");

                entity.HasOne(d => d.NewsImpact)
                    .WithMany(p => p.TblNews)
                    .HasForeignKey(d => d.NewsImpactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblNews__NewsImp__6477ECF3");

                entity.HasOne(d => d.NewsTag)
                    .WithMany(p => p.TblNews)
                    .HasForeignKey(d => d.NewsTagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblNews__NewsImp__6383C8BA");
            });

            modelBuilder.Entity<NewsFileModel>(entity =>
            {
                entity.HasKey(e => e.NewsFileId)
                    .HasName("PK__tblNewsF__3ABEB92EF951C08B");

                entity.ToTable("tblNewsFile");

                entity.Property(e => e.NewsFilePath).HasMaxLength(128);
            });

            modelBuilder.Entity<NewsImpactModel>(entity =>
            {
                entity.HasKey(e => e.NewsImpactId)
                    .HasName("PK__tblNewsI__A784AD6E972394D1");

                entity.ToTable("tblNewsImpact");

                entity.Property(e => e.NewsImpactDesc)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<NewsTagModel>(entity =>
            {
                entity.HasKey(e => e.NewsTagId)
                    .HasName("PK__tblNewsT__6158562B86FF11F7");

                entity.ToTable("tblNewsTag");

                entity.Property(e => e.NewsTagDesc)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__tblUsers__1788CC4C49CA2844");

                entity.ToTable("tblUsers");

                entity.HasIndex(e => e.Username, "UQ__tblUsers__536C85E4FB40A4EF")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.RegisterDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
