using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DuAnThucTap_vs.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=DuAnThucTapDEV;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brand__DAD4F05EFF9C2F9B");

            entity.ToTable("Brand");

            entity.Property(e => e.BrandName).HasMaxLength(200);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LogoUrl).HasMaxLength(300);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B35317911");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryName).HasMaxLength(200);
            entity.Property(e => e.ImageUrl).HasMaxLength(300);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Slug)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__Contact__5C66259B5B2A5690");

            entity.ToTable("Contact");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(50);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDF35BCC0BFF");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ShortDescription).HasMaxLength(500);
            entity.Property(e => e.Thumbnail).HasMaxLength(300);
            entity.Property(e => e.Title).HasMaxLength(300);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6CD564E3B33");

            entity.ToTable("Product");

            entity.Property(e => e.ImageUrl).HasMaxLength(300);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ProductName).HasMaxLength(300);
            entity.Property(e => e.ShortDescription).HasMaxLength(500);

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Product__BrandId__52593CB8");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Product__Categor__5165187F");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Project__761ABEF0988BD090");

            entity.ToTable("Project");

            entity.Property(e => e.Customer).HasMaxLength(200);
            entity.Property(e => e.ImageUrl).HasMaxLength(300);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.ProjectName).HasMaxLength(300);
            entity.Property(e => e.ShortDescription).HasMaxLength(500);
            entity.Property(e => e.Time).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
