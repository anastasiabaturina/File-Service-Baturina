using FileService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileService;

public class DocumentContext : DbContext
{
    public DocumentContext(DbContextOptions<DocumentContext> options) : base(options)
    {
    }

    public DbSet<Document> Files { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UniqueName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Path)
                .HasMaxLength(255);

            entity.Property(e => e.UploadDateTime)
                .IsRequired();

            entity.Property(e => e.Password)
                .HasMaxLength(100);
        });
    }
}