using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NetCore.Databases.Data
{
    public partial class AppDbContext : DbContext
    {
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<UserRolesByUser> UserRolesByUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=localhost,1433;Database=DBFirstDB;User Id=sa;Password=Test1234!;Encrypt=False;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserEmail)
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.JoinedUtcDate).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(130)
                    .IsUnicode(false);

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasMaxLength(320)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.ModifiedUtcDate).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<UserRolesByUser>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => new { e.RoleId, e.UserId })
                    .HasName("AK_UserRolesByUser_RoleId_UserId")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OwnedUtcDate).HasDefaultValueSql("(sysutcdatetime())");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRolesByUser)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRolesByUser)
                    .HasForeignKey(d => d.UserId);
            });
        }
    }
}
