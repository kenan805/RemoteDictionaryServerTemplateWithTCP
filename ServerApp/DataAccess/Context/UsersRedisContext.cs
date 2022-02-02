using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ServerApp.DataAccess
{
    public partial class UsersRedisContext : DbContext
    {
        public UsersRedisContext()
        {
        }

        public UsersRedisContext(DbContextOptions<UsersRedisContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=WIN-ML3N7I995PP;Database=UsersRedis;User ID=sa; Password=asus1212;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__Users__536C85E5410D3445");

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.Property(e => e.Likes).HasDefaultValueSql("((0))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
