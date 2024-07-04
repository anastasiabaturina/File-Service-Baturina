using FileService.Models;
using Microsoft.EntityFrameworkCore;

namespace FileService;

public class DocumentContext : DbContext
{
    public DocumentContext(DbContextOptions<DocumentContext> options) : base(options)
    {}
    public DbSet<Document> Files { get; set; }
}
