using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Infrastructure.Entities;

namespace Infrastructure.Context
{
    public partial class wbookdbContext : DbContext
    {
        public wbookdbContext()
        {
        }

        public wbookdbContext(DbContextOptions<wbookdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BasketItem> BasketItems { get; set; } = null!;
        public virtual DbSet<Bundle> Bundles { get; set; } = null!;
        public virtual DbSet<BundleGameMapping> BundleGameMappings { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<GameVariant> GameVariants { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<WishlistItem> WishlistItems { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:proiectfinalsergiu.database.windows.net,1433;Initial Catalog=wbook-db;Persist Security Info=False;User ID=adminSergiu;Password=Copernic@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BasketItem>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BasketItem");

                entity.Property(e => e.IdBundle).HasColumnName("idBundle");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.IdVariant).HasColumnName("idVariant");

                entity.HasOne(d => d.IdBundleNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdBundle)
                    .HasConstraintName("FK_BasketItem_Bundle");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BasketItem_User");

                entity.HasOne(d => d.IdVariantNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdVariant)
                    .HasConstraintName("FK_BasketItem_GameVariant");
            });

            modelBuilder.Entity<Bundle>(entity =>
            {
                entity.ToTable("Bundle");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("category");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(sysdatetime())");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Rrp).HasColumnName("rrp");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<BundleGameMapping>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BundleGameMapping");

                entity.Property(e => e.IdBundle).HasColumnName("idBundle");

                entity.Property(e => e.IdGame).HasColumnName("idGame");

                entity.Property(e => e.IdVariant).HasColumnName("idVariant");

                entity.HasOne(d => d.IdGameNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdGame)
                    .HasConstraintName("FK_BundleGameMapping_Game");

                entity.HasOne(d => d.IdVariantNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdVariant)
                    .HasConstraintName("FK_BundleGameMapping_GameVariant");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Game");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("category");

                entity.Property(e => e.CheapestVariantId).HasColumnName("cheapestVariantId");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(sysdatetime())");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.HoverImage).HasColumnName("hoverImage");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Rrp).HasColumnName("rrp");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.Url).HasColumnName("url");

                entity.HasOne(d => d.CheapestVariant)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.CheapestVariantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Game_GameVariant");
            });

            modelBuilder.Entity<GameVariant>(entity =>
            {
                entity.ToTable("GameVariant");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(sysdatetime())");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.GameId).HasColumnName("gameId");

                entity.Property(e => e.HoverImage).HasColumnName("hoverImage");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Rrp).HasColumnName("rrp");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameVariants)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cheapestVariantId");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AddressLine1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("address_line1");

                entity.Property(e => e.AddressLine2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("address_line2");

                entity.Property(e => e.Bio)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("bio");

                entity.Property(e => e.City)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.County)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("county");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("date_of_birth");

                entity.Property(e => e.Email)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");

                entity.Property(e => e.LastName)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("last_name");

                entity.Property(e => e.Password)
                    .HasMaxLength(1024)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("postal_code");

                entity.Property(e => e.Salt).HasColumnName("salt");

                entity.Property(e => e.Type)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<WishlistItem>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("WishlistItem");

                entity.Property(e => e.IdBundle).HasColumnName("idBundle");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.IdVariant).HasColumnName("idVariant");

                entity.HasOne(d => d.IdBundleNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdBundle)
                    .HasConstraintName("FK_WishlistItem_Bundle");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WishlistItem_User");

                entity.HasOne(d => d.IdVariantNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdVariant)
                    .HasConstraintName("FK_WishlistItem_GameVariant");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
